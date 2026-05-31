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

			public const int WindowHeight = 570;

			public const int ContentWidth = 390;

			public const int HeaderOffset = 20;

			public const int FilterBarHeight = 32;

			public const int SongListHeight = 450;

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
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 570));
		}

		public CommunityWindow(CommunityService communityService)
			: this(GetBackground(), new Rectangle(0, 0, 420, 570), new Rectangle(15, 20, 390, 570))
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

		private static string[] BuildInstrumentFilterItems()
		{
			List<string> list = new List<string>();
			list.Add("All");
			list.AddRange(InstrumentCatalog.Pickable.Select((InstrumentInfo i) => i.DisplayName));
			return list.ToArray();
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
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Expected O, but got Unknown
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Expected O, but got Unknown
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Expected O, but got Unknown
			int currentY = 22;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, currentY));
			((Control)val).set_Size(new Point(390, 32));
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
			GenericFilterButton genericFilterButton = new GenericFilterButton(new FilterSection
			{
				Items = BuildInstrumentFilterItems(),
				DefaultValue = "All"
			}, new FilterSection
			{
				Items = new string[3] { "Newest", "Name A-Z", "Name Z-A" },
				DefaultValue = "Newest"
			});
			((Control)genericFilterButton).set_Parent((Container)(object)filterPanel);
			((Control)genericFilterButton).set_Location(new Point(180, 0));
			((Control)genericFilterButton).set_Width(210);
			_filterButton = genericFilterButton;
			_filterButton.FilterChanged += OnFilterChanged;
			currentY += 39;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(0, currentY));
			((Control)val3).set_Size(new Point(390, 450));
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_ControlPadding(new Vector2(0f, 5f));
			((Panel)val3).set_CanScroll(true);
			((Panel)val3).set_ShowBorder(true);
			_songListPanel = val3;
			currentY += 457;
			int refreshWidth = 40;
			int uploadWidth = 40;
			int spinnerSize = 26;
			int spacing = 7;
			int uploadX = 390 - uploadWidth;
			int refreshX = uploadX - spacing - refreshWidth;
			int spinnerX = refreshX - spacing - spinnerSize;
			IconButton iconButton = new IconButton(MaestroIcons.Upload, MaestroTheme.IconGlyph);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_BasicTooltipText("Upload a song");
			((Control)iconButton).set_Location(new Point(uploadX, currentY));
			((Control)iconButton).set_Width(uploadWidth);
			((Control)iconButton).set_Height(26);
			_uploadButton = (StandardButton)(object)iconButton;
			((Control)_uploadButton).add_Click((EventHandler<MouseEventArgs>)OnUploadClicked);
			IconButton iconButton2 = new IconButton(MaestroIcons.Refresh, MaestroTheme.IconGlyph);
			((Control)iconButton2).set_Parent((Container)(object)this);
			((Control)iconButton2).set_BasicTooltipText("Refresh");
			((Control)iconButton2).set_Location(new Point(refreshX, currentY));
			((Control)iconButton2).set_Width(refreshWidth);
			((Control)iconButton2).set_Height(26);
			_refreshButton = (StandardButton)(object)iconButton2;
			((Control)_refreshButton).add_Click((EventHandler<MouseEventArgs>)OnRefreshClicked);
			LoadingSpinner val4 = new LoadingSpinner();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(spinnerX, currentY));
			((Control)val4).set_Size(new Point(spinnerSize, spinnerSize));
			((Control)val4).set_Visible(false);
			_loadingSpinner = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(0, currentY));
			((Control)val5).set_Width(spinnerX - spacing);
			((Control)val5).set_Height(30);
			val5.set_Font(GameService.Content.get_DefaultFont12());
			val5.set_TextColor(MaestroTheme.LightGray);
			val5.set_Text("Loading...");
			_statusLabel = val5;
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
