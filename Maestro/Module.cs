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
using Maestro.Services;
using Maestro.Services.Community;
using Maestro.Services.Data;
using Maestro.Services.Playback;
using Maestro.Settings;
using Maestro.UI.Community;
using Maestro.UI.Import;
using Maestro.UI.MaestroCreator;
using Maestro.UI.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private const int CORNER_ICON_PRIORITY = 1316531834;

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private ModuleSettings _moduleSettings;

		private KeyboardService _keyboardService;

		private SongPlayer _songPlayer;

		private SongStorage _songStorage;

		private FavoriteService _favoriteService;

		private CommunityService _communityService;

		private CommunityUploadService _uploadService;

		private UploadRateLimiter _uploadRateLimiter;

		private MaestroWindow _maestroWindow;

		private ImportWindow _importWindow;

		private CommunityWindow _communityWindow;

		private UploadWindow _uploadWindow;

		private MaestroCreatorWindow _maestroCreatorWindow;

		private CornerIcon _cornerIcon;

		private List<Song> _songs;

		private Song _editingOriginalSong;

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
			_songStorage = new SongStorage(DirectoriesManager);
			_favoriteService = new FavoriteService(_songStorage.Database);
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
			List<Song> userSongs = _songStorage.GetAllSongs();
			Logger.Info($"Loaded {userSongs.Count} user songs from storage");
			_songs.AddRange(userSongs);
			_communityService = new CommunityService(_songStorage, _songs);
			_uploadRateLimiter = new UploadRateLimiter(_songStorage.Database);
			_uploadRateLimiter.CleanupOldRecords();
			_uploadService = new CommunityUploadService(new CommunityApiClient(_moduleSettings.ClientId), _communityService, _uploadRateLimiter, _songStorage);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			try
			{
				Texture2D iconTexture = ContentsManager.GetTexture("icon.png");
				CornerIcon val = new CornerIcon();
				val.set_Icon(AsyncTexture2D.op_Implicit(iconTexture ?? Textures.get_Error()));
				((Control)val).set_BasicTooltipText("Maestro - Music Player");
				val.set_Priority(1316531834);
				_cornerIcon = val;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load icon, using default");
				CornerIcon val2 = new CornerIcon();
				val2.set_Icon(AsyncTexture2D.op_Implicit(Textures.get_Error()));
				((Control)val2).set_BasicTooltipText("Maestro - Music Player");
				val2.set_Priority(1316531834);
				_cornerIcon = val2;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
			}
			((Module)this).OnModuleLoaded(e);
		}

		private void OnCornerIconClick(object sender, MouseEventArgs e)
		{
			if (_maestroWindow == null)
			{
				_maestroWindow = new MaestroWindow(_songPlayer, _songs, _favoriteService);
				_maestroWindow.ImportRequested += OnImportRequested;
				_maestroWindow.CommunityRequested += OnCommunityRequested;
				_maestroWindow.CreateRequested += OnCreateRequested;
				_maestroWindow.SongDeleteRequested += OnSongDeleteRequested;
				_maestroWindow.EditRequested += OnEditRequested;
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
				_communityWindow.UploadRequested += OnUploadRequested;
			}
			if (((Control)_communityWindow).get_Visible())
			{
				((Control)_communityWindow).Hide();
				return;
			}
			((Control)_communityWindow).Show();
			_communityWindow.LoadContent();
		}

		private void OnCreateRequested(object sender, InstrumentType instrument)
		{
			if (_maestroCreatorWindow == null)
			{
				_maestroCreatorWindow = new MaestroCreatorWindow();
				_maestroCreatorWindow.SongCreated += OnPianoSongCreated;
				_maestroCreatorWindow.SongEdited += OnSongEdited;
				_maestroCreatorWindow.WindowClosed += OnCreatorWindowClosed;
			}
			_maestroCreatorWindow.SetInstrument(instrument);
			if (((Control)_maestroCreatorWindow).get_Visible())
			{
				((Control)_maestroCreatorWindow).Hide();
				return;
			}
			_maestroWindow?.SetCreateButtonEnabled(enabled: false);
			((Control)_maestroCreatorWindow).Show();
		}

		private void OnCreatorWindowClosed(object sender, EventArgs e)
		{
			_maestroWindow?.SetCreateButtonEnabled(enabled: true);
		}

		private void OnEditRequested(object sender, Song song)
		{
			if (_maestroCreatorWindow == null)
			{
				_maestroCreatorWindow = new MaestroCreatorWindow();
				_maestroCreatorWindow.SongCreated += OnPianoSongCreated;
				_maestroCreatorWindow.SongEdited += OnSongEdited;
				_maestroCreatorWindow.WindowClosed += OnCreatorWindowClosed;
			}
			_editingOriginalSong = song;
			_maestroCreatorWindow.LoadSong(song);
			_maestroWindow?.SetCreateButtonEnabled(enabled: false);
			((Control)_maestroCreatorWindow).Show();
		}

		private void OnSongEdited(object sender, Song editedSong)
		{
			try
			{
				if (_editingOriginalSong != null)
				{
					_songStorage.DeleteSong(_editingOriginalSong);
					int index = _songs.IndexOf(_editingOriginalSong);
					if (index >= 0)
					{
						_songs[index] = editedSong;
					}
					else
					{
						_songs.Add(editedSong);
					}
					_editingOriginalSong = null;
				}
				else
				{
					_songs.Add(editedSong);
				}
				_songStorage.SaveSong(editedSong);
				_maestroWindow?.RefreshAfterCommunityDownload();
				_uploadWindow?.RefreshSongList();
				Logger.Info("Edited and saved song: " + editedSong.Name + " by " + editedSong.Artist);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to save edited song: " + editedSong.Name);
				ScreenNotification.ShowNotification("Failed to save song", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void OnPianoSongCreated(object sender, Song song)
		{
			try
			{
				_songStorage.SaveSong(song);
				_maestroWindow?.AddImportedSong(song);
				_uploadWindow?.RefreshSongList();
				Logger.Info("Created and saved song: " + song.Name + " by " + song.Artist);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to save created song: " + song.Name);
				ScreenNotification.ShowNotification("Failed to save song", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		public void PlayNote(string note, bool isSharp = false, bool isHighC = false)
		{
			_keyboardService?.PlayNoteByName(note, isSharp, isHighC);
		}

		public void PlayOctaveChange(bool up)
		{
			_keyboardService?.PlayOctaveChange(up);
		}

		public void PauseIfPlaying()
		{
			if (_songPlayer.IsPlaying && !_songPlayer.IsPaused)
			{
				_songPlayer.Pause();
			}
		}

		public void ResetToMiddleOctave()
		{
			_keyboardService?.ResetToMiddleOctave();
		}

		public void PreviewSong(Song song)
		{
			_songPlayer?.Play(song);
		}

		private void OnUploadRequested(object sender, EventArgs e)
		{
			if (_uploadWindow == null)
			{
				_uploadWindow = new UploadWindow(_uploadService, _songs);
				_uploadWindow.UploadCompleted += OnUploadCompleted;
			}
			if (((Control)_uploadWindow).get_Visible())
			{
				((Control)_uploadWindow).Hide();
			}
			else
			{
				((Control)_uploadWindow).Show();
			}
		}

		private void OnUploadCompleted(object sender, UploadResponse response)
		{
			if (response.Success)
			{
				Logger.Info("Song upload complete. Song ID: " + response.SongId);
			}
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

		private void OnSongImported(object sender, Song song)
		{
			try
			{
				_songStorage.SaveSong(song);
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
				_communityService.DeleteDownloadedSong(song);
				_maestroWindow?.RemoveSong(song);
				if (!string.IsNullOrEmpty(song.CommunityId))
				{
					_communityWindow?.MarkSongAsDeleted(song.CommunityId);
				}
				Logger.Info("Deleted song: " + song.Name + " by " + song.Artist);
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
			MaestroCreatorWindow maestroCreatorWindow = _maestroCreatorWindow;
			if (maestroCreatorWindow != null)
			{
				((Control)maestroCreatorWindow).Dispose();
			}
			UploadWindow uploadWindow = _uploadWindow;
			if (uploadWindow != null)
			{
				((Control)uploadWindow).Dispose();
			}
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
			_songStorage?.Dispose();
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			Instance = null;
		}
	}
}
