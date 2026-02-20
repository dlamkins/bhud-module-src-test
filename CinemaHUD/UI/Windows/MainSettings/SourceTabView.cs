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
using CinemaHUD.UI.Windows.SettingsSmall;
using CinemaModule;
using CinemaModule.Models;
using CinemaModule.Services;
using CinemaModule.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaHUD.UI.Windows.MainSettings
{
	public class SourceTabView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<SourceTabView>();

		private const int MenuPanelWidth = 240;

		private const int CardVerticalSpacing = 4;

		private const string KeyPrefixChannel = "channel:";

		private const string KeyPrefixPresetTwitch = "preset_twitch:";

		private const string KeyPrefixSaved = "saved:";

		private const string KeyPrefixFollowed = "followed:";

		private const string CategoryFollowed = "Followed Channels";

		private const string CategoryMyStreams = "My Streams";

		private readonly CinemaUserSettings _settings;

		private readonly CinemaController _controller;

		private readonly TwitchService _twitchService;

		private readonly TwitchAuthService _twitchAuthService;

		private readonly PresetService _presetService;

		private Menu _categoryMenu;

		private FlowPanel _contentPanel;

		private readonly Dictionary<string, ListCard> _streamCards = new Dictionary<string, ListCard>();

		private string _selectedStreamKey;

		private string _selectedCategoryId;

		private StreamEditorWindow _editorWindow;

		private TwitchAuthWindow _twitchAuthWindow;

		private CancellationTokenSource _cts;

		private CancellationTokenSource _contentCts;

		private readonly Dictionary<string, StreamCategory> _categoryLookup = new Dictionary<string, StreamCategory>();

		private StreamCardFactory _cardFactory;

		private StreamStatusLoader _statusLoader;

		public SourceTabView(CinemaUserSettings settings, CinemaController controller, TwitchService twitchService, TwitchAuthService twitchAuthService, PresetService presetService)
			: this()
		{
			_settings = settings;
			_controller = controller;
			_twitchService = twitchService;
			_twitchAuthService = twitchAuthService;
			_presetService = presetService;
			_cts = new CancellationTokenSource();
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
			_cardFactory = new StreamCardFactory(global::CinemaModule.CinemaModule.Instance.TextureService, _twitchService, () => _selectedStreamKey, _streamCards)
			{
				OnOpenChat = OpenTwitchChat,
				OnCopyWaypoint = CopyWaypointToClipboard,
				OnApplyWorldPosition = ApplyWorldPosition
			};
			_statusLoader = new StreamStatusLoader(_twitchService);
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
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(true);
			((Control)val).set_Size(new Point(240, ((Control)parent).get_Height() - 110));
			((Control)val).set_Location(new Point(23, 10));
			val.set_Title("Categories");
			((Control)val).set_Parent(parent);
			val.set_CanScroll(true);
			Panel menuPanel = val;
			Image val2 = new Image();
			val2.set_Texture(global::CinemaModule.CinemaModule.Instance.TextureService.GetRefreshIcon());
			((Control)val2).set_Size(new Point(24, 24));
			((Control)val2).set_Location(new Point(229, 16));
			((Control)val2).set_BasicTooltipText("Refresh");
			((Control)val2).set_Opacity(0.6f);
			((Control)val2).set_Parent(parent);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await RefreshAllAsync();
			});
			Menu val3 = new Menu();
			Rectangle contentRegion = ((Container)menuPanel).get_ContentRegion();
			((Control)val3).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val3.set_MenuItemHeight(50);
			((Control)val3).set_Parent((Container)(object)menuPanel);
			val3.set_CanSelect(true);
			_categoryMenu = val3;
			PopulateCategoryMenu();
			_categoryMenu.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)OnCategorySelected);
		}

		private void PopulateCategoryMenu()
		{
			((Container)_categoryMenu).ClearChildren();
			_categoryLookup.Clear();
			foreach (StreamCategory category in _presetService.StreamCategories)
			{
				_categoryMenu.AddMenuItem(category.Name, (Texture2D)null).set_Icon(category.IconTexture);
				_categoryLookup[category.Name] = category;
			}
			_categoryMenu.AddMenuItem("Followed Channels", (Texture2D)null).set_Icon(global::CinemaModule.CinemaModule.Instance.TextureService.GetTwitchBigIcon());
			_categoryMenu.AddMenuItem("My Streams", (Texture2D)null).set_Icon(global::CinemaModule.CinemaModule.Instance.TextureService.GetEmblem());
		}

		private void BuildContentPanel(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Size(new Point(((Control)parent).get_Width() - 240 - 90, ((Control)parent).get_Height() - 110));
			((Control)val).set_Location(new Point(269, 10));
			val.set_ControlPadding(new Vector2(0f, 4f));
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_Parent(parent);
			_contentPanel = val;
		}

		private void SelectInitialCategory()
		{
			string initialCategory = DetermineInitialCategory();
			MenuItem menuItem = ((IEnumerable)((Container)_categoryMenu).get_Children()).OfType<MenuItem>().FirstOrDefault((MenuItem m) => m.get_Text() == initialCategory);
			if (menuItem != null)
			{
				_categoryMenu.Select(menuItem);
			}
		}

		private string DetermineInitialCategory()
		{
			string lastCategory = _settings.LastSelectedSourceCategory;
			if (string.IsNullOrEmpty(lastCategory) || (!_categoryLookup.ContainsKey(lastCategory) && !(lastCategory == "Followed Channels") && !(lastCategory == "My Streams")))
			{
				return _presetService.StreamCategories.FirstOrDefault()?.Name ?? "Followed Channels";
			}
			return lastCategory;
		}

		private void OnCategorySelected(object sender, ControlActivatedEventArgs e)
		{
			Control activatedControl = e.get_ActivatedControl();
			MenuItem menuItem = (MenuItem)(object)((activatedControl is MenuItem) ? activatedControl : null);
			if (menuItem != null)
			{
				_selectedCategoryId = menuItem.get_Text();
				_settings.LastSelectedSourceCategory = _selectedCategoryId;
				RefreshContent();
			}
		}

		private async Task RefreshAllAsync()
		{
			await _presetService.LoadPresetsAsync();
			PopulateCategoryMenu();
			ReselectCurrentCategory();
			RefreshContent();
		}

		private void ReselectCurrentCategory()
		{
			if (!string.IsNullOrEmpty(_selectedCategoryId))
			{
				MenuItem menuItem = ((IEnumerable)((Container)_categoryMenu).get_Children()).OfType<MenuItem>().FirstOrDefault((MenuItem m) => m.get_Text() == _selectedCategoryId);
				if (menuItem != null)
				{
					_categoryMenu.Select(menuItem);
				}
			}
		}

		private void RefreshContent()
		{
			_contentCts?.Cancel();
			_contentCts?.Dispose();
			_contentCts = new CancellationTokenSource();
			_streamCards.Clear();
			if (_selectedCategoryId == "Followed Channels")
			{
				LoadFollowedContentAsync(_contentCts.Token);
				return;
			}
			if (_selectedCategoryId == "My Streams")
			{
				LoadCustomContent();
				return;
			}
			StreamCategory category = _presetService.StreamCategories.FirstOrDefault((StreamCategory c) => c.Name == _selectedCategoryId);
			if (category != null)
			{
				LoadCategoryContentAsync(category, _contentCts.Token);
			}
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
					_cardFactory.CreateFollowedCard(_contentPanel, key, stream, SelectFollowedChannel);
				}
				LoadFollowedAvatarsAsync(followedStreams, token);
			}
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					Logger.Warn(ex, "Failed to load followed channels");
					((Container)_contentPanel).ClearChildren();
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
				_cardFactory.CreateTwitchCard(_contentPanel, item, SelectTwitchChannel);
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
				_cardFactory.CreateChannelCard(_contentPanel, item2, SelectChannel);
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
				_cardFactory.CreateChannelCard(_contentPanel, item, SelectChannel);
			}
			LoadAvatarsAsync(items, token);
		}

		private async void LoadCustomContent()
		{
			List<SavedStream> customStreams = _settings.SavedStreams.Streams.ToList();
			if (customStreams.Count == 0)
			{
				BuildCustomToolbar();
				ShowEmptyMessage("No custom streams. Click '+ Add New' to create one.");
				return;
			}
			ShowLoadingSpinner();
			Dictionary<string, StreamStatus> statusMap = await _statusLoader.FetchCustomStreamStatusesAsync(customStreams, _contentCts.Token);
			if (_contentCts.Token.IsCancellationRequested)
			{
				return;
			}
			ReplaceSpinnerWithContent();
			BuildCustomToolbar();
			foreach (SavedStream stream in customStreams)
			{
				statusMap.TryGetValue(stream.Id, out var status);
				string key = GetSavedStreamKey(stream.Id);
				_cardFactory.CreateCustomCard(_contentPanel, key, stream, status, delegate
				{
					_settings.DeleteSavedStream(stream.Id);
				}, delegate
				{
					OpenEditorForEdit(stream);
				}, SelectSavedStream);
			}
			LoadCustomAvatarsAsync(customStreams, statusMap, _contentCts.Token);
		}

		private List<StreamListItem> BuildTwitchItems(StreamCategory category)
		{
			return category.TwitchChannelNames.Select((string channelName, int index) => new StreamListItem
			{
				Key = GetPresetTwitchKey(channelName),
				Title = channelName,
				TwitchChannel = channelName,
				AvatarTexture = global::CinemaModule.CinemaModule.Instance.TextureService.GetDefaultAvatar(),
				Index = index
			}).ToList();
		}

		private List<StreamListItem> BuildChannelItems(StreamCategory category)
		{
			return category.Channels.Select((ChannelData channel, int index) => new StreamListItem
			{
				Key = GetChannelKey(channel.Id),
				Title = channel.Title,
				Subtitle = (string.IsNullOrEmpty(channel.Url) ? "No URL configured" : null),
				ChannelData = channel,
				AvatarTexture = (channel.AvatarTexture ?? global::CinemaModule.CinemaModule.Instance.TextureService.GetDefaultAvatar()),
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
			((Container)_contentPanel).ClearChildren();
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(200);
			((Control)val).set_Parent((Container)(object)_contentPanel);
			Panel container = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Login to Twitch");
			((Control)val2).set_Width(160);
			((Control)val2).set_Height(40);
			((Control)val2).set_Left((((Control)_contentPanel).get_Width() - 160) / 2);
			((Control)val2).set_Top(80);
			((Control)val2).set_Parent((Container)(object)container);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowTwitchAuthWindow();
			});
		}

		private void BuildTwitchToolbar()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(40);
			((Control)val).set_Parent((Container)(object)_contentPanel);
			Panel toolbar = val;
			string text = ((_twitchAuthService.IsAuthenticated && !string.IsNullOrEmpty(_twitchAuthService.Username)) ? ("Twitch: " + _twitchAuthService.Username) : "Twitch Login");
			StandardButton val2 = new StandardButton();
			val2.set_Text(text);
			((Control)val2).set_Width(140);
			((Control)val2).set_Left(5);
			((Control)val2).set_Top(5);
			((Control)val2).set_Parent((Container)(object)toolbar);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowTwitchAuthWindow();
			});
		}

		private void BuildFollowedHeader(int liveCount)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(50);
			((Control)val).set_Parent((Container)(object)_contentPanel);
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
			((Control)val3).set_Left(((Control)_contentPanel).get_Width() - 110);
			((Control)val3).set_Top(12);
			((Control)val3).set_Parent((Container)(object)headerPanel);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await _twitchAuthService.LogoutAsync();
				_settings.TwitchAccessToken = string.Empty;
				_settings.TwitchRefreshToken = string.Empty;
				RefreshContent();
			});
		}

		private void BuildCustomToolbar()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(40);
			((Control)val).set_Parent((Container)(object)_contentPanel);
			Panel toolbar = val;
			Label val2 = new Label();
			val2.set_Text("Add your own Twitch channels or streams");
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_TextColor(Color.get_Gray());
			((Control)val2).set_Left(10);
			((Control)val2).set_Top(12);
			((Control)val2).set_Parent((Container)(object)toolbar);
			StandardButton val3 = new StandardButton();
			val3.set_Text("+ Add New");
			((Control)val3).set_Width(100);
			((Control)val3).set_Left(((Control)_contentPanel).get_Width() - 110);
			((Control)val3).set_Top(5);
			((Control)val3).set_Parent((Container)(object)toolbar);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenEditorForNew();
			});
		}

		private void BuildCategoryHeader(StreamCategory category)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(category.Description) && string.IsNullOrEmpty(category.InfoUrl))
			{
				return;
			}
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Parent((Container)(object)_contentPanel);
			Panel headerPanel = val;
			int contentHeight = 0;
			if (!string.IsNullOrEmpty(category.Description))
			{
				bool hasInfoButton = !string.IsNullOrEmpty(category.InfoUrl);
				int labelWidth = ((Control)_contentPanel).get_Width() - (hasInfoButton ? 90 : 40);
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
			if (!string.IsNullOrEmpty(category.InfoUrl))
			{
				string infoUrl = category.InfoUrl;
				StandardButton val3 = new StandardButton();
				val3.set_Text("Info");
				((Control)val3).set_Width(50);
				((Control)val3).set_Height(26);
				((Control)val3).set_Left(((Control)_contentPanel).get_Width() - 80);
				((Control)val3).set_Top((((Control)headerPanel).get_Height() - 26) / 2);
				((Control)val3).set_Parent((Container)(object)headerPanel);
				((Control)val3).set_BasicTooltipText("Open in browser");
				((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					OpenUrlInBrowser(infoUrl);
				});
			}
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
			((Control)val).set_Parent((Container)(object)_contentPanel);
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
			((Container)_contentPanel).ClearChildren();
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(100);
			((Control)val).set_Parent((Container)(object)_contentPanel);
			Panel container = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Parent((Container)(object)container);
			LoadingSpinner spinner = val2;
			((Control)spinner).set_Left((((Control)_contentPanel).get_Width() - ((Control)spinner).get_Width()) / 2);
			((Control)spinner).set_Top((((Control)container).get_Height() - ((Control)spinner).get_Height()) / 2);
		}

		private void ReplaceSpinnerWithContent()
		{
			((Container)_contentPanel).ClearChildren();
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
			((Control)val).set_Parent((Container)(object)_contentPanel);
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

		private async Task LoadCustomAvatarsAsync(List<SavedStream> streams, Dictionary<string, StreamStatus> statusMap, CancellationToken token)
		{
			await Task.WhenAll(from s in streams
				where s.SourceType == StreamSourceType.TwitchChannel
				where statusMap.TryGetValue(s.Id, out var value) && !string.IsNullOrEmpty(value.AvatarUrl)
				select UpdateAvatarAsync(GetSavedStreamKey(s.Id), s.Value, statusMap[s.Id].AvatarUrl, token));
		}

		private async Task LoadAvatarsAsync(List<StreamListItem> items, CancellationToken token)
		{
			await Task.WhenAll(from i in items
				where !string.IsNullOrEmpty(i.AvatarUrl ?? i.ChannelData?.Avatar) && !string.IsNullOrEmpty(i.TwitchChannel ?? i.ChannelData?.Id)
				select UpdateAvatarAsync(i.Key, i.TwitchChannel ?? i.ChannelData?.Id, i.AvatarUrl ?? i.ChannelData?.Avatar, token));
		}

		private async Task UpdateAvatarAsync(string itemKey, string cacheKey, string avatarUrl, CancellationToken token)
		{
			try
			{
				AsyncTexture2D avatarTexture = await _twitchService.GetAvatarTextureAsync(cacheKey, avatarUrl);
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
			_selectedStreamKey = key;
			_settings.SelectTwitchChannel(channelName);
			UpdateCardSelection();
			await TrySetStreamUrlAsync(() => _twitchService.GetPlayableStreamUrlAsync(channelName), description + ": " + channelName);
		}

		private void SelectChannel(ChannelData channel, string key)
		{
			_selectedStreamKey = key;
			_settings.SelectUrlChannel(channel);
			UpdateCardSelection();
		}

		private async void SelectSavedStream(SavedStream stream)
		{
			_selectedStreamKey = GetSavedStreamKey(stream.Id);
			_settings.SelectSavedStream(stream);
			UpdateCardSelection();
			if (stream.SourceType == StreamSourceType.TwitchChannel)
			{
				await TrySetStreamUrlAsync(() => _twitchService.GetPlayableStreamUrlAsync(stream.Value), "stream: " + stream.Name);
			}
			else
			{
				_settings.StreamUrl = stream.Value;
			}
		}

		private async Task TrySetStreamUrlAsync(Func<Task<string>> getUrlAsync, string streamDescription)
		{
			try
			{
				string url = await getUrlAsync();
				if (!_cts.IsCancellationRequested && !string.IsNullOrEmpty(url))
				{
					_settings.StreamUrl = url;
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				if (!_cts.IsCancellationRequested)
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

		private void OpenEditorForNew()
		{
			EnsureEditorWindow();
			_editorWindow.OpenForNew();
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
				_editorWindow = new StreamEditorWindow(_settings);
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
			_contentCts?.Cancel();
			_contentCts?.Dispose();
			_cts?.Cancel();
			_cts?.Dispose();
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
