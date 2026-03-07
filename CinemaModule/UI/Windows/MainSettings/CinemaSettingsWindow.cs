using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using CinemaModule.Controllers;
using CinemaModule.Controllers.WatchParty;
using CinemaModule.Services;
using CinemaModule.Services.Twitch;
using CinemaModule.Services.YouTube;
using CinemaModule.Settings;
using CinemaModule.UI.Windows.Info;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.Windows.MainSettings
{
	public class CinemaSettingsWindow : TabbedWindow2
	{
		private const int WindowWidth = 890;

		private const int BackgroundHeight = 688;

		private const int MinWindowHeight = 460;

		private const int MaxWindowHeight = 1400;

		private const int ContentWidth = 836;

		private const int ContentHeightOffset = 49;

		private readonly CinemaSettings _settings;

		private readonly CinemaUserSettings _userSettings;

		private readonly CinemaController _controller;

		private readonly Gw2MapService _mapService;

		private readonly TwitchService _twitchService;

		private readonly TwitchAuthService _twitchAuthService;

		private readonly PresetService _presetService;

		private readonly YouTubeService _youtubeService;

		private readonly WatchPartyController _watchPartyController;

		private ThirdPartyNoticesWindow _thirdPartyNoticesWindow;

		private StandardButton _infoButton;

		private int _fixedWidth;

		private Tab _sourceTab;

		private const string SourceTabName = "Channel guide";

		private const string SourceTabDisabledName = "Channel guide (disabled while in Watch Party)";

		public CinemaSettingsWindow(CinemaSettings settings, CinemaUserSettings userSettings, CinemaController controller, AsyncTexture2D emblemTexture, AsyncTexture2D windowBackgroundTexture, Gw2MapService mapService, TwitchService twitchService, TwitchAuthService twitchAuthService, PresetService presetService, YouTubeService youtubeService, WatchPartyController watchPartyController)
			: this(windowBackgroundTexture, new Rectangle(40, 26, 890, 688), new Rectangle(70, 36, 836, 639))
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			_settings = settings;
			_userSettings = userSettings;
			_controller = controller;
			_mapService = mapService;
			_twitchService = twitchService;
			_twitchAuthService = twitchAuthService;
			_presetService = presetService;
			_youtubeService = youtubeService;
			_watchPartyController = watchPartyController;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("CinemaHUD");
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(emblemTexture));
			((Control)this).set_Location(new Point(300, 300));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("CinemaModule_SettingsWindow");
			((WindowBase2)this).set_CanResize(true);
			BuildTabs();
			BuildInfoButton();
			_fixedWidth = ((Control)this).get_Width();
			((TabbedWindow2)this).add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnWindowResized);
			_watchPartyController.RoomJoined += OnWatchPartyRoomChanged;
			_watchPartyController.RoomLeft += OnWatchPartyRoomChanged;
			UpdateSubtitleForCurrentTab();
			UpdateSourceTabEnabled();
			ApplySavedHeight();
		}

		private void ApplySavedHeight()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			int savedHeight = _userSettings.SettingsWindowHeight;
			if (savedHeight >= 460 && savedHeight <= 1400 && savedHeight != ((Control)this).get_Height())
			{
				((Control)this).set_Size(new Point(_fixedWidth, savedHeight));
			}
		}

		public override void Show()
		{
			((WindowBase2)this).Show();
			RestoreSelectedTab();
		}

		private void BuildTabs()
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Expected O, but got Unknown
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			AsyncTexture2D displayIcon = CinemaModule.Instance.TextureService.GetDisplayIcon();
			AsyncTexture2D sourceIcon = CinemaModule.Instance.TextureService.GetSourceIcon();
			AsyncTexture2D watchPartyIcon = CinemaModule.Instance.TextureService.GetWatchPartyIcon();
			Tab displayTab = new Tab(displayIcon, (Func<IView>)(() => (IView)(object)new DisplayTabView(_settings, _userSettings, _controller, _mapService, _presetService)), "Display settings", (int?)null);
			((TabbedWindow2)this).get_Tabs().Add(displayTab);
			_sourceTab = new Tab(sourceIcon, (Func<IView>)(() => (IView)(object)new SourceTabView(_userSettings, _controller, _twitchService, _twitchAuthService, _presetService, _youtubeService)), "Channel guide", (int?)null);
			((TabbedWindow2)this).get_Tabs().Add(_sourceTab);
			Tab watchPartyTab = new Tab(watchPartyIcon, (Func<IView>)(() => (IView)(object)new WatchPartyTabView(_watchPartyController, _youtubeService, _userSettings, _controller)), "Watch Party", (int?)null);
			((TabbedWindow2)this).get_Tabs().Add(watchPartyTab);
		}

		private void BuildInfoButton()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Third-Party Notices");
			((Control)val).set_Width(140);
			((Control)val).set_Location(new Point(((Container)this).get_ContentRegion().Width - 150, ((Container)this).get_ContentRegion().Height + 10));
			_infoButton = val;
			((Control)_infoButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowThirdPartyNotices();
			});
		}

		private void ShowThirdPartyNotices()
		{
			_thirdPartyNoticesWindow = _thirdPartyNoticesWindow ?? new ThirdPartyNoticesWindow();
			((Control)_thirdPartyNoticesWindow).Show();
		}

		private void OnTabChanged(object sender, ValueChangedEventArgs<Tab> e)
		{
			UpdateSubtitleForCurrentTab();
			SaveSelectedTab();
		}

		private void UpdateSubtitleForCurrentTab()
		{
			Tab selectedTab = ((TabbedWindow2)this).get_SelectedTab();
			((WindowBase2)this).set_Subtitle(((selectedTab != null) ? selectedTab.get_Name() : null) ?? "Settings");
		}

		private void SaveSelectedTab()
		{
			if (((TabbedWindow2)this).get_SelectedTab() != null)
			{
				int tabIndex = ((TabbedWindow2)this).get_Tabs().IndexOf(((TabbedWindow2)this).get_SelectedTab());
				if (tabIndex >= 0)
				{
					_userSettings.SelectedSettingsTab = tabIndex;
				}
			}
		}

		private void RestoreSelectedTab()
		{
			int savedTabIndex = _userSettings.SelectedSettingsTab;
			if (savedTabIndex >= 0 && savedTabIndex < ((TabbedWindow2)this).get_Tabs().get_Count() && savedTabIndex != ((TabbedWindow2)this).get_Tabs().IndexOf(((TabbedWindow2)this).get_SelectedTab()))
			{
				((TabbedWindow2)this).set_SelectedTab(((TabbedWindow2)this).get_Tabs().FromIndex(savedTabIndex));
			}
		}

		private void OnWindowResized(object sender, ResizedEventArgs e)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			int clampedHeight = Math.Max(460, Math.Min(((Control)this).get_Height(), 1400));
			if (((Control)this).get_Width() != _fixedWidth || ((Control)this).get_Height() != clampedHeight)
			{
				((Control)this).set_Size(new Point(_fixedWidth, clampedHeight));
			}
			_userSettings.SettingsWindowHeight = ((Control)this).get_Height() - 40;
			UpdateInfoButtonPosition();
		}

		private void UpdateInfoButtonPosition()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			((Control)_infoButton).set_Location(new Point(((Container)this).get_ContentRegion().Width - 150, ((Container)this).get_ContentRegion().Height + 10));
		}

		private void OnWatchPartyRoomChanged(object sender, EventArgs e)
		{
			UpdateSourceTabEnabled();
		}

		private void UpdateSourceTabEnabled()
		{
			bool inRoom = _watchPartyController.IsInRoom;
			_sourceTab.set_Enabled(!inRoom);
			_sourceTab.set_Name(inRoom ? "Channel guide (disabled while in Watch Party)" : "Channel guide");
		}

		protected override void DisposeControl()
		{
			_watchPartyController.RoomJoined -= OnWatchPartyRoomChanged;
			_watchPartyController.RoomLeft -= OnWatchPartyRoomChanged;
			StandardButton infoButton = _infoButton;
			if (infoButton != null)
			{
				((Control)infoButton).Dispose();
			}
			ThirdPartyNoticesWindow thirdPartyNoticesWindow = _thirdPartyNoticesWindow;
			if (thirdPartyNoticesWindow != null)
			{
				((Control)thirdPartyNoticesWindow).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
