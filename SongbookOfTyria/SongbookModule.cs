using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Models.Api;
using SongbookOfTyria.Services;
using SongbookOfTyria.Settings;
using SongbookOfTyria.UI.Controls.Notation;
using SongbookOfTyria.UI.Utilities;
using SongbookOfTyria.UI.Views;
using SongbookOfTyria.UI.Windows;

namespace SongbookOfTyria
{
	[Export(typeof(Module))]
	public class SongbookModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<SongbookModule>();

		private const string CacheDirectoryName = "songbook_cache";

		private const string TexturesCacheFolder = "textures";

		private const int CornerIconPriority = 1645843524;

		private ApiService _apiService;

		private TextureService _textureService;

		private TabsService _tabsService;

		private GuildAuthService _guildAuthService;

		private UserSettingsService _userSettingsService;

		private ModuleSettings _moduleSettings;

		private CornerIcon _cornerIcon;

		private SongbookMainWindow _mainWindow;

		internal static SongbookModule ModuleInstance { get; private set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public SongbookModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_moduleSettings = new ModuleSettings(settings);
		}

		protected override async Task LoadAsync()
		{
			string cacheDirectory = EnsureCacheDirectoryExists();
			Logger.Info("Cache directory: {Directory}", new object[1] { cacheDirectory });
			NotationRenderer.InitializeFonts(ContentsManager);
			string texturesCacheDirectory = Path.Combine(cacheDirectory, "textures");
			_textureService = new TextureService(ContentsManager, texturesCacheDirectory);
			_guildAuthService = new GuildAuthService();
			_apiService = new ApiService(_guildAuthService);
			_tabsService = new TabsService(_apiService);
			_userSettingsService = new UserSettingsService(cacheDirectory);
			_tabsService.TabsLoaded += OnTabsLoaded;
			_moduleSettings.InitializeServices(_tabsService, _textureService, _guildAuthService);
			_guildAuthService.AuthStatusChanged += OnAuthStatusChanged;
			_mainWindow = new SongbookMainWindow(_tabsService, _textureService, _userSettingsService, _guildAuthService, _moduleSettings, cacheDirectory);
			CreateCornerIcon();
			_moduleSettings.InitializeGuildAuthAsync();
		}

		private string EnsureCacheDirectoryExists()
		{
			string text = Path.Combine(DirectoryUtil.get_CachePath(), "songbook_cache");
			Directory.CreateDirectory(text);
			return text;
		}

		private void CreateCornerIcon()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			AsyncTexture2D cornerTexture = _textureService.GetCornerIcon();
			CornerIcon val = new CornerIcon();
			val.set_Icon(cornerTexture);
			val.set_HoverIcon(cornerTexture);
			((Control)val).set_BasicTooltipText("Songbook of Tyria");
			val.set_Priority(1645843524);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SongbookMainWindow mainWindow = _mainWindow;
				if (mainWindow != null)
				{
					((WindowBase2)mainWindow).ToggleWindow();
				}
			});
		}

		private void OnTabsLoaded(object sender, TabsResponse response)
		{
			if (response?.Tabs != null)
			{
				_textureService.PreloadThumbnails(response.Tabs);
			}
		}

		private async void OnAuthStatusChanged(object sender, GuildAuthStatusChangedEventArgs e)
		{
			if (_tabsService != null)
			{
				await _tabsService.RefreshTabsAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			if (_mainWindow != null)
			{
				await _mainWindow.RefreshTabListAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleSettingsView(_moduleSettings);
		}

		protected override void Update(GameTime gameTime)
		{
			_textureService?.ProcessPendingTextures();
		}

		protected override void Unload()
		{
			SafeUnsubscribe(delegate
			{
				_tabsService.TabsLoaded -= OnTabsLoaded;
			});
			SafeUnsubscribe(delegate
			{
				_guildAuthService.AuthStatusChanged -= OnAuthStatusChanged;
			});
			SafeDispose(_moduleSettings);
			SafeDispose(_guildAuthService);
			SafeDispose((IDisposable)_cornerIcon);
			SafeDispose((IDisposable)_mainWindow);
			SafeDispose(_apiService);
			SafeDispose(_textureService);
			BitmapFontLoader.ClearCache();
			ModuleInstance = null;
		}

		private static void SafeUnsubscribe(Action unsubscribe)
		{
			try
			{
				unsubscribe();
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Error during event unsubscription");
			}
		}

		private static void SafeDispose(IDisposable disposable)
		{
			try
			{
				disposable?.Dispose();
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Error during disposal");
			}
		}
	}
}
