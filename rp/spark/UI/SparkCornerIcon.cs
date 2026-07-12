using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using rp.spark.Services;

namespace rp.spark.UI
{
	internal sealed class SparkCornerIcon : IDisposable
	{
		private const int MenuWidth = 170;

		private readonly SparkSettings _settings;

		private readonly AsyncTexture2D _icon;

		private readonly AsyncTexture2D _hoverIcon;

		private readonly bool _disposeHoverIcon;

		private readonly Action _openProfileManager;

		private readonly Action _openOnlineList;

		private readonly Action _openNearby;

		private readonly Action _openSavedProfiles;

		private readonly Action _openBlocklist;

		private CornerIcon _cornerIcon;

		private ContextMenuStrip _menu;

		private ContextMenuStripItem _profileEditorItem;

		private ContextMenuStripItem _onlineListItem;

		private ContextMenuStripItem _nearbyPlayersItem;

		private ContextMenuStripItem _savedProfilesItem;

		private bool _isDisposed;

		public SparkCornerIcon(SparkSettings settings, ContentsManager contentsManager, Action openProfileManager, Action openOnlineList, Action openNearby, Action openSavedProfiles, Action openBlocklist)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			_settings = settings;
			_openProfileManager = openProfileManager;
			_openOnlineList = openOnlineList;
			_openNearby = openNearby;
			_openSavedProfiles = openSavedProfiles;
			_openBlocklist = openBlocklist;
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
		}

		public void Refresh()
		{
			if (!_isDisposed)
			{
				if (_settings.ShowCornerIcon.get_Value())
				{
					EnsureCreated();
				}
				else
				{
					Clear();
				}
			}
		}

		private void EnsureCreated()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			if (_cornerIcon == null)
			{
				_menu = BuildMenu();
				CornerIcon val = new CornerIcon(_icon, _hoverIcon, "SPARK");
				val.set_Priority(0);
				_cornerIcon = val;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
			}
		}

		private ContextMenuStrip BuildMenu()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			ContextMenuStrip val = new ContextMenuStrip();
			((Control)val).set_Width(170);
			ContextMenuStrip menu = val;
			_profileEditorItem = AddMenuItem(menu, "Profile Editor", _openProfileManager);
			_onlineListItem = AddMenuItem(menu, "Online List", _openOnlineList);
			_nearbyPlayersItem = AddMenuItem(menu, "Nearby Players", _openNearby);
			_savedProfilesItem = AddMenuItem(menu, "Saved Profiles", _openSavedProfiles);
			AddMenuItem(menu, "Manage Blocks", _openBlocklist);
			RefreshMenuState();
			return menu;
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

		private void RefreshMenuState()
		{
			bool enabled = !ShouldDisableGameplayMenuItems();
			string tooltip = (enabled ? string.Empty : DisabledMenuTooltip());
			SetMenuItemState(_profileEditorItem, enabled, tooltip);
			SetMenuItemState(_onlineListItem, enabled, tooltip);
			SetMenuItemState(_nearbyPlayersItem, enabled, tooltip);
			SetMenuItemState(_savedProfilesItem, enabled, tooltip);
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
					if (((Control)_menu).get_Visible())
					{
						((Control)_menu).Hide();
					}
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
				return "SPARK profile tools are unavailable while the map or game UI is open.";
			}
			return "SPARK profile tools are unavailable during loading screens or character select.";
		}

		private void OnCornerIconClick(object sender, MouseEventArgs e)
		{
			RefreshMenuState();
			ContextMenuStrip menu = _menu;
			if (menu != null)
			{
				menu.Show((Control)(object)_cornerIcon);
			}
		}

		private void OnShowCornerIconChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			SparkUiThread.Queue(Refresh);
		}

		private void Clear()
		{
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
				((Control)_cornerIcon).Dispose();
				_cornerIcon = null;
			}
			ContextMenuStrip menu = _menu;
			if (menu != null)
			{
				((Control)menu).Dispose();
			}
			_menu = null;
			_profileEditorItem = null;
			_onlineListItem = null;
			_nearbyPlayersItem = null;
			_savedProfilesItem = null;
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			_settings.ShowCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowCornerIconChanged);
			Clear();
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
