using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Models.Api;

namespace rp.spark.Services
{
	public class NearbyPresenceService : IDisposable
	{
		private static readonly TimeSpan PublishInterval = TimeSpan.FromSeconds(10.0);

		private readonly SparkClient _apiClient;

		private readonly SparkSettings _settings;

		private readonly PresenceLoop _presenceLoop;

		private readonly GW2TokenVerification _tokens;

		private readonly SemaphoreSlim _publishGate = new SemaphoreSlim(1, 1);

		private CancellationTokenSource _loopCancellation;

		private Task _loopTask;

		private bool _published;

		public string LastStatus { get; private set; } = string.Empty;


		public uint CurrentShardId { get; private set; }

		public string CurrentServerAddress { get; private set; } = string.Empty;


		public NearbyPresenceService(SparkClient apiClient, SparkSettings settings, PresenceLoop presenceLoop, GW2TokenVerification tokens)
		{
			_apiClient = apiClient;
			_settings = settings;
			_presenceLoop = presenceLoop;
			_tokens = tokens;
		}

		public void Start()
		{
			if (_loopTask == null || _loopTask.IsCompleted)
			{
				_loopCancellation = new CancellationTokenSource();
				_loopTask = RunAsync(_loopCancellation.Token);
			}
		}

		public void Stop()
		{
			_loopCancellation?.Cancel();
			_loopCancellation?.Dispose();
			_loopCancellation = null;
			_loopTask = null;
		}

		public async Task<IReadOnlyList<NearbyPresence>> SearchAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			NearbyPresenceSearchRequest query = BuildSearchRequest();
			if (query == null)
			{
				return Array.Empty<NearbyPresence>();
			}
			string token = await GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (string.IsNullOrWhiteSpace(token))
			{
				LastStatus = "Add a valid GW2 API key before checking nearby players.";
				return Array.Empty<NearbyPresence>();
			}
			ApiResult<NearbyPresenceListResponse> result = await _apiClient.SearchNearbyPresenceResultAsync(query, token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!result.Succeeded)
			{
				LastStatus = result.ErrorMessage ?? "Unable to refresh nearby players.";
				return Array.Empty<NearbyPresence>();
			}
			LastStatus = "Nearby players updated.";
			return (result.Value?.Entries ?? new List<NearbyPresence>()).Where((NearbyPresence entry) => entry?.Presence != null).ToList();
		}

		public async Task PublishNowAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			await _publishGate.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				NearbyPresence nearby = await BuildNearbyPresenceAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (nearby == null)
				{
					if (_published)
					{
						await RemoveAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
					return;
				}
				string token = await GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (string.IsNullOrWhiteSpace(token))
				{
					LastStatus = "Add a valid GW2 API key before sharing nearby presence.";
					return;
				}
				ApiResult<bool> result = await _apiClient.PublishNearbyPresenceResultAsync(nearby, token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				_published = result.Succeeded;
				LastStatus = (result.Succeeded ? "Nearby presence shared." : (result.ErrorMessage ?? "Unable to share nearby presence."));
			}
			finally
			{
				_publishGate.Release();
			}
		}

		public async Task RemoveAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			string token = await GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!string.IsNullOrWhiteSpace(token))
			{
				ApiResult<bool> result = await _apiClient.RemoveNearbyPresenceResultAsync(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (result.Succeeded)
				{
					_published = false;
				}
				LastStatus = (result.Succeeded ? "Nearby presence hidden." : (result.ErrorMessage ?? "Unable to hide nearby presence."));
			}
		}

		private async Task RunAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					if (_settings.ShowNearbyPresence.get_Value())
					{
						await PublishNowAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
					else if (_published)
					{
						await RemoveAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex)
				{
					LastStatus = ex.Message;
				}
				try
				{
					await Task.Delay(PublishInterval, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException)
				{
					return;
				}
			}
		}

		private async Task<NearbyPresence> BuildNearbyPresenceAsync(CancellationToken cancellationToken)
		{
			PlayerPresence presence = await GetPresenceAsync(cancellationToken, forceRefresh: true).ConfigureAwait(continueOnCapturedContext: false);
			string sharingNotice = BuildSharingNotice(presence);
			if (!string.IsNullOrWhiteSpace(sharingNotice))
			{
				LastStatus = sharingNotice;
				return null;
			}
			if (!TryGetLocation(out var mapId, out var shardId, out var serverAddress, out var x, out var y, out var z))
			{
				return null;
			}
			return new NearbyPresence
			{
				Presence = presence,
				MapId = mapId,
				ShardId = shardId,
				ServerAddress = serverAddress,
				HasPosition = true,
				X = x,
				Y = y,
				Z = z,
				LastSeen = DateTime.UtcNow
			};
		}

		private NearbyPresenceSearchRequest BuildSearchRequest()
		{
			if (!TryGetLocation(out var mapId, out var shardId, out var serverAddress, out var x, out var y, out var z))
			{
				return null;
			}
			return new NearbyPresenceSearchRequest
			{
				Region = _settings.RegionFilter.get_Value(),
				IncludeMature = _settings.ShowMatureProfiles.get_Value(),
				MapId = mapId,
				ShardId = shardId,
				ServerAddress = serverAddress,
				HasPosition = true,
				X = x,
				Y = y,
				Z = z,
				MaxDistanceMeters = 600.0
			};
		}

		private bool TryGetLocation(out int mapId, out uint shardId, out string serverAddress, out double x, out double y, out double z)
		{
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			mapId = 0;
			shardId = 0u;
			serverAddress = string.Empty;
			x = (y = (z = 0.0));
			CurrentShardId = 0u;
			CurrentServerAddress = string.Empty;
			if (_settings.HideLocation.get_Value())
			{
				LastStatus = "Nearby Players are unavailable while location is hidden.";
				return false;
			}
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				LastStatus = "GW2 location data is not available yet.";
				return false;
			}
			mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			shardId = GameService.Gw2Mumble.get_Info().get_ShardId();
			serverAddress = Convert.ToString(GameService.Gw2Mumble.get_Info().get_ServerAddress())?.Trim() ?? string.Empty;
			Vector3 position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			x = position.X;
			y = position.Y;
			z = position.Z;
			if (mapId <= 0 || shardId == 0)
			{
				LastStatus = "Enter a map before checking nearby players.";
				return false;
			}
			CurrentShardId = shardId;
			CurrentServerAddress = serverAddress;
			return true;
		}

		private async Task<PlayerPresence> GetPresenceAsync(CancellationToken cancellationToken, bool forceRefresh = false)
		{
			if (!forceRefresh && _presenceLoop?.CurrentPresence != null)
			{
				return _presenceLoop.CurrentPresence;
			}
			return (_presenceLoop != null) ? (await _presenceLoop.RefreshAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) : null;
		}

		public async Task<string> GetSharingNoticeAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!_settings.ShowNearbyPresence.get_Value())
			{
				return string.Empty;
			}
			return BuildSharingNotice(await GetPresenceAsync(cancellationToken, forceRefresh: true).ConfigureAwait(continueOnCapturedContext: false));
		}

		private static string BuildSharingNotice(PlayerPresence presence)
		{
			if (presence == null)
			{
				return "'Show me nearby' is on, but your presence is not available yet.";
			}
			if (!presence.HasActiveProfile)
			{
				return "'Show me nearby' is on, but no active profile is selected, so you will not appear to other nearby players.";
			}
			if (presence.IsLocationHidden)
			{
				return "'Show me nearby' is on, but location is hidden, so you will not appear to other nearby players.";
			}
			if (presence.Status == RPStatus.Invisible)
			{
				return "'Show me nearby is on', but invisible status prevents nearby sharing.";
			}
			if (!presence.CanShare && !string.IsNullOrWhiteSpace(presence.ShareBlockReason))
			{
				return "'Show me nearby is on', but you will not appear: " + presence.ShareBlockReason;
			}
			if (!presence.CanShare)
			{
				return "'Show me nearby is on', but your profile is not currently shareable.";
			}
			return string.Empty;
		}

		public bool IsCurrentMapIp(NearbyPresence nearby)
		{
			if (nearby == null)
			{
				return false;
			}
			if (!string.IsNullOrWhiteSpace(CurrentServerAddress) && !string.IsNullOrWhiteSpace(nearby.ServerAddress))
			{
				return string.Equals(nearby.ServerAddress.Trim(), CurrentServerAddress.Trim(), StringComparison.OrdinalIgnoreCase);
			}
			if (CurrentShardId != 0)
			{
				return nearby.ShardId == CurrentShardId;
			}
			return false;
		}

		private Task<string> GetTokenAsync(CancellationToken cancellationToken)
		{
			return _tokens.GetTokenAsync(cancellationToken);
		}

		public void Dispose()
		{
			Stop();
			_publishGate.Dispose();
		}
	}
}
