using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services;
using Maestro.Services.Playback;
using Maestro.UI.Playlist;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Main
{
	public class MaestroWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 420;

			public const int WindowHeight = 536;

			public const int ContentWidth = 390;
		}

		private static Texture2D _backgroundTexture;

		private readonly SongPlayer _songPlayer;

		private readonly List<Song> _allSongs;

		private readonly PlaylistService _playlistService;

		private readonly FavoriteService _favoriteService;

		private NowPlayingPanel _nowPlayingPanel;

		private SongFilterBar _filterBar;

		private SongListPanel _songListPanel;

		private StatusBar _statusBar;

		private PlaylistDrawerWindow _playlistDrawer;

		private bool _isDrawerOpen;

		private bool _isPlayingFromQueue;

		private InstrumentType? _lastPlayedInstrument;

		private Song _pendingSong;

		public event EventHandler ImportRequested;

		public event EventHandler CommunityRequested;

		public event EventHandler SupportRequested;

		public event EventHandler<InstrumentType> CreateRequested;

		public event EventHandler<Song> SongDeleteRequested;

		public event EventHandler<Song> EditRequested;

		public MaestroWindow(SongPlayer songPlayer, List<Song> songs, FavoriteService favoriteService)
			: this(GetBackground(), new Rectangle(0, 0, 420, 536), new Rectangle(15, 20, 390, 536))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			_songPlayer = songPlayer;
			_allSongs = songs;
			_playlistService = new PlaylistService();
			_favoriteService = favoriteService;
			((WindowBase2)this).set_Title("Maestro");
			((WindowBase2)this).set_Subtitle("Music player");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("maestro-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("MaestroWindow_v4");
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			BuildUi();
			SubscribeToEvents();
			((Control)this).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnWindowClicked);
		}

		public void AddImportedSong(Song song)
		{
			_allSongs.Add(song);
			_statusBar.TotalCount = _allSongs.Count;
			RefreshSongList();
		}

		public void RefreshAfterCommunityDownload()
		{
			_statusBar.TotalCount = _allSongs.Count;
			RefreshSongList();
		}

		public void SetCreateButtonEnabled(bool enabled)
		{
			_statusBar.SetCreateButtonEnabled(enabled);
		}

		public void SetImportActive(bool active)
		{
			_statusBar.SetImportActive(active);
		}

		public void SetCommunityActive(bool active)
		{
			_statusBar.SetCommunityActive(active);
		}

		public void SetSupportActive(bool active)
		{
			_statusBar.SetSupportActive(active);
		}

		public void RemoveSong(Song song)
		{
			if (_songPlayer.CurrentSong == song)
			{
				_songPlayer.Stop();
			}
			_allSongs.Remove(song);
			_statusBar.TotalCount = _allSongs.Count;
			RefreshSongList();
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			((Control)this).OnMoved(e);
			if (_isDrawerOpen)
			{
				UpdateDrawerPosition();
			}
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).OnHidden(e);
			PlaylistDrawerWindow playlistDrawer = _playlistDrawer;
			if (playlistDrawer != null)
			{
				((Control)playlistDrawer).Hide();
			}
		}

		protected override void OnShown(EventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnShown(e);
			if (_isDrawerOpen)
			{
				Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
				int targetX = ((Rectangle)(ref absoluteBounds)).get_Right() + 5;
				absoluteBounds = ((Control)this).get_AbsoluteBounds();
				int targetY = ((Rectangle)(ref absoluteBounds)).get_Top() + 35;
				_playlistDrawer.ShowWithAnimation(targetX, targetY);
			}
		}

		protected override void DisposeControl()
		{
			UnsubscribeFromEvents();
			_songPlayer.Stop();
			DisposeControls();
			((WindowBase2)this).DisposeControl();
		}

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 536));
		}

		private void BuildUi()
		{
			int currentY = 2;
			currentY = BuildNowPlayingPanel(currentY);
			currentY = BuildFilterBar(currentY);
			currentY = BuildSongListPanel(currentY);
			BuildStatusBar(currentY);
			BuildPlaylistDrawer();
			RefreshSongList();
		}

		private int BuildNowPlayingPanel(int currentY)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			NowPlayingPanel nowPlayingPanel = new NowPlayingPanel(_songPlayer, 390);
			((Control)nowPlayingPanel).set_Parent((Container)(object)this);
			((Control)nowPlayingPanel).set_Location(new Point(0, currentY));
			_nowPlayingPanel = nowPlayingPanel;
			_nowPlayingPanel.StopRequested += OnStopRequested;
			_nowPlayingPanel.PlayPendingRequested += OnPlayPendingRequested;
			_nowPlayingPanel.QueueToggleClicked += OnQueueToggleClicked;
			return currentY + 144 + 7;
		}

		private int BuildFilterBar(int currentY)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			SongFilterBar songFilterBar = new SongFilterBar(390);
			((Control)songFilterBar).set_Parent((Container)(object)this);
			((Control)songFilterBar).set_Location(new Point(0, currentY));
			_filterBar = songFilterBar;
			_filterBar.SearchChanged += OnFilterChanged;
			_filterBar.FilterChanged += OnFilterChanged;
			return currentY + 36 + 7 - 3;
		}

		private int BuildSongListPanel(int currentY)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			SongListPanel songListPanel = new SongListPanel(_songPlayer, 390);
			((Control)songListPanel).set_Parent((Container)(object)this);
			((Control)songListPanel).set_Location(new Point(0, currentY));
			_songListPanel = songListPanel;
			_songListPanel.SongPlayRequested += OnSongPlayRequested;
			_songListPanel.SongDeleteRequested += OnSongDeleteRequested;
			_songListPanel.EditRequested += OnEditRequested;
			_songListPanel.AddToQueueRequested += OnAddToQueueRequested;
			_songListPanel.FavoriteToggleRequested += OnFavoriteToggleRequested;
			_songListPanel.CountChanged += OnCountChanged;
			return currentY + 280 + 7;
		}

		private void BuildStatusBar(int currentY)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			StatusBar statusBar = new StatusBar(390);
			((Control)statusBar).set_Parent((Container)(object)this);
			((Control)statusBar).set_Location(new Point(0, currentY));
			_statusBar = statusBar;
			_statusBar.TotalCount = _allSongs.Count;
			_statusBar.ImportClicked += OnImportClicked;
			_statusBar.CommunityClicked += OnCommunityClicked;
			_statusBar.SupportClicked += OnSupportClicked;
			_statusBar.CreateClicked += OnCreateClicked;
		}

		private void BuildPlaylistDrawer()
		{
			PlaylistDrawerWindow playlistDrawerWindow = new PlaylistDrawerWindow(_playlistService);
			((Control)playlistDrawerWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)playlistDrawerWindow).set_Visible(false);
			_playlistDrawer = playlistDrawerWindow;
			((Control)_playlistDrawer).add_Hidden((EventHandler<EventArgs>)OnDrawerHidden);
			_playlistDrawer.PlayQueueRequested += OnPlayQueueRequested;
		}

		private void SubscribeToEvents()
		{
			_songPlayer.OnStarted += OnPlaybackStateChanged;
			_songPlayer.OnPaused += OnPlaybackStateChanged;
			_songPlayer.OnResumed += OnPlaybackStateChanged;
			_songPlayer.OnStopped += OnPlaybackStateChanged;
			_songPlayer.OnCompleted += OnPlaybackStateChanged;
			_songPlayer.OnCompleted += OnSongCompleted;
			_playlistService.QueueChanged += OnQueueChanged;
			_favoriteService.FavoritesChanged += OnFavoritesChanged;
			Module.Instance.PracticeActiveChanged += OnPracticeActiveChanged;
		}

		private void OnPracticeActiveChanged(object sender, EventArgs e)
		{
			RefreshPlayButtonState();
		}

		private void RefreshPlayButtonState()
		{
			_songListPanel.UpdateCardStates();
		}

		private bool BlockIfPracticeActive()
		{
			if (Module.Instance != null && Module.Instance.IsPracticeActive)
			{
				ScreenNotification.ShowNotification("Practice mode is running. Close it first to play normally.", (NotificationType)1, (Texture2D)null, 4);
				return true;
			}
			return false;
		}

		private void OnWindowClicked(object sender, MouseEventArgs e)
		{
			Control focusedControl = Control.get_FocusedControl();
			TextInputBase textInput = (TextInputBase)(object)((focusedControl is TextInputBase) ? focusedControl : null);
			if (textInput != null && !((Control)textInput).get_MouseOver())
			{
				textInput.set_Focused(false);
			}
		}

		private void OnPlaybackStateChanged(object sender, EventArgs e)
		{
			_songListPanel.UpdateCardStates();
		}

		private void OnFilterChanged(object sender, EventArgs e)
		{
			RefreshSongList();
		}

		private void OnSongPlayRequested(object sender, Song song)
		{
			if (!BlockIfPracticeActive())
			{
				if (_pendingSong != null)
				{
					_pendingSong = null;
					_nowPlayingPanel.ClearPendingSong();
					_playlistDrawer.HideInstrumentConfirmation();
				}
				PlaySongDirectly(song);
			}
		}

		private void OnSongDeleteRequested(object sender, Song song)
		{
			this.SongDeleteRequested?.Invoke(this, song);
		}

		private void OnEditRequested(object sender, Song song)
		{
			this.EditRequested?.Invoke(this, song);
		}

		private void OnCountChanged(object sender, int count)
		{
			_statusBar.VisibleCount = count;
		}

		private void OnImportClicked(object sender, EventArgs e)
		{
			this.ImportRequested?.Invoke(this, EventArgs.Empty);
		}

		private void OnCommunityClicked(object sender, EventArgs e)
		{
			this.CommunityRequested?.Invoke(this, EventArgs.Empty);
		}

		private void OnSupportClicked(object sender, EventArgs e)
		{
			this.SupportRequested?.Invoke(this, EventArgs.Empty);
		}

		private void OnCreateClicked(object sender, InstrumentType instrument)
		{
			this.CreateRequested?.Invoke(this, instrument);
		}

		private void OnQueueToggleClicked(object sender, EventArgs e)
		{
			ToggleDrawer();
		}

		private void OnAddToQueueRequested(object sender, Song song)
		{
			_playlistService.Add(song);
		}

		private void OnFavoriteToggleRequested(object sender, Song song)
		{
			_favoriteService.ToggleFavorite(song);
		}

		private void OnFavoritesChanged(object sender, EventArgs e)
		{
			HashSet<string> favoriteKeys = _favoriteService.GetAllFavoriteKeys();
			_songListPanel.UpdateFavoriteStates(favoriteKeys);
			if (_filterBar.SelectedSource == "Favorites")
			{
				RefreshSongList();
			}
		}

		private void OnQueueChanged(object sender, EventArgs e)
		{
		}

		private void OnDrawerHidden(object sender, EventArgs e)
		{
			_isDrawerOpen = false;
			_nowPlayingPanel.SetQueueActive(active: false);
			if (_pendingSong != null)
			{
				_pendingSong = null;
				_nowPlayingPanel.ClearPendingSong();
				if (_isPlayingFromQueue)
				{
					SetQueuePlaybackMode(isPlaying: false);
				}
			}
		}

		private void OnPlayPendingRequested(object sender, Song song)
		{
			if (!BlockIfPracticeActive())
			{
				_playlistDrawer.HideInstrumentConfirmation();
				if (!_isPlayingFromQueue)
				{
					SetQueuePlaybackMode(isPlaying: false);
				}
				if (!_playlistService.HasItems)
				{
					_isDrawerOpen = false;
					((Control)_playlistDrawer).Hide();
				}
				_lastPlayedInstrument = song.Instrument;
				_nowPlayingPanel.SetCurrentInstrument(song.Instrument);
				_pendingSong = null;
				_songPlayer.Play(song);
			}
		}

		private void ShowInstrumentConfirmation(Song song)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			_pendingSong = song;
			_nowPlayingPanel.SetPendingSong(song);
			if (!_isDrawerOpen)
			{
				_isDrawerOpen = true;
				_nowPlayingPanel.SetQueueActive(active: true);
				Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
				int targetX = ((Rectangle)(ref absoluteBounds)).get_Right() + 5;
				absoluteBounds = ((Control)this).get_AbsoluteBounds();
				int targetY = ((Rectangle)(ref absoluteBounds)).get_Top() + 35;
				_playlistDrawer.ShowWithAnimation(targetX, targetY);
			}
			_playlistDrawer.ShowInstrumentConfirmation(song.Instrument);
		}

		private void PlaySongDirectly(Song song)
		{
			SetQueuePlaybackMode(isPlaying: false);
			_lastPlayedInstrument = song.Instrument;
			_nowPlayingPanel.SetCurrentInstrument(song.Instrument);
			_songPlayer.Play(song);
		}

		private void OnSongCompleted(object sender, EventArgs e)
		{
			if (_isPlayingFromQueue)
			{
				AdvanceQueue();
			}
			else
			{
				SetQueuePlaybackMode(isPlaying: false);
			}
		}

		private void OnPlayQueueRequested(object sender, EventArgs e)
		{
			if (!BlockIfPracticeActive() && _playlistService.HasItems)
			{
				if (_isPlayingFromQueue)
				{
					AdvanceQueue();
					return;
				}
				_songPlayer.Stop();
				SetQueuePlaybackMode(isPlaying: true);
				_playlistService.StartPlayback();
				PlayCurrentFromQueue();
			}
		}

		private void OnStopRequested(object sender, EventArgs e)
		{
			SetQueuePlaybackMode(isPlaying: false);
			_songPlayer.Stop();
		}

		private void SetQueuePlaybackMode(bool isPlaying)
		{
			_isPlayingFromQueue = isPlaying;
			_playlistDrawer.SetQueuePlaybackMode(isPlaying);
			_nowPlayingPanel.SetQueuePlaybackMode(isPlaying);
		}

		private void AdvanceQueue()
		{
			if (_playlistService.MoveNext())
			{
				PlayCurrentFromQueue();
			}
			else
			{
				SetQueuePlaybackMode(isPlaying: false);
			}
		}

		private void PlayCurrentFromQueue()
		{
			Song song = _playlistService.Current;
			if (song == null)
			{
				SetQueuePlaybackMode(isPlaying: false);
				return;
			}
			Song pendingSong = _pendingSong;
			InstrumentType? currentInstrument = ((pendingSong != null) ? new InstrumentType?(pendingSong.Instrument) : _lastPlayedInstrument);
			if (currentInstrument.HasValue && song.Instrument != currentInstrument.Value)
			{
				ShowInstrumentConfirmation(song);
				return;
			}
			if (_pendingSong != null)
			{
				_pendingSong = null;
				_nowPlayingPanel.ClearPendingSong();
				_playlistDrawer.HideInstrumentConfirmation();
			}
			_lastPlayedInstrument = song.Instrument;
			_nowPlayingPanel.SetCurrentInstrument(song.Instrument);
			_songPlayer.Play(song);
		}

		private void ToggleDrawer()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			_isDrawerOpen = !_isDrawerOpen;
			if (_isDrawerOpen)
			{
				Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
				int targetX = ((Rectangle)(ref absoluteBounds)).get_Right() + 5;
				absoluteBounds = ((Control)this).get_AbsoluteBounds();
				int targetY = ((Rectangle)(ref absoluteBounds)).get_Top() + 35;
				_playlistDrawer.ShowWithAnimation(targetX, targetY);
			}
			else
			{
				((Control)_playlistDrawer).Hide();
			}
			_nowPlayingPanel.SetQueueActive(_isDrawerOpen);
		}

		private void UpdateDrawerPosition()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			if (_playlistDrawer != null)
			{
				PlaylistDrawerWindow playlistDrawer = _playlistDrawer;
				Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
				int num = ((Rectangle)(ref absoluteBounds)).get_Right() + 5;
				absoluteBounds = ((Control)this).get_AbsoluteBounds();
				((Control)playlistDrawer).set_Location(new Point(num, ((Rectangle)(ref absoluteBounds)).get_Top() + 35));
			}
		}

		private void RefreshSongList()
		{
			IEnumerable<Song> filteredSongs = GetFilteredSongs();
			_songListPanel.RefreshSongs(filteredSongs);
			HashSet<string> favoriteKeys = _favoriteService.GetAllFavoriteKeys();
			_songListPanel.UpdateFavoriteStates(favoriteKeys);
		}

		private IEnumerable<Song> GetFilteredSongs()
		{
			IEnumerable<Song> songs = _allSongs.AsEnumerable();
			songs = FilterBySource(songs);
			songs = FilterByInstrument(songs);
			songs = FilterBySearchTerm(songs);
			return ApplySort(songs);
		}

		private IEnumerable<Song> ApplySort(IEnumerable<Song> songs)
		{
			string sort = _filterBar.SelectedSort;
			if (!(sort == "Name Z-A"))
			{
				if (!(sort == "Name A-Z"))
				{
				}
				return songs.OrderBy((Song s) => s.Name);
			}
			return songs.OrderByDescending((Song s) => s.Name);
		}

		private IEnumerable<Song> FilterBySource(IEnumerable<Song> songs)
		{
			return _filterBar.SelectedSource switch
			{
				"Favorites" => songs.Where((Song s) => _favoriteService.IsFavorite(s)), 
				"Bundled" => songs.Where((Song s) => !s.IsUserImported && !s.IsCreated && !s.IsCommunityDownloaded), 
				"Community" => songs.Where((Song s) => s.IsCommunityDownloaded), 
				"Created" => songs.Where((Song s) => s.IsCreated), 
				"Imported" => songs.Where((Song s) => s.IsUserImported), 
				_ => songs, 
			};
		}

		private IEnumerable<Song> FilterByInstrument(IEnumerable<Song> songs)
		{
			string filter = _filterBar.SelectedInstrument;
			if (filter == "All")
			{
				return songs;
			}
			if (InstrumentCatalog.TryFromDisplayName(filter, out var instrument))
			{
				return songs.Where((Song s) => s.Instrument == instrument);
			}
			return songs;
		}

		private IEnumerable<Song> FilterBySearchTerm(IEnumerable<Song> songs)
		{
			string searchTerm = _filterBar.SearchText;
			if (string.IsNullOrEmpty(searchTerm))
			{
				return songs;
			}
			return songs.Where((Song s) => s.Name.ToLower().Contains(searchTerm) || s.Artist.ToLower().Contains(searchTerm));
		}

		private void UnsubscribeFromEvents()
		{
			_songPlayer.OnStarted -= OnPlaybackStateChanged;
			_songPlayer.OnPaused -= OnPlaybackStateChanged;
			_songPlayer.OnResumed -= OnPlaybackStateChanged;
			_songPlayer.OnStopped -= OnPlaybackStateChanged;
			_songPlayer.OnCompleted -= OnPlaybackStateChanged;
			_songPlayer.OnCompleted -= OnSongCompleted;
			_playlistService.QueueChanged -= OnQueueChanged;
			_favoriteService.FavoritesChanged -= OnFavoritesChanged;
			_filterBar.SearchChanged -= OnFilterChanged;
			_filterBar.FilterChanged -= OnFilterChanged;
			_songListPanel.SongPlayRequested -= OnSongPlayRequested;
			_songListPanel.SongDeleteRequested -= OnSongDeleteRequested;
			_songListPanel.EditRequested -= OnEditRequested;
			_songListPanel.AddToQueueRequested -= OnAddToQueueRequested;
			_songListPanel.FavoriteToggleRequested -= OnFavoriteToggleRequested;
			_songListPanel.CountChanged -= OnCountChanged;
			_statusBar.ImportClicked -= OnImportClicked;
			_statusBar.CommunityClicked -= OnCommunityClicked;
			_statusBar.CreateClicked -= OnCreateClicked;
			((Control)_playlistDrawer).remove_Hidden((EventHandler<EventArgs>)OnDrawerHidden);
			_playlistDrawer.PlayQueueRequested -= OnPlayQueueRequested;
			_nowPlayingPanel.StopRequested -= OnStopRequested;
			_nowPlayingPanel.PlayPendingRequested -= OnPlayPendingRequested;
			_nowPlayingPanel.QueueToggleClicked -= OnQueueToggleClicked;
			Module.Instance.PracticeActiveChanged -= OnPracticeActiveChanged;
		}

		private void DisposeControls()
		{
			NowPlayingPanel nowPlayingPanel = _nowPlayingPanel;
			if (nowPlayingPanel != null)
			{
				((Control)nowPlayingPanel).Dispose();
			}
			SongFilterBar filterBar = _filterBar;
			if (filterBar != null)
			{
				((Control)filterBar).Dispose();
			}
			SongListPanel songListPanel = _songListPanel;
			if (songListPanel != null)
			{
				((Control)songListPanel).Dispose();
			}
			StatusBar statusBar = _statusBar;
			if (statusBar != null)
			{
				((Control)statusBar).Dispose();
			}
			PlaylistDrawerWindow playlistDrawer = _playlistDrawer;
			if (playlistDrawer != null)
			{
				((Control)playlistDrawer).Dispose();
			}
		}
	}
}
