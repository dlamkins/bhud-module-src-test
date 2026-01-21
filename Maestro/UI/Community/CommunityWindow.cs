using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Community;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Community
{
	public class CommunityWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 450;

			public const int WindowHeight = 405;

			public const int ContentWidth = 420;

			public const int ContentHeight = 380;

			public const int TopPadding = 10;

			public const int FilterBarHeight = 32;

			public const int SongListHeight = 283;

			public const int StatusBarHeight = 30;

			public const int CardWidth = 400;
		}

		private static Texture2D _backgroundTexture;

		private readonly CommunityService _communityService;

		private readonly Dictionary<string, CommunitySongCard> _songCards;

		private TextBox _searchBox;

		private Dropdown _instrumentFilter;

		private Dropdown _sortFilter;

		private FlowPanel _songListPanel;

		private Label _statusLabel;

		private StandardButton _refreshButton;

		private LoadingSpinner _loadingSpinner;

		public event EventHandler<Song> SongDownloaded;

		public event EventHandler<string> SongDeleteRequested;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(450, 405));
		}

		public CommunityWindow(CommunityService communityService)
			: this(GetBackground(), new Rectangle(0, 0, 450, 405), new Rectangle(15, 30, 420, 380))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			_communityService = communityService;
			_songCards = new Dictionary<string, CommunitySongCard>();
			((WindowBase2)this).set_Title("Community Songs");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("community-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("CommunityWindow_v1");
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
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Expected O, but got Unknown
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Expected O, but got Unknown
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Expected O, but got Unknown
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Expected O, but got Unknown
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Expected O, but got Unknown
			int currentY = 12;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, currentY));
			((Control)val).set_Size(new Point(420, 32));
			((Control)val).set_BackgroundColor(Color.get_Transparent());
			Panel filterPanel = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)filterPanel);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(150);
			((Control)val2).set_Height(26);
			((TextInputBase)val2).set_PlaceholderText("Search songs...");
			_searchBox = val2;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)OnFilterChanged);
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Parent((Container)(object)filterPanel);
			((Control)val3).set_Location(new Point(155, 0));
			((Control)val3).set_Width(75);
			_instrumentFilter = val3;
			_instrumentFilter.get_Items().Add("All");
			_instrumentFilter.get_Items().Add("Piano");
			_instrumentFilter.get_Items().Add("Harp");
			_instrumentFilter.get_Items().Add("Lute");
			_instrumentFilter.get_Items().Add("Bass");
			_instrumentFilter.set_SelectedItem("All");
			_instrumentFilter.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnFilterChanged);
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent((Container)(object)filterPanel);
			((Control)val4).set_Location(new Point(235, 0));
			((Control)val4).set_Width(80);
			_sortFilter = val4;
			_sortFilter.get_Items().Add("Popular");
			_sortFilter.get_Items().Add("Newest");
			_sortFilter.get_Items().Add("Name A-Z");
			_sortFilter.set_SelectedItem("Popular");
			_sortFilter.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnFilterChanged);
			LoadingSpinner val5 = new LoadingSpinner();
			((Control)val5).set_Parent((Container)(object)filterPanel);
			((Control)val5).set_Location(new Point(320, 0));
			((Control)val5).set_Size(new Point(26, 26));
			((Control)val5).set_Visible(false);
			_loadingSpinner = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)filterPanel);
			val6.set_Text("Refresh");
			((Control)val6).set_Location(new Point(350, 0));
			((Control)val6).set_Width(70);
			_refreshButton = val6;
			((Control)_refreshButton).add_Click((EventHandler<MouseEventArgs>)OnRefreshClicked);
			currentY += 39;
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)this);
			((Control)val7).set_Location(new Point(0, currentY));
			((Control)val7).set_Size(new Point(420, 283));
			val7.set_FlowDirection((ControlFlowDirection)3);
			val7.set_ControlPadding(new Vector2(0f, 5f));
			((Panel)val7).set_CanScroll(true);
			((Panel)val7).set_ShowBorder(true);
			_songListPanel = val7;
			currentY += 290;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Location(new Point(0, currentY));
			((Control)val8).set_Width(420);
			((Control)val8).set_Height(30);
			val8.set_Font(GameService.Content.get_DefaultFont12());
			val8.set_TextColor(MaestroTheme.LightGray);
			val8.set_Text("Loading...");
			_statusLabel = val8;
		}

		private void SubscribeToEvents()
		{
			_communityService.ManifestRefreshed += OnManifestRefreshed;
			_communityService.DownloadProgressChanged += OnDownloadProgressChanged;
		}

		private void OnManifestRefreshed(object sender, EventArgs e)
		{
			RefreshSongList();
		}

		private void OnDownloadProgressChanged(object sender, DownloadProgressEventArgs e)
		{
			if (_songCards.TryGetValue(e.CommunityId, out var card))
			{
				card.UpdateDownloadProgress(e.Progress, e.State);
			}
			if (e.State == DownloadState.Completed)
			{
				UpdateStatusLabel();
			}
		}

		private void OnFilterChanged(object sender, EventArgs e)
		{
			RefreshSongList();
		}

		private async void OnRefreshClicked(object sender, MouseEventArgs e)
		{
			((Control)_refreshButton).set_Enabled(false);
			((Control)_loadingSpinner).set_Visible(true);
			_statusLabel.set_Text("Refreshing...");
			try
			{
				await _communityService.RefreshManifestAsync();
			}
			finally
			{
				((Control)_refreshButton).set_Enabled(true);
				((Control)_loadingSpinner).set_Visible(false);
			}
		}

		public async void LoadContent()
		{
			((Control)_loadingSpinner).set_Visible(true);
			_statusLabel.set_Text("Loading community songs...");
			try
			{
				await _communityService.RefreshManifestAsync();
				RefreshSongList();
			}
			catch (Exception ex)
			{
				Logger.GetLogger<CommunityWindow>().Error(ex, "Failed to load community songs");
				_statusLabel.set_Text("Failed to load. Click Refresh to try again.");
			}
			finally
			{
				((Control)_loadingSpinner).set_Visible(false);
			}
		}

		private void RefreshSongList()
		{
			foreach (CommunitySongCard value in _songCards.Values)
			{
				((Control)value).Dispose();
			}
			_songCards.Clear();
			((Container)_songListPanel).ClearChildren();
			TextBox searchBox = _searchBox;
			string searchTerm = ((searchBox == null) ? null : ((TextInputBase)searchBox).get_Text()?.ToLower()) ?? "";
			Dropdown instrumentFilter2 = _instrumentFilter;
			string instrumentFilter = ((instrumentFilter2 != null) ? instrumentFilter2.get_SelectedItem() : null) ?? "All";
			Dropdown sortFilter = _sortFilter;
			string sortOption = ((sortFilter != null) ? sortFilter.get_SelectedItem() : null) ?? "Popular";
			IEnumerable<CommunitySong> songs = _communityService.SearchSongs(searchTerm, instrumentFilter);
			switch (sortOption)
			{
			case "Popular":
				songs = songs.OrderByDescending((CommunitySong s) => s.Downloads);
				break;
			case "Newest":
				songs = songs.OrderByDescending((CommunitySong s) => s.CreatedAt);
				break;
			case "Name A-Z":
				songs = songs.OrderBy((CommunitySong s) => s.Name);
				break;
			}
			foreach (CommunitySong song in songs.ToList())
			{
				bool isDownloaded = _communityService.IsSongDownloaded(song.Id);
				CommunitySongCard communitySongCard = new CommunitySongCard(song, 400, isDownloaded);
				((Control)communitySongCard).set_Parent((Container)(object)_songListPanel);
				CommunitySongCard card = communitySongCard;
				card.DownloadRequested += OnDownloadRequested;
				card.DeleteRequested += OnDeleteRequested;
				_songCards[song.Id] = card;
			}
			UpdateStatusLabel();
		}

		private async void OnDownloadRequested(object sender, CommunitySong song)
		{
			Song downloadedSong = await _communityService.DownloadSongAsync(song);
			if (downloadedSong != null)
			{
				this.SongDownloaded?.Invoke(this, downloadedSong);
				ScreenNotification.ShowNotification("Downloaded: " + downloadedSong.Name, (NotificationType)0, (Texture2D)null, 4);
			}
		}

		private void OnDeleteRequested(object sender, CommunitySong song)
		{
			this.SongDeleteRequested?.Invoke(this, song.Id);
		}

		public void MarkSongAsDeleted(string communityId)
		{
			if (_songCards.TryGetValue(communityId, out var card))
			{
				card.UpdateDownloadProgress(0, DownloadState.Idle);
				UpdateStatusLabel();
			}
		}

		private void UpdateStatusLabel()
		{
			int total = _communityService.GetAvailableSongs().Count();
			int displayed = _songCards.Count;
			int downloaded = _songCards.Values.Count((CommunitySongCard c) => c.IsDownloaded);
			_statusLabel.set_Text((displayed == total) ? $"{total} songs available | {downloaded} downloaded" : $"Showing {displayed} of {total} songs | {downloaded} downloaded");
		}

		protected override void DisposeControl()
		{
			_communityService.ManifestRefreshed -= OnManifestRefreshed;
			_communityService.DownloadProgressChanged -= OnDownloadProgressChanged;
			foreach (CommunitySongCard value in _songCards.Values)
			{
				value.DownloadRequested -= OnDownloadRequested;
				value.DeleteRequested -= OnDeleteRequested;
				((Control)value).Dispose();
			}
			_songCards.Clear();
			TextBox searchBox = _searchBox;
			if (searchBox != null)
			{
				((Control)searchBox).Dispose();
			}
			Dropdown instrumentFilter = _instrumentFilter;
			if (instrumentFilter != null)
			{
				((Control)instrumentFilter).Dispose();
			}
			Dropdown sortFilter = _sortFilter;
			if (sortFilter != null)
			{
				((Control)sortFilter).Dispose();
			}
			FlowPanel songListPanel = _songListPanel;
			if (songListPanel != null)
			{
				((Control)songListPanel).Dispose();
			}
			Label statusLabel = _statusLabel;
			if (statusLabel != null)
			{
				((Control)statusLabel).Dispose();
			}
			StandardButton refreshButton = _refreshButton;
			if (refreshButton != null)
			{
				((Control)refreshButton).Dispose();
			}
			LoadingSpinner loadingSpinner = _loadingSpinner;
			if (loadingSpinner != null)
			{
				((Control)loadingSpinner).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
