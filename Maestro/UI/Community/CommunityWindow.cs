using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Community;
using Maestro.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Community
{
	public class CommunityWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 420;

			public const int WindowHeight = 350;

			public const int ContentWidth = 390;

			public const int FilterBarHeight = 32;

			public const int SongListHeight = 250;

			public const int StatusBarHeight = 30;

			public const int CardWidth = 378;
		}

		private static Texture2D _backgroundTexture;

		private readonly CommunityService _communityService;

		private readonly Dictionary<string, CommunitySongCard> _songCards;

		private TextBox _searchBox;

		private GenericFilterButton _filterButton;

		private FlowPanel _songListPanel;

		private Label _statusLabel;

		private StandardButton _refreshButton;

		private StandardButton _uploadButton;

		private LoadingSpinner _loadingSpinner;

		public event EventHandler<Song> SongDownloaded;

		public event EventHandler<string> SongDeleteRequested;

		public event EventHandler UploadRequested;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 350));
		}

		public CommunityWindow(CommunityService communityService)
			: this(GetBackground(), new Rectangle(0, 0, 420, 350), new Rectangle(15, 20, 390, 350))
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Expected O, but got Unknown
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Expected O, but got Unknown
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Expected O, but got Unknown
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Expected O, but got Unknown
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Expected O, but got Unknown
			int currentY = 2;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, currentY));
			((Control)val).set_Size(new Point(390, 32));
			((Control)val).set_BackgroundColor(Color.get_Transparent());
			Panel filterPanel = val;
			int uploadWidth = 70;
			int num = 390 - uploadWidth - 7;
			int searchWidth = (num - 7) / 2;
			int filterWidth = num - searchWidth - 7;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)filterPanel);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(searchWidth);
			((Control)val2).set_Height(26);
			((TextInputBase)val2).set_PlaceholderText("Search...");
			_searchBox = val2;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)OnFilterChanged);
			GenericFilterButton genericFilterButton = new GenericFilterButton(new FilterSection
			{
				Items = new string[5] { "All", "Piano", "Harp", "Lute", "Bass" },
				DefaultValue = "All"
			}, new FilterSection
			{
				Items = new string[3] { "Newest", "Name A-Z", "Name Z-A" },
				DefaultValue = "Newest"
			});
			((Control)genericFilterButton).set_Parent((Container)(object)filterPanel);
			((Control)genericFilterButton).set_Location(new Point(searchWidth + 7, 0));
			((Control)genericFilterButton).set_Width(filterWidth);
			_filterButton = genericFilterButton;
			_filterButton.FilterChanged += OnFilterChanged;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)filterPanel);
			val3.set_Text("Upload");
			((Control)val3).set_Location(new Point(390 - uploadWidth, 0));
			((Control)val3).set_Width(uploadWidth);
			_uploadButton = val3;
			((Control)_uploadButton).add_Click((EventHandler<MouseEventArgs>)OnUploadClicked);
			currentY += 39;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(0, currentY));
			((Control)val4).set_Size(new Point(390, 250));
			val4.set_FlowDirection((ControlFlowDirection)3);
			val4.set_ControlPadding(new Vector2(0f, 5f));
			((Panel)val4).set_CanScroll(true);
			((Panel)val4).set_ShowBorder(true);
			_songListPanel = val4;
			currentY += 257;
			int refreshWidth = 70;
			int spinnerSize = 26;
			LoadingSpinner val5 = new LoadingSpinner();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(390 - refreshWidth - 7 - spinnerSize, currentY));
			((Control)val5).set_Size(new Point(spinnerSize, spinnerSize));
			((Control)val5).set_Visible(false);
			_loadingSpinner = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Refresh");
			((Control)val6).set_Location(new Point(390 - refreshWidth, currentY));
			((Control)val6).set_Width(refreshWidth);
			((Control)val6).set_Height(26);
			_refreshButton = val6;
			((Control)_refreshButton).add_Click((EventHandler<MouseEventArgs>)OnRefreshClicked);
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)this);
			((Control)val7).set_Location(new Point(0, currentY));
			((Control)val7).set_Width(390 - refreshWidth - 7 - spinnerSize - 7);
			((Control)val7).set_Height(30);
			val7.set_Font(GameService.Content.get_DefaultFont12());
			val7.set_TextColor(MaestroTheme.LightGray);
			val7.set_Text("Loading...");
			_statusLabel = val7;
		}

		private void OnUploadClicked(object sender, MouseEventArgs e)
		{
			this.UploadRequested?.Invoke(this, EventArgs.Empty);
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
			string instrumentFilter = _filterButton?.SelectedValue1 ?? "All";
			string sortOption = _filterButton?.SelectedValue2 ?? "Newest";
			IEnumerable<CommunitySong> songs = _communityService.SearchSongs(searchTerm, instrumentFilter);
			switch (sortOption)
			{
			case "Newest":
				songs = songs.OrderByDescending((CommunitySong s) => s.CreatedAt);
				break;
			case "Name A-Z":
				songs = songs.OrderBy((CommunitySong s) => s.Name);
				break;
			case "Name Z-A":
				songs = songs.OrderByDescending((CommunitySong s) => s.Name);
				break;
			}
			foreach (CommunitySong song in songs.ToList())
			{
				bool isDownloaded = _communityService.IsSongDownloaded(song.Id);
				CommunitySongCard communitySongCard = new CommunitySongCard(song, 378, isDownloaded);
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
			((Control)_uploadButton).remove_Click((EventHandler<MouseEventArgs>)OnUploadClicked);
			TextBox searchBox = _searchBox;
			if (searchBox != null)
			{
				((Control)searchBox).Dispose();
			}
			GenericFilterButton filterButton = _filterButton;
			if (filterButton != null)
			{
				((Control)filterButton).Dispose();
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
			StandardButton uploadButton = _uploadButton;
			if (uploadButton != null)
			{
				((Control)uploadButton).Dispose();
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
