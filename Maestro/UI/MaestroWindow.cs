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
		public static class Layout
		{
			public const int ContentWidth = 390;

			public const int ContentHeight = 420;

			public const int ComponentGap = 5;

			public const int NowPlayingY = 0;

			public static int FilterBarY => 75;

			public static int SongListY => FilterBarY + 36 - 1;

			public static int StatusBarY => 396;

			public static int SongListHeight => 420 - SongListY - 24;
		}

		private static readonly Logger Logger = Logger.GetLogger<MaestroWindow>();

		private readonly SongPlayer _songPlayer;

		private readonly List<Song> _allSongs;

		private NowPlayingPanel _nowPlayingPanel;

		private SongFilterBar _filterBar;

		private SongListPanel _songListPanel;

		private StatusBar _statusBar;

		public MaestroWindow(Texture2D background, SongPlayer songPlayer, List<Song> songs)
			: base(background, new Rectangle(0, 0, 420, 460), new Rectangle(15, 30, 390, 420))
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			_nowPlayingPanel = new NowPlayingPanel(_songPlayer, 390)
			{
				Parent = this,
				Location = new Point(0, 0)
			};
			_filterBar = new SongFilterBar(390)
			{
				Parent = this,
				Location = new Point(0, Layout.FilterBarY)
			};
			_filterBar.SearchChanged += OnFilterChanged;
			_filterBar.FilterChanged += OnFilterChanged;
			_songListPanel = new SongListPanel(_songPlayer, 390, Layout.SongListHeight)
			{
				Parent = this,
				Location = new Point(0, Layout.SongListY)
			};
			_songListPanel.SongPlayRequested += OnSongPlayRequested;
			_songListPanel.CountChanged += OnCountChanged;
			_statusBar = new StatusBar(390)
			{
				Parent = this,
				Location = new Point(0, Layout.StatusBarY)
			};
			_statusBar.TotalCount = _allSongs.Count;
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

		private void RefreshSongList()
		{
			IEnumerable<Song> filteredSongs = GetFilteredSongs();
			_songListPanel.RefreshSongs(filteredSongs);
		}

		private IEnumerable<Song> GetFilteredSongs()
		{
			IEnumerable<Song> songs = _allSongs.AsEnumerable();
			string filter = _filterBar.SelectedInstrument;
			if (filter != "All Instruments")
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
			_songListPanel.CountChanged -= OnCountChanged;
			_songPlayer.Stop();
			_nowPlayingPanel?.Dispose();
			_filterBar?.Dispose();
			_songListPanel?.Dispose();
			_statusBar?.Dispose();
			base.DisposeControl();
		}
	}
}
