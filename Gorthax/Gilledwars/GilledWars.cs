using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using GilledWars;
using Gw2Sharp;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gorthax.Gilledwars
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class GilledWars : Blish_HUD.Modules.Module
	{
		private static readonly Logger Logger = Logger.GetLogger<GilledWars>();

		private const string AllSpeciesKey = "__ALL_SPECIES__";

		private SettingEntry<CornerIconType> _cornerIconChoice;

		private SettingEntry<bool> _autoStartLogging;

		private SettingEntry<bool> _showTimeOfDayWidget;

		private SettingEntry<bool> _lockTimeOfDayWidget;

		private SettingEntry<int> _todWidgetSize;

		private SettingEntry<TodTextLayout> _todTextLayout;

		private SettingEntry<int> _todLocX;

		private SettingEntry<int> _todLocY;

		private Blish_HUD.Controls.Panel _timeOfDayPanel;

		private Image _todIcon;

		private Blish_HUD.Controls.Label _todLabel;

		private bool _isTodDragging;

		private Point _todDragOffset;

		private AsyncTexture2D _texDawn;

		private AsyncTexture2D _texDay;

		private AsyncTexture2D _texDusk;

		private AsyncTexture2D _texCaughtCheck;

		private AsyncTexture2D _texNight;

		private string _currentTodPhase = "";

		private const string API_BASE_URL = "https://api.gilledwars.com";

		private static readonly HttpClient _httpClient = CreateHttpClient();

		private Blish_HUD.Controls.Panel _mainWindow;

		private Blish_HUD.Controls.Panel _casualPanel;

		private Blish_HUD.Controls.Panel _tournamentPanel;

		private Blish_HUD.Controls.Panel _recentCatchesPanel;

		private Blish_HUD.Controls.Panel _fishLogPanel;

		private Blish_HUD.Controls.Panel _tourneyHostPanel;

		private Blish_HUD.Controls.Panel _tourneyParticipantPanel;

		private Blish_HUD.Controls.Panel _tourneyActivePanel;

		private Blish_HUD.Controls.Label _activeTimerLabel;

		private Blish_HUD.Controls.Label _waitingRoomLabel;

		private Blish_HUD.Controls.Label _activeSyncTimerLabel;

		private FlowPanel _activeCoolerList;

		private StandardButton _activeMeasureBtn;

		private StandardButton _activeEndBtn;

		private StandardButton _activeRecopyBtn;

		private StandardButton _activeExitBtn;

		private CornerIcon _cornerIcon;

		private List<FishUIEntry> _allFishEntries = new List<FishUIEntry>();

		private List<FlowPanel> _categoryPanels = new List<FlowPanel>();

		private List<TournamentCatch> _recentCatches = new List<TournamentCatch>();

		private List<TournamentCatch> _tourneyCatches = new List<TournamentCatch>();

		private Dictionary<int, int> _startInventory = new Dictionary<int, int>();

		private HashSet<int> _caughtFishIds = new HashSet<int>();

		private Dictionary<int, PersonalBestRecord> _personalBests = new Dictionary<int, PersonalBestRecord>();

		private Blish_HUD.Controls.Label _lbDynamicTitleLabel;

		private bool _isAnalyzerMinified;

		private int _analyzerGridCols = 4;

		private string _openAnalyzerLocationName;

		private int _openAnalyzerAchievementId;

		private int _openAnalyzerSubMax;

		private string _openAnalyzerSubDescription;

		private SettingEntry<bool> _showProfitWidget;

		private SettingEntry<int> _profitLocX;

		private SettingEntry<int> _profitLocY;

		private int _fineFilletPriceCopper;

		private Blish_HUD.Controls.Panel _profitWidgetPanel;

		private Blish_HUD.Controls.Label _profitTotalLabel;

		private Blish_HUD.Controls.Label _gphLabel;

		private bool _isProfitDragging;

		private Point _profitDragOffset;

		private int _ambergrisPriceCopper;

		private int _filletPriceCopper;

		private int _sessionTotalCopper;

		private DateTime _sessionStartTime = DateTime.MinValue;

		private ClientWebSocket _drfSocket;

		private CancellationTokenSource _drfCts;

		private Task _drfReceiveTask;

		private static readonly Random _rnd = new Random();

		private bool _isCasualLoggingActive;

		private DateTime _lastSubmitTime = DateTime.MinValue;

		private DateTime _lastAutoSubmitTime = DateTime.Now;

		private string _localAccountName = "UnknownAccount";

		private Blish_HUD.Controls.Panel _leaderboardWindow;

		private Dropdown _lbSortDropdown;

		private FlowPanel _lbListPanel;

		private bool _isDraggingLeaderboard;

		private Point _leaderboardDragOffset;

		private List<LeaderboardEntry> _cachedLeaderboardData;

		private DateTime _lastLeaderboardFetchTime = DateTime.MinValue;

		private Blish_HUD.Controls.Panel _speciesSelectionWindow;

		private StandardButton _speciesFilterBtn;

		private string _currentlySelectedSpecies = "__ALL_SPECIES__";

		private Blish_HUD.Controls.TextBox _speciesSearchBox;

		private string[] _junkMessages;

		private string[] _treasureMessages;

		private Blish_HUD.Controls.Panel _achievementPanel;

		private Blish_HUD.Controls.Panel _achievementResultsPanel;

		private Blish_HUD.Controls.Panel _achievementLegendPanel;

		private bool _isDraggingAchievement;

		private Point _achievementDragOffset;

		private bool _isSpeciesSelectionDragging;

		private Point _speciesSelectionDragOffset;

		private Blish_HUD.Controls.Panel _metaProgressWindow;

		private Point _metaDragOffset;

		private bool _isMetaDragging;

		private List<FlowPanel> _allMetaSubPanels = new List<FlowPanel>();

		private Blish_HUD.Controls.Panel _currentlyExpandedRow;

		private SettingEntry<bool> _autoStartBitingWidget;

		private Blish_HUD.Controls.Panel _bitingWidgetPanel;

		private SettingEntry<string> _selectedLanguage;

		private FlowPanel _bitingFishList;

		private Blish_HUD.Controls.Label _bitingTitleLabel;

		private bool _isBitingDragging;

		private Point _bitingDragOffset;

		private int _lastBitingMapId;

		private SettingEntry<int> _bitingLocX;

		private SettingEntry<int> _bitingLocY;

		private bool _showAllBitingFish;

		private StandardButton _bitingToggleBtn;

		private bool _isTournamentActive;

		private bool _isTourneyWaitingRoom;

		private DateTime _tourneyStartTimeUtc;

		private DateTime _tourneyEndTimeUtc;

		private string _tourneyRoomCode = "";

		private Dropdown _hostWinFactorDrop;

		private int _tourneyTargetItemId;

		private string _tourneyWinFactor = "Weight";

		private Blish_HUD.Controls.Label _casualSyncTimerLabel;

		private DateTime _nextSyncTime;

		private bool _isSyncTimerActive;

		private StandardButton _casualLogToggleBtn;

		private StandardButton _casualMeasureBtn;

		private Checkbox _useDrfCheckbox;

		private string _lastGeneratedCode = "";

		private bool _isDragging;

		private bool _isActivePanelDragging;

		private bool _isDraggingSummary;

		private bool _isCompactDragging;

		private bool _isDraggingTarget;

		private Point _dragOffset;

		private Point _activePanelDragOffset;

		private Point _summaryDragOffset;

		private Point _compactDragOffset;

		private Point _targetDragOffset;

		private Blish_HUD.Controls.Panel _currentSummaryWindow;

		private Blish_HUD.Controls.Panel _targetSelectionWindow;

		private Blish_HUD.Controls.Panel _casualCompactPanel;

		private FlowPanel _compactCoolerList;

		private StandardButton _compactFishLogBtn;

		private StandardButton _compactMaxBtn;

		private string _tourneyModeUsed = "API";

		private bool _isTourneyWrapUpActive;

		private DateTime _tourneyWrapUpEndTime;

		private bool _didMidWrapUpPing;

		private bool _isCheater;

		private Blish_HUD.Controls.Label _cheaterLabel;

		private double _cheaterTimer = 25.0;

		public static GilledWars Instance { get; private set; }

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		private string ModuleDirectory => DirectoriesManager.GetFullDirectoryPath("gilledwars");

		private SettingEntry<KeyBinding> ToggleHotkey { get; set; }

		private SettingEntry<string> _customApiKey { get; set; }

		private SettingEntry<string> _drfToken { get; set; }

		private SettingEntry<string> _discordWebhookUrl { get; set; }

		private static HttpClient CreateHttpClient()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			((HttpHeaders)val.get_DefaultRequestHeaders()).Add("X-Gilled-Wars-Client", "SecureBlishModule_v1");
			return val;
		}

		[ImportingConstructor]
		public GilledWars([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			ToggleHotkey = settings.DefineSetting("ToggleHotkey", new KeyBinding(ModifierKeys.Ctrl | ModifierKeys.Alt, Microsoft.Xna.Framework.Input.Keys.F), () => "Toggle UI", () => "Open/Close UI");
			_customApiKey = settings.DefineSetting("CustomApiKey", "", () => "Custom API Key", () => "Paste an API key with 'inventories', 'characters', and 'progression' permissions here.");
			_drfToken = settings.DefineSetting("DrfToken", "", () => "DRF Token", () => "Paste your drf.rs token here for Real-Time tracking.");
			_discordWebhookUrl = settings.DefineSetting("DiscordWebhookUrl", "", () => "Discord Webhook (Host Only)", () => "Paste a Discord channel Webhook URL to automatically post your tournament results!");
			_cornerIconChoice = settings.DefineSetting("CornerIconChoice", CornerIconType.Hook, () => "Corner Icon", () => "Choose the icon displayed in the top-left menu.");
			_autoStartLogging = settings.DefineSetting("AutoStartLogging", defaultValue: false, () => "Auto-Start Logging", () => "Automatically starts Casual Logging when you load into the game.");
			_autoStartBitingWidget = settings.DefineSetting("AutoStartBitingWidget", defaultValue: false, () => "Auto-Open Biting Widget", () => "Automatically opens the 'Currently Biting' widget on load based on your map.");
			_bitingLocX = settings.DefineSetting("BitingLocX", 10);
			_bitingLocY = settings.DefineSetting("BitingLocY", 300);
			_showProfitWidget = settings.DefineSetting("ShowProfitWidget", defaultValue: false, () => "Show Live Profit Widget", () => "Displays an estimated session profit and GpH tracker.");
			_profitLocX = settings.DefineSetting("ProfitLocX", 10);
			_profitLocY = settings.DefineSetting("ProfitLocY", 500);
			_showTimeOfDayWidget = settings.DefineSetting("ShowTimeOfDay", defaultValue: true, () => "Show Time of Day", () => "Displays a widget showing current Tyrian time.");
			_lockTimeOfDayWidget = settings.DefineSetting("LockTimeOfDay", defaultValue: false, () => "Lock Time of Day Widget", () => "Prevents the widget from being dragged accidentally.");
			_todWidgetSize = settings.DefineSetting("TodWidgetSize", 48, () => "Widget Icon Size", () => "Changes the size of the Time of Day icon.");
			_todWidgetSize.SetRange(16, 128);
			_todTextLayout = settings.DefineSetting("TodTextLayout", TodTextLayout.Right, () => "Text Layout", () => "Where should the time text be relative to the icon?");
			_todLocX = settings.DefineSetting("TodLocX", 100);
			_todLocY = settings.DefineSetting("TodLocY", 100);
			_selectedLanguage = settings.DefineSetting("SelectedLanguage", "auto", () => "UI Language", () => "Language used for the Gilled Wars interface. 'auto' follows your BlishHUD language setting.");
		}

		private AsyncTexture2D GetCornerIconTexture(CornerIconType type)
		{
			return type switch
			{
				CornerIconType.Bait => ContentsManager.GetTexture("images/bait.png"), 
				CornerIconType.Hook => ContentsManager.GetTexture("images/hook.png"), 
				CornerIconType.Hook2 => ContentsManager.GetTexture("images/hook2.png"), 
				CornerIconType.Lure => ContentsManager.GetTexture("images/lure.png"), 
				CornerIconType.Net => ContentsManager.GetTexture("images/net.png"), 
				CornerIconType.FishMaster => ContentsManager.GetTexture("images/fishmaster.png"), 
				_ => ContentsManager.GetTexture("images/hook.png"), 
			};
		}

		private void OnMouseLeftButtonReleased(object sender, Blish_HUD.Input.MouseEventArgs e)
		{
			if (_isTodDragging && _timeOfDayPanel != null)
			{
				_todLocX.Value = _timeOfDayPanel.Location.X;
				_todLocY.Value = _timeOfDayPanel.Location.Y;
			}
			if (_isBitingDragging && _bitingWidgetPanel != null)
			{
				_bitingLocX.Value = _bitingWidgetPanel.Location.X;
				_bitingLocY.Value = _bitingWidgetPanel.Location.Y;
			}
			if (_isProfitDragging && _profitWidgetPanel != null)
			{
				_profitLocX.Value = _profitWidgetPanel.Location.X;
				_profitLocY.Value = _profitWidgetPanel.Location.Y;
			}
			_isProfitDragging = false;
			_isBitingDragging = false;
			_isDragging = false;
			_isActivePanelDragging = false;
			_isDraggingSummary = false;
			_isCompactDragging = false;
			_isDraggingTarget = false;
			_isDraggingLeaderboard = false;
			_isDraggingAchievement = false;
			_isSpeciesSelectionDragging = false;
			_isMetaDragging = false;
			_isTodDragging = false;
		}

		private string DetectLanguage()
		{
			if (_selectedLanguage.Value != "auto" && !string.IsNullOrEmpty(_selectedLanguage.Value))
			{
				return _selectedLanguage.Value;
			}
			return GameService.Overlay.UserLocale.Value switch
			{
				Locale.French => "fr", 
				Locale.German => "de", 
				Locale.Spanish => "es", 
				_ => "en", 
			};
		}

		private void ApplyLanguage(string langCode)
		{
			_selectedLanguage.Value = langCode;
			CultureInfo culture = new CultureInfo(langCode);
			Thread.CurrentThread.CurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			strings.Culture = culture;
			_junkMessages = strings.JunkMessages.Split('|');
			_treasureMessages = strings.TreasureMessages.Split('|');
			if (_mainWindow != null)
			{
				_mainWindow.Dispose();
				_mainWindow = null;
			}
			if (_casualCompactPanel != null)
			{
				_casualCompactPanel.Dispose();
				_casualCompactPanel = null;
			}
			BuildMainWindow();
			BuildCasualCompactPanel();
			if (_profitWidgetPanel != null)
			{
				_profitWidgetPanel.Dispose();
				_profitWidgetPanel = null;
			}
			BuildProfitWidget();
			if (_timeOfDayPanel != null)
			{
				_timeOfDayPanel.Dispose();
				_timeOfDayPanel = null;
			}
			BuildTimeOfDayWidget();
			if (_bitingWidgetPanel != null)
			{
				bool visible = _bitingWidgetPanel.Visible;
				_bitingWidgetPanel.Dispose();
				_bitingWidgetPanel = null;
				if (visible)
				{
					ToggleBitingWidget(forceOpen: true);
				}
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			CultureInfo culture = new CultureInfo(DetectLanguage());
			Thread.CurrentThread.CurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			strings.Culture = culture;
			_junkMessages = strings.JunkMessages.Split('|');
			_treasureMessages = strings.TreasureMessages.Split('|');
			Logger.Info("[GilledWars] Culture: " + culture.Name + " | Test: " + strings.StartLoggingBtnText);
			Logger.Info("Gw2ApiClient Full Type: " + Gw2ApiManager.Gw2ApiClient.GetType().FullName);
			LoadFishDatabase();
			InitializeAccountAndLoadAsync();
			LocalizeFishNamesAsync();
			BuildMainWindow();
			BuildCasualCompactPanel();
			GameService.Input.Mouse.LeftMouseButtonReleased += OnMouseLeftButtonReleased;
			_cornerIcon = new CornerIcon
			{
				Icon = GetCornerIconTexture(_cornerIconChoice.Value),
				BasicTooltipText = strings.CornerIconTooltip,
				Priority = 5
			};
			_cornerIconChoice.SettingChanged += delegate(object s, ValueChangedEventArgs<CornerIconType> ev)
			{
				_cornerIcon.Icon = GetCornerIconTexture(ev.NewValue);
			};
			_cornerIcon.Click += delegate
			{
				if (_casualCompactPanel != null && _casualCompactPanel.Visible)
				{
					_casualCompactPanel.Visible = false;
					if (_mainWindow != null)
					{
						_mainWindow.Visible = true;
					}
					ScreenNotification.ShowNotification(strings.ExpandingMainView);
				}
				else if (_mainWindow != null)
				{
					_mainWindow.Visible = !_mainWindow.Visible;
				}
			};
			ToggleHotkey.Value.Enabled = true;
			ToggleHotkey.Value.Activated += OnToggleHotkeyActivated;
			_cheaterLabel = new Blish_HUD.Controls.Label
			{
				Parent = GameService.Graphics.SpriteScreen,
				TextColor = Microsoft.Xna.Framework.Color.Red,
				Font = GameService.Content.DefaultFont32,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Visible = false,
				ZIndex = 9999,
				StrokeText = true,
				ShowShadow = true
			};
			_texDawn = ContentsManager.GetTexture("images/tod_dawn.png");
			_texDay = ContentsManager.GetTexture("images/tod_day.png");
			_texDusk = ContentsManager.GetTexture("images/tod_dusk.png");
			_texNight = ContentsManager.GetTexture("images/tod_night.png");
			_texCaughtCheck = ContentsManager.GetTexture("images/caught_check.png");
			BuildTimeOfDayWidget();
			RefreshFishLogUI();
			if (_autoStartLogging.Value)
			{
				StartCasualLogging();
			}
			if (_autoStartBitingWidget.Value)
			{
				ToggleBitingWidget(forceOpen: true);
			}
			FetchTradingPostPrices();
			BuildProfitWidget();
			base.OnModuleLoaded(e);
		}

		private async Task FetchTradingPostPrices()
		{
			try
			{
				IReadOnlyList<CommercePrices> source = await Gw2ApiManager.Gw2ApiClient.V2.Commerce.Prices.ManyAsync(new int[3] { 96347, 95673, 96762 });
				CommercePrices ambergris = source.FirstOrDefault((CommercePrices p) => p.Id == 96347);
				CommercePrices fillet = source.FirstOrDefault((CommercePrices p) => p.Id == 95673);
				CommercePrices fineFillet = source.FirstOrDefault((CommercePrices p) => p.Id == 96762);
				if (ambergris != null)
				{
					_ambergrisPriceCopper = ambergris.Sells.UnitPrice;
				}
				if (fillet != null)
				{
					_filletPriceCopper = fillet.Sells.UnitPrice;
				}
				if (fineFillet != null)
				{
					_fineFilletPriceCopper = fineFillet.Sells.UnitPrice;
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch TP prices for Profit Tracker.");
			}
			finally
			{
				if (_ambergrisPriceCopper <= 0)
				{
					_ambergrisPriceCopper = 15000;
				}
				if (_filletPriceCopper <= 0)
				{
					_filletPriceCopper = 200;
				}
				if (_fineFilletPriceCopper <= 0)
				{
					_fineFilletPriceCopper = 25;
				}
			}
		}

		private string FormatMoney(int totalCopper)
		{
			int g = totalCopper / 10000;
			int s = totalCopper % 10000 / 100;
			int c = totalCopper % 100;
			return $"{g}g {s}s {c}c";
		}

		private void BuildProfitWidget()
		{
			_profitWidgetPanel = new Blish_HUD.Controls.Panel
			{
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(220, 65),
				Location = new Point(_profitLocX.Value, _profitLocY.Value),
				BackgroundColor = new Microsoft.Xna.Framework.Color(13, 27, 42) * 0.9f,
				ShowBorder = true,
				Visible = _showProfitWidget.Value,
				ZIndex = 950
			};
			Blish_HUD.Controls.Panel hBar = new Blish_HUD.Controls.Panel
			{
				Parent = _profitWidgetPanel,
				Size = new Point(220, 20),
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.6f,
				Location = new Point(0, 0)
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.ProfitTrackerBtnText,
				Parent = hBar,
				Location = new Point(5, 0),
				Font = GameService.Content.DefaultFont14,
				TextColor = new Microsoft.Xna.Framework.Color(201, 168, 76),
				AutoSizeWidth = true
			};
			new Blish_HUD.Controls.Label
			{
				Text = "[WIP]",
				Parent = hBar,
				Location = new Point(180, 2),
				Font = GameService.Content.DefaultFont12,
				TextColor = Microsoft.Xna.Framework.Color.Orange,
				AutoSizeWidth = true
			};
			hBar.LeftMouseButtonPressed += delegate
			{
				_isProfitDragging = true;
				_profitDragOffset = new Point(GameService.Input.Mouse.Position.X - _profitWidgetPanel.Location.X, GameService.Input.Mouse.Position.Y - _profitWidgetPanel.Location.Y);
			};
			_profitTotalLabel = new Blish_HUD.Controls.Label
			{
				Text = strings.ProfitTotalDefault,
				Parent = _profitWidgetPanel,
				Location = new Point(10, 25),
				Font = GameService.Content.DefaultFont14,
				TextColor = Microsoft.Xna.Framework.Color.LimeGreen,
				AutoSizeWidth = true
			};
			_gphLabel = new Blish_HUD.Controls.Label
			{
				Text = strings.GphDefault,
				Parent = _profitWidgetPanel,
				Location = new Point(10, 45),
				Font = GameService.Content.DefaultFont12,
				TextColor = Microsoft.Xna.Framework.Color.LightGray,
				AutoSizeWidth = true
			};
			_showProfitWidget.SettingChanged += delegate(object s, ValueChangedEventArgs<bool> e)
			{
				_profitWidgetPanel.Visible = e.NewValue;
			};
		}

		private void UpdateTodLayout()
		{
			if (_timeOfDayPanel != null && _todIcon != null && _todLabel != null)
			{
				int iconSize = _todWidgetSize.Value;
				_todIcon.Size = new Point(iconSize, iconSize);
				int textW = 90;
				int textH = 20;
				int pw = Math.Max(textW, iconSize);
				switch (_todTextLayout.Value)
				{
				case TodTextLayout.Right:
					_todIcon.Location = new Point(0, 0);
					_todLabel.Size = new Point(textW, textH);
					_todLabel.Location = new Point(iconSize + 5, (iconSize - textH) / 2);
					_todLabel.HorizontalAlignment = Blish_HUD.Controls.HorizontalAlignment.Left;
					_timeOfDayPanel.Size = new Point(iconSize + 5 + textW, Math.Max(iconSize, textH));
					_todLabel.Visible = true;
					break;
				case TodTextLayout.Left:
					_todLabel.Size = new Point(textW, textH);
					_todLabel.Location = new Point(0, (iconSize - textH) / 2);
					_todLabel.HorizontalAlignment = Blish_HUD.Controls.HorizontalAlignment.Right;
					_todIcon.Location = new Point(textW + 5, 0);
					_timeOfDayPanel.Size = new Point(textW + 5 + iconSize, Math.Max(iconSize, textH));
					_todLabel.Visible = true;
					break;
				case TodTextLayout.Bottom:
					_todIcon.Location = new Point((pw - iconSize) / 2, 0);
					_todLabel.Size = new Point(textW, textH);
					_todLabel.Location = new Point((pw - textW) / 2, iconSize + 2);
					_todLabel.HorizontalAlignment = Blish_HUD.Controls.HorizontalAlignment.Center;
					_timeOfDayPanel.Size = new Point(pw, iconSize + 2 + textH);
					_todLabel.Visible = true;
					break;
				case TodTextLayout.Top:
					_todLabel.Size = new Point(textW, textH);
					_todLabel.Location = new Point((pw - textW) / 2, 0);
					_todLabel.HorizontalAlignment = Blish_HUD.Controls.HorizontalAlignment.Center;
					_todIcon.Location = new Point((pw - iconSize) / 2, textH + 2);
					_timeOfDayPanel.Size = new Point(pw, textH + 2 + iconSize);
					_todLabel.Visible = true;
					break;
				case TodTextLayout.OnImage:
					_todIcon.Location = new Point(0, 0);
					_todLabel.Size = new Point(iconSize, textH);
					_todLabel.Location = new Point(0, (iconSize - textH) / 2);
					_todLabel.HorizontalAlignment = Blish_HUD.Controls.HorizontalAlignment.Center;
					_timeOfDayPanel.Size = new Point(iconSize, iconSize);
					_todLabel.Visible = true;
					break;
				case TodTextLayout.Hidden:
					_todIcon.Location = new Point(0, 0);
					_timeOfDayPanel.Size = new Point(iconSize, iconSize);
					_todLabel.Visible = false;
					break;
				}
			}
		}

		private void BuildTimeOfDayWidget()
		{
			_timeOfDayPanel = new Blish_HUD.Controls.Panel
			{
				Parent = GameService.Graphics.SpriteScreen,
				Location = new Point(_todLocX.Value, _todLocY.Value),
				BackgroundColor = Microsoft.Xna.Framework.Color.Transparent,
				ShowBorder = false,
				Visible = _showTimeOfDayWidget.Value,
				ZIndex = 900
			};
			_todIcon = new Image
			{
				Parent = _timeOfDayPanel,
				Texture = _texDay
			};
			_todLabel = new Blish_HUD.Controls.Label
			{
				Parent = _timeOfDayPanel,
				Font = GameService.Content.DefaultFont16,
				TextColor = Microsoft.Xna.Framework.Color.White,
				ShowShadow = true,
				StrokeText = true,
				Text = strings.Loading
			};
			UpdateTodLayout();
			_todWidgetSize.SettingChanged += delegate
			{
				UpdateTodLayout();
			};
			_todTextLayout.SettingChanged += delegate
			{
				UpdateTodLayout();
			};
			_showTimeOfDayWidget.SettingChanged += delegate(object s, ValueChangedEventArgs<bool> e)
			{
				_timeOfDayPanel.Visible = e.NewValue;
			};
			_timeOfDayPanel.LeftMouseButtonPressed += delegate
			{
				if (!_lockTimeOfDayWidget.Value && (GameService.Input.Mouse.ActiveControl == _timeOfDayPanel || GameService.Input.Mouse.ActiveControl == _todLabel || GameService.Input.Mouse.ActiveControl == _todIcon))
				{
					_isTodDragging = true;
					_todDragOffset = new Point(GameService.Input.Mouse.Position.X - _timeOfDayPanel.Location.X, GameService.Input.Mouse.Position.Y - _timeOfDayPanel.Location.Y);
				}
			};
		}

		private void ShowLeaderboardWindow()
		{
			Microsoft.Xna.Framework.Color deepNavyBg = new Microsoft.Xna.Framework.Color(13, 27, 42);
			new Microsoft.Xna.Framework.Color(26, 47, 69);
			Microsoft.Xna.Framework.Color agedGoldText = new Microsoft.Xna.Framework.Color(201, 168, 76);
			if (_leaderboardWindow != null)
			{
				_leaderboardWindow.Visible = true;
				return;
			}
			_leaderboardWindow = new Blish_HUD.Controls.Panel
			{
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(480, 620),
				Location = new Point(400, 150),
				ShowBorder = true,
				BackgroundColor = deepNavyBg,
				ZIndex = 1000,
				ClipsBounds = false
			};
			Blish_HUD.Controls.Panel headerBar = new Blish_HUD.Controls.Panel
			{
				Parent = _leaderboardWindow,
				Size = new Point(_leaderboardWindow.Width, 30),
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.6f,
				Location = new Point(0, 0)
			};
			_lbDynamicTitleLabel = new Blish_HUD.Controls.Label
			{
				Text = strings.LeaderboardTitle,
				Parent = headerBar,
				Location = new Point(10, 5),
				Font = GameService.Content.DefaultFont16,
				TextColor = agedGoldText,
				AutoSizeWidth = true
			};
			Blish_HUD.Controls.Label closeX = new Blish_HUD.Controls.Label
			{
				Text = "X",
				Parent = headerBar,
				Location = new Point(headerBar.Width - 25, 5),
				Font = GameService.Content.DefaultFont16,
				TextColor = Microsoft.Xna.Framework.Color.Red,
				AutoSizeWidth = true
			};
			closeX.Click += delegate
			{
				_leaderboardWindow.Visible = false;
				if (_speciesSelectionWindow != null)
				{
					_speciesSelectionWindow.Visible = false;
				}
			};
			closeX.MouseEntered += delegate
			{
				closeX.TextColor = Microsoft.Xna.Framework.Color.White;
			};
			closeX.MouseLeft += delegate
			{
				closeX.TextColor = Microsoft.Xna.Framework.Color.Red;
			};
			headerBar.LeftMouseButtonPressed += delegate
			{
				_isDraggingLeaderboard = true;
				_leaderboardDragOffset = new Point(GameService.Input.Mouse.Position.X - _leaderboardWindow.Location.X, GameService.Input.Mouse.Position.Y - _leaderboardWindow.Location.Y);
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.SortLabel,
				Parent = _leaderboardWindow,
				Location = new Point(10, 45),
				AutoSizeWidth = true
			};
			_lbSortDropdown = new Dropdown
			{
				Parent = _leaderboardWindow,
				Location = new Point(50, 40),
				Width = 90
			};
			_lbSortDropdown.Items.Add(strings.WeightDropdown);
			_lbSortDropdown.Items.Add(strings.LengthDropdown);
			_lbSortDropdown.SelectedItem = strings.WeightDropdown;
			_lbSortDropdown.ValueChanged += async delegate
			{
				await RefreshLeaderboardData();
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.FishLabel,
				Parent = _leaderboardWindow,
				Location = new Point(150, 45),
				AutoSizeWidth = true
			};
			_speciesFilterBtn = new StandardButton
			{
				Text = strings.AllSpeciesFilterBtn,
				Parent = _leaderboardWindow,
				Location = new Point(190, 40),
				Width = 140
			};
			_speciesFilterBtn.Click += delegate
			{
				ShowSpeciesPicker();
			};
			StandardButton refreshBtn = new StandardButton
			{
				Text = strings.RefreshBtn,
				Parent = _leaderboardWindow,
				Location = new Point(350, 40),
				Width = 90
			};
			refreshBtn.Click += async delegate
			{
				double elapsedMinutes = (DateTime.Now - _lastLeaderboardFetchTime).TotalMinutes;
				if (elapsedMinutes < 5.0 && _cachedLeaderboardData != null)
				{
					int remaining = 5 - (int)elapsedMinutes;
					ScreenNotification.ShowNotification(string.Format(strings.RefreshCooldown, remaining), ScreenNotification.NotificationType.Warning);
				}
				else
				{
					refreshBtn.Enabled = false;
					_cachedLeaderboardData = null;
					_lastLeaderboardFetchTime = DateTime.MinValue;
					await RefreshLeaderboardData();
					refreshBtn.Enabled = true;
					ScreenNotification.ShowNotification(strings.LeaderboardRefreshed);
				}
			};
			_lbListPanel = new FlowPanel
			{
				Parent = _leaderboardWindow,
				Location = new Point(10, 85),
				Size = new Point(460, 520),
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			_leaderboardWindow.Visible = true;
			RefreshLeaderboardData();
		}

		private async Task RefreshLeaderboardData()
		{
			if (_lbListPanel == null || _leaderboardWindow == null)
			{
				return;
			}
			_lbListPanel.ClearChildren();
			new Blish_HUD.Controls.Label
			{
				Text = strings.LoadingData,
				Parent = _lbListPanel,
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Yellow
			};
			try
			{
				string sortMode = ((_lbSortDropdown.SelectedItem == strings.LengthDropdown) ? "length" : "weight");
				string selectedSpecies = _currentlySelectedSpecies;
				if (_cachedLeaderboardData != null && !((DateTime.Now - _lastLeaderboardFetchTime).TotalMinutes >= 10.0))
				{
					goto IL_0218;
				}
				HttpResponseMessage response = await _httpClient.GetAsync("https://api.gilledwars.com/get-leaderboard");
				if (response.get_IsSuccessStatusCode())
				{
					_cachedLeaderboardData = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(await response.get_Content().ReadAsStringAsync());
					_lastLeaderboardFetchTime = DateTime.Now;
					goto IL_0218;
				}
				_lbListPanel.ClearChildren();
				new Blish_HUD.Controls.Label
				{
					Text = strings.ServerError,
					Parent = _lbListPanel,
					TextColor = Microsoft.Xna.Framework.Color.Red,
					AutoSizeWidth = true
				};
				goto end_IL_006d;
				IL_0218:
				_lbListPanel.ClearChildren();
				if (_lbDynamicTitleLabel != null)
				{
					_lbDynamicTitleLabel.Text = ((selectedSpecies == "__ALL_SPECIES__") ? ("Global Top 10 (" + sortMode.ToUpper() + ")") : ("Top 10 " + selectedSpecies));
				}
				if (_cachedLeaderboardData == null || _cachedLeaderboardData.Count == 0)
				{
					new Blish_HUD.Controls.Label
					{
						Text = strings.NoRecordsFound,
						Parent = _lbListPanel,
						AutoSizeWidth = true
					};
					return;
				}
				IEnumerable<LeaderboardEntry> filteredRecords = _cachedLeaderboardData.Where((LeaderboardEntry r) => r.RecordType == sortMode);
				if (selectedSpecies != "__ALL_SPECIES__")
				{
					filteredRecords = filteredRecords.Where((LeaderboardEntry r) => r.FishName.Equals(selectedSpecies, StringComparison.OrdinalIgnoreCase));
				}
				List<LeaderboardEntry> top10List = filteredRecords.OrderByDescending((LeaderboardEntry r) => (!(sortMode == "weight")) ? r.Length : r.Weight).Take(10).ToList();
				if (top10List.Count == 0)
				{
					new Blish_HUD.Controls.Label
					{
						Text = strings.NoCatchesYet,
						Parent = _lbListPanel,
						AutoSizeWidth = true,
						TextColor = Microsoft.Xna.Framework.Color.LightGray
					};
					return;
				}
				Blish_HUD.Controls.Panel headerRow = new Blish_HUD.Controls.Panel
				{
					Parent = _lbListPanel,
					Width = _lbListPanel.Width - 20,
					Height = 30
				};
				new Blish_HUD.Controls.Label
				{
					Text = strings.RankLabel,
					Parent = headerRow,
					Location = new Point(5, 5),
					Width = 45,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Blish_HUD.Controls.Label
				{
					Text = strings.AnglerLabel,
					Parent = headerRow,
					Location = new Point(65, 5),
					Width = 160,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Blish_HUD.Controls.Label
				{
					Text = strings.SpeciesLabel,
					Parent = headerRow,
					Location = new Point(235, 5),
					Width = 110,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Blish_HUD.Controls.Label
				{
					Text = ((sortMode == "weight") ? "Weight" : "Length"),
					Parent = headerRow,
					Location = new Point(355, 5),
					Width = 65,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Image
				{
					Texture = ContentService.Textures.Pixel,
					Parent = _lbListPanel,
					Width = _lbListPanel.Width - 25,
					Height = 2,
					Tint = Microsoft.Xna.Framework.Color.Gray * 0.5f
				};
				int rank = 1;
				Microsoft.Xna.Framework.Color customSilver = new Microsoft.Xna.Framework.Color(190, 210, 230);
				Microsoft.Xna.Framework.Color customBronze = new Microsoft.Xna.Framework.Color(205, 127, 50);
				foreach (LeaderboardEntry entry in top10List)
				{
					Blish_HUD.Controls.Panel row = new Blish_HUD.Controls.Panel
					{
						Parent = _lbListPanel,
						Width = _lbListPanel.Width - 25,
						Height = 40
					};
					Microsoft.Xna.Framework.Color rankColor = rank switch
					{
						3 => customBronze, 
						2 => customSilver, 
						1 => Microsoft.Xna.Framework.Color.Gold, 
						_ => Microsoft.Xna.Framework.Color.White, 
					};
					new Blish_HUD.Controls.Label
					{
						Text = $"#{rank}",
						Parent = row,
						Location = new Point(5, 10),
						Width = 45,
						TextColor = rankColor,
						Font = GameService.Content.DefaultFont18
					};
					new Image
					{
						Texture = ContentService.Textures.Pixel,
						Parent = row,
						Location = new Point(55, 5),
						Width = 1,
						Height = 30,
						Tint = Microsoft.Xna.Framework.Color.White * 0.1f
					};
					string countryCode = (entry.Country ?? "xx").ToLower();
					int nameXPos = 65;
					if (countryCode != "xx")
					{
						try
						{
							Texture2D flagTex = ContentsManager.GetTexture("flags/" + countryCode + ".png");
							new Image
							{
								Texture = flagTex,
								Parent = row,
								Location = new Point(65, 12),
								Size = new Point(24, 16)
							};
							nameXPos = 95;
						}
						catch
						{
						}
					}
					new Blish_HUD.Controls.Label
					{
						Text = entry.PlayerName,
						Parent = row,
						Location = new Point(nameXPos, 10),
						Width = 225 - nameXPos,
						TextColor = rankColor,
						Font = GameService.Content.DefaultFont14,
						AutoSizeWidth = false
					};
					new Image
					{
						Texture = ContentService.Textures.Pixel,
						Parent = row,
						Location = new Point(230, 5),
						Width = 1,
						Height = 30,
						Tint = Microsoft.Xna.Framework.Color.White * 0.1f
					};
					int speciesXPos = 235;
					FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.Name.Equals(entry.FishName, StringComparison.OrdinalIgnoreCase))?.Data;
					if (dbFish != null)
					{
						string safeName = dbFish.Name.Replace(" ", "_").Replace("'", "").Replace("-", "");
						new Image
						{
							Texture = ContentsManager.GetTexture("images/" + safeName + ".png"),
							Parent = row,
							Location = new Point(235, 4),
							Size = new Point(32, 32),
							BasicTooltipText = dbFish.Name
						};
						speciesXPos = 270;
					}
					new Blish_HUD.Controls.Label
					{
						Text = entry.FishName,
						Parent = row,
						Location = new Point(speciesXPos, 10),
						Width = 350 - speciesXPos,
						TextColor = Microsoft.Xna.Framework.Color.LightGray,
						WrapText = false,
						Font = GameService.Content.DefaultFont12
					};
					new Image
					{
						Texture = ContentService.Textures.Pixel,
						Parent = row,
						Location = new Point(350, 5),
						Width = 1,
						Height = 30,
						Tint = Microsoft.Xna.Framework.Color.White * 0.1f
					};
					string statText = ((sortMode == "weight") ? $"{entry.Weight} lbs" : $"{entry.Length} in");
					new Blish_HUD.Controls.Label
					{
						Text = statText,
						Parent = row,
						Location = new Point(355, 10),
						Width = 75,
						TextColor = Microsoft.Xna.Framework.Color.White,
						Font = GameService.Content.DefaultFont14
					};
					new Image
					{
						Texture = ContentService.Textures.Pixel,
						Parent = _lbListPanel,
						Width = _lbListPanel.Width - 25,
						Height = 1,
						Tint = Microsoft.Xna.Framework.Color.White * 0.15f
					};
					rank++;
				}
				end_IL_006d:;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to load in-game leaderboard.");
				_lbListPanel.ClearChildren();
				new Blish_HUD.Controls.Label
				{
					Text = strings.NetworkError,
					Parent = _lbListPanel,
					TextColor = Microsoft.Xna.Framework.Color.Red,
					AutoSizeWidth = true
				};
			}
		}

		private async Task ShowMetaProgressWindow()
		{
			Microsoft.Xna.Framework.Color deepNavyBg = new Microsoft.Xna.Framework.Color(13, 27, 42);
			Microsoft.Xna.Framework.Color darkTealPanel = new Microsoft.Xna.Framework.Color(26, 47, 69);
			Microsoft.Xna.Framework.Color agedGoldText = new Microsoft.Xna.Framework.Color(201, 168, 76);
			if (_metaProgressWindow == null)
			{
				_metaProgressWindow = new Blish_HUD.Controls.Panel
				{
					ShowBorder = true,
					Size = new Point(540, 550),
					Location = new Point(350, 150),
					Parent = GameService.Graphics.SpriteScreen,
					BackgroundColor = deepNavyBg,
					ZIndex = 1100,
					ClipsBounds = false
				};
			}
			_metaProgressWindow.ClearChildren();
			_allMetaSubPanels.Clear();
			_currentlyExpandedRow = null;
			_metaProgressWindow.Visible = true;
			FlowPanel scroll = new FlowPanel
			{
				Parent = _metaProgressWindow,
				Size = new Point(_metaProgressWindow.Width - 10, _metaProgressWindow.Height - 40),
				Location = new Point(5, 35),
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				CanScroll = true,
				ControlPadding = new Vector2(0f, 5f)
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.FetchingAchievements,
				Parent = scroll,
				Font = GameService.Content.DefaultFont14,
				TextColor = Microsoft.Xna.Framework.Color.Yellow,
				AutoSizeWidth = true
			};
			try
			{
				List<int> metaIds = new List<int> { 6201, 6478, 6109, 6284, 6279, 6111 };
				int[] metaMaxes = new int[6] { 5, 10, 15, 20, 25, 30 };
				int[] base30 = new int[30]
				{
					6068, 6179, 6330, 6344, 6363, 6317, 6106, 6489, 6336, 6342,
					6258, 6506, 6471, 6224, 6439, 6505, 6263, 6153, 6484, 6475,
					6227, 6509, 6250, 6339, 6264, 6192, 6466, 6402, 6393, 6110
				};
				List<int> allTargetIds = metaIds.Concat(base30).Distinct().ToList();
				IReadOnlyList<Achievement> allDefs = await Gw2ApiManager.Gw2ApiClient.V2.Achievements.ManyAsync(allTargetIds);
				IApiV2ObjectList<AccountAchievement> accAchievements = await Gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync();
				int realCompletedCollections = 0;
				int[] array = base30;
				foreach (int bId in array)
				{
					if (accAchievements.FirstOrDefault((AccountAchievement a) => a.Id == bId)?.Done ?? false)
					{
						realCompletedCollections++;
					}
				}
				scroll.ClearChildren();
				Blish_HUD.Controls.Panel hBar = new Blish_HUD.Controls.Panel
				{
					Parent = _metaProgressWindow,
					Size = new Point(_metaProgressWindow.Width, 30),
					BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.6f,
					Location = new Point(0, 0)
				};
				new Blish_HUD.Controls.Label
				{
					Text = strings.MetaTrackerTitle,
					Parent = hBar,
					Location = new Point(10, 5),
					Font = GameService.Content.DefaultFont16,
					TextColor = agedGoldText,
					AutoSizeWidth = true
				};
				Blish_HUD.Controls.Label closeX = new Blish_HUD.Controls.Label
				{
					Text = "X",
					Parent = hBar,
					Location = new Point(hBar.Width - 25, 5),
					Font = GameService.Content.DefaultFont16,
					TextColor = Microsoft.Xna.Framework.Color.Red,
					AutoSizeWidth = true
				};
				closeX.Click += delegate
				{
					_metaProgressWindow.Visible = false;
				};
				closeX.MouseEntered += delegate
				{
					closeX.TextColor = Microsoft.Xna.Framework.Color.White;
				};
				closeX.MouseLeft += delegate
				{
					closeX.TextColor = Microsoft.Xna.Framework.Color.Red;
				};
				hBar.LeftMouseButtonPressed += delegate
				{
					_isMetaDragging = true;
					_metaDragOffset = new Point(GameService.Input.Mouse.Position.X - _metaProgressWindow.Location.X, GameService.Input.Mouse.Position.Y - _metaProgressWindow.Location.Y);
				};
				for (int i = 0; i < metaIds.Count; i++)
				{
					int mId = metaIds[i];
					Achievement def = allDefs.FirstOrDefault((Achievement x) => x.Id == mId);
					if (def == null)
					{
						continue;
					}
					AccountAchievement progress = accAchievements.FirstOrDefault((AccountAchievement a) => a.Id == mId);
					int max = metaMaxes[i];
					int current = realCompletedCollections;
					bool isDone = progress?.Done ?? false;
					if (isDone || current > max)
					{
						current = max;
					}
					Blish_HUD.Controls.Panel row = new Blish_HUD.Controls.Panel
					{
						Parent = scroll,
						Width = 500,
						Height = 55,
						BackgroundColor = darkTealPanel,
						ShowBorder = true
					};
					new Blish_HUD.Controls.Label
					{
						Text = def.Name,
						Parent = row,
						Location = new Point(10, 5),
						Font = GameService.Content.DefaultFont16,
						TextColor = (isDone ? Microsoft.Xna.Framework.Color.LimeGreen : Microsoft.Xna.Framework.Color.White),
						AutoSizeWidth = true
					};
					new Blish_HUD.Controls.Label
					{
						Text = $"{current} / {max}",
						Parent = row,
						Location = new Point(430, 5),
						Font = GameService.Content.DefaultFont14,
						TextColor = Microsoft.Xna.Framework.Color.Cyan,
						AutoSizeWidth = true
					};
					Blish_HUD.Controls.Panel barBg = new Blish_HUD.Controls.Panel
					{
						Parent = row,
						Location = new Point(10, 30),
						Size = new Point(480, 15),
						BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.5f
					};
					new Blish_HUD.Controls.Panel
					{
						Parent = barBg,
						Size = new Point((int)(480f * ((float)current / (float)((max <= 0) ? 1 : max))), 15),
						BackgroundColor = (isDone ? Microsoft.Xna.Framework.Color.LimeGreen : agedGoldText)
					};
					FlowPanel subContainer = new FlowPanel
					{
						Parent = scroll,
						Width = 500,
						HeightSizingMode = SizingMode.AutoSize,
						FlowDirection = ControlFlowDirection.LeftToRight,
						ControlPadding = new Vector2(5f, 5f),
						Visible = false
					};
					_allMetaSubPanels.Add(subContainer);
					row.Click += delegate
					{
						bool flag = _currentlyExpandedRow == row;
						foreach (FlowPanel allMetaSubPanel in _allMetaSubPanels)
						{
							allMetaSubPanel.Visible = false;
						}
						if (flag)
						{
							_currentlyExpandedRow = null;
						}
						else
						{
							subContainer.ClearChildren();
							BuildSubAchievements(def, subContainer, current, isDone, accAchievements);
							subContainer.Visible = true;
							_currentlyExpandedRow = row;
						}
						scroll.RecalculateLayout();
					};
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Meta Tracker failed.");
			}
		}

		private void BuildSubAchievements(Achievement metaDef, FlowPanel container, int currentProgress, bool isFullyDone, IReadOnlyList<AccountAchievement> accProgress)
		{
			int[] obj = new int[35]
			{
				6068, 6179, 6330, 6344, 6363, 6317, 6106, 6489, 6336, 6342,
				6258, 6506, 6471, 6224, 6439, 6505, 6263, 6153, 6484, 6475,
				6227, 6509, 6250, 6339, 6264, 6192, 6466, 6402, 6393, 6110,
				7114, 7804, 8168, 8246, 8554
			};
			FlowPanel gridPanel = new FlowPanel
			{
				Parent = container,
				FlowDirection = ControlFlowDirection.LeftToRight,
				OuterControlPadding = new Vector2(5f, 5f),
				ControlPadding = new Vector2(5f, 5f),
				Width = container.Width - 20,
				CanScroll = true,
				HeightSizingMode = SizingMode.AutoSize
			};
			int[] array = obj;
			foreach (int subId in array)
			{
				StandardButton btn = new StandardButton
				{
					Parent = gridPanel,
					Text = strings.Loading,
					Width = 150,
					Height = 35
				};
				Task<Achievement> subTask = Gw2ApiManager.Gw2ApiClient.V2.Achievements.GetAsync(subId);
				subTask.ContinueWith(delegate(Task<Achievement> t)
				{
					if (t.Status == TaskStatus.RanToCompletion)
					{
						Achievement result = t.Result;
						bool flag = accProgress.FirstOrDefault((AccountAchievement a) => a.Id == subId)?.Done ?? false;
						string text = result.Name.Replace(" Fisher", "");
						btn.Text = (flag ? "✓ " : "") + text;
						btn.BasicTooltipText = result.Description ?? "";
					}
				}, TaskScheduler.FromCurrentSynchronizationContext());
				btn.Click += async delegate
				{
					btn.Enabled = false;
					Achievement subAch = await subTask;
					int subCurrent = accProgress.FirstOrDefault((AccountAchievement a) => a.Id == subId)?.Current ?? 0;
					int subMax = subAch.Bits?.Count ?? 0;
					string cleanName = subAch.Name.Replace(" Fisher", "");
					await ShowAchievementResultsPanel(cleanName, subId, subCurrent, subMax, subAch.Description ?? "");
					btn.Enabled = true;
				};
			}
		}

		private void ShowSpeciesPicker()
		{
			if (_speciesSelectionWindow != null)
			{
				_speciesSelectionWindow.Visible = !_speciesSelectionWindow.Visible;
				return;
			}
			_speciesSelectionWindow = new Blish_HUD.Controls.Panel
			{
				Title = "Filter by Species",
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(280, 500),
				Location = new Point(_leaderboardWindow.Right + 5, _leaderboardWindow.Top),
				ShowBorder = true,
				BackgroundColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 220),
				ZIndex = 1100
			};
			_speciesSearchBox = new Blish_HUD.Controls.TextBox
			{
				Parent = _speciesSelectionWindow,
				Location = new Point(10, 10),
				Width = 240,
				PlaceholderText = strings.SearchSpeciesPlaceholder
			};
			FlowPanel scroll = new FlowPanel
			{
				Parent = _speciesSelectionWindow,
				Size = new Point(260, 420),
				Location = new Point(10, 50),
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			Action<string> populateList = delegate(string filter)
			{
				scroll.ClearChildren();
				StandardButton standardButton = new StandardButton();
				standardButton.Text = strings.AllSpeciesFilterBtn;
				standardButton.Parent = scroll;
				standardButton.Width = 230;
				standardButton.Click += async delegate
				{
					_currentlySelectedSpecies = "__ALL_SPECIES__";
					_speciesFilterBtn.Text = strings.AllSpeciesFilterBtn;
					_speciesSelectionWindow.Visible = false;
					await RefreshLeaderboardData();
				};
				foreach (string name in from n in (from x in _allFishEntries
						where IsRealFish(x.Data)
						select x.Data.Name).Distinct()
					where string.IsNullOrEmpty(filter) || n.ToLower().Contains(filter.ToLower())
					orderby n
					select n)
				{
					StandardButton standardButton2 = new StandardButton();
					standardButton2.Text = name;
					standardButton2.Parent = scroll;
					standardButton2.Width = 230;
					standardButton2.Click += async delegate
					{
						_currentlySelectedSpecies = name;
						_speciesFilterBtn.Text = ((name.Length > 15) ? (name.Substring(0, 12) + "...") : name);
						_speciesSelectionWindow.Visible = false;
						await RefreshLeaderboardData();
					};
				}
			};
			populateList("");
			_speciesSearchBox.TextChanged += delegate
			{
				populateList(_speciesSearchBox.Text);
			};
		}

		private void CopyToClipboard(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			try
			{
				Thread thread = new Thread((ThreadStart)delegate
				{
					try
					{
						Clipboard.SetText(text);
					}
					catch (Exception exception)
					{
						Logger.Error(exception, "WinForms Clipboard failed.");
					}
				});
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Threaded clipboard copy failed.");
			}
		}

		private string GetGlobalSeed()
		{
			byte[] buffer = new byte[32];
			byte[] seedData = new byte[32]
			{
				167, 226, 217, 200, 241, 180, 141, 126, 47, 106,
				83, 76, 53, 30, 7, 248, 225, 202, 179, 156,
				133, 110, 87, 64, 41, 18, 251, 228, 205, 182,
				159, 136
			};
			byte[] seedKey = new byte[8] { 85, 170, 51, 204, 17, 238, 119, 153 };
			for (int j = 0; j < seedData.Length; j++)
			{
				buffer[j] = (byte)(seedData[j] ^ seedKey[j % seedKey.Length]);
				buffer[j] = (byte)((buffer[j] << 3) | (buffer[j] >> 5));
				buffer[j] ^= (byte)(j * 55);
			}
			for (int i = 0; i < buffer.Length; i += 2)
			{
				if (i + 1 < buffer.Length)
				{
					byte temp = buffer[i];
					buffer[i] = buffer[i + 1];
					buffer[i + 1] = temp;
				}
			}
			return BitConverter.ToString(buffer, 0, 13).Replace("-", "");
		}

		private static string DescrambleString(string input, int index, string seed)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			byte[] bytes = Convert.FromBase64String(input);
			byte[] key = BitConverter.GetBytes(seed.GetHashCode() ^ index);
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] ^= key[i % key.Length];
			}
			return Encoding.UTF8.GetString(bytes);
		}

		private static int DescrambleInt(int value, int index, string seed)
		{
			return value ^ (seed.GetHashCode() ^ index);
		}

		private static double DescrambleDouble(double value, int index, string seed)
		{
			return BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(value) ^ (((long)(seed.GetHashCode() ^ index) << 32) | (uint)(seed.GetHashCode() ^ index)));
		}

		private string GenerateSignature(double weight, double length, int itemId, bool isSuperPb, string salt)
		{
			string raw = string.Format(CultureInfo.InvariantCulture, "{0}|{1:F2}|{2:F2}|{3}|{4}", salt, weight, length, itemId, isSuperPb);
			using SHA256 sha256 = SHA256.Create();
			return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(raw))).Substring(0, 12);
		}

		private string GenerateSignatureLegacy(double weight, double length, string name, bool isSuperPb, string salt)
		{
			string raw = string.Format(CultureInfo.InvariantCulture, "{0}|{1:F2}|{2:F2}|{3}|{4}", salt, weight, length, name, isSuperPb);
			using SHA256 sha256 = SHA256.Create();
			return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(raw))).Substring(0, 12);
		}

		private string GenerateMasterSignature(string payload, string salt)
		{
			using SHA256 sha256 = SHA256.Create();
			return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(payload + salt))).Substring(0, 12);
		}

		private byte[] GetRawSeedBuffer()
		{
			byte[] buffer = new byte[32];
			byte[] seedData = new byte[32]
			{
				167, 226, 217, 200, 241, 180, 141, 126, 47, 106,
				83, 76, 53, 30, 7, 248, 225, 202, 179, 156,
				133, 110, 87, 64, 41, 18, 251, 228, 205, 182,
				159, 136
			};
			byte[] seedKey = new byte[8] { 85, 170, 51, 204, 17, 238, 119, 153 };
			for (int j = 0; j < seedData.Length; j++)
			{
				buffer[j] = (byte)(seedData[j] ^ seedKey[j % seedKey.Length]);
				buffer[j] = (byte)((buffer[j] << 3) | (buffer[j] >> 5));
				buffer[j] ^= (byte)(j * 55);
			}
			for (int i = 0; i < buffer.Length; i += 2)
			{
				if (i + 1 < buffer.Length)
				{
					byte temp = buffer[i];
					buffer[i] = buffer[i + 1];
					buffer[i + 1] = temp;
				}
			}
			return buffer;
		}

		private void LoadFishDatabase()
		{
			try
			{
				string json;
				using (Stream stream = ContentsManager.GetFileStream("GilledWarsMasterFishList.json"))
				{
					using StreamReader reader = new StreamReader(stream);
					json = reader.ReadToEnd();
				}
				List<FishData> db = JsonConvert.DeserializeObject<List<FishData>>(json);
				byte[] rawBuffer = GetRawSeedBuffer();
				string legacySeed = Encoding.UTF8.GetString(rawBuffer, 0, 13);
				for (int i = 0; i < db.Count; i++)
				{
					FishData f = db[i];
					f.Name = DescrambleString(f.Name, i, legacySeed);
					f.Rarity = DescrambleString(f.Rarity, i, legacySeed);
					f.Location = DescrambleString(f.Location, i, legacySeed);
					f.Time = DescrambleString(f.Time, i, legacySeed);
					f.Bait = DescrambleString(f.Bait, i, legacySeed);
					f.FishingHole = DescrambleString(f.FishingHole, i, legacySeed);
					f.ItemId = DescrambleInt(f.ItemId, i, legacySeed);
					f.MinW = DescrambleDouble(f.MinW, i, legacySeed);
					f.MaxW = DescrambleDouble(f.MaxW, i, legacySeed);
					f.MinL = DescrambleDouble(f.MinL, i, legacySeed);
					f.MaxL = DescrambleDouble(f.MaxL, i, legacySeed);
					_allFishEntries.Add(new FishUIEntry
					{
						Data = f
					});
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "JSON Load Fail");
			}
		}

		private async Task InitializeAccountAndLoadAsync()
		{
			string newDir = ModuleDirectory;
			try
			{
				Directory.CreateDirectory(newDir);
				string path = Path.Combine(newDir, "permissions_check.txt");
				System.IO.File.WriteAllText(path, "Gilled Wars Write Test - Success");
				System.IO.File.Delete(path);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "CRITICAL: Could not write to module storage.");
			}
			bool accountFound = false;
			if (!string.IsNullOrWhiteSpace(_customApiKey.Value))
			{
				try
				{
					Connection connection = new Connection(_customApiKey.Value);
					using Gw2Client client = new Gw2Client(connection);
					_localAccountName = (await client.WebApi.V2.Account.GetAsync()).Name.Replace(".", "_");
					accountFound = true;
				}
				catch
				{
					Logger.Warn("Custom API Key failed.");
				}
			}
			if (!accountFound)
			{
				for (int retries = 15; retries > 0; retries--)
				{
					if (Gw2ApiManager.HasPermissions(new TokenPermission[1] { TokenPermission.Account }))
					{
						_localAccountName = (await Gw2ApiManager.Gw2ApiClient.V2.Account.GetAsync()).Name.Replace(".", "_");
						accountFound = true;
						break;
					}
					await Task.Delay(800);
				}
			}
			if (!accountFound)
			{
				_localAccountName = "UnknownAccount";
			}
			LoadPersonalBests();
			RefreshFishLogUI();
		}

		private string GetPbFileName()
		{
			if (!(_localAccountName == "UnknownAccount"))
			{
				return "personal_bests_" + _localAccountName + ".json";
			}
			return "personal_bests.json";
		}

		private void LoadPersonalBests()
		{
			string path = Path.Combine(ModuleDirectory, GetPbFileName());
			_isCheater = false;
			_personalBests = new Dictionary<int, PersonalBestRecord>();
			if (!System.IO.File.Exists(path))
			{
				return;
			}
			try
			{
				string json = System.IO.File.ReadAllText(path);
				string seed = GetGlobalSeed();
				PersonalBestFile pbFile = null;
				try
				{
					pbFile = JsonConvert.DeserializeObject<PersonalBestFile>(json);
				}
				catch
				{
				}
				int loadedVersion = ((pbFile == null || pbFile.Records == null || pbFile.Version < 2) ? 1 : pbFile.Version);
				if (pbFile != null && pbFile.Version >= 2 && pbFile.Records != null)
				{
					Logger.Info($"[GilledWars] Loading PB file version {pbFile.Version}");
					foreach (KeyValuePair<int, PersonalBestRecord> kvp2 in pbFile.Records)
					{
						int itemId = kvp2.Key;
						PersonalBestRecord rec3 = kvp2.Value;
						ValidateSubRecord(rec3.BestWeight, itemId, seed);
						ValidateSubRecord(rec3.BestLength, itemId, seed);
						_personalBests[itemId] = rec3;
						_caughtFishIds.Add(itemId);
					}
				}
				else
				{
					Logger.Info("[GilledWars] Migrating PB file from version 1 to version 2");
					Dictionary<int, PersonalBestRecord> legacy = null;
					try
					{
						legacy = JsonConvert.DeserializeObject<Dictionary<int, PersonalBestRecord>>(json);
					}
					catch
					{
					}
					if (legacy != null)
					{
						foreach (KeyValuePair<int, PersonalBestRecord> kvp in legacy)
						{
							int itemId2 = kvp.Key;
							PersonalBestRecord rec2 = kvp.Value;
							string englishName = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == itemId2)?.Data.Name ?? "Unknown";
							MigrateSubRecord(rec2.BestWeight, itemId2, englishName, seed);
							MigrateSubRecord(rec2.BestLength, itemId2, englishName, seed);
							_personalBests[itemId2] = rec2;
							_caughtFishIds.Add(itemId2);
						}
						Logger.Info("[GilledWars] v1 -> v2 migration complete");
					}
				}
				if (loadedVersion >= 3 || _personalBests.Count <= 0)
				{
					return;
				}
				int reset = 0;
				foreach (PersonalBestRecord rec in _personalBests.Values)
				{
					if (rec.BestWeight != null && !rec.BestWeight.IsCheater && rec.BestWeight.IsSubmitted)
					{
						rec.BestWeight.IsSubmitted = false;
						reset++;
					}
					if (rec.BestLength != null && !rec.BestLength.IsCheater && rec.BestLength.IsSubmitted)
					{
						rec.BestLength.IsSubmitted = false;
						reset++;
					}
				}
				SavePersonalBests();
				Logger.Info($"[GilledWars] v3 migration: reset IsSubmitted on {reset} sub-record(s); saved as version 3");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to load personal bests");
			}
		}

		private void ValidateSubRecord(SubRecord sub, int itemId, string seed)
		{
			if (sub != null)
			{
				string cName = sub.CharacterName ?? "Unknown";
				string expected = GenerateSignature(sub.Weight, sub.Length, itemId, sub.IsSuperPb, seed + cName + _localAccountName);
				if (sub.Signature != expected)
				{
					Logger.Warn($"[GilledWars] CHEAT CHECK FAIL itemId={itemId} stored={sub.Signature} expected={expected} weight={sub.Weight} len={sub.Length} char={cName} account={_localAccountName} super={sub.IsSuperPb} seed={seed}");
					sub.IsCheater = (_isCheater = true);
				}
				else
				{
					Logger.Info($"[GilledWars] CHEAT CHECK OK itemId={itemId}");
				}
			}
		}

		private void MigrateSubRecord(SubRecord sub, int itemId, string englishName, string seed)
		{
			if (sub == null)
			{
				return;
			}
			string cName = sub.CharacterName ?? "Unknown";
			string expectedNew = GenerateSignature(sub.Weight, sub.Length, itemId, sub.IsSuperPb, seed + cName + _localAccountName);
			if (!(sub.Signature == expectedNew))
			{
				string expectedOld = GenerateSignatureLegacy(sub.Weight, sub.Length, englishName, sub.IsSuperPb, seed + cName + _localAccountName);
				if (sub.Signature == expectedOld)
				{
					sub.Signature = expectedNew;
					sub.EnglishFishName = englishName;
					Logger.Info($"[GilledWars] Migrated record for {englishName} (itemId {itemId})");
				}
				else
				{
					Logger.Warn($"[GilledWars] Migration: invalid signature for {englishName} (itemId {itemId}) — marking as cheater");
					sub.IsCheater = (_isCheater = true);
				}
			}
		}

		private void SavePersonalBests()
		{
			string path = Path.Combine(ModuleDirectory, GetPbFileName());
			try
			{
				PersonalBestFile pbFile = new PersonalBestFile
				{
					Version = 3,
					Records = _personalBests
				};
				System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(pbFile, Formatting.Indented));
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to save personal bests");
			}
		}

		private async Task<Dictionary<int, int>> GetActiveCharacterBags()
		{
			if (string.IsNullOrWhiteSpace(_customApiKey.Value))
			{
				return null;
			}
			string charName = GameService.Gw2Mumble.PlayerCharacter.Name;
			if (string.IsNullOrEmpty(charName))
			{
				return null;
			}
			try
			{
				Connection connection = new Connection(_customApiKey.Value);
				using Gw2Client client = new Gw2Client(connection);
				Character character = await client.WebApi.V2.Characters[charName].GetAsync();
				if (character == null || character.Bags == null)
				{
					return null;
				}
				Dictionary<int, int> inventory = new Dictionary<int, int>();
				foreach (CharacterInventoryBag bag in character.Bags!)
				{
					if (bag == null)
					{
						continue;
					}
					foreach (AccountItem item in bag.Inventory)
					{
						if (item != null)
						{
							if (inventory.ContainsKey(item.Id))
							{
								inventory[item.Id] += item.Count;
							}
							else
							{
								inventory[item.Id] = item.Count;
							}
						}
					}
				}
				return inventory;
			}
			catch
			{
				return null;
			}
		}

		private async Task CheckApiForNewCatches()
		{
			Dictionary<int, int> currentInventory = await GetActiveCharacterBags();
			if (currentInventory == null)
			{
				ScreenNotification.ShowNotification(strings.ApiErrorBags, ScreenNotification.NotificationType.Error);
				return;
			}
			int newCatches = 0;
			if (_startInventory.Count > 0)
			{
				foreach (KeyValuePair<int, int> kvp in currentInventory)
				{
					int itemId = kvp.Key;
					int currentCount = kvp.Value;
					int oldCount = (_startInventory.ContainsKey(itemId) ? _startInventory[itemId] : 0);
					if (currentCount > oldCount)
					{
						int diff = currentCount - oldCount;
						for (int i = 0; i < diff; i++)
						{
							ProcessCaughtFish(itemId);
							newCatches++;
						}
					}
				}
			}
			_startInventory = currentInventory;
			ScreenNotification.ShowNotification(string.Format(strings.FishMeasuredCount, currentInventory.Values.Sum(), newCatches));
		}

		private async Task TakeInventorySnapshot()
		{
			if (string.IsNullOrWhiteSpace(_customApiKey.Value))
			{
				ScreenNotification.ShowNotification(strings.ApiErrorKey, ScreenNotification.NotificationType.Error);
				return;
			}
			Dictionary<int, int> inv = await GetActiveCharacterBags();
			if (inv != null)
			{
				_startInventory = inv;
				ScreenNotification.ShowNotification(string.Format(strings.SnapshotSaved, inv.Values.Sum()));
			}
			else
			{
				ScreenNotification.ShowNotification(strings.ApiErrorKeyOrChar, ScreenNotification.NotificationType.Error);
			}
		}

		private async Task StartDrfListener()
		{
			if (string.IsNullOrWhiteSpace(_drfToken.Value))
			{
				ScreenNotification.ShowNotification(strings.DrfErrorToken, ScreenNotification.NotificationType.Error);
				return;
			}
			try
			{
				AllowToSetUserAgentHeaderForWebSockets();
				_drfSocket = new ClientWebSocket();
				_drfSocket.Options.SetRequestHeader("User-Agent", "GilledWarsAnglers/0.1.0 BlishHUD/1.3.0");
				_drfCts = new CancellationTokenSource();
				string drfUrl = "wss://drf.rs/ws";
				await _drfSocket.ConnectAsync(new Uri(drfUrl), _drfCts.Token);
				ArraySegment<byte> authBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Bearer " + _drfToken.Value));
				await _drfSocket.SendAsync(authBuffer, WebSocketMessageType.Text, endOfMessage: true, _drfCts.Token);
				ScreenNotification.ShowNotification(strings.DrfConnected, ScreenNotification.NotificationType.Warning);
				_drfReceiveTask = Task.Run((Func<Task>)ReceiveDrfMessages, _drfCts.Token);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to connect to DRF WebSocket.");
				ScreenNotification.ShowNotification(strings.DrfFailed, ScreenNotification.NotificationType.Error);
			}
		}

		private async Task ReceiveDrfMessages()
		{
			byte[] buffer = new byte[8192];
			try
			{
				while (_drfSocket != null && _drfSocket.State == WebSocketState.Open && _drfCts != null && !_drfCts.Token.IsCancellationRequested)
				{
					WebSocketReceiveResult result = await _drfSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _drfCts.Token);
					if (result.MessageType == WebSocketMessageType.Close)
					{
						await _drfSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", _drfCts.Token);
					}
					else
					{
						if (result.MessageType != 0)
						{
							continue;
						}
						string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
						try
						{
							JObject drfData = JObject.Parse(message);
							if (drfData == null || !((string?)drfData["kind"] == "data"))
							{
								continue;
							}
							JObject items = drfData["payload"]?["drop"]?["items"] as JObject;
							if (items == null)
							{
								continue;
							}
							Dictionary<string, int> itemsDict = items.ToObject<Dictionary<string, int>>();
							if (itemsDict == null)
							{
								continue;
							}
							foreach (KeyValuePair<string, int> kvp in itemsDict)
							{
								if (!int.TryParse(kvp.Key, out var itemId))
								{
									continue;
								}
								int count = kvp.Value;
								if (count > 0)
								{
									for (int i = 0; i < count; i++)
									{
										ProcessCaughtFish(itemId);
									}
								}
							}
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
		}

		private void StopDrfListener()
		{
			try
			{
				if (_drfCts != null && !_drfCts.IsCancellationRequested)
				{
					_drfCts.Cancel();
				}
				if (_drfSocket != null)
				{
					_drfSocket.Abort();
					_drfSocket.Dispose();
				}
				_drfCts?.Dispose();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error safely stopping DRF WebSocket.");
			}
			finally
			{
				_drfSocket = null;
				_drfCts = null;
			}
		}

		private void StopCasualLogging()
		{
			if (_isCasualLoggingActive)
			{
				_isCasualLoggingActive = false;
				_isSyncTimerActive = false;
				if (_casualLogToggleBtn != null)
				{
					_casualLogToggleBtn.Text = strings.StartLoggingBtnText;
				}
				if (_casualMeasureBtn != null)
				{
					_casualMeasureBtn.Enabled = false;
				}
				if (_casualSyncTimerLabel != null)
				{
					_casualSyncTimerLabel.Visible = false;
				}
				if (_useDrfCheckbox != null)
				{
					_useDrfCheckbox.Enabled = true;
				}
				StopDrfListener();
				if (_casualCompactPanel != null)
				{
					_casualCompactPanel.Visible = false;
				}
			}
		}

		private static void AllowToSetUserAgentHeaderForWebSockets()
		{
			Assembly assembly = typeof(HttpWebRequest).Assembly;
			FieldInfo[] fields = assembly.GetType("System.Net.HeaderInfoTable").GetFields(BindingFlags.Static | BindingFlags.NonPublic);
			foreach (FieldInfo headerInfoTableFieldInfo in fields)
			{
				if (!(headerInfoTableFieldInfo.Name == "HeaderHashTable"))
				{
					continue;
				}
				Hashtable headerHashTable = headerInfoTableFieldInfo.GetValue(null) as Hashtable;
				if (headerHashTable == null)
				{
					break;
				}
				foreach (string key in headerHashTable.Keys)
				{
					object headerInfo = headerHashTable[key];
					FieldInfo[] fields2 = assembly.GetType("System.Net.HeaderInfo").GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
					foreach (FieldInfo headerInfoFieldInfo in fields2)
					{
						if (headerInfoFieldInfo.Name == "IsRequestRestricted" && (bool)headerInfoFieldInfo.GetValue(headerInfo))
						{
							headerInfoFieldInfo.SetValue(headerInfo, false);
						}
					}
				}
			}
		}

		private static bool IsJunkFish(FishData f)
		{
			if (f == null)
			{
				return false;
			}
			if (f.Rarity == null || !f.Rarity.Equals("Junk", StringComparison.OrdinalIgnoreCase))
			{
				if (f.Location != null)
				{
					return f.Location.Contains("Trash Collector");
				}
				return false;
			}
			return true;
		}

		private static bool IsTreasureFish(FishData f)
		{
			if (f == null)
			{
				return false;
			}
			bool num = f.Name != null && (f.Name.IndexOf("Treasure", StringComparison.OrdinalIgnoreCase) >= 0 || f.Name.IndexOf("Chest", StringComparison.OrdinalIgnoreCase) >= 0 || f.Name.IndexOf("Box", StringComparison.OrdinalIgnoreCase) >= 0 || f.Name.IndexOf("Runestone", StringComparison.OrdinalIgnoreCase) >= 0 || f.Name.IndexOf("Cache", StringComparison.OrdinalIgnoreCase) >= 0 || f.Name.IndexOf("Message", StringComparison.OrdinalIgnoreCase) >= 0);
			bool locHit = f.Location != null && f.Location.Contains("Treasure Collector");
			return num || locHit;
		}

		private static bool IsRealFish(FishData f)
		{
			if (!IsJunkFish(f))
			{
				return !IsTreasureFish(f);
			}
			return false;
		}

		private void ProcessCaughtFish(int itemId)
		{
			FishData matchingFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == itemId)?.Data;
			if (matchingFish == null)
			{
				return;
			}
			bool num = IsJunkFish(matchingFish);
			bool isTreasure = IsTreasureFish(matchingFish);
			if (num)
			{
				ScreenNotification.ShowNotification(_junkMessages[_rnd.Next(_junkMessages.Length)], ScreenNotification.NotificationType.Error);
				return;
			}
			if (isTreasure)
			{
				ScreenNotification.ShowNotification(_treasureMessages[_rnd.Next(_treasureMessages.Length)], ScreenNotification.NotificationType.Warning);
				return;
			}
			if (_isCasualLoggingActive && _sessionStartTime != DateTime.MinValue)
			{
				if (matchingFish.Rarity.Equals("Legendary", StringComparison.OrdinalIgnoreCase))
				{
					_sessionTotalCopper += _ambergrisPriceCopper;
				}
				else if (matchingFish.Rarity.Equals("Ascended", StringComparison.OrdinalIgnoreCase))
				{
					_sessionTotalCopper += _filletPriceCopper;
				}
				else if (matchingFish.Rarity.Equals("Exotic", StringComparison.OrdinalIgnoreCase) || matchingFish.Rarity.Equals("Rare", StringComparison.OrdinalIgnoreCase))
				{
					_sessionTotalCopper += (int)((double)_filletPriceCopper * 0.2);
				}
				else if (matchingFish.Rarity.Equals("Masterwork", StringComparison.OrdinalIgnoreCase))
				{
					_sessionTotalCopper += _fineFilletPriceCopper * 2;
				}
				else if (matchingFish.Rarity.Equals("Fine", StringComparison.OrdinalIgnoreCase) || matchingFish.Rarity.Equals("Basic", StringComparison.OrdinalIgnoreCase))
				{
					if (matchingFish.Name.IndexOf("Crab", StringComparison.OrdinalIgnoreCase) >= 0 || matchingFish.Name.IndexOf("Crawfish", StringComparison.OrdinalIgnoreCase) >= 0 || matchingFish.Name.IndexOf("Prawn", StringComparison.OrdinalIgnoreCase) >= 0 || matchingFish.Name.IndexOf("Shrimp", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						_sessionTotalCopper += _fineFilletPriceCopper;
					}
					else
					{
						_sessionTotalCopper += _fineFilletPriceCopper;
					}
				}
			}
			double minW = ((matchingFish.MinW > 0.0) ? matchingFish.MinW : 1.0);
			double maxW = ((matchingFish.MaxW > minW) ? matchingFish.MaxW : (minW + 5.0));
			double minL = ((matchingFish.MinL > 0.0) ? matchingFish.MinL : 5.0);
			double maxL = ((matchingFish.MaxL > minL) ? matchingFish.MaxL : (minL + 10.0));
			double skewed = Math.Pow(_rnd.NextDouble(), 3.2);
			double weight = Math.Round(minW + (maxW - minW) * skewed, 2);
			double length = Math.Round(minL + (maxL - minL) * skewed, 2);
			bool isSuperPb = false;
			if (_rnd.NextDouble() <= 5E-05)
			{
				isSuperPb = true;
				double bonusMult = 1.01 + Math.Pow(_rnd.NextDouble(), 4.0) * 0.11;
				weight = Math.Round(maxW * bonusMult, 2);
				length = Math.Round(maxL * bonusMult, 2);
			}
			string charName = GameService.Gw2Mumble.PlayerCharacter.Name ?? "Unknown";
			string globalSig = GenerateSignature(weight, length, itemId, isSuperPb, GetGlobalSeed() + charName + _localAccountName);
			string tSig = (_isTournamentActive ? GenerateSignature(weight, length, itemId, isSuperPb, _tourneyRoomCode) : "");
			bool isNewPbWeight = false;
			bool isNewPbLength = false;
			if (!_personalBests.ContainsKey(itemId))
			{
				_personalBests[itemId] = new PersonalBestRecord();
			}
			PersonalBestRecord pbObj = _personalBests[itemId];
			bool usedDrf = _useDrfCheckbox != null && _useDrfCheckbox.Checked;
			if (pbObj.BestWeight == null || weight > pbObj.BestWeight.Weight)
			{
				isNewPbWeight = true;
				pbObj.BestWeight = new SubRecord
				{
					Weight = weight,
					Length = length,
					Signature = globalSig,
					IsCheater = false,
					IsSuperPb = isSuperPb,
					CaughtWithDrf = usedDrf,
					CharacterName = charName,
					IsSubmitted = false
				};
			}
			if (pbObj.BestLength == null || length > pbObj.BestLength.Length)
			{
				isNewPbLength = true;
				pbObj.BestLength = new SubRecord
				{
					Weight = weight,
					Length = length,
					Signature = globalSig,
					IsCheater = false,
					IsSuperPb = isSuperPb,
					CaughtWithDrf = usedDrf,
					CharacterName = charName,
					IsSubmitted = false
				};
			}
			if (isNewPbWeight || isNewPbLength)
			{
				SavePersonalBests();
				RefreshFishLogUI();
			}
			_caughtFishIds.Add(itemId);
			string characterName = GameService.Gw2Mumble.PlayerCharacter.Name;
			if (string.IsNullOrEmpty(characterName))
			{
				characterName = "UnknownPlayer";
			}
			TournamentCatch catchRecord = new TournamentCatch
			{
				Id = itemId,
				Name = matchingFish.Name,
				Weight = weight,
				Length = length,
				Rarity = matchingFish.Rarity,
				CharacterName = characterName,
				Signature = globalSig,
				TourneySig = tSig,
				IsNewPb = (isNewPbWeight || isNewPbLength),
				IsSuperPb = isSuperPb
			};
			_recentCatches.Insert(0, catchRecord);
			if (_recentCatches.Count > 20)
			{
				_recentCatches.RemoveAt(_recentCatches.Count - 1);
			}
			if (_recentCatchesPanel != null && _recentCatchesPanel.Visible)
			{
				UpdateRecentCatchesUI();
			}
			if (_casualCompactPanel != null && _casualCompactPanel.Visible)
			{
				UpdateCompactCooler();
			}
			if (_achievementResultsPanel != null && _achievementResultsPanel.Visible && _openAnalyzerAchievementId != 0)
			{
				ShowAchievementResultsPanel(_openAnalyzerLocationName, _openAnalyzerAchievementId, 0, _openAnalyzerSubMax, _openAnalyzerSubDescription);
			}
			string pbAlert = (isSuperPb ? " - SUPER PB!" : (catchRecord.IsNewPb ? " - NEW PB!" : ""));
			ScreenNotification.NotificationType notifType = (isSuperPb ? ScreenNotification.NotificationType.Warning : ScreenNotification.NotificationType.Info);
			if (_isTournamentActive && !_isTourneyWaitingRoom)
			{
				if (_tourneyTargetItemId == 0 || _tourneyTargetItemId == itemId)
				{
					_tourneyCatches.Add(catchRecord);
					UpdateActiveTourneyCoolerUI();
					ScreenNotification.ShowNotification(string.Format(strings.TourneyCatchAlert, catchRecord.Name, catchRecord.Weight, catchRecord.Length, pbAlert), notifType);
				}
				else
				{
					ScreenNotification.ShowNotification(string.Format(strings.CaughtAlert, catchRecord.Name, catchRecord.Weight, catchRecord.Length, pbAlert), notifType);
				}
			}
			else
			{
				ScreenNotification.ShowNotification(string.Format(strings.CaughtAlert, catchRecord.Name, catchRecord.Weight, catchRecord.Length, pbAlert), notifType);
			}
			if (_bitingWidgetPanel != null && _bitingWidgetPanel.Visible)
			{
				RefreshBitingWidget();
			}
		}

		private void BuildMainWindow()
		{
			Microsoft.Xna.Framework.Color deepNavyBg = new Microsoft.Xna.Framework.Color(13, 27, 42);
			Microsoft.Xna.Framework.Color darkTealPanel = new Microsoft.Xna.Framework.Color(26, 47, 69);
			Microsoft.Xna.Framework.Color agedGoldText = new Microsoft.Xna.Framework.Color(201, 168, 76);
			_mainWindow = new Blish_HUD.Controls.Panel
			{
				ShowBorder = true,
				Size = new Point(650, 500),
				Location = new Point(300, 300),
				Parent = GameService.Graphics.SpriteScreen,
				Visible = false,
				BackgroundColor = deepNavyBg,
				ClipsBounds = false,
				ZIndex = 1000
			};
			Blish_HUD.Controls.Panel headerBar = new Blish_HUD.Controls.Panel
			{
				Parent = _mainWindow,
				Size = new Point(_mainWindow.Width, 30),
				Location = new Point(0, 0),
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.6f
			};
			Blish_HUD.Controls.Label titleLabel = new Blish_HUD.Controls.Label
			{
				Text = strings.CornerIconSubtitle,
				Parent = headerBar,
				Location = new Point(10, 5),
				Font = GameService.Content.DefaultFont16,
				TextColor = agedGoldText,
				AutoSizeWidth = true
			};
			Blish_HUD.Controls.Label xCloseBtn = new Blish_HUD.Controls.Label
			{
				Text = "X",
				Parent = headerBar,
				Location = new Point(headerBar.Width - 25, 5),
				Font = GameService.Content.DefaultFont16,
				TextColor = Microsoft.Xna.Framework.Color.Red,
				AutoSizeWidth = true,
				BasicTooltipText = strings.CloseLodgeTooltip
			};
			Image flagEN = new Image
			{
				Texture = ContentsManager.GetTexture("flags/gb.png"),
				Parent = headerBar,
				Size = new Point(26, 18),
				Location = new Point(headerBar.Width - 135, 6),
				BasicTooltipText = "English (click again to reset to BlishHUD language)"
			};
			Image flagFR = new Image
			{
				Texture = ContentsManager.GetTexture("flags/fr.png"),
				Parent = headerBar,
				Size = new Point(26, 18),
				Location = new Point(headerBar.Width - 107, 6),
				BasicTooltipText = "Français (click again to reset to BlishHUD language)"
			};
			Image flagDE = new Image
			{
				Texture = ContentsManager.GetTexture("flags/de.png"),
				Parent = headerBar,
				Size = new Point(26, 18),
				Location = new Point(headerBar.Width - 79, 6),
				BasicTooltipText = "Deutsch (click again to reset to BlishHUD language)"
			};
			Image flagES = new Image
			{
				Texture = ContentsManager.GetTexture("flags/es.png"),
				Parent = headerBar,
				Size = new Point(26, 18),
				Location = new Point(headerBar.Width - 51, 6),
				BasicTooltipText = "Español (click again to reset to BlishHUD language)"
			};
			Action updateFlagOpacity = delegate
			{
				string text5 = DetectLanguage();
				_ = _selectedLanguage.Value;
				flagEN.Opacity = ((text5 == "en") ? 1f : 0.3f);
				flagFR.Opacity = ((text5 == "fr") ? 1f : 0.3f);
				flagDE.Opacity = ((text5 == "de") ? 1f : 0.3f);
				flagES.Opacity = ((text5 == "es") ? 1f : 0.3f);
			};
			updateFlagOpacity();
			flagEN.Click += delegate
			{
				string text4 = ((_selectedLanguage.Value == "en") ? "auto" : "en");
				ApplyLanguage((text4 == "auto") ? DetectLanguage() : text4);
				_selectedLanguage.Value = text4;
				updateFlagOpacity();
				ScreenNotification.ShowNotification((text4 == "auto") ? "Language reset to BlishHUD default." : "Language: English — reopen UI to apply.");
			};
			flagFR.Click += delegate
			{
				string text3 = ((_selectedLanguage.Value == "fr") ? "auto" : "fr");
				ApplyLanguage((text3 == "auto") ? DetectLanguage() : text3);
				_selectedLanguage.Value = text3;
				updateFlagOpacity();
				ScreenNotification.ShowNotification((text3 == "auto") ? "Langue réinitialisée." : "Langue: Français — rouvrir l'UI pour appliquer.");
			};
			flagDE.Click += delegate
			{
				string text2 = ((_selectedLanguage.Value == "de") ? "auto" : "de");
				ApplyLanguage((text2 == "auto") ? DetectLanguage() : text2);
				_selectedLanguage.Value = text2;
				updateFlagOpacity();
				ScreenNotification.ShowNotification((text2 == "auto") ? "Sprache zurückgesetzt." : "Sprache: Deutsch — UI neu öffnen.");
			};
			flagES.Click += delegate
			{
				string text = ((_selectedLanguage.Value == "es") ? "auto" : "es");
				ApplyLanguage((text == "auto") ? DetectLanguage() : text);
				_selectedLanguage.Value = text;
				updateFlagOpacity();
				ScreenNotification.ShowNotification((text == "auto") ? "Idioma restablecido." : "Idioma: Español — reabrir UI para aplicar.");
			};
			xCloseBtn.Click += delegate
			{
				_mainWindow.Visible = false;
			};
			xCloseBtn.MouseEntered += delegate
			{
				xCloseBtn.TextColor = Microsoft.Xna.Framework.Color.White;
			};
			xCloseBtn.MouseLeft += delegate
			{
				xCloseBtn.TextColor = Microsoft.Xna.Framework.Color.Red;
			};
			headerBar.LeftMouseButtonPressed += delegate
			{
				if (GameService.Input.Mouse.ActiveControl == headerBar || GameService.Input.Mouse.ActiveControl == titleLabel)
				{
					_isDragging = true;
					_dragOffset = new Point(GameService.Input.Mouse.Position.X - _mainWindow.Location.X, GameService.Input.Mouse.Position.Y - _mainWindow.Location.Y);
				}
			};
			FlowPanel navPanel = new FlowPanel
			{
				Parent = _mainWindow,
				Location = new Point(0, 30),
				Size = new Point(50, _mainWindow.Height - 30),
				BackgroundColor = darkTealPanel,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				OuterControlPadding = new Vector2(9f, 15f),
				ControlPadding = new Vector2(0f, 30f)
			};
			Blish_HUD.Controls.Panel contentHost = new Blish_HUD.Controls.Panel
			{
				Parent = _mainWindow,
				Location = new Point(50, 30),
				Size = new Point(_mainWindow.Width - 50, _mainWindow.Height - 30),
				BackgroundColor = Microsoft.Xna.Framework.Color.Transparent
			};
			_casualPanel = new Blish_HUD.Controls.Panel
			{
				Parent = contentHost,
				Size = contentHost.Size,
				Visible = true
			};
			_tournamentPanel = new Blish_HUD.Controls.Panel
			{
				Parent = contentHost,
				Size = contentHost.Size,
				Visible = false
			};
			_fishLogPanel = new Blish_HUD.Controls.Panel
			{
				Parent = contentHost,
				Size = contentHost.Size,
				Visible = false
			};
			_achievementPanel = new Blish_HUD.Controls.Panel
			{
				Parent = contentHost,
				Size = contentHost.Size,
				Visible = false
			};
			Blish_HUD.Controls.Panel casualBtnHost = new Blish_HUD.Controls.Panel
			{
				Parent = navPanel,
				Size = new Point(32, 32),
				BasicTooltipText = strings.CasualFishingTooltip
			};
			Image obj = new Image
			{
				Texture = ContentsManager.GetTexture("images/casualico.png"),
				Parent = casualBtnHost,
				Size = new Point(24, 32),
				Location = new Point(4, 0)
			};
			casualBtnHost.Click += delegate
			{
				ShowPanel(_casualPanel);
			};
			obj.Click += delegate
			{
				ShowPanel(_casualPanel);
			};
			Image image = new Image();
			image.Texture = ContentsManager.GetTexture("images/tournamentico.png");
			image.Parent = navPanel;
			image.Size = new Point(32, 32);
			image.BasicTooltipText = strings.TourneyModeTooltip;
			image.Click += delegate
			{
				ShowPanel(_tournamentPanel);
			};
			Image image2 = new Image();
			image2.Texture = ContentsManager.GetTexture("images/fishlogico.png");
			image2.Parent = navPanel;
			image2.Size = new Point(32, 32);
			image2.BasicTooltipText = strings.FishLogTooltip;
			image2.Click += delegate
			{
				ShowPanel(_fishLogPanel);
			};
			Image image3 = new Image();
			image3.Texture = ContentsManager.GetTexture("images/leaderboardico.png");
			image3.Parent = navPanel;
			image3.Size = new Point(32, 32);
			image3.BasicTooltipText = strings.LeaderboardTooltip;
			image3.Click += delegate
			{
				ScreenNotification.ShowNotification(strings.LoadingTop10);
				ShowLeaderboardWindow();
			};
			Image image4 = new Image();
			image4.Texture = ContentsManager.GetTexture("images/websiteico.png");
			image4.Parent = navPanel;
			image4.Size = new Point(32, 32);
			image4.BasicTooltipText = strings.OpenWebsiteTooltip;
			image4.Click += delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://www.gilledwars.com",
					UseShellExecute = true
				});
			};
			BuildFishLogGrid(_fishLogPanel);
			BuildCasualUI(_casualPanel);
			BuildTournamentUI(_tournamentPanel);
			BuildActiveTournamentWidget();
			void ShowPanel(Blish_HUD.Controls.Panel active)
			{
				Blish_HUD.Controls.Panel casualPanel = _casualPanel;
				Blish_HUD.Controls.Panel tournamentPanel = _tournamentPanel;
				Blish_HUD.Controls.Panel fishLogPanel = _fishLogPanel;
				bool flag2 = (_achievementPanel.Visible = false);
				bool flag4 = (fishLogPanel.Visible = flag2);
				bool visible = (tournamentPanel.Visible = flag4);
				casualPanel.Visible = visible;
				active.Visible = true;
			}
		}

		private void BuildFishLogGrid(Blish_HUD.Controls.Panel parent)
		{
			if (parent == null)
			{
				return;
			}
			_categoryPanels.Clear();
			Blish_HUD.Controls.Panel filterPanel = new Blish_HUD.Controls.Panel
			{
				Parent = parent,
				Location = new Point(10, 0),
				Size = new Point(parent.Width - 20, 110)
			};
			Blish_HUD.Controls.TextBox searchBar = new Blish_HUD.Controls.TextBox
			{
				Parent = filterPanel,
				Location = new Point(0, 0),
				Width = 140,
				PlaceholderText = strings.SearchFishPlaceholder
			};
			Dropdown rarityDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(150, 0),
				Width = 110
			};
			Dropdown locationDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(270, 0),
				Width = 130
			};
			Dropdown holeDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(410, 0),
				Width = 110
			};
			Dropdown timeDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(0, 35),
				Width = 100
			};
			Dropdown baitDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(110, 35),
				Width = 120
			};
			StandardButton collapseBtn = new StandardButton
			{
				Text = strings.CollapseBtnText,
				Parent = filterPanel,
				Location = new Point(240, 35),
				Width = 100
			};
			StandardButton revealBtn = new StandardButton
			{
				Text = strings.RevealBtnText,
				Parent = filterPanel,
				Location = new Point(350, 35),
				Width = 100
			};
			StandardButton obj = new StandardButton
			{
				Text = strings.ResetFiltersBtnText,
				Parent = filterPanel,
				Location = new Point(460, 35),
				Width = 130
			};
			StandardButton pushLeaderboardBtn = new StandardButton
			{
				Text = strings.PushPbBtnText,
				Parent = filterPanel,
				Location = new Point(0, 70),
				Width = 150,
				BasicTooltipText = strings.PushPbTooltip
			};
			StandardButton zoneAnalyzerBtn = new StandardButton
			{
				Text = strings.ZoneAnalyzerBtnText,
				Parent = filterPanel,
				Location = new Point(160, 70),
				Width = 150,
				BasicTooltipText = strings.ZoneAnalyzerTooltip
			};
			StandardButton metaProgressBtn = new StandardButton
			{
				Text = strings.MetaProgressBtnText,
				Parent = filterPanel,
				Location = new Point(320, 70),
				Width = 150,
				BasicTooltipText = strings.MetaProgressTooltip
			};
			FlowPanel scroll = new FlowPanel
			{
				Parent = parent,
				Location = new Point(10, 115),
				Size = new Point(parent.Width - 20, parent.Height - 120),
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			searchBar.TextChanged += delegate
			{
				ApplyFilters();
			};
			rarityDrop.ValueChanged += delegate
			{
				ApplyFilters();
			};
			locationDrop.ValueChanged += delegate
			{
				ApplyFilters();
			};
			holeDrop.ValueChanged += delegate
			{
				ApplyFilters();
			};
			timeDrop.ValueChanged += delegate
			{
				ApplyFilters();
			};
			baitDrop.ValueChanged += delegate
			{
				ApplyFilters();
			};
			collapseBtn.Click += delegate
			{
				foreach (FlowPanel categoryPanel in _categoryPanels)
				{
					categoryPanel.Collapsed = true;
				}
			};
			revealBtn.Click += delegate
			{
				foreach (FlowPanel categoryPanel2 in _categoryPanels)
				{
					categoryPanel2.Collapsed = false;
				}
			};
			obj.Click += delegate
			{
				searchBar.Text = "";
				rarityDrop.SelectedItem = "All Rarities";
				locationDrop.SelectedItem = "All Locations";
				holeDrop.SelectedItem = "All Holes";
				timeDrop.SelectedItem = "All Times";
				baitDrop.SelectedItem = "All Baits";
				ApplyFilters();
			};
			pushLeaderboardBtn.Click += async delegate
			{
				if ((DateTime.Now - _lastSubmitTime).TotalMinutes < 5.0)
				{
					ScreenNotification.ShowNotification(string.Format(arg0: (5.0 - (DateTime.Now - _lastSubmitTime).TotalMinutes).ToString("F1"), format: strings.SubmitCooldown), ScreenNotification.NotificationType.Error);
				}
				else
				{
					pushLeaderboardBtn.Enabled = false;
					pushLeaderboardBtn.Text = strings.UploadingText;
					await ForceUploadPB();
					_lastSubmitTime = DateTime.Now;
					pushLeaderboardBtn.Text = strings.PushPbBtnText;
					pushLeaderboardBtn.Enabled = true;
				}
			};
			Map mapInfo;
			zoneAnalyzerBtn.Click += async delegate
			{
				zoneAnalyzerBtn.Enabled = false;
				zoneAnalyzerBtn.Text = strings.Scanning;
				try
				{
					int currentMapId = GameService.Gw2Mumble.CurrentMap.Id;
					if (currentMapId != 0)
					{
						mapInfo = await Gw2ApiManager.Gw2ApiClient.V2.Maps.GetAsync(currentMapId);
						Dictionary<string, int> achievementMap = new Dictionary<string, int>
						{
							{ "Kryta", 6068 },
							{ "Shiverpeak", 6179 },
							{ "Ascalon", 6330 },
							{ "Maguuma Jungle", 6344 },
							{ "Ruins of Orr", 6363 },
							{ "Southsun Cove", 6106 },
							{ "Sandswept Isles", 6106 },
							{ "Elona", 6106 },
							{ "Crystal Desert", 6317 },
							{ "Ring of Fire", 6489 },
							{ "Seitung Province", 6336 },
							{ "New Kaineng City", 6342 },
							{ "The Echovald Wilds", 6258 },
							{ "Dragon's End", 6506 },
							{ "Skywatch Archipelago", 7114 },
							{ "Amnytas", 7114 },
							{ "Inner Nayos", 7114 },
							{ "Lowland Shore", 8168 },
							{ "Janthir Syntri", 8168 },
							{ "Mistburned Barrens", 8554 },
							{ "Castora", 8900 }
						};
						string target = achievementMap.Keys.FirstOrDefault((string k) => mapInfo.Name.Contains(k) || (mapInfo.RegionName != null && mapInfo.RegionName.Contains(k))) ?? "All Locations";
						if (target != "All Locations")
						{
							int achId = achievementMap[target];
							Achievement achDef = await Gw2ApiManager.Gw2ApiClient.V2.Achievements.GetAsync(achId);
							int subCurrent = 0;
							int subMax = achDef.Tiers?.LastOrDefault()?.Count ?? achDef.Bits?.Count ?? 0;
							string subDescription = achDef.Description ?? "No description available.";
							await ShowAchievementResultsPanel(target, achId, subCurrent, subMax, subDescription);
						}
						else
						{
							ScreenNotification.ShowNotification(strings.NoAchievFound, ScreenNotification.NotificationType.Error);
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Zone Analyzer failed.");
				}
				finally
				{
					zoneAnalyzerBtn.Enabled = true;
					zoneAnalyzerBtn.Text = strings.ZoneAnalyzerBtnText;
				}
			};
			metaProgressBtn.Click += async delegate
			{
				metaProgressBtn.Enabled = false;
				metaProgressBtn.Text = strings.Loading;
				await ShowMetaProgressWindow();
				metaProgressBtn.Text = strings.MetaProgressBtnText;
				metaProgressBtn.Enabled = true;
			};
			HashSet<string> uniqueHoles = new HashSet<string>();
			foreach (FishUIEntry entry in _allFishEntries)
			{
				if (string.IsNullOrEmpty(entry.Data.FishingHole))
				{
					continue;
				}
				foreach (string sh in from h in entry.Data.FishingHole.Replace("None, ", "").Split(',')
					select h.Trim())
				{
					uniqueHoles.Add(sh);
				}
			}
			rarityDrop.Items.Add("All Rarities");
			foreach (string r in _allFishEntries.Select((FishUIEntry x) => x.Data.Rarity).Distinct())
			{
				rarityDrop.Items.Add(r);
			}
			rarityDrop.SelectedItem = "All Rarities";
			locationDrop.Items.Add("All Locations");
			foreach (string i in (from x in _allFishEntries
				select x.Data.Location into loc
				where !loc.StartsWith("Avid", StringComparison.OrdinalIgnoreCase)
				select loc).Distinct())
			{
				locationDrop.Items.Add(i);
			}
			locationDrop.SelectedItem = "All Locations";
			holeDrop.Items.Add("All Holes");
			foreach (string h2 in uniqueHoles.OrderBy((string x) => x))
			{
				if (h2 != "None")
				{
					holeDrop.Items.Add(h2);
				}
			}
			holeDrop.SelectedItem = "All Holes";
			timeDrop.Items.Add("All Times");
			timeDrop.Items.Add("Daytime");
			timeDrop.Items.Add("Nighttime");
			timeDrop.Items.Add("Dawn/Dusk");
			timeDrop.Items.Add("Any");
			timeDrop.SelectedItem = "All Times";
			baitDrop.Items.Add("All Baits");
			foreach (string b in _allFishEntries.Select((FishUIEntry x) => x.Data.Bait).Distinct())
			{
				baitDrop.Items.Add(b);
			}
			baitDrop.SelectedItem = "All Baits";
			foreach (IGrouping<string, FishData> group in from x in _allFishEntries
				select x.Data into x
				where x.Location != "Any" && !x.Location.StartsWith("Avid", StringComparison.OrdinalIgnoreCase)
				group x by x.Location into x
				orderby x.Key
				select x)
			{
				FlowPanel p = new FlowPanel
				{
					Parent = scroll,
					Title = group.Key,
					CanCollapse = true,
					ShowBorder = true,
					Width = parent.Width - 40,
					HeightSizingMode = SizingMode.AutoSize,
					FlowDirection = ControlFlowDirection.LeftToRight
				};
				_categoryPanels.Add(p);
				foreach (FishData fish in group)
				{
					string safeName = fish.Name.Replace(" ", "_").Replace("'", "").Replace("-", "");
					bool num = _caughtFishIds.Contains(fish.ItemId);
					string pbWText = strings.NoneLogged;
					string pbLText = strings.NoneLogged;
					Microsoft.Xna.Framework.Color tintColor = (num ? Microsoft.Xna.Framework.Color.White : (Microsoft.Xna.Framework.Color.Black * 0.4f));
					if (_personalBests.TryGetValue(fish.ItemId, out var rec))
					{
						if (rec.BestWeight != null)
						{
							pbWText = (rec.BestWeight.IsCheater ? strings.CheaterDetected : $"{rec.BestWeight.Weight} lbs");
						}
						if (rec.BestLength != null)
						{
							pbLText = (rec.BestLength.IsCheater ? strings.CheaterDetected : $"{rec.BestLength.Length} in");
						}
						if ((rec.BestWeight != null && rec.BestWeight.IsSuperPb) || (rec.BestLength != null && rec.BestLength.IsSuperPb))
						{
							tintColor = Microsoft.Xna.Framework.Color.Gold;
						}
					}
					string tooltip = fish.Name + "\nRarity: " + fish.Rarity + "\nLocation: " + fish.Location + "\nHole: " + fish.FishingHole + "\nTime: " + fish.Time + "\nBait: " + fish.Bait;
					bool isColl = (fish.Rarity != null && fish.Rarity.Equals("Junk", StringComparison.OrdinalIgnoreCase)) || (fish.Location != null && fish.Location.Contains("Collector"));
					if (!isColl)
					{
						tooltip = tooltip + "\n\nPB Weight: " + pbWText + "\nPB Length: " + pbLText;
					}
					Image img = new Image
					{
						Parent = p,
						Size = new Point(64, 64),
						BasicTooltipText = tooltip,
						Texture = ContentsManager.GetTexture("images/" + safeName + ".png"),
						Tint = tintColor
					};
					img.Click += delegate
					{
						byte[] array = new byte[6] { 2, 1, 0, 0, 0, 0 };
						BitConverter.GetBytes(fish.ItemId).CopyTo(array, 2);
						string text = "[&" + Convert.ToBase64String(array) + "]";
						string text2 = text;
						if (!isColl && _personalBests.TryGetValue(fish.ItemId, out var value))
						{
							double num2 = value.BestWeight?.Weight ?? 0.0;
							double num3 = value.BestLength?.Length ?? 0.0;
							if (num2 > 0.0 || num3 > 0.0)
							{
								text2 = string.Format(strings.PbClipText, text, num2, num3);
							}
						}
						Clipboard.SetText(text2);
						ScreenNotification.ShowNotification(string.Format(strings.FishCodeCopied, fish.Name));
					};
					FishUIEntry fishUIEntry = _allFishEntries.First((FishUIEntry x) => x.Data.ItemId == fish.ItemId);
					fishUIEntry.Icon = img;
					fishUIEntry.CategoryPanel = p;
				}
			}
			new Blish_HUD.Controls.Panel
			{
				Parent = scroll,
				Width = parent.Width - 40,
				Height = 60
			};
			void ApplyFilters()
			{
				if (rarityDrop.SelectedItem != null && locationDrop.SelectedItem != null && holeDrop.SelectedItem != null && timeDrop.SelectedItem != null && baitDrop.SelectedItem != null)
				{
					bool hasFilter = !string.IsNullOrEmpty(searchBar.Text) || rarityDrop.SelectedItem != "All Rarities" || locationDrop.SelectedItem != "All Locations" || holeDrop.SelectedItem != "All Holes" || timeDrop.SelectedItem != "All Times" || baitDrop.SelectedItem != "All Baits";
					foreach (FlowPanel cat in _categoryPanels)
					{
						bool anyVisible = false;
						foreach (FishUIEntry entry2 in _allFishEntries.Where((FishUIEntry x) => x.CategoryPanel == cat))
						{
							bool match = true;
							if (!string.IsNullOrEmpty(searchBar.Text) && !entry2.Data.Name.ToLower().Contains(searchBar.Text.ToLower()))
							{
								match = false;
							}
							if (rarityDrop.SelectedItem != null && rarityDrop.SelectedItem != "All Rarities" && entry2.Data.Rarity != rarityDrop.SelectedItem)
							{
								match = false;
							}
							if (locationDrop.SelectedItem != null && locationDrop.SelectedItem != "All Locations" && entry2.Data.Location != locationDrop.SelectedItem)
							{
								match = false;
							}
							if (holeDrop.SelectedItem != null && holeDrop.SelectedItem != "All Holes" && !entry2.Data.FishingHole.Contains(holeDrop.SelectedItem))
							{
								match = false;
							}
							if (timeDrop.SelectedItem != null && timeDrop.SelectedItem != "All Times" && !entry2.Data.Time.Contains((timeDrop.SelectedItem == "Any") ? "Any" : timeDrop.SelectedItem))
							{
								match = false;
							}
							if (baitDrop.SelectedItem != null && baitDrop.SelectedItem != "All Baits" && !entry2.Data.Bait.Contains(baitDrop.SelectedItem))
							{
								match = false;
							}
							entry2.Icon.Visible = match;
							if (match)
							{
								anyVisible = true;
							}
						}
						cat.Visible = anyVisible;
						if (hasFilter && anyVisible)
						{
							cat.Collapsed = false;
						}
						else if (!hasFilter)
						{
							cat.Collapsed = false;
						}
					}
					scroll.Invalidate();
					scroll.VerticalScrollOffset = 0;
				}
			}
		}

		private async Task ShowAchievementResultsPanel(string locationName, int achievementId, int subCurrent, int subMax, string subDescription)
		{
			_openAnalyzerLocationName = locationName;
			_openAnalyzerAchievementId = achievementId;
			_openAnalyzerSubMax = subMax;
			_openAnalyzerSubDescription = subDescription;
			Microsoft.Xna.Framework.Color deepNavyBg = new Microsoft.Xna.Framework.Color(13, 27, 42);
			Microsoft.Xna.Framework.Color darkTealPanel = new Microsoft.Xna.Framework.Color(26, 47, 69);
			Microsoft.Xna.Framework.Color agedGoldText = new Microsoft.Xna.Framework.Color(201, 168, 76);
			if (_achievementResultsPanel == null)
			{
				_achievementResultsPanel = new Blish_HUD.Controls.Panel
				{
					Parent = GameService.Graphics.SpriteScreen,
					Location = new Point(400, 100),
					ShowBorder = true,
					BackgroundColor = deepNavyBg,
					ZIndex = 1001,
					ClipsBounds = false
				};
			}
			if (_achievementLegendPanel == null)
			{
				_achievementLegendPanel = new Blish_HUD.Controls.Panel
				{
					Parent = GameService.Graphics.SpriteScreen,
					Size = new Point(800, 45),
					ShowBorder = true,
					BackgroundColor = darkTealPanel,
					ZIndex = 1002
				};
				int lx = 5;
				string[] array = new string[7] { "Legendary", "Ascended", "Exotic", "Rare", "Masterwork", "Fine", "Basic" };
				foreach (string rName in array)
				{
					Microsoft.Xna.Framework.Color rColor = GetRarityColor(rName);
					new Blish_HUD.Controls.Label
					{
						Text = "■",
						Parent = _achievementLegendPanel,
						Location = new Point(lx, 15),
						TextColor = rColor,
						Font = GameService.Content.DefaultFont14,
						AutoSizeWidth = true
					};
					new Blish_HUD.Controls.Label
					{
						Text = rName,
						Parent = _achievementLegendPanel,
						Location = new Point(lx + 15, 15),
						TextColor = rColor,
						Font = GameService.Content.DefaultFont12,
						AutoSizeWidth = true
					};
					lx += rName.Length * 7 + 20;
				}
			}
			_achievementResultsPanel.Visible = true;
			try
			{
				Achievement achievementDef = await Gw2ApiManager.Gw2ApiClient.V2.Achievements.GetAsync(achievementId);
				IApiV2ObjectList<AccountAchievement> source = await Gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync();
				if (_isAnalyzerMinified)
				{
					_achievementResultsPanel.Size = new Point(320, 520);
					_achievementLegendPanel.Visible = false;
				}
				else
				{
					_achievementResultsPanel.Size = new Point(800, 600);
					_achievementLegendPanel.Visible = true;
					_achievementLegendPanel.Location = new Point(_achievementResultsPanel.Location.X, _achievementResultsPanel.Location.Y + _achievementResultsPanel.Height + 2);
				}
				Blish_HUD.Controls.Panel ghostContainer = new Blish_HUD.Controls.Panel
				{
					Size = _achievementResultsPanel.Size,
					BackgroundColor = Microsoft.Xna.Framework.Color.Transparent
				};
				Blish_HUD.Controls.Panel hBar = new Blish_HUD.Controls.Panel
				{
					Parent = ghostContainer,
					Size = new Point(ghostContainer.Width, 30),
					BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.6f,
					Location = new Point(0, 0)
				};
				hBar.LeftMouseButtonPressed += delegate
				{
					_isDraggingAchievement = true;
					_achievementDragOffset = new Point(GameService.Input.Mouse.Position.X - _achievementResultsPanel.Location.X, GameService.Input.Mouse.Position.Y - _achievementResultsPanel.Location.Y);
				};
				string titleText = (_isAnalyzerMinified ? locationName : (locationName + " Progress"));
				new Blish_HUD.Controls.Label
				{
					Text = titleText,
					Parent = hBar,
					Location = new Point(10, 5),
					Font = GameService.Content.DefaultFont16,
					TextColor = agedGoldText,
					AutoSizeWidth = true
				};
				Blish_HUD.Controls.Label closeX = new Blish_HUD.Controls.Label
				{
					Text = "X",
					Parent = hBar,
					Location = new Point(hBar.Width - 25, 5),
					Font = GameService.Content.DefaultFont16,
					TextColor = Microsoft.Xna.Framework.Color.Red,
					AutoSizeWidth = true
				};
				closeX.Click += delegate
				{
					_achievementResultsPanel.Visible = false;
					_achievementLegendPanel.Visible = false;
					_openAnalyzerAchievementId = 0;
				};
				closeX.MouseEntered += delegate
				{
					closeX.TextColor = Microsoft.Xna.Framework.Color.White;
				};
				closeX.MouseLeft += delegate
				{
					closeX.TextColor = Microsoft.Xna.Framework.Color.Red;
				};
				StandardButton standardButton = new StandardButton();
				standardButton.Text = (_isAnalyzerMinified ? "[+]" : "[-]");
				standardButton.Parent = hBar;
				standardButton.Location = new Point(hBar.Width - 75, 2);
				standardButton.Width = 40;
				standardButton.Height = 26;
				standardButton.BasicTooltipText = strings.ToggleGridView;
				standardButton.Click += async delegate
				{
					_isAnalyzerMinified = !_isAnalyzerMinified;
					await ShowAchievementResultsPanel(locationName, achievementId, subCurrent, subMax, subDescription);
				};
				AccountAchievement progress = source.FirstOrDefault((AccountAchievement a) => a.Id == achievementId);
				int totalBits = achievementDef.Bits?.Count ?? 0;
				object obj;
				if (progress == null || !progress.Done)
				{
					obj = progress?.Bits ?? new List<int>();
				}
				else
				{
					IReadOnlyList<int> readOnlyList = Enumerable.Range(0, totalBits).ToList();
					obj = readOnlyList;
				}
				IReadOnlyList<int> completedBits = (IReadOnlyList<int>)obj;
				List<int> trueMissingBits = new List<int>();
				if (achievementDef.Bits != null)
				{
					for (int k = 0; k < achievementDef.Bits!.Count; k++)
					{
						PropertyInfo idProp3 = achievementDef.Bits![k].GetType().GetProperty("Id");
						if (!(idProp3 == null))
						{
							int fishItemId3 = (int)idProp3.GetValue(achievementDef.Bits![k]);
							if (!completedBits.Contains(k) && !_caughtFishIds.Contains(fishItemId3))
							{
								trueMissingBits.Add(k);
							}
						}
					}
				}
				int missingCount = trueMissingBits.Count;
				if (_isAnalyzerMinified)
				{
					FlowPanel scrollContainer = new FlowPanel
					{
						Parent = ghostContainer,
						Location = new Point(10, 35),
						Size = new Point(ghostContainer.Width - 20, ghostContainer.Height - 45),
						CanScroll = true,
						FlowDirection = ControlFlowDirection.SingleTopToBottom,
						ControlPadding = new Vector2(0f, 10f)
					};
					new Blish_HUD.Controls.Label
					{
						Text = $"Missing Fish ({missingCount})",
						Parent = scrollContainer,
						AutoSizeWidth = true,
						TextColor = Microsoft.Xna.Framework.Color.Red,
						Font = GameService.Content.DefaultFont14
					};
					FlowPanel missingGrid = new FlowPanel
					{
						Parent = scrollContainer,
						Width = scrollContainer.Width - 15,
						HeightSizingMode = SizingMode.AutoSize,
						FlowDirection = ControlFlowDirection.LeftToRight
					};
					new Blish_HUD.Controls.Label
					{
						Text = $"All Zone Fish ({totalBits})",
						Parent = scrollContainer,
						AutoSizeWidth = true,
						TextColor = Microsoft.Xna.Framework.Color.Cyan,
						Font = GameService.Content.DefaultFont14
					};
					FlowPanel allGrid = new FlowPanel
					{
						Parent = scrollContainer,
						Width = scrollContainer.Width - 15,
						HeightSizingMode = SizingMode.AutoSize,
						FlowDirection = ControlFlowDirection.LeftToRight
					};
					if (achievementDef.Bits != null)
					{
						for (int j = 0; j < achievementDef.Bits!.Count; j++)
						{
							PropertyInfo idProp2 = achievementDef.Bits![j].GetType().GetProperty("Id");
							if (idProp2 == null)
							{
								continue;
							}
							int fishItemId2 = (int)idProp2.GetValue(achievementDef.Bits![j]);
							FishData dbFish2 = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == fishItemId2)?.Data;
							if (dbFish2 != null)
							{
								bool isMissing = trueMissingBits.Contains(j);
								if (isMissing)
								{
									CreateFishIconWithBorder(dbFish2, missingGrid);
								}
								CreateFishIconWithBorder(dbFish2, allGrid, 48, isMissing);
							}
						}
					}
				}
				else
				{
					Blish_HUD.Controls.Panel progressHeader = new Blish_HUD.Controls.Panel
					{
						Parent = ghostContainer,
						Location = new Point(10, 45),
						Size = new Point(780, 35)
					};
					FlowPanel listContainer = new FlowPanel
					{
						Parent = ghostContainer,
						Location = new Point(10, 115),
						Size = new Point(780, 475),
						CanScroll = true,
						FlowDirection = ControlFlowDirection.SingleTopToBottom,
						ControlPadding = new Vector2(0f, 2f)
					};
					new Blish_HUD.Controls.Label
					{
						Text = locationName.ToUpper() + ":",
						Parent = progressHeader,
						Location = new Point(0, 5),
						Font = GameService.Content.DefaultFont18,
						AutoSizeWidth = true,
						TextColor = agedGoldText
					};
					new Blish_HUD.Controls.Label
					{
						Text = $"MISSING: {missingCount} / {totalBits}",
						Parent = progressHeader,
						Location = new Point(310, 7),
						Font = GameService.Content.DefaultFont14,
						AutoSizeWidth = true,
						TextColor = Microsoft.Xna.Framework.Color.Cyan
					};
					if (missingCount == 0 && totalBits > 0)
					{
						new Blish_HUD.Controls.Label
						{
							Text = "✓ ACHIEVEMENT COMPLETE! GOOD JOB!",
							Parent = listContainer,
							Location = new Point(20, 20),
							Font = GameService.Content.DefaultFont16,
							TextColor = Microsoft.Xna.Framework.Color.LimeGreen,
							AutoSizeWidth = true
						};
					}
					else
					{
						Blish_HUD.Controls.Panel columnHeader = new Blish_HUD.Controls.Panel
						{
							Parent = ghostContainer,
							Location = new Point(10, 85),
							Size = new Point(780, 30),
							BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.4f
						};
						new Blish_HUD.Controls.Label
						{
							Text = strings.NameColumnHeader,
							Parent = columnHeader,
							Location = new Point(60, 5),
							Width = 170,
							TextColor = agedGoldText,
							Font = GameService.Content.DefaultFont16
						};
						new Blish_HUD.Controls.Label
						{
							Text = strings.BaitColumnHeader,
							Parent = columnHeader,
							Location = new Point(240, 5),
							Width = 130,
							TextColor = agedGoldText,
							Font = GameService.Content.DefaultFont16
						};
						new Blish_HUD.Controls.Label
						{
							Text = strings.TimeColumnHeader,
							Parent = columnHeader,
							Location = new Point(380, 5),
							Width = 150,
							TextColor = agedGoldText,
							Font = GameService.Content.DefaultFont16
						};
						new Blish_HUD.Controls.Label
						{
							Text = strings.HoleColumnHeader,
							Parent = columnHeader,
							Location = new Point(540, 5),
							Width = 230,
							TextColor = agedGoldText,
							Font = GameService.Content.DefaultFont16
						};
						if (achievementDef.Bits != null)
						{
							for (int i = 0; i < achievementDef.Bits!.Count; i++)
							{
								if (!trueMissingBits.Contains(i))
								{
									continue;
								}
								PropertyInfo idProp = achievementDef.Bits![i].GetType().GetProperty("Id");
								if (!(idProp == null))
								{
									int fishItemId = (int)idProp.GetValue(achievementDef.Bits![i]);
									FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == fishItemId)?.Data;
									Blish_HUD.Controls.Panel row = new Blish_HUD.Controls.Panel
									{
										Parent = listContainer,
										Width = 760,
										Height = 55,
										BackgroundColor = darkTealPanel,
										ShowBorder = true
									};
									if (dbFish != null)
									{
										string safeName = dbFish.Name.Replace(" ", "_").Replace("'", "").Replace("-", "");
										new Image
										{
											Texture = ContentsManager.GetTexture("images/" + safeName + ".png"),
											Parent = row,
											Location = new Point(10, 7),
											Size = new Point(40, 40)
										};
										new Blish_HUD.Controls.Label
										{
											Text = dbFish.Name,
											Parent = row,
											Location = new Point(60, 5),
											Size = new Point(170, 45),
											WrapText = true,
											VerticalAlignment = VerticalAlignment.Middle,
											TextColor = GetRarityColor(dbFish.Rarity),
											Font = GameService.Content.DefaultFont14
										};
										new Blish_HUD.Controls.Label
										{
											Text = dbFish.Bait,
											Parent = row,
											Location = new Point(240, 5),
											Size = new Point(130, 45),
											WrapText = true,
											VerticalAlignment = VerticalAlignment.Middle,
											TextColor = Microsoft.Xna.Framework.Color.LightGray,
											Font = GameService.Content.DefaultFont12
										};
										new Blish_HUD.Controls.Label
										{
											Text = dbFish.Time,
											Parent = row,
											Location = new Point(380, 5),
											Size = new Point(150, 45),
											WrapText = true,
											VerticalAlignment = VerticalAlignment.Middle,
											TextColor = Microsoft.Xna.Framework.Color.White,
											Font = GameService.Content.DefaultFont12
										};
										new Blish_HUD.Controls.Label
										{
											Text = dbFish.FishingHole,
											Parent = row,
											Location = new Point(540, 5),
											Size = new Point(210, 45),
											WrapText = true,
											VerticalAlignment = VerticalAlignment.Middle,
											TextColor = Microsoft.Xna.Framework.Color.LightGray,
											Font = GameService.Content.DefaultFont12
										};
									}
								}
							}
						}
					}
				}
				_achievementResultsPanel.ClearChildren();
				ghostContainer.Parent = _achievementResultsPanel;
				ghostContainer.Location = new Point(0, 0);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Achievement Detail Panel failed.");
			}
		}

		private Microsoft.Xna.Framework.Color GetRarityColor(string rarity)
		{
			return rarity switch
			{
				"Legendary" => Microsoft.Xna.Framework.Color.DarkOrange, 
				"Ascended" => Microsoft.Xna.Framework.Color.Violet, 
				"Exotic" => Microsoft.Xna.Framework.Color.Orange, 
				"Rare" => Microsoft.Xna.Framework.Color.Yellow, 
				"Masterwork" => Microsoft.Xna.Framework.Color.LimeGreen, 
				"Fine" => Microsoft.Xna.Framework.Color.DeepSkyBlue, 
				_ => Microsoft.Xna.Framework.Color.White, 
			};
		}

		private Blish_HUD.Controls.Panel CreateFishIconWithBorder(FishData fish, Container parent, int size = 48, bool isMissing = true)
		{
			Microsoft.Xna.Framework.Color rarityColor = GetRarityColor(fish.Rarity);
			string tooltip = fish.Name + " [" + fish.Rarity + "]\nBait: " + fish.Bait + "\nTime: " + fish.Time + "\nHole: " + fish.FishingHole;
			Blish_HUD.Controls.Panel borderPanel = new Blish_HUD.Controls.Panel
			{
				Parent = parent,
				Size = new Point(size, size),
				BackgroundColor = rarityColor,
				BasicTooltipText = tooltip
			};
			string safeName = fish.Name.Replace(" ", "_").Replace("'", "").Replace("-", "");
			new Image
			{
				Texture = ContentsManager.GetTexture("images/" + safeName + ".png"),
				Parent = borderPanel,
				Size = new Point(size - 4, size - 4),
				Location = new Point(2, 2),
				Tint = (isMissing ? Microsoft.Xna.Framework.Color.White : (Microsoft.Xna.Framework.Color.White * 0.4f)),
				BasicTooltipText = tooltip
			};
			new Image
			{
				Texture = _texCaughtCheck,
				Parent = borderPanel,
				Size = new Point(16, 16),
				Location = new Point(size - 18, size - 18),
				Visible = !isMissing
			};
			return borderPanel;
		}

		private void BuildCasualUI(Blish_HUD.Controls.Panel parent)
		{
			Microsoft.Xna.Framework.Color agedGoldText = new Microsoft.Xna.Framework.Color(201, 168, 76);
			_casualLogToggleBtn = new StandardButton
			{
				Text = strings.StartLoggingBtnText,
				Parent = parent,
				Location = new Point(10, 10),
				Width = 120
			};
			_casualMeasureBtn = new StandardButton
			{
				Text = strings.MeasureFishBtnText,
				Parent = parent,
				Location = new Point(140, 10),
				Width = 120,
				Enabled = false
			};
			_useDrfCheckbox = new Checkbox
			{
				Text = strings.UseDrfCheckboxText,
				Parent = parent,
				Location = new Point(275, 15),
				BasicTooltipText = strings.UseDrfTooltip,
				Checked = !string.IsNullOrWhiteSpace(_drfToken.Value)
			};
			StandardButton obj = new StandardButton
			{
				Text = strings.CompactModeBtnText,
				Parent = parent,
				Location = new Point(440, 10),
				Width = 120
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Text = strings.WhatsBitingBtnText;
			standardButton.Parent = parent;
			standardButton.Location = new Point(440, 45);
			standardButton.Width = 140;
			standardButton.BasicTooltipText = strings.WhatsBitingTooltip;
			standardButton.Click += delegate
			{
				ToggleBitingWidget();
			};
			StandardButton standardButton2 = new StandardButton();
			standardButton2.Text = strings.ProfitTrackerBtnText;
			standardButton2.Parent = parent;
			standardButton2.Location = new Point(310, 45);
			standardButton2.Width = 120;
			standardButton2.BasicTooltipText = strings.ProfitTrackerTooltip;
			standardButton2.Click += delegate
			{
				_showProfitWidget.Value = !_showProfitWidget.Value;
			};
			_casualSyncTimerLabel = new Blish_HUD.Controls.Label
			{
				Text = "05:00",
				Parent = parent,
				Location = new Point(140, 45),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Yellow,
				Visible = false
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.RecentCatchesText,
				Parent = parent,
				Location = new Point(10, 60),
				AutoSizeWidth = true,
				Font = GameService.Content.DefaultFont18,
				TextColor = agedGoldText
			};
			_recentCatchesPanel = new FlowPanel
			{
				Parent = parent,
				Location = new Point(10, 85),
				Size = new Point(parent.Width - 20, parent.Height - 90),
				FlowDirection = ControlFlowDirection.LeftToRight,
				CanScroll = true,
				ControlPadding = new Vector2(10f, 10f)
			};
			obj.Click += delegate
			{
				if (_isCasualLoggingActive)
				{
					_mainWindow.Visible = false;
					_casualCompactPanel.Visible = true;
					UpdateCompactCooler();
				}
			};
			_casualLogToggleBtn.Click += async delegate
			{
				if (_isCasualLoggingActive)
				{
					StopCasualLogging();
					_mainWindow.Visible = true;
					ScreenNotification.ShowNotification(strings.CasualLoggingStopped);
				}
				else
				{
					await StartCasualLogging();
				}
			};
			_casualMeasureBtn.Click += async delegate
			{
				_casualMeasureBtn.Enabled = false;
				_casualSyncTimerLabel.Text = "strings.Measuring";
				await CheckApiForNewCatches();
				_nextSyncTime = DateTime.Now.AddMinutes(5.0);
				_isSyncTimerActive = true;
			};
			UpdateRecentCatchesUI();
		}

		private async Task StartCasualLogging()
		{
			if (_isCasualLoggingActive)
			{
				return;
			}
			_isCasualLoggingActive = true;
			if (_casualLogToggleBtn != null)
			{
				_casualLogToggleBtn.Text = strings.StopLoggingBtnText;
			}
			if (_useDrfCheckbox != null)
			{
				_useDrfCheckbox.Enabled = false;
			}
			if (string.IsNullOrWhiteSpace(_drfToken.Value))
			{
				if (_casualMeasureBtn != null)
				{
					_casualMeasureBtn.Enabled = false;
				}
				await TakeInventorySnapshot();
				_nextSyncTime = DateTime.Now.AddMinutes(5.0);
				_isSyncTimerActive = true;
				if (_casualSyncTimerLabel != null)
				{
					_casualSyncTimerLabel.Text = "05:00";
					_casualSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.Yellow;
					_casualSyncTimerLabel.Visible = true;
				}
			}
			else
			{
				if (_casualMeasureBtn != null)
				{
					_casualMeasureBtn.Enabled = false;
				}
				if (_casualSyncTimerLabel != null)
				{
					_casualSyncTimerLabel.Text = strings.DrfActive;
					_casualSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.LimeGreen;
					_casualSyncTimerLabel.Visible = true;
				}
				if (_useDrfCheckbox != null)
				{
					_useDrfCheckbox.Checked = true;
				}
				StartDrfListener();
			}
			_lastAutoSubmitTime = DateTime.Now;
			_sessionStartTime = DateTime.Now;
			_sessionTotalCopper = 0;
			UpdateRecentCatchesUI();
		}

		private void ToggleBitingWidget(bool forceOpen = false)
		{
			if (_bitingWidgetPanel == null)
			{
				_bitingWidgetPanel = new Blish_HUD.Controls.Panel
				{
					Parent = GameService.Graphics.SpriteScreen,
					Width = 320,
					HeightSizingMode = SizingMode.AutoSize,
					Location = new Point(_bitingLocX.Value, _bitingLocY.Value),
					BackgroundColor = new Microsoft.Xna.Framework.Color(13, 27, 42) * 0.9f,
					ShowBorder = true,
					Visible = false,
					ZIndex = 950
				};
				Blish_HUD.Controls.Panel hBar = new Blish_HUD.Controls.Panel
				{
					Parent = _bitingWidgetPanel,
					Size = new Point(320, 30),
					BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.6f,
					Location = new Point(0, 0)
				};
				_bitingTitleLabel = new Blish_HUD.Controls.Label
				{
					Text = strings.BitingNowLoading,
					Parent = hBar,
					Location = new Point(10, 5),
					Font = GameService.Content.DefaultFont14,
					TextColor = new Microsoft.Xna.Framework.Color(201, 168, 76),
					AutoSizeWidth = true
				};
				_bitingToggleBtn = new StandardButton
				{
					Text = strings.MissingLabel,
					Parent = hBar,
					Location = new Point(hBar.Width - 95, 2),
					Width = 65,
					Height = 26,
					BasicTooltipText = strings.ToggleBitingTooltip
				};
				_bitingToggleBtn.Click += async delegate
				{
					_showAllBitingFish = !_showAllBitingFish;
					_bitingToggleBtn.Text = (_showAllBitingFish ? strings.AllLabel : strings.MissingLabel);
					await RefreshBitingWidget();
				};
				Blish_HUD.Controls.Label label = new Blish_HUD.Controls.Label();
				label.Text = "X";
				label.Parent = hBar;
				label.Location = new Point(hBar.Width - 25, 5);
				label.Font = GameService.Content.DefaultFont14;
				label.TextColor = Microsoft.Xna.Framework.Color.Red;
				label.AutoSizeWidth = true;
				label.Click += delegate
				{
					_bitingWidgetPanel.Visible = false;
				};
				hBar.LeftMouseButtonPressed += delegate
				{
					if (GameService.Input.Mouse.ActiveControl == hBar || GameService.Input.Mouse.ActiveControl == _bitingTitleLabel)
					{
						_isBitingDragging = true;
						_bitingDragOffset = new Point(GameService.Input.Mouse.Position.X - _bitingWidgetPanel.Location.X, GameService.Input.Mouse.Position.Y - _bitingWidgetPanel.Location.Y);
					}
				};
				_bitingFishList = new FlowPanel
				{
					Parent = _bitingWidgetPanel,
					Location = new Point(10, 35),
					Width = 300,
					HeightSizingMode = SizingMode.AutoSize,
					CanScroll = false,
					FlowDirection = ControlFlowDirection.LeftToRight,
					ControlPadding = new Vector2(5f, 5f)
				};
			}
			_bitingWidgetPanel.Visible = forceOpen || !_bitingWidgetPanel.Visible;
			if (_bitingWidgetPanel.Visible)
			{
				RefreshBitingWidget();
			}
		}

		private async Task RefreshBitingWidget()
		{
			if (_bitingWidgetPanel == null || !_bitingWidgetPanel.Visible)
			{
				return;
			}
			int currentMapId = GameService.Gw2Mumble.CurrentMap.Id;
			if (currentMapId == 0)
			{
				return;
			}
			string currentPhase = _currentTodPhase;
			if (string.IsNullOrEmpty(currentPhase))
			{
				currentPhase = "Day";
			}
			Dictionary<string, int> achievementMap = new Dictionary<string, int>
			{
				{ "Kryta", 6068 },
				{ "Shiverpeak", 6179 },
				{ "Ascalon", 6330 },
				{ "Maguuma Jungle", 6344 },
				{ "Ruins of Orr", 6363 },
				{ "Southsun Cove", 6106 },
				{ "Sandswept Isles", 6106 },
				{ "Elona", 6106 },
				{ "Crystal Desert", 6317 },
				{ "Ring of Fire", 6489 },
				{ "Seitung Province", 6336 },
				{ "New Kaineng City", 6342 },
				{ "The Echovald Wilds", 6258 },
				{ "Dragon's End", 6506 },
				{ "Skywatch Archipelago", 7114 },
				{ "Amnytas", 7114 },
				{ "Inner Nayos", 7114 },
				{ "Lowland Shore", 8168 },
				{ "Janthir Syntri", 8168 },
				{ "Mistburned Barrens", 8554 },
				{ "Castora", 8900 }
			};
			try
			{
				Map mapInfo = await Gw2ApiManager.Gw2ApiClient.V2.Maps.GetAsync(currentMapId);
				string target = achievementMap.Keys.FirstOrDefault((string k) => mapInfo.Name.Contains(k) || (mapInfo.RegionName != null && mapInfo.RegionName.Contains(k)));
				if (target == null)
				{
					_bitingTitleLabel.Text = strings.NoFishingZone;
					_bitingFishList.ClearChildren();
					return;
				}
				_bitingTitleLabel.Text = strings.BitingNowTitle + ": " + target + " (" + currentPhase + ")";
				int achId = achievementMap[target];
				Achievement achievementDef = await Gw2ApiManager.Gw2ApiClient.V2.Achievements.GetAsync(achId);
				AccountAchievement progress = (await Gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync()).FirstOrDefault((AccountAchievement a) => a.Id == achId);
				int totalBitsForBiting = achievementDef.Bits?.Count ?? 0;
				object obj;
				if (progress == null || !progress.Done)
				{
					obj = progress?.Bits ?? new List<int>();
				}
				else
				{
					IReadOnlyList<int> readOnlyList = Enumerable.Range(0, totalBitsForBiting).ToList();
					obj = readOnlyList;
				}
				IReadOnlyList<int> completedBits = (IReadOnlyList<int>)obj;
				_bitingFishList.ClearChildren();
				int matchCount = 0;
				if (achievementDef.Bits != null)
				{
					for (int i = 0; i < achievementDef.Bits!.Count; i++)
					{
						PropertyInfo idProp = achievementDef.Bits![i].GetType().GetProperty("Id");
						if (idProp == null)
						{
							continue;
						}
						int fishItemId = (int)idProp.GetValue(achievementDef.Bits![i]);
						bool isCaught = completedBits.Contains(i) || _caughtFishIds.Contains(fishItemId);
						if (!_showAllBitingFish && isCaught)
						{
							continue;
						}
						FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == fishItemId)?.Data;
						if (dbFish != null)
						{
							bool timeMatch = false;
							string fTime = dbFish.Time.ToLower();
							string cPhase = currentPhase.ToLower();
							if (fTime.Contains("any"))
							{
								timeMatch = true;
							}
							else if (cPhase == "dawn" && fTime.Contains("dawn"))
							{
								timeMatch = true;
							}
							else if (cPhase == "day" && fTime.Contains("daytime"))
							{
								timeMatch = true;
							}
							else if (cPhase == "dusk" && fTime.Contains("dusk"))
							{
								timeMatch = true;
							}
							else if (cPhase == "night" && fTime.Contains("nighttime"))
							{
								timeMatch = true;
							}
							if (timeMatch)
							{
								CreateFishIconWithBorder(dbFish, _bitingFishList, 38, !isCaught);
								matchCount++;
							}
						}
					}
				}
				string emptyText = (_showAllBitingFish ? "Nothing biting right now!" : "You caught everything biting right now!");
				if (matchCount == 0)
				{
					new Blish_HUD.Controls.Label
					{
						Text = emptyText,
						Parent = _bitingFishList,
						AutoSizeWidth = true,
						TextColor = Microsoft.Xna.Framework.Color.LimeGreen
					};
				}
				new Blish_HUD.Controls.Panel
				{
					Parent = _bitingFishList,
					Width = 290,
					Height = 5
				};
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Biting Widget failed on load. Retrying...");
				_bitingTitleLabel.Text = strings.BitingApiSyncing;
				await Task.Delay(2000);
				_lastBitingMapId = 0;
			}
		}

		private void UpdateRecentCatchesUI()
		{
			if (_recentCatchesPanel == null)
			{
				return;
			}
			FlowPanel flowPanel = _recentCatchesPanel as FlowPanel;
			if (flowPanel == null)
			{
				return;
			}
			flowPanel.ClearChildren();
			Microsoft.Xna.Framework.Color darkTealPanel = new Microsoft.Xna.Framework.Color(26, 47, 69);
			foreach (TournamentCatch c in _recentCatches.Take(20))
			{
				Blish_HUD.Controls.Panel card = new Blish_HUD.Controls.Panel
				{
					Parent = flowPanel,
					Size = new Point(100, 130),
					BackgroundColor = darkTealPanel,
					ShowBorder = true
				};
				Microsoft.Xna.Framework.Color catchColor = Microsoft.Xna.Framework.Color.White;
				if (c.IsSuperPb)
				{
					catchColor = Microsoft.Xna.Framework.Color.Gold;
				}
				else if (c.IsNewPb)
				{
					catchColor = Microsoft.Xna.Framework.Color.DeepSkyBlue;
				}
				FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == c.Id)?.Data;
				if (dbFish != null)
				{
					string safeName = dbFish.Name.Replace(" ", "_").Replace("'", "").Replace("-", "");
					new Image
					{
						Texture = ContentsManager.GetTexture("images/" + safeName + ".png"),
						Parent = card,
						Size = new Point(56, 56),
						Location = new Point(22, 5),
						BasicTooltipText = c.Name
					};
				}
				new Blish_HUD.Controls.Label
				{
					Text = $"{c.Weight} lbs",
					Parent = card,
					Location = new Point(0, 65),
					Width = 100,
					HorizontalAlignment = Blish_HUD.Controls.HorizontalAlignment.Center,
					TextColor = catchColor,
					Font = GameService.Content.DefaultFont14
				};
				new Blish_HUD.Controls.Label
				{
					Text = $"{c.Length} in",
					Parent = card,
					Location = new Point(0, 85),
					Width = 100,
					HorizontalAlignment = Blish_HUD.Controls.HorizontalAlignment.Center,
					TextColor = Microsoft.Xna.Framework.Color.LightGray,
					Font = GameService.Content.DefaultFont12
				};
			}
		}

		private void RefreshFishLogUI()
		{
			foreach (FishUIEntry allFishEntry in _allFishEntries)
			{
				FishData fish = allFishEntry.Data;
				Image img = allFishEntry.Icon;
				if (img == null)
				{
					continue;
				}
				bool num = _caughtFishIds.Contains(fish.ItemId) || _personalBests.ContainsKey(fish.ItemId);
				string pbWText = strings.NoneLogged;
				string pbLText = strings.NoneLogged;
				Microsoft.Xna.Framework.Color tint = (num ? Microsoft.Xna.Framework.Color.White : (Microsoft.Xna.Framework.Color.Gray * 0.5f));
				if (_personalBests.TryGetValue(fish.ItemId, out var rec))
				{
					if (rec.BestWeight != null)
					{
						if (rec.BestWeight.IsCheater)
						{
							pbWText = strings.CheaterDetected;
						}
						else if (rec.BestWeight.IsSuperPb)
						{
							pbWText = $"[SUPER] {rec.BestWeight.Weight} lbs";
							tint = Microsoft.Xna.Framework.Color.Gold;
						}
						else
						{
							pbWText = $"{rec.BestWeight.Weight} lbs";
						}
					}
					if (rec.BestLength != null)
					{
						if (rec.BestLength.IsCheater)
						{
							pbLText = strings.CheaterDetected;
						}
						else if (rec.BestLength.IsSuperPb)
						{
							pbLText = $"[SUPER] {rec.BestLength.Length} in";
							tint = Microsoft.Xna.Framework.Color.Gold;
						}
						else
						{
							pbLText = $"{rec.BestLength.Length} in";
						}
					}
				}
				img.Tint = tint;
				bool isCollector = (fish.Rarity != null && fish.Rarity.Equals("Junk", StringComparison.OrdinalIgnoreCase)) || (fish.Location != null && fish.Location.Contains("Collector"));
				string tooltip = fish.Name + "\n" + strings.BaitLabel + ": " + fish.Bait + "\n" + fish.Rarity + "\n" + fish.Location + "\n" + fish.Time;
				if (!isCollector)
				{
					tooltip = tooltip + "\n\n" + strings.PbWeightLabel + ": " + pbWText + "\n" + strings.PbLengthLabel + ": " + pbLText;
				}
				img.BasicTooltipText = tooltip;
			}
		}

		private void ShowTargetSelectionWindow(StandardButton targetBtn)
		{
			if (_targetSelectionWindow != null)
			{
				_targetSelectionWindow.Dispose();
			}
			_targetSelectionWindow = new Blish_HUD.Controls.Panel
			{
				Title = "Select Target Species",
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(400, 500),
				Location = new Point(450, 200),
				ShowBorder = true,
				BackgroundColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 230),
				CanScroll = false
			};
			_targetSelectionWindow.LeftMouseButtonPressed += delegate
			{
				if (GameService.Input.Mouse.ActiveControl == _targetSelectionWindow)
				{
					_isDraggingTarget = true;
					_targetDragOffset = new Point(GameService.Input.Mouse.Position.X - _targetSelectionWindow.Location.X, GameService.Input.Mouse.Position.Y - _targetSelectionWindow.Location.Y);
				}
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Text = strings.CloseBtnText;
			standardButton.Parent = _targetSelectionWindow;
			standardButton.Location = new Point(125, 430);
			standardButton.Width = 150;
			standardButton.Click += delegate
			{
				_targetSelectionWindow.Dispose();
			};
			FlowPanel scroll = new FlowPanel
			{
				Parent = _targetSelectionWindow,
				Size = new Point(380, 420),
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			StandardButton standardButton2 = new StandardButton();
			standardButton2.Text = strings.AllSpeciesFilterBtn;
			standardButton2.Parent = scroll;
			standardButton2.Width = 350;
			standardButton2.Click += delegate
			{
				_tourneyTargetItemId = 0;
				targetBtn.Text = strings.TargetAllSpecies;
				_targetSelectionWindow.Dispose();
			};
			foreach (IGrouping<string, FishUIEntry> g in from x in _allFishEntries
				group x by x.Data.Location into x
				orderby x.Key
				select x)
			{
				FlowPanel p = new FlowPanel
				{
					Parent = scroll,
					Title = g.Key,
					CanCollapse = true,
					ShowBorder = true,
					Width = 360,
					HeightSizingMode = SizingMode.AutoSize,
					FlowDirection = ControlFlowDirection.SingleTopToBottom,
					Collapsed = true
				};
				foreach (FishUIEntry fish in g.OrderBy((FishUIEntry x) => x.Data.Name))
				{
					StandardButton standardButton3 = new StandardButton();
					standardButton3.Text = fish.Data.Name;
					standardButton3.Parent = p;
					standardButton3.Width = 330;
					standardButton3.Click += delegate
					{
						_tourneyTargetItemId = fish.Data.ItemId;
						targetBtn.Text = strings.TargetPrefix + fish.Data.Name;
						_targetSelectionWindow.Dispose();
					};
				}
			}
		}

		private void BuildTournamentUI(Blish_HUD.Controls.Panel parent)
		{
			Microsoft.Xna.Framework.Color darkTealPanel = new Microsoft.Xna.Framework.Color(26, 47, 69);
			Microsoft.Xna.Framework.Color agedGoldText = new Microsoft.Xna.Framework.Color(201, 168, 76);
			Blish_HUD.Controls.Panel topNav = new Blish_HUD.Controls.Panel
			{
				Parent = parent,
				Size = new Point(parent.Width, 40),
				Location = new Point(0, 5)
			};
			StandardButton hostModeBtn = new StandardButton
			{
				Text = strings.HostTournamentBtnText,
				Parent = topNav,
				Location = new Point(10, 0),
				Width = 150
			};
			StandardButton obj = new StandardButton
			{
				Text = strings.JoinTournamentBtnText,
				Parent = topNav,
				Location = new Point(170, 0),
				Width = 150
			};
			_tourneyHostPanel = new Blish_HUD.Controls.Panel
			{
				Parent = parent,
				Location = new Point(0, 45),
				Size = new Point(parent.Width, parent.Height - 45),
				Visible = true
			};
			_tourneyParticipantPanel = new Blish_HUD.Controls.Panel
			{
				Parent = parent,
				Location = new Point(0, 45),
				Size = new Point(parent.Width, parent.Height - 45),
				Visible = false
			};
			hostModeBtn.Click += delegate
			{
				_tourneyHostPanel.Visible = true;
				_tourneyParticipantPanel.Visible = false;
			};
			obj.Click += delegate
			{
				_tourneyHostPanel.Visible = false;
				_tourneyParticipantPanel.Visible = true;
			};
			Blish_HUD.Controls.Panel hostSettingsBg = new Blish_HUD.Controls.Panel
			{
				Parent = _tourneyHostPanel,
				Location = new Point(10, 5),
				Size = new Point(550, 175),
				BackgroundColor = darkTealPanel,
				ShowBorder = true
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.HostSetupTitle,
				Parent = hostSettingsBg,
				Location = new Point(10, 5),
				AutoSizeWidth = true,
				TextColor = agedGoldText,
				Font = GameService.Content.DefaultFont16
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.StartDelayLabel,
				Parent = hostSettingsBg,
				Location = new Point(10, 35),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			Dropdown hostStartDelayDrop = new Dropdown
			{
				Parent = hostSettingsBg,
				Location = new Point(10, 55),
				Width = 130
			};
			hostStartDelayDrop.Items.Add("Start Immediately");
			hostStartDelayDrop.Items.Add("2 Minutes");
			hostStartDelayDrop.Items.Add("5 Minutes");
			hostStartDelayDrop.Items.Add("10 Minutes");
			new Blish_HUD.Controls.Label
			{
				Text = strings.DurationLabel,
				Parent = hostSettingsBg,
				Location = new Point(160, 35),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			Blish_HUD.Controls.TextBox hostTimerMin = new Blish_HUD.Controls.TextBox
			{
				Parent = hostSettingsBg,
				Location = new Point(160, 55),
				Width = 100,
				Text = "30"
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.TrackingModeLabel,
				Parent = hostSettingsBg,
				Location = new Point(280, 35),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			Dropdown hostTrackingModeDrop = new Dropdown
			{
				Parent = hostSettingsBg,
				Location = new Point(280, 55),
				Width = 160
			};
			hostTrackingModeDrop.Items.Add("API (5-Min Wait)");
			hostTrackingModeDrop.Items.Add("DRF (Real-Time)");
			hostTrackingModeDrop.SelectedItem = "DRF (Real-Time)";
			new Blish_HUD.Controls.Label
			{
				Text = strings.TargetSpeciesLabel,
				Parent = hostSettingsBg,
				Location = new Point(10, 95),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			StandardButton targetSpeciesBtn = new StandardButton
			{
				Text = strings.TargetAllSpecies,
				Parent = hostSettingsBg,
				Location = new Point(10, 115),
				Width = 200
			};
			targetSpeciesBtn.Click += delegate
			{
				ShowTargetSelectionWindow(targetSpeciesBtn);
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.WinFactorLabel,
				Parent = hostSettingsBg,
				Location = new Point(230, 95),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			_hostWinFactorDrop = new Dropdown
			{
				Parent = hostSettingsBg,
				Location = new Point(230, 115),
				Width = 130
			};
			_hostWinFactorDrop.Items.Add(strings.WeightDropdown);
			_hostWinFactorDrop.Items.Add(strings.LengthDropdown);
			_hostWinFactorDrop.SelectedItem = strings.WeightDropdown;
			StandardButton genKeyBtn = new StandardButton
			{
				Text = strings.CreateRoomBtn,
				Parent = hostSettingsBg,
				Location = new Point(380, 110),
				Width = 150,
				Height = 35
			};
			Blish_HUD.Controls.Panel backupBg = new Blish_HUD.Controls.Panel
			{
				Parent = _tourneyHostPanel,
				Location = new Point(10, 185),
				Size = new Point(550, 230),
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.4f,
				ShowBorder = true
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.ManualVerifyTitle,
				Parent = backupBg,
				Location = new Point(10, 5),
				AutoSizeWidth = true,
				TextColor = agedGoldText
			};
			Blish_HUD.Controls.TextBox verifyInput = new Blish_HUD.Controls.TextBox
			{
				Parent = backupBg,
				Location = new Point(10, 30),
				Width = 250,
				PlaceholderText = strings.PasteCodePlaceholder
			};
			StandardButton obj2 = new StandardButton
			{
				Text = strings.VerifyCodeBtn,
				Parent = backupBg,
				Location = new Point(270, 30),
				Width = 120
			};
			new Blish_HUD.Controls.Label
			{
				Parent = backupBg,
				Location = new Point(10, 70),
				Width = 530,
				Height = 150,
				WrapText = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray,
				Text = strings.TrackingModesInfo
			};
			Blish_HUD.Controls.Panel partSettingsBg = new Blish_HUD.Controls.Panel
			{
				Parent = _tourneyParticipantPanel,
				Location = new Point(10, 10),
				Size = new Point(550, 140),
				BackgroundColor = darkTealPanel,
				ShowBorder = true
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.JoinRoomTitle,
				Parent = partSettingsBg,
				Location = new Point(10, 5),
				AutoSizeWidth = true,
				TextColor = agedGoldText,
				Font = GameService.Content.DefaultFont16
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.EnterRoomCodeLabel,
				Parent = partSettingsBg,
				Location = new Point(10, 40),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			Blish_HUD.Controls.TextBox partSessionKey = new Blish_HUD.Controls.TextBox
			{
				Parent = partSettingsBg,
				Location = new Point(10, 65),
				Width = 180,
				PlaceholderText = "e.g. GW-1234"
			};
			StandardButton joinBtn = new StandardButton
			{
				Text = strings.JoinRoomBtn,
				Parent = partSettingsBg,
				Location = new Point(200, 65),
				Width = 150,
				Height = 32
			};
			genKeyBtn.Click += async delegate
			{
				genKeyBtn.Enabled = false;
				genKeyBtn.Text = strings.Creating;
				string charName = GameService.Gw2Mumble.PlayerCharacter.Name;
				if (string.IsNullOrEmpty(charName))
				{
					charName = "Host";
				}
				int delayMins = 0;
				if (hostStartDelayDrop.SelectedItem == "2 Minutes")
				{
					delayMins = 2;
				}
				if (hostStartDelayDrop.SelectedItem == "5 Minutes")
				{
					delayMins = 5;
				}
				if (hostStartDelayDrop.SelectedItem == "10 Minutes")
				{
					delayMins = 10;
				}
				string mode = (hostTrackingModeDrop.SelectedItem.Contains("API") ? "API" : "DRF");
				string apiWinFactor = ((_hostWinFactorDrop.SelectedItem == strings.LengthDropdown) ? "Length" : "Weight");
				var payload = new
				{
					hostName = charName,
					startDelayMins = delayMins,
					durationMins = int.Parse(hostTimerMin.Text),
					mode = mode,
					targetId = _tourneyTargetItemId,
					winFactor = apiWinFactor,
					webhookUrl = _discordWebhookUrl.Value
				};
				try
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
					HttpResponseMessage response2 = await _httpClient.PostAsync("https://api.gilledwars.com/create", (HttpContent)(object)content);
					string resultString2 = await response2.get_Content().ReadAsStringAsync();
					if (response2.get_IsSuccessStatusCode())
					{
						Dictionary<string, string> resultObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultString2);
						if (resultObj != null && resultObj.ContainsKey("roomCode"))
						{
							string code = resultObj["roomCode"];
							CopyToClipboard(code);
							ScreenNotification.ShowNotification(string.Format(strings.RoomCreated, code));
						}
					}
					else
					{
						ScreenNotification.ShowNotification("strings.ApiErrorRoom", ScreenNotification.NotificationType.Error);
					}
				}
				catch (Exception ex2)
				{
					Logger.Error(ex2, "Failed to connect to API.");
					ScreenNotification.ShowNotification("Network Error: Could not connect to API.", ScreenNotification.NotificationType.Error);
				}
				genKeyBtn.Enabled = true;
				genKeyBtn.Text = strings.CreateRoomBtn;
			};
			obj2.Click += delegate
			{
				try
				{
					string @string = Encoding.UTF8.GetString(Convert.FromBase64String(verifyInput.Text));
					int num = @string.LastIndexOf('|');
					if (num == -1)
					{
						throw new Exception();
					}
					string text = @string.Substring(0, num);
					string text2 = @string.Substring(num + 1);
					string[] array = text.Split('|');
					string salt = array[0];
					string text3 = GenerateMasterSignature(text, salt);
					if (text2 != text3)
					{
						ShowTournamentSummary("Error", "TAMPERED RESULTS\nSignatures do not match.", Microsoft.Xna.Framework.Color.Red);
					}
					else
					{
						string characterName = array[1];
						List<TournamentCatch> list = new List<TournamentCatch>();
						for (int i = 2; i < array.Length; i++)
						{
							string[] array2 = array[i].Split(',');
							int fId = int.Parse(array2[0]);
							double weight = double.Parse(array2[1]);
							double length = double.Parse(array2[2]);
							FishData fishData = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == fId)?.Data;
							if (fishData != null)
							{
								list.Add(new TournamentCatch
								{
									Id = fId,
									Name = fishData.Name,
									Weight = weight,
									Length = length,
									Rarity = fishData.Rarity,
									CharacterName = characterName
								});
							}
						}
						string selectedItem = _hostWinFactorDrop.SelectedItem;
						ShowTournamentSummary("Verified Results", null, Microsoft.Xna.Framework.Color.LimeGreen, list, selectedItem);
					}
				}
				catch
				{
					ShowTournamentSummary("Error", "INVALID DATA\nEnsure the player copied the entire string.", Microsoft.Xna.Framework.Color.Red);
				}
			};
			joinBtn.Click += async delegate
			{
				joinBtn.Enabled = false;
				joinBtn.Text = strings.Joining;
				try
				{
					string roomCode = partSessionKey.Text.Trim().ToUpper();
					if (!roomCode.StartsWith("GW-") || roomCode.Length != 8)
					{
						ScreenNotification.ShowNotification(strings.InvalidRoomCode, ScreenNotification.NotificationType.Error);
						joinBtn.Enabled = true;
						joinBtn.Text = strings.JoinRoomBtn;
						return;
					}
					HttpResponseMessage response = await _httpClient.GetAsync("https://api.gilledwars.com/join/" + roomCode);
					string resultString = await response.get_Content().ReadAsStringAsync();
					if (response.get_IsSuccessStatusCode())
					{
						Dictionary<string, object> tData = JsonConvert.DeserializeObject<Dictionary<string, object>>(resultString);
						StopCasualLogging();
						StopDrfListener();
						_tourneyRoomCode = roomCode;
						_tourneyModeUsed = tData["mode"].ToString();
						_tourneyTargetItemId = Convert.ToInt32(tData["targetId"]);
						_tourneyWinFactor = tData["winFactor"].ToString();
						int durMins = Convert.ToInt32(tData["durationMins"]);
						long startTimeMs = Convert.ToInt64(tData["startTime"]);
						_tourneyStartTimeUtc = DateTimeOffset.FromUnixTimeMilliseconds(startTimeMs).UtcDateTime;
						_tourneyEndTimeUtc = _tourneyStartTimeUtc.AddMinutes(durMins);
						_tourneyCatches.Clear();
						string targetName = strings.AllSpeciesFilterBtn;
						if (_tourneyTargetItemId != 0)
						{
							FishUIEntry targetFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == _tourneyTargetItemId);
							if (targetFish != null)
							{
								targetName = targetFish.Data.Name;
							}
						}
						if (_tourneyActivePanel != null)
						{
							_tourneyActivePanel.Title = targetName + " (" + _tourneyWinFactor + ")";
						}
						UpdateActiveTourneyCoolerUI();
						if (DateTime.UtcNow < _tourneyStartTimeUtc)
						{
							_isTourneyWaitingRoom = true;
							_waitingRoomLabel.Visible = true;
							_activeTimerLabel.Visible = false;
							_activeMeasureBtn.Enabled = false;
							if (_activeSyncTimerLabel != null)
							{
								_activeSyncTimerLabel.Text = ((_tourneyModeUsed == "DRF") ? strings.DrfPaused : strings.ApiPaused);
								_activeSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.LightGray;
								_activeSyncTimerLabel.Visible = true;
							}
							ScreenNotification.ShowNotification(strings.DrfResumeWait, ScreenNotification.NotificationType.Warning);
						}
						else
						{
							_isTourneyWaitingRoom = false;
							_isTournamentActive = true;
							_waitingRoomLabel.Visible = false;
							_activeTimerLabel.Visible = true;
							StartTrackingMode();
							ScreenNotification.ShowNotification(strings.TourneyStarted);
						}
						_mainWindow.Visible = false;
						_tourneyActivePanel.Visible = true;
					}
					else
					{
						ScreenNotification.ShowNotification(strings.RoomNotFound, ScreenNotification.NotificationType.Error);
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "API Join Failed");
					ScreenNotification.ShowNotification(strings.NetworkError, ScreenNotification.NotificationType.Error);
				}
				joinBtn.Enabled = true;
				joinBtn.Text = strings.JoinRoomBtn;
			};
		}

		private async void StartTrackingMode()
		{
			if (_tourneyModeUsed == "API")
			{
				await TakeInventorySnapshot();
				_activeMeasureBtn.Enabled = false;
				_nextSyncTime = DateTime.Now.AddMinutes(5.0);
				_isSyncTimerActive = true;
				_activeSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.Yellow;
				_activeSyncTimerLabel.Visible = true;
			}
			else
			{
				_activeMeasureBtn.Enabled = false;
				_activeSyncTimerLabel.Text = "DRF Active";
				_activeSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.LimeGreen;
				_activeSyncTimerLabel.Visible = true;
				StartDrfListener();
			}
		}

		private void BuildActiveTournamentWidget()
		{
			_tourneyActivePanel = new Blish_HUD.Controls.Panel
			{
				Title = "Active Tournament",
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(320, 350),
				Location = new Point(400, 300),
				ShowBorder = true,
				BackgroundColor = new Microsoft.Xna.Framework.Color(13, 27, 42),
				Visible = false
			};
			_tourneyActivePanel.LeftMouseButtonPressed += delegate
			{
				if (GameService.Input.Mouse.ActiveControl == _tourneyActivePanel)
				{
					_isActivePanelDragging = true;
					_activePanelDragOffset = new Point(GameService.Input.Mouse.Position.X - _tourneyActivePanel.Location.X, GameService.Input.Mouse.Position.Y - _tourneyActivePanel.Location.Y);
				}
			};
			_activeTimerLabel = new Blish_HUD.Controls.Label
			{
				Text = "00:00",
				Parent = _tourneyActivePanel,
				Location = new Point(10, 10),
				Font = GameService.Content.DefaultFont32,
				TextColor = Microsoft.Xna.Framework.Color.White,
				AutoSizeWidth = true,
				Visible = false
			};
			_waitingRoomLabel = new Blish_HUD.Controls.Label
			{
				Text = strings.StartingIn,
				Parent = _tourneyActivePanel,
				Location = new Point(10, 10),
				Font = GameService.Content.DefaultFont18,
				TextColor = Microsoft.Xna.Framework.Color.Yellow,
				AutoSizeWidth = true,
				Visible = false
			};
			_activeSyncTimerLabel = new Blish_HUD.Controls.Label
			{
				Text = "05:00",
				Parent = _tourneyActivePanel,
				Location = new Point(190, 85),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Yellow,
				Visible = false
			};
			_activeMeasureBtn = new StandardButton
			{
				Text = strings.MeasureFishBtnText,
				Parent = _tourneyActivePanel,
				Location = new Point(180, 10),
				Width = 110,
				Enabled = false
			};
			_activeEndBtn = new StandardButton
			{
				Text = strings.EndSubmitBtn,
				Parent = _tourneyActivePanel,
				Location = new Point(10, 50),
				Width = 120
			};
			_activeRecopyBtn = new StandardButton
			{
				Text = strings.ReCopyCodeBtn,
				Parent = _tourneyActivePanel,
				Location = new Point(10, 50),
				Width = 120,
				Visible = false,
				BasicTooltipText = strings.ReCopyCodeTooltip
			};
			_activeExitBtn = new StandardButton
			{
				Text = strings.ExitTourneyBtn,
				Parent = _tourneyActivePanel,
				Location = new Point(140, 50),
				Width = 120,
				Visible = false,
				BasicTooltipText = strings.ExitTourneyTooltip
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.Top5CatchesLabel,
				Parent = _tourneyActivePanel,
				Location = new Point(10, 85),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Cyan
			};
			_activeCoolerList = new FlowPanel
			{
				Parent = _tourneyActivePanel,
				Location = new Point(10, 110),
				Size = new Point(290, 230),
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			_activeMeasureBtn.Click += async delegate
			{
				_activeMeasureBtn.Enabled = false;
				_activeSyncTimerLabel.Text = strings.Measuring;
				await CheckApiForNewCatches();
				_nextSyncTime = DateTime.Now.AddMinutes(5.0);
				_isSyncTimerActive = true;
			};
			_activeEndBtn.Click += async delegate
			{
				await CompleteTournamentAsync(strings.TourneyEndedManualTitle, strings.TourneyResultsInitial);
			};
			_activeRecopyBtn.Click += delegate
			{
				if (!string.IsNullOrEmpty(_lastGeneratedCode))
				{
					CopyToClipboard(_lastGeneratedCode);
					ScreenNotification.ShowNotification(strings.BackupCopied);
				}
			};
			_activeExitBtn.Click += delegate
			{
				StopDrfListener();
				StopCasualLogging();
				_isTournamentActive = false;
				_isTourneyWaitingRoom = false;
				_isTourneyWrapUpActive = false;
				_isSyncTimerActive = false;
				_lastGeneratedCode = "";
				_tourneyRoomCode = "";
				_tourneyCatches.Clear();
				_activeCoolerList.ClearChildren();
				_activeRecopyBtn.Visible = false;
				_activeExitBtn.Visible = false;
				_activeEndBtn.Visible = true;
				_activeMeasureBtn.Visible = true;
				_activeMeasureBtn.Enabled = false;
				_waitingRoomLabel.Visible = false;
				_activeTimerLabel.Visible = false;
				_tourneyActivePanel.Visible = false;
				_mainWindow.Visible = true;
				ScreenNotification.ShowNotification(strings.ExitedTournament);
				StartCasualLogging();
			};
		}

		private void BuildCasualCompactPanel()
		{
			Microsoft.Xna.Framework.Color deepNavyBg = new Microsoft.Xna.Framework.Color(13, 27, 42);
			Microsoft.Xna.Framework.Color agedGoldText = new Microsoft.Xna.Framework.Color(201, 168, 76);
			_casualCompactPanel = new Blish_HUD.Controls.Panel
			{
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(320, 240),
				Location = new Point(400, 300),
				ShowBorder = true,
				BackgroundColor = deepNavyBg,
				Visible = false,
				ZIndex = 1001
			};
			Blish_HUD.Controls.Panel headerBar = new Blish_HUD.Controls.Panel
			{
				Parent = _casualCompactPanel,
				Size = new Point(_casualCompactPanel.Width, 30),
				Location = new Point(0, 0),
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.6f
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.CasualFishingLabel,
				Parent = headerBar,
				Location = new Point(10, 5),
				Font = GameService.Content.DefaultFont16,
				TextColor = agedGoldText,
				AutoSizeWidth = true
			};
			headerBar.LeftMouseButtonPressed += delegate
			{
				_isCompactDragging = true;
				_compactDragOffset = new Point(GameService.Input.Mouse.Position.X - _casualCompactPanel.Location.X, GameService.Input.Mouse.Position.Y - _casualCompactPanel.Location.Y);
			};
			new Blish_HUD.Controls.Label
			{
				Text = strings.RecentCatchesCompact,
				Parent = _casualCompactPanel,
				Location = new Point(10, 40),
				AutoSizeWidth = true,
				Font = GameService.Content.DefaultFont14,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			_compactCoolerList = new FlowPanel
			{
				Parent = _casualCompactPanel,
				Location = new Point(10, 65),
				Size = new Point(300, 130),
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			_compactMaxBtn = new StandardButton
			{
				Text = strings.MaximizeUI,
				Parent = _casualCompactPanel,
				Location = new Point(100, 200),
				Width = 120
			};
			_compactMaxBtn.Click += delegate
			{
				_casualCompactPanel.Visible = false;
				_mainWindow.Visible = true;
			};
		}

		private void UpdateCompactCooler()
		{
			if (_compactCoolerList == null)
			{
				return;
			}
			_compactCoolerList.ClearChildren();
			foreach (TournamentCatch c in _recentCatches.Take(5))
			{
				Microsoft.Xna.Framework.Color catchColor = Microsoft.Xna.Framework.Color.White;
				if (c.IsSuperPb)
				{
					catchColor = Microsoft.Xna.Framework.Color.Gold;
				}
				else if (c.IsNewPb)
				{
					catchColor = Microsoft.Xna.Framework.Color.DeepSkyBlue;
				}
				new Blish_HUD.Controls.Label
				{
					Text = $"{c.Name} - {c.Weight} lbs | {c.Length} in",
					Parent = _compactCoolerList,
					AutoSizeWidth = true,
					TextColor = catchColor,
					Font = GameService.Content.DefaultFont18
				};
			}
		}

		private async Task CompleteTournamentAsync(string title, string msg)
		{
			_isTournamentActive = false;
			_isTourneyWaitingRoom = false;
			_isTourneyWrapUpActive = false;
			if (_activeMeasureBtn != null)
			{
				_activeMeasureBtn.Enabled = false;
			}
			_isSyncTimerActive = false;
			StopDrfListener();
			StopCasualLogging();
			List<TournamentCatch> list = ((_tourneyWinFactor == "Length") ? _tourneyCatches.OrderByDescending((TournamentCatch x) => x.Length) : _tourneyCatches.OrderByDescending((TournamentCatch x) => x.Weight)).Take(5).ToList();
			List<string> parts = new List<string>();
			string charName = GameService.Gw2Mumble.PlayerCharacter.Name;
			if (string.IsNullOrEmpty(charName))
			{
				charName = "UnknownPlayer";
			}
			parts.Add(_tourneyRoomCode);
			parts.Add(charName);
			List<object> catchPayloadList = new List<object>();
			foreach (TournamentCatch c in list)
			{
				parts.Add($"{c.Id},{c.Weight},{c.Length}");
				catchPayloadList.Add(new
				{
					id = c.Id,
					name = c.Name,
					weight = c.Weight,
					length = c.Length
				});
			}
			string rawPayload = string.Join("|", parts);
			string masterSig = GenerateMasterSignature(rawPayload, _tourneyRoomCode);
			_lastGeneratedCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawPayload + "|" + masterSig));
			try
			{
				string accountName = "UnknownAccount";
				if (Gw2ApiManager.HasPermissions(new TokenPermission[1] { TokenPermission.Account }))
				{
					accountName = (await Gw2ApiManager.Gw2ApiClient.V2.Account.GetAsync()).Name;
				}
				StringContent content = new StringContent(JsonConvert.SerializeObject(new
				{
					accountName = accountName,
					playerName = charName,
					verifyCode = _lastGeneratedCode,
					catches = catchPayloadList
				}), Encoding.UTF8, "application/json");
				if ((await _httpClient.PostAsync("https://api.gilledwars.com/submit/" + _tourneyRoomCode, (HttpContent)(object)content)).get_IsSuccessStatusCode())
				{
					msg = strings.TourneyPostedDiscord;
					ScreenNotification.ShowNotification(strings.ResultsSubmitted);
				}
				else
				{
					msg = strings.TourneyApiFailed;
					CopyToClipboard(_lastGeneratedCode);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to submit results.");
				msg = strings.TourneyNetworkError;
				CopyToClipboard(_lastGeneratedCode);
			}
			if (_activeEndBtn != null)
			{
				_activeEndBtn.Visible = false;
			}
			if (_activeMeasureBtn != null)
			{
				_activeMeasureBtn.Visible = false;
			}
			if (_activeRecopyBtn != null)
			{
				_activeRecopyBtn.Visible = true;
			}
			if (_activeExitBtn != null)
			{
				_activeExitBtn.Visible = true;
			}
			ShowTournamentSummary(title, msg, Microsoft.Xna.Framework.Color.Cyan, _tourneyCatches, _tourneyWinFactor);
			ScreenNotification.ShowNotification(strings.TourneyComplete, ScreenNotification.NotificationType.Warning);
			StartCasualLogging();
		}

		private void UpdateActiveTourneyCoolerUI()
		{
			if (_activeCoolerList == null)
			{
				return;
			}
			_activeCoolerList.ClearChildren();
			foreach (TournamentCatch c in ((_tourneyWinFactor == "Length") ? _tourneyCatches.OrderByDescending((TournamentCatch x) => x.Length) : _tourneyCatches.OrderByDescending((TournamentCatch x) => x.Weight)).Take(5))
			{
				Microsoft.Xna.Framework.Color catchColor = Microsoft.Xna.Framework.Color.White;
				if (c.IsSuperPb)
				{
					catchColor = Microsoft.Xna.Framework.Color.Gold;
				}
				else if (c.IsNewPb)
				{
					catchColor = Microsoft.Xna.Framework.Color.DeepSkyBlue;
				}
				Blish_HUD.Controls.Panel row = new Blish_HUD.Controls.Panel
				{
					Parent = _activeCoolerList,
					Size = new Point(270, 65),
					BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.4f,
					ShowBorder = true
				};
				FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == c.Id)?.Data;
				if (dbFish != null)
				{
					string safeName = dbFish.Name.Replace(" ", "_").Replace("'", "").Replace("-", "");
					new Image
					{
						Texture = ContentsManager.GetTexture("images/" + safeName + ".png"),
						Parent = row,
						Location = new Point(5, 16),
						Size = new Point(32, 32)
					};
				}
				string statText = ((_tourneyWinFactor == "Length") ? $"{c.Length} in | {c.Weight} lbs" : $"{c.Weight} lbs | {c.Length} in");
				new Blish_HUD.Controls.Label
				{
					Text = (c.Name ?? ""),
					Parent = row,
					Location = new Point(45, 8),
					AutoSizeWidth = true,
					AutoSizeHeight = true,
					TextColor = catchColor,
					Font = GameService.Content.DefaultFont14
				};
				new Blish_HUD.Controls.Label
				{
					Text = statText,
					Parent = row,
					Location = new Point(45, 34),
					AutoSizeWidth = true,
					AutoSizeHeight = true,
					TextColor = Microsoft.Xna.Framework.Color.LightGray,
					Font = GameService.Content.DefaultFont12
				};
			}
		}

		private void ShowTournamentSummary(string title, string errorMsg, Microsoft.Xna.Framework.Color ColorTheme, List<TournamentCatch> catches = null, string winFactor = "Weight")
		{
			Microsoft.Xna.Framework.Color deepNavyBg = new Microsoft.Xna.Framework.Color(13, 27, 42);
			Microsoft.Xna.Framework.Color darkTealPanel = new Microsoft.Xna.Framework.Color(26, 47, 69);
			Microsoft.Xna.Framework.Color agedGoldText = new Microsoft.Xna.Framework.Color(201, 168, 76);
			if (_currentSummaryWindow != null)
			{
				_currentSummaryWindow.Dispose();
			}
			_currentSummaryWindow = new Blish_HUD.Controls.Panel
			{
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(450, 480),
				Location = new Point(500, 300),
				ShowBorder = true,
				BackgroundColor = deepNavyBg,
				ClipsBounds = false
			};
			Blish_HUD.Controls.Panel hBar = new Blish_HUD.Controls.Panel
			{
				Parent = _currentSummaryWindow,
				Size = new Point(_currentSummaryWindow.Width, 30),
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.6f,
				Location = new Point(0, 0)
			};
			new Blish_HUD.Controls.Label
			{
				Text = title,
				Parent = hBar,
				Location = new Point(10, 5),
				Font = GameService.Content.DefaultFont16,
				TextColor = agedGoldText,
				AutoSizeWidth = true
			};
			Blish_HUD.Controls.Label closeX = new Blish_HUD.Controls.Label
			{
				Text = "X",
				Parent = hBar,
				Location = new Point(hBar.Width - 25, 5),
				Font = GameService.Content.DefaultFont16,
				TextColor = Microsoft.Xna.Framework.Color.Red,
				AutoSizeWidth = true
			};
			closeX.Click += delegate
			{
				_currentSummaryWindow.Dispose();
			};
			closeX.MouseEntered += delegate
			{
				closeX.TextColor = Microsoft.Xna.Framework.Color.White;
			};
			closeX.MouseLeft += delegate
			{
				closeX.TextColor = Microsoft.Xna.Framework.Color.Red;
			};
			hBar.LeftMouseButtonPressed += delegate
			{
				_isDraggingSummary = true;
				_summaryDragOffset = new Point(GameService.Input.Mouse.Position.X - _currentSummaryWindow.Location.X, GameService.Input.Mouse.Position.Y - _currentSummaryWindow.Location.Y);
			};
			FlowPanel list = new FlowPanel
			{
				Parent = _currentSummaryWindow,
				Location = new Point(20, 45),
				Size = new Point(410, 380),
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(0f, 5f)
			};
			if (!string.IsNullOrEmpty(errorMsg))
			{
				new Blish_HUD.Controls.Label
				{
					Text = errorMsg,
					Parent = list,
					AutoSizeHeight = true,
					WrapText = true,
					Width = 380,
					TextColor = ColorTheme,
					Font = GameService.Content.DefaultFont18
				};
			}
			if (catches != null && catches.Count > 0)
			{
				new Blish_HUD.Controls.Label
				{
					Text = "Angler: " + (catches.FirstOrDefault()?.CharacterName ?? GameService.Gw2Mumble.PlayerCharacter.Name),
					Parent = list,
					AutoSizeWidth = true,
					Font = GameService.Content.DefaultFont18,
					TextColor = agedGoldText
				};
				foreach (TournamentCatch c in ((winFactor == "Length") ? catches.OrderByDescending((TournamentCatch x) => x.Length) : catches.OrderByDescending((TournamentCatch x) => x.Weight)).Take(5))
				{
					Blish_HUD.Controls.Panel row = new Blish_HUD.Controls.Panel
					{
						Parent = list,
						Size = new Point(380, 65),
						BackgroundColor = darkTealPanel,
						ShowBorder = true
					};
					FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == c.Id)?.Data;
					if (dbFish != null)
					{
						string safeName = dbFish.Name.Replace(" ", "_").Replace("'", "").Replace("-", "");
						new Image
						{
							Texture = ContentsManager.GetTexture("images/" + safeName + ".png"),
							Parent = row,
							Location = new Point(5, 16),
							Size = new Point(32, 32)
						};
					}
					string statText = ((winFactor == "Length") ? $"{c.Length} in | {c.Weight} lbs" : $"{c.Weight} lbs | {c.Length} in");
					new Blish_HUD.Controls.Label
					{
						Text = "[" + c.Rarity + "] " + c.Name,
						Parent = row,
						Location = new Point(45, 8),
						AutoSizeWidth = true,
						AutoSizeHeight = true,
						TextColor = Microsoft.Xna.Framework.Color.White,
						Font = GameService.Content.DefaultFont14
					};
					new Blish_HUD.Controls.Label
					{
						Text = statText,
						Parent = row,
						Location = new Point(45, 34),
						AutoSizeWidth = true,
						AutoSizeHeight = true,
						TextColor = Microsoft.Xna.Framework.Color.LightGray,
						Font = GameService.Content.DefaultFont12
					};
				}
			}
			StandardButton standardButton = new StandardButton();
			standardButton.Text = strings.CloseBtnText;
			standardButton.Parent = _currentSummaryWindow;
			standardButton.Location = new Point(150, 435);
			standardButton.Width = 150;
			standardButton.Click += delegate
			{
				_currentSummaryWindow.Dispose();
			};
		}

		protected override void Update(GameTime gt)
		{
			if (_isProfitDragging && _profitWidgetPanel != null)
			{
				_profitWidgetPanel.Location = new Point(GameService.Input.Mouse.Position.X - _profitDragOffset.X, GameService.Input.Mouse.Position.Y - _profitDragOffset.Y);
			}
			if (_isCasualLoggingActive && _profitWidgetPanel != null && _profitWidgetPanel.Visible && _sessionStartTime != DateTime.MinValue)
			{
				_profitTotalLabel.Text = "Total: " + FormatMoney(_sessionTotalCopper);
				double elapsedHours = (DateTime.Now - _sessionStartTime).TotalHours;
				if (elapsedHours > 0.0005)
				{
					int gphCopper = (int)((double)_sessionTotalCopper / elapsedHours);
					_gphLabel.Text = "GpH: " + FormatMoney(gphCopper);
				}
			}
			if (_isBitingDragging && _bitingWidgetPanel != null)
			{
				_bitingWidgetPanel.Location = new Point(GameService.Input.Mouse.Position.X - _bitingDragOffset.X, GameService.Input.Mouse.Position.Y - _bitingDragOffset.Y);
			}
			if (GameService.Gw2Mumble.CurrentMap.Id != _lastBitingMapId)
			{
				_lastBitingMapId = GameService.Gw2Mumble.CurrentMap.Id;
				RefreshBitingWidget();
			}
			if (_isCasualLoggingActive && _useDrfCheckbox != null && _useDrfCheckbox.Checked && (DateTime.Now - _lastAutoSubmitTime).TotalMinutes >= 15.0)
			{
				_lastAutoSubmitTime = DateTime.Now;
				ForceUploadPB(isAuto: true);
			}
			if (_isDragging && _mainWindow != null)
			{
				_mainWindow.Location = new Point(GameService.Input.Mouse.Position.X - _dragOffset.X, GameService.Input.Mouse.Position.Y - _dragOffset.Y);
			}
			if (_isActivePanelDragging && _tourneyActivePanel != null)
			{
				_tourneyActivePanel.Location = new Point(GameService.Input.Mouse.Position.X - _activePanelDragOffset.X, GameService.Input.Mouse.Position.Y - _activePanelDragOffset.Y);
			}
			if (_isDraggingSummary && _currentSummaryWindow != null)
			{
				_currentSummaryWindow.Location = new Point(GameService.Input.Mouse.Position.X - _summaryDragOffset.X, GameService.Input.Mouse.Position.Y - _summaryDragOffset.Y);
			}
			if (_isCompactDragging && _casualCompactPanel != null)
			{
				_casualCompactPanel.Location = new Point(GameService.Input.Mouse.Position.X - _compactDragOffset.X, GameService.Input.Mouse.Position.Y - _compactDragOffset.Y);
			}
			if (_isDraggingTarget && _targetSelectionWindow != null)
			{
				_targetSelectionWindow.Location = new Point(GameService.Input.Mouse.Position.X - _targetDragOffset.X, GameService.Input.Mouse.Position.Y - _targetDragOffset.Y);
			}
			if (_isDraggingLeaderboard && _leaderboardWindow != null)
			{
				_leaderboardWindow.Location = new Point(GameService.Input.Mouse.Position.X - _leaderboardDragOffset.X, GameService.Input.Mouse.Position.Y - _leaderboardDragOffset.Y);
			}
			if (_isSpeciesSelectionDragging && _speciesSelectionWindow != null)
			{
				_speciesSelectionWindow.Location = new Point(GameService.Input.Mouse.Position.X - _speciesSelectionDragOffset.X, GameService.Input.Mouse.Position.Y - _speciesSelectionDragOffset.Y);
			}
			if (_isDraggingAchievement && _achievementResultsPanel != null)
			{
				_achievementResultsPanel.Location = new Point(GameService.Input.Mouse.Position.X - _achievementDragOffset.X, GameService.Input.Mouse.Position.Y - _achievementDragOffset.Y);
				if (_achievementLegendPanel != null)
				{
					_achievementLegendPanel.Location = new Point(_achievementResultsPanel.Location.X, _achievementResultsPanel.Location.Y + _achievementResultsPanel.Height + 2);
				}
			}
			if (_isMetaDragging && _metaProgressWindow != null)
			{
				_metaProgressWindow.Location = new Point(GameService.Input.Mouse.Position.X - _metaDragOffset.X, GameService.Input.Mouse.Position.Y - _metaDragOffset.Y);
			}
			if (_isTodDragging && _timeOfDayPanel != null)
			{
				_timeOfDayPanel.Location = new Point(GameService.Input.Mouse.Position.X - _todDragOffset.X, GameService.Input.Mouse.Position.Y - _todDragOffset.Y);
			}
			if (_timeOfDayPanel != null && _timeOfDayPanel.Visible)
			{
				double currentCycle = DateTime.UtcNow.TimeOfDay.TotalMinutes % 120.0;
				string phase = "";
				double remainingMinutes = 0.0;
				AsyncTexture2D targetTex = null;
				Microsoft.Xna.Framework.Color phaseColor = Microsoft.Xna.Framework.Color.White;
				if (currentCycle >= 25.0 && currentCycle < 30.0)
				{
					phase = "Dawn";
					remainingMinutes = 30.0 - currentCycle;
					targetTex = _texDawn;
					phaseColor = new Microsoft.Xna.Framework.Color(255, 200, 150);
				}
				else if (currentCycle >= 30.0 && currentCycle < 100.0)
				{
					phase = "Day";
					remainingMinutes = 100.0 - currentCycle;
					targetTex = _texDay;
					phaseColor = Microsoft.Xna.Framework.Color.LightSkyBlue;
				}
				else if (currentCycle >= 100.0 && currentCycle < 105.0)
				{
					phase = "Dusk";
					remainingMinutes = 105.0 - currentCycle;
					targetTex = _texDusk;
					phaseColor = Microsoft.Xna.Framework.Color.Orange;
				}
				else
				{
					phase = "Night";
					remainingMinutes = ((!(currentCycle >= 105.0)) ? (25.0 - currentCycle) : (145.0 - currentCycle));
					targetTex = _texNight;
					phaseColor = new Microsoft.Xna.Framework.Color(220, 190, 255);
				}
				TimeSpan timeRemaining = TimeSpan.FromMinutes(remainingMinutes);
				_todLabel.Text = $"{phase}: {timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";
				_todLabel.TextColor = phaseColor;
				if (_currentTodPhase != phase)
				{
					bool num = string.IsNullOrEmpty(_currentTodPhase);
					_currentTodPhase = phase;
					RefreshBitingWidget();
					if (num)
					{
						_todIcon.Texture = targetTex;
					}
					else
					{
						GameService.Animation.Tweener.Tween(_todIcon, new
						{
							Opacity = 0f
						}, 0.5f).OnComplete(delegate
						{
							_todIcon.Texture = targetTex;
							GameService.Animation.Tweener.Tween(_todIcon, new
							{
								Opacity = 1f
							}, 0.5f);
						});
					}
				}
			}
			if (_isCheater && _cheaterLabel != null)
			{
				_cheaterTimer += gt.ElapsedGameTime.TotalSeconds;
				if (_cheaterTimer >= 25.0)
				{
					_cheaterTimer = 0.0;
					int maxX = Math.Max(100, GameService.Graphics.SpriteScreen.Width - 800);
					int maxY = Math.Max(100, GameService.Graphics.SpriteScreen.Height - 100);
					_cheaterLabel.Location = new Point(_rnd.Next(50, maxX), _rnd.Next(50, maxY));
					string charName = GameService.Gw2Mumble.PlayerCharacter.Name;
					if (string.IsNullOrEmpty(charName))
					{
						charName = "Player";
					}
					_cheaterLabel.Text = "CHEATER DETECTED: " + charName + "\nPlease disable Gilled Wars or revert your personal best file\nto stop this message!";
				}
				if (_cheaterTimer < 5.0)
				{
					_cheaterLabel.Visible = true;
					if (_cheaterTimer < 1.0)
					{
						_cheaterLabel.Opacity = (float)_cheaterTimer;
					}
					else if (_cheaterTimer > 4.0)
					{
						_cheaterLabel.Opacity = (float)(5.0 - _cheaterTimer);
					}
					else
					{
						_cheaterLabel.Opacity = 1f;
					}
				}
				else
				{
					_cheaterLabel.Visible = false;
				}
			}
			if (_isSyncTimerActive)
			{
				TimeSpan rem2 = _nextSyncTime - DateTime.Now;
				if (rem2.TotalSeconds <= 0.0)
				{
					_isSyncTimerActive = false;
					if (_casualMeasureBtn != null)
					{
						_casualMeasureBtn.Enabled = true;
					}
					if (_activeMeasureBtn != null)
					{
						_activeMeasureBtn.Enabled = true;
					}
					if (_casualSyncTimerLabel != null && _casualSyncTimerLabel.Visible)
					{
						_casualSyncTimerLabel.Text = strings.Ready;
						_casualSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.LimeGreen;
					}
					if (_activeSyncTimerLabel != null && _activeSyncTimerLabel.Visible)
					{
						_activeSyncTimerLabel.Text = "strings.Ready";
						_activeSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.LimeGreen;
					}
				}
				else
				{
					string timeText = $"{rem2.Minutes:D2}:{rem2.Seconds:D2}";
					if (_casualSyncTimerLabel != null && _casualSyncTimerLabel.Visible)
					{
						_casualSyncTimerLabel.Text = timeText;
						_casualSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.Yellow;
					}
					if (_activeSyncTimerLabel != null && _activeSyncTimerLabel.Visible)
					{
						_activeSyncTimerLabel.Text = timeText;
						_activeSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.Yellow;
					}
				}
			}
			if (_isTourneyWaitingRoom)
			{
				TimeSpan waitTime = _tourneyStartTimeUtc - DateTime.UtcNow;
				if (waitTime.TotalSeconds <= 0.0)
				{
					_isTourneyWaitingRoom = false;
					_isTournamentActive = true;
					if (_waitingRoomLabel != null)
					{
						_waitingRoomLabel.Visible = false;
					}
					if (_activeTimerLabel != null)
					{
						_activeTimerLabel.Visible = true;
					}
					StartTrackingMode();
					ScreenNotification.ShowNotification("strings.TourneyBegin", ScreenNotification.NotificationType.Warning);
				}
				else if (_waitingRoomLabel != null)
				{
					_waitingRoomLabel.Text = $"Starting in: {waitTime.Minutes:D2}:{waitTime.Seconds:D2}";
				}
			}
			if (_isTournamentActive && !_isTourneyWaitingRoom)
			{
				TimeSpan rem = _tourneyEndTimeUtc - DateTime.UtcNow;
				if (rem.TotalSeconds <= 0.0)
				{
					_isTournamentActive = false;
					if (_activeTimerLabel != null)
					{
						_activeTimerLabel.Visible = false;
					}
					_isSyncTimerActive = false;
					if (_activeSyncTimerLabel != null)
					{
						_activeSyncTimerLabel.Visible = false;
					}
					if (_tourneyModeUsed == "API")
					{
						ScreenNotification.ShowNotification(strings.TournamentOverWeighing, ScreenNotification.NotificationType.Warning, null, 10);
						CheckApiForNewCatches();
						_isTourneyWrapUpActive = true;
						_tourneyWrapUpEndTime = DateTime.Now.AddSeconds(30.0);
						_didMidWrapUpPing = false;
					}
					else
					{
						CompleteTournamentAsync(strings.TourneyEndedTitle, strings.TourneySubmitting);
					}
				}
				else if (_activeTimerLabel != null)
				{
					_activeTimerLabel.Text = $"{rem.Minutes:D2}:{rem.Seconds:D2}";
				}
			}
			if (_isTourneyWrapUpActive)
			{
				TimeSpan remWrap = _tourneyWrapUpEndTime - DateTime.Now;
				if (remWrap.TotalSeconds <= 15.0 && !_didMidWrapUpPing)
				{
					_didMidWrapUpPing = true;
					CheckApiForNewCatches();
				}
				if (remWrap.TotalSeconds <= 0.0)
				{
					_isTourneyWrapUpActive = false;
					CompleteTournamentAsync(strings.TourneyEndedTitle, strings.TourneySubmitting);
				}
			}
		}

		private async Task ForceUploadPB(bool isAuto = false)
		{
			_ = 1;
			try
			{
				List<KeyValuePair<int, PersonalBestRecord>> unsubmittedKvps = _personalBests.Where((KeyValuePair<int, PersonalBestRecord> kvp) => (kvp.Value.BestWeight != null && !kvp.Value.BestWeight.IsSubmitted && !kvp.Value.BestWeight.IsCheater) || (kvp.Value.BestLength != null && !kvp.Value.BestLength.IsSubmitted && !kvp.Value.BestLength.IsCheater)).ToList();
				if (!unsubmittedKvps.Any())
				{
					if (!isAuto)
					{
						ScreenNotification.ShowNotification(strings.NoNewSubmissions);
					}
					return;
				}
				if (!isAuto)
				{
					ScreenNotification.ShowNotification($"Pushing {unsubmittedKvps.Count} unsubmitted PBs to Leaderboard...");
				}
				string accountName = _localAccountName;
				List<object> catchesToSubmit = new List<object>();
				Dictionary<string, SubRecord> recordsByKey = new Dictionary<string, SubRecord>();
				foreach (KeyValuePair<int, PersonalBestRecord> kvp2 in unsubmittedKvps)
				{
					int itemId = kvp2.Key;
					PersonalBestRecord record = kvp2.Value;
					FishData fishInfo = _allFishEntries.FirstOrDefault((FishUIEntry f) => f.Data.ItemId == itemId)?.Data;
					if (fishInfo != null)
					{
						string englishWeightName = record.BestWeight?.EnglishFishName ?? fishInfo.Name;
						string englishLengthName = record.BestLength?.EnglishFishName ?? fishInfo.Name;
						if (record.BestWeight != null && !record.BestWeight.IsSubmitted && !record.BestWeight.IsCheater)
						{
							catchesToSubmit.Add(new
							{
								itemId = itemId,
								name = englishWeightName,
								weight = record.BestWeight.Weight,
								length = record.BestWeight.Length,
								characterName = record.BestWeight.CharacterName,
								accountName = accountName,
								isSuper = record.BestWeight.IsSuperPb,
								signature = record.BestWeight.Signature,
								type = "weight",
								location = fishInfo.Location
							});
							recordsByKey[$"{itemId}|weight"] = record.BestWeight;
						}
						if (record.BestLength != null && !record.BestLength.IsSubmitted && !record.BestLength.IsCheater)
						{
							catchesToSubmit.Add(new
							{
								itemId = itemId,
								name = englishLengthName,
								weight = record.BestLength.Weight,
								length = record.BestLength.Length,
								characterName = record.BestLength.CharacterName,
								accountName = accountName,
								isSuper = record.BestLength.IsSuperPb,
								signature = record.BestLength.Signature,
								type = "length",
								location = fishInfo.Location
							});
							recordsByKey[$"{itemId}|length"] = record.BestLength;
						}
					}
				}
				if (!catchesToSubmit.Any())
				{
					return;
				}
				StringContent content = new StringContent(JsonConvert.SerializeObject(new
				{
					catches = catchesToSubmit
				}), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await _httpClient.PostAsync("https://api.gilledwars.com/submit-leaderboard", (HttpContent)(object)content);
				if (response.get_IsSuccessStatusCode())
				{
					string respBody = await response.get_Content().ReadAsStringAsync();
					int markedCount = 0;
					try
					{
						SubmitResponse resp = JsonConvert.DeserializeObject<SubmitResponse>(respBody);
						if (resp?.Accepted != null && resp.Accepted.Count > 0)
						{
							foreach (AcceptedKey key in resp.Accepted)
							{
								string lookup = $"{key.ItemId}|{key.Type}";
								if (recordsByKey.TryGetValue(lookup, out var sub))
								{
									sub.IsSubmitted = true;
									markedCount++;
								}
							}
						}
						else if (resp != null && resp.Count == recordsByKey.Count)
						{
							foreach (SubRecord value in recordsByKey.Values)
							{
								value.IsSubmitted = true;
								markedCount++;
							}
						}
					}
					catch (Exception parseEx)
					{
						Logger.Warn(parseEx, "Failed to parse /submit-leaderboard response body.");
					}
					SavePersonalBests();
					if (markedCount == 0)
					{
						if (!isAuto)
						{
							ScreenNotification.ShowNotification(strings.ServerRejected, ScreenNotification.NotificationType.Error);
						}
					}
					else if (!isAuto)
					{
						ScreenNotification.ShowNotification(strings.SubmitCooldown.Replace("{0}", "5"), ScreenNotification.NotificationType.Warning);
					}
					else
					{
						ScreenNotification.ShowNotification(strings.AutoSyncSuccess);
					}
				}
				else if (!isAuto)
				{
					ScreenNotification.ShowNotification(strings.ServerRejected, ScreenNotification.NotificationType.Error);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to force upload PBs.");
				if (!isAuto)
				{
					ScreenNotification.ShowNotification("Failed to connect to leaderboards.", ScreenNotification.NotificationType.Error);
				}
			}
		}

		private void OnToggleHotkeyActivated(object sender, EventArgs e)
		{
			if (_mainWindow != null)
			{
				_mainWindow.Visible = !_mainWindow.Visible;
			}
			ScreenNotification.ShowNotification(strings.UiToggled);
		}

		private async Task LocalizeFishNamesAsync()
		{
			try
			{
				List<int> allItemIds = _allFishEntries.Select((FishUIEntry x) => x.Data.ItemId).Distinct().ToList();
				foreach (Item item in await Gw2ApiManager.Gw2ApiClient.V2.Items.ManyAsync(allItemIds))
				{
					FishUIEntry fishEntry = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == item.Id);
					if (fishEntry != null)
					{
						fishEntry.Data.Name = item.Name;
					}
				}
				RefreshFishLogUI();
				Logger.Info("strings.FishLocalizedSuccess");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "strings.FishLocalizedFail");
			}
		}

		protected override void Unload()
		{
			GameService.Input.Mouse.LeftMouseButtonReleased -= OnMouseLeftButtonReleased;
			if (ToggleHotkey != null)
			{
				ToggleHotkey.Value.Activated -= OnToggleHotkeyActivated;
			}
			StopDrfListener();
			_cheaterLabel?.Dispose();
			_cornerIcon?.Dispose();
			_mainWindow?.Dispose();
			_tourneyActivePanel?.Dispose();
			_casualCompactPanel?.Dispose();
			_currentSummaryWindow?.Dispose();
			_targetSelectionWindow?.Dispose();
			_timeOfDayPanel?.Dispose();
			_leaderboardWindow?.Dispose();
			_speciesSelectionWindow?.Dispose();
			_achievementResultsPanel?.Dispose();
			_achievementLegendPanel?.Dispose();
			_metaProgressWindow?.Dispose();
		}
	}
}
