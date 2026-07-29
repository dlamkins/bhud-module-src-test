using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using rp.spark.Models;
using rp.spark.Models.Api;

namespace rp.spark.Services
{
	public class ServerSync : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<ServerSync>();

		private static readonly TimeSpan FirstRetryDelay = TimeSpan.FromSeconds(30.0);

		private static readonly TimeSpan MaxRetryDelay = TimeSpan.FromMinutes(5.0);

		private static readonly TimeSpan AuthRetryDelay = TimeSpan.FromSeconds(15.0);

		private static readonly TimeSpan PresenceHeartbeatInterval = TimeSpan.FromSeconds(90.0);

		private readonly SparkClient _apiClient;

		private readonly SparkSettings _settings;

		private readonly PresenceLoop _presenceLoop;

		private readonly ProfileRepository _profileRepository;

		private readonly GW2TokenVerification _tokens;

		private readonly SemaphoreSlim _syncGate = new SemaphoreSlim(1, 1);

		private readonly object _syncWorkerLock = new object();

		private Func<CancellationToken, Task<bool>> _privacyReadyAsync;

		private CancellationTokenSource _cancellation;

		private Task _syncWorker;

		private bool _syncQueued;

		private bool _isStarted;

		private bool _isDisposed;

		private int _failureCount;

		private DateTime _nextSyncAttempt = DateTime.MinValue;

		private string _lastUploadedProfileId = string.Empty;

		private DateTime _lastProfileUpdatedAt = DateTime.MinValue;

		private string _lastRemovedPresenceKey = string.Empty;

		private bool _presencePublished;

		private PlayerPresence _lastPublishedPresence;

		public ServerSyncStatus CurrentStatus { get; private set; }

		public DateTime LastPublishedAt { get; private set; }

		public DateTime LastProfileUploadedAt { get; private set; }

		public event Action<ServerSyncStatus> StatusChanged;

		public ServerSync(SparkClient apiClient, SparkSettings settings, PresenceLoop presenceLoop, ProfileRepository profileRepository, GW2TokenVerification tokens)
		{
			_apiClient = apiClient;
			_settings = settings;
			_presenceLoop = presenceLoop;
			_profileRepository = profileRepository;
			_tokens = tokens;
			CurrentStatus = ServerSyncStatus.Disconnected("Server sync is unavailable.");
		}

		public void SetPrivacyCheck(Func<CancellationToken, Task<bool>> privacyReadyAsync)
		{
			_privacyReadyAsync = privacyReadyAsync;
		}

		public void Start()
		{
			if (!_isStarted && !_isDisposed)
			{
				_isStarted = true;
				_cancellation = new CancellationTokenSource();
				_presenceLoop.PresenceUpdated += HandlePresenceUpdated;
				_profileRepository.ProfileSaved += HandleProfileSaved;
				_profileRepository.ActiveProfileChanged += HandleActiveProfileChanged;
				SetStatus(GetOfflineStatus());
				QueueSync();
			}
		}

		public void Stop()
		{
			StopWorker();
		}

		private Task StopWorker()
		{
			if (!_isStarted)
			{
				return TakeSyncWorker();
			}
			_isStarted = false;
			_presenceLoop.PresenceUpdated -= HandlePresenceUpdated;
			_profileRepository.ProfileSaved -= HandleProfileSaved;
			_profileRepository.ActiveProfileChanged -= HandleActiveProfileChanged;
			CancellationTokenSource cancellation = _cancellation;
			_cancellation = null;
			Task worker = TakeSyncWorker();
			_syncQueued = false;
			if (cancellation == null)
			{
				return worker;
			}
			cancellation.Cancel();
			DisposeCancellation(cancellation, worker);
			return worker;
		}

		public void RefreshConfig()
		{
			_failureCount = 0;
			_nextSyncAttempt = DateTime.MinValue;
			SetStatus(GetOfflineStatus());
			QueueSync();
		}

		public void InvalidateProfileUpload()
		{
			_lastUploadedProfileId = string.Empty;
			_lastProfileUpdatedAt = DateTime.MinValue;
		}

		public void SyncSoon()
		{
			_nextSyncAttempt = DateTime.MinValue;
			QueueSync();
		}

		public async Task<ProfileDownload> DownloadProfileAsync(PlayerPresence presence, CancellationToken cancellationToken = default(CancellationToken), bool useOfflineMessage = false)
		{
			if (presence == null || string.IsNullOrWhiteSpace(presence.ActiveProfileId))
			{
				return null;
			}
			if (!_apiClient.IsConfigured)
			{
				SetStatus(GetOfflineStatus());
				return null;
			}
			string verificationToken = await GetVerificationTokenAsync(cancellationToken);
			SparkClient apiClient = _apiClient;
			string accountName = presence.AccountName;
			string officialCharacterName = presence.OfficialCharacterName;
			string activeProfileId = presence.ActiveProfileId;
			SparkSettings settings = _settings;
			ApiResult<ProfileDownload> result = await apiClient.DownloadProfileResultAsync(accountName, officialCharacterName, activeProfileId, settings != null && (settings.ShowMatureProfiles?.get_Value()).GetValueOrDefault(), verificationToken, cancellationToken);
			if (result.Succeeded)
			{
				Success("Profile downloaded.");
				return result.Value;
			}
			if (IsProfileUnavailable(result) || useOfflineMessage)
			{
				if (useOfflineMessage)
				{
					SetInfoStatus("User not online, loading local copy.");
				}
				return null;
			}
			Fail(result, "Could not download profile.");
			return null;
		}

		public async Task<IReadOnlyList<PlayerPresence>> GetOnlinePresenceAsync(ProfileRegion region, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!_apiClient.IsConfigured)
			{
				SetStatus(GetOfflineStatus());
				return new List<PlayerPresence>();
			}
			string verificationToken = await GetVerificationTokenAsync(cancellationToken);
			SparkClient apiClient = _apiClient;
			SparkSettings settings = _settings;
			ApiResult<PresenceListResponse> result = await apiClient.ListPresenceResultAsync(region, settings != null && (settings.ShowMatureProfiles?.get_Value()).GetValueOrDefault(), verificationToken, cancellationToken);
			if (result.Succeeded)
			{
				Success("Online list updated.");
				return result.Value?.Entries ?? new List<PlayerPresence>();
			}
			Fail(result, "Could not refresh online list.");
			return new List<PlayerPresence>();
		}

		private void HandlePresenceUpdated(PlayerPresence presence)
		{
			QueueSync();
		}

		private void HandleProfileSaved(CharacterProfile profile)
		{
			SyncSoon();
		}

		private void HandleActiveProfileChanged(string accountName, string officialCharacterName, string profileId)
		{
			_lastUploadedProfileId = string.Empty;
			_lastProfileUpdatedAt = DateTime.MinValue;
			_lastRemovedPresenceKey = string.Empty;
			SyncSoon();
		}

		private void QueueSync()
		{
			if (!_isStarted || _isDisposed || _cancellation == null || _cancellation.IsCancellationRequested)
			{
				return;
			}
			lock (_syncWorkerLock)
			{
				if (_isStarted && !_isDisposed && _cancellation != null && !_cancellation.IsCancellationRequested)
				{
					if (_syncWorker != null && !_syncWorker.IsCompleted)
					{
						_syncQueued = true;
						return;
					}
					_syncQueued = false;
					_syncWorker = RunQueueAsync(_cancellation.Token);
				}
			}
		}

		private async Task RunQueueAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				await SyncAsync(cancellationToken);
				lock (_syncWorkerLock)
				{
					if (!_syncQueued)
					{
						return;
					}
					_syncQueued = false;
				}
			}
		}

		private async Task SyncAsync(CancellationToken cancellationToken)
		{
			if (!(await _syncGate.WaitAsync(0, cancellationToken)))
			{
				return;
			}
			try
			{
				PlayerPresence presence = _presenceLoop.CurrentPresence;
				PlayerPresence removalPresence = FindPresenceToRemove(presence);
				if (removalPresence == null)
				{
					goto IL_0158;
				}
				if (!(DateTime.UtcNow < _nextSyncAttempt))
				{
					await RemovePresenceAsync(removalPresence, cancellationToken);
					if (presence != null && presence.CanShare)
					{
						goto IL_0158;
					}
				}
				goto end_IL_009e;
				IL_0158:
				if (CanSync(presence) && !(DateTime.UtcNow < _nextSyncAttempt) && await CheckPrivacyAsync(cancellationToken))
				{
					bool profileNeedsUpload = NeedsProfileUpload(presence);
					if (await UploadIfNeededAsync(presence, cancellationToken) && ShouldPublishPresence(presence, profileNeedsUpload))
					{
						await PublishPresenceAsync(presence, cancellationToken);
					}
				}
				end_IL_009e:;
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "SPARK server sync failed.");
				_failureCount++;
				_nextSyncAttempt = DateTime.UtcNow + GetRetryDelay(_failureCount);
				SetStatus(new ServerSyncStatus(ServerSyncState.ApiUnavailable, "Server sync failed. Retrying later.", DateTime.UtcNow, CurrentStatus?.LastSuccess ?? default(DateTime)));
			}
			finally
			{
				_syncGate.Release();
			}
		}

		private bool CanSync(PlayerPresence presence)
		{
			if (!_apiClient.IsConfigured)
			{
				SetStatus(GetOfflineStatus());
				return false;
			}
			if (presence == null)
			{
				SetStatus(ServerSyncStatus.Disconnected("Connecting, please wait."));
				return false;
			}
			if (!presence.CanShare)
			{
				SetStatus(ServerSyncStatus.Disconnected(presence.ShareBlockReason));
				return false;
			}
			return true;
		}

		private async Task<bool> CheckPrivacyAsync(CancellationToken cancellationToken)
		{
			if (_privacyReadyAsync == null)
			{
				return true;
			}
			bool num = await _privacyReadyAsync(cancellationToken);
			if (!num)
			{
				SetInfoStatus("Waiting for block list to sync with SPARK.");
			}
			return num;
		}

		private async Task<bool> UploadIfNeededAsync(PlayerPresence presence, CancellationToken cancellationToken)
		{
			if (!NeedsProfileUpload(presence))
			{
				return true;
			}
			CharacterProfile profile = _profileRepository.Load(presence.ActiveProfileId);
			if (profile == null)
			{
				SetStatus(ServerSyncStatus.Disconnected("Active profile not detected. Please set a profile to be your active one."));
				return false;
			}
			string verificationToken = await GetRequiredVerificationTokenAsync(cancellationToken);
			if (string.IsNullOrWhiteSpace(verificationToken))
			{
				return false;
			}
			ApiResult<bool> result = await _apiClient.UploadProfileResultAsync(profile, presence, verificationToken, cancellationToken);
			if (!result.Succeeded)
			{
				Fail(result, "Could not upload profile.");
				return false;
			}
			_lastUploadedProfileId = presence.ActiveProfileId?.Trim() ?? string.Empty;
			_lastProfileUpdatedAt = presence.ProfileUpdatedAtTime;
			LastProfileUploadedAt = DateTime.UtcNow;
			Success("Profile uploaded.");
			return true;
		}

		private async Task PublishPresenceAsync(PlayerPresence presence, CancellationToken cancellationToken)
		{
			string verificationToken = await GetRequiredVerificationTokenAsync(cancellationToken);
			if (!string.IsNullOrWhiteSpace(verificationToken))
			{
				ApiResult<bool> result = await _apiClient.PublishPresenceResultAsync(presence, verificationToken, cancellationToken);
				if (!result.Succeeded)
				{
					Fail(result, "Could not establish connection.");
					return;
				}
				_presencePublished = true;
				_lastPublishedPresence = PresenceMapper.ClonePresence(presence);
				_lastRemovedPresenceKey = string.Empty;
				LastPublishedAt = DateTime.UtcNow;
				Success("SPARK available.");
			}
		}

		private async Task RemovePresenceAsync(PlayerPresence presence, CancellationToken cancellationToken)
		{
			PlayerPresence removalPresence = PresenceMapper.CreateOfflinePresence(presence);
			string verificationToken = await GetRequiredVerificationTokenAsync(cancellationToken);
			if (string.IsNullOrWhiteSpace(verificationToken))
			{
				return;
			}
			ApiResult<bool> result = await _apiClient.PublishPresenceResultAsync(removalPresence, verificationToken, cancellationToken);
			if (!result.Succeeded)
			{
				Fail(result, "Could not remove presence.");
				return;
			}
			_presencePublished = false;
			_lastRemovedPresenceKey = removalPresence.Key();
			if (IsPresenceOwner(_lastPublishedPresence, removalPresence))
			{
				_lastPublishedPresence = null;
			}
			LastPublishedAt = DateTime.UtcNow;
			Success(string.Empty);
		}

		private bool NeedsProfileUpload(PlayerPresence presence)
		{
			if (presence == null || !presence.HasActiveProfile)
			{
				return false;
			}
			string activeProfileId = presence.ActiveProfileId?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(activeProfileId))
			{
				return false;
			}
			if (string.Equals(activeProfileId, _lastUploadedProfileId, StringComparison.OrdinalIgnoreCase))
			{
				return presence.ProfileUpdatedAtTime > _lastProfileUpdatedAt;
			}
			return true;
		}

		private bool ShouldPublishPresence(PlayerPresence presence, bool profileWasUploaded)
		{
			if (presence == null)
			{
				return false;
			}
			if (!_presencePublished || _lastPublishedPresence == null)
			{
				return true;
			}
			if (profileWasUploaded)
			{
				return true;
			}
			if (DateTime.UtcNow - LastPublishedAt >= PresenceHeartbeatInterval)
			{
				return true;
			}
			return PresenceChanged(_lastPublishedPresence, presence);
		}

		private static bool PresenceChanged(PlayerPresence previous, PlayerPresence current)
		{
			if (previous == null || current == null)
			{
				return true;
			}
			if (SameTrimmed(previous.AccountName, current.AccountName) && SameTrimmed(previous.OfficialCharacterName, current.OfficialCharacterName) && SameTrimmed(previous.DisplayCharacterName, current.DisplayCharacterName) && SameTrimmed(previous.Race, current.Race) && SameTrimmed(previous.CustomRace, current.CustomRace) && SameTrimmed(previous.Profession, current.Profession) && SameTrimmed(previous.CustomProfession, current.CustomProfession) && SameTrimmed(previous.ActiveProfileId, current.ActiveProfileId) && previous.IsMature == current.IsMature && previous.Experience == current.Experience && SameTrimmed(previous.ActiveProfileName, current.ActiveProfileName) && !(previous.ProfileUpdatedAtTime != current.ProfileUpdatedAtTime) && previous.Status == current.Status && SameTrimmed(previous.KnownFor, current.KnownFor) && SameTrimmed(previous.Currently, current.Currently) && SameTrimmed(previous.OutOfCharacterInfo, current.OutOfCharacterInfo) && SameTrimmed(previous.LocationName, current.LocationName) && previous.IsLocationHidden == current.IsLocationHidden && previous.Region == current.Region && previous.IsVerified == current.IsVerified && previous.IsInGame == current.IsInGame && previous.HasActiveProfile == current.HasActiveProfile && previous.ShareEnabled == current.ShareEnabled && previous.CanShare == current.CanShare && SameTrimmed(previous.ShareBlockReason, current.ShareBlockReason))
			{
				return !ProfileDiscoveryMapper.AreEqual(previous.DiscoveryTags, current.DiscoveryTags);
			}
			return true;
		}

		private static bool SameTrimmed(string left, string right)
		{
			return string.Equals(left?.Trim() ?? string.Empty, right?.Trim() ?? string.Empty, StringComparison.Ordinal);
		}

		private PlayerPresence FindPresenceToRemove(PlayerPresence presence)
		{
			if (!_apiClient.IsConfigured)
			{
				return null;
			}
			if (presence != null && presence.CanShare)
			{
				if (_lastPublishedPresence != null && !IsPresenceOwner(_lastPublishedPresence, presence))
				{
					return _lastPublishedPresence;
				}
				return null;
			}
			PlayerPresence removalPresence = _lastPublishedPresence;
			if (removalPresence == null && HasPresenceIdentity(presence))
			{
				removalPresence = presence;
			}
			if (!HasPresenceIdentity(removalPresence))
			{
				return null;
			}
			string key = removalPresence.Key();
			if (!_presencePublished && string.Equals(_lastRemovedPresenceKey, key, StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return removalPresence;
		}

		private async Task<string> GetVerificationTokenAsync(CancellationToken cancellationToken)
		{
			return (_tokens != null) ? (await _tokens.GetTokenAsync(cancellationToken)) : string.Empty;
		}

		private async Task<string> GetRequiredVerificationTokenAsync(CancellationToken cancellationToken)
		{
			string verificationToken = await GetVerificationTokenAsync(cancellationToken);
			if (!string.IsNullOrWhiteSpace(verificationToken))
			{
				return verificationToken;
			}
			SetWaitingForAuthStatus();
			return string.Empty;
		}

		private void SetWaitingForAuthStatus()
		{
			_failureCount = 0;
			_nextSyncAttempt = DateTime.UtcNow + AuthRetryDelay;
			SetStatus(new ServerSyncStatus(ServerSyncState.Info, "Waiting for GW2 API verification before syncing to SPARK.", DateTime.UtcNow, CurrentStatus?.LastSuccess ?? default(DateTime)));
		}

		private static bool HasPresenceIdentity(PlayerPresence presence)
		{
			if (!string.IsNullOrWhiteSpace(presence?.AccountName))
			{
				return !string.IsNullOrWhiteSpace(presence?.OfficialCharacterName);
			}
			return false;
		}

		private static bool IsPresenceOwner(PlayerPresence left, PlayerPresence right)
		{
			return string.Equals(left?.Key() ?? string.Empty, right?.Key() ?? string.Empty, StringComparison.OrdinalIgnoreCase);
		}

		private static bool IsProfileUnavailable<T>(ApiResult<T> result)
		{
			if (result == null || result.StatusCode.GetValueOrDefault() != HttpStatusCode.NotFound)
			{
				if (result == null)
				{
					return false;
				}
				return result.StatusCode.GetValueOrDefault() == HttpStatusCode.Forbidden;
			}
			return true;
		}

		private void Fail<T>(ApiResult<T> result, string fallbackMessage)
		{
			if (result != null && result.StatusCode.GetValueOrDefault() == HttpStatusCode.Unauthorized)
			{
				_tokens?.Clear();
				SetWaitingForAuthStatus();
				return;
			}
			_failureCount++;
			_nextSyncAttempt = DateTime.UtcNow + GetRetryDelay(_failureCount);
			ServerSyncState state = GetFailureState(result);
			string message = ((result != null && result.StatusCode.GetValueOrDefault() == HttpStatusCode.Forbidden && !string.IsNullOrWhiteSpace(result.ErrorMessage)) ? result.ErrorMessage : fallbackMessage);
			SetStatus(new ServerSyncStatus(state, message, DateTime.UtcNow, CurrentStatus?.LastSuccess ?? default(DateTime)));
		}

		private static ServerSyncState GetFailureState<T>(ApiResult<T> result)
		{
			if (result != null && result.FailureKind == ApiFailure.BlockedByWindows)
			{
				return ServerSyncState.BlockedByWindows;
			}
			if (result != null && result.FailureKind == ApiFailure.NotConfigured)
			{
				return ServerSyncState.Disconnected;
			}
			if ((result != null && result.FailureKind == ApiFailure.Timeout) || (result != null && result.FailureKind == ApiFailure.Network))
			{
				return ServerSyncState.ApiUnavailable;
			}
			if (result != null && result.StatusCode >= HttpStatusCode.InternalServerError)
			{
				return ServerSyncState.ServerError;
			}
			return ServerSyncState.ApiUnavailable;
		}

		private void Success(string message)
		{
			_failureCount = 0;
			_nextSyncAttempt = DateTime.MinValue;
			SetStatus(new ServerSyncStatus(ServerSyncState.Connected, message, DateTime.UtcNow, DateTime.UtcNow));
		}

		private void SetInfoStatus(string message)
		{
			_failureCount = 0;
			_nextSyncAttempt = DateTime.MinValue;
			SetStatus(new ServerSyncStatus(ServerSyncState.Info, message, DateTime.UtcNow, CurrentStatus?.LastSuccess ?? default(DateTime)));
		}

		private ServerSyncStatus GetOfflineStatus()
		{
			if (!_apiClient.IsConfigured)
			{
				return ServerSyncStatus.Disconnected("Server URL is invalid or unable to be found.");
			}
			SparkSettings settings = _settings;
			if (settings == null || !(settings.BroadcastProfile?.get_Value()).GetValueOrDefault())
			{
				return ServerSyncStatus.Disconnected("Profile sharing is disabled.");
			}
			return ServerSyncStatus.Disconnected("Waiting for Spark...");
		}

		private static TimeSpan GetRetryDelay(int failureCount)
		{
			int multiplier = Math.Max(1, Math.Min(8, failureCount));
			TimeSpan delay = TimeSpan.FromTicks(FirstRetryDelay.Ticks * multiplier);
			if (!(delay > MaxRetryDelay))
			{
				return delay;
			}
			return MaxRetryDelay;
		}

		private void SetStatus(ServerSyncStatus status)
		{
			if (CurrentStatus == null || status == null || status.State == ServerSyncState.Connected || CurrentStatus.State != status.State || !string.Equals(CurrentStatus.Message, status.Message, StringComparison.Ordinal))
			{
				CurrentStatus = status ?? ServerSyncStatus.Disconnected("Unable to connect to SPARK.");
				this.StatusChanged?.Invoke(CurrentStatus);
			}
		}

		private Task TakeSyncWorker()
		{
			lock (_syncWorkerLock)
			{
				Task syncWorker = _syncWorker;
				_syncWorker = null;
				_syncQueued = false;
				return syncWorker;
			}
		}

		private static void DisposeCancellation(CancellationTokenSource cancellation, Task worker)
		{
			TaskCleanup.DisposeWhenComplete(worker, cancellation);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				StopWorker();
			}
		}
	}
}
