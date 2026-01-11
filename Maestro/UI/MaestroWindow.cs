using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Maestro.Models;
using Maestro.Services.Playback;
using Maestro.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI
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

		public event EventHandler<Song> SongDeleteRequested;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 495));
		}

		public MaestroWindow(SongPlayer songPlayer, List<Song> songs)
			: base(GetBackground(), new Rectangle(0, 0, 420, 495), new Rectangle(15, 30, 390, 445))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			_songPlayer = songPlayer;
			_allSongs = songs;
			base.Title = "Maestro";
			base.Subtitle = "Music player";
			base.Emblem = Module.Instance.ContentsManager.GetTexture("emblem.png");
			base.SavesPosition = true;
			base.Id = "MaestroWindow_v3";
			base.CanResize = false;
			base.Parent = GameService.Graphics.SpriteScreen;
			BuildUi();
			SubscribeToEvents();
		}

		private void BuildUi()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			int currentY = 2;
			_nowPlayingPanel = new NowPlayingPanel(_songPlayer, 390)
			{
				Parent = this,
				Location = new Point(0, currentY)
			};
			currentY += 102;
			_filterBar = new SongFilterBar(390)
			{
				Parent = this,
				Location = new Point(0, currentY)
			};
			_filterBar.SearchChanged += OnFilterChanged;
			_filterBar.FilterChanged += OnFilterChanged;
			currentY += 40;
			_songListPanel = new SongListPanel(_songPlayer, 390)
			{
				Parent = this,
				Location = new Point(0, currentY)
			};
			_songListPanel.SongPlayRequested += OnSongPlayRequested;
			_songListPanel.SongDeleteRequested += OnSongDeleteRequested;
			_songListPanel.CountChanged += OnCountChanged;
			currentY += 287;
			_statusBar = new StatusBar(390)
			{
				Parent = this,
				Location = new Point(0, currentY)
			};
			_statusBar.TotalCount = _allSongs.Count;
			_statusBar.ImportClicked += OnImportClicked;
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

		public void AddImportedSong(Song song)
		{
			_allSongs.Add(song);
			_statusBar.TotalCount = _allSongs.Count;
			RefreshSongList();
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
			string source = _filterBar.SelectedSource;
			if (source == "Bundled")
			{
				songs = songs.Where((Song s) => !s.IsUserImported);
			}
			else if (source == "Imported")
			{
				songs = songs.Where((Song s) => s.IsUserImported);
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
			_songPlayer.Stop();
			_nowPlayingPanel?.Dispose();
			_filterBar?.Dispose();
			_songListPanel?.Dispose();
			_statusBar?.Dispose();
			base.DisposeControl();
		}
	}
}
