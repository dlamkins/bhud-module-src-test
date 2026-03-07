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
using CinemaModule.Controllers;
using CinemaModule.Controllers.WatchParty;
using CinemaModule.Models;
using CinemaModule.Services;
using CinemaModule.Services.Twitch;
using CinemaModule.Services.YouTube;
using CinemaModule.Settings;
using CinemaModule.UI.Chat;
using CinemaModule.UI.VideoDisplays;
using CinemaModule.UI.Views;
using CinemaModule.UI.Windows.MainSettings;
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

		private YouTubeService _youtubeService;

		private TwitchAuthService _twitchAuthService;

		private TwitchChatService _twitchChatService;

		private TwitchChatWindow _twitchChatWindow;

		private PresetService _presetService;

		private TextureService _textureService;

		private WatchPartyController _watchPartyController;

		private CornerIcon _cornerIcon;

		private bool _needsVideoPlayerInit;

		private bool _needsTwitchChatRestore;

		private bool _needsWindowDisplayRestore;

		private string _libvlcDir;

		private bool _libvlcInitialized;

		internal const string ModuleVersion = "2.0.0";

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
			try
			{
				_libvlcDir = DirectoriesManager.GetFullDirectoryPath("libvlc");
				Logger.Info("LibVLC directory: " + _libvlcDir);
				string cacheDirectory = Path.Combine(DirectoryUtil.get_CachePath(), "cinema");
				_userSettings = new CinemaUserSettings(cacheDirectory);
				Task.Run(async delegate
				{
					await InitializeLibVlcAsync();
				});
				_mapService = new Gw2MapService(cacheDirectory);
				_twitchService = new TwitchService();
				_youtubeService = new YouTubeService();
				_twitchAuthService = new TwitchAuthService();
				InitializeTwitchAuth();
				_twitchChatService = new TwitchChatService();
				_textureService = new TextureService(cacheDirectory);
				_presetService = new PresetService(_textureService);
				_presetService.PresetImagesLoaded += OnPresetImagesLoaded;
				_presetService.LoadPresetsAsync();
				_needsVideoPlayerInit = true;
				_watchPartyController = new WatchPartyController(base.ModuleParameters.get_Gw2ApiManager(), _youtubeService);
				_controller = new CinemaController(_cinemaSettings, _userSettings, _twitchService, _youtubeService);
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
				_controller.RegisterWatchParty(_watchPartyController);
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

		private async Task InitializeLibVlcAsync()
		{
			try
			{
				await new LibVlcService(ContentsManager).ExtractAsync(_libvlcDir).ConfigureAwait(continueOnCapturedContext: false);
				string libvlcBinPath = LibVlcService.GetBinPath(_libvlcDir);
				if (!Directory.Exists(libvlcBinPath))
				{
					Logger.Error("LibVLC bin path does not exist: " + libvlcBinPath);
					return;
				}
				Core.Initialize(libvlcBinPath);
				_libvlcInitialized = true;
				Logger.Info("LibVLC initialized successfully");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to initialize LibVLC");
			}
		}

		private void InitializeVideoPlayer()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (_videoPlayer == null && _libvlcInitialized)
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
			if (_needsVideoPlayerInit && _libvlcInitialized)
			{
				_needsVideoPlayerInit = false;
				InitializeVideoPlayer();
			}
			ProcessDeferredRestoration();
			_controller?.Update();
		}

		private void ProcessDeferredRestoration()
		{
			if ((_needsTwitchChatRestore || _needsWindowDisplayRestore) && IsScreenSizeValid())
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

		private static bool IsScreenSizeValid()
		{
			if (((Control)GameService.Graphics.get_SpriteScreen()).get_Width() >= 640)
			{
				return ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() >= 480;
			}
			return false;
		}

		protected override void Unload()
		{
			SafeUnsubscribe(delegate
			{
				_twitchAuthService.AuthStatusChanged -= OnTwitchAuthStatusChanged;
			});
			SafeUnsubscribe(delegate
			{
				_twitchService.ScopeError -= OnTwitchScopeError;
			});
			SafeUnsubscribe(delegate
			{
				_presetService.PresetImagesLoaded -= OnPresetImagesLoaded;
			});
			SafeUnsubscribe(delegate
			{
				_controller.ShowChatRequested -= OnShowChatRequested;
			});
			SafeUnsubscribe(delegate
			{
				_controller.ToggleChatRequested -= OnToggleChatRequested;
			});
			SafeUnsubscribe(delegate
			{
				_controller.ChatChannelChangeRequested -= OnChatChannelChangeRequested;
			});
			SafeDispose(_controller);
			SafeDispose((IDisposable)_cornerIcon);
			SafeDispose((IDisposable)_settingsWindow);
			SafeDispose((IDisposable)_twitchChatWindow);
			SafeDispose(_windowDisplayPanel);
			SafeDispose(_worldDisplayPanel);
			SafeDispose(_videoPlayer);
			SafeDispose(_twitchService);
			SafeDispose(_youtubeService);
			SafeDispose(_twitchAuthService);
			SafeDispose(_twitchChatService);
			SafeDispose(_watchPartyController);
			SafeDispose(_presetService);
			SafeDispose(_textureService);
			SafeDispose(_mapService);
			SafeDispose(_cinemaSettings);
			Instance = null;
		}

		private static void SafeUnsubscribe(Action unsubscribe)
		{
			try
			{
				unsubscribe();
			}
			catch
			{
			}
		}

		private static void SafeDispose(IDisposable disposable)
		{
			try
			{
				disposable?.Dispose();
			}
			catch
			{
			}
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
			AsyncTexture2D windowBackgroundTexture = _textureService.GetTabbedWindowBackground();
			_settingsWindow = new CinemaSettingsWindow(_cinemaSettings, _userSettings, _controller, emblemTexture, windowBackgroundTexture, _mapService, _twitchService, _twitchAuthService, _presetService, _youtubeService, _watchPartyController);
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
				ApplyTwitchCredentials(e);
			}
			else if (e.Status == TwitchAuthStatus.NotAuthenticated)
			{
				ClearTwitchCredentials();
			}
		}

		private void ApplyTwitchCredentials(TwitchAuthStatusEventArgs e)
		{
			_userSettings.TwitchAccessToken = e.AccessToken;
			_userSettings.TwitchRefreshToken = e.RefreshToken;
			_twitchService.SetAuthToken(e.AccessToken, e.UserId);
			_twitchChatService.SetCredentials(e.Username, e.AccessToken);
		}

		private void ClearTwitchCredentials()
		{
			_userSettings.TwitchAccessToken = null;
			_userSettings.TwitchRefreshToken = null;
			_twitchService.SetAuthToken(null);
			_twitchChatService.SetCredentials(null, null);
		}

		private void OnShowChatRequested(object sender, string channelName)
		{
			EnsureChatWindowCreated();
			_twitchChatWindow.ConnectToChannel(channelName);
			if (!((Control)_twitchChatWindow).get_Visible())
			{
				((Control)_twitchChatWindow).Show();
			}
		}

		private void OnToggleChatRequested(object sender, string channelName)
		{
			EnsureChatWindowCreated();
			if (((Control)_twitchChatWindow).get_Visible())
			{
				((Control)_twitchChatWindow).Hide();
				return;
			}
			_twitchChatWindow.ConnectToChannel(channelName);
			((Control)_twitchChatWindow).Show();
		}

		private void EnsureChatWindowCreated()
		{
			if (_twitchChatWindow == null)
			{
				_twitchChatWindow = new TwitchChatWindow(_twitchChatService, _twitchAuthService, _userSettings);
				_controller.ChatChannelChangeRequested += OnChatChannelChangeRequested;
			}
		}

		private void OnChatChannelChangeRequested(object sender, string channelName)
		{
			if (_twitchChatWindow != null && ((Control)_twitchChatWindow).get_Visible())
			{
				if (string.IsNullOrEmpty(channelName))
				{
					_twitchChatWindow.Disconnect();
				}
				else
				{
					_twitchChatWindow.ConnectToChannel(channelName);
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
