using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Maestro.Models;
using Maestro.Services.Community;
using Maestro.Services.Data;
using Maestro.Services.Playback;
using Maestro.Settings;
using Maestro.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private ModuleSettings _moduleSettings;

		private KeyboardService _keyboardService;

		private SongPlayer _songPlayer;

		private CommunitySongCache _songCache;

		private UserSongStorage _userSongStorage;

		private CommunityService _communityService;

		private MaestroWindow _maestroWindow;

		private ImportWindow _importWindow;

		private CommunityWindow _communityWindow;

		private CornerIcon _cornerIcon;

		private List<Song> _songs;

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
			_songCache = new CommunitySongCache(DirectoriesManager);
			_userSongStorage = new UserSongStorage(_songCache);
			if (Directory.Exists("C:\\git\\perso\\Maestro\\Songs"))
			{
				Logger.Info("Debug mode: Loading songs from directory");
				_songs = await SongLoader.LoadFromDirectoryAsync("C:\\git\\perso\\Maestro\\Songs");
			}
			else
			{
				Logger.Info("Production mode: Loading songs from ContentsManager");
				_songs = await SongLoader.LoadFromContentsManagerAsync(ContentsManager);
			}
			List<Song> userSongs = await _userSongStorage.LoadUserSongsAsync();
			_songs.AddRange(userSongs);
			_communityService = new CommunityService(_songCache, _songs);
			_communityService.LoadCachedSongsIntoMainList();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
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

		private void OnCornerIconClick(object sender, MouseEventArgs e)
		{
			if (_maestroWindow == null)
			{
				_maestroWindow = new MaestroWindow(_songPlayer, _songs);
				_maestroWindow.ImportRequested += OnImportRequested;
				_maestroWindow.CommunityRequested += OnCommunityRequested;
				_maestroWindow.SongDeleteRequested += OnSongDeleteRequested;
			}
			((WindowBase2)_maestroWindow).ToggleWindow();
		}

		private void OnImportRequested(object sender, EventArgs e)
		{
			if (_importWindow == null)
			{
				_importWindow = new ImportWindow();
				_importWindow.SongImported += OnSongImported;
			}
			if (((Control)_importWindow).get_Visible())
			{
				((Control)_importWindow).Hide();
			}
			else
			{
				((Control)_importWindow).Show();
			}
		}

		private void OnCommunityRequested(object sender, EventArgs e)
		{
			if (_communityWindow == null)
			{
				_communityWindow = new CommunityWindow(_communityService);
				_communityWindow.SongDownloaded += OnCommunitySongDownloaded;
				_communityWindow.SongDeleteRequested += OnCommunitySongDeleteRequested;
			}
			if (((Control)_communityWindow).get_Visible())
			{
				((Control)_communityWindow).Hide();
				return;
			}
			((Control)_communityWindow).Show();
			_communityWindow.LoadContent();
		}

		private void OnCommunitySongDownloaded(object sender, Song song)
		{
			_maestroWindow?.RefreshAfterCommunityDownload();
		}

		private void OnCommunitySongDeleteRequested(object sender, string communityId)
		{
			Song song = _songs.Find((Song s) => s.CommunityId == communityId);
			if (song != null)
			{
				OnSongDeleteRequested(this, song);
			}
		}

		private async void OnSongImported(object sender, Song song)
		{
			try
			{
				await _userSongStorage.SaveSongAsync(song);
				_maestroWindow?.AddImportedSong(song);
				Logger.Info("Imported and saved song: " + song.Name + " by " + song.Artist);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to save imported song: " + song.Name);
				ScreenNotification.ShowNotification("Failed to save song", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void OnSongDeleteRequested(object sender, Song song)
		{
			try
			{
				if (song.IsUserImported)
				{
					_userSongStorage.DeleteSong(song);
					_maestroWindow?.RemoveSong(song);
					Logger.Info("Deleted user song: " + song.Name + " by " + song.Artist);
				}
				else if (song.IsCommunityDownloaded)
				{
					_communityService.DeleteDownloadedSong(song);
					_maestroWindow?.RemoveSong(song);
					_communityWindow?.MarkSongAsDeleted(song.CommunityId);
					Logger.Info("Deleted community song: " + song.Name + " by " + song.Artist);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to delete song: " + song.Name);
				ScreenNotification.ShowNotification("Failed to delete song", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			_songPlayer?.Stop();
			CommunityWindow communityWindow = _communityWindow;
			if (communityWindow != null)
			{
				((Control)communityWindow).Dispose();
			}
			ImportWindow importWindow = _importWindow;
			if (importWindow != null)
			{
				((Control)importWindow).Dispose();
			}
			MaestroWindow maestroWindow = _maestroWindow;
			if (maestroWindow != null)
			{
				((Control)maestroWindow).Dispose();
			}
			_communityService?.Dispose();
			_songCache?.Dispose();
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			Instance = null;
		}
	}
}
