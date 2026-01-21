using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Playback;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Main
{
	public class MaestroWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 420;

			public const int WindowHeight = 495;

			public const int ContentWidth = 390;

			public const int ContentHeight = 445;
		}

		private readonly SongPlayer _songPlayer;

		private readonly List<Song> _allSongs;

		private NowPlayingPanel _nowPlayingPanel;

		private SongFilterBar _filterBar;

		private SongListPanel _songListPanel;

		private StatusBar _statusBar;

		private static Texture2D _backgroundTexture;

		public event EventHandler ImportRequested;

		public event EventHandler CommunityRequested;

		public event EventHandler<InstrumentType> CreateRequested;

		public event EventHandler<Song> SongDeleteRequested;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 495));
		}

		public MaestroWindow(SongPlayer songPlayer, List<Song> songs)
			: this(GetBackground(), new Rectangle(0, 0, 420, 495), new Rectangle(15, 30, 390, 445))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			_songPlayer = songPlayer;
			_allSongs = songs;
			((WindowBase2)this).set_Title("Maestro");
			((WindowBase2)this).set_Subtitle("Music player");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("maestro-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("MaestroWindow_v3");
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			BuildUi();
			SubscribeToEvents();
			((Control)this).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnWindowClicked);
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

		private void BuildUi()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			int currentY = 2;
			NowPlayingPanel nowPlayingPanel = new NowPlayingPanel(_songPlayer, 390);
			((Control)nowPlayingPanel).set_Parent((Container)(object)this);
			((Control)nowPlayingPanel).set_Location(new Point(0, currentY));
			_nowPlayingPanel = nowPlayingPanel;
			currentY += 102;
			SongFilterBar songFilterBar = new SongFilterBar(390);
			((Control)songFilterBar).set_Parent((Container)(object)this);
			((Control)songFilterBar).set_Location(new Point(0, currentY));
			_filterBar = songFilterBar;
			_filterBar.SearchChanged += OnFilterChanged;
			_filterBar.FilterChanged += OnFilterChanged;
			currentY += 40;
			SongListPanel songListPanel = new SongListPanel(_songPlayer, 390);
			((Control)songListPanel).set_Parent((Container)(object)this);
			((Control)songListPanel).set_Location(new Point(0, currentY));
			_songListPanel = songListPanel;
			_songListPanel.SongPlayRequested += OnSongPlayRequested;
			_songListPanel.SongDeleteRequested += OnSongDeleteRequested;
			_songListPanel.CountChanged += OnCountChanged;
			currentY += 287;
			StatusBar statusBar = new StatusBar(390);
			((Control)statusBar).set_Parent((Container)(object)this);
			((Control)statusBar).set_Location(new Point(0, currentY));
			_statusBar = statusBar;
			_statusBar.TotalCount = _allSongs.Count;
			_statusBar.ImportClicked += OnImportClicked;
			_statusBar.CommunityClicked += OnCommunityClicked;
			_statusBar.CreateClicked += OnCreateClicked;
			RefreshSongList();
		}

		private void SubscribeToEvents()
		{
			_songPlayer.OnStarted += OnPlaybackStateChanged;
			_songPlayer.OnPaused += OnPlaybackStateChanged;
			_songPlayer.OnResumed += OnPlaybackStateChanged;
			_songPlayer.OnStopped += OnPlaybackStateChanged;
			_songPlayer.OnCompleted += OnPlaybackStateChanged;
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
			_songPlayer.Play(song);
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

		private void OnCreateClicked(object sender, InstrumentType instrument)
		{
			this.CreateRequested?.Invoke(this, instrument);
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

		private void OnSongDeleteRequested(object sender, Song song)
		{
			this.SongDeleteRequested?.Invoke(this, song);
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

		private void RefreshSongList()
		{
			IEnumerable<Song> filteredSongs = GetFilteredSongs();
			_songListPanel.RefreshSongs(filteredSongs);
		}

		private IEnumerable<Song> GetFilteredSongs()
		{
			IEnumerable<Song> songs = _allSongs.AsEnumerable();
			switch (_filterBar.SelectedSource)
			{
			case "Bundled":
				songs = songs.Where((Song s) => !s.IsUserImported && !s.IsCommunityDownloaded);
				break;
			case "Community":
				songs = songs.Where((Song s) => s.IsCommunityDownloaded);
				break;
			case "Imported":
				songs = songs.Where((Song s) => s.IsUserImported);
				break;
			}
			string filter = _filterBar.SelectedInstrument;
			if (filter != "All")
			{
				if (Enum.TryParse<InstrumentType>(filter, out var instrument))
				{
					songs = songs.Where((Song s) => s.Instrument == instrument);
				}
			}
			string searchTerm = _filterBar.SearchText;
			if (!string.IsNullOrEmpty(searchTerm))
			{
				songs = songs.Where((Song s) => s.Name.ToLower().Contains(searchTerm) || s.Artist.ToLower().Contains(searchTerm));
			}
			return songs.OrderBy((Song s) => s.Name);
		}

		protected override void DisposeControl()
		{
			_songPlayer.OnStarted -= OnPlaybackStateChanged;
			_songPlayer.OnPaused -= OnPlaybackStateChanged;
			_songPlayer.OnResumed -= OnPlaybackStateChanged;
			_songPlayer.OnStopped -= OnPlaybackStateChanged;
			_songPlayer.OnCompleted -= OnPlaybackStateChanged;
			_filterBar.SearchChanged -= OnFilterChanged;
			_filterBar.FilterChanged -= OnFilterChanged;
			_songListPanel.SongPlayRequested -= OnSongPlayRequested;
			_songListPanel.SongDeleteRequested -= OnSongDeleteRequested;
			_songListPanel.CountChanged -= OnCountChanged;
			_statusBar.ImportClicked -= OnImportClicked;
			_statusBar.CommunityClicked -= OnCommunityClicked;
			_statusBar.CreateClicked -= OnCreateClicked;
			_songPlayer.Stop();
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
			((WindowBase2)this).DisposeControl();
		}
	}
}
