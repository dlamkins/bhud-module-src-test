using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using CinemaHUD.UI.Windows.MainSettings;
using CinemaModule.Models;
using CinemaModule.Services;
using CinemaModule.Settings;
using CinemaModule.UI.Chat;
using CinemaModule.UI.VideoDisplays;
using CinemaModule.UI.Views;
using CinemaModule.VideoPlayer;
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

		private global::CinemaModule.VideoPlayer.VideoPlayer _videoPlayer;

		private CinemaSettingsWindow _settingsWindow;

		private WindowVideoDisplay _windowDisplayPanel;

		private WorldVideoDisplay _worldDisplayPanel;

		private Gw2MapService _mapService;

		private TwitchService _twitchService;

		private TwitchAuthService _twitchAuthService;

		private TwitchChatService _twitchChatService;

		private TwitchChatWindow _twitchChatWindow;

		private PresetService _presetService;

		private TextureService _textureService;

		private CornerIcon _cornerIcon;

		private bool _needsVideoPlayerInit;

		private bool _needsTwitchChatRestore;

		private bool _needsWindowDisplayRestore;

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

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleSettingsView(SettingsManager.get_ModuleSettings(), _userSettings, _twitchAuthService, delegate
			{
				_cinemaSettings?.ShowThirdPartyNotices();
			});
		}

		protected override async Task LoadAsync()
		{
			_ = 1;
			try
			{
				_libvlcDir = DirectoriesManager.GetFullDirectoryPath("libvlc");
				Logger.Info("LibVLC directory: " + _libvlcDir);
				string cacheDirectory = Path.Combine(DirectoryUtil.get_CachePath(), "cinema");
				_userSettings = new CinemaUserSettings(cacheDirectory);
				await new LibVlcService(ContentsManager).ExtractAsync(_libvlcDir);
				string libvlcBinPath = LibVlcService.GetBinPath(_libvlcDir);
				if (!Directory.Exists(libvlcBinPath))
				{
					Logger.Error("LibVLC bin path does not exist: " + libvlcBinPath);
					return;
				}
				Core.Initialize(libvlcBinPath);
				_mapService = new Gw2MapService(cacheDirectory);
				_twitchService = new TwitchService(cacheDirectory);
				_twitchAuthService = new TwitchAuthService();
				InitializeTwitchAuth();
				_twitchChatService = new TwitchChatService();
				_presetService = new PresetService(cacheDirectory);
				_presetService.PresetImagesLoaded += OnPresetImagesLoaded;
				await _presetService.LoadPresetsAsync();
				_textureService = new TextureService(cacheDirectory);
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
				_controller.ShowChatRequested += OnShowChatRequested;
				_controller.ToggleChatRequested += OnToggleChatRequested;
				CreateCornerIcon();
				CreateSettingsWindow();
				CreateVideoDisplays();
				_needsTwitchChatRestore = true;
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
					_videoPlayer = new global::CinemaModule.VideoPlayer.VideoPlayer(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), VideoPlayerOptions.Default);
				}
				finally
				{
					((GraphicsDeviceContext)(ref ctx)).Dispose();
				}
				_controller.RegisterPlayer(_videoPlayer);
				_controller.StartInitialPlaybackIfEnabled();
			}
		}

		protected override void Update(GameTime gameTime)
		{
			if (_needsVideoPlayerInit)
			{
				_needsVideoPlayerInit = false;
				InitializeVideoPlayer();
			}
			if (_needsTwitchChatRestore || _needsWindowDisplayRestore)
			{
				int width = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
				int screenHeight = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
				if (width >= 640 && screenHeight >= 480)
				{
					if (_needsWindowDisplayRestore)
					{
						_needsWindowDisplayRestore = false;
						RestoreWindowDisplayPosition();
					}
					if (_needsTwitchChatRestore)
					{
						_needsTwitchChatRestore = false;
						RestoreTwitchChatWindow();
					}
				}
			}
			_controller?.Update();
		}

		protected override void Unload()
		{
			_twitchAuthService.AuthStatusChanged -= OnTwitchAuthStatusChanged;
			_twitchService.ScopeError -= OnTwitchScopeError;
			_presetService.PresetImagesLoaded -= OnPresetImagesLoaded;
			_controller.ShowChatRequested -= OnShowChatRequested;
			_controller.ToggleChatRequested -= OnToggleChatRequested;
			_controller.ChatChannelChangeRequested -= OnChatChannelChangeRequested;
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
			TwitchChatWindow twitchChatWindow = _twitchChatWindow;
			if (twitchChatWindow != null)
			{
				((Control)twitchChatWindow).Dispose();
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
			_twitchAuthService?.Dispose();
			_twitchChatService?.Dispose();
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
			AsyncTexture2D emblemTexture = _textureService.GetEmblem();
			_settingsWindow = new CinemaSettingsWindow(_cinemaSettings, _userSettings, _controller, emblemTexture, _mapService, _twitchService, _twitchAuthService, _presetService);
		}

		private void InitializeTwitchAuth()
		{
			_twitchAuthService.LoadTokens(_userSettings.TwitchAccessToken, _userSettings.TwitchRefreshToken);
			if (_twitchAuthService.IsAuthenticated)
			{
				_twitchService.SetAuthToken(_userSettings.TwitchAccessToken, _twitchAuthService.UserId);
			}
			_twitchAuthService.AuthStatusChanged += OnTwitchAuthStatusChanged;
			_twitchService.ScopeError += OnTwitchScopeError;
		}

		private void OnTwitchScopeError(object sender, EventArgs e)
		{
			Logger.Warn("Twitch token missing required scopes - forcing re-authentication");
			_twitchAuthService.LogoutAsync();
		}

		private void OnTwitchAuthStatusChanged(object sender, TwitchAuthStatusEventArgs e)
		{
			if (e.Status == TwitchAuthStatus.Authenticated)
			{
				_userSettings.TwitchAccessToken = e.AccessToken;
				_userSettings.TwitchRefreshToken = e.RefreshToken;
				_twitchService.SetAuthToken(e.AccessToken, e.UserId);
				_twitchChatService.SetCredentials(e.Username, e.AccessToken);
			}
			else if (e.Status == TwitchAuthStatus.NotAuthenticated)
			{
				_userSettings.TwitchAccessToken = null;
				_userSettings.TwitchRefreshToken = null;
				_twitchService.SetAuthToken(null);
				_twitchChatService.SetCredentials(null, null);
			}
		}

		private void OnShowChatRequested(object sender, string channelName)
		{
			if (_twitchChatWindow == null)
			{
				_twitchChatWindow = new TwitchChatWindow(_twitchChatService, _twitchAuthService, _userSettings);
				_controller.ChatChannelChangeRequested += OnChatChannelChangeRequested;
			}
			_twitchChatWindow.ConnectToChannel(channelName);
			if (!((Control)_twitchChatWindow).get_Visible())
			{
				((Control)_twitchChatWindow).Show();
			}
		}

		private void OnToggleChatRequested(object sender, string channelName)
		{
			if (_twitchChatWindow == null)
			{
				_twitchChatWindow = new TwitchChatWindow(_twitchChatService, _twitchAuthService, _userSettings);
				_controller.ChatChannelChangeRequested += OnChatChannelChangeRequested;
			}
			if (((Control)_twitchChatWindow).get_Visible())
			{
				((Control)_twitchChatWindow).Hide();
				return;
			}
			_twitchChatWindow.ConnectToChannel(channelName);
			((Control)_twitchChatWindow).Show();
		}

		private void OnChatChannelChangeRequested(object sender, string channelName)
		{
			if (_twitchChatWindow != null && ((Control)_twitchChatWindow).get_Visible())
			{
				if (!string.IsNullOrEmpty(channelName))
				{
					_twitchChatWindow.ConnectToChannel(channelName);
				}
				else
				{
					_twitchChatWindow.Disconnect();
				}
			}
		}

		private void RestoreTwitchChatWindow()
		{
			if (_userSettings.TwitchChatWindowOpen)
			{
				string channel = _userSettings.TwitchChatWindowChannel;
				if (!string.IsNullOrEmpty(channel))
				{
					OnShowChatRequested(this, channel);
				}
			}
		}

		private void CreateVideoDisplays()
		{
			_windowDisplayPanel = new WindowVideoDisplay();
			((Control)_windowDisplayPanel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_windowDisplayPanel.IsLocked = _userSettings.WindowLocked;
			_needsWindowDisplayRestore = true;
			WorldVideoDisplay obj = new WorldVideoDisplay
			{
				WorldPosition = _userSettings.WorldPosition,
				WorldWidth = _userSettings.WorldScreenWidth
			};
			((Control)obj).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_worldDisplayPanel = obj;
			_worldDisplayPanel.Initialize((Container)(object)GameService.Graphics.get_SpriteScreen());
			_controller.RegisterDisplays(_windowDisplayPanel, _worldDisplayPanel);
		}

		private void RestoreWindowDisplayPosition()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (_windowDisplayPanel != null)
			{
				_windowDisplayPanel.Size = _userSettings.WindowSize;
				_windowDisplayPanel.Location = _userSettings.WindowPosition;
			}
		}

		private void OnPresetImagesLoaded(object sender, EventArgs e)
		{
			if (_userSettings.CurrentStreamSourceType != 0)
			{
				return;
			}
			string channelId = _userSettings.SelectedUrlChannelId;
			if (!string.IsNullOrEmpty(channelId))
			{
				ChannelData channel = _presetService.FindChannelById(channelId);
				if (channel != null && channel.IsRadio)
				{
					_userSettings.CurrentStreamPreset = channel.ToStreamPresetData();
				}
			}
		}
	}
}
