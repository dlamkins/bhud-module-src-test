using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gorthax.GilledWarsAnglers
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
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			LoadFishDatabase();
			LoadPersonalBests();
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
				if (_mainWindow != null)
				{
					_mainWindow.Visible = !_mainWindow.Visible;
				}
			};
			ToggleHotkey.Value.Enabled = true;
			ToggleHotkey.Value.Activated += delegate
			{
				if (_mainWindow != null)
				{
					_mainWindow.Visible = !_mainWindow.Visible;
				}
				ScreenNotification.ShowNotification("Gilled Wars UI Toggled!");
			};
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

		private void LoadPersonalBests()
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "addons", "blishhud", "gilledwarsanglers");
			Directory.CreateDirectory(text);
			string path = Path.Combine(text, "personal_bests.json");
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
					FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == itemId)?.Data;
					string fishName = ((dbFish != null) ? dbFish.Name : "Unknown");
					if (rec.BestWeight != null)
					{
						string cNameW = rec.BestWeight.CharacterName ?? "Unknown";
						string expectedSig2 = GenerateSignature(rec.BestWeight.Weight, rec.BestWeight.Length, fishName, rec.BestWeight.IsSuperPb, seed + cNameW);
						if (rec.BestWeight.Signature != expectedSig2)
						{
							rec.BestWeight.IsCheater = true;
						}
						if (rec.BestWeight.IsCheater)
						{
							_isCheater = true;
						}
					}
					if (rec.BestLength != null)
					{
						string cNameL = rec.BestLength.CharacterName ?? "Unknown";
						string expectedSig = GenerateSignature(rec.BestLength.Weight, rec.BestLength.Length, fishName, rec.BestLength.IsSuperPb, seed + cNameL);
						if (rec.BestLength.Signature != expectedSig)
						{
							rec.BestLength.IsCheater = true;
						}
						if (rec.BestLength.IsCheater)
						{
							_isCheater = true;
						}
					}
					_personalBests.Add(itemId, rec);
					_caughtFishIds.Add(itemId);
				}
				RefreshFishLogUI();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to load personal bests.");
			}
		}

		private void SavePersonalBests()
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "addons", "blishhud", "gilledwarsanglers");
			Directory.CreateDirectory(text);
			System.IO.File.WriteAllText(Path.Combine(text, "personal_bests.json"), JsonConvert.SerializeObject(_personalBests));
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
			string saltedSeed = GetGlobalSeed() + charName;
			string globalSig = GenerateSignature(weight, length, matchingFish.Name, isSuperPb, saltedSeed);
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
			}
			RefreshFishLogUI();
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
				Title = "Gilled Wars Anglers",
				ShowBorder = true,
				Size = new Point(620, 580),
				Location = new Point(300, 300),
				Parent = GameService.Graphics.SpriteScreen,
				Visible = false,
				BackgroundColor = new Microsoft.Xna.Framework.Color(30, 30, 30, 180)
			};
			_mainWindow.LeftMouseButtonPressed += delegate
			{
				if (GameService.Input.Mouse.ActiveControl == _mainWindow)
				{
					_isDragging = true;
					_dragOffset = new Point(GameService.Input.Mouse.PositionRaw.X - _mainWindow.Location.X, GameService.Input.Mouse.PositionRaw.Y - _mainWindow.Location.Y);
				}
			};
			StandardButton casualBtn = new StandardButton
			{
				Text = "Casual Fishing",
				Parent = _mainWindow,
				Location = new Point(10, 10),
				Width = 150
			};
			StandardButton obj = new StandardButton
			{
				Text = "Tournament Mode",
				Parent = _mainWindow,
				Location = new Point(170, 10),
				Width = 150
			};
			_casualPanel = new Blish_HUD.Controls.Panel
			{
				Parent = _mainWindow,
				Size = new Point(600, 500),
				Location = new Point(10, 50),
				Visible = true
			};
			_tournamentPanel = new Blish_HUD.Controls.Panel
			{
				Parent = _mainWindow,
				Size = new Point(600, 500),
				Location = new Point(10, 50),
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
			_fishLogPanel = new Blish_HUD.Controls.Panel
			{
				Parent = _casualPanel,
				Location = new Point(0, 65),
				Size = new Point(_casualPanel.Width, _casualPanel.Height - 65),
				Visible = false
			};
			BuildFishLogGrid(_fishLogPanel);
			BuildCasualUI(_casualPanel);
			BuildTournamentUI(_tournamentPanel);
			BuildActiveTournamentWidget();
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
				Width = 100,
				BasicTooltipText = "Clear all active filters."
			};
			StandardButton pushLeaderboardBtn = new StandardButton
			{
				Text = "Push PBs to Leaderboard",
				Parent = filterPanel,
				Location = new Point(120, 70),
				Width = 200,
				BasicTooltipText = "Submit your valid DRF-tracked catches to the global leaderboards!"
			};
			pushLeaderboardBtn.Click += async delegate
			{
				if (_isCheater)
				{
					ScreenNotification.ShowNotification("Submission Rejected: Tampered Data Detected", ScreenNotification.NotificationType.Error);
				}
				else if ((DateTime.Now - _lastSubmitTime).TotalMinutes < 5.0)
				{
					ScreenNotification.ShowNotification("Button on Cool down. Please wait a few minutes.", ScreenNotification.NotificationType.Warning);
				}
				else
				{
					pushLeaderboardBtn.Enabled = false;
					pushLeaderboardBtn.Text = "Pushing...";
					List<object> eligibleCatches = new List<object>();
					_ = GameService.Gw2Mumble.PlayerCharacter.Name;
					List<SubRecord> submittedWeights = new List<SubRecord>();
					List<SubRecord> submittedLengths = new List<SubRecord>();
					foreach (KeyValuePair<int, PersonalBestRecord> kvp in _personalBests)
					{
						int fId = kvp.Key;
						PersonalBestRecord rec2 = kvp.Value;
						FishData dbFish = _allFishEntries.FirstOrDefault((FishUIEntry x) => x.Data.ItemId == fId)?.Data;
						string fName = ((dbFish != null) ? dbFish.Name : "Unknown");
						string fLoc = ((dbFish != null) ? dbFish.Location : "Unknown");
						if (rec2.BestWeight != null && rec2.BestWeight.CaughtWithDrf && !rec2.BestWeight.IsCheater && !rec2.BestWeight.IsSubmitted)
						{
							string cName2 = rec2.BestWeight.CharacterName ?? "Unknown";
							eligibleCatches.Add(new
							{
								itemId = fId,
								name = fName,
								weight = rec2.BestWeight.Weight,
								length = rec2.BestWeight.Length,
								signature = rec2.BestWeight.Signature,
								isSuper = rec2.BestWeight.IsSuperPb,
								type = "weight",
								characterName = cName2,
								location = fLoc
							});
							submittedWeights.Add(rec2.BestWeight);
						}
						if (rec2.BestLength != null && rec2.BestLength.CaughtWithDrf && !rec2.BestLength.IsCheater && !rec2.BestLength.IsSubmitted)
						{
							string cName = rec2.BestLength.CharacterName ?? "Unknown";
							eligibleCatches.Add(new
							{
								itemId = fId,
								name = fName,
								weight = rec2.BestLength.Weight,
								length = rec2.BestLength.Length,
								signature = rec2.BestLength.Signature,
								isSuper = rec2.BestLength.IsSuperPb,
								type = "length",
								characterName = cName,
								location = fLoc
							});
							submittedLengths.Add(rec2.BestLength);
						}
					}
					if (eligibleCatches.Count == 0)
					{
						ScreenNotification.ShowNotification("No new PB recorded.", ScreenNotification.NotificationType.Warning);
						pushLeaderboardBtn.Enabled = true;
						pushLeaderboardBtn.Text = "Push PBs to Leaderboard";
					}
					else
					{
						_lastSubmitTime = DateTime.Now;
						var payload = new
						{
							catches = eligibleCatches
						};
						try
						{
							StringContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
							if ((await _httpClient.PostAsync("https://api.gilledwars.com/submit-leaderboard", (HttpContent)(object)content)).get_IsSuccessStatusCode())
							{
								foreach (SubRecord item in submittedWeights)
								{
									item.IsSubmitted = true;
								}
								foreach (SubRecord item2 in submittedLengths)
								{
									item2.IsSubmitted = true;
								}
								SavePersonalBests();
								ScreenNotification.ShowNotification("PB submitted to the Leaderboards, good luck!");
							}
							else
							{
								ScreenNotification.ShowNotification("Server Error: Leaderboard update failed.", ScreenNotification.NotificationType.Error);
							}
						}
						catch (Exception ex2)
						{
							Logger.Error(ex2, "Failed to submit to leaderboards.");
							ScreenNotification.ShowNotification("Network Error!", ScreenNotification.NotificationType.Error);
						}
						pushLeaderboardBtn.Enabled = true;
						pushLeaderboardBtn.Text = "Push PBs to Leaderboard";
					}
				}
			};
			FlowPanel scroll = new FlowPanel
			{
				Parent = parent,
				Location = new Point(10, 115),
				Size = new Point(580, parent.Height - 120),
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
			try
			{
				HashSet<string> uniqueHoles = new HashSet<string>();
				foreach (FishUIEntry allFishEntry in _allFishEntries)
				{
					FishData f = allFishEntry.Data;
					if (string.IsNullOrEmpty(f.FishingHole))
					{
						f.FishingHole = "Unknown";
					}
					f.FishingHole = f.FishingHole.Replace("None, ", "");
					if (f.FishingHole == "None")
					{
						f.FishingHole = "Open Water";
					}
					foreach (string sh in from h in f.FishingHole.Split(',')
						select h.Trim())
					{
						if (!string.IsNullOrEmpty(sh))
						{
							uniqueHoles.Add(sh);
						}
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
					holeDrop.Items.Add(h2);
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
						if (_personalBests.ContainsKey(fish.ItemId))
						{
							PersonalBestRecord rec = _personalBests[fish.ItemId];
							if (rec.BestWeight != null)
							{
								if (rec.BestWeight.IsCheater)
								{
									pbWText = "CHEATER DETECTED";
								}
								else if (rec.BestWeight.IsSuperPb)
								{
									pbWText = $"[SUPER] {rec.BestWeight.Weight} lbs | {rec.BestWeight.Length} in";
									tintColor = Microsoft.Xna.Framework.Color.Gold;
								}
								else
								{
									pbWText = $"{rec.BestWeight.Weight} lbs | {rec.BestWeight.Length} in";
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
									pbLText = $"[SUPER] {rec.BestLength.Length} in | {rec.BestLength.Weight} lbs";
									tintColor = Microsoft.Xna.Framework.Color.Gold;
								}
								else
								{
									pbLText = $"{rec.BestLength.Length} in | {rec.BestLength.Weight} lbs";
								}
							}
						}
						Image image = new Image();
						image.Parent = p;
						image.Size = new Point(64, 64);
						image.BasicTooltipText = fish.Name + "\nRarity: " + fish.Rarity + "\nLocation: " + fish.Location + "\nHole: " + fish.FishingHole + "\nTime: " + fish.Time + "\nBait: " + fish.Bait + "\n\nPB Weight: " + pbWText + "\nPB Length: " + pbLText;
						image.Texture = ContentsManager.GetTexture("images/" + safeName + ".png");
						image.Tint = tintColor;
						Image img = image;
						img.Click += delegate
						{
							byte[] array = new byte[6] { 2, 1, 0, 0, 0, 0 };
							BitConverter.GetBytes(fish.ItemId).CopyTo(array, 2);
							CopyToClipboard("[&" + Convert.ToBase64String(array) + "]");
							ScreenNotification.ShowNotification("Link copied: " + fish.Name);
						};
						FishUIEntry fishUIEntry = _allFishEntries.First((FishUIEntry x) => x.Data.ItemId == fish.ItemId);
						fishUIEntry.Icon = img;
						fishUIEntry.CategoryPanel = p;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "UI Build Fail");
			}
			void ApplyFilters()
			{
				foreach (FlowPanel cat in _categoryPanels)
				{
					bool anyVisible = false;
					foreach (FishUIEntry entry in _allFishEntries.Where((FishUIEntry x) => x.CategoryPanel == cat))
					{
						bool match = true;
						if (!string.IsNullOrEmpty(searchBar.Text) && !entry.Data.Name.ToLower().Contains(searchBar.Text.ToLower()))
						{
							match = false;
						}
						if (rarityDrop.SelectedItem != "All Rarities" && entry.Data.Rarity != rarityDrop.SelectedItem)
						{
							match = false;
						}
						if (locationDrop.SelectedItem != "All Locations" && entry.Data.Location != locationDrop.SelectedItem)
						{
							match = false;
						}
						if (holeDrop.SelectedItem != "All Holes" && !entry.Data.FishingHole.Contains(holeDrop.SelectedItem))
						{
							match = false;
						}
						if (timeDrop.SelectedItem != "All Times" && !entry.Data.Time.Contains((timeDrop.SelectedItem == "Any") ? "Any" : timeDrop.SelectedItem))
						{
							match = false;
						}
						if (baitDrop.SelectedItem != "All Baits" && !entry.Data.Bait.Contains(baitDrop.SelectedItem))
						{
							match = false;
						}
						if (match)
						{
							anyVisible = true;
							entry.Icon.Parent = null;
							entry.Icon.Parent = cat;
							entry.Icon.Visible = true;
						}
						else
						{
							entry.Icon.Parent = null;
							entry.Icon.Visible = false;
						}
					}
					if (anyVisible)
					{
						cat.Parent = null;
						cat.Parent = scroll;
						cat.Visible = true;
					}
					else
					{
						cat.Parent = null;
						cat.Visible = false;
					}
				}
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
						if (resultObj != null && resultObj.ContainsKey("code"))
						{
							string code = resultObj["code"];
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
					if (!roomCode.StartsWith("GW-"))
					{
						ScreenNotification.ShowNotification("Invalid Code Format!", ScreenNotification.NotificationType.Error);
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
							pbWText = $"[SUPER] {rec.BestWeight.Weight} lbs | {rec.BestWeight.Length} in";
							tint = Microsoft.Xna.Framework.Color.Gold;
						}
						else
						{
							pbWText = $"{rec.BestWeight.Weight} lbs | {rec.BestWeight.Length} in";
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
							pbLText = $"[SUPER] {rec.BestLength.Length} in | {rec.BestLength.Weight} lbs";
							tint = Microsoft.Xna.Framework.Color.Gold;
						}
						else
						{
							pbLText = $"{rec.BestLength.Length} in | {rec.BestLength.Weight} lbs";
						}
					}
				}
				img.Tint = tint;
				img.BasicTooltipText = fish.Name + "\nRarity: " + fish.Rarity + "\nLocation: " + fish.Location + "\nHole: " + fish.FishingHole + "\nTime: " + fish.Time + "\nBait: " + fish.Bait + "\n\nPB Weight: " + pbWText + "\nPB Length: " + pbLText;
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
				StringContent content = new StringContent(JsonConvert.SerializeObject(new
				{
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

		protected override void Unload()
		{
			GameService.Input.Mouse.LeftMouseButtonReleased -= OnMouseLeftButtonReleased;
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
