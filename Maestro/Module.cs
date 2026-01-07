using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Maestro.Models;
using Maestro.Services.Data;
using Maestro.Services.Playback;
using Maestro.Settings;
using Maestro.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class Module : Blish_HUD.Modules.Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private ModuleSettings _moduleSettings;

		private KeyboardService _keyboardService;

		private SongPlayer _songPlayer;

		private MaestroWindow _maestroWindow;

		private CornerIcon _cornerIcon;

		private List<Song> _songs;

		private Texture2D _windowBackground;

		internal static Module Instance { get; private set; }

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_moduleSettings = new ModuleSettings(settings);
		}

		protected override void Initialize()
		{
			_keyboardService = new KeyboardService(_moduleSettings.GetKeyMappings(), _moduleSettings.GetSharpMappings());
			_songPlayer = new SongPlayer(_keyboardService);
			_songs = new List<Song>();
		}

		protected override async Task LoadAsync()
		{
			_songs = await SongLoader.LoadAllAsync(GetSongsDirectory());
		}

		private string GetSongsDirectory()
		{
			try
			{
				string assemblyLocation = GetType().Assembly.Location;
				if (!string.IsNullOrEmpty(assemblyLocation))
				{
					string embeddedSongsPath = Path.Combine(Path.GetDirectoryName(assemblyLocation), "Songs");
					if (Directory.Exists(embeddedSongsPath))
					{
						Logger.Info("Using embedded songs path: " + embeddedSongsPath);
						return embeddedSongsPath;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Debug("Could not get assembly location: " + ex.Message);
			}
			if (Directory.Exists("C:\\git\\Maestro\\Songs"))
			{
				Logger.Info("Using debug songs path");
				return "C:\\git\\Maestro\\Songs";
			}
			Logger.Warn("No songs directory found");
			return "C:\\git\\Maestro\\Songs";
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			CreateWindowBackground();
			try
			{
				Texture2D iconTexture = ContentsManager.GetTexture("icon.png");
				_cornerIcon = new CornerIcon
				{
					Icon = (iconTexture ?? ContentService.Textures.Error),
					BasicTooltipText = "Maestro - Music Player"
				};
				_cornerIcon.Click += OnCornerIconClick;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load icon, using default");
				_cornerIcon = new CornerIcon
				{
					Icon = ContentService.Textures.Error,
					BasicTooltipText = "Maestro - Music Player"
				};
				_cornerIcon.Click += OnCornerIconClick;
			}
			base.OnModuleLoaded(e);
		}

		private void CreateWindowBackground()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			using GraphicsDeviceContext graphicsDeviceContext = GameService.Graphics.LendGraphicsDeviceContext();
			_windowBackground = new Texture2D(graphicsDeviceContext.GraphicsDevice, 419, 447);
			Color[] data = (Color[])(object)new Color[187293];
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = new Color(30, 30, 30, 255);
			}
			_windowBackground.SetData<Color>(data);
		}

		private void OnCornerIconClick(object sender, MouseEventArgs e)
		{
			if (_maestroWindow == null)
			{
				_maestroWindow = new MaestroWindow(_windowBackground, _songPlayer, _songs);
			}
			_maestroWindow.ToggleWindow();
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			_songPlayer?.Stop();
			_maestroWindow?.Dispose();
			_cornerIcon?.Dispose();
			Texture2D windowBackground = _windowBackground;
			if (windowBackground != null)
			{
				((GraphicsResource)windowBackground).Dispose();
			}
			Instance = null;
		}
	}
}
