using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using rp.spark.Models;
using rp.spark.Models.Api;

namespace rp.spark.Services
{
	public sealed class ProfileActions : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<ProfileActions>();

		private const string BlockRemovalHelp = "You can remove blocks using the 'Manage Blocks' in the settings menu.";

		private const string ReportUnavailableMessage = "Report failed. Profile not found on SPARK server. Profiles only exist for 24h on the server at a time.";

		private readonly ProfileCache _profileCache;

		private readonly SparkSettings _settings;

		private readonly PlayerStateService _playerState;

		private readonly SparkClient _apiClient;

		private readonly GW2TokenVerification _tokens;

		private readonly ServerSync _serverSync;

		private readonly object _blockSyncLock = new object();

		private readonly SemaphoreSlim _fullBlockSyncGate = new SemaphoreSlim(1, 1);

		private CancellationTokenSource _blockSyncCancellation;

		private Task _blockSyncWorker;

		private bool _needsFullBlockSync;

		private bool _blocklistSynced;

		private string _lastSyncedBlocklistKey = string.Empty;

		private int _blockSyncOperations;

		private bool _isStarted;

		private bool _isDisposed;

		public bool IsBlockSyncInProgress => Volatile.Read(ref _blockSyncOperations) > 0;

		public event Action SavedProfilesChanged;

		public event Action BlockedAccountsChanged;

		public ProfileActions(ProfileCache profileCache, SparkSettings settings, PlayerStateService playerState, SparkClient apiClient, GW2TokenVerification tokens, ServerSync serverSync)
		{
			_profileCache = profileCache;
			_settings = settings;
			_playerState = playerState;
			_apiClient = apiClient;
			_tokens = tokens;
			_serverSync = serverSync;
		}

		public void Start()
		{
			if (!_isStarted && !_isDisposed)
			{
				_isStarted = true;
				_blockSyncCancellation = new CancellationTokenSource();
				if (_serverSync != null)
				{
					_serverSync.StatusChanged += HandleServerSyncStatusChanged;
				}
				SyncBlocks();
			}
		}

		public void SyncBlocks()
		{
			if (!_isDisposed)
			{
				lock (_blockSyncLock)
				{
					_needsFullBlockSync = true;
				}
				StartBlockSyncWorker();
			}
		}

		public async Task<bool> EnsureBlocksSyncedAsync(CancellationToken cancellationToken)
		{
			if (_isDisposed)
			{
				return false;
			}
			List<string> accountNames = LocalBlocks();
			if (accountNames.Count == 0)
			{
				return true;
			}
			string blocklistKey = GetBlocklistKey(accountNames);
			if (IsBlocklistSynced(blocklistKey))
			{
				return true;
			}
			if (!(await PushBlocksAsync(accountNames, blocklistKey, cancellationToken)))
			{
				SyncBlocks();
				return false;
			}
			string currentBlocklistKey = GetBlocklistKey(LocalBlocks());
			if (IsBlocklistSynced(currentBlocklistKey))
			{
				return true;
			}
			SyncBlocks();
			return false;
		}

		public string ToggleProfileBookmark(CharacterProfile profile, PlayerPresence presence)
		{
			try
			{
				if (IsProfileBookmarked(profile, presence))
				{
					string cacheKey = ProfileCache.GetProfileCacheKey(profile, presence);
					_profileCache.RemoveBookmark(cacheKey);
					NotifySavedChanged();
					return "Bookmark removed.";
				}
				_profileCache.Bookmark(profile, presence);
				NotifySavedChanged();
				return "Profile bookmarked locally.";
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to update SPARK profile bookmark.");
				return "Couldn't update this bookmark.";
			}
		}

		public bool IsProfileBookmarked(CharacterProfile profile, PlayerPresence presence)
		{
			string cacheKey = ProfileCache.GetProfileCacheKey(profile, presence);
			if (!string.IsNullOrWhiteSpace(cacheKey))
			{
				return _profileCache.IsBookmarked(cacheKey);
			}
			return false;
		}

		public bool IsPresenceBookmarked(PlayerPresence presence)
		{
			string cacheKey = ProfileCache.GetProfileCacheKey(null, presence);
			if (!string.IsNullOrWhiteSpace(cacheKey))
			{
				return _profileCache.IsBookmarked(cacheKey);
			}
			return false;
		}

		public void RemoveBookmark(SavedProfileSummary savedProfile)
		{
			if (savedProfile != null && !string.IsNullOrWhiteSpace(savedProfile.CacheKey))
			{
				_profileCache.RemoveBookmark(savedProfile.CacheKey);
				NotifySavedChanged();
			}
		}

		public void RemoveSavedProfile(SavedProfileSummary savedProfile)
		{
			if (savedProfile != null && !string.IsNullOrWhiteSpace(savedProfile.CacheKey))
			{
				_profileCache.Remove(savedProfile.CacheKey);
				NotifySavedChanged();
			}
		}

		public bool IsSavedProfileBookmarked(SavedProfile record)
		{
			if (record == null)
			{
				return false;
			}
			if (string.IsNullOrWhiteSpace(record.CacheKey))
			{
				return record.IsBookmarked;
			}
			return _profileCache.IsBookmarked(record.CacheKey);
		}

		public void SaveUpdatedProfile(CharacterProfile profile, PlayerPresence presence, SavedProfile record)
		{
			_profileCache.Save(profile, presence, IsSavedProfileBookmarked(record));
			NotifySavedChanged();
		}

		public void SaveToRecent(CharacterProfile profile, PlayerPresence presence)
		{
			if (CanSaveToRecent(profile, presence))
			{
				try
				{
					_profileCache.Save(profile, presence);
					NotifySavedChanged();
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to cache viewed SPARK profile.");
				}
			}
		}

		public string ToggleProfileBlock(CharacterProfile profile, PlayerPresence presence)
		{
			string accountName = TextUtil.FirstNonEmpty(presence?.AccountName, profile?.AccountName);
			if (IsProfileBlocked(profile, presence))
			{
				return UnblockAccount(accountName);
			}
			string result = BlockAccount(accountName);
			if (!_settings.IsBlockedAccount(accountName))
			{
				return result;
			}
			return BlockedMessage(GetProfileBlockSubject(profile, presence, accountName), includeRemovalHelp: true);
		}

		public bool IsProfileBlocked(CharacterProfile profile, PlayerPresence presence)
		{
			string accountName = TextUtil.FirstNonEmpty(presence?.AccountName, profile?.AccountName);
			return _settings.IsBlockedAccount(accountName);
		}

		public string BlockAccount(string accountName)
		{
			accountName = accountName?.Trim() ?? string.Empty;
			if (!SparkSettings.IsValidAccountName(accountName))
			{
				return "No account name available to block.";
			}
			string currentAccountName = _playerState?.GetCached()?.AccountName ?? string.Empty;
			if (string.Equals(accountName, currentAccountName, StringComparison.OrdinalIgnoreCase))
			{
				return "You can't block your own account.";
			}
			try
			{
				if (_settings.AddBlockedAccount(accountName))
				{
					NotifyBlockedAccountsChanged();
					QueueBlockChange(accountName, isBlocked: true);
					SyncBlocks();
				}
				return BlockedMessage(accountName);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to block a SPARK account.");
				return "Couldn't update the block list.";
			}
		}

		public async Task<string> ReportProfile(CharacterProfile profile, PlayerPresence presence, string reason)
		{
			reason = reason?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(reason))
			{
				return "Please add a short reason for the report.";
			}
			if (reason.Length > 140)
			{
				reason = reason.Substring(0, 140);
			}
			if (!CanReportProfile(profile, presence))
			{
				return "Report failed. Profile not found on SPARK server. Profiles only exist for 24h on the server at a time.";
			}
			try
			{
				string verificationToken = await GetVerificationTokenAsync(CancellationToken.None);
				if (string.IsNullOrWhiteSpace(verificationToken))
				{
					return "Report failed. Add a GW2 API key in Blish HUD first.";
				}
				ApiResult<ProfileReportResponse> result = await _apiClient.ReportProfileResultAsync(ProfileReportRequest.FromProfile(profile, presence, reason), verificationToken);
				if (result.Succeeded)
				{
					string reportId = result.Value?.ReportId?.Trim() ?? string.Empty;
					return string.IsNullOrWhiteSpace(reportId) ? "Report submitted. Thank you." : ("Report submitted. Report #" + reportId + ".");
				}
				if (result.StatusCode.GetValueOrDefault() == HttpStatusCode.NotFound || result.StatusCode.GetValueOrDefault() == HttpStatusCode.Forbidden)
				{
					return "Report failed. Profile not found on SPARK server. Profiles only exist for 24h on the server at a time.";
				}
				if (result.StatusCode.GetValueOrDefault() == HttpStatusCode.Unauthorized)
				{
					return "Report failed. Add a GW2 API key in Blish HUD first.";
				}
				return string.IsNullOrWhiteSpace(result.ErrorMessage) ? "Report failed." : result.ErrorMessage;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to report SPARK profile.");
				return "Report failed.";
			}
		}

		public string UnblockAccount(string accountName)
		{
			accountName = accountName?.Trim() ?? string.Empty;
			if (!SparkSettings.IsValidAccountName(accountName))
			{
				return "No account name available to unblock.";
			}
			try
			{
				if (_settings.RemoveBlockedAccount(accountName))
				{
					NotifyBlockedAccountsChanged();
					QueueBlockChange(accountName, isBlocked: false);
					SyncBlocks();
				}
				return "Unblocked " + accountName + ".";
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to unblock a SPARK account.");
				return "Couldn't update the block list.";
			}
		}

		public void WatchSavedProfiles(Action handler)
		{
			if (handler != null)
			{
				SavedProfilesChanged += handler;
			}
		}

		public void UnwatchSavedProfiles(Action handler)
		{
			if (handler != null)
			{
				SavedProfilesChanged -= handler;
			}
		}

		public void WatchBlockedAccounts(Action handler)
		{
			if (handler != null)
			{
				BlockedAccountsChanged += handler;
			}
		}

		public void UnwatchBlockedAccounts(Action handler)
		{
			if (handler != null)
			{
				BlockedAccountsChanged -= handler;
			}
		}

		private void HandleServerSyncStatusChanged(ServerSyncStatus status)
		{
			if (status != null && status.State == ServerSyncState.Connected)
			{
				SyncBlocks();
			}
		}

		private void StartBlockSyncWorker()
		{
			if (!_isStarted || _isDisposed)
			{
				return;
			}
			lock (_blockSyncLock)
			{
				if (_blockSyncWorker == null || _blockSyncWorker.IsCompleted)
				{
					if (_blockSyncCancellation == null || _blockSyncCancellation.IsCancellationRequested)
					{
						_blockSyncCancellation = new CancellationTokenSource();
					}
					_blockSyncWorker = RunBlocksAsync(_blockSyncCancellation.Token);
				}
			}
		}

		private async Task RunBlocksAsync(CancellationToken cancellationToken)
		{
			try
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					lock (_blockSyncLock)
					{
						if (!_needsFullBlockSync)
						{
							return;
						}
						_needsFullBlockSync = false;
					}
					if (!(await SyncBlocksAsync(cancellationToken)))
					{
						lock (_blockSyncLock)
						{
							_needsFullBlockSync = true;
						}
						break;
					}
				}
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
			}
			catch (Exception ex)
			{
				lock (_blockSyncLock)
				{
					_needsFullBlockSync = true;
				}
				Logger.Warn(ex, "Failed to sync the SPARK block list.");
			}
		}

		private Task<bool> SyncBlocksAsync(CancellationToken cancellationToken)
		{
			List<string> accountNames = LocalBlocks();
			return PushBlocksAsync(accountNames, GetBlocklistKey(accountNames), cancellationToken);
		}

		private async Task<bool> PushBlocksAsync(IReadOnlyList<string> accountNames, string blocklistKey, CancellationToken cancellationToken)
		{
			if (_apiClient == null || !_apiClient.IsConfigured)
			{
				return false;
			}
			await _fullBlockSyncGate.WaitAsync(cancellationToken);
			BeginBlockSyncOperation();
			try
			{
				if (IsBlocklistSynced(blocklistKey))
				{
					return true;
				}
				string verificationToken = await GetVerificationTokenAsync(cancellationToken);
				if (string.IsNullOrWhiteSpace(verificationToken))
				{
					return false;
				}
				ApiResult<bool> result = await _apiClient.ReplaceBlocklistResultAsync(accountNames, verificationToken, cancellationToken);
				if (result.Succeeded)
				{
					MarkBlocklistSynced(blocklistKey);
					Logger.Info("Synced {count} SPARK blocked account(s) to the server.", new object[1] { accountNames.Count });
					return true;
				}
				if (result.StatusCode.GetValueOrDefault() == HttpStatusCode.NotFound || result.StatusCode.GetValueOrDefault() == HttpStatusCode.MethodNotAllowed)
				{
					Logger.Warn("SPARK full block-list sync endpoint is unavailable.");
				}
				Logger.Warn("Failed to sync the full SPARK block list to the server. Status: {status}.", new object[1] { result.StatusCode });
				return false;
			}
			finally
			{
				EndBlockSyncOperation();
				_fullBlockSyncGate.Release();
			}
		}

		private void QueueBlockChange(string accountName, bool isBlocked)
		{
			CancellationToken cancellationToken = _blockSyncCancellation?.Token ?? CancellationToken.None;
			PushBlockChangeAsync(accountName, isBlocked, cancellationToken);
		}

		private async Task PushBlockChangeAsync(string accountName, bool isBlocked, CancellationToken cancellationToken)
		{
			if (_apiClient == null || string.IsNullOrWhiteSpace(accountName))
			{
				return;
			}
			try
			{
				string verificationToken = await GetVerificationTokenAsync(cancellationToken);
				if (!string.IsNullOrWhiteSpace(verificationToken))
				{
					ApiResult<bool> apiResult = ((!isBlocked) ? (await _apiClient.UnblockAccountResultAsync(accountName, verificationToken, cancellationToken)) : (await _apiClient.BlockAccountResultAsync(accountName, verificationToken, cancellationToken)));
					ApiResult<bool> result = apiResult;
					if (!result.Succeeded)
					{
						Logger.Warn("Failed to publish SPARK account {action}. Status: {status}.", new object[2]
						{
							isBlocked ? "block" : "unblock",
							result.StatusCode
						});
					}
				}
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to publish SPARK account block change.");
			}
		}

		private async Task<string> GetVerificationTokenAsync(CancellationToken cancellationToken)
		{
			return (_tokens != null) ? (await _tokens.GetTokenAsync(cancellationToken)) : string.Empty;
		}

		private void BeginBlockSyncOperation()
		{
			Interlocked.Increment(ref _blockSyncOperations);
		}

		private void EndBlockSyncOperation()
		{
			Interlocked.Decrement(ref _blockSyncOperations);
		}

		private void NotifySavedChanged()
		{
			this.SavedProfilesChanged?.Invoke();
		}

		private void NotifyBlockedAccountsChanged()
		{
			this.BlockedAccountsChanged?.Invoke();
		}

		private List<string> LocalBlocks()
		{
			return _settings.GetBlockedAccountNames().ToList();
		}

		private bool IsBlocklistSynced(string blocklistKey)
		{
			lock (_blockSyncLock)
			{
				return _blocklistSynced && string.Equals(_lastSyncedBlocklistKey, blocklistKey ?? string.Empty, StringComparison.Ordinal);
			}
		}

		private void MarkBlocklistSynced(string blocklistKey)
		{
			lock (_blockSyncLock)
			{
				_blocklistSynced = true;
				_lastSyncedBlocklistKey = blocklistKey ?? string.Empty;
			}
		}

		private static string GetBlocklistKey(IEnumerable<string> accountNames)
		{
			return string.Join("\n", from account in (from account in accountNames ?? Enumerable.Empty<string>()
					select account?.Trim() ?? string.Empty into account
					where account.Length > 0
					select account).OrderBy((string account) => account, StringComparer.OrdinalIgnoreCase)
				select account.ToLowerInvariant());
		}

		private static string BlockedMessage(string subject, bool includeRemovalHelp = false)
		{
			string message = TextUtil.FirstNonEmpty(subject, "Account") + " blocked.";
			if (!includeRemovalHelp)
			{
				return message;
			}
			return message + " You can remove blocks using the 'Manage Blocks' in the settings menu.";
		}

		private static string GetProfileBlockSubject(CharacterProfile profile, PlayerPresence presence, string accountName)
		{
			return TextUtil.FirstNonEmpty(presence?.DisplayCharacterName, profile?.DisplayName, presence?.OfficialCharacterName, profile?.CharacterName, accountName);
		}

		private static bool CanReportProfile(CharacterProfile profile, PlayerPresence presence)
		{
			if (!string.IsNullOrWhiteSpace(TextUtil.FirstNonEmpty(presence?.AccountName, profile?.AccountName)) && !string.IsNullOrWhiteSpace(TextUtil.FirstNonEmpty(presence?.OfficialCharacterName, profile?.CharacterName)))
			{
				return !string.IsNullOrWhiteSpace(TextUtil.FirstNonEmpty(presence?.ActiveProfileId, profile?.ProfileId));
			}
			return false;
		}

		private static bool CanSaveToRecent(CharacterProfile profile, PlayerPresence presence)
		{
			if (profile == null)
			{
				return false;
			}
			if (string.Equals(profile.ProfileName?.Trim(), "No Active Profile", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if ((presence == null || !presence.HasActiveProfile) && string.IsNullOrWhiteSpace(presence?.ActiveProfileId))
			{
				if (!string.IsNullOrWhiteSpace(profile.ProfileId) && !string.IsNullOrWhiteSpace(profile.AccountName))
				{
					return !string.IsNullOrWhiteSpace(profile.CharacterName);
				}
				return false;
			}
			return true;
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (_serverSync != null)
				{
					_serverSync.StatusChanged -= HandleServerSyncStatusChanged;
				}
				CancellationTokenSource blockSyncCancellation = _blockSyncCancellation;
				_blockSyncCancellation = null;
				_blockSyncWorker = null;
				blockSyncCancellation?.Cancel();
			}
		}
	}
}
