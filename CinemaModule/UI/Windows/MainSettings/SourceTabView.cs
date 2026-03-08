using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using CinemaModule.Controllers;
using CinemaModule.Models;
using CinemaModule.Models.Twitch;
using CinemaModule.Services;
using CinemaModule.Services.Twitch;
using CinemaModule.Services.YouTube;
using CinemaModule.Settings;
using CinemaModule.UI.Windows.Dialogs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Windows.MainSettings
{
	public class SourceTabView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<SourceTabView>();

		private const int MenuPanelWidth = 240;

		private const int CardVerticalSpacing = 4;

		private const int VerticalPadding = 110;

		private const string KeyPrefixChannel = "channel:";

		private const string KeyPrefixPresetTwitch = "preset_twitch:";

		private const string KeyPrefixSaved = "saved:";

		private const string KeyPrefixFollowed = "followed:";

		private const string KeyPrefixCustomTab = "customtab:";

		private const string CategoryFollowed = "Followed Channels";

		private readonly CinemaUserSettings _settings;

		private readonly CinemaController _controller;

		private readonly TwitchService _twitchService;

		private readonly TwitchAuthService _twitchAuthService;

		private readonly PresetService _presetService;

		private readonly YouTubeService _youtubeService;

		private Menu _categoryMenu;

		private Panel _menuPanel;

		private Panel _contentContainer;

		private Panel _headerSection;

		private FlowPanel _cardsPanel;

		private readonly Dictionary<string, ListCard> _streamCards = new Dictionary<string, ListCard>();

		private string _selectedStreamKey;

		private string _selectedCategoryId;

		private StreamEditorWindow _editorWindow;

		private TwitchAuthWindow _twitchAuthWindow;

		private CancellationTokenSource _contentCts;

		private CancellationTokenSource _selectionCts;

		private readonly Dictionary<string, StreamCategory> _categoryLookup = new Dictionary<string, StreamCategory>();

		private readonly Dictionary<string, MenuItem> _customTabMenuItems = new Dictionary<string, MenuItem>();

		private StreamCardFactory _cardFactory;

		private StreamStatusLoader _statusLoader;

		private Panel _addTabPanel;

		private TextBox _editingTextBox;

		private string _editingTabId;

		public SourceTabView(CinemaUserSettings settings, CinemaController controller, TwitchService twitchService, TwitchAuthService twitchAuthService, PresetService presetService, YouTubeService youtubeService)
			: this()
		{
			_settings = settings;
			_controller = controller;
			_twitchService = twitchService;
			_twitchAuthService = twitchAuthService;
			_presetService = presetService;
			_youtubeService = youtubeService;
		}

		protected override void Build(Container buildPanel)
		{
			InitializeHelpers();
			InitializeSelectedStreamKey();
			BuildCategoryMenu(buildPanel);
			BuildContentPanel(buildPanel);
			SubscribeToEvents();
			SelectInitialCategory();
		}

		private void InitializeHelpers()
		{
			_cardFactory = new StreamCardFactory(CinemaModule.Instance.TextureService, _twitchService, () => _selectedStreamKey, _streamCards)
			{
				OnOpenChat = OpenTwitchChat,
				OnCopyWaypoint = CopyWaypointToClipboard,
				OnApplyWorldPosition = ApplyWorldPosition
			};
			_statusLoader = new StreamStatusLoader(_twitchService, _youtubeService);
		}

		private void SubscribeToEvents()
		{
			_settings.SavedStreamsChanged += OnSavedStreamsChanged;
			_presetService.PresetsLoaded += OnPresetsLoaded;
			_twitchAuthService.AuthStatusChanged += OnTwitchAuthStatusChanged;
		}

		private void OnSavedStreamsChanged(object s, EventArgs e)
		{
			RefreshContent();
		}

		private void OnPresetsLoaded(object s, EventArgs e)
		{
			RefreshContent();
		}

		private void OnTwitchAuthStatusChanged(object s, TwitchAuthStatusEventArgs e)
		{
			if (_selectedCategoryId == "Followed Channels")
			{
				RefreshContent();
			}
		}

		private void BuildCategoryMenu(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_ShowBorder(true);
			((Control)val).set_Size(new Point(240, ((Control)parent).get_Height() - 110));
			((Control)val).set_Location(new Point(23, 10));
			val.set_Title("Categories");
			((Control)val).set_Parent(parent);
			val.set_CanScroll(true);
			_menuPanel = val;
			Image val2 = new Image();
			val2.set_Texture(CinemaModule.Instance.TextureService.GetRefreshIcon());
			((Control)val2).set_Size(new Point(24, 24));
			((Control)val2).set_Location(new Point(229, 16));
			((Control)val2).set_BasicTooltipText("Refresh categories and stream status");
			((Control)val2).set_Opacity(0.6f);
			((Control)val2).set_Parent(parent);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await RefreshAllAsync();
			});
			Menu val3 = new Menu();
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			val3.set_MenuItemHeight(50);
			((Control)val3).set_Parent((Container)(object)_menuPanel);
			val3.set_CanSelect(true);
			_categoryMenu = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Size(new Point(((Container)_menuPanel).get_ContentRegion().Width, 36));
			((Control)val4).set_Parent((Container)(object)_menuPanel);
			_addTabPanel = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("+ Add Category");
			((Control)val5).set_Size(new Point(((Container)_menuPanel).get_ContentRegion().Width - 10, 28));
			((Control)val5).set_Location(new Point(5, 4));
			((Control)val5).set_Parent((Container)(object)_addTabPanel);
			((Control)val5).set_BasicTooltipText("Create a custom category to manage your own streams and videos");
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				StartAddingNewTab();
			});
			PopulateCategoryMenu();
			_categoryMenu.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)OnCategorySelected);
			((Control)parent).add_Resized((EventHandler<ResizedEventArgs>)OnParentResized);
		}

		private void PopulateCategoryMenu()
		{
			((Container)_categoryMenu).ClearChildren();
			_categoryLookup.Clear();
			_customTabMenuItems.Clear();
			foreach (StreamCategory category in _presetService.StreamCategories)
			{
				_categoryMenu.AddMenuItem(category.Name, (Texture2D)null).set_Icon(category.IconTexture);
				_categoryLookup[category.Name] = category;
			}
			_categoryMenu.AddMenuItem("Followed Channels", (Texture2D)null).set_Icon(CinemaModule.Instance.TextureService.GetTwitchBigIcon());
			foreach (CustomStreamTab tab in _settings.SavedStreams.Tabs)
			{
				MenuItem tabItem = _categoryMenu.AddMenuItem(tab.Name, (Texture2D)null);
				tabItem.set_Icon(CinemaModule.Instance.TextureService.GetEmblem());
				((Control)tabItem).set_BasicTooltipText("Custom category: " + tab.Name);
				_customTabMenuItems[tab.Id] = tabItem;
			}
			UpdateAddTabPanelPosition();
		}

		private void UpdateAddTabPanelPosition()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			int menuHeight = ((IEnumerable<Control>)((Container)_categoryMenu).get_Children()).Count() * _categoryMenu.get_MenuItemHeight();
			((Control)_addTabPanel).set_Location(new Point(0, menuHeight));
		}

		private void BuildContentPanel(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Size(new Point(((Control)parent).get_Width() - 240 - 90, ((Control)parent).get_Height() - 110));
			((Control)val).set_Location(new Point(269, 10));
			val.set_ShowBorder(true);
			((Control)val).set_Parent(parent);
			_contentContainer = val;
			Panel val2 = new Panel();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Height(0);
			((Control)val2).set_Parent((Container)(object)_contentContainer);
			_headerSection = val2;
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Control)val3).set_Size(new Point(((Container)_contentContainer).get_ContentRegion().Width, ((Container)_contentContainer).get_ContentRegion().Height));
			((Control)val3).set_Location(new Point(0, 0));
			val3.set_ControlPadding(new Vector2(0f, 4f));
			((Panel)val3).set_CanScroll(true);
			((Control)val3).set_Parent((Container)(object)_contentContainer);
			_cardsPanel = val3;
		}

		private void OnParentResized(object sender, ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Container val = (Container)sender;
			int newHeight = ((Control)val).get_Height() - 110;
			int newWidth = ((Control)val).get_Width() - 240 - 90;
			((Control)_menuPanel).set_Height(newHeight);
			((Control)_contentContainer).set_Size(new Point(newWidth, newHeight));
			((Control)_cardsPanel).set_Width(((Container)_contentContainer).get_ContentRegion().Width);
			UpdateCardsPanelLayout();
		}

		private void SelectInitialCategory()
		{
			string initialCategory = DetermineInitialCategory();
			MenuItem menuItem;
			if (initialCategory.StartsWith("customtab:"))
			{
				string tabId = initialCategory.Substring("customtab:".Length);
				_customTabMenuItems.TryGetValue(tabId, out menuItem);
			}
			else
			{
				menuItem = ((IEnumerable)((Container)_categoryMenu).get_Children()).OfType<MenuItem>().FirstOrDefault((MenuItem m) => m.get_Text() == initialCategory);
			}
			if (menuItem != null)
			{
				_categoryMenu.Select(menuItem);
			}
		}

		private string DetermineInitialCategory()
		{
			string lastCategory = _settings.LastSelectedSourceCategory;
			if (!string.IsNullOrEmpty(lastCategory))
			{
				if (_categoryLookup.ContainsKey(lastCategory) || lastCategory == "Followed Channels")
				{
					return lastCategory;
				}
				if (lastCategory.StartsWith("customtab:"))
				{
					string tabId = lastCategory.Substring("customtab:".Length);
					if (_settings.SavedStreams.Tabs.Exists((CustomStreamTab t) => t.Id == tabId))
					{
						return lastCategory;
					}
				}
			}
			return _presetService.StreamCategories.FirstOrDefault()?.Name ?? "Followed Channels";
		}

		private void OnCategorySelected(object sender, ControlActivatedEventArgs e)
		{
			Control activatedControl = e.get_ActivatedControl();
			MenuItem menuItem = (MenuItem)(object)((activatedControl is MenuItem) ? activatedControl : null);
			if (menuItem != null)
			{
				string tabId = GetTabIdFromMenuItem(menuItem);
				_selectedCategoryId = ((tabId != null) ? ("customtab:" + tabId) : menuItem.get_Text());
				_settings.LastSelectedSourceCategory = _selectedCategoryId;
				RefreshContent();
			}
		}

		private string GetTabIdFromMenuItem(MenuItem menuItem)
		{
			return _customTabMenuItems.FirstOrDefault((KeyValuePair<string, MenuItem> kvp) => kvp.Value == menuItem).Key;
		}

		private async Task RefreshAllAsync()
		{
			_youtubeService.ClearVideoInfoCache();
			await _presetService.LoadPresetsAsync();
			PopulateCategoryMenu();
			ReselectCurrentCategory();
			RefreshContent();
		}

		private void ReselectCurrentCategory()
		{
			if (string.IsNullOrEmpty(_selectedCategoryId))
			{
				return;
			}
			MenuItem menuItem;
			if (_selectedCategoryId.StartsWith("customtab:"))
			{
				string tabId = _selectedCategoryId.Substring("customtab:".Length);
				_customTabMenuItems.TryGetValue(tabId, out menuItem);
			}
			else
			{
				menuItem = ((IEnumerable)((Container)_categoryMenu).get_Children()).OfType<MenuItem>().FirstOrDefault((MenuItem m) => m.get_Text() == _selectedCategoryId);
			}
			if (menuItem != null)
			{
				_categoryMenu.Select(menuItem);
			}
		}

		private void UpdateCardsPanelLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			((Control)_cardsPanel).set_Location(new Point(0, ((Control)_headerSection).get_Height()));
			((Control)_cardsPanel).set_Height(((Container)_contentContainer).get_ContentRegion().Height - ((Control)_headerSection).get_Height());
		}

		private void RefreshContent()
		{
			_contentCts?.Cancel();
			_contentCts?.Dispose();
			_contentCts = new CancellationTokenSource();
			_streamCards.Clear();
			((Container)_headerSection).ClearChildren();
			((Control)_headerSection).set_Height(0);
			((Container)_cardsPanel).ClearChildren();
			UpdateCardsPanelLayout();
			if (_selectedCategoryId == "Followed Channels")
			{
				LoadFollowedContentAsync(_contentCts.Token);
				return;
			}
			if (IsCustomTab(_selectedCategoryId))
			{
				LoadCustomTabContent(_contentCts.Token);
				return;
			}
			StreamCategory category = _presetService.StreamCategories.FirstOrDefault((StreamCategory c) => c.Name == _selectedCategoryId);
			if (category != null)
			{
				LoadCategoryContentAsync(category, _contentCts.Token);
			}
		}

		private bool IsCustomTab(string categoryId)
		{
			return categoryId?.StartsWith("customtab:") ?? false;
		}

		private CustomStreamTab GetCurrentCustomTab()
		{
			if (!IsCustomTab(_selectedCategoryId))
			{
				return null;
			}
			string tabId = _selectedCategoryId.Substring("customtab:".Length);
			return _settings.SavedStreams.Tabs.Find((CustomStreamTab t) => t.Id == tabId);
		}

		private async void LoadFollowedContentAsync(CancellationToken token)
		{
			if (!_twitchAuthService.IsAuthenticated)
			{
				BuildCenteredLoginButton();
				return;
			}
			ShowLoadingSpinner();
			try
			{
				List<TwitchStreamInfo> followedStreams = await _twitchService.GetFollowedChannelsAsync();
				if (token.IsCancellationRequested)
				{
					return;
				}
				ReplaceSpinnerWithContent();
				BuildFollowedHeader(followedStreams.Count);
				if (followedStreams.Count == 0)
				{
					ShowEmptyMessage("No followed channels are currently live");
					return;
				}
				foreach (TwitchStreamInfo stream in followedStreams.OrderByDescending((TwitchStreamInfo s) => s.ViewerCount))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					string key = "followed:" + stream.ChannelName;
					_cardFactory.CreateFollowedCard(_cardsPanel, key, stream, SelectFollowedChannel);
				}
				LoadFollowedAvatarsAsync(followedStreams, token);
			}
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					Logger.Warn(ex, "Failed to load followed channels");
					((Container)_cardsPanel).ClearChildren();
					BuildTwitchToolbar();
					ShowEmptyMessage("Failed to load followed channels");
				}
			}
		}

		private async void LoadCategoryContentAsync(StreamCategory category, CancellationToken token)
		{
			if (category.IsTwitch)
			{
				await LoadTwitchCategoryAsync(category, token);
			}
			else
			{
				await LoadStreamCategoryAsync(category, token);
			}
		}

		private async Task LoadTwitchCategoryAsync(StreamCategory category, CancellationToken token)
		{
			List<StreamListItem> items = BuildTwitchItems(category);
			if (items.Count == 0)
			{
				ShowEmptyMessage("No Twitch channels available");
				return;
			}
			ShowLoadingSpinner();
			await _statusLoader.FetchTwitchStatusesAsync(items, token);
			if (token.IsCancellationRequested)
			{
				return;
			}
			ReplaceSpinnerWithContent();
			BuildCategoryHeader(category);
			foreach (StreamListItem item in from i in items
				orderby i.IsOnline descending, i.ViewerCount descending
				select i)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				_cardFactory.CreateTwitchCard(_cardsPanel, item, SelectTwitchChannel);
			}
			LoadAvatarsAsync(items, token);
		}

		private async Task LoadStreamCategoryAsync(StreamCategory category, CancellationToken token)
		{
			List<StreamListItem> items = BuildChannelItems(category);
			if (items.Count == 0)
			{
				ShowEmptyMessage("No channels available");
				return;
			}
			ShowLoadingSpinner();
			await _statusLoader.FetchUrlStatusesAsync(items, token);
			if (token.IsCancellationRequested)
			{
				return;
			}
			ReplaceSpinnerWithContent();
			BuildCategoryHeader(category);
			List<StreamListItem> list = (from i in items
				where !i.IsOnDemand
				orderby i.IsOnline descending, i.Index
				select i).ToList();
			List<StreamListItem> onDemand = (from i in items
				where i.IsOnDemand
				orderby i.Index
				select i).ToList();
			bool hasBothSections = list.Count > 0 && onDemand.Count > 0;
			if (hasBothSections)
			{
				BuildSectionHeader("Livestreams");
			}
			foreach (StreamListItem item2 in list)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				_cardFactory.CreateChannelCard(_cardsPanel, item2, SelectChannel);
			}
			if (hasBothSections)
			{
				BuildSectionHeader("On Demand");
			}
			foreach (StreamListItem item in onDemand)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				_cardFactory.CreateChannelCard(_cardsPanel, item, SelectChannel);
			}
			LoadAvatarsAsync(items, token);
		}

		private async void LoadCustomTabContent(CancellationToken token)
		{
			CustomStreamTab currentTab = GetCurrentCustomTab();
			if (currentTab == null)
			{
				return;
			}
			List<SavedStream> customStreams = _settings.SavedStreams.Streams.Where((SavedStream s) => s.TabId == currentTab.Id).ToList();
			BuildCustomToolbar(currentTab);
			if (customStreams.Count == 0)
			{
				ShowEmptyMessage("No streams in this tab. Click '+ Add New' to create one.");
				return;
			}
			foreach (SavedStream stream in customStreams)
			{
				string key = GetSavedStreamKey(stream.Id);
				_cardFactory.CreateCustomCard(_cardsPanel, key, stream, null, delegate
				{
					_settings.DeleteSavedStream(stream.Id);
				}, delegate
				{
					OpenEditorForEdit(stream);
				}, SelectSavedStream);
			}
			LoadYouTubeThumbnailsImmediatelyAsync(customStreams, token);
			FetchAndApplyCustomStatusesAsync(customStreams, token);
		}

		private async Task LoadYouTubeThumbnailsImmediatelyAsync(List<SavedStream> streams, CancellationToken token)
		{
			await Task.WhenAll(from s in streams
				where s.SourceType == StreamSourceType.YouTubeVideo && !string.IsNullOrEmpty(s.Value)
				select UpdateYouTubeThumbnailAsync(GetSavedStreamKey(s.Id), s.Value, token));
		}

		private async Task FetchAndApplyCustomStatusesAsync(List<SavedStream> streams, CancellationToken token)
		{
			Dictionary<string, StreamStatus> statusMap = await _statusLoader.FetchCustomStreamStatusesAsync(streams, token);
			if (token.IsCancellationRequested)
			{
				return;
			}
			foreach (SavedStream stream in streams)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				if (statusMap.TryGetValue(stream.Id, out var status))
				{
					string key = GetSavedStreamKey(stream.Id);
					if (_streamCards.TryGetValue(key, out var card))
					{
						card.SetSubtitle(status.Subtitle, status.SubtitleColor);
					}
				}
			}
			await Task.WhenAll(from s in streams
				where s.SourceType == StreamSourceType.TwitchChannel
				where statusMap.TryGetValue(s.Id, out var value) && !string.IsNullOrEmpty(value.AvatarUrl)
				select UpdateAvatarAsync(GetSavedStreamKey(s.Id), s.Value, statusMap[s.Id].AvatarUrl, token));
		}

		private List<StreamListItem> BuildTwitchItems(StreamCategory category)
		{
			return category.TwitchChannelNames.Select((string channelName, int index) => new StreamListItem
			{
				Key = GetPresetTwitchKey(channelName),
				Title = channelName,
				TwitchChannel = channelName,
				AvatarTexture = CinemaModule.Instance.TextureService.GetDefaultAvatar(),
				Index = index
			}).ToList();
		}

		private List<StreamListItem> BuildChannelItems(StreamCategory category)
		{
			return category.Channels.Select((ChannelData channel, int index) => new StreamListItem
			{
				Key = GetChannelKey(channel.Id),
				Title = channel.Title,
				Subtitle = ((string.IsNullOrEmpty(channel.Url) && !channel.IsTwitchChannel) ? "No URL configured" : null),
				ChannelData = channel,
				TwitchChannel = (channel.IsTwitchChannel ? channel.TwitchName : null),
				AvatarTexture = (channel.AvatarTexture ?? CinemaModule.Instance.TextureService.GetDefaultAvatar()),
				IsOnDemand = channel.IsOnDemand,
				Index = index
			}).ToList();
		}

		private void BuildCenteredLoginButton()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			((Container)_cardsPanel).ClearChildren();
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(200);
			((Control)val).set_Parent((Container)(object)_cardsPanel);
			Panel container = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Login to Twitch");
			((Control)val2).set_Width(160);
			((Control)val2).set_Height(40);
			((Control)val2).set_Left((((Control)_contentContainer).get_Width() - 160) / 2);
			((Control)val2).set_Top(80);
			((Control)val2).set_Parent((Container)(object)container);
			((Control)val2).set_BasicTooltipText("Connect your Twitch account to see followed channels");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowTwitchAuthWindow();
			});
		}

		private void BuildTwitchToolbar()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			((Control)_headerSection).set_Height(40);
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(40);
			((Control)val).set_Parent((Container)(object)_headerSection);
			Panel toolbar = val;
			string text = ((_twitchAuthService.IsAuthenticated && !string.IsNullOrEmpty(_twitchAuthService.Username)) ? ("Twitch: " + _twitchAuthService.Username) : "Twitch Login");
			StandardButton val2 = new StandardButton();
			val2.set_Text(text);
			((Control)val2).set_Width(140);
			((Control)val2).set_Left(5);
			((Control)val2).set_Top(5);
			((Control)val2).set_Parent((Container)(object)toolbar);
			((Control)val2).set_BasicTooltipText("Manage Twitch connection");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowTwitchAuthWindow();
			});
			UpdateCardsPanelLayout();
		}

		private void BuildFollowedHeader(int liveCount)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			((Control)_headerSection).set_Height(50);
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(50);
			((Control)val).set_Parent((Container)(object)_headerSection);
			Panel headerPanel = val;
			string username = _twitchAuthService.Username ?? "Unknown";
			string statsText = ((liveCount > 0) ? $"Logged in as {username}  •  {liveCount} followed channels live" : ("Logged in as " + username));
			Label val2 = new Label();
			val2.set_Text(statsText);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_TextColor(Color.get_LightGray());
			val2.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val2).set_Left(10);
			((Control)val2).set_Top(15);
			((Control)val2).set_Parent((Container)(object)headerPanel);
			StandardButton val3 = new StandardButton();
			val3.set_Text("Logout");
			((Control)val3).set_Width(80);
			((Control)val3).set_Height(26);
			((Control)val3).set_Left(((Control)_contentContainer).get_Width() - 110);
			((Control)val3).set_Top(12);
			((Control)val3).set_Parent((Container)(object)headerPanel);
			((Control)val3).set_BasicTooltipText("Disconnect your Twitch account");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await _twitchAuthService.LogoutAsync();
				_settings.TwitchAccessToken = string.Empty;
				_settings.TwitchRefreshToken = string.Empty;
				RefreshContent();
			});
			UpdateCardsPanelLayout();
		}

		private void BuildCustomToolbar(CustomStreamTab currentTab)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			((Control)_headerSection).set_Height(40);
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(40);
			((Control)val).set_Parent((Container)(object)_headerSection);
			Panel toolbar = val;
			Label val2 = new Label();
			val2.set_Text(currentTab.Name);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_White());
			((Control)val2).set_Left(10);
			((Control)val2).set_Top(10);
			((Control)val2).set_Parent((Container)(object)toolbar);
			int rightX = ((Control)_contentContainer).get_Width() - 10;
			StandardButton val3 = new StandardButton();
			val3.set_Text("+ Add");
			((Control)val3).set_Width(70);
			((Control)val3).set_Left(rightX - 70);
			((Control)val3).set_Top(5);
			((Control)val3).set_Parent((Container)(object)toolbar);
			((Control)val3).set_BasicTooltipText("Add a new stream or video");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenEditorForNew(currentTab.Id);
			});
			rightX -= 80;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Rename");
			((Control)val4).set_Width(70);
			((Control)val4).set_Left(rightX - 70);
			((Control)val4).set_Top(5);
			((Control)val4).set_Parent((Container)(object)toolbar);
			((Control)val4).set_BasicTooltipText("Rename this category");
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				StartRenamingTab(currentTab);
			});
			rightX -= 80;
			if (_settings.SavedStreams.Tabs.Count > 1)
			{
				StandardButton val5 = new StandardButton();
				val5.set_Text("Delete");
				((Control)val5).set_Width(60);
				((Control)val5).set_Left(rightX - 60);
				((Control)val5).set_Top(5);
				((Control)val5).set_Parent((Container)(object)toolbar);
				((Control)val5).set_BasicTooltipText("Delete this category and all its streams");
				((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					DeleteCurrentTab(currentTab);
				});
			}
			UpdateCardsPanelLayout();
		}

		private void StartAddingNewTab()
		{
			CancelInlineEditing();
			CustomStreamTab newTab = _settings.AddCustomTab("New Category");
			_selectedCategoryId = "customtab:" + newTab.Id;
			_settings.LastSelectedSourceCategory = _selectedCategoryId;
			PopulateCategoryMenu();
			if (_customTabMenuItems.TryGetValue(newTab.Id, out var menuItem))
			{
				_categoryMenu.Select(menuItem);
				StartRenamingTabById(newTab.Id);
			}
		}

		private void StartRenamingTab(CustomStreamTab tab)
		{
			StartRenamingTabById(tab.Id);
		}

		private void StartRenamingTabById(string tabId)
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			CancelInlineEditing();
			if (_customTabMenuItems.TryGetValue(tabId, out var menuItem))
			{
				CustomStreamTab tab = _settings.SavedStreams.Tabs.Find((CustomStreamTab t) => t.Id == tabId);
				if (tab != null)
				{
					_editingTabId = tabId;
					int yPosition = ((Container)_categoryMenu).get_Children().ToList().IndexOf((Control)(object)menuItem) * _categoryMenu.get_MenuItemHeight() + 10;
					TextBox val = new TextBox();
					((TextInputBase)val).set_Text(tab.Name);
					((Control)val).set_Size(new Point(180, 30));
					((Control)val).set_Location(new Point(45, yPosition));
					((TextInputBase)val).set_MaxLength(16);
					((Control)val).set_Parent((Container)(object)_menuPanel);
					_editingTextBox = val;
					menuItem.set_Text("");
					((TextInputBase)_editingTextBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)OnEditingTextBoxFocusChanged);
					_editingTextBox.add_EnterPressed((EventHandler<EventArgs>)OnEditingTextBoxEnterPressed);
					((TextInputBase)_editingTextBox).set_Focused(true);
					((TextInputBase)_editingTextBox).set_SelectionStart(0);
					((TextInputBase)_editingTextBox).set_SelectionEnd(((TextInputBase)_editingTextBox).get_Text().Length);
				}
			}
		}

		private void OnEditingTextBoxEnterPressed(object sender, EventArgs e)
		{
			FinishInlineEditing(save: true);
		}

		private void OnEditingTextBoxFocusChanged(object sender, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				FinishInlineEditing(save: true);
			}
		}

		private void FinishInlineEditing(bool save)
		{
			if (_editingTextBox != null && !string.IsNullOrEmpty(_editingTabId))
			{
				string newName = ((TextInputBase)_editingTextBox).get_Text()?.Trim();
				string tabId = _editingTabId;
				DisposeEditingTextBox();
				if (save && !string.IsNullOrWhiteSpace(newName))
				{
					_settings.RenameCustomTab(tabId, newName);
				}
				_selectedCategoryId = "customtab:" + tabId;
				_settings.LastSelectedSourceCategory = _selectedCategoryId;
				PopulateCategoryMenu();
				SelectTabById(tabId);
			}
		}

		private void CancelInlineEditing()
		{
			if (_editingTextBox != null)
			{
				DisposeEditingTextBox();
			}
		}

		private void DisposeEditingTextBox()
		{
			((TextInputBase)_editingTextBox).remove_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)OnEditingTextBoxFocusChanged);
			_editingTextBox.remove_EnterPressed((EventHandler<EventArgs>)OnEditingTextBoxEnterPressed);
			((Control)_editingTextBox).Dispose();
			_editingTextBox = null;
			_editingTabId = null;
		}

		private void DeleteCurrentTab(CustomStreamTab tab)
		{
			if (_settings.SavedStreams.Tabs.Count > 1)
			{
				_settings.DeleteCustomTab(tab.Id);
				CustomStreamTab firstTab = _settings.SavedStreams.Tabs[0];
				_selectedCategoryId = "customtab:" + firstTab.Id;
				_settings.LastSelectedSourceCategory = _selectedCategoryId;
				PopulateCategoryMenu();
				SelectTabById(firstTab.Id);
			}
		}

		private void SelectTabById(string tabId)
		{
			if (_customTabMenuItems.TryGetValue(tabId, out var menuItem))
			{
				_categoryMenu.Select(menuItem);
			}
		}

		private void BuildCategoryHeader(StreamCategory category)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			bool hasDescription = !string.IsNullOrEmpty(category.Description);
			bool hasInfoUrl = !string.IsNullOrEmpty(category.InfoUrl);
			if (!hasDescription && !hasInfoUrl)
			{
				return;
			}
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Parent((Container)(object)_headerSection);
			Panel headerPanel = val;
			int contentHeight = 0;
			if (hasDescription)
			{
				int labelWidth = ((Control)_contentContainer).get_Width() - (hasInfoUrl ? 90 : 40);
				Label val2 = new Label();
				val2.set_Text(category.Description);
				((Control)val2).set_Width(Math.Max(labelWidth, 100));
				val2.set_AutoSizeHeight(true);
				val2.set_WrapText(true);
				val2.set_TextColor(Color.get_LightGray());
				val2.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val2).set_Left(10);
				((Control)val2).set_Top(10);
				((Control)val2).set_Parent((Container)(object)headerPanel);
				contentHeight = ((Control)val2).get_Height();
			}
			((Control)headerPanel).set_Height(Math.Max(contentHeight + 20, 46));
			((Control)_headerSection).set_Height(((Control)headerPanel).get_Height());
			if (hasInfoUrl)
			{
				string infoUrl = category.InfoUrl;
				StandardButton val3 = new StandardButton();
				val3.set_Text("Info");
				((Control)val3).set_Width(50);
				((Control)val3).set_Height(26);
				((Control)val3).set_Left(((Control)_contentContainer).get_Width() - 80);
				((Control)val3).set_Top((((Control)headerPanel).get_Height() - 26) / 2);
				((Control)val3).set_Parent((Container)(object)headerPanel);
				((Control)val3).set_BasicTooltipText("Learn more about this category");
				((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					OpenUrlInBrowser(infoUrl);
				});
			}
			UpdateCardsPanelLayout();
		}

		private void BuildSectionHeader(string title)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(32);
			((Control)val).set_Parent((Container)(object)_cardsPanel);
			Panel headerPanel = val;
			Label val2 = new Label();
			val2.set_Text(title);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_TextColor(Color.get_White());
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Left(10);
			((Control)val2).set_Top(8);
			((Control)val2).set_Parent((Container)(object)headerPanel);
		}

		private void ShowLoadingSpinner()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			((Container)_cardsPanel).ClearChildren();
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(100);
			((Control)val).set_Parent((Container)(object)_cardsPanel);
			Panel container = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Parent((Container)(object)container);
			LoadingSpinner spinner = val2;
			((Control)spinner).set_Left((((Control)_contentContainer).get_Width() - ((Control)spinner).get_Width()) / 2);
			((Control)spinner).set_Top((((Control)container).get_Height() - ((Control)spinner).get_Height()) / 2);
		}

		private void ReplaceSpinnerWithContent()
		{
			((Container)_cardsPanel).ClearChildren();
		}

		private void ShowEmptyMessage(string message)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(40);
			((Control)val).set_Parent((Container)(object)_cardsPanel);
			Panel container = val;
			Label val2 = new Label();
			val2.set_Text(message);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_TextColor(Color.get_Gray());
			((Control)val2).set_Left(10);
			((Control)val2).set_Top(12);
			((Control)val2).set_Parent((Container)(object)container);
		}

		private async Task LoadFollowedAvatarsAsync(List<TwitchStreamInfo> streams, CancellationToken token)
		{
			await Task.WhenAll(from s in streams
				where !string.IsNullOrEmpty(s.AvatarUrl)
				select UpdateAvatarAsync("followed:" + s.ChannelName, s.ChannelName, s.AvatarUrl, token));
		}

		private async Task LoadAvatarsAsync(List<StreamListItem> items, CancellationToken token)
		{
			await Task.WhenAll(from i in items
				where HasAvatarUrlAndCacheKey(i)
				select UpdateAvatarAsync(i.Key, i.TwitchChannel ?? i.ChannelData?.Id, i.AvatarUrl ?? i.ChannelData?.Avatar, token));
		}

		private static bool HasAvatarUrlAndCacheKey(StreamListItem item)
		{
			string value = item.AvatarUrl ?? item.ChannelData?.Avatar;
			string cacheKey = item.TwitchChannel ?? item.ChannelData?.Id;
			if (!string.IsNullOrEmpty(value))
			{
				return !string.IsNullOrEmpty(cacheKey);
			}
			return false;
		}

		private async Task UpdateAvatarAsync(string itemKey, string cacheKey, string avatarUrl, CancellationToken token)
		{
			try
			{
				AsyncTexture2D avatarTexture = await CinemaModule.Instance.TextureService.GetTwitchAvatarAsync(cacheKey, avatarUrl);
				if (!token.IsCancellationRequested && avatarTexture != null && _streamCards.TryGetValue(itemKey, out var card))
				{
					card.SetAvatar(avatarTexture);
				}
			}
			catch (Exception ex)
			{
				Logger.Debug("Failed to load avatar for " + cacheKey + ": " + ex.Message);
			}
		}

		private async Task UpdateYouTubeThumbnailAsync(string itemKey, string videoIdOrUrl, CancellationToken token)
		{
			try
			{
				AsyncTexture2D texture = await CinemaModule.Instance.TextureService.GetYouTubeThumbnailAsync(videoIdOrUrl);
				if (!token.IsCancellationRequested && texture != null && _streamCards.TryGetValue(itemKey, out var card))
				{
					card.SetAvatar(texture);
				}
			}
			catch (Exception ex)
			{
				Logger.Debug("Failed to load YouTube thumbnail for " + videoIdOrUrl + ": " + ex.Message);
			}
		}

		private void InitializeSelectedStreamKey()
		{
			if (!string.IsNullOrEmpty(_settings.SelectedSavedStreamId))
			{
				_selectedStreamKey = GetSavedStreamKey(_settings.SelectedSavedStreamId);
			}
			else if (_settings.CurrentStreamSourceType == StreamSourceType.TwitchChannel && !string.IsNullOrEmpty(_settings.CurrentTwitchChannel))
			{
				string channelName = _settings.CurrentTwitchChannel;
				string presetChannel = _presetService.TwitchChannels.FirstOrDefault((string c) => string.Equals(c, channelName, StringComparison.OrdinalIgnoreCase));
				if (presetChannel != null)
				{
					_selectedStreamKey = GetPresetTwitchKey(presetChannel);
				}
				else if (_settings.LastSelectedSourceCategory == "Followed Channels")
				{
					_selectedStreamKey = "followed:" + channelName;
				}
				else
				{
					_selectedStreamKey = GetPresetTwitchKey(channelName);
				}
			}
			else if (_settings.CurrentStreamSourceType == StreamSourceType.Url && !string.IsNullOrEmpty(_settings.SelectedUrlChannelId))
			{
				_selectedStreamKey = GetChannelKey(_settings.SelectedUrlChannelId);
			}
		}

		private void UpdateCardSelection()
		{
			foreach (KeyValuePair<string, ListCard> kvp in _streamCards)
			{
				kvp.Value.IsSelected = kvp.Key == _selectedStreamKey;
			}
		}

		private async void SelectFollowedChannel(string channelName, string key)
		{
			await SelectTwitchChannelInternal(channelName, key, "followed channel");
		}

		private async void SelectTwitchChannel(string channelName, string key)
		{
			await SelectTwitchChannelInternal(channelName, key, "Twitch channel");
		}

		private async Task SelectTwitchChannelInternal(string channelName, string key, string description)
		{
			CancelPendingSelection();
			_selectedStreamKey = key;
			_settings.SelectTwitchChannel(channelName);
			_controller.PrepareForStreamChange();
			UpdateCardSelection();
			await TrySetStreamUrlAsync(() => _twitchService.GetPlayableStreamUrlAsync(channelName), description + ": " + channelName, _selectionCts.Token);
		}

		private async void SelectChannel(ChannelData channel, string key)
		{
			CancelPendingSelection();
			_selectedStreamKey = key;
			if (channel.IsTwitchChannel && !string.IsNullOrEmpty(channel.TwitchName))
			{
				_settings.SelectTwitchChannel(channel.TwitchName);
				_controller.PrepareForStreamChange();
				UpdateCardSelection();
				await TrySetStreamUrlAsync(() => _twitchService.GetPlayableStreamUrlAsync(channel.TwitchName), "channel: " + channel.Title, _selectionCts.Token);
			}
			else
			{
				_settings.SelectUrlChannel(channel);
				UpdateCardSelection();
			}
		}

		private async void SelectSavedStream(SavedStream stream)
		{
			CancelPendingSelection();
			_selectedStreamKey = GetSavedStreamKey(stream.Id);
			_controller.SelectSavedStream(stream.Id);
			_controller.PrepareForStreamChange();
			UpdateCardSelection();
			switch (stream.SourceType)
			{
			case StreamSourceType.TwitchChannel:
				await TrySetStreamUrlAsync(() => _twitchService.GetPlayableStreamUrlAsync(stream.Value), "stream: " + stream.Name, _selectionCts.Token);
				break;
			case StreamSourceType.YouTubeVideo:
				await TrySetYouTubeStreamUrlAsync(stream.Value, "YouTube video: " + stream.Name, _selectionCts.Token);
				break;
			}
		}

		private void CancelPendingSelection()
		{
			_selectionCts?.Cancel();
			_selectionCts?.Dispose();
			_selectionCts = new CancellationTokenSource();
		}

		private async Task TrySetStreamUrlAsync(Func<Task<string>> getUrlAsync, string streamDescription, CancellationToken token)
		{
			try
			{
				string url = await getUrlAsync();
				if (!token.IsCancellationRequested)
				{
					_settings.StreamUrl = url ?? "";
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					Logger.Error(ex, "Failed to get stream URL for " + streamDescription);
				}
			}
		}

		private async Task TrySetYouTubeStreamUrlAsync(string videoIdOrUrl, string streamDescription, CancellationToken token)
		{
			try
			{
				YouTubeStreamUrls urls = await _youtubeService.GetBestStreamUrlsAsync(videoIdOrUrl);
				if (!token.IsCancellationRequested)
				{
					_settings.AudioUrl = urls.AudioUrl;
					_settings.StreamUrl = urls.VideoUrl ?? "";
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					Logger.Error(ex, "Failed to get stream URL for " + streamDescription);
				}
			}
		}

		private void ShowTwitchAuthWindow()
		{
			if (_twitchAuthWindow == null)
			{
				_twitchAuthWindow = new TwitchAuthWindow(_twitchAuthService, OnTwitchTokensChanged);
			}
			((Control)_twitchAuthWindow).Show();
		}

		private void OnTwitchTokensChanged(string accessToken, string refreshToken)
		{
			_settings.TwitchAccessToken = accessToken;
			_settings.TwitchRefreshToken = refreshToken;
		}

		private void OpenEditorForNew(string tabId = null)
		{
			EnsureEditorWindow();
			_editorWindow.OpenForNew(tabId);
		}

		private void OpenEditorForEdit(SavedStream stream)
		{
			EnsureEditorWindow();
			_editorWindow.OpenForEdit(stream);
		}

		private void EnsureEditorWindow()
		{
			if (_editorWindow == null)
			{
				_editorWindow = new StreamEditorWindow(_settings, CinemaModule.Instance.TextureService);
				_editorWindow.StreamSaved += delegate
				{
					RefreshContent();
				};
				_editorWindow.StreamDeleted += delegate
				{
					RefreshContent();
				};
			}
		}

		private void OpenUrlInBrowser(string url)
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = url,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to open URL in browser: " + url);
			}
		}

		private void CopyWaypointToClipboard(string waypoint)
		{
			if (!string.IsNullOrEmpty(waypoint))
			{
				try
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(waypoint);
					ScreenNotification.ShowNotification("Waypoint copied!", (NotificationType)0, (Texture2D)null, 4);
				}
				catch (Exception ex)
				{
					Logger.Debug("Failed to copy waypoint to clipboard: " + ex.Message);
				}
			}
		}

		private void ApplyWorldPosition(ChannelData channelData)
		{
			if (channelData?.Position != null && channelData.ScreenWidth.HasValue)
			{
				_settings.WorldPosition = channelData.Position;
				_settings.WorldScreenWidth = channelData.ScreenWidth.Value;
				_settings.DisplayMode = CinemaDisplayMode.InGame;
			}
		}

		private void OpenTwitchChat(string channelName)
		{
			if (string.IsNullOrEmpty(channelName))
			{
				Logger.Warn("Cannot open Twitch chat - channel name is empty");
			}
			else
			{
				_controller.RequestShowChat(channelName);
			}
		}

		private static string GetChannelKey(string channelId)
		{
			return "channel:" + channelId;
		}

		private static string GetPresetTwitchKey(string channel)
		{
			return "preset_twitch:" + channel;
		}

		private static string GetSavedStreamKey(string id)
		{
			return "saved:" + id;
		}

		protected override void Unload()
		{
			CancelInlineEditing();
			_contentCts?.Cancel();
			_contentCts?.Dispose();
			_selectionCts?.Cancel();
			_selectionCts?.Dispose();
			Panel contentContainer = _contentContainer;
			if (((contentContainer != null) ? ((Control)contentContainer).get_Parent() : null) != null)
			{
				((Control)((Control)_contentContainer).get_Parent()).remove_Resized((EventHandler<ResizedEventArgs>)OnParentResized);
			}
			_categoryMenu.remove_ItemSelected((EventHandler<ControlActivatedEventArgs>)OnCategorySelected);
			_settings.SavedStreamsChanged -= OnSavedStreamsChanged;
			_presetService.PresetsLoaded -= OnPresetsLoaded;
			_twitchAuthService.AuthStatusChanged -= OnTwitchAuthStatusChanged;
			StreamEditorWindow editorWindow = _editorWindow;
			if (editorWindow != null)
			{
				((Control)editorWindow).Dispose();
			}
			TwitchAuthWindow twitchAuthWindow = _twitchAuthWindow;
			if (twitchAuthWindow != null)
			{
				((Control)twitchAuthWindow).Dispose();
			}
			((View<IPresenter>)this).Unload();
		}
	}
}
