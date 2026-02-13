using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using CinemaHUD.UI.Windows.MainSettings;
using CinemaModule.Player;
using CinemaModule.Services;
using CinemaModule.Settings;
using CinemaModule.UI.Displays;
using LibVLCSharp.Shared;
using Microsoft.Xna.Framework;

namespace CinemaModule
{
	[Export(typeof(Module))]
	public class CinemaModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<CinemaModule>();

		private const int CornerIconPriority = int.MaxValue;

		private const string CacheDirectoryName = "cinema";

		private CinemaSettings _cinemaSettings;

		private CinemaUserSettings _userSettings;

		private CinemaController _controller;

		private VideoPlayer _videoPlayer;

		private CinemaSettingsWindow _settingsWindow;

		private WindowVideoDisplay _windowDisplayPanel;

		private WorldVideoDisplay _worldDisplayPanel;

		private Gw2MapService _mapService;

		private TwitchService _twitchService;

		private PresetService _presetService;

		private TextureService _textureService;

		private CornerIcon _cornerIcon;

		private bool _needsVideoPlayerInit;

		private string _libvlcDir;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal static CinemaModule Instance { get; private set; }

		internal CinemaUserSettings UserSettings => _userSettings;

		internal TextureService TextureService => _textureService;

		[ImportingConstructor]
		public CinemaModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_cinemaSettings = new CinemaSettings(settings);
		}

		protected override async Task LoadAsync()
		{
			_ = 1;
			try
			{
				_libvlcDir = DirectoriesManager.GetFullDirectoryPath("libvlc");
				Logger.Info("LibVLC directory: " + _libvlcDir);
				string cacheDirectory = Path.Combine(DirectoryUtil.get_CachePath(), "cinema");
				Logger.Info("Settings directory: " + cacheDirectory);
				_userSettings = new CinemaUserSettings(cacheDirectory);
				Logger.Info("CinemaUserSettings initialized successfully");
				await new LibVlcService(ContentsManager).ExtractAsync(_libvlcDir);
				string libvlcBinPath = LibVlcService.GetBinPath(_libvlcDir);
				Logger.Info("Initializing LibVLC from: " + libvlcBinPath);
				if (!Directory.Exists(libvlcBinPath))
				{
					Logger.Error("LibVLC bin path does not exist: " + libvlcBinPath);
					return;
				}
				Core.Initialize(libvlcBinPath);
				Logger.Info("LibVLC initialized successfully");
				_mapService = new Gw2MapService(cacheDirectory);
				Logger.Info("Gw2MapService initialized successfully");
				_twitchService = new TwitchService(cacheDirectory);
				Logger.Info("TwitchService initialized successfully");
				_presetService = new PresetService(cacheDirectory);
				await _presetService.LoadPresetsAsync();
				Logger.Info("PresetService initialized successfully");
				_textureService = new TextureService(cacheDirectory);
				Logger.Info("TextureService initialized successfully");
				_needsVideoPlayerInit = true;
				_controller = new CinemaController(_cinemaSettings, _userSettings, _twitchService);
				_controller.ShowSettingsRequested += delegate
				{
					CinemaSettingsWindow settingsWindow = _settingsWindow;
					if (settingsWindow != null)
					{
						((WindowBase2)settingsWindow).ToggleWindow();
					}
				};
				CreateCornerIcon();
				CreateSettingsWindow();
				CreateVideoDisplays();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to load CinemaHUD module");
			}
		}

		private void InitializeVideoPlayer()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (_videoPlayer == null)
			{
				GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					_videoPlayer = new VideoPlayer(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), VideoPlayerOptions.Default);
				}
				finally
				{
					((GraphicsDeviceContext)(ref ctx)).Dispose();
				}
				_controller.RegisterPlayer(_videoPlayer);
			}
		}

		protected override void Update(GameTime gameTime)
		{
			if (_needsVideoPlayerInit)
			{
				_needsVideoPlayerInit = false;
				InitializeVideoPlayer();
			}
			_controller?.Update();
		}

		protected override void Unload()
		{
			_controller?.Dispose();
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			CinemaSettingsWindow settingsWindow = _settingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Dispose();
			}
			WindowVideoDisplay windowDisplayPanel = _windowDisplayPanel;
			if (windowDisplayPanel != null)
			{
				((Control)windowDisplayPanel).Dispose();
			}
			WorldVideoDisplay worldDisplayPanel = _worldDisplayPanel;
			if (worldDisplayPanel != null)
			{
				((Control)worldDisplayPanel).Dispose();
			}
			_videoPlayer?.Dispose();
			_twitchService?.Dispose();
			_presetService?.Dispose();
			_textureService?.Dispose();
			_cinemaSettings?.Dispose();
			Instance = null;
		}

		private void CreateCornerIcon()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			AsyncTexture2D tvIcon = _textureService.GetCornerIcon();
			if (tvIcon == null)
			{
				Logger.Warn("Failed to load corner icon texture");
				return;
			}
			CornerIcon val = new CornerIcon();
			val.set_Icon(tvIcon);
			val.set_HoverIcon(tvIcon);
			((Control)val).set_BasicTooltipText("CinemaHUD Settings");
			val.set_Priority(int.MaxValue);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CinemaSettingsWindow settingsWindow = _settingsWindow;
				if (settingsWindow != null)
				{
					((WindowBase2)settingsWindow).ToggleWindow();
				}
			});
		}

		private void CreateSettingsWindow()
		{
			Logger.Info("Creating settings window...");
			AsyncTexture2D windowTexture = _textureService.GetWindowTexture();
			AsyncTexture2D emblemTexture = _textureService.GetEmblem();
			if (emblemTexture == null)
			{
				Logger.Warn("Failed to load settings window emblem texture");
			}
			_settingsWindow = new CinemaSettingsWindow(windowTexture, _cinemaSettings, _userSettings, _controller, emblemTexture, _mapService, _twitchService, _presetService);
			Logger.Info("Settings window created successfully");
		}

		private void CreateVideoDisplays()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			WindowVideoDisplay obj = new WindowVideoDisplay
			{
				Size = _userSettings.WindowSize,
				Location = _userSettings.WindowPosition
			};
			((Control)obj).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_windowDisplayPanel = obj;
			WorldVideoDisplay obj2 = new WorldVideoDisplay
			{
				WorldPosition = _userSettings.WorldPosition,
				WorldWidth = _userSettings.WorldScreenWidth
			};
			((Control)obj2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_worldDisplayPanel = obj2;
			_worldDisplayPanel.Initialize((Container)(object)GameService.Graphics.get_SpriteScreen());
			_controller.RegisterDisplays(_windowDisplayPanel, _worldDisplayPanel);
		}
	}
}
