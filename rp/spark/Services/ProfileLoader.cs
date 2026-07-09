using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using rp.spark.Models;
using rp.spark.Models.Api;

namespace rp.spark.Services
{
	public class ProfileLoader
	{
		private static readonly Logger Logger = Logger.GetLogger<ProfileLoader>();

		private readonly ProfileRepository _profileRepository;

		private readonly PlayerStateService _playerState;

		private readonly PresenceService _presenceService;

		private readonly PresenceLoop _presenceLoop;

		private readonly ServerSync _serverSync;

		private readonly SparkSettings _settings;

		private readonly ProfileActions _profileActions;

		public ProfileLoader(ProfileRepository profileRepository, PlayerStateService playerState, PresenceService presenceService, PresenceLoop presenceLoop, ServerSync serverSync, SparkSettings settings, ProfileActions profileActions)
		{
			_profileRepository = profileRepository;
			_playerState = playerState;
			_presenceService = presenceService;
			_presenceLoop = presenceLoop;
			_serverSync = serverSync;
			_settings = settings;
			_profileActions = profileActions;
		}

		public ProfileViewData LoadMyProfile()
		{
			PlayerState state = _playerState.GetCached();
			CharacterProfile profile = _profileRepository.LoadActiveForCharacter(state.AccountName, state.OfficialCharacterName) ?? CreateEmptyProfile(state);
			return BuildLocal(profile, state);
		}

		public ProfileViewData BuildLocal(CharacterProfile profile, PlayerState state)
		{
			if (profile == null)
			{
				profile = CreateEmptyProfile(state);
			}
			if (state == null)
			{
				state = _playerState.GetCached();
			}
			if (string.IsNullOrWhiteSpace(profile.CharacterName) && !string.IsNullOrWhiteSpace(state.OfficialCharacterName))
			{
				profile.CharacterName = state.OfficialCharacterName;
			}
			if (!string.IsNullOrWhiteSpace(state.Race))
			{
				profile.Race = state.Race;
			}
			if (!string.IsNullOrWhiteSpace(state.Profession))
			{
				profile.Profession = state.Profession;
			}
			if (!string.IsNullOrWhiteSpace(state.Specialization))
			{
				profile.Specialization = state.Specialization;
			}
			profile.IsCharacterVerified = state.IsCharacterApiVerified;
			PlayerPresence presence = _presenceService.BuildPresence(state, profile);
			return new ProfileViewData(profile, presence);
		}

		public async Task<IReadOnlyList<PlayerPresence>> LoadOnlineAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			List<PlayerPresence> rows = new List<PlayerPresence>();
			try
			{
				if (_serverSync != null)
				{
					List<PlayerPresence> list = rows;
					list.AddRange(await _serverSync.GetOnlinePresenceAsync(_settings.RegionFilter.get_Value(), cancellationToken));
				}
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				throw;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load SPARK online profiles from the server.");
			}
			cancellationToken.ThrowIfCancellationRequested();
			RemoveOwnServerRows(rows);
			foreach (PlayerPresence localPresence in GetOwnPresenceRows())
			{
				UpdatePresence(rows, localPresence);
			}
			return NormalizePresenceRows(rows);
		}

		public IReadOnlyList<PlayerPresence> LoadCachedOnlineRows()
		{
			return NormalizePresenceRows(GetOwnPresenceRows());
		}

		private IReadOnlyList<PlayerPresence> NormalizePresenceRows(IEnumerable<PlayerPresence> rows)
		{
			return (from @group in (rows ?? Enumerable.Empty<PlayerPresence>()).Where(CanShowPresence).GroupBy((PlayerPresence presence) => presence?.Key() ?? string.Empty, StringComparer.OrdinalIgnoreCase)
				select @group.First()).ToList();
		}

		public async Task<ProfileViewData> LoadOnlineProfileAsync(PlayerPresence presence, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (presence == null)
			{
				return null;
			}
			if (IsMatureHidden(null, presence))
			{
				return null;
			}
			CharacterProfile profile = null;
			try
			{
				PlayerPresence livePresence = presence;
				ProfileDownload downloadedProfile = await _serverSync.DownloadProfileAsync(presence, cancellationToken, useOfflineMessage: true);
				if (downloadedProfile != null)
				{
					profile = downloadedProfile.ToCharacterProfile();
					presence = PreferLivePresence(livePresence, downloadedProfile.Presence, profile);
					if (IsMatureHidden(profile, presence))
					{
						return null;
					}
				}
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				return null;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to download SPARK profile before opening viewer.");
			}
			if (profile == null && !string.IsNullOrWhiteSpace(presence.ActiveProfileId))
			{
				profile = _profileRepository.Load(presence.ActiveProfileId);
			}
			if (profile == null)
			{
				profile = PresenceMapper.CreateFromPresence(presence);
			}
			if (IsMatureHidden(profile, presence))
			{
				return null;
			}
			return new ProfileViewData(profile, presence);
		}

		public async Task<ProfileViewData> LoadSavedProfileAsync(SavedProfile record, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (record == null)
			{
				return null;
			}
			if (IsMatureHidden(record.Profile, record.Presence))
			{
				return null;
			}
			PlayerPresence presence = record.Presence ?? new PlayerPresence();
			CharacterProfile profile = record.Profile ?? PresenceMapper.CreateFromPresence(presence);
			bool refreshedFromServer = false;
			PlayerPresence livePresence = null;
			PresenceMapper.FillMissingPresence(presence, profile);
			try
			{
				livePresence = await FindLivePresenceAsync(record, cancellationToken);
				ProfileDownload profileDownload = ((livePresence != null) ? (await _serverSync.DownloadProfileAsync(livePresence, cancellationToken)) : null);
				ProfileDownload downloadedProfile = profileDownload;
				if (livePresence != null)
				{
					presence = livePresence;
					PresenceMapper.FillMissingPresence(presence, profile);
				}
				if (downloadedProfile != null)
				{
					profile = downloadedProfile.ToCharacterProfile();
					presence = PreferLivePresence(livePresence, downloadedProfile.Presence, profile);
					if (IsMatureHidden(profile, presence))
					{
						return null;
					}
					_profileActions.SaveUpdatedProfile(profile, presence, record);
					refreshedFromServer = true;
				}
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				return null;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to refresh cached SPARK profile before opening viewer.");
			}
			if (!refreshedFromServer && livePresence == null)
			{
				MarkOffline(presence);
			}
			if (IsMatureHidden(profile, presence))
			{
				return null;
			}
			return new ProfileViewData(profile, presence);
		}

		private IReadOnlyList<PlayerPresence> GetOwnPresenceRows()
		{
			try
			{
				PlayerPresence presence = _presenceService.GetCurrentPresence();
				if (CanShowPresence(presence))
				{
					return ToOwnPresenceRows(presence);
				}
			}
			catch
			{
			}
			return ToOwnPresenceRows(_presenceLoop?.CurrentPresence);
		}

		private IReadOnlyList<PlayerPresence> ToOwnPresenceRows(PlayerPresence presence)
		{
			if (!CanShowPresence(presence))
			{
				return new List<PlayerPresence>();
			}
			return new List<PlayerPresence> { presence };
		}

		private bool CanShowPresence(PlayerPresence presence)
		{
			if (presence != null && presence.Status != RPStatus.Invisible && presence.CanShare && presence.HasActiveProfile && !_settings.IsBlockedAccount(presence.AccountName) && !IsMatureHidden(null, presence) && !string.IsNullOrWhiteSpace(presence.ActiveProfileId))
			{
				return !string.IsNullOrWhiteSpace(presence.OfficialCharacterName);
			}
			return false;
		}

		private bool IsMatureHidden(CharacterProfile profile, PlayerPresence presence)
		{
			if (_settings.ShowMatureProfiles.get_Value())
			{
				return false;
			}
			string accountName = TextUtil.FirstNonEmpty(presence?.AccountName, profile?.AccountName);
			string ownAccountName = _playerState?.GetCached()?.AccountName ?? string.Empty;
			if (!string.IsNullOrWhiteSpace(accountName) && !string.IsNullOrWhiteSpace(ownAccountName) && string.Equals(accountName.Trim(), ownAccountName.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (profile == null || !profile.IsMature)
			{
				return presence?.IsMature ?? false;
			}
			return true;
		}

		private async Task<PlayerPresence> FindLivePresenceAsync(SavedProfile record, CancellationToken cancellationToken)
		{
			if (_serverSync == null || record == null)
			{
				return null;
			}
			string accountName = TextUtil.FirstNonEmpty(record.Presence?.AccountName, record.Profile?.AccountName);
			string characterName = TextUtil.FirstNonEmpty(record.Presence?.OfficialCharacterName, record.Profile?.CharacterName);
			string profileId = TextUtil.FirstNonEmpty(record.Presence?.ActiveProfileId, record.Profile?.ProfileId);
			if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(characterName))
			{
				return null;
			}
			ProfileRegion region = record.Presence?.Region ?? record.Profile?.Region ?? _settings.RegionFilter.get_Value();
			return (await _serverSync.GetOnlinePresenceAsync(region, cancellationToken)).FirstOrDefault((PlayerPresence presence) => IsSameProfilePresence(presence, accountName, characterName, profileId));
		}

		private static bool IsSameProfilePresence(PlayerPresence presence, string accountName, string characterName, string profileId)
		{
			if (presence == null)
			{
				return false;
			}
			if (!string.Equals(presence.AccountName?.Trim(), accountName?.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (!string.Equals(presence.OfficialCharacterName?.Trim(), characterName?.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (!string.IsNullOrWhiteSpace(profileId))
			{
				return string.Equals(presence.ActiveProfileId?.Trim(), profileId.Trim(), StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}

		private static void UpdatePresence(List<PlayerPresence> rows, PlayerPresence presence)
		{
			if (rows != null && presence != null)
			{
				string key = presence.Key();
				int existingIndex = rows.FindIndex((PlayerPresence row) => string.Equals(row?.Key() ?? string.Empty, key, StringComparison.OrdinalIgnoreCase));
				if (existingIndex >= 0)
				{
					rows[existingIndex] = presence;
				}
				else
				{
					rows.Add(presence);
				}
			}
		}

		private void RemoveOwnServerRows(List<PlayerPresence> rows)
		{
			if (rows == null)
			{
				return;
			}
			string accountName = _playerState?.GetCached()?.AccountName?.Trim() ?? string.Empty;
			if (!string.IsNullOrWhiteSpace(accountName))
			{
				rows.RemoveAll((PlayerPresence row) => string.Equals(row?.AccountName?.Trim() ?? string.Empty, accountName, StringComparison.OrdinalIgnoreCase));
			}
		}

		private static PlayerPresence PreferLivePresence(PlayerPresence livePresence, PlayerPresence downloadedPresence, CharacterProfile profile)
		{
			PlayerPresence obj = livePresence ?? downloadedPresence ?? new PlayerPresence();
			PresenceMapper.FillPresence(obj, downloadedPresence);
			PresenceMapper.FillMissingPresence(obj, profile);
			return obj;
		}

		private static CharacterProfile CreateEmptyProfile(PlayerState state)
		{
			return new CharacterProfile
			{
				ProfileName = "No Active Profile",
				AccountName = (state?.AccountName?.Trim() ?? string.Empty),
				CharacterName = (state?.OfficialCharacterName?.Trim() ?? string.Empty),
				Race = (state?.Race?.Trim() ?? string.Empty),
				Profession = (state?.Profession?.Trim() ?? string.Empty),
				Specialization = (state?.Specialization?.Trim() ?? string.Empty),
				IsCharacterVerified = (state?.IsCharacterApiVerified ?? false),
				KnownFor = "Having no profile set as active.",
				Description = "Open the Profile Editor, create/choose a profile, then click on Set Active to begin!"
			};
		}

		private static void MarkOffline(PlayerPresence presence)
		{
			if (presence != null)
			{
				presence.Status = RPStatus.Offline;
				presence.LocationName = "Unknown";
				presence.IsLocationHidden = false;
				presence.LastSeen = default(DateTime);
			}
		}
	}
}
