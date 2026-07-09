using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace rp.spark.Services
{
	public class PlayerStateService
	{
		private class LocationNameResult
		{
			public string Name { get; set; } = string.Empty;


			public bool IsResolved { get; set; }

			public static LocationNameResult Resolved(string name)
			{
				return new LocationNameResult
				{
					Name = (name ?? string.Empty),
					IsResolved = true
				};
			}

			public static LocationNameResult Unresolved(string name)
			{
				return new LocationNameResult
				{
					Name = (name ?? string.Empty),
					IsResolved = false
				};
			}
		}

		private class CharacterApiSnapshot
		{
			public string Race { get; set; } = string.Empty;


			public string Profession { get; set; } = string.Empty;


			public bool IsVerified { get; set; }

			public DateTime FetchedAt { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<PlayerStateService>();

		private const string HiddenLocationName = "Hidden";

		private static readonly TimeSpan AccountRefreshInterval = TimeSpan.FromMinutes(5.0);

		private static readonly TimeSpan CharacterRefreshInterval = TimeSpan.FromMinutes(5.0);

		private static readonly TimeSpan AccountFailureRetryInterval = TimeSpan.FromMinutes(2.0);

		private static readonly TimeSpan CharacterFailureRetryInterval = TimeSpan.FromSeconds(15.0);

		private static readonly TimeSpan MapResolveRetryInterval = TimeSpan.FromSeconds(15.0);

		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly SparkSettings _settings;

		private readonly Dictionary<int, string> _mapNameCache = new Dictionary<int, string>();

		private readonly Dictionary<int, DateTime> _mapRetryAfter = new Dictionary<int, DateTime>();

		private readonly Dictionary<string, CharacterApiSnapshot> _characterCache = new Dictionary<string, CharacterApiSnapshot>(StringComparer.OrdinalIgnoreCase);

		private readonly Dictionary<string, DateTime> _characterRetryAfter = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

		private readonly object _lastStateLock = new object();

		private readonly object _accountCacheLock = new object();

		private PlayerState _lastState = new PlayerState();

		private string _cachedAccountName = string.Empty;

		private DateTime _accountFetchedAt = DateTime.MinValue;

		private DateTime _nextAccountLookup = DateTime.MinValue;

		public PlayerStateService(Gw2ApiManager gw2ApiManager, SparkSettings settings)
		{
			_gw2ApiManager = gw2ApiManager;
			_settings = settings;
		}

		public async Task<PlayerState> GetCurrentAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			PlayerState state = ReadMumble();
			PlayerState playerState = state;
			playerState.AccountName = await GetAccountNameAsync(cancellationToken);
			await TryVerifyCharacterAsync(state, cancellationToken);
			if (IsLocationHidden())
			{
				state.MapId = 0;
				state.LocationName = "Hidden";
				state.IsLocationResolved = true;
			}
			else
			{
				LocationNameResult location = await GetLocationNameAsync(state.MapId, cancellationToken);
				state.LocationName = location.Name;
				state.IsLocationResolved = location.IsResolved;
			}
			SetLastState(state);
			return state;
		}

		public PlayerState GetCached()
		{
			PlayerState state = ReadMumble();
			PlayerState lastState = GetLastState();
			bool sameCharacter = !string.IsNullOrWhiteSpace(state.OfficialCharacterName) && string.Equals(state.OfficialCharacterName, lastState?.OfficialCharacterName, StringComparison.OrdinalIgnoreCase);
			if (lastState != null)
			{
				state.AccountName = lastState.AccountName;
				state.HasCharactersPermission = lastState.HasCharactersPermission;
				state.IsCharacterApiVerified = sameCharacter && lastState.IsCharacterApiVerified;
				if (sameCharacter)
				{
					if (string.IsNullOrWhiteSpace(state.Race))
					{
						state.Race = lastState.Race;
					}
					if (string.IsNullOrWhiteSpace(state.Profession))
					{
						state.Profession = lastState.Profession;
					}
					if (string.IsNullOrWhiteSpace(state.Specialization))
					{
						state.Specialization = lastState.Specialization;
					}
				}
				if (state.MapId > 0 && state.MapId == lastState.MapId)
				{
					state.LocationName = lastState.LocationName;
					state.IsLocationResolved = lastState.IsLocationResolved;
				}
			}
			if (IsLocationHidden())
			{
				state.MapId = 0;
				state.LocationName = "Hidden";
				state.IsLocationResolved = true;
				return state;
			}
			if (string.IsNullOrWhiteSpace(state.LocationName))
			{
				state.LocationName = GetLocationFallback(state.MapId);
				state.IsLocationResolved = state.MapId <= 0;
			}
			return state;
		}

		private PlayerState GetLastState()
		{
			lock (_lastStateLock)
			{
				return CloneState(_lastState);
			}
		}

		private void SetLastState(PlayerState state)
		{
			lock (_lastStateLock)
			{
				_lastState = CloneState(state) ?? new PlayerState();
			}
		}

		private bool TryGetCachedAccountName(out string accountName)
		{
			lock (_accountCacheLock)
			{
				if (IsFresh(_accountFetchedAt, AccountRefreshInterval) || DateTime.UtcNow < _nextAccountLookup)
				{
					accountName = _cachedAccountName;
					return true;
				}
			}
			accountName = string.Empty;
			return false;
		}

		private void CacheAccountName(string accountName)
		{
			lock (_accountCacheLock)
			{
				_cachedAccountName = accountName ?? string.Empty;
				_accountFetchedAt = DateTime.UtcNow;
				_nextAccountLookup = DateTime.MinValue;
			}
		}

		private void SetAccountRetry()
		{
			lock (_accountCacheLock)
			{
				_nextAccountLookup = DateTime.UtcNow + AccountFailureRetryInterval;
			}
		}

		private PlayerState ReadMumble()
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			PlayerState state = new PlayerState
			{
				IsMumbleAvailable = GameService.Gw2Mumble.get_IsAvailable(),
				IsInGame = GameService.GameIntegration.get_Gw2Instance().get_IsInGame()
			};
			if (!state.IsMumbleAvailable || !state.IsInGame)
			{
				return state;
			}
			state.OfficialCharacterName = GameService.Gw2Mumble.get_PlayerCharacter().get_Name()?.Trim() ?? string.Empty;
			state.Race = FormatGameValue(GameService.Gw2Mumble.get_PlayerCharacter().get_Race());
			state.Profession = FormatGameValue(GameService.Gw2Mumble.get_PlayerCharacter().get_Profession());
			state.Specialization = FormatGameValue(GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization());
			if (IsLocationHidden())
			{
				state.LocationName = "Hidden";
				state.IsLocationResolved = true;
			}
			else
			{
				state.MapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				state.IsLocationResolved = state.MapId <= 0;
			}
			return state;
		}

		private async Task<string> GetAccountNameAsync(CancellationToken cancellationToken)
		{
			try
			{
				if (!HasApiPermissions((TokenPermission)1))
				{
					return string.Empty;
				}
				if (TryGetCachedAccountName(out var cachedAccountName))
				{
					return cachedAccountName;
				}
				Account obj = await ((IBlobClient<Account>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(cancellationToken);
				string accountName = ((obj == null) ? null : obj.get_Name()?.Trim()) ?? string.Empty;
				CacheAccountName(accountName);
				return accountName;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				SetAccountRetry();
				BlishWarnings.HttpBlocked(ex, "load your GW2 account name");
				Logger.Warn(ex, "Failed to load account name from the GW2 API.");
				return string.Empty;
			}
		}

		private async Task<LocationNameResult> GetLocationNameAsync(int mapId, CancellationToken cancellationToken)
		{
			if (mapId <= 0)
			{
				return LocationNameResult.Resolved(GetLocationFallback(mapId));
			}
			lock (_mapNameCache)
			{
				if (_mapNameCache.TryGetValue(mapId, out var cachedName))
				{
					return LocationNameResult.Resolved(cachedName);
				}
			}
			try
			{
				if (IsMapRetryCoolingDown(mapId))
				{
					return LocationNameResult.Unresolved(GetLocationFallback(mapId));
				}
				Map obj = await ((IBulkExpandableClient<Map, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
					.get_Maps()).GetAsync(mapId, cancellationToken);
				string mapName = ((obj == null) ? null : obj.get_Name()?.Trim()) ?? string.Empty;
				if (string.IsNullOrWhiteSpace(mapName))
				{
					SetMapRetry(mapId);
					return LocationNameResult.Unresolved(GetLocationFallback(mapId));
				}
				lock (_mapNameCache)
				{
					_mapNameCache[mapId] = mapName;
				}
				ClearMapRetry(mapId);
				return LocationNameResult.Resolved(mapName);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				SetMapRetry(mapId);
				BlishWarnings.HttpBlocked(ex, "resolve your current GW2 map");
				Logger.Warn("Failed to resolve the current GW2 map ({errorType}).", new object[1] { ex.GetType().Name });
				return LocationNameResult.Unresolved(GetLocationFallback(mapId));
			}
		}

		private static string GetLocationFallback(int mapId)
		{
			if (mapId <= 0)
			{
				return "Unknown";
			}
			return $"Map {mapId}";
		}

		private async Task TryVerifyCharacterAsync(PlayerState state, CancellationToken cancellationToken)
		{
			state.HasCharactersPermission = HasApiPermissions((TokenPermission)1, (TokenPermission)3);
			if (!state.CanEditProfile || !state.HasCharactersPermission)
			{
				return;
			}
			try
			{
				if (TryApplyCachedCharacter(state) || IsCharacterRetryCoolingDown(state.OfficialCharacterName))
				{
					return;
				}
				Character character = await ((IBulkExpandableClient<Character, string>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).GetAsync(state.OfficialCharacterName, cancellationToken);
				if (character != null)
				{
					state.IsCharacterApiVerified = true;
					if (!string.IsNullOrWhiteSpace(character.get_Race()))
					{
						state.Race = character.get_Race().Trim();
					}
					if (!string.IsNullOrWhiteSpace(character.get_Profession()))
					{
						state.Profession = character.get_Profession().Trim();
					}
					CacheCharacter(state);
					ClearCharacterRetry(state.OfficialCharacterName);
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				SetCharacterRetry(state.OfficialCharacterName);
				BlishWarnings.HttpBlocked(ex, "verify your GW2 character");
				Logger.Warn("Failed to verify the current GW2 character with the GW2 API ({errorType}).", new object[1] { ex.GetType().Name });
				state.IsCharacterApiVerified = false;
			}
		}

		private bool TryApplyCachedCharacter(PlayerState state)
		{
			if (state == null || string.IsNullOrWhiteSpace(state.OfficialCharacterName))
			{
				return false;
			}
			lock (_characterCache)
			{
				if (!_characterCache.TryGetValue(state.OfficialCharacterName.Trim(), out var cached) || !IsFresh(cached.FetchedAt, CharacterRefreshInterval))
				{
					return false;
				}
				state.IsCharacterApiVerified = cached.IsVerified;
				if (!string.IsNullOrWhiteSpace(cached.Race))
				{
					state.Race = cached.Race;
				}
				if (!string.IsNullOrWhiteSpace(cached.Profession))
				{
					state.Profession = cached.Profession;
				}
				return true;
			}
		}

		private void CacheCharacter(PlayerState state)
		{
			if (state != null && !string.IsNullOrWhiteSpace(state.OfficialCharacterName))
			{
				lock (_characterCache)
				{
					_characterCache[state.OfficialCharacterName.Trim()] = new CharacterApiSnapshot
					{
						Race = (state.Race?.Trim() ?? string.Empty),
						Profession = (state.Profession?.Trim() ?? string.Empty),
						IsVerified = state.IsCharacterApiVerified,
						FetchedAt = DateTime.UtcNow
					};
				}
			}
		}

		private bool IsCharacterRetryCoolingDown(string characterName)
		{
			if (string.IsNullOrWhiteSpace(characterName))
			{
				return false;
			}
			lock (_characterRetryAfter)
			{
				DateTime retryAfter;
				return _characterRetryAfter.TryGetValue(characterName.Trim(), out retryAfter) && DateTime.UtcNow < retryAfter;
			}
		}

		private void SetCharacterRetry(string characterName)
		{
			if (!string.IsNullOrWhiteSpace(characterName))
			{
				lock (_characterRetryAfter)
				{
					_characterRetryAfter[characterName.Trim()] = DateTime.UtcNow + CharacterFailureRetryInterval;
				}
			}
		}

		private void ClearCharacterRetry(string characterName)
		{
			if (!string.IsNullOrWhiteSpace(characterName))
			{
				lock (_characterRetryAfter)
				{
					_characterRetryAfter.Remove(characterName.Trim());
				}
			}
		}

		private bool IsMapRetryCoolingDown(int mapId)
		{
			lock (_mapRetryAfter)
			{
				DateTime retryAfter;
				return _mapRetryAfter.TryGetValue(mapId, out retryAfter) && DateTime.UtcNow < retryAfter;
			}
		}

		private void SetMapRetry(int mapId)
		{
			lock (_mapRetryAfter)
			{
				_mapRetryAfter[mapId] = DateTime.UtcNow + MapResolveRetryInterval;
			}
		}

		private void ClearMapRetry(int mapId)
		{
			lock (_mapRetryAfter)
			{
				_mapRetryAfter.Remove(mapId);
			}
		}

		private bool HasApiPermissions(params TokenPermission[] permissions)
		{
			if (_gw2ApiManager != null && _gw2ApiManager.get_HasSubtoken())
			{
				return _gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)permissions);
			}
			return false;
		}

		private bool IsLocationHidden()
		{
			return (_settings?.HideLocation?.get_Value()).GetValueOrDefault();
		}

		private static string FormatGameValue(object value)
		{
			string text = value?.ToString() ?? string.Empty;
			if (!string.Equals(text, "None", StringComparison.OrdinalIgnoreCase) && !string.Equals(text, "Unknown", StringComparison.OrdinalIgnoreCase))
			{
				return text;
			}
			return string.Empty;
		}

		private static PlayerState CloneState(PlayerState state)
		{
			if (state == null)
			{
				return null;
			}
			return new PlayerState
			{
				IsMumbleAvailable = state.IsMumbleAvailable,
				IsInGame = state.IsInGame,
				OfficialCharacterName = state.OfficialCharacterName,
				Race = state.Race,
				Profession = state.Profession,
				Specialization = state.Specialization,
				MapId = state.MapId,
				LocationName = state.LocationName,
				IsLocationResolved = state.IsLocationResolved,
				AccountName = state.AccountName,
				IsCharacterApiVerified = state.IsCharacterApiVerified,
				HasCharactersPermission = state.HasCharactersPermission
			};
		}

		private static bool IsFresh(DateTime fetchedAt, TimeSpan interval)
		{
			if (fetchedAt != default(DateTime))
			{
				return DateTime.UtcNow - fetchedAt < interval;
			}
			return false;
		}
	}
}
