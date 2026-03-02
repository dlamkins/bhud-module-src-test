using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp;
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

		private ClientWebSocket _drfSocket;

		private CancellationTokenSource _drfCts;

		private Task _drfReceiveTask;

		private static readonly Random _rnd = new Random();

		private bool _isCasualLoggingActive;

		private DateTime _lastSubmitTime = DateTime.MinValue;

		private string _localAccountName = "UnknownAccount";

		private Blish_HUD.Controls.Panel _leaderboardWindow;

		private Dropdown _lbSortDropdown;

		private Dropdown _lbSpeciesDropdown;

		private FlowPanel _lbListPanel;

		private bool _isDraggingLeaderboard;

		private Point _leaderboardDragOffset;

		private List<LeaderboardEntry> _cachedLeaderboardData;

		private DateTime _lastLeaderboardFetchTime = DateTime.MinValue;

		private Blish_HUD.Controls.Panel _speciesSelectionWindow;

		private StandardButton _speciesFilterBtn;

		private string _currentlySelectedSpecies = "All Species";

		private Blish_HUD.Controls.TextBox _speciesSearchBox;

		private static readonly string[] _junkMessages = new string[5] { "You caught... trash! The oceans are healing.", "A soggy boot! A true angler's prize.", "Just some literal garbage. Better luck next cast!", "You reeled in a tangled mess. Peak gameplay.", "Is it a legendary fish?! No... it's just debris." };

		private static readonly string[] _treasureMessages = new string[5] { "Woah, Shiny! You caught some actual treasure!", "A sunken chest! Hope it's not full of more boots.", "Treasure! You're gonna be rich... probably.", "You reeled in the jackpot! Nice catch!", "Move over, Blackbeard! Sunken loot acquired." };

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

		private FlowPanel _metaSubContainer;

		private Blish_HUD.Controls.Panel _currentlyExpandedRow;

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
		}

		private void OnMouseLeftButtonReleased(object sender, Blish_HUD.Input.MouseEventArgs e)
		{
			_isDragging = false;
			_isActivePanelDragging = false;
			_isDraggingSummary = false;
			_isCompactDragging = false;
			_isDraggingTarget = false;
			_isDraggingLeaderboard = false;
			_isDraggingAchievement = false;
			_isSpeciesSelectionDragging = false;
			_isMetaDragging = false;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			LoadFishDatabase();
			InitializeAccountAndLoadAsync();
			BuildMainWindow();
			BuildCasualCompactPanel();
			GameService.Input.Mouse.LeftMouseButtonReleased += OnMouseLeftButtonReleased;
			_cornerIcon = new CornerIcon
			{
				Icon = ContentsManager.GetTexture("images/603243.png"),
				BasicTooltipText = "Gilled Wars",
				Priority = 5
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
					ScreenNotification.ShowNotification("Expanding to Main View");
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
			RefreshFishLogUI();
			base.OnModuleLoaded(e);
		}

		private void ShowLeaderboardWindow()
		{
			if (_leaderboardWindow != null)
			{
				_leaderboardWindow.Visible = true;
				return;
			}
			_leaderboardWindow = new Blish_HUD.Controls.Panel
			{
				Title = "Global Leaderboards",
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(460, 600),
				Location = new Point(400, 150),
				ShowBorder = true,
				BackgroundColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 240),
				ZIndex = 1000
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Text = "Close";
			standardButton.Parent = _leaderboardWindow;
			standardButton.Location = new Point(340, 10);
			standardButton.Width = 90;
			standardButton.Click += delegate
			{
				_leaderboardWindow.Visible = false;
				if (_speciesSelectionWindow != null)
				{
					_speciesSelectionWindow.Visible = false;
				}
			};
			StandardButton refreshBtn = new StandardButton
			{
				Text = "Refresh",
				Parent = _leaderboardWindow,
				Location = new Point(340, 45),
				Width = 90,
				BasicTooltipText = "Force fetch latest leaderboard data (5-minute cooldown)."
			};
			refreshBtn.Click += async delegate
			{
				double elapsedMinutes = (DateTime.Now - _lastLeaderboardFetchTime).TotalMinutes;
				if (elapsedMinutes < 5.0 && _cachedLeaderboardData != null)
				{
					int remaining = 5 - (int)elapsedMinutes;
					ScreenNotification.ShowNotification($"Refresh is on cooldown! Wait {remaining}m.", ScreenNotification.NotificationType.Warning);
				}
				else
				{
					refreshBtn.Enabled = false;
					_cachedLeaderboardData = null;
					_lastLeaderboardFetchTime = DateTime.MinValue;
					await RefreshLeaderboardData();
					refreshBtn.Enabled = true;
					ScreenNotification.ShowNotification("Leaderboard Refreshed!");
				}
			};
			_leaderboardWindow.LeftMouseButtonPressed += delegate
			{
				if (GameService.Input.Mouse.ActiveControl == _leaderboardWindow)
				{
					_isDraggingLeaderboard = true;
					_leaderboardDragOffset = new Point(GameService.Input.Mouse.PositionRaw.X - _leaderboardWindow.Location.X, GameService.Input.Mouse.PositionRaw.Y - _leaderboardWindow.Location.Y);
				}
			};
			new Blish_HUD.Controls.Label
			{
				Text = "Sort:",
				Parent = _leaderboardWindow,
				Location = new Point(10, 15),
				AutoSizeWidth = true
			};
			_lbSortDropdown = new Dropdown
			{
				Parent = _leaderboardWindow,
				Location = new Point(50, 10),
				Width = 90
			};
			_lbSortDropdown.Items.Add("Weight");
			_lbSortDropdown.Items.Add("Length");
			_lbSortDropdown.SelectedItem = "Weight";
			_lbSortDropdown.ValueChanged += async delegate
			{
				await RefreshLeaderboardData();
			};
			new Blish_HUD.Controls.Label
			{
				Text = "Fish:",
				Parent = _leaderboardWindow,
				Location = new Point(150, 15),
				AutoSizeWidth = true
			};
			_speciesFilterBtn = new StandardButton
			{
				Text = "All Species",
				Parent = _leaderboardWindow,
				Location = new Point(190, 10),
				Width = 140,
				BasicTooltipText = "Click to select a specific fish species to filter."
			};
			_speciesFilterBtn.Click += delegate
			{
				ShowSpeciesPicker();
			};
			_lbListPanel = new FlowPanel
			{
				Parent = _leaderboardWindow,
				Location = new Point(10, 85),
				Size = new Point(440, 500),
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
				Text = "Loading data...",
				Parent = _lbListPanel,
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Yellow
			};
			try
			{
				string sortMode = _lbSortDropdown.SelectedItem.ToLower();
				string selectedSpecies = _currentlySelectedSpecies;
				if (_cachedLeaderboardData != null && !((DateTime.Now - _lastLeaderboardFetchTime).TotalMinutes >= 10.0))
				{
					goto IL_0205;
				}
				HttpResponseMessage response = await _httpClient.GetAsync("https://api.gilledwars.com/get-leaderboard");
				if (response.get_IsSuccessStatusCode())
				{
					_cachedLeaderboardData = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(await response.get_Content().ReadAsStringAsync());
					_lastLeaderboardFetchTime = DateTime.Now;
					goto IL_0205;
				}
				_lbListPanel.ClearChildren();
				new Blish_HUD.Controls.Label
				{
					Text = "Server Error: Waiting for website API...",
					Parent = _lbListPanel,
					TextColor = Microsoft.Xna.Framework.Color.Red,
					AutoSizeWidth = true
				};
				goto end_IL_006d;
				IL_0205:
				_lbListPanel.ClearChildren();
				_leaderboardWindow.Title = ((selectedSpecies == "All Species") ? ("Global Top 10 (" + sortMode.ToUpper() + ")") : ("Top 10 " + selectedSpecies));
				if (_cachedLeaderboardData == null || _cachedLeaderboardData.Count == 0)
				{
					new Blish_HUD.Controls.Label
					{
						Text = "No records found.",
						Parent = _lbListPanel,
						AutoSizeWidth = true
					};
					return;
				}
				IEnumerable<LeaderboardEntry> filteredRecords = _cachedLeaderboardData.Where((LeaderboardEntry r) => r.RecordType == sortMode);
				if (selectedSpecies != "All Species")
				{
					filteredRecords = filteredRecords.Where((LeaderboardEntry r) => r.FishName.Equals(selectedSpecies, StringComparison.OrdinalIgnoreCase));
				}
				List<LeaderboardEntry> top10List = filteredRecords.OrderByDescending((LeaderboardEntry r) => (!(sortMode == "weight")) ? r.Length : r.Weight).Take(10).ToList();
				if (top10List.Count == 0)
				{
					new Blish_HUD.Controls.Label
					{
						Text = "No catches logged for this species yet.",
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
					Text = "Rank",
					Parent = headerRow,
					Location = new Point(5, 5),
					Width = 45,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Blish_HUD.Controls.Label
				{
					Text = "Angler",
					Parent = headerRow,
					Location = new Point(65, 5),
					Width = 160,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Blish_HUD.Controls.Label
				{
					Text = "Species",
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
					Text = "Network Error!",
					Parent = _lbListPanel,
					TextColor = Microsoft.Xna.Framework.Color.Red,
					AutoSizeWidth = true
				};
			}
		}

		private async Task ShowMetaProgressWindow()
		{
			if (_metaProgressWindow == null)
			{
				_metaProgressWindow = new Blish_HUD.Controls.Panel
				{
					ShowBorder = true,
					Size = new Point(540, 550),
					Location = new Point(350, 150),
					Parent = GameService.Graphics.SpriteScreen,
					BackgroundColor = new Microsoft.Xna.Framework.Color(20, 20, 20, 240),
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
				Text = "Fetching all achievement data...",
				Parent = scroll,
				Font = GameService.Content.DefaultFont14,
				TextColor = Microsoft.Xna.Framework.Color.Yellow,
				AutoSizeWidth = true
			};
			try
			{
				List<int> metaIds = new List<int> { 6478, 6109, 6284, 6201, 6279, 6111 };
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
					BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.8f,
					Location = new Point(0, 0)
				};
				new Blish_HUD.Controls.Label
				{
					Text = "Meta Achievement Tracker",
					Parent = hBar,
					Location = new Point(10, 5),
					Font = GameService.Content.DefaultFont16,
					TextColor = Microsoft.Xna.Framework.Color.Gold,
					AutoSizeWidth = true
				};
				Blish_HUD.Controls.Label label = new Blish_HUD.Controls.Label();
				label.Text = "X";
				label.Parent = hBar;
				label.Location = new Point(hBar.Width - 25, 5);
				label.Font = GameService.Content.DefaultFont16;
				label.TextColor = Microsoft.Xna.Framework.Color.Red;
				label.AutoSizeWidth = true;
				label.Click += delegate
				{
					_metaProgressWindow.Visible = false;
				};
				hBar.LeftMouseButtonPressed += delegate
				{
					_isMetaDragging = true;
					_metaDragOffset = new Point(GameService.Input.Mouse.PositionRaw.X - _metaProgressWindow.Location.X, GameService.Input.Mouse.PositionRaw.Y - _metaProgressWindow.Location.Y);
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
					int current = ((mId != 6279) ? realCompletedCollections : (progress?.Current ?? 0));
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
						BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.4f,
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
						BackgroundColor = Microsoft.Xna.Framework.Color.DarkGray * 0.5f
					};
					new Blish_HUD.Controls.Panel
					{
						Parent = barBg,
						Size = new Point((int)(480f * ((float)current / (float)((max <= 0) ? 1 : max))), 15),
						BackgroundColor = (isDone ? Microsoft.Xna.Framework.Color.LimeGreen : Microsoft.Xna.Framework.Color.Cyan)
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
					Text = "Loading...",
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
				PlaceholderText = "Search species..."
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
				standardButton.Text = "All Species";
				standardButton.Parent = scroll;
				standardButton.Width = 230;
				standardButton.Click += async delegate
				{
					_currentlySelectedSpecies = "All Species";
					_speciesFilterBtn.Text = "All Species";
					_speciesSelectionWindow.Visible = false;
					await RefreshLeaderboardData();
				};
				foreach (string name in from n in _allFishEntries.Select((FishUIEntry x) => x.Data.Name).Distinct()
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

		private string GetLegacySeed1()
		{
			byte[] _mS = new byte[7] { 1, 60, 51, 51, 52, 59, 44 };
			byte[] s = new byte[_mS.Length];
			for (int i = 0; i < _mS.Length; i++)
			{
				s[i] = (byte)(_mS[i] ^ 0x55u);
			}
			return Encoding.UTF8.GetString(s) + "031388";
		}

		private string GetLegacySeed2()
		{
			byte[] _d1 = new byte[8] { 167, 226, 217, 200, 241, 180, 141, 126 };
			byte[] _d2 = new byte[5] { 47, 106, 83, 76, 53 };
			byte[] _k1 = new byte[4] { 85, 170, 51, 204 };
			byte[] b = new byte[_d1.Length + _d2.Length];
			for (int j = 0; j < _d1.Length; j++)
			{
				byte val = (byte)(_d1[j] ^ _k1[j % _k1.Length]);
				b[j] = (byte)((val << 3) | (val >> 5));
			}
			for (int i = 0; i < _d2.Length; i++)
			{
				b[i + _d1.Length] = (byte)(_d2[i] ^ 0x37u);
			}
			return Encoding.UTF8.GetString(b);
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

		private string GenerateSignature(double weight, double length, string name, bool isSuperPb, string salt)
		{
			string raw = $"{salt}|{weight:F2}|{length:F2}|{name}|{isSuperPb}";
			using SHA256 sha256 = SHA256.Create();
			return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(raw))).Substring(0, 12);
		}

		private string GenerateOldestSignature(double weight, double length, string name, string salt)
		{
			string raw = $"{salt}|{weight}|{length}|{name}";
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
				Logger.Info("[GilledWars] Storage directory verified: " + newDir);
			}
			catch (Exception ex3)
			{
				Logger.Error(ex3, "CRITICAL: Could not write to module storage. OneDrive or Permissions issue.");
				ScreenNotification.ShowNotification("Gilled Wars: Folder Access Error! Check your Documents permissions.", ScreenNotification.NotificationType.Error);
			}
			string[] obj = new string[2]
			{
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "addons", "blishhud", "gilledwarsanglers"),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "addons", "blishhud", "gilledwarsanglers_OLD")
			};
			bool didMigrate = false;
			string[] array = obj;
			foreach (string oldDir in array)
			{
				if (!Directory.Exists(oldDir))
				{
					continue;
				}
				Logger.Info("[GilledWars] Found old folder to clean: " + oldDir);
				string[] files = Directory.GetFiles(oldDir, "personal_bests*.json");
				foreach (string oldFile in files)
				{
					string dest = Path.Combine(newDir, Path.GetFileName(oldFile));
					try
					{
						if (System.IO.File.Exists(dest))
						{
							System.IO.File.Delete(dest);
						}
						System.IO.File.Move(oldFile, dest);
						Logger.Info("[GilledWars] Migrated: " + Path.GetFileName(oldFile));
						didMigrate = true;
					}
					catch (Exception ex)
					{
						Logger.Warn(ex, "Failed to migrate " + Path.GetFileName(oldFile));
					}
				}
				try
				{
					Directory.Delete(oldDir, recursive: true);
					Logger.Info("[GilledWars] ✅ Completely deleted old folder: " + oldDir);
				}
				catch (Exception ex2)
				{
					Logger.Warn(ex2, "Could not delete " + oldDir + " (files may be locked)");
				}
			}
			if (didMigrate)
			{
				ScreenNotification.ShowNotification("Gilled Wars: Files migrated & old folder cleaned up!");
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
			FetchTrueFishingAchievementsAsync();
			Logger.Info("[GilledWars] Module fully initialized for: " + _localAccountName);
		}

		private void LoadPersonalBests()
		{
			string fileName = ((_localAccountName == "UnknownAccount") ? "personal_bests.json" : ("personal_bests_" + _localAccountName + ".json"));
			string path = Path.Combine(ModuleDirectory, fileName);
			_isCheater = false;
			if (!System.IO.File.Exists(path))
			{
				return;
			}
			try
			{
				Dictionary<int, PersonalBestRecord>? obj = JsonConvert.DeserializeObject<Dictionary<int, PersonalBestRecord>>(System.IO.File.ReadAllText(path)) ?? new Dictionary<int, PersonalBestRecord>();
				_personalBests = new Dictionary<int, PersonalBestRecord>();
				string seed = GetGlobalSeed();
				foreach (KeyValuePair<int, PersonalBestRecord> kvp in obj!)
				{
					int itemId = kvp.Key;
					PersonalBestRecord rec = kvp.Value;
					ValidateRecord(rec.BestWeight);
					ValidateRecord(rec.BestLength);
					_personalBests[itemId] = rec;
					_caughtFishIds.Add(itemId);
					void ValidateRecord(SubRecord sub)
					{
						if (sub != null)
						{
							string cName = sub.CharacterName ?? "Unknown";
							string expected = GenerateSignature(sub.Weight, sub.Length, _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == itemId)?.Data.Name ?? "Unknown", sub.IsSuperPb, seed + cName + _localAccountName);
							if (sub.Signature != expected)
							{
								sub.IsCheater = (_isCheater = true);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to load personal bests");
			}
		}

		private void SavePersonalBests()
		{
			string fileName = ((_localAccountName == "UnknownAccount") ? "personal_bests.json" : ("personal_bests_" + _localAccountName + ".json"));
			string path = Path.Combine(ModuleDirectory, fileName);
			try
			{
				System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(_personalBests));
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to save personal bests");
			}
		}

		public async Task FetchTrueFishingAchievementsAsync()
		{
			_ = 1;
			try
			{
				int codAchievementId = 6112;
				Achievement obj = await Gw2ApiManager.Gw2ApiClient.V2.Achievements.GetAsync(codAchievementId);
				List<int> fishingAchievementIds = new List<int>();
				foreach (AchievementBit bit in obj.Bits!)
				{
					if (bit.Type.ToString()!.Contains("Achievement"))
					{
						PropertyInfo idProp = bit.GetType().GetProperty("Id");
						if (idProp != null)
						{
							int bitId = (int)idProp.GetValue(bit);
							fishingAchievementIds.Add(bitId);
						}
					}
				}
				fishingAchievementIds.Add(7114);
				fishingAchievementIds.Add(8168);
				fishingAchievementIds.Add(8554);
				fishingAchievementIds.Add(8900);
				foreach (Achievement collection in await Gw2ApiManager.Gw2ApiClient.V2.Achievements.ManyAsync(fishingAchievementIds))
				{
					Logger.Info("[GilledWars] Clean Fishing Collection Loaded: " + collection.Name);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to fetch verified fishing achievements.");
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
				ScreenNotification.ShowNotification("API Error: Could not read bags.", ScreenNotification.NotificationType.Error);
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
			ScreenNotification.ShowNotification($"Current Fish Measured! Scanned {currentInventory.Values.Sum()} items. Found {newCatches} new.");
		}

		private async Task TakeInventorySnapshot()
		{
			if (string.IsNullOrWhiteSpace(_customApiKey.Value))
			{
				ScreenNotification.ShowNotification("API Error: Paste Custom API Key in Module Settings!", ScreenNotification.NotificationType.Error);
				return;
			}
			Dictionary<int, int> inv = await GetActiveCharacterBags();
			if (inv != null)
			{
				_startInventory = inv;
				ScreenNotification.ShowNotification($"API: Snapshot saved! Tracking {inv.Values.Sum()} items in bags.");
			}
			else
			{
				ScreenNotification.ShowNotification("API Error: Invalid Key or Character Data!", ScreenNotification.NotificationType.Error);
			}
		}

		private async Task StartDrfListener()
		{
			if (string.IsNullOrWhiteSpace(_drfToken.Value))
			{
				ScreenNotification.ShowNotification("DRF Error: Token is missing!", ScreenNotification.NotificationType.Error);
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
				ScreenNotification.ShowNotification("DRF Connected! Tracking in real-time.", ScreenNotification.NotificationType.Warning);
				_drfReceiveTask = Task.Run((Func<Task>)ReceiveDrfMessages, _drfCts.Token);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to connect to DRF WebSocket.");
				ScreenNotification.ShowNotification("DRF Connection Failed! Check Token.", ScreenNotification.NotificationType.Error);
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
					_casualLogToggleBtn.Text = "Start Logging";
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

		private void ProcessCaughtFish(int itemId)
		{
			FishData matchingFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == itemId)?.Data;
			if (matchingFish == null)
			{
				return;
			}
			bool num = (matchingFish.Rarity != null && matchingFish.Rarity.Equals("Junk", StringComparison.OrdinalIgnoreCase)) || (matchingFish.Location != null && matchingFish.Location.Contains("Trash Collector"));
			bool isTreasure = (matchingFish.Name != null && (matchingFish.Name.Contains("Treasure") || matchingFish.Name.Contains("Chest"))) || (matchingFish.Location != null && matchingFish.Location.Contains("Treasure Collector"));
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
			string globalSig = GenerateSignature(weight, length, matchingFish.Name, isSuperPb, GetGlobalSeed() + charName + _localAccountName);
			string tSig = (_isTournamentActive ? GenerateSignature(weight, length, matchingFish.Name, isSuperPb, _tourneyRoomCode) : "");
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
			if (_recentCatches.Count > 5)
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
			string pbAlert = (isSuperPb ? " - SUPER PB!" : (catchRecord.IsNewPb ? " - NEW PB!" : ""));
			ScreenNotification.NotificationType notifType = (isSuperPb ? ScreenNotification.NotificationType.Warning : ScreenNotification.NotificationType.Info);
			if (_isTournamentActive && !_isTourneyWaitingRoom)
			{
				if (_tourneyTargetItemId == 0 || _tourneyTargetItemId == itemId)
				{
					_tourneyCatches.Add(catchRecord);
					UpdateActiveTourneyCoolerUI();
					ScreenNotification.ShowNotification($"Tourney Catch: {catchRecord.Name} ({catchRecord.Weight} lbs, {catchRecord.Length} in){pbAlert}", notifType);
				}
				else
				{
					ScreenNotification.ShowNotification($"Caught: {catchRecord.Name} ({catchRecord.Weight} lbs, {catchRecord.Length} in){pbAlert}", notifType);
				}
			}
			else
			{
				ScreenNotification.ShowNotification($"Caught: {catchRecord.Name} ({catchRecord.Weight} lbs, {catchRecord.Length} in){pbAlert}", notifType);
			}
		}

		private void BuildMainWindow()
		{
			_mainWindow = new Blish_HUD.Controls.Panel
			{
				ShowBorder = true,
				Size = new Point(620, 580),
				Location = new Point(300, 300),
				Parent = GameService.Graphics.SpriteScreen,
				Visible = false,
				BackgroundColor = new Microsoft.Xna.Framework.Color(30, 30, 30, 230),
				ClipsBounds = false
			};
			Blish_HUD.Controls.Panel headerBar = new Blish_HUD.Controls.Panel
			{
				Parent = _mainWindow,
				Size = new Point(_mainWindow.Width, 30),
				Location = new Point(0, 0),
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.8f
			};
			Blish_HUD.Controls.Label titleLabel = new Blish_HUD.Controls.Label
			{
				Text = "Gilled Wars (visit www.GilledWars.com)",
				Parent = headerBar,
				Location = new Point(10, 5),
				Font = GameService.Content.DefaultFont16,
				TextColor = Microsoft.Xna.Framework.Color.Gold,
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
				BasicTooltipText = "Close Gilled Wars"
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
					_dragOffset = new Point(GameService.Input.Mouse.PositionRaw.X - _mainWindow.Location.X, GameService.Input.Mouse.PositionRaw.Y - _mainWindow.Location.Y);
				}
			};
			StandardButton casualBtn = new StandardButton
			{
				Text = "Casual Fishing",
				Parent = _mainWindow,
				Location = new Point(10, 40),
				Width = 150
			};
			StandardButton obj = new StandardButton
			{
				Text = "Tournament Mode",
				Parent = _mainWindow,
				Location = new Point(170, 40),
				Width = 150
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Text = "Website";
			standardButton.Parent = _mainWindow;
			standardButton.Location = new Point(330, 40);
			standardButton.Width = 100;
			standardButton.BasicTooltipText = "Opens gilledwars.com in your web browser.";
			standardButton.Click += delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://www.gilledwars.com",
					UseShellExecute = true
				});
			};
			StandardButton standardButton2 = new StandardButton();
			standardButton2.Text = "In-Game Top 10";
			standardButton2.Parent = _mainWindow;
			standardButton2.Location = new Point(440, 40);
			standardButton2.Width = 150;
			standardButton2.BasicTooltipText = "View the live top 10 without leaving the game!";
			standardButton2.Click += delegate
			{
				ScreenNotification.ShowNotification("Loading Top 10...");
				ShowLeaderboardWindow();
			};
			_casualPanel = new Blish_HUD.Controls.Panel
			{
				Parent = _mainWindow,
				Size = new Point(600, 490),
				Location = new Point(10, 80),
				Visible = true
			};
			_tournamentPanel = new Blish_HUD.Controls.Panel
			{
				Parent = _mainWindow,
				Size = new Point(600, 490),
				Location = new Point(10, 80),
				Visible = false
			};
			_fishLogPanel = new Blish_HUD.Controls.Panel
			{
				Parent = _casualPanel,
				Location = new Point(0, 65),
				Size = new Point(_casualPanel.Width, _casualPanel.Height - 65),
				Visible = false
			};
			_achievementPanel = new Blish_HUD.Controls.Panel
			{
				Parent = _casualPanel,
				Location = new Point(0, 65),
				Size = new Point(_casualPanel.Width, _casualPanel.Height - 65),
				Visible = false
			};
			casualBtn.Click += delegate
			{
				_casualPanel.Visible = true;
				_tournamentPanel.Visible = false;
			};
			obj.Click += delegate
			{
				_casualPanel.Visible = false;
				_tournamentPanel.Visible = true;
			};
			BuildFishLogGrid(_fishLogPanel);
			BuildCasualUI(_casualPanel);
			BuildTournamentUI(_tournamentPanel);
			BuildActiveTournamentWidget();
		}

		private void BuildFishLogGrid(Blish_HUD.Controls.Panel parent)
		{
			if (parent == null)
			{
				return;
			}
			Blish_HUD.Controls.Panel filterPanel = new Blish_HUD.Controls.Panel
			{
				Parent = parent,
				Location = new Point(10, 0),
				Size = new Point(580, 110)
			};
			Blish_HUD.Controls.TextBox searchBar = new Blish_HUD.Controls.TextBox
			{
				Parent = filterPanel,
				Location = new Point(0, 0),
				Width = 115,
				PlaceholderText = "Search..."
			};
			Dropdown rarityDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(120, 0),
				Width = 125
			};
			Dropdown locationDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(250, 0),
				Width = 150
			};
			Dropdown holeDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(405, 0),
				Width = 165
			};
			Dropdown timeDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(0, 35),
				Width = 115
			};
			Dropdown baitDrop = new Dropdown
			{
				Parent = filterPanel,
				Location = new Point(120, 35),
				Width = 125
			};
			StandardButton collapseBtn = new StandardButton
			{
				Text = "Collapse",
				Parent = filterPanel,
				Location = new Point(250, 35),
				Width = 75
			};
			StandardButton revealBtn = new StandardButton
			{
				Text = "Reveal",
				Parent = filterPanel,
				Location = new Point(330, 35),
				Width = 70
			};
			StandardButton obj = new StandardButton
			{
				Text = "Reset Filters",
				Parent = filterPanel,
				Location = new Point(0, 70),
				Width = 100
			};
			StandardButton pushLeaderboardBtn = new StandardButton
			{
				Text = "Push PBs to Leaderboard",
				Parent = filterPanel,
				Location = new Point(120, 70),
				Width = 200,
				BasicTooltipText = "Submit your valid DRF-tracked catches to the global leaderboards!"
			};
			StandardButton zoneAnalyzerBtn = new StandardButton
			{
				Text = "Zone Analyzer",
				Parent = filterPanel,
				Location = new Point(330, 70),
				Width = 110,
				BasicTooltipText = "Analyze current map for missing achievement fish via API!"
			};
			StandardButton metaProgressBtn = new StandardButton
			{
				Text = "Meta Progress",
				Parent = filterPanel,
				Location = new Point(450, 70),
				Width = 120,
				BasicTooltipText = "Track your progress towards Cod Swimming and other big titles!"
			};
			FlowPanel scroll = new FlowPanel
			{
				Parent = parent,
				Location = new Point(10, 135),
				Size = new Point(580, parent.Height - 140),
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
					double rem = 5.0 - (DateTime.Now - _lastSubmitTime).TotalMinutes;
					ScreenNotification.ShowNotification($"Please wait {rem:F1} minutes before pushing again.", ScreenNotification.NotificationType.Error);
				}
				else
				{
					pushLeaderboardBtn.Enabled = false;
					pushLeaderboardBtn.Text = "Uploading...";
					await ForceUploadPB();
					_lastSubmitTime = DateTime.Now;
					pushLeaderboardBtn.Text = "Push PBs to Leaderboard";
					pushLeaderboardBtn.Enabled = true;
				}
			};
			Map mapInfo;
			zoneAnalyzerBtn.Click += async delegate
			{
				zoneAnalyzerBtn.Enabled = false;
				zoneAnalyzerBtn.Text = "Scanning...";
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
							{ "Crystal Desert", 6317 },
							{ "Elona", 6106 },
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
							{ "Mistburned Barrens", 8554 }
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
							ScreenNotification.ShowNotification("No fishing achievement found for this area.", ScreenNotification.NotificationType.Error);
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Zone Analyzer failed.");
					ScreenNotification.ShowNotification("Zone Analyzer failed.", ScreenNotification.NotificationType.Error);
				}
				finally
				{
					zoneAnalyzerBtn.Enabled = true;
					zoneAnalyzerBtn.Text = "Zone Analyzer";
				}
			};
			metaProgressBtn.Click += async delegate
			{
				metaProgressBtn.Enabled = false;
				metaProgressBtn.Text = "Loading...";
				await ShowMetaProgressWindow();
				metaProgressBtn.Text = "Meta Progress";
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
			foreach (string i in _allFishEntries.Select((FishUIEntry x) => x.Data.Location).Distinct())
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
				where x.Location != "Any"
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
					Width = 550,
					HeightSizingMode = SizingMode.AutoSize,
					FlowDirection = ControlFlowDirection.LeftToRight
				};
				_categoryPanels.Add(p);
				foreach (FishData fish in group)
				{
					string safeName = fish.Name.Replace(" ", "_").Replace("'", "").Replace("-", "");
					bool num = _caughtFishIds.Contains(fish.ItemId);
					string pbWText = "NONE LOGGED";
					string pbLText = "NONE LOGGED";
					Microsoft.Xna.Framework.Color tintColor = (num ? Microsoft.Xna.Framework.Color.White : (Microsoft.Xna.Framework.Color.Gray * 0.5f));
					if (_personalBests.TryGetValue(fish.ItemId, out var rec))
					{
						if (rec.BestWeight != null)
						{
							pbWText = (rec.BestWeight.IsCheater ? "CHEATER DETECTED" : $"{rec.BestWeight.Weight} lbs");
						}
						if (rec.BestLength != null)
						{
							pbLText = (rec.BestLength.IsCheater ? "CHEATER DETECTED" : $"{rec.BestLength.Length} in");
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
								text2 = $"{text} my PB weight for this guy is: {num2} lbs and my PB for length is: {num3} in";
							}
						}
						Clipboard.SetText(text2);
						ScreenNotification.ShowNotification("Copied " + fish.Name + " code!");
					};
					FishUIEntry fishUIEntry = _allFishEntries.First((FishUIEntry x) => x.Data.ItemId == fish.ItemId);
					fishUIEntry.Icon = img;
					fishUIEntry.CategoryPanel = p;
				}
			}
			new Blish_HUD.Controls.Panel
			{
				Parent = scroll,
				Width = 550,
				Height = 60
			};
			void ApplyFilters()
			{
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
						if (rarityDrop.SelectedItem != "All Rarities" && entry2.Data.Rarity != rarityDrop.SelectedItem)
						{
							match = false;
						}
						if (locationDrop.SelectedItem != "All Locations" && entry2.Data.Location != locationDrop.SelectedItem)
						{
							match = false;
						}
						if (holeDrop.SelectedItem != "All Holes" && !entry2.Data.FishingHole.Contains(holeDrop.SelectedItem))
						{
							match = false;
						}
						if (timeDrop.SelectedItem != "All Times" && !entry2.Data.Time.Contains((timeDrop.SelectedItem == "Any") ? "Any" : timeDrop.SelectedItem))
						{
							match = false;
						}
						if (baitDrop.SelectedItem != "All Baits" && !entry2.Data.Bait.Contains(baitDrop.SelectedItem))
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
				}
			}
		}

		private async Task ShowAchievementResultsPanel(string locationName, int achievementId, int subCurrent, int subMax, string subDescription)
		{
			if (_achievementResultsPanel == null)
			{
				_achievementResultsPanel = new Blish_HUD.Controls.Panel
				{
					Title = locationName + " Progress",
					Parent = GameService.Graphics.SpriteScreen,
					Size = new Point(800, 600),
					Location = new Point(400, 100),
					ShowBorder = true,
					BackgroundColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 240),
					ZIndex = 1001
				};
				_achievementResultsPanel.LeftMouseButtonPressed += delegate
				{
					if (GameService.Input.Mouse.ActiveControl == _achievementResultsPanel)
					{
						_isDraggingAchievement = true;
						_achievementDragOffset = new Point(GameService.Input.Mouse.PositionRaw.X - _achievementResultsPanel.Location.X, GameService.Input.Mouse.PositionRaw.Y - _achievementResultsPanel.Location.Y);
					}
				};
			}
			else
			{
				_achievementResultsPanel.Title = locationName + " Progress";
			}
			if (_achievementLegendPanel == null)
			{
				_achievementLegendPanel = new Blish_HUD.Controls.Panel
				{
					Parent = GameService.Graphics.SpriteScreen,
					Size = new Point(800, 60),
					ShowBorder = true,
					BackgroundColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 240),
					ZIndex = 1002
				};
			}
			_achievementResultsPanel.Visible = true;
			_achievementLegendPanel.Visible = true;
			_achievementResultsPanel.ClearChildren();
			_achievementLegendPanel.ClearChildren();
			_achievementLegendPanel.Location = new Point(_achievementResultsPanel.Location.X, _achievementResultsPanel.Location.Y + _achievementResultsPanel.Height + 2);
			int lx = 5;
			string[] array = new string[7] { "Legendary", "Ascended", "Exotic", "Rare", "Masterwork", "Fine", "Basic" };
			foreach (string rName in array)
			{
				Microsoft.Xna.Framework.Color rColor = GetRarityColor(rName);
				new Blish_HUD.Controls.Label
				{
					Text = "■",
					Parent = _achievementLegendPanel,
					Location = new Point(lx, 10),
					TextColor = rColor,
					Font = GameService.Content.DefaultFont14,
					AutoSizeWidth = true
				};
				new Blish_HUD.Controls.Label
				{
					Text = rName,
					Parent = _achievementLegendPanel,
					Location = new Point(lx + 15, 10),
					TextColor = rColor,
					Font = GameService.Content.DefaultFont12,
					AutoSizeWidth = true
				};
				lx += rName.Length * 7 + 20;
			}
			StandardButton standardButton = new StandardButton();
			standardButton.Text = "Close";
			standardButton.Parent = _achievementResultsPanel;
			standardButton.Location = new Point(690, 10);
			standardButton.Width = 90;
			standardButton.Click += delegate
			{
				_achievementResultsPanel.Visible = false;
				_achievementLegendPanel.Visible = false;
			};
			Blish_HUD.Controls.Panel progressHeader = new Blish_HUD.Controls.Panel
			{
				Parent = _achievementResultsPanel,
				Location = new Point(10, 45),
				Size = new Point(780, 35)
			};
			FlowPanel listContainer = new FlowPanel
			{
				Parent = _achievementResultsPanel,
				Location = new Point(10, 115),
				Size = new Point(780, 470),
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(0f, 2f)
			};
			try
			{
				Achievement achievementDef = await Gw2ApiManager.Gw2ApiClient.V2.Achievements.GetAsync(achievementId);
				IReadOnlyList<int> completedBits = (await Gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync()).FirstOrDefault((AccountAchievement a) => a.Id == achievementId)?.Bits ?? new List<int>();
				int totalBits = achievementDef.Bits?.Count ?? 0;
				int missingCount = totalBits - completedBits.Count;
				new Blish_HUD.Controls.Label
				{
					Text = locationName.ToUpper() + ":",
					Parent = progressHeader,
					Location = new Point(0, 5),
					Font = GameService.Content.DefaultFont18,
					AutoSizeWidth = true,
					TextColor = Microsoft.Xna.Framework.Color.Gold
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
					return;
				}
				Blish_HUD.Controls.Panel columnHeader = new Blish_HUD.Controls.Panel
				{
					Parent = _achievementResultsPanel,
					Location = new Point(10, 85),
					Size = new Point(780, 30),
					BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.3f
				};
				new Blish_HUD.Controls.Label
				{
					Text = "NAME",
					Parent = columnHeader,
					Location = new Point(60, 5),
					Width = 170,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Blish_HUD.Controls.Label
				{
					Text = "BAIT",
					Parent = columnHeader,
					Location = new Point(240, 5),
					Width = 130,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Blish_HUD.Controls.Label
				{
					Text = "TIME",
					Parent = columnHeader,
					Location = new Point(380, 5),
					Width = 150,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Blish_HUD.Controls.Label
				{
					Text = "HOLE",
					Parent = columnHeader,
					Location = new Point(540, 5),
					Width = 230,
					TextColor = Microsoft.Xna.Framework.Color.Cyan,
					Font = GameService.Content.DefaultFont16
				};
				new Image
				{
					Texture = ContentService.Textures.Pixel,
					Parent = listContainer,
					Width = 760,
					Height = 2,
					Tint = Microsoft.Xna.Framework.Color.Gray * 0.5f
				};
				if (achievementDef.Bits != null)
				{
					for (int i = 0; i < achievementDef.Bits!.Count; i++)
					{
						if (completedBits.Contains(i))
						{
							continue;
						}
						AchievementBit bit = achievementDef.Bits![i];
						PropertyInfo idProp = bit.GetType().GetProperty("Id");
						if (!(idProp == null))
						{
							int fishItemId = (int)idProp.GetValue(bit);
							FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == fishItemId)?.Data;
							Blish_HUD.Controls.Panel row = new Blish_HUD.Controls.Panel
							{
								Parent = listContainer,
								Width = 760,
								Height = 55,
								BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.2f,
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
								new Image
								{
									Texture = ContentService.Textures.Pixel,
									Parent = row,
									Location = new Point(55, 5),
									Width = 1,
									Height = 45,
									Tint = Microsoft.Xna.Framework.Color.White * 0.1f
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
								new Image
								{
									Texture = ContentService.Textures.Pixel,
									Parent = row,
									Location = new Point(235, 5),
									Width = 1,
									Height = 45,
									Tint = Microsoft.Xna.Framework.Color.White * 0.1f
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
								new Image
								{
									Texture = ContentService.Textures.Pixel,
									Parent = row,
									Location = new Point(375, 5),
									Width = 1,
									Height = 45,
									Tint = Microsoft.Xna.Framework.Color.White * 0.1f
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
								new Image
								{
									Texture = ContentService.Textures.Pixel,
									Parent = row,
									Location = new Point(535, 5),
									Width = 1,
									Height = 45,
									Tint = Microsoft.Xna.Framework.Color.White * 0.1f
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
							else
							{
								new Blish_HUD.Controls.Label
								{
									Text = "?",
									Parent = row,
									Location = new Point(20, 15),
									Font = GameService.Content.DefaultFont18,
									TextColor = Microsoft.Xna.Framework.Color.Red
								};
								new Blish_HUD.Controls.Label
								{
									Text = $"API ID: {fishItemId} (MISSING FROM JSON)",
									Parent = row,
									Location = new Point(60, 17),
									Font = GameService.Content.DefaultFont14,
									TextColor = Microsoft.Xna.Framework.Color.White,
									AutoSizeWidth = true
								};
							}
							new Image
							{
								Texture = ContentService.Textures.Pixel,
								Parent = listContainer,
								Width = 760,
								Height = 1,
								Tint = Microsoft.Xna.Framework.Color.White * 0.15f
							};
						}
					}
				}
				new Blish_HUD.Controls.Panel
				{
					Parent = listContainer,
					Width = 760,
					Height = 60
				};
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

		private async Task ShowAchievementAnalysis(string locationName, int achievementId)
		{
			_fishLogPanel.Visible = false;
			_achievementPanel.Visible = true;
			_achievementPanel.ClearChildren();
			Blish_HUD.Controls.Panel headerPanel = new Blish_HUD.Controls.Panel
			{
				Parent = _achievementPanel,
				Size = new Point(580, 50),
				Location = new Point(10, 0),
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.5f
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Parent = headerPanel;
			standardButton.Text = "<- BACK";
			standardButton.Location = new Point(5, 10);
			standardButton.Width = 75;
			standardButton.Click += delegate
			{
				_achievementPanel.Visible = false;
				_fishLogPanel.Visible = true;
			};
			new Blish_HUD.Controls.Label
			{
				Text = locationName.ToUpper() + " ACHIEVEMENT PROGRESS",
				Parent = headerPanel,
				Location = new Point(90, 12),
				Font = GameService.Content.DefaultFont18,
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Gold
			};
			Blish_HUD.Controls.Panel columnHeader = new Blish_HUD.Controls.Panel
			{
				Parent = _achievementPanel,
				Location = new Point(10, 55),
				Size = new Point(580, 30),
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.3f
			};
			new Blish_HUD.Controls.Label
			{
				Text = "ICON",
				Parent = columnHeader,
				Location = new Point(10, 5),
				Font = GameService.Content.DefaultFont12,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			new Blish_HUD.Controls.Label
			{
				Text = "FISH NAME",
				Parent = columnHeader,
				Location = new Point(65, 5),
				Font = GameService.Content.DefaultFont12,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			new Blish_HUD.Controls.Label
			{
				Text = "BAIT",
				Parent = columnHeader,
				Location = new Point(230, 5),
				Font = GameService.Content.DefaultFont12,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			new Blish_HUD.Controls.Label
			{
				Text = "TIME",
				Parent = columnHeader,
				Location = new Point(370, 5),
				Font = GameService.Content.DefaultFont12,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			new Blish_HUD.Controls.Label
			{
				Text = "LOCATION",
				Parent = columnHeader,
				Location = new Point(480, 5),
				Font = GameService.Content.DefaultFont12,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			FlowPanel list = new FlowPanel
			{
				Parent = _achievementPanel,
				Location = new Point(10, 90),
				Size = new Point(580, 400),
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(0f, 2f)
			};
			try
			{
				Achievement achievementDef = await Gw2ApiManager.Gw2ApiClient.V2.Achievements.GetAsync(achievementId);
				IReadOnlyList<int> bits = (await Gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync()).FirstOrDefault((AccountAchievement a) => a.Id == achievementId)?.Bits ?? new List<int>();
				for (int i = 0; i < achievementDef.Bits!.Count; i++)
				{
					if (bits.Contains(i))
					{
						continue;
					}
					AchievementBit achievementBit = achievementDef.Bits![i];
					AchievementItemBit itemBit = achievementBit as AchievementItemBit;
					if (itemBit != null)
					{
						FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == itemBit.Id)?.Data;
						if (dbFish != null)
						{
							Blish_HUD.Controls.Panel row = new Blish_HUD.Controls.Panel
							{
								Parent = list,
								Width = 560,
								Height = 45,
								BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.2f,
								ShowBorder = true
							};
							new Image
							{
								Parent = row,
								Size = new Point(36, 36),
								Location = new Point(5, 4),
								Texture = ContentsManager.GetTexture("images/" + dbFish.Name.Replace(" ", "_") + ".png")
							};
							new Blish_HUD.Controls.Label
							{
								Text = dbFish.Name,
								Parent = row,
								Location = new Point(65, 12),
								AutoSizeWidth = true,
								TextColor = GetRarityColor(dbFish.Rarity),
								Font = GameService.Content.DefaultFont14
							};
							new Blish_HUD.Controls.Label
							{
								Text = dbFish.Bait,
								Parent = row,
								Location = new Point(230, 12),
								AutoSizeWidth = true,
								TextColor = Microsoft.Xna.Framework.Color.Cyan
							};
							new Blish_HUD.Controls.Label
							{
								Text = dbFish.Time,
								Parent = row,
								Location = new Point(370, 12),
								AutoSizeWidth = true,
								TextColor = Microsoft.Xna.Framework.Color.Yellow
							};
							new Blish_HUD.Controls.Label
							{
								Text = dbFish.Location,
								Parent = row,
								Location = new Point(480, 12),
								AutoSizeWidth = true,
								TextColor = Microsoft.Xna.Framework.Color.LightGray,
								Font = GameService.Content.DefaultFont12
							};
						}
					}
				}
				if (list.Children.Count == 0)
				{
					new Blish_HUD.Controls.Label
					{
						Text = "✔ Area Fully Logged!",
						Parent = list,
						AutoSizeWidth = true,
						TextColor = Microsoft.Xna.Framework.Color.LimeGreen,
						Padding = new Thickness(10f, 50f, 0f, 0f)
					};
				}
			}
			catch
			{
				new Blish_HUD.Controls.Label
				{
					Text = "API Link Failed - Verify Key Permissions",
					Parent = list,
					AutoSizeWidth = true,
					TextColor = Microsoft.Xna.Framework.Color.Red
				};
			}
		}

		private void BuildCasualUI(Blish_HUD.Controls.Panel parent)
		{
			Image chestIcon = new Image
			{
				Parent = parent,
				Location = new Point(10, 0),
				Size = new Point(48, 48),
				BasicTooltipText = "View Fish Cooler",
				Texture = ContentsManager.GetTexture("images/603243.png")
			};
			StandardButton obj = new StandardButton
			{
				Text = "Fish Log",
				Parent = parent,
				Location = new Point(70, 10),
				Width = 100
			};
			_casualLogToggleBtn = new StandardButton
			{
				Text = "Start Logging",
				Parent = parent,
				Location = new Point(180, 10),
				Width = 120
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Text = "Compact";
			standardButton.Parent = parent;
			standardButton.Location = new Point(530, 10);
			standardButton.Width = 80;
			standardButton.Click += delegate
			{
				if (_isCasualLoggingActive)
				{
					_mainWindow.Visible = false;
					_casualCompactPanel.Visible = true;
					UpdateCompactCooler();
				}
			};
			_useDrfCheckbox = new Checkbox
			{
				Text = "Use DRF (Real-Time)",
				Parent = parent,
				Location = new Point(180, 45),
				BasicTooltipText = "Requires drf.rs Addon installed and Token in settings."
			};
			_casualMeasureBtn = new StandardButton
			{
				Text = "Measure Fish",
				Parent = parent,
				Location = new Point(310, 10),
				Width = 120,
				Enabled = false
			};
			_casualSyncTimerLabel = new Blish_HUD.Controls.Label
			{
				Text = "05:00",
				Parent = parent,
				Location = new Point(440, 15),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Yellow,
				Visible = false
			};
			_casualLogToggleBtn.Click += async delegate
			{
				if (_isCasualLoggingActive)
				{
					StopCasualLogging();
					_mainWindow.Size = new Point(620, 580);
					_mainWindow.Visible = true;
					ScreenNotification.ShowNotification("Casual Logging Stopped.");
				}
				else
				{
					_isCasualLoggingActive = true;
					_casualLogToggleBtn.Text = "Stop Logging";
					_useDrfCheckbox.Enabled = false;
					if (!_useDrfCheckbox.Checked)
					{
						_casualMeasureBtn.Enabled = false;
						await TakeInventorySnapshot();
						_nextSyncTime = DateTime.Now.AddMinutes(5.0);
						_isSyncTimerActive = true;
						_casualSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.Yellow;
						_casualSyncTimerLabel.Visible = true;
					}
					else
					{
						_casualMeasureBtn.Enabled = false;
						_casualSyncTimerLabel.Text = "DRF Active";
						_casualSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.LimeGreen;
						_casualSyncTimerLabel.Visible = true;
						StartDrfListener();
					}
					_mainWindow.Visible = false;
					_casualCompactPanel.Visible = true;
					UpdateCompactCooler();
				}
			};
			_casualMeasureBtn.Click += async delegate
			{
				_casualMeasureBtn.Enabled = false;
				_casualSyncTimerLabel.Text = "Measuring...";
				await CheckApiForNewCatches();
				_nextSyncTime = DateTime.Now.AddMinutes(5.0);
				_isSyncTimerActive = true;
			};
			_recentCatchesPanel = new Blish_HUD.Controls.Panel
			{
				Parent = parent,
				Location = new Point(0, 65),
				Size = new Point(parent.Width, parent.Height - 65),
				Visible = true
			};
			chestIcon.Click += delegate
			{
				_recentCatchesPanel.Visible = true;
				_fishLogPanel.Visible = false;
				UpdateRecentCatchesUI();
			};
			obj.Click += delegate
			{
				_recentCatchesPanel.Visible = false;
				_fishLogPanel.Visible = true;
			};
			BuildRecentCatchesUI(_recentCatchesPanel);
		}

		private void BuildRecentCatchesUI(Blish_HUD.Controls.Panel parent)
		{
			new Blish_HUD.Controls.Label
			{
				Text = "Fish Cooler (Last 5 Catches)",
				Parent = parent,
				Location = new Point(10, 0),
				AutoSizeWidth = true,
				Font = GameService.Content.DefaultFont18,
				TextColor = Microsoft.Xna.Framework.Color.Cyan
			};
		}

		private void UpdateRecentCatchesUI()
		{
			if (_recentCatchesPanel == null)
			{
				return;
			}
			_recentCatchesPanel.ClearChildren();
			BuildRecentCatchesUI(_recentCatchesPanel);
			int y = 40;
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
					Parent = _recentCatchesPanel,
					Location = new Point(20, y),
					AutoSizeWidth = true,
					TextColor = catchColor,
					Font = GameService.Content.DefaultFont18
				};
				y += 30;
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
				string pbWText = "NONE LOGGED";
				string pbLText = "NONE LOGGED";
				Microsoft.Xna.Framework.Color tint = (num ? Microsoft.Xna.Framework.Color.White : (Microsoft.Xna.Framework.Color.Gray * 0.5f));
				if (_personalBests.TryGetValue(fish.ItemId, out var rec))
				{
					if (rec.BestWeight != null)
					{
						if (rec.BestWeight.IsCheater)
						{
							pbWText = "CHEATER DETECTED";
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
							pbLText = "CHEATER DETECTED";
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
				string tooltip = fish.Name + "\nRarity: " + fish.Rarity + "\nLocation: " + fish.Location + "\nHole: " + fish.FishingHole + "\nTime: " + fish.Time + "\nBait: " + fish.Bait;
				if (!isCollector)
				{
					tooltip = tooltip + "\n\nPB Weight: " + pbWText + "\nPB Length: " + pbLText;
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
					_targetDragOffset = new Point(GameService.Input.Mouse.PositionRaw.X - _targetSelectionWindow.Location.X, GameService.Input.Mouse.PositionRaw.Y - _targetSelectionWindow.Location.Y);
				}
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Text = "Close";
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
			standardButton2.Text = "All Species";
			standardButton2.Parent = scroll;
			standardButton2.Width = 350;
			standardButton2.Click += delegate
			{
				_tourneyTargetItemId = 0;
				targetBtn.Text = "Target: All Species";
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
						targetBtn.Text = "Target: " + fish.Data.Name;
						_targetSelectionWindow.Dispose();
					};
				}
			}
		}

		private void BuildTournamentUI(Blish_HUD.Controls.Panel parent)
		{
			StandardButton hostModeBtn = new StandardButton
			{
				Text = "Host",
				Parent = parent,
				Location = new Point(10, 0),
				Width = 100
			};
			StandardButton obj = new StandardButton
			{
				Text = "Participant",
				Parent = parent,
				Location = new Point(115, 0),
				Width = 100
			};
			_tourneyHostPanel = new Blish_HUD.Controls.Panel
			{
				Parent = parent,
				Location = new Point(0, 40),
				Size = new Point(parent.Width, parent.Height - 40),
				Visible = true
			};
			_tourneyParticipantPanel = new Blish_HUD.Controls.Panel
			{
				Parent = parent,
				Location = new Point(0, 40),
				Size = new Point(parent.Width, parent.Height - 40),
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
			new Blish_HUD.Controls.Label
			{
				Text = "Host Tournament Setup",
				Parent = _tourneyHostPanel,
				Location = new Point(10, 0),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Yellow
			};
			new Blish_HUD.Controls.Label
			{
				Text = "Start Delay",
				Parent = _tourneyHostPanel,
				Location = new Point(10, 25),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray,
				BasicTooltipText = "How long until the tournament starts for everyone."
			};
			new Blish_HUD.Controls.Label
			{
				Text = "Mins",
				Parent = _tourneyHostPanel,
				Location = new Point(160, 25),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			new Blish_HUD.Controls.Label
			{
				Text = "Tracking Mode",
				Parent = _tourneyHostPanel,
				Location = new Point(230, 25),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			Dropdown hostStartDelayDrop = new Dropdown
			{
				Parent = _tourneyHostPanel,
				Location = new Point(10, 45),
				Width = 130
			};
			hostStartDelayDrop.Items.Add("Start Immediately");
			hostStartDelayDrop.Items.Add("2 Minutes");
			hostStartDelayDrop.Items.Add("5 Minutes");
			hostStartDelayDrop.Items.Add("10 Minutes");
			Blish_HUD.Controls.TextBox hostTimerMin = new Blish_HUD.Controls.TextBox
			{
				Parent = _tourneyHostPanel,
				Location = new Point(160, 45),
				Width = 60,
				Text = "30"
			};
			Dropdown hostTrackingModeDrop = new Dropdown
			{
				Parent = _tourneyHostPanel,
				Location = new Point(230, 45),
				Width = 150
			};
			hostTrackingModeDrop.Items.Add("API (5-Min Wait)");
			hostTrackingModeDrop.Items.Add("DRF (Real-Time)");
			hostTrackingModeDrop.SelectedItem = "DRF (Real-Time)";
			new Blish_HUD.Controls.Label
			{
				Text = "Target Species",
				Parent = _tourneyHostPanel,
				Location = new Point(10, 80),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			new Blish_HUD.Controls.Label
			{
				Text = "Win Factor",
				Parent = _tourneyHostPanel,
				Location = new Point(230, 80),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray
			};
			StandardButton targetSpeciesBtn = new StandardButton
			{
				Text = "Target: All Species",
				Parent = _tourneyHostPanel,
				Location = new Point(10, 100),
				Width = 200,
				BasicTooltipText = "Click to select a specific fish for the tournament."
			};
			targetSpeciesBtn.Click += delegate
			{
				ShowTargetSelectionWindow(targetSpeciesBtn);
			};
			_hostWinFactorDrop = new Dropdown
			{
				Parent = _tourneyHostPanel,
				Location = new Point(230, 100),
				Width = 120
			};
			_hostWinFactorDrop.Items.Add("Weight");
			_hostWinFactorDrop.Items.Add("Length");
			StandardButton genKeyBtn = new StandardButton
			{
				Text = "Create Room",
				Parent = _tourneyHostPanel,
				Location = new Point(10, 150),
				Width = 150
			};
			genKeyBtn.Click += async delegate
			{
				genKeyBtn.Enabled = false;
				genKeyBtn.Text = "Creating...";
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
				var payload = new
				{
					hostName = charName,
					startDelayMins = delayMins,
					durationMins = int.Parse(hostTimerMin.Text),
					mode = mode,
					targetId = _tourneyTargetItemId,
					winFactor = _hostWinFactorDrop.SelectedItem,
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
							ScreenNotification.ShowNotification("Room " + code + " created & copied!");
						}
					}
					else
					{
						ScreenNotification.ShowNotification("API Error: Could not create room.", ScreenNotification.NotificationType.Error);
					}
				}
				catch (Exception ex2)
				{
					Logger.Error(ex2, "Failed to connect to API.");
					ScreenNotification.ShowNotification("Network Error: Could not connect to API.", ScreenNotification.NotificationType.Error);
				}
				genKeyBtn.Enabled = true;
				genKeyBtn.Text = "Create Room";
			};
			Blish_HUD.Controls.TextBox verifyInput = new Blish_HUD.Controls.TextBox
			{
				Parent = _tourneyHostPanel,
				Location = new Point(10, 195),
				Width = 300,
				PlaceholderText = "Paste Member End Code Here..."
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Text = "Manual Verify";
			standardButton.Parent = _tourneyHostPanel;
			standardButton.Location = new Point(10, 230);
			standardButton.Width = 150;
			standardButton.BasicTooltipText = "Backup tool in case API submission fails.";
			standardButton.Click += delegate
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
			new Blish_HUD.Controls.Label
			{
				Parent = _tourneyHostPanel,
				Location = new Point(10, 280),
				Width = 550,
				Height = 200,
				WrapText = true,
				TextColor = Microsoft.Xna.Framework.Color.LightGray,
				Text = "TRACKING MODES EXPLAINED:\n\nAPI (5-Min Wait): Uses official GW2 servers. Highly secure, no extra downloads needed. Catch detection delayed by ArenaNet's cache.\n\nDRF (Real-Time): Uses the drf.rs memory reader. Instant catch detection! Participants MUST install the 3rd-party DRF .dll to participate."
			};
			new Blish_HUD.Controls.Label
			{
				Text = "Participant Join",
				Parent = _tourneyParticipantPanel,
				Location = new Point(10, 0),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Cyan
			};
			Blish_HUD.Controls.TextBox partSessionKey = new Blish_HUD.Controls.TextBox
			{
				Parent = _tourneyParticipantPanel,
				Location = new Point(10, 30),
				Width = 150,
				PlaceholderText = "Code (e.g. GW-1234)"
			};
			StandardButton joinBtn = new StandardButton
			{
				Text = "Join Room",
				Parent = _tourneyParticipantPanel,
				Location = new Point(10, 70),
				Width = 150
			};
			joinBtn.Click += async delegate
			{
				joinBtn.Enabled = false;
				joinBtn.Text = "Joining...";
				try
				{
					string roomCode = partSessionKey.Text.Trim().ToUpper();
					if (!roomCode.StartsWith("GW-") || roomCode.Length != 8)
					{
						ScreenNotification.ShowNotification("Invalid Code Format! Use GW-XXXXX", ScreenNotification.NotificationType.Error);
						joinBtn.Enabled = true;
						joinBtn.Text = "Join Room";
						return;
					}
					HttpResponseMessage response = await _httpClient.GetAsync("https://api.gilledwars.com/join/" + roomCode);
					string resultString = await response.get_Content().ReadAsStringAsync();
					if (response.get_IsSuccessStatusCode())
					{
						Dictionary<string, object> tData = JsonConvert.DeserializeObject<Dictionary<string, object>>(resultString);
						StopCasualLogging();
						_tourneyRoomCode = roomCode;
						_tourneyModeUsed = tData["mode"].ToString();
						_tourneyTargetItemId = Convert.ToInt32(tData["targetId"]);
						_tourneyWinFactor = tData["winFactor"].ToString();
						int durMins = Convert.ToInt32(tData["durationMins"]);
						long startTimeMs = Convert.ToInt64(tData["startTime"]);
						_tourneyStartTimeUtc = DateTimeOffset.FromUnixTimeMilliseconds(startTimeMs).UtcDateTime;
						_tourneyEndTimeUtc = _tourneyStartTimeUtc.AddMinutes(durMins);
						_tourneyCatches.Clear();
						string targetName = "All Species";
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
							ScreenNotification.ShowNotification("Entered Waiting Room...");
						}
						else
						{
							_isTourneyWaitingRoom = false;
							_isTournamentActive = true;
							_waitingRoomLabel.Visible = false;
							_activeTimerLabel.Visible = true;
							StartTrackingMode();
							ScreenNotification.ShowNotification("Tournament Fishing Started!");
						}
						_mainWindow.Visible = false;
						_tourneyActivePanel.Visible = true;
					}
					else
					{
						ScreenNotification.ShowNotification("Room Not Found or Expired!", ScreenNotification.NotificationType.Error);
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "API Join Failed");
					ScreenNotification.ShowNotification("Network Error!", ScreenNotification.NotificationType.Error);
				}
				joinBtn.Enabled = true;
				joinBtn.Text = "Join Room";
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
				Size = new Point(320, 280),
				Location = new Point(400, 300),
				ShowBorder = true,
				BackgroundColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 200),
				Visible = false
			};
			_tourneyActivePanel.LeftMouseButtonPressed += delegate
			{
				if (GameService.Input.Mouse.ActiveControl == _tourneyActivePanel)
				{
					_isActivePanelDragging = true;
					_activePanelDragOffset = new Point(GameService.Input.Mouse.PositionRaw.X - _tourneyActivePanel.Location.X, GameService.Input.Mouse.PositionRaw.Y - _tourneyActivePanel.Location.Y);
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
				Text = "Starting in...",
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
				Location = new Point(100, 20),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Yellow,
				Visible = false
			};
			_activeMeasureBtn = new StandardButton
			{
				Text = "Measure Fish",
				Parent = _tourneyActivePanel,
				Location = new Point(180, 10),
				Width = 110,
				Enabled = false
			};
			_activeEndBtn = new StandardButton
			{
				Text = "End & Submit",
				Parent = _tourneyActivePanel,
				Location = new Point(10, 50),
				Width = 120
			};
			_activeRecopyBtn = new StandardButton
			{
				Text = "Re-Copy Code",
				Parent = _tourneyActivePanel,
				Location = new Point(10, 50),
				Width = 120,
				Visible = false,
				BasicTooltipText = "Manual backup."
			};
			_activeExitBtn = new StandardButton
			{
				Text = "Exit Tourney",
				Parent = _tourneyActivePanel,
				Location = new Point(140, 50),
				Width = 120,
				Visible = false,
				BasicTooltipText = "Close and return to UI."
			};
			new Blish_HUD.Controls.Label
			{
				Text = "Top 5 Catches:",
				Parent = _tourneyActivePanel,
				Location = new Point(10, 85),
				AutoSizeWidth = true,
				TextColor = Microsoft.Xna.Framework.Color.Cyan
			};
			_activeCoolerList = new FlowPanel
			{
				Parent = _tourneyActivePanel,
				Location = new Point(10, 110),
				Size = new Point(280, 150),
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			_activeMeasureBtn.Click += async delegate
			{
				_activeMeasureBtn.Enabled = false;
				_activeSyncTimerLabel.Text = "Measuring...";
				await CheckApiForNewCatches();
				_nextSyncTime = DateTime.Now.AddMinutes(5.0);
				_isSyncTimerActive = true;
			};
			_activeEndBtn.Click += async delegate
			{
				await CompleteTournamentAsync("Tournament Ended Manually", "Results submitted to server.");
			};
			_activeRecopyBtn.Click += delegate
			{
				if (!string.IsNullOrEmpty(_lastGeneratedCode))
				{
					CopyToClipboard(_lastGeneratedCode);
					ScreenNotification.ShowNotification("Backup Code Re-copied!");
				}
			};
			_activeExitBtn.Click += delegate
			{
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
			};
		}

		private void BuildCasualCompactPanel()
		{
			_casualCompactPanel = new Blish_HUD.Controls.Panel
			{
				Title = "Casual Fishing",
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(340, 240),
				Location = new Point(400, 300),
				ShowBorder = true,
				BackgroundColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 200),
				Visible = false
			};
			_casualCompactPanel.LeftMouseButtonPressed += delegate
			{
				if (GameService.Input.Mouse.ActiveControl == _casualCompactPanel)
				{
					_isCompactDragging = true;
					_compactDragOffset = new Point(GameService.Input.Mouse.PositionRaw.X - _casualCompactPanel.Location.X, GameService.Input.Mouse.PositionRaw.Y - _casualCompactPanel.Location.Y);
				}
			};
			new Blish_HUD.Controls.Label
			{
				Text = "Fish Cooler (Last 5 Catches)",
				Parent = _casualCompactPanel,
				Location = new Point(10, 10),
				AutoSizeWidth = true,
				Font = GameService.Content.DefaultFont18,
				TextColor = Microsoft.Xna.Framework.Color.Cyan
			};
			_compactCoolerList = new FlowPanel
			{
				Parent = _casualCompactPanel,
				Location = new Point(10, 40),
				Size = new Point(310, 120),
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			_compactFishLogBtn = new StandardButton
			{
				Text = "Fish Log",
				Parent = _casualCompactPanel,
				Location = new Point(10, 170),
				Width = 100
			};
			_compactMaxBtn = new StandardButton
			{
				Text = "Max Size",
				Parent = _casualCompactPanel,
				Location = new Point(120, 170),
				Width = 100
			};
			_compactFishLogBtn.Click += delegate
			{
				_casualCompactPanel.Visible = false;
				_mainWindow.Visible = true;
				_recentCatchesPanel.Visible = false;
				_fishLogPanel.Visible = true;
			};
			_compactMaxBtn.Click += delegate
			{
				_casualCompactPanel.Visible = false;
				_mainWindow.Visible = true;
				_recentCatchesPanel.Visible = true;
				_fishLogPanel.Visible = false;
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
			_isTourneyWrapUpActive = false;
			if (_activeMeasureBtn != null)
			{
				_activeMeasureBtn.Enabled = false;
			}
			_isSyncTimerActive = false;
			StopDrfListener();
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
					msg = "Results successfully auto-posted to host's Discord!";
					ScreenNotification.ShowNotification("Results Submitted to Server!");
				}
				else
				{
					msg = "API submission failed. Please copy backup code manually.";
					CopyToClipboard(_lastGeneratedCode);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to submit results.");
				msg = "Network Error. Backup code copied to clipboard.";
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
				string statText = ((_tourneyWinFactor == "Length") ? $"{c.Length} in | {c.Weight} lbs" : $"{c.Weight} lbs | {c.Length} in");
				new Blish_HUD.Controls.Label
				{
					Text = c.Name + " - " + statText,
					Parent = _activeCoolerList,
					AutoSizeWidth = true,
					TextColor = catchColor
				};
			}
		}

		private void ShowTournamentSummary(string title, string errorMsg, Microsoft.Xna.Framework.Color ColorTheme, List<TournamentCatch> catches = null, string winFactor = "Weight")
		{
			if (_currentSummaryWindow != null)
			{
				_currentSummaryWindow.Dispose();
			}
			_currentSummaryWindow = new Blish_HUD.Controls.Panel
			{
				Title = title,
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(450, 450),
				Location = new Point(500, 300),
				ShowBorder = true,
				BackgroundColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 230)
			};
			_currentSummaryWindow.LeftMouseButtonPressed += delegate
			{
				if (GameService.Input.Mouse.ActiveControl == _currentSummaryWindow)
				{
					_isDraggingSummary = true;
					_summaryDragOffset = new Point(GameService.Input.Mouse.PositionRaw.X - _currentSummaryWindow.Location.X, GameService.Input.Mouse.PositionRaw.Y - _currentSummaryWindow.Location.Y);
				}
			};
			FlowPanel list = new FlowPanel
			{
				Parent = _currentSummaryWindow,
				Location = new Point(20, 40),
				Size = new Point(410, 300),
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom
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
					TextColor = ColorTheme
				};
				foreach (TournamentCatch c in ((winFactor == "Length") ? catches.OrderByDescending((TournamentCatch x) => x.Length) : catches.OrderByDescending((TournamentCatch x) => x.Weight)).Take(5))
				{
					string statText = ((winFactor == "Length") ? $"{c.Length} in | {c.Weight} lbs" : $"{c.Weight} lbs | {c.Length} in");
					Blish_HUD.Controls.Label label = new Blish_HUD.Controls.Label();
					label.Text = "[" + c.Rarity + "] " + c.Name + " - " + statText;
					label.Parent = list;
					label.AutoSizeWidth = true;
					label.TextColor = Microsoft.Xna.Framework.Color.White;
				}
			}
			StandardButton standardButton = new StandardButton();
			standardButton.Text = "Close";
			standardButton.Parent = _currentSummaryWindow;
			standardButton.Location = new Point(150, 360);
			standardButton.Width = 150;
			standardButton.Click += delegate
			{
				_currentSummaryWindow.Dispose();
			};
		}

		protected override void Update(GameTime gt)
		{
			if (_isDragging && _mainWindow != null)
			{
				_mainWindow.Location = new Point(GameService.Input.Mouse.PositionRaw.X - _dragOffset.X, GameService.Input.Mouse.PositionRaw.Y - _dragOffset.Y);
			}
			if (_isActivePanelDragging && _tourneyActivePanel != null)
			{
				_tourneyActivePanel.Location = new Point(GameService.Input.Mouse.PositionRaw.X - _activePanelDragOffset.X, GameService.Input.Mouse.PositionRaw.Y - _activePanelDragOffset.Y);
			}
			if (_isDraggingSummary && _currentSummaryWindow != null)
			{
				_currentSummaryWindow.Location = new Point(GameService.Input.Mouse.PositionRaw.X - _summaryDragOffset.X, GameService.Input.Mouse.PositionRaw.Y - _summaryDragOffset.Y);
			}
			if (_isCompactDragging && _casualCompactPanel != null)
			{
				_casualCompactPanel.Location = new Point(GameService.Input.Mouse.PositionRaw.X - _compactDragOffset.X, GameService.Input.Mouse.PositionRaw.Y - _compactDragOffset.Y);
			}
			if (_isDraggingTarget && _targetSelectionWindow != null)
			{
				_targetSelectionWindow.Location = new Point(GameService.Input.Mouse.PositionRaw.X - _targetDragOffset.X, GameService.Input.Mouse.PositionRaw.Y - _targetDragOffset.Y);
			}
			if (_isDraggingLeaderboard && _leaderboardWindow != null)
			{
				_leaderboardWindow.Location = new Point(GameService.Input.Mouse.PositionRaw.X - _leaderboardDragOffset.X, GameService.Input.Mouse.PositionRaw.Y - _leaderboardDragOffset.Y);
			}
			if (_isSpeciesSelectionDragging && _speciesSelectionWindow != null)
			{
				_speciesSelectionWindow.Location = new Point(GameService.Input.Mouse.PositionRaw.X - _speciesSelectionDragOffset.X, GameService.Input.Mouse.PositionRaw.Y - _speciesSelectionDragOffset.Y);
			}
			if (_isDraggingAchievement && _achievementResultsPanel != null)
			{
				_achievementResultsPanel.Location = new Point(GameService.Input.Mouse.PositionRaw.X - _achievementDragOffset.X, GameService.Input.Mouse.PositionRaw.Y - _achievementDragOffset.Y);
				if (_achievementLegendPanel != null)
				{
					_achievementLegendPanel.Location = new Point(_achievementResultsPanel.Location.X, _achievementResultsPanel.Location.Y + _achievementResultsPanel.Height + 2);
				}
			}
			if (_isMetaDragging && _metaProgressWindow != null)
			{
				_metaProgressWindow.Location = new Point(GameService.Input.Mouse.PositionRaw.X - _metaDragOffset.X, GameService.Input.Mouse.PositionRaw.Y - _metaDragOffset.Y);
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
						_casualSyncTimerLabel.Text = "Ready!";
						_casualSyncTimerLabel.TextColor = Microsoft.Xna.Framework.Color.LimeGreen;
					}
					if (_activeSyncTimerLabel != null && _activeSyncTimerLabel.Visible)
					{
						_activeSyncTimerLabel.Text = "Ready!";
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
					ScreenNotification.ShowNotification("Tournament Has Begun!", ScreenNotification.NotificationType.Warning);
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
						ScreenNotification.ShowNotification("Tournament over! Weighing all your fish, please wait...", ScreenNotification.NotificationType.Warning, null, 10);
						CheckApiForNewCatches();
						_isTourneyWrapUpActive = true;
						_tourneyWrapUpEndTime = DateTime.Now.AddSeconds(30.0);
						_didMidWrapUpPing = false;
					}
					else
					{
						CompleteTournamentAsync("Tournament Ended", "Submitting your catches to the server...");
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
					CompleteTournamentAsync("Tournament Ended", "Submitting your catches to the server...");
				}
			}
		}

		private async Task ForceUploadPB()
		{
			_ = 1;
			try
			{
				List<KeyValuePair<int, PersonalBestRecord>> unsubmittedKvps = _personalBests.Where((KeyValuePair<int, PersonalBestRecord> kvp) => (kvp.Value.BestWeight != null && !kvp.Value.BestWeight.IsSubmitted) || (kvp.Value.BestLength != null && !kvp.Value.BestLength.IsSubmitted)).ToList();
				if (!unsubmittedKvps.Any())
				{
					ScreenNotification.ShowNotification("No new submissions found.");
					return;
				}
				ScreenNotification.ShowNotification($"Pushing {unsubmittedKvps.Count} unsubmitted PBs to Leaderboard...");
				foreach (KeyValuePair<int, PersonalBestRecord> kvp2 in unsubmittedKvps)
				{
					int itemId = kvp2.Key;
					PersonalBestRecord record = kvp2.Value;
					FishData fishInfo = _allFishEntries.FirstOrDefault((FishUIEntry f) => f.Data.ItemId == itemId)?.Data;
					if (fishInfo == null)
					{
						continue;
					}
					string safeName = fishInfo.Name.Replace(" ", "_").Replace("'", "").Replace("-", "");
					string accountName = GameService.Gw2Mumble.PlayerCharacter.Name;
					if (string.IsNullOrEmpty(accountName))
					{
						accountName = "Unknown";
					}
					HttpClient client2;
					if (record.BestWeight != null && !record.BestWeight.IsSubmitted && !record.BestWeight.IsCheater)
					{
						StringContent content2 = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
						{
							player_name = accountName,
							fish_name = safeName,
							weight = record.BestWeight.Weight,
							length = 0.0,
							catch_time = DateTime.UtcNow.ToString("o")
						}), Encoding.UTF8, "application/json");
						client2 = new HttpClient();
						try
						{
							if ((await client2.PostAsync("https://gilledwars.com/api/submit_catch", (HttpContent)(object)content2)).get_IsSuccessStatusCode())
							{
								record.BestWeight.IsSubmitted = true;
							}
						}
						finally
						{
							((IDisposable)client2)?.Dispose();
						}
					}
					if (record.BestLength == null || record.BestLength.IsSubmitted || record.BestLength.IsCheater)
					{
						continue;
					}
					StringContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
					{
						player_name = accountName,
						fish_name = safeName,
						weight = 0.0,
						length = record.BestLength.Length,
						catch_time = DateTime.UtcNow.ToString("o")
					}), Encoding.UTF8, "application/json");
					client2 = new HttpClient();
					try
					{
						if ((await client2.PostAsync("https://gilledwars.com/api/submit_catch", (HttpContent)(object)content)).get_IsSuccessStatusCode())
						{
							record.BestLength.IsSubmitted = true;
						}
					}
					finally
					{
						((IDisposable)client2)?.Dispose();
					}
				}
				SavePersonalBests();
				ScreenNotification.ShowNotification("Fish submitted to leaderboards, Good Luck!", ScreenNotification.NotificationType.Warning);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to force upload PBs.");
				ScreenNotification.ShowNotification("Failed to connect to leaderboards.", ScreenNotification.NotificationType.Error);
			}
		}

		private void OnToggleHotkeyActivated(object sender, EventArgs e)
		{
			if (_mainWindow != null)
			{
				_mainWindow.Visible = !_mainWindow.Visible;
			}
			ScreenNotification.ShowNotification("Gilled Wars UI Toggled!");
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
		}
	}
}
