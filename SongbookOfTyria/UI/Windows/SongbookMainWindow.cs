using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Models;
using SongbookOfTyria.Services;
using SongbookOfTyria.UI.Views;

namespace SongbookOfTyria.UI.Windows
{
	public class SongbookMainWindow : TabbedWindow2
	{
		private const int WindowWidth = 920;

		private const int WindowHeight = 660;

		private const int MinWindowHeight = 400;

		private const int MaxWindowHeight = 1200;

		private const int WindowBackgroundAssetId = 155985;

		private const string ModuleName = "Songbook of Tyria";

		private const string MainWindowId = "SongbookOfTyria_Main_Window";

		private readonly TabsCacheService _tabsCacheService;

		private readonly TextureService _textureService;

		private readonly UserSettingsService _userSettingsService;

		private readonly GuildAuthService _guildAuthService;

		private readonly string _cacheDirectory;

		private readonly AudioService _audioService;

		private Tab _aboutTab;

		private Tab _tabLibraryTab;

		private SongLibraryView _tabLibraryView;

		private TabDetailWindow _currentDetailWindow;

		public SongbookMainWindow(TabsCacheService tabsCacheService, TextureService textureService, UserSettingsService userSettingsService, GuildAuthService guildAuthService, string cacheDirectory)
			: this(AsyncTexture2D.FromAssetId(155985), new Rectangle(40, 26, 920, 660), new Rectangle(60, 50, 880, 600))
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_tabsCacheService = tabsCacheService;
			_textureService = textureService;
			_userSettingsService = userSettingsService;
			_guildAuthService = guildAuthService;
			_cacheDirectory = cacheDirectory;
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(_textureService.GetEmblem()));
			_audioService = new AudioService(_cacheDirectory);
			InitializeWindow();
			BuildTabs();
		}

		private void InitializeWindow()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Songbook of Tyria");
			((Control)this).set_Location(new Point(300, 300));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_Id("SongbookOfTyria_Main_Window");
		}

		private void BuildTabs()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			AsyncTexture2D aboutIcon = _textureService.GetAboutIcon();
			AsyncTexture2D tabLibraryIcon = _textureService.GetSongLibraryIcon();
			_tabLibraryView = new SongLibraryView(_tabsCacheService, _textureService, _userSettingsService, _guildAuthService);
			_tabLibraryView.TabClicked += OnTabClicked;
			_aboutTab = new Tab(aboutIcon, (Func<IView>)(() => (IView)(object)new AboutView()), "About", (int?)null);
			_tabLibraryTab = new Tab(tabLibraryIcon, (Func<IView>)(() => (IView)(object)_tabLibraryView), "Songbook", (int?)null);
			((TabbedWindow2)this).get_Tabs().Add(_tabLibraryTab);
			((TabbedWindow2)this).get_Tabs().Add(_aboutTab);
			((TabbedWindow2)this).add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			UpdateSubtitle();
		}

		private void OnTabChanged(object sender, ValueChangedEventArgs<Tab> e)
		{
			UpdateSubtitle();
		}

		private void UpdateSubtitle()
		{
			if (((TabbedWindow2)this).get_SelectedTab() == _aboutTab)
			{
				((WindowBase2)this).set_Subtitle("How to play");
			}
			else if (((TabbedWindow2)this).get_SelectedTab() == _tabLibraryTab)
			{
				((WindowBase2)this).set_Subtitle("Tabs");
			}
		}

		public async Task RefreshTabListAsync()
		{
			await _tabLibraryView.RefreshTabListAsync();
		}

		private async void OnTabClicked(object sender, MusicTab musicTab)
		{
			try
			{
				TabDetailWindow currentDetailWindow = _currentDetailWindow;
				if (currentDetailWindow != null)
				{
					((Control)currentDetailWindow).Dispose();
				}
				MusicTab tabToDisplay = (await _tabsCacheService.GetTabDetailsAsync(musicTab)) ?? musicTab;
				_currentDetailWindow = new TabDetailWindow(tabToDisplay, _textureService, _audioService, _userSettingsService);
				((Control)_currentDetailWindow).Show();
			}
			finally
			{
				_tabLibraryView?.SetTabOpeningComplete();
			}
		}

		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return new Point(920, MathHelper.Clamp(newSize.Y, 400, 1200));
		}

		protected override void DisposeControl()
		{
			TabDetailWindow currentDetailWindow = _currentDetailWindow;
			if (currentDetailWindow != null)
			{
				((Control)currentDetailWindow).Dispose();
			}
			_audioService?.Dispose();
			((TabbedWindow2)this).remove_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			if (_tabLibraryView != null)
			{
				_tabLibraryView.TabClicked -= OnTabClicked;
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
