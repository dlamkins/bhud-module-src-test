using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GW2app
{
	[Export(typeof(Module))]
	public class GW2app : Module
	{
		private sealed class PollChannel
		{
			public DateTime LastPollUtc;

			public bool StateSeen;

			private volatile bool _superseded;

			private readonly ConcurrentQueue<string> _outbound = new ConcurrentQueue<string>();

			private string _fragId;

			private int _fragNextSeq;

			private StringBuilder _fragData;

			public string SessionId { get; }

			public bool Superseded => _superseded;

			public PollChannel(string sessionId)
			{
				SessionId = sessionId;
				LastPollUtc = DateTime.UtcNow;
			}

			public void MarkSuperseded()
			{
				_superseded = true;
			}

			public void Enqueue(string json)
			{
				if (!_superseded)
				{
					_outbound.Enqueue(json);
				}
			}

			public string AcceptFragment(JObject msg)
			{
				JObject f = msg["__frag"] as JObject;
				if (f == null)
				{
					return null;
				}
				string id = f["id"]?.Value<string>();
				if (string.IsNullOrEmpty(id))
				{
					return null;
				}
				int seq = f["seq"]?.Value<int>() ?? (-1);
				bool final = ((f["final"]?.Value<bool>() ?? false) ? ((byte)1) : ((byte)0)) != 0;
				string data = msg["data"]?.Value<string>() ?? "";
				if (id != _fragId)
				{
					if (seq != 0)
					{
						_fragId = null;
						return null;
					}
					_fragId = id;
					_fragNextSeq = 0;
					_fragData = new StringBuilder();
				}
				if (seq != _fragNextSeq)
				{
					_fragId = null;
					_fragData = null;
					return null;
				}
				_fragData.Append(data);
				_fragNextSeq++;
				if (!final)
				{
					return null;
				}
				string result = _fragData.ToString();
				_fragId = null;
				_fragData = null;
				return result;
			}

			public List<string> DrainOutbound()
			{
				List<string> list = new List<string>();
				string s;
				while (_outbound.TryDequeue(out s))
				{
					list.Add(s);
				}
				return list;
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<GW2app>();

		private const int HttpPort = 38473;

		private const int BaseDisplayWidth = 400;

		private const int ProtocolVersion = 2;

		private const string ModuleId = "blish";

		private const int HandshakeTimeoutMs = 5000;

		private const int CloseCodeSuperseded = 4000;

		private const int CloseCodeHandshakeTimeout = 4001;

		private const int CloseCodeProtocolViolation = 4002;

		private static readonly TimeSpan PollSessionTimeout = TimeSpan.FromSeconds(20.0);

		internal const int Gw2ChatMaxLength = 199;

		internal const int MaxWaypointsPerMessage = 15;

		internal static GW2app GW2appInstance;

		private Texture2D _iconTexture;

		private Texture2D _iconHiddenTexture;

		private Texture2D _cornerSourceTexture;

		private Texture2D _logoTexture;

		private Texture2D _dotConnectedTexture;

		private Texture2D _dotNotConnectedTexture;

		private Texture2D _dividerTexture;

		private Texture2D _rechargeTexture;

		private int _lastCountdownMinute = -1;

		private CornerIcon _cornerIcon;

		private ContextMenuStrip _contextMenuStrip;

		private readonly Dictionary<string, ListWindowEntry> _listWindows = new Dictionary<string, ListWindowEntry>();

		private HttpListener _httpListener;

		private CancellationTokenSource _httpCts;

		private readonly object _clientLock = new object();

		private WebSocket _activeClient;

		private CancellationTokenSource _activeClientCts;

		private PollChannel _activePollSession;

		private string _lastSupersededPollId;

		private int _hasActiveConnection;

		private int _connectionStateDirty;

		private StateMessage _catalog;

		private readonly Dictionary<string, Texture2D> _entryImages = new Dictionary<string, Texture2D>();

		private readonly Dictionary<string, string> _entryChatLinks = new Dictionary<string, string>();

		private readonly Dictionary<string, string> _entryLinks = new Dictionary<string, string>();

		private readonly Dictionary<string, Texture2D> _hoverImageCache = new Dictionary<string, Texture2D>();

		private HashSet<string> _lastSubscribedIds = new HashSet<string>();

		private readonly HashSet<string> _loadingLists = new HashSet<string>();

		private readonly Dictionary<string, DateTime> _loadingStartTimes = new Dictionary<string, DateTime>();

		private readonly HashSet<string> _loadingFailures = new HashSet<string>();

		private const int LoadingTimeoutSeconds = 20;

		private readonly HashSet<string> _pendingEntries = new HashSet<string>();

		private readonly HashSet<string> _completedSectionCollapsed = new HashSet<string>();

		private bool _restoredFromPersistence;

		private bool _unloading;

		private SettingEntry<string> _persistedOpenListsJson;

		private SettingEntry<string> _persistedCollapsedCompletedJson;

		private SettingEntry<int> _maxWaypointsPerCopy;

		private readonly HashSet<string> _copyModeListIds = new HashSet<string>();

		private readonly HashSet<string> _deferredRefreshes = new HashSet<string>();

		private bool _contextMenuRebuildPending;

		private SettingEntry<GW2appWindow.WindowTheme> _windowTheme;

		private SettingEntry<int> _bgOpacityPct;

		private SettingEntry<int> _uiScalePct;

		private SettingEntry<bool> _showAccountName;

		private SettingEntry<bool> _showCopyWaypointsButton;

		private SettingEntry<KeyBinding> _toggleListsKeybind;

		private readonly HashSet<string> _peekHiddenIds = new HashSet<string>();

		private bool _suppressListVisibilityHandlers;

		private readonly ConcurrentQueue<IncomingMessage> _incomingMessages = new ConcurrentQueue<IncomingMessage>();

		private GW2appWindow _infoWindow;

		private Label _infoStatusLabel;

		private Image _infoStatusDot;

		private int _infoStatusRowY;

		private Label _infoConnectedHint1;

		private Label _infoConnectedHint2;

		private const int DotIconSize = 32;

		private const int DotVisibleSize = 12;

		private const int DotTextGap = 6;

		private const int DotVerticalNudge = 2;

		private const int TooltipMaxChars = 40;

		private const int DividerLeftX = 0;

		private static readonly Color DividerColor = new Color(38, 38, 38, 38);

		private static readonly FieldInfo _panelScrollbarField = typeof(Panel).GetField("_panelScrollbar", BindingFlags.Instance | BindingFlags.NonPublic);

		private static readonly FieldInfo _labelBaseFontField = typeof(LabelBase).GetField("_font", BindingFlags.Instance | BindingFlags.NonPublic);

		private const int FooterTopMargin = -4;

		private const int FooterBottomMargin = 6;

		private const int LoadingTopPad = 10;

		private const int LoadingBottomPad = 20;

		private const int LoadingTextGap = 0;

		private const int FailedTextHeight = 18;

		private const int FailedButtonGap = 8;

		private const int BaseTitleMaxChars = 12;

		private const int BaseTitleMaxCharsWithCountdown = 10;

		private const int BaseSubtitleMaxChars = 15;

		private const int BaseSubtitleMaxCharsWithCountdown = 13;

		private const float CompactTitleBonus = 1.5f;

		private const float CompactSubtitleBonus = 1.15f;

		private const float SubtitleScaleSpeed = 1.5f;

		private const int FixedRightChrome = 20;

		private const int CheckboxSize = 22;

		private const int CheckboxLeftMargin = 8;

		private const int CheckboxColumnWidth = 30;

		private static readonly Color PendingTint = new Color(110, 110, 110);

		private const int BaseWindowMaxHeight = 440;

		private const int BaseWindowBaseHeight = 130;

		private const int ImagesTopMargin = 0;

		private const int ImagesBottomMargin = 10;

		private const int WindowVerticalChrome = 40;

		private static readonly TimeSpan ListenerRestartCooldown = TimeSpan.FromSeconds(5.0);

		private DateTime _lastListenerRestartUtc = DateTime.MinValue;

		private static int _wsDetectionState;

		private int DisplayWidth => (int)Math.Round(400f * UiScale);

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		private float UiScale => (float)(_uiScalePct?.get_Value() ?? 100) / 100f;

		private float BgOpacity => (float)(_bgOpacityPct?.get_Value() ?? 85) / 100f;

		private bool IsClientConnected
		{
			get
			{
				WebSocket ws;
				PollChannel poll;
				lock (_clientLock)
				{
					ws = _activeClient;
					poll = _activePollSession;
				}
				if (ws != null && ws.State == WebSocketState.Open)
				{
					return true;
				}
				if (poll != null)
				{
					return !poll.Superseded;
				}
				return false;
			}
		}

		private float CaptureScale
		{
			get
			{
				foreach (Texture2D tex in _entryImages.Values)
				{
					if (tex != null && tex.get_Width() > 0)
					{
						return (float)DisplayWidth / (float)tex.get_Width();
					}
				}
				return UiScale;
			}
		}

		private bool ShowAccountName => _showAccountName?.get_Value() ?? true;

		private bool ShowCopyWaypointsButton => _showCopyWaypointsButton?.get_Value() ?? true;

		private GW2appWindow.WindowTheme CurrentTheme => _windowTheme?.get_Value() ?? GW2appWindow.WindowTheme.Game;

		private int ActionButtonWidth => ActionButton.WidthFor(UiScale);

		private int ActionButtonHeight => ActionButton.HeightFor(UiScale);

		private int FooterReserveTotal => -4 + ActionButtonHeight + 6;

		private int WindowWidth => 30 + DisplayWidth + 20;

		private int LootBagWindowWidth => DisplayWidth + 20;

		private int WindowMaxHeight => (int)Math.Round(440f * UiScale);

		private int WindowBaseHeight => (int)Math.Round(130f * UiScale);

		[ImportingConstructor]
		public GW2app([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			GW2appInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Expected O, but got Unknown
			SettingCollection appearance = settings.AddSubCollection("appearance", true, (Func<string>)(() => "Appearance"));
			_windowTheme = appearance.DefineSetting<GW2appWindow.WindowTheme>("windowTheme", GW2appWindow.WindowTheme.Game, (Func<string>)(() => "Window theme"), (Func<string>)(() => "Changes the background of windows"));
			_showAccountName = appearance.DefineSetting<bool>("showAccountName", true, (Func<string>)(() => "Show GW2 account name in list header"), (Func<string>)(() => "Hides the account-name subtitle on the list windows when off."));
			_showCopyWaypointsButton = appearance.DefineSetting<bool>("showCopyWaypointsButton", true, (Func<string>)(() => "Show \"Copy waypoints\" button in lists"), (Func<string>)(() => "Hides the bottom-of-list button used to copy chat-link waypoints."));
			_bgOpacityPct = appearance.DefineSetting<int>("bgOpacityPct", 85, (Func<string>)(() => "Window background opacity"), (Func<string>)(() => "How opaque the window background is. Lower is more see-through."));
			SettingComplianceExtensions.SetRange(_bgOpacityPct, 75, 100);
			SettingCollection sizing = settings.AddSubCollection("sizing", true, (Func<string>)(() => "Sizing"));
			_uiScalePct = sizing.DefineSetting<int>("uiScalePct", 100, (Func<string>)(() => "List scale"), (Func<string>)(() => "Scales list window dimensions and entry images"));
			SettingComplianceExtensions.SetRange(_uiScalePct, 75, 125);
			SettingCollection controls = settings.AddSubCollection("controls", true, (Func<string>)(() => "Controls"));
			_toggleListsKeybind = controls.DefineSetting<KeyBinding>("toggleListsVisibility", new KeyBinding((Keys)0), (Func<string>)(() => "Show/hide all lists"), (Func<string>)(() => "Hides or restores every open list window. Purely visual: hidden lists stay connected and reappear instantly."));
			SettingCollection internalSettings = settings.AddSubCollection("internal", false);
			_persistedOpenListsJson = internalSettings.DefineSetting<string>("openLists", "[]", (Func<string>)null, (Func<string>)null);
			_persistedCollapsedCompletedJson = internalSettings.DefineSetting<string>("collapsedCompleted", "[]", (Func<string>)null, (Func<string>)null);
			_maxWaypointsPerCopy = internalSettings.DefineSetting<int>("maxWaypointsPerCopy", 15, (Func<string>)null, (Func<string>)null);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new GW2appSettingsView(_windowTheme, _bgOpacityPct, _showAccountName, _showCopyWaypointsButton, _uiScalePct, _toggleListsKeybind, delegate
			{
				if (_uiScalePct != null)
				{
					_uiScalePct.set_Value(100);
				}
			});
		}

		protected override void Initialize()
		{
			_iconTexture = ContentsManager.GetTexture("gw2app-icon.png");
			CreateCornerIcon();
			RebuildContextMenu();
			RestoreCollapsedCompleted();
			if (_toggleListsKeybind?.get_Value() != null)
			{
				_toggleListsKeybind.get_Value().set_Enabled(true);
				_toggleListsKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleListsKeybind);
			}
		}

		protected override async Task LoadAsync()
		{
			_rechargeTexture = ContentsManager.GetTexture("recharge.png");
			_cornerSourceTexture = ContentsManager.GetTexture("gw2app-corner.png");
			_logoTexture = ContentsManager.GetTexture("gw2app-logo.png");
			_dotConnectedTexture = ContentsManager.GetTexture("connected.png");
			_dotNotConnectedTexture = ContentsManager.GetTexture("not-connected.png");
			Texture2D hiddenIcon = ContentsManager.GetTexture("gw2app-icon-lists-hidden.png");
			if (hiddenIcon != null && hiddenIcon != Textures.get_Error())
			{
				_iconHiddenTexture = hiddenIcon;
			}
			AsyncTexture2D.FromAssetId(155997);
			if (_windowTheme != null)
			{
				_windowTheme.add_SettingChanged((EventHandler<ValueChangedEventArgs<GW2appWindow.WindowTheme>>)OnWindowThemeChanged);
			}
			if (_bgOpacityPct != null)
			{
				_bgOpacityPct.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnBgOpacityChanged);
			}
			if (_uiScalePct != null)
			{
				_uiScalePct.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUiScaleChanged);
			}
			if (_showAccountName != null)
			{
				_showAccountName.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowAccountNameChanged);
			}
			if (_showCopyWaypointsButton != null)
			{
				_showCopyWaypointsButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowCopyWaypointsButtonChanged);
			}
			StartHttpServer();
			HoverCard.Init(delegate(string lid, int idx)
			{
				SendOpenHoverAsync(lid, idx);
			}, delegate
			{
				SendCloseHoverAsync();
			}, () => UiScale, () => CaptureScale, (string lid, int idx) => (!_hoverImageCache.TryGetValue(EntryKey(lid, idx), out var value)) ? null : value, () => IsClientConnected);
			await Task.CompletedTask;
		}

		private void OnBgOpacityChanged(object sender, ValueChangedEventArgs<int> e)
		{
			float opacity = (float)e.get_NewValue() / 100f;
			foreach (ListWindowEntry value in _listWindows.Values)
			{
				value.Window?.SetBackgroundOpacity(opacity);
			}
			_infoWindow?.SetBackgroundOpacity(opacity);
		}

		private void OnWindowThemeChanged(object sender, ValueChangedEventArgs<GW2appWindow.WindowTheme> e)
		{
			foreach (ListWindowEntry value in _listWindows.Values)
			{
				value.Window?.SetWindowTheme(e.get_NewValue());
			}
			_infoWindow?.SetWindowTheme(e.get_NewValue());
			foreach (string id in _listWindows.Keys.ToList())
			{
				RefreshListWindow(id);
			}
		}

		private void OnUiScaleChanged(object sender, ValueChangedEventArgs<int> e)
		{
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			bool compact = UiScale < 1f;
			foreach (KeyValuePair<string, ListWindowEntry> kvp in _listWindows)
			{
				ListWindowEntry entry = kvp.Value;
				ListDto list = _catalog?.Lists?.FirstOrDefault((ListDto l) => l.Id == kvp.Key);
				entry.Window.SetCompactTitle(compact);
				entry.Window.SetWindowSize(WindowWidthFor(list), WindowMaxHeight);
				if (list != null)
				{
					entry.Window.Title = TitleFor(list);
					entry.Window.Subtitle = SubtitleFor(list);
					entry.Window.SetEmblemTinted(_cornerSourceTexture, EmblemTintFor(list), UiScale);
				}
				RefreshListWindow(kvp.Key);
			}
			HoverCard.RefreshScale();
		}

		private void OnShowAccountNameChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			foreach (KeyValuePair<string, ListWindowEntry> kvp in _listWindows)
			{
				ListDto list = _catalog?.Lists?.FirstOrDefault((ListDto l) => l.Id == kvp.Key);
				if (list != null)
				{
					kvp.Value.Window.Subtitle = SubtitleFor(list);
				}
			}
		}

		private void OnShowCopyWaypointsButtonChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (!e.get_NewValue())
			{
				_copyModeListIds.Clear();
			}
			foreach (string id in _listWindows.Keys.ToList())
			{
				RefreshListWindow(id);
			}
		}

		protected override void Update(GameTime gameTime)
		{
			try
			{
				UpdateTick(gameTime);
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Unhandled error in Update tick.");
			}
		}

		private void UpdateTick(GameTime gameTime)
		{
			bool catalogChanged = false;
			HashSet<string> dirtyLists = new HashSet<string>();
			ReapStalePollSession();
			EnsureHttpListenerAlive();
			IncomingMessage msg;
			while (_incomingMessages.TryDequeue(out msg))
			{
				try
				{
					switch (msg.Kind)
					{
					case MessageKind.State:
						if (ApplyState(msg.State, dirtyLists))
						{
							catalogChanged = true;
						}
						break;
					case MessageKind.Entry:
						if (ApplyEntry(msg.Entry))
						{
							dirtyLists.Add(msg.Entry.ListId);
						}
						break;
					case MessageKind.HoverImage:
						ApplyHoverImage(msg.HoverImage);
						break;
					case MessageKind.Synced:
						Logger.Info("Received synced for [" + string.Join(",", msg.SyncedListIds ?? new List<string>()) + "] (loading was [" + string.Join(",", _loadingLists) + "])");
						foreach (string id2 in msg.SyncedListIds ?? new List<string>())
						{
							PruneImagesAfterSynced(id2);
							bool wasLoading = _loadingLists.Contains(id2);
							MarkLoaded(id2);
							Logger.Info($"Synced: cleared loading={wasLoading} for {id2}");
							dirtyLists.Add(id2);
						}
						break;
					case MessageKind.ConnectionLost:
						ResetClientState(dropCatalog: true);
						catalogChanged = true;
						break;
					case MessageKind.ClientReplaced:
						ResetClientState(dropCatalog: false);
						catalogChanged = true;
						break;
					}
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to apply message.");
				}
			}
			if (Interlocked.Exchange(ref _connectionStateDirty, 0) != 0)
			{
				catalogChanged = true;
			}
			int currentMinuteEpoch = (int)(DateTime.UtcNow.Ticks / 600000000);
			if (_lastCountdownMinute != currentMinuteEpoch)
			{
				_lastCountdownMinute = currentMinuteEpoch;
				RefreshOpenWindowCountdowns();
			}
			if (catalogChanged)
			{
				ReconcileListWindows();
				RebuildContextMenu();
				foreach (string id in _listWindows.Keys)
				{
					dirtyLists.Add(id);
				}
			}
			if (_contextMenuRebuildPending)
			{
				_contextMenuRebuildPending = false;
				if (!catalogChanged)
				{
					RebuildContextMenu();
				}
			}
			if (_loadingLists.Count > 0)
			{
				DateTime nowUtc = DateTime.UtcNow;
				foreach (string listId3 in _loadingLists.ToList())
				{
					if (!_loadingFailures.Contains(listId3) && _loadingStartTimes.TryGetValue(listId3, out var start) && (nowUtc - start).TotalSeconds > 20.0)
					{
						_loadingFailures.Add(listId3);
						Logger.Warn($"Loading timeout for list {listId3} after {20}s; surfacing Retry.");
						dirtyLists.Add(listId3);
					}
				}
			}
			foreach (string listId2 in dirtyLists)
			{
				RefreshListWindow(listId2);
			}
			if (_deferredRefreshes.Count > 0)
			{
				foreach (string listId in _deferredRefreshes.ToList())
				{
					RefreshListWindow(listId);
				}
				_deferredRefreshes.Clear();
			}
			HoverCard.Tick();
		}

		private bool ApplyState(StateMessage state, HashSet<string> dirtyLists)
		{
			StateMessage oldCatalog = _catalog;
			_catalog = state;
			if (!_restoredFromPersistence)
			{
				_restoredFromPersistence = true;
				RestorePersistedOpenLists();
			}
			foreach (ListDto list in state.Lists ?? new List<ListDto>())
			{
				if (string.IsNullOrEmpty(list.Id))
				{
					continue;
				}
				dirtyLists.Add(list.Id);
				if (_lastSubscribedIds.Contains(list.Id) && EntriesChanged(oldCatalog, list.Id, list))
				{
					bool num = _loadingLists.Contains(list.Id);
					MarkLoading(list.Id);
					if (!num)
					{
						Logger.Info("Loading: marked " + list.Id + " as loading (state changed)");
					}
				}
			}
			return true;
		}

		private static bool EntriesChanged(StateMessage oldState, string listId, ListDto newList)
		{
			ListDto oldList = oldState?.Lists?.FirstOrDefault((ListDto l) => l.Id == listId);
			if (oldList == null)
			{
				return true;
			}
			List<EntryDto> oldEntries = oldList.Entries ?? new List<EntryDto>();
			List<EntryDto> newEntries = newList.Entries ?? new List<EntryDto>();
			if (oldEntries.Count != newEntries.Count)
			{
				return true;
			}
			for (int i = 0; i < oldEntries.Count; i++)
			{
				if (oldEntries[i].Completed != newEntries[i].Completed || oldEntries[i].AutoCompleted != newEntries[i].AutoCompleted)
				{
					return true;
				}
			}
			return false;
		}

		private void PruneImagesAfterSynced(string listId)
		{
			int validCount = ((_catalog?.Lists?.FirstOrDefault((ListDto l) => l.Id == listId))?.Entries?.Count).GetValueOrDefault();
			string prefix = listId + ":";
			List<string> keysToRemove = new List<string>();
			foreach (string key2 in _entryImages.Keys)
			{
				if (key2.StartsWith(prefix) && int.TryParse(key2.Substring(prefix.Length), out var idx) && idx >= validCount)
				{
					keysToRemove.Add(key2);
				}
			}
			foreach (string key in keysToRemove)
			{
				if (_entryImages.TryGetValue(key, out var tex) && tex != null)
				{
					((GraphicsResource)tex).Dispose();
				}
				_entryImages.Remove(key);
				_entryChatLinks.Remove(key);
				_entryLinks.Remove(key);
			}
		}

		private bool ApplyEntry(EntryMessage entry)
		{
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			if (entry == null || string.IsNullOrEmpty(entry.ListId))
			{
				return false;
			}
			if (_catalog?.Lists == null)
			{
				return false;
			}
			ListDto list = _catalog.Lists.FirstOrDefault((ListDto l) => l.Id == entry.ListId);
			if (list == null || list.Entries == null)
			{
				return false;
			}
			if (entry.Index < 0 || entry.Index >= list.Entries.Count)
			{
				return false;
			}
			EntryDto e = list.Entries[entry.Index];
			if (!string.IsNullOrEmpty(entry.Name))
			{
				e.Name = entry.Name;
			}
			e.Completed = entry.Completed;
			e.AutoCompleted = entry.AutoCompleted;
			e.HasHoverCard = entry.HasHoverCard;
			string chatKey = EntryKey(entry.ListId, entry.Index);
			if (string.IsNullOrEmpty(entry.ChatLink))
			{
				_entryChatLinks.Remove(chatKey);
			}
			else
			{
				_entryChatLinks[chatKey] = entry.ChatLink;
			}
			if (string.IsNullOrEmpty(entry.Link))
			{
				_entryLinks.Remove(chatKey);
			}
			else
			{
				_entryLinks[chatKey] = entry.Link;
			}
			if (!string.IsNullOrEmpty(entry.ImageB64))
			{
				try
				{
					Texture2D newTex;
					using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(entry.ImageB64)))
					{
						GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
						try
						{
							newTex = Texture2D.FromStream(((GraphicsDeviceContext)(ref gdc)).get_GraphicsDevice(), (Stream)ms);
							PremultiplyAlpha(newTex);
						}
						finally
						{
							((GraphicsDeviceContext)(ref gdc)).Dispose();
						}
					}
					string key = EntryKey(entry.ListId, entry.Index);
					if (_entryImages.TryGetValue(key, out var oldTex) && oldTex != null)
					{
						((GraphicsResource)oldTex).Dispose();
					}
					_entryImages[key] = newTex;
					_pendingEntries.Remove(key);
					if (_hoverImageCache.TryGetValue(key, out var staleHover))
					{
						try
						{
							if (staleHover != null)
							{
								((GraphicsResource)staleHover).Dispose();
							}
						}
						catch
						{
						}
						_hoverImageCache.Remove(key);
					}
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, $"Failed to decode image for {entry.ListId}[{entry.Index}] ({entry.Mime}).");
				}
			}
			return true;
		}

		private void ApplyHoverImage(HoverImageMessage hi)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (hi == null || string.IsNullOrEmpty(hi.ListId) || string.IsNullOrEmpty(hi.ImageB64))
			{
				return;
			}
			Texture2D newTex;
			try
			{
				using MemoryStream ms = new MemoryStream(Convert.FromBase64String(hi.ImageB64));
				GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					newTex = Texture2D.FromStream(((GraphicsDeviceContext)(ref gdc)).get_GraphicsDevice(), (Stream)ms);
					PremultiplyAlpha(newTex);
				}
				finally
				{
					((GraphicsDeviceContext)(ref gdc)).Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, $"Failed to decode hover_image for {hi.ListId}[{hi.Index}] ({hi.Mime}).");
				return;
			}
			string key = EntryKey(hi.ListId, hi.Index);
			_hoverImageCache.TryGetValue(key, out var oldTex);
			_hoverImageCache[key] = newTex;
			HoverCard.SetImage(hi.ListId, hi.Index, newTex);
			if (oldTex != null)
			{
				try
				{
					((GraphicsResource)oldTex).Dispose();
				}
				catch
				{
				}
			}
		}

		private static string EntryKey(string listId, int index)
		{
			return listId + ":" + index;
		}

		private void MarkLoading(string listId)
		{
			_loadingLists.Add(listId);
			_loadingStartTimes[listId] = DateTime.UtcNow;
			_loadingFailures.Remove(listId);
		}

		private void MarkLoaded(string listId)
		{
			_loadingLists.Remove(listId);
			_loadingStartTimes.Remove(listId);
			_loadingFailures.Remove(listId);
		}

		private void ResetClientState(bool dropCatalog)
		{
			HoverCard.Teardown();
			foreach (Texture2D tex2 in _entryImages.Values)
			{
				try
				{
					if (tex2 != null)
					{
						((GraphicsResource)tex2).Dispose();
					}
				}
				catch
				{
				}
			}
			_entryImages.Clear();
			_entryChatLinks.Clear();
			_entryLinks.Clear();
			foreach (Texture2D tex in _hoverImageCache.Values)
			{
				try
				{
					if (tex != null)
					{
						((GraphicsResource)tex).Dispose();
					}
				}
				catch
				{
				}
			}
			_hoverImageCache.Clear();
			_pendingEntries.Clear();
			_loadingLists.Clear();
			_loadingStartTimes.Clear();
			_loadingFailures.Clear();
			_copyModeListIds.Clear();
			_lastSubscribedIds = new HashSet<string>();
			_restoredFromPersistence = false;
			if (dropCatalog)
			{
				_catalog = null;
			}
		}

		protected override void Unload()
		{
			_unloading = true;
			if (_windowTheme != null)
			{
				_windowTheme.remove_SettingChanged((EventHandler<ValueChangedEventArgs<GW2appWindow.WindowTheme>>)OnWindowThemeChanged);
			}
			if (_bgOpacityPct != null)
			{
				_bgOpacityPct.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnBgOpacityChanged);
			}
			if (_uiScalePct != null)
			{
				_uiScalePct.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUiScaleChanged);
			}
			if (_showAccountName != null)
			{
				_showAccountName.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowAccountNameChanged);
			}
			if (_showCopyWaypointsButton != null)
			{
				_showCopyWaypointsButton.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowCopyWaypointsButtonChanged);
			}
			if (_toggleListsKeybind?.get_Value() != null)
			{
				_toggleListsKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleListsKeybind);
			}
			try
			{
				_httpCts?.Cancel();
			}
			catch
			{
			}
			try
			{
				_httpListener?.Stop();
			}
			catch
			{
			}
			try
			{
				_httpListener?.Close();
			}
			catch
			{
			}
			_httpListener = null;
			_httpCts?.Dispose();
			_httpCts = null;
			WebSocket activeClient;
			lock (_clientLock)
			{
				activeClient = _activeClient;
				_activeClient = null;
				try
				{
					_activeClientCts?.Cancel();
				}
				catch
				{
				}
				try
				{
					_activeClientCts?.Dispose();
				}
				catch
				{
				}
				_activeClientCts = null;
				_activePollSession?.MarkSuperseded();
				_activePollSession = null;
			}
			if (activeClient != null)
			{
				try
				{
					activeClient.Dispose();
				}
				catch
				{
				}
			}
			foreach (ListWindowEntry entry in _listWindows.Values.ToList())
			{
				try
				{
					GW2appWindow window = entry.Window;
					if (window != null)
					{
						((Control)window).Dispose();
					}
				}
				catch
				{
				}
			}
			_listWindows.Clear();
			ContextMenuStrip contextMenuStrip = _contextMenuStrip;
			if (contextMenuStrip != null)
			{
				((Control)contextMenuStrip).Dispose();
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			GW2appWindow infoWindow = _infoWindow;
			if (infoWindow != null)
			{
				((Control)infoWindow).Dispose();
			}
			HoverCard.Dispose();
			foreach (Texture2D value in _entryImages.Values)
			{
				if (value != null)
				{
					((GraphicsResource)value).Dispose();
				}
			}
			_entryImages.Clear();
			foreach (Texture2D tex in _hoverImageCache.Values)
			{
				try
				{
					if (tex != null)
					{
						((GraphicsResource)tex).Dispose();
					}
				}
				catch
				{
				}
			}
			_hoverImageCache.Clear();
			Texture2D iconTexture = _iconTexture;
			if (iconTexture != null)
			{
				((GraphicsResource)iconTexture).Dispose();
			}
			Texture2D iconHiddenTexture = _iconHiddenTexture;
			if (iconHiddenTexture != null)
			{
				((GraphicsResource)iconHiddenTexture).Dispose();
			}
			Texture2D cornerSourceTexture = _cornerSourceTexture;
			if (cornerSourceTexture != null)
			{
				((GraphicsResource)cornerSourceTexture).Dispose();
			}
			Texture2D logoTexture = _logoTexture;
			if (logoTexture != null)
			{
				((GraphicsResource)logoTexture).Dispose();
			}
			Texture2D dotConnectedTexture = _dotConnectedTexture;
			if (dotConnectedTexture != null)
			{
				((GraphicsResource)dotConnectedTexture).Dispose();
			}
			Texture2D dotNotConnectedTexture = _dotNotConnectedTexture;
			if (dotNotConnectedTexture != null)
			{
				((GraphicsResource)dotNotConnectedTexture).Dispose();
			}
			Texture2D dividerTexture = _dividerTexture;
			if (dividerTexture != null)
			{
				((GraphicsResource)dividerTexture).Dispose();
			}
			Texture2D rechargeTexture = _rechargeTexture;
			if (rechargeTexture != null)
			{
				((GraphicsResource)rechargeTexture).Dispose();
			}
			GW2appInstance = null;
		}

		private void CreateCornerIcon()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_iconTexture));
			((Control)val).set_BasicTooltipText("GW2.app (not connected)");
			val.set_Priority(1645843523);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			_contextMenuStrip = new ContextMenuStrip();
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_contextMenuStrip.Show((Control)(object)_cornerIcon);
			});
			((Control)_cornerIcon).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				_contextMenuStrip.Show((Control)(object)_cornerIcon);
			});
		}

		private void OpenInfoWindow()
		{
			if (_infoWindow == null)
			{
				BuildInfoWindow();
			}
			((WindowBase2)_infoWindow).ToggleWindow();
		}

		private void BuildInfoWindow()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Expected O, but got Unknown
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Expected O, but got Unknown
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Expected O, but got Unknown
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Expected O, but got Unknown
			GW2appWindow gW2appWindow = new GW2appWindow(430, 460, _windowTheme?.get_Value() ?? GW2appWindow.WindowTheme.Game, compactTitle: false, BgOpacity);
			((Control)gW2appWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			gW2appWindow.Title = "Connect GW2.app";
			gW2appWindow.Subtitle = "";
			((Control)gW2appWindow).set_Location(new Point(300, 120));
			((WindowBase2)gW2appWindow).set_SavesPosition(true);
			((WindowBase2)gW2appWindow).set_Id("GW2app_InfoWindow");
			((WindowBase2)gW2appWindow).set_CanCloseWithEscape(true);
			_infoWindow = gW2appWindow;
			_infoWindow.SetEmblemTinted(_cornerSourceTexture, Color.get_White());
			((Control)_infoWindow).add_Disposed((EventHandler<EventArgs>)delegate
			{
				_infoWindow = null;
				_infoStatusLabel = null;
				_infoStatusDot = null;
			});
			Texture2D logoTexture = _logoTexture;
			int srcW = ((logoTexture != null) ? logoTexture.get_Width() : 200);
			Texture2D logoTexture2 = _logoTexture;
			int num = ((logoTexture2 != null) ? logoTexture2.get_Height() : 108);
			int logoW = srcW * 2 / 3;
			int logoH = num * 2 / 3;
			Image val = new Image(AsyncTexture2D.op_Implicit(_logoTexture));
			((Control)val).set_Size(new Point(logoW, logoH));
			((Control)val).set_Location(new Point((((Container)_infoWindow).get_ContentRegion().Width - logoW) / 2, 54));
			((Control)val).set_Parent((Container)(object)_infoWindow);
			int y = 54 + logoH + 28;
			Label val2 = new Label();
			val2.set_Text("Visit gw2.app/blish in a web browser");
			val2.set_TextColor(Color.get_LightGray());
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Size(new Point(((Container)_infoWindow).get_ContentRegion().Width, 22));
			((Control)val2).set_Location(new Point(0, y));
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_Parent((Container)(object)_infoWindow);
			y += 22;
			Label val3 = new Label();
			val3.set_Text("and click Connect to send your lists in-game.");
			val3.set_TextColor(Color.get_LightGray());
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Size(new Point(((Container)_infoWindow).get_ContentRegion().Width, 22));
			((Control)val3).set_Location(new Point(0, y));
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val3).set_Parent((Container)(object)_infoWindow);
			y += 28;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Open gw2.app/blish");
			((Control)val4).set_Width(200);
			((Control)val4).set_Height(30);
			((Control)val4).set_Location(new Point((((Container)_infoWindow).get_ContentRegion().Width - 200) / 2, y));
			((Control)val4).set_Parent((Container)(object)_infoWindow);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					Process.Start("https://gw2.app/blish");
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to open browser.");
				}
			});
			y = (_infoStatusRowY = y + 48);
			Image val5 = new Image(AsyncTexture2D.op_Implicit(_dotConnectedTexture));
			((Control)val5).set_Size(new Point(32, 32));
			((Control)val5).set_Location(new Point(0, _infoStatusRowY));
			((Control)val5).set_Parent((Container)(object)_infoWindow);
			_infoStatusDot = val5;
			Label val6 = new Label();
			val6.set_Text("");
			val6.set_Font(GameService.Content.get_DefaultFont16());
			val6.set_AutoSizeWidth(true);
			val6.set_AutoSizeHeight(true);
			((Control)val6).set_Location(new Point(0, _infoStatusRowY));
			((Control)val6).set_Parent((Container)(object)_infoWindow);
			_infoStatusLabel = val6;
			int hintY = _infoStatusRowY + 32 + 14;
			Label val7 = new Label();
			val7.set_Text("Right click the GW2.app icon at the top");
			val7.set_TextColor(Color.get_LightGray());
			val7.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val7).set_Size(new Point(((Container)_infoWindow).get_ContentRegion().Width, 22));
			((Control)val7).set_Location(new Point(0, hintY));
			val7.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val7).set_Parent((Container)(object)_infoWindow);
			_infoConnectedHint1 = val7;
			Label val8 = new Label();
			val8.set_Text("to open the list(s) you want to track.");
			val8.set_TextColor(Color.get_LightGray());
			val8.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val8).set_Size(new Point(((Container)_infoWindow).get_ContentRegion().Width, 22));
			((Control)val8).set_Location(new Point(0, hintY + 22));
			val8.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val8).set_Parent((Container)(object)_infoWindow);
			_infoConnectedHint2 = val8;
			RefreshInfoStatus();
		}

		private void RefreshInfoStatus()
		{
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			if (_infoWindow != null && _infoStatusLabel != null && _infoStatusDot != null)
			{
				bool connected = _hasActiveConnection != 0;
				int listCount = (_catalog?.Lists?.Count).GetValueOrDefault();
				_infoStatusDot.set_Texture(AsyncTexture2D.op_Implicit(connected ? _dotConnectedTexture : _dotNotConnectedTexture));
				_infoStatusLabel.set_Text(connected ? ("Connected (" + listCount + ((listCount == 1) ? " list)" : " lists)")) : "Not connected");
				_infoStatusLabel.set_TextColor(connected ? new Color(50, 205, 50) : new Color(220, 20, 60));
				int visualWidth = 18 + ((Control)_infoStatusLabel).get_Width();
				int num = (((Container)_infoWindow).get_ContentRegion().Width - visualWidth) / 2;
				int dotIconX = num - 10;
				int textX = num + 12 + 6;
				((Control)_infoStatusDot).set_Location(new Point(dotIconX, _infoStatusRowY + 2));
				int labelY = _infoStatusRowY + (32 - ((Control)_infoStatusLabel).get_Height()) / 2;
				((Control)_infoStatusLabel).set_Location(new Point(textX, labelY));
				if (_infoConnectedHint1 != null)
				{
					((Control)_infoConnectedHint1).set_Visible(connected);
				}
				if (_infoConnectedHint2 != null)
				{
					((Control)_infoConnectedHint2).set_Visible(connected);
				}
			}
		}

		private void RebuildContextMenu()
		{
			if (_contextMenuStrip == null)
			{
				return;
			}
			foreach (Control item in ((Container)_contextMenuStrip).get_Children().ToList())
			{
				item.Dispose();
			}
			bool connected = _hasActiveConnection != 0;
			int hiddenCount = _peekHiddenIds.Count;
			if (_cornerIcon != null)
			{
				if (connected)
				{
					int listCount = (_catalog?.Lists?.Count).GetValueOrDefault();
					string tip = "GW2.app (connected, " + listCount + ((listCount == 1) ? " list" : " lists");
					if (hiddenCount > 0)
					{
						tip = tip + ", " + hiddenCount + " hidden";
					}
					((Control)_cornerIcon).set_BasicTooltipText(tip + ")");
				}
				else
				{
					((Control)_cornerIcon).set_BasicTooltipText("GW2.app (not connected)");
				}
				_cornerIcon.set_Icon(AsyncTexture2D.op_Implicit((hiddenCount > 0 && _iconHiddenTexture != null) ? _iconHiddenTexture : _iconTexture));
			}
			RefreshInfoStatus();
			((Control)_contextMenuStrip.AddMenuItem("Show instructions")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenInfoWindow();
			});
			int visibleCount = 0;
			foreach (ListWindowEntry w in _listWindows.Values)
			{
				if (w.Window != null && ((Control)w.Window).get_Visible())
				{
					visibleCount++;
				}
			}
			if (visibleCount > 0 || hiddenCount > 0)
			{
				ContextMenuStripItem obj = _contextMenuStrip.AddMenuItem("Hide all subscribed lists");
				obj.set_CanCheck(true);
				obj.set_Checked(hiddenCount > 0);
				obj.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
				{
					ToggleActiveListsVisibility();
				});
			}
			List<ListDto> lists = _catalog?.Lists;
			if (!connected)
			{
				((Control)_contextMenuStrip.AddMenuItem("(Not connected, no lists available)")).set_Enabled(false);
				return;
			}
			if (lists == null || lists.Count == 0)
			{
				((Control)_contextMenuStrip.AddMenuItem("(no lists)")).set_Enabled(false);
				return;
			}
			SortedDictionary<string, List<ListDto>> byAccount = new SortedDictionary<string, List<ListDto>>(StringComparer.OrdinalIgnoreCase);
			foreach (ListDto list2 in lists)
			{
				if (list2 != null && !string.IsNullOrEmpty(list2.Id))
				{
					string acct = null;
					try
					{
						acct = list2.Settings?["gw2AccountName"]?.Value<string>();
					}
					catch
					{
					}
					string key = acct ?? "";
					if (!byAccount.TryGetValue(key, out var bucket))
					{
						bucket = (byAccount[key] = new List<ListDto>());
					}
					bucket.Add(list2);
				}
			}
			int addedCount = 0;
			foreach (KeyValuePair<string, List<ListDto>> kvp in byAccount)
			{
				string headerText = (string.IsNullOrEmpty(kvp.Key) ? "Lists with no account" : kvp.Key);
				((Control)_contextMenuStrip.AddMenuItem(headerText)).set_Enabled(false);
				foreach (ListDto list in kvp.Value.OrderBy((ListDto l) => l.Name ?? l.Id, StringComparer.OrdinalIgnoreCase))
				{
					string listId = list.Id;
					string name = list.Name ?? list.Id;
					((Control)_contextMenuStrip.AddMenuItem("   " + name)).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						OpenListWindow(listId);
					});
					addedCount++;
				}
			}
			if (addedCount == 0)
			{
				((Control)_contextMenuStrip.AddMenuItem("(no usable lists)")).set_Enabled(false);
			}
		}

		private void OpenListWindow(string listId)
		{
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Expected O, but got Unknown
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Expected O, but got Unknown
			RestorePeekedLists();
			if (_listWindows.TryGetValue(listId, out var existing))
			{
				if (!((Control)existing.Window).get_Visible())
				{
					((Control)existing.Window).Show();
				}
				return;
			}
			ListDto list = _catalog?.Lists?.FirstOrDefault((ListDto l) => l.Id == listId);
			if (list == null)
			{
				return;
			}
			bool compact = UiScale < 1f;
			int initialWidth = WindowWidthFor(list);
			GW2appWindow gW2appWindow = new GW2appWindow(initialWidth, WindowMaxHeight, _windowTheme?.get_Value() ?? GW2appWindow.WindowTheme.Game, compact, BgOpacity);
			((Control)gW2appWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			gW2appWindow.Title = TitleFor(list);
			gW2appWindow.Subtitle = SubtitleFor(list);
			((Control)gW2appWindow).set_Location(new Point(300, 300));
			((WindowBase2)gW2appWindow).set_SavesPosition(true);
			((WindowBase2)gW2appWindow).set_CanResize(true);
			((WindowBase2)gW2appWindow).set_SavesSize(true);
			((WindowBase2)gW2appWindow).set_Id("GW2app_List_" + listId);
			GW2appWindow window = gW2appWindow;
			window.SetEmblemTinted(_cornerSourceTexture, EmblemTintFor(list), UiScale);
			window.SetResetCountdownOverlay(_rechargeTexture, ResetCountdownFor(list));
			Panel val = new Panel();
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(((Container)window).get_ContentRegion().Width, ((Container)window).get_ContentRegion().Height));
			val.set_CanScroll(true);
			val.set_ShowBorder(true);
			((Control)val).set_Parent((Container)(object)window);
			Panel panel = val;
			Panel val2 = new Panel();
			((Control)val2).set_Location(new Point(0, ((Container)window).get_ContentRegion().Height));
			((Control)val2).set_Size(new Point(((Container)window).get_ContentRegion().Width, 0));
			((Control)val2).set_Parent((Container)(object)window);
			Panel footerPanel = val2;
			ListWindowEntry entry = new ListWindowEntry
			{
				Window = window,
				Panel = panel,
				FooterPanel = footerPanel,
				ListId = listId
			};
			window.LayoutRefreshed += delegate
			{
				ResizePanelToWindow(entry);
			};
			EventHandler<EventArgs> onHidden = delegate
			{
				if (!_suppressListVisibilityHandlers)
				{
					RemovePersisted(listId);
					UpdateSubscriptions();
				}
			};
			EventHandler<EventArgs> onShown = delegate
			{
				if (!_suppressListVisibilityHandlers)
				{
					AddPersisted(listId);
					UpdateSubscriptions();
				}
			};
			EventHandler<EventArgs> onDisposed = null;
			onDisposed = delegate
			{
				((Control)window).remove_Hidden(onHidden);
				((Control)window).remove_Shown(onShown);
				((Control)window).remove_Disposed(onDisposed);
				_listWindows.Remove(listId);
				_peekHiddenIds.Remove(listId);
				UpdateSubscriptions();
			};
			((Control)window).add_Hidden(onHidden);
			((Control)window).add_Shown(onShown);
			((Control)window).add_Disposed(onDisposed);
			_listWindows[listId] = entry;
			((Control)window).Show();
			RefreshListWindow(listId);
		}

		private void RefreshOpenWindowCountdowns()
		{
			if (_catalog?.Lists == null)
			{
				return;
			}
			foreach (KeyValuePair<string, ListWindowEntry> kvp in _listWindows)
			{
				ListDto list = _catalog.Lists.FirstOrDefault((ListDto l) => l.Id == kvp.Key);
				if (list != null)
				{
					kvp.Value.Window.SetResetCountdownOverlay(_rechargeTexture, ResetCountdownFor(list));
				}
			}
		}

		private void ReconcileListWindows()
		{
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			if (_hasActiveConnection == 0)
			{
				foreach (string id3 in _listWindows.Keys.ToList())
				{
					if (_listWindows.TryGetValue(id3, out var entry2))
					{
						try
						{
							((Control)entry2.Window).Dispose();
						}
						catch
						{
						}
					}
				}
				UpdateSubscriptions();
				return;
			}
			if (_catalog?.Lists != null)
			{
				HashSet<string> validIds = new HashSet<string>();
				foreach (ListDto i in _catalog.Lists)
				{
					if (i != null && !string.IsNullOrEmpty(i.Id))
					{
						validIds.Add(i.Id);
					}
				}
				foreach (string id2 in _listWindows.Keys.Where((string id) => !validIds.Contains(id)).ToList())
				{
					if (_listWindows.TryGetValue(id2, out var entry))
					{
						try
						{
							((Control)entry.Window).Dispose();
						}
						catch
						{
						}
					}
				}
				foreach (KeyValuePair<string, ListWindowEntry> kvp in _listWindows)
				{
					string listId = kvp.Key;
					ListDto list = _catalog.Lists.FirstOrDefault((ListDto l) => l.Id == listId);
					if (list != null)
					{
						string newTitle = TitleFor(list);
						string newSubtitle = SubtitleFor(list);
						if (kvp.Value.Window.Title != newTitle)
						{
							kvp.Value.Window.Title = newTitle;
						}
						if (kvp.Value.Window.Subtitle != newSubtitle)
						{
							kvp.Value.Window.Subtitle = newSubtitle;
						}
						kvp.Value.Window.SetEmblemTinted(_cornerSourceTexture, EmblemTintFor(list), UiScale);
						kvp.Value.Window.SetResetCountdownOverlay(_rechargeTexture, ResetCountdownFor(list));
					}
				}
			}
			UpdateSubscriptions();
		}

		private void RefreshListWindow(string listId)
		{
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
			if (!_listWindows.TryGetValue(listId, out var entry) || entry.Panel == null)
			{
				return;
			}
			foreach (Control item in ((Container)entry.Panel).get_Children().ToList())
			{
				item.Dispose();
			}
			if (entry.FooterPanel != null)
			{
				foreach (Control item2 in ((Container)entry.FooterPanel).get_Children().ToList())
				{
					item2.Dispose();
				}
			}
			entry.FooterReserve = 0;
			entry.RerenderCopyChunks = null;
			((WindowBase2)entry.Window).set_CanResize(true);
			ListDto list = _catalog?.Lists?.FirstOrDefault((ListDto l) => l.Id == listId);
			if (_loadingFailures.Contains(listId))
			{
				entry.Panel.set_CanScroll(false);
				_copyModeListIds.Remove(listId);
				RenderLoadingFailed(entry, listId);
				return;
			}
			if (_loadingLists.Contains(listId))
			{
				int totalForLoading = (list?.Entries?.Count).GetValueOrDefault();
				int loadedCount = 0;
				if (totalForLoading > 0)
				{
					for (int i2 = 0; i2 < totalForLoading; i2++)
					{
						if (_entryImages.ContainsKey(EntryKey(listId, i2)))
						{
							loadedCount++;
						}
					}
				}
				if (totalForLoading <= 0 || loadedCount < totalForLoading)
				{
					entry.Panel.set_CanScroll(false);
					_copyModeListIds.Remove(listId);
					RenderLoadingProgress(entry, loadedCount, totalForLoading);
					return;
				}
				Logger.Info($"All {totalForLoading} images cached for list {listId} but no `synced` received; auto-clearing loading state.");
				MarkLoaded(listId);
			}
			if (list == null)
			{
				return;
			}
			int total = list.Entries?.Count ?? 0;
			if (total == 0)
			{
				entry.Panel.set_CanScroll(false);
				_copyModeListIds.Remove(listId);
				ResizeWindowAndPanel(entry, WindowBaseHeight);
				Label val = new Label();
				val.set_Text("Empty list");
				val.set_TextColor(Color.get_LightGray());
				val.set_Font(GameService.Content.get_DefaultFont18());
				((Control)val).set_Size(new Point(((Control)entry.Panel).get_Width(), ((Control)entry.Panel).get_Height()));
				((Control)val).set_Location(new Point(0, 0));
				val.set_HorizontalAlignment((HorizontalAlignment)1);
				val.set_VerticalAlignment((VerticalAlignment)1);
				((Control)val).set_Parent((Container)(object)entry.Panel);
				return;
			}
			List<string> chatLinks = new List<string>();
			for (int n = 0; n < total; n++)
			{
				EntryDto e = list.Entries[n];
				if (e == null || (!e.Completed && !e.AutoCompleted))
				{
					string t = e?.EntryType;
					if ((string.IsNullOrEmpty(t) || t == "location" || t == "dailypsna" || t == "vendoritem") && _entryChatLinks.TryGetValue(EntryKey(listId, n), out var cl) && !string.IsNullOrEmpty(cl))
					{
						chatLinks.AddRange(cl.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
					}
				}
			}
			bool footerEnabled = ShowCopyWaypointsButton;
			bool num = chatLinks.Count > 0;
			bool inCopyMode = num && footerEnabled && _copyModeListIds.Contains(listId);
			if (!num || !footerEnabled)
			{
				_copyModeListIds.Remove(listId);
			}
			if (num && footerEnabled)
			{
				entry.FooterReserve = FooterReserveTotal;
				RenderFooter(entry, listId, chatLinks.Count, inCopyMode);
			}
			if (inCopyMode)
			{
				int copyTarget = RenderCopyMode(entry, listId, chatLinks) + 40 + entry.FooterReserve;
				copyTarget = Math.Min(WindowMaxHeight, Math.Max(WindowBaseHeight, copyTarget));
				entry.Window.MaxAllowedHeight = Math.Min(1200, copyTarget);
				ResizeWindowAndPanel(entry, copyTarget);
				return;
			}
			entry.Panel.set_CanScroll(true);
			int y = 0;
			bool num2 = string.Equals(SortEntriesFor(list), "GROUP_COMPLETED", StringComparison.OrdinalIgnoreCase);
			bool noCheckbox = list.IsLootBag;
			bool sectionHasContent = false;
			if (!num2)
			{
				for (int m = 0; m < total; m++)
				{
					RenderEntry(entry, listId, list, m, ref y, ref sectionHasContent, noCheckbox);
				}
			}
			else
			{
				for (int k = 0; k < total; k++)
				{
					EntryDto e2 = list.Entries[k];
					if (e2 != null && !e2.Completed && !e2.AutoCompleted)
					{
						RenderEntry(entry, listId, list, k, ref y, ref sectionHasContent, noCheckbox);
					}
				}
				int completedCount = 0;
				for (int j = 0; j < total; j++)
				{
					EntryDto e3 = list.Entries[j];
					if (e3 != null && (e3.Completed || e3.AutoCompleted))
					{
						completedCount++;
					}
				}
				if (completedCount > 0)
				{
					bool num3 = _completedSectionCollapsed.Contains(listId);
					string btnText = (num3 ? "Show completed (" : "Hide completed (") + completedCount + ")";
					y += 6;
					int btnWidth = ActionButtonWidth;
					int btnHeight = ActionButtonHeight;
					int btnX = Math.Max(0, (((Container)entry.Panel).get_ContentRegion().Width - btnWidth) / 2);
					string capturedListId = listId;
					ActionButton.Create((Container)(object)entry.Panel, btnText, new Point(btnX, y), btnWidth, btnHeight, UiScale, CurrentTheme, delegate
					{
						if (!_completedSectionCollapsed.Add(capturedListId))
						{
							_completedSectionCollapsed.Remove(capturedListId);
						}
						WriteCollapsedCompleted();
						RefreshListWindow(capturedListId);
					});
					y += btnHeight + 6;
					if (!num3)
					{
						sectionHasContent = false;
						for (int i = 0; i < total; i++)
						{
							EntryDto e4 = list.Entries[i];
							if (e4 != null && (e4.Completed || e4.AutoCompleted))
							{
								RenderEntry(entry, listId, list, i, ref y, ref sectionHasContent, noCheckbox);
							}
						}
					}
				}
			}
			int desiredWidth = WindowWidthFor(list);
			if (((Control)entry.Window).get_Size().X != desiredWidth)
			{
				entry.Window.SetWindowSize(desiredWidth, ((Control)entry.Window).get_Size().Y);
			}
			int contentBased = y + 10 + 40 + entry.FooterReserve;
			int fitHeight = Math.Min(WindowMaxHeight, Math.Max(WindowBaseHeight, contentBased));
			int dragCap = Math.Min(1200, Math.Max(WindowBaseHeight, contentBased));
			entry.Window.MaxAllowedHeight = dragCap;
			int targetHeight = (entry.Window.UserPreferredHeight.HasValue ? Math.Min(entry.Window.UserPreferredHeight.Value, dragCap) : fitHeight);
			ResizeWindowAndPanel(entry, targetHeight);
		}

		private static string TruncateTooltipText(string s)
		{
			if (string.IsNullOrEmpty(s) || s.Length <= 40)
			{
				return s;
			}
			return s.Substring(0, 39) + "…";
		}

		private void RenderEntry(ListWindowEntry entry, string listId, ListDto list, int index, ref int y, ref bool sectionHasContent, bool noCheckbox)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Expected O, but got Unknown
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			string key = EntryKey(listId, index);
			if (!_entryImages.TryGetValue(key, out var tex) || tex == null)
			{
				return;
			}
			if (sectionHasContent)
			{
				DrawDivider((Container)(object)entry.Panel, ref y, noCheckbox);
			}
			sectionHasContent = true;
			Point size = ScaledSize(tex);
			EntryDto entryDto = list.Entries[index];
			bool isPending = _pendingEntries.Contains(key);
			int imageX = ((!noCheckbox) ? 30 : 0);
			if (!noCheckbox)
			{
				bool autoChecked = entryDto?.AutoCompleted ?? false;
				bool isChecked = autoChecked || (entryDto?.Completed ?? false);
				Checkbox val = new Checkbox();
				((Control)val).set_Location(new Point(8, y + (size.Y - 22) / 2));
				((Control)val).set_Size(new Point(22, 22));
				val.set_Checked(isChecked);
				((Control)val).set_Enabled(!autoChecked && !isPending);
				((Control)val).set_Parent((Container)(object)entry.Panel);
				Checkbox checkbox = val;
				if (!autoChecked && !isPending)
				{
					string capturedListId = listId;
					int capturedIndex = index;
					checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
					{
						OnCheckboxToggled(checkbox, capturedListId, capturedIndex);
					});
				}
			}
			Image val2 = new Image(AsyncTexture2D.op_Implicit(tex));
			((Control)val2).set_Location(new Point(imageX, y));
			((Control)val2).set_Size(size);
			val2.set_Tint(isPending ? PendingTint : Color.get_White());
			((Control)val2).set_Parent((Container)(object)entry.Panel);
			Image image = val2;
			string url;
			if (_entryChatLinks.TryGetValue(key, out var chatLink) && !string.IsNullOrEmpty(chatLink))
			{
				((Control)image).set_BasicTooltipText(TruncateTooltipText("Click to copy: " + chatLink));
				string capturedLink = chatLink;
				((Control)image).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					CopyChatLinkToClipboard(capturedLink);
				});
			}
			else if (_entryLinks.TryGetValue(key, out url) && !string.IsNullOrEmpty(url))
			{
				((Control)image).set_BasicTooltipText(TruncateTooltipText("Click to open " + url));
				string capturedUrl = url;
				((Control)image).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					OpenUrlInBrowser(capturedUrl);
				});
			}
			AttachEntryContextMenu(image, listId, index, entryDto);
			if (entryDto != null && entryDto.HasHoverCard)
			{
				HoverCard.Attach((Control)(object)image, listId, index);
			}
			if (isPending)
			{
				LoadingSpinner val3 = new LoadingSpinner();
				((Control)val3).set_Location(new Point(imageX + (size.X - 32) / 2, y + (size.Y - 32) / 2));
				((Control)val3).set_Size(new Point(32, 32));
				((Control)val3).set_Parent((Container)(object)entry.Panel);
			}
			y += size.Y;
		}

		private int DividerRightX(bool noCheckbox)
		{
			return ((!noCheckbox) ? 30 : 0) + DisplayWidth;
		}

		private void DrawDivider(Container parent, ref int y, bool noCheckbox)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			EnsureDividerTexture();
			Image val = new Image(AsyncTexture2D.op_Implicit(_dividerTexture));
			((Control)val).set_Location(new Point(0, y));
			((Control)val).set_Size(new Point(DividerRightX(noCheckbox), 1));
			((Control)val).set_Parent(parent);
			y++;
		}

		private void EnsureDividerTexture()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (_dividerTexture == null)
			{
				GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					_dividerTexture = new Texture2D(((GraphicsDeviceContext)(ref gdc)).get_GraphicsDevice(), 1, 1);
					_dividerTexture.SetData<Color>((Color[])(object)new Color[1] { DividerColor });
				}
				finally
				{
					((GraphicsDeviceContext)(ref gdc)).Dispose();
				}
			}
		}

		private static string SortEntriesFor(ListDto list)
		{
			try
			{
				return list?.Settings?["sortEntries"]?.Value<string>() ?? "STATIC";
			}
			catch
			{
				return "STATIC";
			}
		}

		private static void ResizeWindowAndPanel(ListWindowEntry entry, int newHeight)
		{
			entry.Window.SetWindowHeight(newHeight);
			ResizePanelToWindow(entry);
		}

		private static void ResizePanelToWindow(ListWindowEntry entry)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			if (entry.Panel != null)
			{
				int fullW = ((Container)entry.Window).get_ContentRegion().Width;
				int fullH = ((Container)entry.Window).get_ContentRegion().Height;
				int footer = Math.Max(0, entry.FooterReserve);
				int panelH = Math.Max(0, fullH - footer);
				((Control)entry.Panel).set_Size(new Point(fullW, panelH));
				((Control)entry.Panel).RecalculateLayout();
				object obj = _panelScrollbarField?.GetValue(entry.Panel);
				Scrollbar sb = (Scrollbar)((obj is Scrollbar) ? obj : null);
				if (sb != null)
				{
					((Control)sb).set_Height(((Container)entry.Panel).get_ContentRegion().Height - 20);
				}
				if (entry.FooterPanel != null)
				{
					((Control)entry.FooterPanel).set_Location(new Point(0, panelH));
					((Control)entry.FooterPanel).set_Size(new Point(fullW, footer));
				}
			}
		}

		private void RenderLoadingProgress(ListWindowEntry entry, int loadedCount, int total)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			int spinnerSize = (int)Math.Round(64f * UiScale);
			int windowHeight = 10 + spinnerSize + 18 + 20 + 40;
			((WindowBase2)entry.Window).set_CanResize(false);
			entry.Window.MaxAllowedHeight = windowHeight;
			ResizeWindowAndPanel(entry, windowHeight);
			int sx = Math.Max(0, (((Control)entry.Panel).get_Width() - spinnerSize) / 2);
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Location(new Point(sx, 10));
			((Control)val).set_Size(new Point(spinnerSize, spinnerSize));
			((Control)val).set_Parent((Container)(object)entry.Panel);
			string text = ((total > 0) ? ("Loading " + loadedCount + " / " + total) : "Loading...");
			Label val2 = new Label();
			val2.set_Text(text);
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(Color.get_LightGray());
			((Control)val2).set_Size(new Point(((Control)entry.Panel).get_Width(), 18));
			((Control)val2).set_Location(new Point(0, 10 + spinnerSize));
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val2).set_Parent((Container)(object)entry.Panel);
		}

		private void RenderLoadingFailed(ListWindowEntry entry, string listId)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			int btnW = ActionButtonWidth;
			int btnH = ActionButtonHeight;
			int windowHeight = 36 + btnH + 20 + 40;
			((WindowBase2)entry.Window).set_CanResize(false);
			entry.Window.MaxAllowedHeight = windowHeight;
			ResizeWindowAndPanel(entry, windowHeight);
			Label val = new Label();
			val.set_Text("Failed to load list");
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(Color.get_Red());
			((Control)val).set_Size(new Point(((Control)entry.Panel).get_Width(), 18));
			((Control)val).set_Location(new Point(0, 10));
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			val.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val).set_Parent((Container)(object)entry.Panel);
			int btnX = Math.Max(0, (((Container)entry.Panel).get_ContentRegion().Width - btnW) / 2);
			int btnY = 36;
			string capturedListId = listId;
			ActionButton.Create((Container)(object)entry.Panel, "Retry", new Point(btnX, btnY), btnW, btnH, UiScale, CurrentTheme, delegate
			{
				RetryLoadList(capturedListId);
			});
		}

		private void RetryLoadList(string listId)
		{
			_loadingFailures.Remove(listId);
			_pendingEntries.RemoveWhere((string k) => k.StartsWith(listId + ":"));
			string prefix = listId + ":";
			foreach (string i in _entryImages.Keys.Where((string k) => k.StartsWith(prefix)).ToList())
			{
				if (_entryImages.TryGetValue(i, out var tex))
				{
					try
					{
						if (tex != null)
						{
							((GraphicsResource)tex).Dispose();
						}
					}
					catch
					{
					}
				}
				_entryImages.Remove(i);
			}
			MarkLoading(listId);
			if (_lastSubscribedIds.Contains(listId))
			{
				HashSet<string> without = new HashSet<string>(_lastSubscribedIds);
				without.Remove(listId);
				SendSubscribeAsync(without.ToList());
				_lastSubscribedIds = without;
			}
			HashSet<string> withAgain = new HashSet<string>(_lastSubscribedIds) { listId };
			SendSubscribeAsync(withAgain.ToList());
			_lastSubscribedIds = withAgain;
			RefreshListWindow(listId);
		}

		private void RenderFooter(ListWindowEntry entry, string listId, int chatLinkCount, bool inCopyMode)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			if (entry.FooterPanel == null)
			{
				return;
			}
			string text = (inCopyMode ? "Back" : "Copy waypoints");
			int btnW = ActionButtonWidth;
			int btnH = ActionButtonHeight;
			int btnX = Math.Max(0, (((Control)entry.FooterPanel).get_Width() - btnW) / 2);
			int btnY = -4;
			string capturedListId = listId;
			ActionButton.Create((Container)(object)entry.FooterPanel, text, new Point(btnX, btnY), btnW, btnH, UiScale, CurrentTheme, delegate
			{
				if (_copyModeListIds.Contains(capturedListId))
				{
					_copyModeListIds.Remove(capturedListId);
				}
				else
				{
					_copyModeListIds.Add(capturedListId);
				}
				RefreshListWindow(capturedListId);
			});
		}

		private int RenderCopyMode(ListWindowEntry entry, string listId, List<string> chatLinks)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Expected O, but got Unknown
			entry.Panel.set_CanScroll(true);
			int max = _maxWaypointsPerCopy?.get_Value() ?? 15;
			max = Math.Max(1, Math.Min(15, max));
			int innerWidth = Math.Max(0, ((Control)entry.Panel).get_Width() - 20 - 25);
			int y = 20;
			Label val = new Label();
			val.set_Text(LabelForMaxWaypoints(max));
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(Color.get_LightGray());
			val.set_AutoSizeWidth(true);
			((Control)val).set_Location(new Point(20, y));
			((Control)val).set_Parent((Container)(object)entry.Panel);
			Label headerLabel = val;
			y += 18;
			TrackBar val2 = new TrackBar();
			val2.set_MinValue(1f);
			val2.set_MaxValue(15f);
			val2.set_Value((float)max);
			val2.set_SmallStep(true);
			((Control)val2).set_Width(innerWidth);
			((Control)val2).set_Height(16);
			((Control)val2).set_Location(new Point(20, y));
			((Control)val2).set_Parent((Container)(object)entry.Panel);
			TrackBar slider = val2;
			int chunksStartY = y + 26;
			List<StandardButton> chunkButtons = new List<StandardButton>();
			int result = RenderChunks(max);
			entry.RerenderCopyChunks = delegate
			{
				int val4 = _maxWaypointsPerCopy?.get_Value() ?? 15;
				val4 = Math.Max(1, Math.Min(15, val4));
				headerLabel.set_Text(LabelForMaxWaypoints(val4));
				int val5 = RenderChunks(val4) + 40 + entry.FooterReserve;
				val5 = Math.Min(WindowMaxHeight, Math.Max(WindowBaseHeight, val5));
				entry.Window.MaxAllowedHeight = Math.Min(1200, val5);
				ResizeWindowAndPanel(entry, val5);
			};
			slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
			{
				int num = Math.Max(1, Math.Min(15, (int)Math.Round(e.get_Value())));
				if (_maxWaypointsPerCopy != null && _maxWaypointsPerCopy.get_Value() != num)
				{
					_maxWaypointsPerCopy.set_Value(num);
				}
				entry.RerenderCopyChunks?.Invoke();
			});
			return result;
			int RenderChunks(int currentMax)
			{
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0110: Expected O, but got Unknown
				foreach (StandardButton b in chunkButtons)
				{
					try
					{
						((Control)b).Dispose();
					}
					catch
					{
					}
				}
				chunkButtons.Clear();
				int yy = chunksStartY;
				int idx = 1;
				foreach (List<string> chunk in ChunkChatLinks(chatLinks, currentMax))
				{
					string codes = string.Join(" ", chunk);
					string label = "Copy group " + idx + " (" + chunk.Count + ")";
					StandardButton val3 = new StandardButton();
					val3.set_Text(label);
					((Control)val3).set_Width(innerWidth);
					((Control)val3).set_Height(30);
					((Control)val3).set_Location(new Point(20, yy));
					((Control)val3).set_Parent((Container)(object)entry.Panel);
					StandardButton chunkBtn = val3;
					if (UiScale < 1f && _labelBaseFontField != null)
					{
						try
						{
							_labelBaseFontField.SetValue(chunkBtn, GameService.Content.get_DefaultFont12());
						}
						catch
						{
						}
						((Control)chunkBtn).Invalidate();
					}
					string capturedCodes = codes;
					((Control)chunkBtn).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						CopyChatLinkToClipboard(capturedCodes);
					});
					chunkButtons.Add(chunkBtn);
					yy += 34;
					idx++;
				}
				return yy + 20;
			}
		}

		private static string LabelForMaxWaypoints(int max)
		{
			return "Max waypoints per message: " + ((max == 15) ? "Max" : max.ToString());
		}

		private static List<List<string>> ChunkChatLinks(List<string> links, int maxPerMsg)
		{
			List<List<string>> result = new List<List<string>>();
			if (links == null || links.Count == 0)
			{
				return result;
			}
			List<string> current = new List<string>();
			string currentJoined = "";
			foreach (string link in links)
			{
				string sep = ((currentJoined.Length > 0) ? " " : "");
				string test = currentJoined + sep + link;
				if (!((maxPerMsg == 15) ? (test.Length > 199) : (current.Count >= maxPerMsg)))
				{
					current.Add(link);
					currentJoined = test;
					continue;
				}
				if (current.Count > 0)
				{
					result.Add(current);
				}
				current = new List<string> { link };
				currentJoined = link;
			}
			if (current.Count > 0)
			{
				result.Add(current);
			}
			return result;
		}

		private int WindowWidthFor(ListDto list)
		{
			if (list == null || !list.IsLootBag)
			{
				return WindowWidth;
			}
			return LootBagWindowWidth;
		}

		private string TitleFor(ListDto list)
		{
			int @base = (string.IsNullOrEmpty(ResetCountdownFor(list)) ? 12 : 10);
			float multiplier = UiScale * ((UiScale < 1f) ? 1.5f : 1f);
			return Truncate(list?.Name ?? list?.Id ?? "", Math.Max(1, (int)Math.Round((float)@base * multiplier)));
		}

		private int SubtitleCharBudget(int @base)
		{
			float multiplier = (1f + (UiScale - 1f) * 1.5f) * ((UiScale < 1f) ? 1.15f : 1f);
			return Math.Max(1, (int)Math.Round((float)@base * multiplier));
		}

		private static Color EmblemTintFor(ListDto list)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			string colorName = null;
			try
			{
				colorName = list?.Settings?["color"]?.Value<string>();
			}
			catch
			{
			}
			if (string.IsNullOrEmpty(colorName))
			{
				return Color.get_White();
			}
			return (Color)(colorName.ToLowerInvariant() switch
			{
				"pink" => new Color(194, 25, 93), 
				"orange" => new Color(176, 92, 32), 
				"yellow" => new Color(203, 139, 0), 
				"green" => new Color(28, 145, 65), 
				"blue" => new Color(88, 101, 242), 
				"purple" => new Color(143, 0, 254), 
				_ => Color.get_White(), 
			});
		}

		private string SubtitleFor(ListDto list)
		{
			if (!ShowAccountName)
			{
				return "";
			}
			string acct = null;
			try
			{
				acct = list?.Settings?["gw2AccountName"]?.Value<string>();
			}
			catch
			{
			}
			int @base = (string.IsNullOrEmpty(ResetCountdownFor(list)) ? 15 : 13);
			return TruncateAccountName(acct ?? "", SubtitleCharBudget(@base));
		}

		private static string ResetCountdownFor(ListDto list)
		{
			string reset = null;
			try
			{
				reset = list?.Settings?["reset"]?.Value<string>();
			}
			catch
			{
			}
			if (string.IsNullOrEmpty(reset))
			{
				return null;
			}
			DateTime now = DateTime.UtcNow;
			string text = reset.ToUpperInvariant();
			if (!(text == "DAILY"))
			{
				if (text == "WEEKLY")
				{
					DateTime resetAt = new DateTime(now.Year, now.Month, now.Day, 7, 30, 0, DateTimeKind.Utc);
					int dow = (int)resetAt.DayOfWeek;
					if (dow != 1 || resetAt <= now)
					{
						int dayDiff = (1 - dow + 7) % 7;
						if (dayDiff == 0)
						{
							dayDiff = 7;
						}
						resetAt = resetAt.AddDays(dayDiff);
					}
					TimeSpan span = resetAt - now;
					if (span.TotalDays < 1.0)
					{
						int nowMins2 = now.Hour * 60 + now.Minute;
						return FormatCountdownDuration((resetAt.Hour * 60 + resetAt.Minute - nowMins2 + 1440) % 1440);
					}
					int num = (int)Math.Floor(span.TotalHours);
					int days = num / 24;
					int hours = num % 24;
					if (hours <= 0)
					{
						return days + "d";
					}
					return days + "d " + hours + "h";
				}
				return null;
			}
			int nowMins = now.Hour * 60 + now.Minute;
			return FormatCountdownDuration((1440 - nowMins) % 1440);
		}

		private static string FormatCountdownDuration(int totalMinutes)
		{
			int hours = totalMinutes / 60;
			int mins = totalMinutes % 60;
			if (hours > 0 && mins > 0)
			{
				return hours + "h" + mins.ToString("D2");
			}
			if (hours > 0)
			{
				return hours + "h";
			}
			return mins + "m";
		}

		private static string Truncate(string s, int max)
		{
			if (string.IsNullOrEmpty(s))
			{
				return "";
			}
			if (s.Length <= max)
			{
				return s;
			}
			return s.Substring(0, Math.Max(0, max - 1)) + "…";
		}

		private static string TruncateAccountName(string s, int max)
		{
			if (string.IsNullOrEmpty(s) || s.Length <= max)
			{
				return s ?? "";
			}
			int dotIdx = s.LastIndexOf('.');
			if (dotIdx < 0)
			{
				return Truncate(s, max);
			}
			string suffix = s.Substring(dotIdx + 1);
			int allowedNameLen = max - 3 - suffix.Length;
			if (allowedNameLen <= 0)
			{
				return Truncate(s, max);
			}
			string namePart = s.Substring(0, dotIdx);
			if (namePart.Length <= allowedNameLen)
			{
				return s;
			}
			return namePart.Substring(0, allowedNameLen) + "..." + suffix;
		}

		private Point ScaledSize(Texture2D tex)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (tex == null || tex.get_Width() <= 0)
			{
				return new Point(DisplayWidth, 0);
			}
			int scaledHeight = (int)Math.Round((double)tex.get_Height() * (double)DisplayWidth / (double)tex.get_Width());
			return new Point(DisplayWidth, scaledHeight);
		}

		private void UpdateSubscriptions()
		{
			if (_unloading)
			{
				return;
			}
			HashSet<string> subs = new HashSet<string>();
			foreach (KeyValuePair<string, ListWindowEntry> kvp in _listWindows)
			{
				if (kvp.Value.Window != null && (((Control)kvp.Value.Window).get_Visible() || _peekHiddenIds.Contains(kvp.Key)))
				{
					subs.Add(kvp.Key);
				}
			}
			if (subs.SetEquals(_lastSubscribedIds))
			{
				return;
			}
			foreach (string id2 in subs)
			{
				if (!_lastSubscribedIds.Contains(id2))
				{
					MarkLoading(id2);
				}
			}
			foreach (string id in _lastSubscribedIds)
			{
				if (!subs.Contains(id))
				{
					MarkLoaded(id);
				}
			}
			_lastSubscribedIds = subs;
			SendSubscribeAsync(subs.ToList());
		}

		private void OnToggleListsKeybind(object sender, EventArgs e)
		{
			ToggleActiveListsVisibility();
		}

		private void ToggleActiveListsVisibility()
		{
			if (_peekHiddenIds.Count > 0)
			{
				RestorePeekedLists();
				return;
			}
			_suppressListVisibilityHandlers = true;
			try
			{
				foreach (KeyValuePair<string, ListWindowEntry> kvp in _listWindows)
				{
					if (kvp.Value.Window != null && ((Control)kvp.Value.Window).get_Visible())
					{
						_peekHiddenIds.Add(kvp.Key);
						((Control)kvp.Value.Window).set_Visible(false);
					}
				}
			}
			finally
			{
				_suppressListVisibilityHandlers = false;
			}
			_contextMenuRebuildPending = true;
		}

		private void RestorePeekedLists()
		{
			if (_peekHiddenIds.Count == 0)
			{
				return;
			}
			_suppressListVisibilityHandlers = true;
			try
			{
				foreach (string id in _peekHiddenIds)
				{
					if (_listWindows.TryGetValue(id, out var entry) && entry.Window != null)
					{
						((Control)entry.Window).set_Visible(true);
					}
				}
			}
			finally
			{
				_suppressListVisibilityHandlers = false;
			}
			_peekHiddenIds.Clear();
			_contextMenuRebuildPending = true;
		}

		private void RestorePersistedOpenLists()
		{
			if (_persistedOpenListsJson == null)
			{
				Logger.Info("Restore: persisted setting is null; skipping.");
				return;
			}
			if (_catalog?.Lists == null)
			{
				Logger.Info("Restore: catalog has no lists yet; skipping.");
				return;
			}
			string raw = _persistedOpenListsJson.get_Value() ?? "[]";
			List<string> persisted;
			try
			{
				persisted = JsonConvert.DeserializeObject<List<string>>(raw) ?? new List<string>();
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Restore: failed to parse persisted JSON (" + raw + ").");
				persisted = new List<string>();
			}
			HashSet<string> availableIds = new HashSet<string>(from l in _catalog.Lists
				where l != null && !string.IsNullOrEmpty(l.Id)
				select l.Id);
			Logger.Info("Restore: persisted=[" + string.Join(",", persisted) + "] available=[" + string.Join(",", availableIds) + "]");
			int opened = 0;
			foreach (string id in persisted)
			{
				if (availableIds.Contains(id) && !_listWindows.ContainsKey(id))
				{
					OpenListWindow(id);
					opened++;
				}
			}
			Logger.Info($"Restore: opened {opened} list window(s).");
		}

		private void OnCheckboxToggled(Checkbox checkbox, string listId, int index)
		{
			ListDto list = _catalog?.Lists?.FirstOrDefault((ListDto l) => l.Id == listId);
			if (list == null || !list.IsLootBag)
			{
				_pendingEntries.Add(EntryKey(listId, index));
				SendSetEntryCompletedAsync(listId, index, checkbox.get_Checked());
				RefreshListWindow(listId);
			}
		}

		private HashSet<string> LoadPersistedSet()
		{
			if (_persistedOpenListsJson == null)
			{
				return new HashSet<string>();
			}
			try
			{
				return new HashSet<string>(JsonConvert.DeserializeObject<List<string>>(_persistedOpenListsJson.get_Value() ?? "[]") ?? new List<string>());
			}
			catch
			{
				return new HashSet<string>();
			}
		}

		private void WritePersistedSet(HashSet<string> set)
		{
			if (_persistedOpenListsJson != null)
			{
				try
				{
					string json = JsonConvert.SerializeObject(set.ToList());
					_persistedOpenListsJson.set_Value(json);
					Logger.Info("Persisted open lists = " + json);
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to persist open lists.");
				}
			}
		}

		private void AddPersisted(string listId)
		{
			HashSet<string> set = LoadPersistedSet();
			if (set.Add(listId))
			{
				WritePersistedSet(set);
			}
		}

		private void RemovePersisted(string listId)
		{
			HashSet<string> set = LoadPersistedSet();
			if (set.Remove(listId))
			{
				WritePersistedSet(set);
			}
		}

		private void RestoreCollapsedCompleted()
		{
			if (_persistedCollapsedCompletedJson == null)
			{
				return;
			}
			string raw = _persistedCollapsedCompletedJson.get_Value() ?? "[]";
			try
			{
				foreach (string id in (JsonConvert.DeserializeObject<List<string>>(raw) ?? new List<string>())!)
				{
					if (!string.IsNullOrEmpty(id))
					{
						_completedSectionCollapsed.Add(id);
					}
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Restore: failed to parse collapsed sections (" + raw + ").");
			}
		}

		private void WriteCollapsedCompleted()
		{
			if (_persistedCollapsedCompletedJson != null)
			{
				try
				{
					_persistedCollapsedCompletedJson.set_Value(JsonConvert.SerializeObject(_completedSectionCollapsed.ToList()));
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to persist collapsed sections.");
				}
			}
		}

		private async void CopyChatLinkToClipboard(string chatLink)
		{
			if (string.IsNullOrEmpty(chatLink))
			{
				return;
			}
			try
			{
				if (!(await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(chatLink)))
				{
					Logger.Warn("Clipboard set returned false for chat link.");
				}
				else
				{
					ScreenNotification.ShowNotification("Copied! Paste into chat to use.", (NotificationType)0, (Texture2D)null, 4);
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to copy chat link.");
			}
		}

		private void AttachEntryContextMenu(Image image, string listId, int index, EntryDto entryDto)
		{
			if (image == null)
			{
				return;
			}
			ContextMenuStrip menu = null;
			string entryName = entryDto?.Name;
			if (!string.IsNullOrEmpty(entryName))
			{
				string capturedName = entryName;
				AddItem("Copy name", delegate
				{
					CopyNameToClipboard(capturedName);
				});
			}
			if (menu == null)
			{
				return;
			}
			((Control)image).set_Menu(menu);
			((Control)image).add_Disposed((EventHandler<EventArgs>)delegate
			{
				try
				{
					((Control)menu).Dispose();
				}
				catch
				{
				}
			});
			ContextMenuStripItem AddItem(string label, Action onClick)
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Expected O, but got Unknown
				if (menu == null)
				{
					menu = new ContextMenuStrip();
				}
				ContextMenuStripItem obj = menu.AddMenuItem(label);
				((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					onClick();
				});
				return obj;
			}
		}

		private async void CopyNameToClipboard(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			try
			{
				if (!(await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(name)))
				{
					Logger.Warn("Clipboard set returned false for entry name.");
				}
				else
				{
					ScreenNotification.ShowNotification("Name copied", (NotificationType)0, (Texture2D)null, 4);
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to copy entry name.");
			}
		}

		private void OpenUrlInBrowser(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return;
			}
			if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
			{
				Logger.Warn("Refusing to open non-web URL '" + url + "'");
				return;
			}
			if (uri.Scheme == Uri.UriSchemeHttp)
			{
				uri = new UriBuilder(uri)
				{
					Scheme = Uri.UriSchemeHttps,
					Port = -1
				}.Uri;
			}
			try
			{
				Process.Start(new ProcessStartInfo(uri.AbsoluteUri)
				{
					UseShellExecute = true
				});
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to open URL '" + uri.AbsoluteUri + "'.");
			}
		}

		private static void PremultiplyAlpha(Texture2D tex)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			if (tex == null)
			{
				return;
			}
			Color[] data = (Color[])(object)new Color[tex.get_Width() * tex.get_Height()];
			tex.GetData<Color>(data);
			for (int i = 0; i < data.Length; i++)
			{
				Color c = data[i];
				if (((Color)(ref c)).get_A() != byte.MaxValue)
				{
					if (((Color)(ref c)).get_A() == 0)
					{
						data[i] = Color.get_Transparent();
					}
					else
					{
						data[i] = new Color((byte)(((Color)(ref c)).get_R() * ((Color)(ref c)).get_A() / 255), (byte)(((Color)(ref c)).get_G() * ((Color)(ref c)).get_A() / 255), (byte)(((Color)(ref c)).get_B() * ((Color)(ref c)).get_A() / 255), ((Color)(ref c)).get_A());
					}
				}
			}
			tex.SetData<Color>(data);
		}

		private void StartHttpServer()
		{
			string prefix = $"http://+:{38473}/";
			_httpListener = new HttpListener();
			_httpListener.Prefixes.Add(prefix);
			try
			{
				_httpListener.Start();
			}
			catch (HttpListenerException e)
			{
				Logger.Info((Exception)e, "Could not bind " + prefix + "; listening on localhost only.");
				_httpListener = new HttpListener();
				_httpListener.Prefixes.Add($"http://localhost:{38473}/");
				_httpListener.Start();
			}
			_httpCts = new CancellationTokenSource();
			HttpListener listener = _httpListener;
			CancellationToken token = _httpCts.Token;
			Task.Run(() => HttpListenLoop(listener, token));
			Logger.Info($"GW2.app HTTP listener started on port {38473}");
		}

		private async Task HttpListenLoop(HttpListener listener, CancellationToken ct)
		{
			while (!ct.IsCancellationRequested && listener.IsListening)
			{
				HttpListenerContext ctx;
				try
				{
					ctx = await listener.GetContextAsync();
				}
				catch (Exception e)
				{
					if (!ct.IsCancellationRequested && listener.IsListening)
					{
						Logger.Warn(e, "Failed to accept an HTTP request; still listening.");
						await Task.Delay(250);
						continue;
					}
					return;
				}
				Task.Run(() => HandleHttpRequest(ctx));
			}
		}

		private void RestartHttpListener()
		{
			if (_unloading)
			{
				return;
			}
			DateTime now = DateTime.UtcNow;
			if (!(now - _lastListenerRestartUtc < ListenerRestartCooldown))
			{
				_lastListenerRestartUtc = now;
				Logger.Info("Recreating the HTTP listener to recover a wedged connection.");
				try
				{
					_httpCts?.Cancel();
				}
				catch
				{
				}
				try
				{
					_httpListener?.Stop();
				}
				catch
				{
				}
				try
				{
					_httpListener?.Close();
				}
				catch
				{
				}
				_httpListener = null;
				try
				{
					_httpCts?.Dispose();
				}
				catch
				{
				}
				_httpCts = null;
				try
				{
					StartHttpServer();
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to recreate the HTTP listener.");
				}
			}
		}

		private void EnsureHttpListenerAlive()
		{
			if (!_unloading && (_httpListener == null || !_httpListener.IsListening))
			{
				RestartHttpListener();
			}
		}

		private static bool IsWebSocketRequestSafe(HttpListenerContext ctx)
		{
			if (Volatile.Read(ref _wsDetectionState) == 2)
			{
				return false;
			}
			try
			{
				bool isWebSocketRequest = ctx.Request.IsWebSocketRequest;
				Volatile.Write(ref _wsDetectionState, 1);
				return isWebSocketRequest;
			}
			catch (Exception e)
			{
				if (Interlocked.Exchange(ref _wsDetectionState, 2) != 2)
				{
					Logger.Info(e, "WebSocket detection unavailable on this platform; serving HTTP polling only.");
				}
				return false;
			}
		}

		private static void CloseResponse(HttpListenerContext ctx, int status, string contentType = null, byte[] body = null)
		{
			try
			{
				ctx.Response.StatusCode = status;
				ctx.Response.KeepAlive = false;
				if (contentType != null)
				{
					ctx.Response.ContentType = contentType;
				}
				ctx.Response.ContentLength64 = ((body != null) ? body.Length : 0);
				if (body != null && body.Length != 0)
				{
					ctx.Response.OutputStream.Write(body, 0, body.Length);
				}
				ctx.Response.Close();
				Logger.Debug($"HTTP response {status} sent ({((body != null) ? body.Length : 0)} bytes).");
			}
			catch (Exception e)
			{
				Logger.Debug(e, $"Writing HTTP response {status} ({((body != null) ? body.Length : 0)} bytes) failed.");
				try
				{
					ctx.Response.Abort();
				}
				catch
				{
				}
			}
		}

		private async Task HandleHttpRequest(HttpListenerContext ctx)
		{
			Logger.Debug("HTTP " + ctx.Request.HttpMethod + " " + ctx.Request.Url?.AbsolutePath + " " + $"len={ctx.Request.ContentLength64} from {ctx.Request.RemoteEndPoint}");
			try
			{
				if (IsWebSocketRequestSafe(ctx))
				{
					string wsOrigin = ctx.Request.Headers["Origin"];
					if (IsAllowedOrigin(wsOrigin))
					{
						await HandleWebSocket(ctx);
						return;
					}
					Logger.Warn($"Rejecting WS handshake from disallowed origin '{wsOrigin}' ({ctx.Request.RemoteEndPoint})");
					CloseResponse(ctx, 403);
					return;
				}
				ApplyCorsHeaders(ctx);
				if (ctx.Request.HttpMethod == "OPTIONS")
				{
					CloseResponse(ctx, 204);
				}
				else if (ctx.Request.HttpMethod == "POST" && ctx.Request.Url.AbsolutePath == "/poll")
				{
					string pollOrigin = ctx.Request.Headers["Origin"];
					if (IsAllowedOrigin(pollOrigin))
					{
						await HandlePoll(ctx);
						return;
					}
					Logger.Warn($"Rejecting poll from disallowed origin '{pollOrigin}' ({ctx.Request.RemoteEndPoint})");
					CloseResponse(ctx, 403);
				}
				else
				{
					CloseResponse(ctx, 426, "text/plain; charset=utf-8", Encoding.UTF8.GetBytes("This endpoint expects a WebSocket connection."));
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Error handling HTTP request.");
				CloseResponse(ctx, 500);
			}
		}

		private static void ApplyCorsHeaders(HttpListenerContext ctx)
		{
			string origin = ctx.Request.Headers["Origin"];
			if (IsAllowedOrigin(origin))
			{
				ctx.Response.Headers["Access-Control-Allow-Origin"] = origin;
				ctx.Response.Headers["Vary"] = "Origin";
			}
			ctx.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, OPTIONS";
			ctx.Response.Headers["Access-Control-Allow-Headers"] = "Upgrade, Connection, Content-Type";
			ctx.Response.Headers["Access-Control-Allow-Private-Network"] = "true";
			ctx.Response.Headers["Access-Control-Max-Age"] = "86400";
		}

		private static bool IsAllowedOrigin(string origin)
		{
			if (string.IsNullOrEmpty(origin))
			{
				return false;
			}
			if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
			{
				return false;
			}
			string host = uri.Host;
			if (string.IsNullOrEmpty(host))
			{
				return false;
			}
			if (host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (host.Equals("gw2.app", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (host.EndsWith(".gw2.app", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (IPAddress.TryParse(host, out var ip) && IsLoopbackOrPrivateIp(ip))
			{
				return true;
			}
			return false;
		}

		private static bool IsLoopbackOrPrivateIp(IPAddress ip)
		{
			if (IPAddress.IsLoopback(ip))
			{
				return true;
			}
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				byte[] b = ip.GetAddressBytes();
				if (b[0] == 10)
				{
					return true;
				}
				if (b[0] == 172 && b[1] >= 16 && b[1] <= 31)
				{
					return true;
				}
				if (b[0] == 192 && b[1] == 168)
				{
					return true;
				}
				if (b[0] == 169 && b[1] == 254)
				{
					return true;
				}
				return false;
			}
			if (ip.AddressFamily == AddressFamily.InterNetworkV6)
			{
				if (ip.IsIPv6LinkLocal)
				{
					return true;
				}
				if ((ip.GetAddressBytes()[0] & 0xFE) == 252)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		private async Task HandleWebSocket(HttpListenerContext ctx)
		{
			HttpListenerWebSocketContext wsCtx;
			try
			{
				wsCtx = await ctx.AcceptWebSocketAsync(null);
			}
			catch (Exception e3)
			{
				Logger.Warn(e3, "WS accept failed.");
				return;
			}
			WebSocket ws = wsCtx.WebSocket;
			IPEndPoint remote = ctx.Request.RemoteEndPoint;
			Logger.Info($"WS client connected from {remote}");
			WebSocket previous;
			CancellationTokenSource previousCts;
			PollChannel previousPoll;
			lock (_clientLock)
			{
				previous = _activeClient;
				previousCts = _activeClientCts;
				previousPoll = _activePollSession;
				_activeClient = ws;
				_activeClientCts = new CancellationTokenSource();
				_activePollSession = null;
				if (previousPoll != null)
				{
					_lastSupersededPollId = previousPoll.SessionId;
				}
			}
			previousPoll?.MarkSuperseded();
			if (previous != null || previousPoll != null)
			{
				Logger.Info("Superseding previous " + ((previous != null) ? "WS" : "poll") + " client.");
				_incomingMessages.Enqueue(new IncomingMessage
				{
					Kind = MessageKind.ClientReplaced
				});
				if (previous != null)
				{
					SupersedePreviousAsync(previous, previousCts);
				}
			}
			Interlocked.Exchange(ref _hasActiveConnection, 1);
			Interlocked.Exchange(ref _connectionStateDirty, 1);
			TaskCompletionSource<bool> handshakeReceived = new TaskCompletionSource<bool>();
			CancellationTokenSource localCts = new CancellationTokenSource();
			CancellationTokenSource clientCts;
			lock (_clientLock)
			{
				clientCts = _activeClientCts;
			}
			Task.Run(async delegate
			{
				_ = 1;
				try
				{
					if (await Task.WhenAny(handshakeReceived.Task, Task.Delay(5000, localCts.Token)) != handshakeReceived.Task)
					{
						Logger.Info($"Handshake timeout; closing WS from {remote}");
						await CloseWsAsync(ws, 4001, "handshake timeout");
					}
				}
				catch
				{
				}
			});
			byte[] buffer = new byte[65536];
			bool stateSeen = false;
			try
			{
				while (ws.State == WebSocketState.Open && !clientCts.IsCancellationRequested)
				{
					using MemoryStream ms = new MemoryStream();
					WebSocketReceiveResult result;
					do
					{
						result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), clientCts.Token);
						if (result.MessageType == WebSocketMessageType.Close)
						{
							try
							{
								await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
							}
							catch
							{
							}
							return;
						}
						ms.Write(buffer, 0, result.Count);
					}
					while (!result.EndOfMessage);
					if (result.MessageType != 0)
					{
						Logger.Warn($"Unexpected non-text WS frame from {remote}; closing.");
						await CloseWsAsync(ws, 4002, "expected text frames");
						return;
					}
					string text = Encoding.UTF8.GetString(ms.ToArray());
					IncomingMessage parsed2;
					try
					{
						parsed2 = ParseMessage(text);
						if (!stateSeen && parsed2.Kind != 0)
						{
							throw new ProtocolException("first message must be 'state'");
						}
					}
					catch (ProtocolException pe)
					{
						Logger.Warn((Exception)pe, $"Protocol violation from {remote}.");
						await CloseWsAsync(ws, 4002, pe.Message);
						return;
					}
					catch (Exception e2)
					{
						Logger.Warn(e2, $"Failed to parse WS message from {remote}.");
						await CloseWsAsync(ws, 4002, "bad json");
						return;
					}
					if (parsed2.Kind == MessageKind.State && !stateSeen)
					{
						stateSeen = true;
						handshakeReceived.TrySetResult(result: true);
					}
					bool stillActive;
					lock (_clientLock)
					{
						stillActive = _activeClient == ws;
					}
					if (!stillActive)
					{
						return;
					}
					_incomingMessages.Enqueue(parsed2);
					parsed2 = null;
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception e)
			{
				Logger.Info(e, $"WS connection from {remote} ended.");
			}
			finally
			{
				handshakeReceived.TrySetResult(result: false);
				try
				{
					localCts.Cancel();
				}
				catch
				{
				}
				localCts.Dispose();
				bool wasActive = false;
				lock (_clientLock)
				{
					if (_activeClient == ws)
					{
						_activeClient = null;
						try
						{
							_activeClientCts?.Dispose();
						}
						catch
						{
						}
						_activeClientCts = null;
						wasActive = true;
					}
				}
				if (wasActive)
				{
					Interlocked.Exchange(ref _hasActiveConnection, 0);
					Interlocked.Exchange(ref _connectionStateDirty, 1);
					_incomingMessages.Enqueue(new IncomingMessage
					{
						Kind = MessageKind.ConnectionLost
					});
					_lastSubscribedIds = new HashSet<string>();
					_restoredFromPersistence = false;
				}
				try
				{
					ws.Dispose();
				}
				catch
				{
				}
				Logger.Info($"WS from {remote} closed.");
			}
		}

		private async Task SupersedePreviousAsync(WebSocket previous, CancellationTokenSource previousCts)
		{
			try
			{
				using CancellationTokenSource sendCts = new CancellationTokenSource(TimeSpan.FromSeconds(2.0));
				await previous.CloseOutputAsync((WebSocketCloseStatus)4000, "superseded", sendCts.Token);
			}
			catch (Exception e)
			{
				Logger.Info(e, "Sending close frame to superseded client failed.");
			}
			Task.Delay(TimeSpan.FromSeconds(10.0)).ContinueWith(delegate
			{
				try
				{
					previousCts?.Cancel();
				}
				catch
				{
				}
				try
				{
					previousCts?.Dispose();
				}
				catch
				{
				}
				try
				{
					previous.Dispose();
				}
				catch
				{
				}
			});
		}

		private static async Task CloseWsAsync(WebSocket ws, int code, string reason)
		{
			try
			{
				using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(2.0));
				await ws.CloseAsync((WebSocketCloseStatus)code, reason ?? "", cts.Token);
			}
			catch
			{
			}
			try
			{
				ws.Dispose();
			}
			catch
			{
			}
		}

		private async Task<bool> SendToClientAsync(string json)
		{
			WebSocket ws;
			PollChannel poll;
			lock (_clientLock)
			{
				ws = _activeClient;
				poll = _activePollSession;
			}
			if (poll != null && !poll.Superseded)
			{
				poll.Enqueue(json);
				return true;
			}
			if (ws != null && ws.State == WebSocketState.Open)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(json);
				try
				{
					await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
					return true;
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to send to client.");
				}
			}
			return false;
		}

		private async Task SendSubscribeAsync(List<string> listIds)
		{
			SubscribeMessage payload = new SubscribeMessage
			{
				Type = "subscribe",
				ListIds = (listIds ?? new List<string>()),
				Module = "blish"
			};
			if (await SendToClientAsync(JsonConvert.SerializeObject(payload)))
			{
				Logger.Info($"Sent subscribe with {listIds?.Count ?? 0} list ids.");
			}
		}

		private async Task SendOpenHoverAsync(string listId, int index)
		{
			OpenHoverMessage payload = new OpenHoverMessage
			{
				Type = "open_hover",
				ListId = listId,
				Index = index
			};
			await SendToClientAsync(JsonConvert.SerializeObject(payload));
		}

		private async Task SendCloseHoverAsync()
		{
			CloseHoverMessage payload = new CloseHoverMessage
			{
				Type = "close_hover"
			};
			await SendToClientAsync(JsonConvert.SerializeObject(payload));
		}

		private async Task SendSetEntryCompletedAsync(string listId, int index, bool completed)
		{
			SetEntryCompletedMessage payload = new SetEntryCompletedMessage
			{
				Type = "set_entry_completed",
				ListId = listId,
				Index = index,
				Completed = completed
			};
			if (await SendToClientAsync(JsonConvert.SerializeObject(payload)))
			{
				Logger.Info($"Sent set_entry_completed listId={listId} index={index} completed={completed}");
			}
		}

		private static IncomingMessage ParseMessage(string text)
		{
			JObject root;
			try
			{
				root = JObject.Parse(text);
			}
			catch (Exception e)
			{
				throw new ProtocolException("invalid json: " + e.Message);
			}
			string type = (root["type"] ?? throw new ProtocolException("missing 'type' field")).Value<string>();
			switch (type)
			{
			case "state":
			{
				int proto = (root["protocol"] ?? throw new ProtocolException("state missing 'protocol' field")).Value<int>();
				if (proto < 1 || proto > 2)
				{
					throw new ProtocolException($"unsupported protocol version {proto}");
				}
				StateMessage state = root.ToObject<StateMessage>() ?? new StateMessage();
				return new IncomingMessage
				{
					Kind = MessageKind.State,
					State = state
				};
			}
			case "entry":
			{
				EntryMessage entry = root.ToObject<EntryMessage>();
				if (entry == null || string.IsNullOrEmpty(entry.ListId))
				{
					throw new ProtocolException("entry missing listId");
				}
				return new IncomingMessage
				{
					Kind = MessageKind.Entry,
					Entry = entry
				};
			}
			case "synced":
			{
				List<string> ids = root["listIds"]?.ToObject<List<string>>() ?? new List<string>();
				return new IncomingMessage
				{
					Kind = MessageKind.Synced,
					SyncedListIds = ids
				};
			}
			case "hover_image":
			{
				HoverImageMessage hi = root.ToObject<HoverImageMessage>();
				if (hi == null || string.IsNullOrEmpty(hi.ListId))
				{
					throw new ProtocolException("hover_image missing listId");
				}
				return new IncomingMessage
				{
					Kind = MessageKind.HoverImage,
					HoverImage = hi
				};
			}
			default:
				throw new ProtocolException("unknown message type '" + type + "'");
			}
		}

		private async Task HandlePoll(HttpListenerContext ctx)
		{
			string body;
			try
			{
				using StreamReader reader = new StreamReader(ctx.Request.InputStream, ctx.Request.ContentEncoding ?? Encoding.UTF8);
				body = await reader.ReadToEndAsync();
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to read poll body.");
				CloseResponse(ctx, 400);
				return;
			}
			string session;
			JArray inbound;
			JToken closeTok;
			try
			{
				JObject jObject = JObject.Parse(body);
				session = jObject["session"]?.Value<string>();
				inbound = (jObject["messages"] as JArray) ?? new JArray();
				closeTok = jObject["close"];
			}
			catch (Exception e3)
			{
				Logger.Warn(e3, "Bad poll request JSON.");
				CloseResponse(ctx, 400);
				return;
			}
			if (string.IsNullOrEmpty(session))
			{
				CloseResponse(ctx, 400);
				return;
			}
			if (closeTok != null && closeTok.Type != JTokenType.Null)
			{
				bool cleared = false;
				lock (_clientLock)
				{
					if (_activePollSession != null && _activePollSession.SessionId == session)
					{
						_activePollSession.MarkSuperseded();
						_activePollSession = null;
						_lastSupersededPollId = session;
						cleared = true;
					}
				}
				if (cleared)
				{
					MarkPollDisconnected();
				}
				WritePollResponse(ctx, null, null);
				return;
			}
			PollChannel channel = null;
			bool returnSuperseded = false;
			bool replacedPrevious = false;
			bool resync = false;
			WebSocket supersededWs = null;
			CancellationTokenSource supersededWsCts = null;
			lock (_clientLock)
			{
				if (_activePollSession != null && _activePollSession.SessionId == session)
				{
					channel = _activePollSession;
					channel.LastPollUtc = DateTime.UtcNow;
				}
				else if (session == _lastSupersededPollId)
				{
					returnSuperseded = true;
				}
				else
				{
					replacedPrevious = _activeClient != null || _activePollSession != null;
					resync = true;
					supersededWs = _activeClient;
					supersededWsCts = _activeClientCts;
					_activeClient = null;
					_activeClientCts = null;
					if (_activePollSession != null)
					{
						_activePollSession.MarkSuperseded();
						_lastSupersededPollId = _activePollSession.SessionId;
					}
					channel = (_activePollSession = new PollChannel(session));
					Interlocked.Exchange(ref _hasActiveConnection, 1);
					Interlocked.Exchange(ref _connectionStateDirty, 1);
				}
			}
			if (returnSuperseded)
			{
				WritePollResponse(ctx, null, MakeClose(4000, "superseded"));
				return;
			}
			if (replacedPrevious)
			{
				_incomingMessages.Enqueue(new IncomingMessage
				{
					Kind = MessageKind.ClientReplaced
				});
			}
			if (supersededWs != null)
			{
				SupersedePreviousAsync(supersededWs, supersededWsCts);
			}
			bool superseded = false;
			foreach (JToken tok in inbound)
			{
				JObject fobj = tok as JObject;
				string messageJson;
				if (fobj != null && fobj["__frag"] != null)
				{
					string reassembled = channel.AcceptFragment(fobj);
					if (reassembled == null)
					{
						continue;
					}
					messageJson = reassembled;
				}
				else
				{
					messageJson = tok.ToString(Formatting.None);
				}
				IncomingMessage parsed;
				try
				{
					parsed = ParseMessage(messageJson);
				}
				catch (Exception e2)
				{
					Logger.Warn(e2, "Skipping bad poll message.");
					continue;
				}
				if (!channel.StateSeen)
				{
					if (parsed.Kind != 0)
					{
						continue;
					}
					channel.StateSeen = true;
				}
				lock (_clientLock)
				{
					superseded = _activePollSession != channel;
				}
				if (!superseded)
				{
					_incomingMessages.Enqueue(parsed);
					continue;
				}
				break;
			}
			List<string> outMsgs = channel.DrainOutbound();
			WritePollResponse(ctx, outMsgs, superseded ? MakeClose(4000, "superseded") : null, resync);
		}

		private static JObject MakeClose(int code, string reason)
		{
			return new JObject
			{
				["code"] = (JToken)code,
				["reason"] = (JToken)reason
			};
		}

		private static void WritePollResponse(HttpListenerContext ctx, List<string> messages, JObject close, bool resync = false)
		{
			JArray arr = new JArray();
			if (messages != null)
			{
				foreach (string s in messages)
				{
					try
					{
						arr.Add(JToken.Parse(s));
					}
					catch
					{
					}
				}
			}
			JObject root = new JObject
			{
				["messages"] = arr,
				["close"] = (JToken?)(((object)close) ?? ((object)JValue.CreateNull()))
			};
			root["serverProtocol"] = (JToken)2;
			root["module"] = (JToken)"blish";
			if (resync)
			{
				root["resync"] = (JToken)true;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(root.ToString(Formatting.None));
			CloseResponse(ctx, 200, "application/json", bytes);
		}

		private void MarkPollDisconnected()
		{
			Interlocked.Exchange(ref _hasActiveConnection, 0);
			Interlocked.Exchange(ref _connectionStateDirty, 1);
			_incomingMessages.Enqueue(new IncomingMessage
			{
				Kind = MessageKind.ConnectionLost
			});
			_lastSubscribedIds = new HashSet<string>();
			_restoredFromPersistence = false;
		}

		private void ReapStalePollSession()
		{
			bool reaped = false;
			lock (_clientLock)
			{
				if (_activePollSession != null && DateTime.UtcNow - _activePollSession.LastPollUtc > PollSessionTimeout)
				{
					Logger.Info("Poll session timed out; treating as disconnected.");
					_activePollSession.MarkSuperseded();
					_activePollSession = null;
					reaped = true;
				}
			}
			if (reaped)
			{
				MarkPollDisconnected();
				RestartHttpListener();
			}
		}
	}
}
