using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;
using rp.spark.UI;
using rp.spark.UI.Views;

namespace rp.spark
{
	[Export(typeof(Module))]
	public class SparkModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<SparkModule>();

		private static readonly TimeSpan GameplayVisibilityCheckInterval = TimeSpan.FromMilliseconds(250.0);

		private static readonly TimeSpan GameplayVisibilityWarningInterval = TimeSpan.FromSeconds(5.0);

		private static readonly TimeSpan StatusRefreshInterval = TimeSpan.FromSeconds(15.0);

		private TimeSpan _gameplayVisibilityCheckElapsed;

		private DateTime _lastGameplayVisibilityWarningAt = DateTime.MinValue;

		private int _closeGameplayWindowsQueued;

		private ProfileValidator _profileValidator;

		private ProfileRepository _profileRepository;

		private GlobalOocInfoStore _globalOocInfo;

		private ProfileCache _profileCache;

		private ProfileNotes _notes;

		private PlayerStateService _playerState;

		private PresenceService _presenceService;

		private NearbyPresenceService _nearbyPresenceService;

		private RollGroupService _rollGroups;

		private PresenceLoop _presenceLoop;

		private ServerSync _sync;

		private GW2TokenVerification _tokens;

		private SparkClient _sparkClient;

		private IconIndexService _iconIndex;

		private SparkSettings _sparkSettings;

		private ProfileActions _profileActions;

		private ProfileLoader _profileLoader;

		private ServiceHost _serviceHost;

		private SparkWindows _windows;

		private SparkCornerIcon _cornerIcon;

		private Task<PlayerState> _initialStateTask;

		private CancellationTokenSource _stateLoadCancel;

		private DateTime _nextStatusRefreshAt = DateTime.MinValue;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public SparkModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_sparkSettings = new SparkSettings(settings);
		}

		protected override void Initialize()
		{
			_profileValidator = new ProfileValidator();
			_profileRepository = new ProfileRepository(DirectoriesManager, _profileValidator);
			_profileRepository.ProfileSaved += ProfileSaved;
			_profileRepository.ActiveProfileChanged += ActiveProfileChanged;
			_globalOocInfo = new GlobalOocInfoStore(DirectoriesManager);
			_globalOocInfo.GlobalOocInfoChanged += GlobalOocInfoChanged;
			_profileCache = new ProfileCache(DirectoriesManager, _profileValidator);
			_notes = new ProfileNotes(DirectoriesManager);
			_playerState = new PlayerStateService(Gw2ApiManager, _sparkSettings);
			_presenceService = new PresenceService(_profileRepository, _playerState, _sparkSettings, _globalOocInfo);
			_sparkClient = new SparkClient(_sparkSettings.GetServerBaseUrl());
			_iconIndex = new IconIndexService(ContentsManager);
			_presenceLoop = new PresenceLoop(_presenceService);
			_tokens = new GW2TokenVerification(Gw2ApiManager);
			_nearbyPresenceService = new NearbyPresenceService(_sparkClient, _sparkSettings, _presenceLoop, _tokens);
			_rollGroups = new RollGroupService(_sparkClient, _playerState, _profileRepository, _tokens);
			_sync = new ServerSync(_sparkClient, _sparkSettings, _presenceLoop, _profileRepository, _tokens);
			_profileActions = new ProfileActions(_profileCache, _sparkSettings, _playerState, _sparkClient, _tokens, _sync);
			_sync.SetPrivacyCheck(_profileActions.EnsureBlocksSyncedAsync);
			_profileLoader = new ProfileLoader(_profileRepository, _playerState, _presenceService, _presenceLoop, _sync, _sparkSettings, _profileActions);
			_serviceHost = new ServiceHost();
			_serviceHost.Add(_presenceLoop, delegate(PresenceLoop service)
			{
				service.Start();
			});
			_serviceHost.Add(_tokens);
			_serviceHost.Add(_profileActions, delegate(ProfileActions service)
			{
				service.Start();
			});
			_serviceHost.Add(_sync, delegate(ServerSync service)
			{
				service.Start();
			});
			_serviceHost.Add(_nearbyPresenceService, delegate(NearbyPresenceService service)
			{
				service.Start();
			});
			_serviceHost.Add(_rollGroups, delegate(RollGroupService service)
			{
				service.Start();
			});
			_windows = new SparkWindows(new WindowBuilder(), _profileRepository, _profileCache, _notes, _playerState, _globalOocInfo, _iconIndex, _sparkSettings, _profileLoader, _profileActions, _nearbyPresenceService, _rollGroups, RefreshPresenceSoon, SetNearbySharing);
			_cornerIcon = new SparkCornerIcon(_sparkSettings, ContentsManager, _windows.OpenMyProfile, _windows.OpenProfileManager, _windows.OpenChatSplitter, _windows.OpenOnlineList, _windows.OpenNearby, _windows.OpenRollGroup, _windows.OpenSavedProfiles, _windows.OpenBlocklist, _windows.OpenSettings, _windows.OpenAbout, RefreshPresenceSoon, SetNearbySharing, GetServerSyncStatus, GetImportantSettingsNotice, WatchServerSyncStatus, UnwatchServerSyncStatus);
		}

		protected override Task LoadAsync()
		{
			return Task.CompletedTask;
		}

		protected override void Update(GameTime gameTime)
		{
			if (_windows == null || gameTime == null)
			{
				return;
			}
			_gameplayVisibilityCheckElapsed += gameTime.get_ElapsedGameTime();
			if (_gameplayVisibilityCheckElapsed < GameplayVisibilityCheckInterval)
			{
				return;
			}
			_gameplayVisibilityCheckElapsed = TimeSpan.Zero;
			try
			{
				if (_windows.ShouldHideGameplayWindows())
				{
					_windows.CloseGameplayWindows();
				}
			}
			catch (Exception ex)
			{
				WarnGameplayVisibilityFailure(ex);
			}
		}

		private void WarnGameplayVisibilityFailure(Exception ex)
		{
			DateTime now = DateTime.UtcNow;
			if (!(now - _lastGameplayVisibilityWarningAt < GameplayVisibilityWarningInterval))
			{
				_lastGameplayVisibilityWarningAt = now;
				Logger.Warn(ex, "SPARK failed while updating gameplay window visibility.");
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)HandleSubtokenUpdated);
			GameService.Gw2Mumble.add_IsAvailableChanged((EventHandler<ValueEventArgs<bool>>)HandleMumbleAvailableChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)HandleIsInGameChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)HandleMapOpenChanged);
			((GameService)GameService.Gw2Mumble).add_FinishedLoading((EventHandler<EventArgs>)HandlePlayerStateChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)HandlePlayerStateChanged);
			EnsurePlayerStateLoad();
			_serviceHost.Start();
			_cornerIcon?.Refresh();
		}

		public override IView GetSettingsView()
		{
			EnsurePlayerStateLoad();
			return (IView)(object)new SparkSettingsView(_cornerIcon.ShowMenu);
		}

		private void GlobalOocInfoChanged(string accountName)
		{
			_sync?.InvalidateProfileUpload();
			try
			{
				PlayerState state = _playerState?.GetCached();
				if (state != null && !string.IsNullOrWhiteSpace(state.AccountName) && string.Equals(state.AccountName.Trim(), accountName?.Trim(), StringComparison.OrdinalIgnoreCase))
				{
					CharacterProfile activeProfile = _profileRepository?.LoadActiveForCharacter(state.AccountName, state.OfficialCharacterName);
					if (activeProfile != null && activeProfile.UseGlobalOutOfCharacterInfo)
					{
						_profileRepository.Save(activeProfile);
						return;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to update the active profile after global OOC info changed.");
			}
			RefreshPresenceSoon();
		}

		private void ProfileSaved(CharacterProfile savedProfile)
		{
			RefreshPresenceSoon();
			SparkUiThread.Queue(delegate
			{
				if (_windows != null && _windows.IsProfileViewerVisible && _windows.IsViewingProfile(savedProfile))
				{
					PlayerState cached = _playerState.GetCached();
					_windows.ShowProfileViewer(_profileLoader.BuildLocal(savedProfile, cached));
				}
			});
		}

		private void ActiveProfileChanged(string accountName, string officialCharacterName, string profileId)
		{
			RefreshRollGroupSoon();
			RefreshPresenceSoon();
			SparkUiThread.Queue(delegate
			{
				if (_windows != null && _windows.IsProfileViewerVisible && _windows.IsViewingCharacter(officialCharacterName))
				{
					PlayerState cached = _playerState.GetCached();
					CharacterProfile profile = (string.IsNullOrWhiteSpace(profileId) ? null : _profileRepository.Load(profileId));
					_windows.ShowProfileViewer(_profileLoader.BuildLocal(profile, cached));
				}
			});
		}

		private void EnsurePlayerStateLoad()
		{
			if (_initialStateTask == null || _initialStateTask.IsFaulted || _initialStateTask.IsCanceled || LoadedNoCharacter(_initialStateTask))
			{
				LoadPlayerState();
			}
		}

		private void ReloadPlayerState()
		{
			LoadPlayerState();
		}

		private async void RefreshPresenceSoon()
		{
			if (_presenceLoop != null)
			{
				try
				{
					await _presenceLoop.RefreshAsync();
					SyncSoon();
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to refresh SPARK data after a profile change.");
				}
			}
		}

		private async void RefreshRollGroupSoon()
		{
			if (_rollGroups != null)
			{
				try
				{
					await _rollGroups.RefreshAsync();
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to refresh the roll group after a profile change.");
				}
			}
		}

		private async void SetNearbySharing(bool enabled)
		{
			_sparkSettings.ShowNearbyPresence.set_Value(enabled);
			try
			{
				if (_nearbyPresenceService != null)
				{
					if (!enabled)
					{
						await _nearbyPresenceService.RemoveAsync();
					}
					else
					{
						await _nearbyPresenceService.PublishNowAsync();
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to update nearby sharing from the SPARK menu.");
			}
		}

		private void LoadPlayerState()
		{
			if (_playerState != null)
			{
				CancelPlayerStateLoad();
				_stateLoadCancel = new CancellationTokenSource();
				_initialStateTask = _playerState.GetCurrentAsync(_stateLoadCancel.Token);
			}
		}

		private void CancelPlayerStateLoad()
		{
			CancellationTokenSource cancellation = _stateLoadCancel;
			Task<PlayerState> stateTask = _initialStateTask;
			_stateLoadCancel = null;
			if (cancellation == null)
			{
				return;
			}
			try
			{
				cancellation.Cancel();
			}
			catch (ObjectDisposedException)
			{
			}
			finally
			{
				if (stateTask == null)
				{
					cancellation.Dispose();
				}
				else
				{
					TaskCleanup.DisposeWhenComplete(stateTask, cancellation);
				}
			}
		}

		private async void HandleSubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			_ = 1;
			try
			{
				_tokens?.Clear();
				_profileActions?.SyncBlocks();
				ReloadPlayerState();
				Task<PlayerState> stateTask = _initialStateTask;
				if (stateTask != null)
				{
					await stateTask;
				}
				if (_rollGroups != null)
				{
					await _rollGroups.RefreshAsync();
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to refresh SPARK player state after API subtoken update.");
			}
		}

		private void HandleMumbleAvailableChanged(object sender, ValueEventArgs<bool> e)
		{
			_cornerIcon?.RefreshForGameState();
			CloseGameplayWindowsIfUnavailableSoon();
			ReloadPlayerState();
		}

		private void HandleIsInGameChanged(object sender, ValueEventArgs<bool> e)
		{
			_cornerIcon?.RefreshForGameState();
			CloseGameplayWindowsIfUnavailableSoon();
			ReloadPlayerState();
		}

		private void HandleMapOpenChanged(object sender, ValueEventArgs<bool> e)
		{
			_cornerIcon?.RefreshForGameState();
			CloseGameplayWindowsIfUnavailableSoon();
		}

		private void CloseGameplayWindowsIfUnavailableSoon()
		{
			if (Interlocked.Exchange(ref _closeGameplayWindowsQueued, 1) == 1)
			{
				return;
			}
			SparkUiThread.Queue(delegate
			{
				try
				{
					if (_windows?.ShouldHideGameplayWindows() ?? false)
					{
						_windows.CloseGameplayWindows();
					}
				}
				catch (Exception ex)
				{
					WarnGameplayVisibilityFailure(ex);
				}
				finally
				{
					Interlocked.Exchange(ref _closeGameplayWindowsQueued, 0);
				}
			});
		}

		private static bool LoadedNoCharacter(Task<PlayerState> task)
		{
			if (task != null && task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
			{
				return !task.Result.CanEditProfile;
			}
			return false;
		}

		private async Task<string> WaitForPlayerStateAsync()
		{
			EnsurePlayerStateLoad();
			Task<PlayerState> stateTask = _initialStateTask;
			if (stateTask == null)
			{
				return "Profile tools unavailable";
			}
			try
			{
				return GetUnavailableReason(await stateTask);
			}
			catch (OperationCanceledException)
			{
				return "Loading character info...";
			}
			catch
			{
				return "Profile tools unavailable";
			}
		}

		private string GetPlayerStateMessage()
		{
			return GetUnavailableReason(_playerState.GetCached());
		}

		private ServerSyncStatus GetServerSyncStatus()
		{
			ServerSyncStatus setupStatus = GetProfileSetupStatus();
			if (setupStatus != null)
			{
				return setupStatus;
			}
			return _sync?.CurrentStatus ?? ServerSyncStatus.Disconnected("Server sync is not connected.");
		}

		private ServerSyncStatus GetProfileSetupStatus()
		{
			if (_playerState == null || _profileRepository == null)
			{
				return null;
			}
			PlayerState state = _playerState.GetCached();
			if (state == null || !state.CanEditProfile)
			{
				return null;
			}
			if (_profileRepository.ListForCharacter(state.AccountName, state.OfficialCharacterName).Count == 0)
			{
				return new ServerSyncStatus(ServerSyncState.Info, "Click 'Open Profile Editor' and make your first profile to begin!");
			}
			if (_profileRepository.LoadActiveForCharacter(state.AccountName, state.OfficialCharacterName) == null)
			{
				return new ServerSyncStatus(ServerSyncState.Info, "No active profile. Open Profile Editor, pick a profile, and click 'Set Active'.");
			}
			return null;
		}

		private void WatchServerSyncStatus(Action<ServerSyncStatus> handler)
		{
			if (_sync != null && handler != null)
			{
				_sync.StatusChanged += handler;
			}
		}

		private void UnwatchServerSyncStatus(Action<ServerSyncStatus> handler)
		{
			if (_sync != null && handler != null)
			{
				_sync.StatusChanged -= handler;
			}
		}

		private void SyncSoon()
		{
			_sync?.SyncSoon();
		}

		private bool HasValidApiKey()
		{
			try
			{
				PlayerState state = _playerState?.GetCached();
				if (state == null || !state.CanEditProfile)
				{
					return true;
				}
				return _tokens?.HasValidApiKey() ?? false;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to check SPARK GW2 API key status.");
				return false;
			}
		}

		private string GetImportantSettingsNotice()
		{
			if (IsGameplayUiBlockingSpark())
			{
				return "SPARK windows closed due to map, vista, or other game UI.";
			}
			PlayerState state = _playerState?.GetCached();
			string unavailableReason = GetUnavailableReason(state);
			if (!string.IsNullOrWhiteSpace(unavailableReason))
			{
				return unavailableReason;
			}
			return GetApiStatus(state);
		}

		private bool IsGameplayUiBlockingSpark()
		{
			SparkWindows windows = _windows;
			if (windows == null || !windows.ShouldHideGameplayWindows())
			{
				return false;
			}
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				return true;
			}
			PlayerState state = _playerState?.GetCached();
			if (state != null && state.IsMumbleAvailable)
			{
				return !string.IsNullOrWhiteSpace(state.OfficialCharacterName);
			}
			return false;
		}

		private static string GetUnavailableReason(PlayerState state)
		{
			if (state == null)
			{
				return "Load into the game on a character to use SPARK.";
			}
			if (state.CanEditProfile)
			{
				return string.Empty;
			}
			if (!SparkWindows.IsLoadingScreen())
			{
				return "Load into the game on a character to use SPARK.";
			}
			return "SPARK profile tools are unavailable during loading screens or character select.";
		}

		private string GetApiStatus(PlayerState state)
		{
			Task<PlayerState> stateTask = _initialStateTask;
			if (stateTask != null && !stateTask.IsCompleted)
			{
				if (!(_profileActions?.IsBlockSyncInProgress ?? false))
				{
					return "Checking GW2 API account and character permissions...";
				}
				return "Checking GW2 API access...";
			}
			if (!HasValidApiKey())
			{
				return "Waiting for GW2 API access from Blish HUD. Add an API key with account and characters permissions.";
			}
			RefreshStateIfStatusBlocked(state);
			if (state == null || !state.HasCharactersPermission)
			{
				return "Refreshing GW2 API permissions...";
			}
			if (string.IsNullOrWhiteSpace(state.AccountName))
			{
				return "Waiting for GW2 account verification...";
			}
			if (!state.IsCharacterApiVerified)
			{
				return "Waiting for current character verification from the GW2 API...";
			}
			if (_profileActions?.IsBlockSyncInProgress ?? false)
			{
				return "Syncing SPARK settings...";
			}
			return string.Empty;
		}

		private void HandlePlayerStateChanged(object sender, EventArgs e)
		{
			_cornerIcon?.RefreshForGameState();
			ReloadPlayerState();
		}

		private void RefreshStateIfStatusBlocked(PlayerState state)
		{
			Task<PlayerState> stateTask = _initialStateTask;
			if ((stateTask == null || stateTask.IsCompleted) && state != null && state.CanEditProfile && (!state.HasCharactersPermission || string.IsNullOrWhiteSpace(state.AccountName) || !state.IsCharacterApiVerified))
			{
				DateTime now = DateTime.UtcNow;
				if (!(now < _nextStatusRefreshAt))
				{
					_nextStatusRefreshAt = now + StatusRefreshInterval;
					LoadPlayerState();
				}
			}
		}

		protected override void Unload()
		{
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)HandleSubtokenUpdated);
			GameService.Gw2Mumble.remove_IsAvailableChanged((EventHandler<ValueEventArgs<bool>>)HandleMumbleAvailableChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)HandleIsInGameChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)HandleMapOpenChanged);
			((GameService)GameService.Gw2Mumble).remove_FinishedLoading((EventHandler<EventArgs>)HandlePlayerStateChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)HandlePlayerStateChanged);
			CancelPlayerStateLoad();
			_initialStateTask = null;
			_cornerIcon?.Dispose();
			_cornerIcon = null;
			_windows?.Dispose();
			_windows = null;
			_serviceHost?.Dispose();
			_serviceHost = null;
			_iconIndex?.Dispose();
			if (_profileRepository != null)
			{
				_profileRepository.ProfileSaved -= ProfileSaved;
				_profileRepository.ActiveProfileChanged -= ActiveProfileChanged;
			}
			if (_globalOocInfo != null)
			{
				_globalOocInfo.GlobalOocInfoChanged -= GlobalOocInfoChanged;
			}
			_profileLoader = null;
			_profileActions = null;
			_sync = null;
			_iconIndex = null;
			_presenceLoop = null;
			_sparkClient = null;
			_presenceService = null;
			_tokens = null;
			_playerState = null;
			_sparkSettings = null;
			_notes = null;
			_profileCache = null;
			_profileRepository = null;
			_globalOocInfo = null;
			_profileValidator = null;
		}
	}
}
