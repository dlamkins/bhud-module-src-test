using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rp.spark.Models;
using rp.spark.Services;
using rp.spark.UI.Controls;
using rp.spark.UI.Views;

namespace rp.spark.UI
{
	internal sealed class SparkCornerIcon : IDisposable
	{
		private const int MinimumMenuWidth = 190;

		private const int MenuItemTextPadding = 72;

		private readonly SparkSettings _settings;

		private readonly AsyncTexture2D _icon;

		private readonly AsyncTexture2D _hoverIcon;

		private readonly bool _disposeHoverIcon;

		private readonly Action _openMyProfile;

		private readonly Action _openProfileManager;

		private readonly Action _openChatSplitter;

		private readonly Action _openOnlineList;

		private readonly Action _openNearby;

		private readonly Action _openRollGroup;

		private readonly Action _openSavedProfiles;

		private readonly Action _openBlocklist;

		private readonly Action _openSettings;

		private readonly Action _openAbout;

		private readonly Action _requestServerSync;

		private readonly Action<bool> _setNearbySharing;

		private readonly Func<ServerSyncStatus> _getServerSyncStatus;

		private readonly Action<Action<ServerSyncStatus>> _watchServerSyncStatus;

		private readonly Action<Action<ServerSyncStatus>> _unwatchServerSyncStatus;

		private readonly Func<string> _getImportantNotice;

		private CornerIcon _cornerIcon;

		private ContextMenuStrip _menu;

		private ContextMenuStripItem _myProfileItem;

		private ContextMenuStripItem _profileEditorItem;

		private ContextMenuStripItem _chatSplitterItem;

		private ContextMenuStripItem _onlineListItem;

		private ContextMenuStripItem _nearbyPlayersItem;

		private ContextMenuStripItem _rollGroupItem;

		private ContextMenuStripItem _savedProfilesItem;

		private ContextMenuStripItem _toolsMenuItem;

		private ContextMenuStripItem _statusMenuItem;

		private ContextMenuColours _readinessMenuItem;

		private ContextMenuColours _serverStatusMenuItem;

		private readonly Dictionary<RPStatus, ContextMenuStripItem> _statusMenuItems = new Dictionary<RPStatus, ContextMenuStripItem>();

		private bool _isSyncingStatusMenu;

		private bool _isDisposed;

		public SparkCornerIcon(SparkSettings settings, ContentsManager contentsManager, Action openMyProfile, Action openProfileManager, Action openChatSplitter, Action openOnlineList, Action openNearby, Action openRollGroup, Action openSavedProfiles, Action openBlocklist, Action openSettings, Action openAbout, Action requestServerSync, Action<bool> setNearbySharing, Func<ServerSyncStatus> getServerSyncStatus, Func<string> getImportantNotice, Action<Action<ServerSyncStatus>> watchServerSyncStatus, Action<Action<ServerSyncStatus>> unwatchServerSyncStatus)
		{
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			_settings = settings;
			_openMyProfile = openMyProfile;
			_openProfileManager = openProfileManager;
			_openChatSplitter = openChatSplitter;
			_openOnlineList = openOnlineList;
			_openNearby = openNearby;
			_openRollGroup = openRollGroup;
			_openSavedProfiles = openSavedProfiles;
			_openBlocklist = openBlocklist;
			_openSettings = openSettings;
			_openAbout = openAbout;
			_requestServerSync = requestServerSync;
			_setNearbySharing = setNearbySharing;
			_getServerSyncStatus = getServerSyncStatus;
			_getImportantNotice = getImportantNotice;
			_watchServerSyncStatus = watchServerSyncStatus;
			_unwatchServerSyncStatus = unwatchServerSyncStatus;
			_watchServerSyncStatus?.Invoke(OnServerStatusChanged);
			Texture2D iconTexture = contentsManager.GetTexture("spark-corner-icon.png");
			Texture2D hoverTexture = contentsManager.GetTexture("spark-corner-icon-hover.png", iconTexture);
			_icon = new AsyncTexture2D(iconTexture);
			if (iconTexture == hoverTexture)
			{
				_hoverIcon = _icon;
			}
			else
			{
				_hoverIcon = new AsyncTexture2D(hoverTexture);
				_disposeHoverIcon = true;
			}
			_settings.ShowCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowCornerIconChanged);
			_settings.CurrentStatus.add_SettingChanged((EventHandler<ValueChangedEventArgs<RPStatus>>)OnCurrentStatusChanged);
		}

		public void Refresh()
		{
			if (!_isDisposed)
			{
				if (_settings.ShowCornerIcon.get_Value())
				{
					EnsureCornerIconCreated();
				}
				else
				{
					ClearCornerIcon();
				}
			}
		}

		private void EnsureCornerIconCreated()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			if (_cornerIcon == null)
			{
				EnsureMenuCreated();
				CornerIcon val = new CornerIcon(_icon, _hoverIcon, "SPARK");
				val.set_Priority(0);
				_cornerIcon = val;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
			}
		}

		private void EnsureMenuCreated()
		{
			if (_menu == null)
			{
				_menu = BuildMenu();
			}
		}

		internal void ShowMenu(Control anchor)
		{
			if (!_isDisposed && anchor != null)
			{
				EnsureMenuCreated();
				RefreshMenuState();
				RefreshSparkStatusItems();
				ContextMenuStrip menu = _menu;
				if (menu != null)
				{
					menu.Show(anchor);
				}
			}
		}

		private ContextMenuStrip BuildMenu()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			ContextMenuStrip val = new ContextMenuStrip();
			((Control)val).set_Width(CalculateMenuWidth());
			ContextMenuStrip menu = val;
			AddStatusSubmenu(menu);
			_myProfileItem = AddMenuItem(menu, "My Profile", _openMyProfile);
			_profileEditorItem = AddMenuItem(menu, "Profile Editor", _openProfileManager);
			AddSectionHeader(menu, "Players");
			_onlineListItem = AddMenuItem(menu, "Online List", _openOnlineList);
			_nearbyPlayersItem = AddMenuItem(menu, "Nearby Players", _openNearby);
			_savedProfilesItem = AddMenuItem(menu, "Saved Profiles", _openSavedProfiles);
			AddSectionHeader(menu, "SPARK Status");
			AddSparkStatusItems(menu);
			AddSectionHeader(menu, "Tools & Settings");
			AddToolsSubmenu(menu);
			AddMenuItem(menu, "Options", _openSettings);
			AddMenuItem(menu, "Manage Blocks", _openBlocklist);
			AddPrivacySubmenu(menu);
			AddMenuItem(menu, "About", _openAbout);
			RefreshMenuState();
			return menu;
		}

		private int CalculateMenuWidth()
		{
			int width = 190;
			width = Math.Max(width, MenuItemWidth("My Profile"));
			width = Math.Max(width, MenuItemWidth("Profile Editor"));
			width = Math.Max(width, MenuItemWidth("Nearby Players"));
			width = Math.Max(width, MenuItemWidth("Saved Profiles"));
			width = Math.Max(width, MenuItemWidth("Manage Blocks"));
			string[] rpStatusOptions = ProfileLabels.RpStatusOptions;
			foreach (string label in rpStatusOptions)
			{
				width = Math.Max(width, MenuItemWidth("Status: " + label));
			}
			width = Math.Max(width, MenuItemWidth("Status: " + CurrentStatusLabel()));
			width = Math.Max(width, MenuItemWidth("SPARK needs attention"));
			return Math.Max(width, MenuItemWidth("Server: SPARK webserver unavailable"));
		}

		private static int MenuItemWidth(string text)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (int)Math.Ceiling(GameService.Content.get_DefaultFont14().MeasureString(text).Width) + 72;
		}

		private static ContextMenuStripItem AddMenuItem(ContextMenuStrip menu, string text, Action action)
		{
			ContextMenuStripItem item = menu.AddMenuItem(text);
			((Control)item).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)item).get_Enabled())
				{
					action?.Invoke();
				}
			});
			return item;
		}

		private static void AddSectionHeader(ContextMenuStrip menu, string text)
		{
			menu.AddMenuItem((ContextMenuStripItem)(object)new ContextMenuHeader(text));
		}

		private void AddSparkStatusItems(ContextMenuStrip menu)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			_readinessMenuItem = new ContextMenuColours("SPARK ready", new Color(140, 220, 140));
			_serverStatusMenuItem = new ContextMenuColours("Server: Disconnected", SparkViewUI.SecondaryTextColor);
			menu.AddMenuItem((ContextMenuStripItem)(object)_readinessMenuItem);
			menu.AddMenuItem((ContextMenuStripItem)(object)_serverStatusMenuItem);
			RefreshSparkStatusItems();
		}

		private void OnServerStatusChanged(ServerSyncStatus status)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isDisposed && _menu != null)
				{
					RefreshSparkStatusItems();
				}
			});
		}

		private void RefreshSparkStatusItems()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			if (_readinessMenuItem != null && _serverStatusMenuItem != null)
			{
				SparkStatusDisplay display = SparkStatusDisplay.Create(_getImportantNotice, _getServerSyncStatus);
				((ContextMenuStripItem)_readinessMenuItem).set_Text(display.ReadinessText);
				_readinessMenuItem.TextColor = display.ReadinessColor;
				((Control)_readinessMenuItem).set_BasicTooltipText(display.ReadinessTooltip);
				((ContextMenuStripItem)_serverStatusMenuItem).set_Text(display.ServerText);
				_serverStatusMenuItem.TextColor = display.ServerColor;
				((Control)_serverStatusMenuItem).set_BasicTooltipText(display.ServerTooltip);
			}
		}

		private void AddPrivacySubmenu(ContextMenuStrip menu)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			menu.AddMenuItem("Privacy").set_Submenu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)GetPrivacyMenuItems));
		}

		private void AddToolsSubmenu(ContextMenuStrip menu)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			ContextMenuStrip val = new ContextMenuStrip();
			((Control)val).set_Width(Math.Max(190, Math.Max(MenuItemWidth("Chat Splitter"), MenuItemWidth("Dice Roll Groups"))));
			ContextMenuStrip toolsMenu = val;
			_chatSplitterItem = AddMenuItem(toolsMenu, "Chat Splitter", _openChatSplitter);
			_rollGroupItem = AddMenuItem(toolsMenu, "Dice Roll Groups", _openRollGroup);
			_toolsMenuItem = menu.AddMenuItem("Tools");
			_toolsMenuItem.set_Submenu(toolsMenu);
		}

		[IteratorStateMachine(typeof(_003CGetPrivacyMenuItems_003Ed__53))]
		private IEnumerable<ContextMenuStripItem> GetPrivacyMenuItems()
		{
			return new _003CGetPrivacyMenuItems_003Ed__53(-2)
			{
				_003C_003E4__this = this
			};
		}

		private static ContextMenuStripItem CreateCheckMenuItem(string text, bool isChecked, Action<bool> onChanged)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(text);
			val.set_CanCheck(true);
			val.set_Checked(isChecked);
			val.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				onChanged?.Invoke(e.get_Checked());
			});
			return val;
		}

		private void SetShareProfile(bool enabled)
		{
			if (_settings.BroadcastProfile.get_Value() != enabled)
			{
				_settings.BroadcastProfile.set_Value(enabled);
				_requestServerSync?.Invoke();
			}
		}

		private void SetHideLocation(bool enabled)
		{
			if (_settings.HideLocation.get_Value() != enabled)
			{
				_settings.HideLocation.set_Value(enabled);
				_requestServerSync?.Invoke();
			}
		}

		private void SetNearbyPresence(bool enabled)
		{
			if (_settings.ShowNearbyPresence.get_Value() != enabled)
			{
				if (_setNearbySharing != null)
				{
					_setNearbySharing(enabled);
				}
				else
				{
					_settings.ShowNearbyPresence.set_Value(enabled);
				}
			}
		}

		private void AddStatusSubmenu(ContextMenuStrip menu)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			RPStatus currentStatus = CurrentStatus();
			_statusMenuItem = menu.AddMenuItem((ContextMenuStripItem)(object)new ContextMenuColours("Status: " + ProfileLabels.StatusLabel(currentStatus), ProfileStatusColors.Get(currentStatus)));
			_statusMenuItem.set_Submenu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)GetStatusMenuItems));
		}

		private static ContextMenuColours CreateColoredCheckMenuItem(string text, RPStatus status, bool isChecked, Action<bool> onChanged)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuColours contextMenuColours = new ContextMenuColours(text, ProfileStatusColors.Get(status));
			((ContextMenuStripItem)contextMenuColours).set_CanCheck(true);
			((ContextMenuStripItem)contextMenuColours).set_Checked(isChecked);
			((ContextMenuStripItem)contextMenuColours).add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				onChanged?.Invoke(e.get_Checked());
			});
			return contextMenuColours;
		}

		[IteratorStateMachine(typeof(_003CGetStatusMenuItems_003Ed__60))]
		private IEnumerable<ContextMenuStripItem> GetStatusMenuItems()
		{
			return new _003CGetStatusMenuItems_003Ed__60(-2)
			{
				_003C_003E4__this = this
			};
		}

		private RPStatus CurrentStatus()
		{
			RPStatus status = _settings.CurrentStatus.get_Value();
			if (status != RPStatus.Offline)
			{
				return status;
			}
			return RPStatus.Online;
		}

		private string CurrentStatusLabel()
		{
			return ProfileLabels.StatusLabel(CurrentStatus());
		}

		private void SetStatus(RPStatus status)
		{
			if (status == RPStatus.Offline)
			{
				status = RPStatus.Online;
			}
			if (_settings.CurrentStatus.get_Value() != status)
			{
				_settings.CurrentStatus.set_Value(status);
				_requestServerSync?.Invoke();
			}
		}

		private void OnCurrentStatusChanged(object sender, ValueChangedEventArgs<RPStatus> e)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isDisposed)
				{
					SyncStatusMenuFromSettings();
				}
			});
		}

		private void SyncStatusMenuFromSettings()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			RPStatus currentStatus = CurrentStatus();
			if (_statusMenuItem != null)
			{
				_statusMenuItem.set_Text("Status: " + ProfileLabels.StatusLabel(currentStatus));
				ContextMenuColours coloredStatusMenuItem = _statusMenuItem as ContextMenuColours;
				if (coloredStatusMenuItem != null)
				{
					coloredStatusMenuItem.TextColor = ProfileStatusColors.Get(currentStatus);
				}
			}
			_isSyncingStatusMenu = true;
			try
			{
				foreach (KeyValuePair<RPStatus, ContextMenuStripItem> pair in _statusMenuItems)
				{
					bool shouldBeChecked = pair.Key == currentStatus;
					if (pair.Value.get_Checked() != shouldBeChecked)
					{
						pair.Value.set_Checked(shouldBeChecked);
					}
				}
			}
			finally
			{
				_isSyncingStatusMenu = false;
			}
		}

		private void RefreshMenuState()
		{
			bool enabled = !ShouldDisableGameplayMenuItems();
			string tooltip = (enabled ? string.Empty : DisabledMenuTooltip());
			SetMenuItemState(_myProfileItem, enabled, tooltip);
			SetMenuItemState(_profileEditorItem, enabled, tooltip);
			SetMenuItemState(_onlineListItem, enabled, tooltip);
			SetMenuItemState(_nearbyPlayersItem, enabled, tooltip);
			SetMenuItemState(_savedProfilesItem, enabled, tooltip);
			SetMenuItemState(_toolsMenuItem, enabled, tooltip);
			SetMenuItemState(_chatSplitterItem, enabled, tooltip);
			SetMenuItemState(_rollGroupItem, enabled, tooltip);
		}

		internal void RefreshForGameState()
		{
			if (_isDisposed)
			{
				return;
			}
			SparkUiThread.Queue(delegate
			{
				if (!_isDisposed && _menu != null)
				{
					RefreshMenuState();
					RefreshSparkStatusItems();
				}
			});
		}

		private static void SetMenuItemState(ContextMenuStripItem item, bool enabled, string tooltip)
		{
			if (item != null)
			{
				((Control)item).set_Enabled(enabled);
				((Control)item).set_BasicTooltipText(enabled ? string.Empty : tooltip);
			}
		}

		private bool ShouldDisableGameplayMenuItems()
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !SparkWindows.IsLoadingScreen())
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

		private string DisabledMenuTooltip()
		{
			if (ShouldHideForGameUi())
			{
				return "SPARK tools are unavailable while the map or game UI is open.";
			}
			return "SPARK tools are unavailable during loading screens or character select.";
		}

		private void OnCornerIconClick(object sender, MouseEventArgs e)
		{
			ShowMenu((Control)(object)_cornerIcon);
		}

		private void OnShowCornerIconChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			SparkUiThread.Queue(Refresh);
		}

		private void ClearCornerIcon()
		{
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
				((Control)_cornerIcon).Dispose();
				_cornerIcon = null;
			}
		}

		private void ClearMenu()
		{
			ContextMenuStrip menu = _menu;
			if (menu != null)
			{
				((Control)menu).Dispose();
			}
			_menu = null;
			_myProfileItem = null;
			_profileEditorItem = null;
			_chatSplitterItem = null;
			_onlineListItem = null;
			_nearbyPlayersItem = null;
			_rollGroupItem = null;
			_savedProfilesItem = null;
			_toolsMenuItem = null;
			_statusMenuItem = null;
			_readinessMenuItem = null;
			_serverStatusMenuItem = null;
			_statusMenuItems.Clear();
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			_settings.ShowCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowCornerIconChanged);
			_settings.CurrentStatus.remove_SettingChanged((EventHandler<ValueChangedEventArgs<RPStatus>>)OnCurrentStatusChanged);
			_unwatchServerSyncStatus?.Invoke(OnServerStatusChanged);
			ClearCornerIcon();
			ClearMenu();
			AsyncTexture2D icon = _icon;
			if (icon != null)
			{
				icon.Dispose();
			}
			if (_disposeHoverIcon)
			{
				AsyncTexture2D hoverIcon = _hoverIcon;
				if (hoverIcon != null)
				{
					hoverIcon.Dispose();
				}
			}
		}
	}
}
