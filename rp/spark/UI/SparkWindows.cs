using System;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;
using rp.spark.UI.Views;

namespace rp.spark.UI
{
	internal sealed class SparkWindows : IDisposable
	{
		private const int ProfileSelectionIcon = 156727;

		private const int EditProfileIcon = 156714;

		private const int ProfilePrefsIcon = 155052;

		private const int CurrentInfoIcon = 440023;

		private const int RecentlySeenIcon = 156680;

		private const int BookmarkIcon = 156722;

		private static readonly Point NearbyWindowSize = new Point(640, 350);

		private static readonly Point NearbyWindowDefaultLocation = new Point(40, 240);

		private readonly WindowBuilder _windowBuilder;

		private readonly ProfileRepository _profileRepository;

		private readonly ProfileCache _profileCache;

		private readonly ProfileNotes _notes;

		private readonly PlayerStateService _playerState;

		private readonly IconIndexService _iconIndexService;

		private readonly SparkSettings _settings;

		private readonly ProfileLoader _profileLoader;

		private readonly ProfileActions _profileActions;

		private readonly NearbyPresenceService _nearbyPresenceService;

		private readonly Action _requestServerSync;

		private readonly Action<bool> _setNearbySharing;

		private TabbedWindow2 _settingsWindow;

		private ProfileEditorSession _profileEditorSession;

		private TabbedWindow2 _profileWindow;

		private TabbedWindow2 _savedProfilesWindow;

		private TabbedWindow2 _profileViewerWindow;

		private ProfileViewerView _profileViewerView;

		private ProfileNotesView _profileNotesView;

		private StandardWindow _onlineListWindow;

		private SparkCompactWindow _nearbyWindow;

		private StandardWindow _aboutWindow;

		private StandardWindow _blocklistWindow;

		private CharacterProfile _viewedProfile;

		private PlayerPresence _viewedPresence;

		private static readonly TimeSpan LoadingScreenTickDelay = TimeSpan.FromSeconds(1.5);

		private static readonly Logger Logger = Logger.GetLogger<SparkWindows>();

		private bool _isDisposed;

		public string ViewedProfileId { get; private set; }

		public string ViewedOfficialCharacterName { get; private set; }

		public bool IsProfileViewerVisible
		{
			get
			{
				if (_profileViewerWindow != null)
				{
					return ((Control)_profileViewerWindow).get_Visible();
				}
				return false;
			}
		}

		public SparkWindows(WindowBuilder windowBuilder, ProfileRepository profileRepository, ProfileCache profileCache, ProfileNotes notes, PlayerStateService playerState, IconIndexService iconIndexService, SparkSettings settings, ProfileLoader profileLoader, ProfileActions profileActions, NearbyPresenceService nearbyPresenceService, Action requestServerSync, Action<bool> setNearbySharing)
		{
			_windowBuilder = windowBuilder;
			_profileRepository = profileRepository;
			_profileCache = profileCache;
			_notes = notes;
			_playerState = playerState;
			_iconIndexService = iconIndexService;
			_settings = settings;
			_profileLoader = profileLoader;
			_profileActions = profileActions;
			_nearbyPresenceService = nearbyPresenceService;
			_requestServerSync = requestServerSync;
			_setNearbySharing = setNearbySharing;
		}

		internal static bool IsLoadingScreen()
		{
			if (GameService.Gw2Mumble.get_IsAvailable())
			{
				return GameService.Gw2Mumble.get_TimeSinceTick() >= LoadingScreenTickDelay;
			}
			return false;
		}

		internal bool ShouldHideGameplayWindows()
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !IsLoadingScreen())
			{
				return ShouldHideForGameUi();
			}
			return true;
		}

		private bool ShouldHideForGameUi()
		{
			if (_settings?.AutoHideGameUi.get_Value() ?? true)
			{
				return GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return false;
		}

		private bool CanShowGameplayWindow()
		{
			return !ShouldHideGameplayWindows();
		}

		public void OpenProfileManager()
		{
			if (CanShowGameplayWindow())
			{
				PlayerState state = _playerState.GetCached();
				if (_profileWindow != null && ((Control)_profileWindow).get_Visible())
				{
					((WindowBase2)_profileWindow).BringWindowToFront();
					return;
				}
				_profileEditorSession = new ProfileEditorSession(_profileRepository, _playerState, state);
				CreateProfileWindow();
				((Control)_profileWindow).Show();
			}
		}

		public void OpenMyProfile()
		{
			if (CanShowGameplayWindow())
			{
				ShowProfileViewer(_profileLoader.LoadMyProfile());
			}
		}

		public void OpenOnlineList()
		{
			if (CanShowGameplayWindow())
			{
				if (_onlineListWindow == null)
				{
					CreateOnlineListWindow();
				}
				_onlineListWindow.Show((IView)(object)new OnlineProfilesView((CancellationToken cancellationToken) => _profileLoader.LoadOnlineAsync(cancellationToken), _profileLoader.LoadCachedOnlineRows, OpenPresence, _profileActions.IsPresenceBookmarked, _profileActions.WatchSavedProfiles, _profileActions.UnwatchSavedProfiles, () => _settings.AutoRefreshOnlineProfiles.get_Value(), delegate(bool value)
				{
					_settings.AutoRefreshOnlineProfiles.set_Value(value);
				}));
			}
		}

		public void OpenNearby()
		{
			if (CanShowGameplayWindow())
			{
				if (_nearbyWindow == null)
				{
					CreateNearbyWindow();
				}
				_nearbyWindow.Show((IView)(object)new NearbyView(_nearbyPresenceService, _settings, OpenPresence, delegate(bool locked)
				{
					_nearbyWindow.DraggingLocked = locked;
				}));
			}
		}

		public void OpenSavedProfiles()
		{
			if (!CanShowGameplayWindow())
			{
				return;
			}
			if (_savedProfilesWindow != null && ((Control)_savedProfilesWindow).get_Visible())
			{
				((WindowBase2)_savedProfilesWindow).BringWindowToFront();
				return;
			}
			if (_savedProfilesWindow == null)
			{
				CreateSavedWindow();
			}
			((Control)_savedProfilesWindow).Show();
		}

		public void OpenAbout()
		{
			if (_aboutWindow != null && ((Control)_aboutWindow).get_Visible())
			{
				((WindowBase2)_aboutWindow).BringWindowToFront();
				return;
			}
			if (_aboutWindow == null)
			{
				CreateAboutWindow();
			}
			_aboutWindow.Show((IView)(object)new SparkAboutView());
		}

		public void OpenBlocklist()
		{
			if (_blocklistWindow != null && ((Control)_blocklistWindow).get_Visible())
			{
				((WindowBase2)_blocklistWindow).BringWindowToFront();
				return;
			}
			if (_blocklistWindow == null)
			{
				CreateBlocklistWindow();
			}
			_blocklistWindow.Show((IView)(object)new SparkBlocklistView(_settings, _profileActions.BlockAccount, _profileActions.UnblockAccount, _profileActions.WatchBlockedAccounts, _profileActions.UnwatchBlockedAccounts));
		}

		public void OpenSettings()
		{
			if (_settingsWindow != null && ((Control)_settingsWindow).get_Visible())
			{
				((WindowBase2)_settingsWindow).BringWindowToFront();
				return;
			}
			if (_settingsWindow == null)
			{
				CreateSettingsWindow();
			}
			((Control)_settingsWindow).Show();
		}

		public void ShowProfileViewer(ProfileViewData viewData)
		{
			if (viewData != null)
			{
				ShowProfileViewer(viewData.Profile, viewData.Presence);
			}
		}

		public void ShowProfileViewer(CharacterProfile profile, PlayerPresence presence)
		{
			if (CanShowGameplayWindow())
			{
				if (profile == null)
				{
					profile = new CharacterProfile();
				}
				if (presence == null)
				{
					presence = new PlayerPresence();
				}
				if (_profileViewerWindow == null)
				{
					CreateViewerWindow();
				}
				_viewedProfile = profile;
				_viewedPresence = presence;
				ViewedProfileId = profile.ProfileId;
				ViewedOfficialCharacterName = profile.CharacterName;
				_profileActions.SaveToRecent(profile, presence);
				_profileViewerView?.SetProfile(profile, presence);
				_profileNotesView?.SetProfile(profile, presence);
				((Control)_profileViewerWindow).Show();
			}
		}

		public bool IsViewingProfile(CharacterProfile profile)
		{
			if (profile == null)
			{
				return false;
			}
			if (!string.IsNullOrWhiteSpace(ViewedProfileId))
			{
				return string.Equals(ViewedProfileId, profile.ProfileId, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		public bool IsViewingCharacter(string officialCharacterName)
		{
			if (!string.IsNullOrWhiteSpace(ViewedOfficialCharacterName))
			{
				return string.Equals(ViewedOfficialCharacterName.Trim(), officialCharacterName?.Trim(), StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		private void CreateProfileWindow()
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Expected O, but got Unknown
			_windowBuilder.DisposeWindow((WindowBase2)(object)_profileWindow);
			_profileWindow = _windowBuilder.MakeTabbedWindow("Profile Editor", "rp.spark.profile-window");
			_profileWindow.get_Tabs().Add(new Tab(_windowBuilder.IconFromAsset(156727), (Func<IView>)(() => (IView)(object)new ProfileManagementView(_profileEditorSession)), "Profile Selection", (int?)100));
			_profileWindow.get_Tabs().Add(new Tab(_windowBuilder.IconFromAsset(156714), (Func<IView>)(() => (IView)(object)new ProfileEditorView(_profileEditorSession, _iconIndexService)), "Edit Profile", (int?)110));
			_profileWindow.get_Tabs().Add(new Tab(_windowBuilder.IconFromAsset(155052), (Func<IView>)(() => (IView)(object)new ProfileEditorPreferencesView(_profileEditorSession)), "Preferences", (int?)120));
			_profileWindow.get_Tabs().Add(new Tab(_windowBuilder.IconFromAsset(440023), (Func<IView>)(() => (IView)(object)new ProfileEditorCurrentInfoView(_profileEditorSession)), "Current Info", (int?)130));
		}

		private void CreateViewerWindow()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			_profileViewerWindow = _windowBuilder.MakeTabbedWindow("Profile Viewer", "rp.spark.profile-viewer-window");
			_profileViewerWindow.get_Tabs().Add(new Tab(_windowBuilder.IconFromAsset(156714), (Func<IView>)delegate
			{
				_profileViewerView = new ProfileViewerView(_viewedProfile, _viewedPresence, _profileActions.ToggleProfileBookmark, _profileActions.IsProfileBookmarked, _profileActions.ToggleProfileBlock, _profileActions.IsProfileBlocked, _profileActions.ReportProfile);
				return (IView)(object)_profileViewerView;
			}, "View Profile", (int?)100));
			_profileViewerWindow.get_Tabs().Add(new Tab(_windowBuilder.IconFromAsset(156727), (Func<IView>)delegate
			{
				_profileNotesView = new ProfileNotesView(_notes, _viewedProfile, _viewedPresence);
				return (IView)(object)_profileNotesView;
			}, "Notes", (int?)110));
		}

		private void CreateOnlineListWindow()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_onlineListWindow = _windowBuilder.MakeWindow("Online Profiles", "rp.spark.online-list-window", new Rectangle(96, 22, 783, 654));
		}

		private void CreateNearbyWindow()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			_nearbyWindow = _windowBuilder.MakeCompactWindow("Nearby Players", NearbyWindowSize);
			((Control)_nearbyWindow).set_Location(ClampToScreen(_settings.GetNearbyWindowLocation(NearbyWindowDefaultLocation), NearbyWindowSize));
			_nearbyWindow.LocationSaved += delegate(Point location)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				_settings.SetNearbyWindowLocation(ClampToScreen(location, NearbyWindowSize));
			};
			_nearbyWindow.DraggingLocked = _settings.NearbyWindowLock.get_Value();
		}

		private static Point ClampToScreen(Point location, Point size)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			Point screenSize = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			int maxX = Math.Max(0, screenSize.X - size.X);
			int maxY = Math.Max(0, screenSize.Y - size.Y);
			return new Point(Clamp(location.X, 0, maxX), Clamp(location.Y, 0, maxY));
		}

		private static int Clamp(int value, int min, int max)
		{
			if (value < min)
			{
				return min;
			}
			if (value <= max)
			{
				return value;
			}
			return max;
		}

		private void CreateSavedWindow()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			_savedProfilesWindow = _windowBuilder.MakeTabbedWindow("Saved Profiles", "rp.spark.saved-profiles-window");
			_savedProfilesWindow.get_Tabs().Add(new Tab(_windowBuilder.IconFromAsset(156680), (Func<IView>)(() => (IView)(object)new SavedProfilesView(_profileCache.ListRecent, OpenSavedProfile, SavedProfilesMode.Recent, _settings.IsBlockedAccount, null, _profileActions.WatchSavedProfiles, _profileActions.UnwatchSavedProfiles, () => _settings.ShowMatureProfiles.get_Value())), "Recent", (int?)100));
			_savedProfilesWindow.get_Tabs().Add(new Tab(_windowBuilder.IconFromAsset(156722), (Func<IView>)(() => (IView)(object)new SavedProfilesView(_profileCache.ListBookmarked, OpenSavedProfile, SavedProfilesMode.Bookmarks, _settings.IsBlockedAccount, _profileActions.RemoveBookmark, _profileActions.WatchSavedProfiles, _profileActions.UnwatchSavedProfiles, () => _settings.ShowMatureProfiles.get_Value())), "Bookmarks", (int?)110));
		}

		private async void OpenPresence(PlayerPresence presence)
		{
			try
			{
				ProfileViewData viewData = await _profileLoader.LoadOnlineProfileAsync(presence);
				if (_isDisposed)
				{
					return;
				}
				SparkUiThread.Queue(delegate
				{
					if (!_isDisposed)
					{
						ShowProfileViewer(viewData);
					}
				});
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to open SPARK profile");
			}
		}

		private async void OpenSavedProfile(SavedProfileSummary summary, Action<string> showStatus)
		{
			if (summary == null || (summary.IsMature && !_settings.ShowMatureProfiles.get_Value()))
			{
				return;
			}
			try
			{
				SavedProfile record = _profileCache.Load(summary.CacheKey);
				if (record == null)
				{
					_profileActions.RemoveSavedProfile(summary);
					string profileName = ProfileText.SavedCharacterName(summary);
					showStatus?.Invoke("Warning: " + profileName + " not found. Entry removed from list.");
					return;
				}
				ProfileViewData viewData = await _profileLoader.LoadSavedProfileAsync(record);
				if (viewData == null)
				{
					return;
				}
				SparkUiThread.Queue(delegate
				{
					if (!_isDisposed)
					{
						ShowProfileViewer(viewData);
					}
				});
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to open saved SPARK profile");
				showStatus?.Invoke("Couldn't open this saved profile.");
			}
		}

		private void CreateAboutWindow()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_aboutWindow = _windowBuilder.MakeWindow("About", "rp.spark.about-window", new Rectangle(70, 22, 760, 654));
		}

		private void CreateBlocklistWindow()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_blocklistWindow = _windowBuilder.MakeWindow("Blocked Accounts", "rp.spark.blocklist-window", new Rectangle(70, 60, 760, 610));
		}

		private void CreateSettingsWindow()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			_settingsWindow = _windowBuilder.MakeTabbedWindow("Settings", "rp.spark.settings-window");
			_settingsWindow.get_Tabs().Add(new Tab(_windowBuilder.IconFromAsset(155052), (Func<IView>)(() => (IView)(object)new SparkOptionsView(_settings, _requestServerSync, _setNearbySharing, HandleMaturePreferenceChanged)), "General", (int?)100));
		}

		public void HandleMaturePreferenceChanged(bool enabled)
		{
			if (enabled)
			{
				return;
			}
			_windowBuilder.DisposeWindow((WindowBase2)(object)_onlineListWindow);
			_onlineListWindow = null;
			_windowBuilder.DisposeWindow((WindowBase2)(object)_savedProfilesWindow);
			_savedProfilesWindow = null;
			if ((_viewedProfile?.IsMature ?? false) || (_viewedPresence?.IsMature ?? false))
			{
				string currentAccountName = _playerState?.GetCached()?.AccountName ?? string.Empty;
				string viewedAccountName = TextUtil.FirstNonEmpty(_viewedPresence?.AccountName, _viewedProfile?.AccountName);
				if (string.IsNullOrWhiteSpace(currentAccountName) || !string.Equals(currentAccountName.Trim(), viewedAccountName?.Trim(), StringComparison.OrdinalIgnoreCase))
				{
					_windowBuilder.DisposeWindow((WindowBase2)(object)_profileViewerWindow);
					_profileViewerWindow = null;
					_profileViewerView = null;
					_profileNotesView = null;
					_viewedProfile = null;
					_viewedPresence = null;
					ViewedProfileId = null;
					ViewedOfficialCharacterName = null;
				}
			}
		}

		public void CloseGameplayWindows()
		{
			_windowBuilder.DisposeWindow((WindowBase2)(object)_profileWindow);
			_profileWindow = null;
			_profileEditorSession = null;
			_windowBuilder.DisposeWindow((WindowBase2)(object)_savedProfilesWindow);
			_savedProfilesWindow = null;
			_windowBuilder.DisposeWindow((WindowBase2)(object)_profileViewerWindow);
			_profileViewerWindow = null;
			_profileViewerView = null;
			_profileNotesView = null;
			_windowBuilder.DisposeWindow((WindowBase2)(object)_onlineListWindow);
			_onlineListWindow = null;
			SparkCompactWindow nearbyWindow = _nearbyWindow;
			if (nearbyWindow != null)
			{
				((Control)nearbyWindow).Dispose();
			}
			_nearbyWindow = null;
			_viewedProfile = null;
			_viewedPresence = null;
			ViewedProfileId = null;
			ViewedOfficialCharacterName = null;
		}

		public void Dispose()
		{
			_isDisposed = true;
			CloseGameplayWindows();
			_windowBuilder.DisposeWindow((WindowBase2)(object)_aboutWindow);
			_aboutWindow = null;
			_windowBuilder.DisposeWindow((WindowBase2)(object)_blocklistWindow);
			_blocklistWindow = null;
			_windowBuilder.DisposeWindow((WindowBase2)(object)_settingsWindow);
			_settingsWindow = null;
			_windowBuilder.Clear();
		}
	}
}
