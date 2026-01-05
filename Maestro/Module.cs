using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Maestro.Models;
using Maestro.Services;
using Maestro.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private KeyboardService _keyboardService;

		private SongPlayer _songPlayer;

		private MaestroWindow _maestroWindow;

		private CornerIcon _cornerIcon;

		private List<Song> _songs;

		private Texture2D _windowBackground;

		internal static Module Instance { get; private set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		protected override void Initialize()
		{
			_keyboardService = new KeyboardService();
			_songPlayer = new SongPlayer(_keyboardService);
			_songs = new List<Song>();
		}

		protected override async Task LoadAsync()
		{
			await LoadSongsFromDirectory();
		}

		private async Task LoadSongsFromDirectory()
		{
			await Task.Run(delegate
			{
				try
				{
					string text = "C:\\git\\Maestro\\Songs";
					if (Directory.Exists(text))
					{
						Logger.Info("Debug mode: Loading songs from source directory: " + text);
						LoadSongsFromPath(text);
					}
					if (_songs.Count == 0)
					{
						LoadSongsFromBundle();
					}
					Logger.Info($"Total songs loaded: {_songs.Count}");
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to load songs - module will continue with empty song list");
				}
			});
		}

		private void LoadSongsFromBundle()
		{
			try
			{
				Assembly assembly = ((object)this).GetType().Assembly;
				string resourceName = "Maestro.Data.songs.bin";
				using Stream stream = assembly.GetManifestResourceStream(resourceName);
				if (stream == null)
				{
					Logger.Warn("Could not find embedded resource: " + resourceName);
					return;
				}
				List<Song> songs = SongSerializer.DeserializeBundle(stream);
				_songs.AddRange(songs);
				Logger.Info($"Loaded {songs.Count} songs from embedded bundle");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load songs from embedded bundle");
			}
		}

		private void LoadSongsFromPath(string path)
		{
			if (!Directory.Exists(path))
			{
				Logger.Debug("Songs directory does not exist: " + path);
				return;
			}
			string[] jsonFiles = Directory.GetFiles(path, "*.json");
			Logger.Info($"Found {jsonFiles.Length} .json files in {path}");
			string[] array = jsonFiles;
			foreach (string file in array)
			{
				try
				{
					Song song = SongSerializer.DeserializeJson(file);
					_songs.Add(song);
					Logger.Debug("Loaded song: " + song.DisplayName);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to load song: " + file);
				}
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			CreateWindowBackground();
			try
			{
				Texture2D iconTexture = ContentsManager.GetTexture("icon.png");
				CornerIcon val = new CornerIcon();
				val.set_Icon(AsyncTexture2D.op_Implicit(iconTexture ?? Textures.get_Error()));
				((Control)val).set_BasicTooltipText("Maestro - Music Player");
				_cornerIcon = val;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load icon, using default");
				CornerIcon val2 = new CornerIcon();
				val2.set_Icon(AsyncTexture2D.op_Implicit(Textures.get_Error()));
				((Control)val2).set_BasicTooltipText("Maestro - Music Player");
				_cornerIcon = val2;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
			}
			((Module)this).OnModuleLoaded(e);
		}

		private void CreateWindowBackground()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDeviceContext graphicsDeviceContext = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				_windowBackground = new Texture2D(((GraphicsDeviceContext)(ref graphicsDeviceContext)).get_GraphicsDevice(), 419, 447);
				Color[] data = (Color[])(object)new Color[187293];
				for (int i = 0; i < data.Length; i++)
				{
					data[i] = new Color(30, 30, 30, 255);
				}
				_windowBackground.SetData<Color>(data);
			}
			finally
			{
				((GraphicsDeviceContext)(ref graphicsDeviceContext)).Dispose();
			}
		}

		private void OnCornerIconClick(object sender, MouseEventArgs e)
		{
			if (_maestroWindow == null)
			{
				_maestroWindow = new MaestroWindow(_windowBackground, _songPlayer, _songs);
			}
			((WindowBase2)_maestroWindow).ToggleWindow();
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			_songPlayer?.Stop();
			MaestroWindow maestroWindow = _maestroWindow;
			if (maestroWindow != null)
			{
				((Control)maestroWindow).Dispose();
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			Texture2D windowBackground = _windowBackground;
			if (windowBackground != null)
			{
				((GraphicsResource)windowBackground).Dispose();
			}
			Instance = null;
		}
	}
}
