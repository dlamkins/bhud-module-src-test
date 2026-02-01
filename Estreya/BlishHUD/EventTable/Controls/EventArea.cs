using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Services;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest;
using Estreya.BlishHUD.Shared.Models.GameIntegration.Chat;
using Estreya.BlishHUD.Shared.MumbleInfo.Map;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Services.GameIntegration;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Threading.Events;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Extensions;
using SemVer;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class EventArea : RenderTarget2DControl
	{
		private const int MIN_HEIGHT = 1;

		private readonly Logger _logger = Logger.GetLogger<EventArea>();

		private static TimeSpan _updateFillersInterval = TimeSpan.FromMinutes(15.0);

		private static TimeSpan _checkForNewEventsInterval = TimeSpan.FromMilliseconds(1000.0);

		private BitmapFont _defaultFont;

		private ConcurrentDictionary<FontSize, BitmapFont> _fonts = new ConcurrentDictionary<FontSize, BitmapFont>();

		private readonly Func<string> _getAccessToken;

		private readonly Func<List<string>> _getAreaNames;

		private readonly Func<List<string>> _getDisabledReminderKeys;

		private ContentsManager _contentsManager;

		private readonly Func<Instant> _getNowAction;

		private readonly Func<Version> _getVersion;

		private Event _activeEvent;

		private List<EventCategory> _allEvents = new List<EventCategory>();

		private string _apiRootUrl;

		private readonly Func<JsonSerializer> _getSerializer;

		private Task _updateFillerTask;

		private bool _receiving;

		private bool _clearing;

		private readonly ConcurrentDictionary<string, List<(Instant Occurence, Event Event)>> _controlEvents = new ConcurrentDictionary<string, List<(Instant, Event)>>();

		private readonly AsyncLock _controlLock = new AsyncLock();

		private int _drawXOffset;

		private int _drawYOffset;

		private List<string> _eventCategoryOrdering;

		private readonly AsyncLock _eventLock = new AsyncLock();

		private EventStateService _eventStateService;

		private IFlurlClient _flurlClient;

		private int _heightFromLastDraw = 1;

		private IconService _iconService;

		private Event _lastActiveEvent;

		private double _lastCheckForNewEventsUpdate;

		private readonly AsyncRef<double> _lastFillerUpdate = new AsyncRef<double>(0.0);

		private MapchestService _mapchestService;

		private MapUtil _mapUtil;

		private List<List<(Instant Occurence, Event Event)>> _orderedControlEvents;

		private PointOfInterestService _pointOfInterestService;

		private AccountService _accountService;

		private readonly ChatService _chatService;

		private TimeSpan? _savedDrawInterval;

		private int _tempHistorySplit = -1;

		private TranslationService _translationService;

		private WorldbossService _worldbossService;

		public bool IsTogglingCompactMode { get; set; }

		private int DrawXOffset
		{
			get
			{
				if (!Configuration.ShowCategoryNames.get_Value())
				{
					return 0;
				}
				return _drawXOffset;
			}
			set
			{
				_drawXOffset = value;
			}
		}

		private int DrawYOffset
		{
			get
			{
				if (!Configuration.ShowTopTimeline.get_Value())
				{
					return 0;
				}
				return _drawYOffset;
			}
			set
			{
				_drawYOffset = value;
			}
		}

		private List<string> EventCategoryOrdering
		{
			get
			{
				if (_eventCategoryOrdering == null)
				{
					_eventCategoryOrdering = GetEventCategoryOrdering();
				}
				return _eventCategoryOrdering;
			}
		}

		private List<List<(Instant Occurence, Event Event)>> OrderedControlEvents
		{
			get
			{
				List<string> order = EventCategoryOrdering;
				using (_controlLock.Lock())
				{
					if (_orderedControlEvents == null)
					{
						_orderedControlEvents = (from x in _controlEvents
							orderby order.IndexOf(x.Key)
							select x.Value).ToList();
					}
				}
				return _orderedControlEvents;
			}
		}

		public bool Enabled => Configuration?.Enabled.get_Value() ?? false;

		private double PixelPerMinute => (double)GetWidth() / (double)Configuration.TimeSpan.get_Value();

		public EventAreaConfiguration Configuration { get; private set; }

		public event EventHandler<(string EventSettingKey, string DestinationArea)> MoveToAreaClicked;

		public event EventHandler<(string EventSettingKey, string DestinationArea)> CopyToAreaClicked;

		public event EventHandler<string> EnableReminderClicked;

		public event EventHandler<string> DisableReminderClicked;

		public event AsyncEventHandler CompactModeToggled;

		public EventArea(EventAreaConfiguration configuration, IconService iconService, TranslationService translationService, EventStateService eventService, WorldbossService worldbossService, MapchestService mapchestService, PointOfInterestService pointOfInterestService, AccountService accountService, ChatService chatService, MapUtil mapUtil, IFlurlClient flurlClient, Func<JsonSerializer> getSerializer, string apiRootUrl, Func<Instant> getNowAction, Func<Version> getVersion, Func<string> getAccessToken, Func<List<string>> getAreaNames, Func<List<string>> getDisabledReminderKeys, ContentsManager contentsManager)
		{
			Configuration = configuration;
			Configuration.EnabledKeybinding.get_Value().add_Activated((EventHandler<EventArgs>)EnabledKeybinding_Activated);
			Configuration.Size.X.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)Size_SettingChanged);
			Configuration.Size.Y.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)Size_SettingChanged);
			Configuration.Location.X.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)Location_SettingChanged);
			Configuration.Location.Y.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)Location_SettingChanged);
			Configuration.TimeSpan.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)TimeSpan_SettingChanged);
			Configuration.Opacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)Opacity_SettingChanged);
			Configuration.BackgroundColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)BackgroundColor_SettingChanged);
			Configuration.UseFiller.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UseFiller_SettingChanged);
			Configuration.BuildDirection.add_SettingChanged((EventHandler<ValueChangedEventArgs<BuildDirection>>)BuildDirection_SettingChanged);
			Configuration.DisabledEventKeys.add_SettingChanged((EventHandler<ValueChangedEventArgs<List<string>>>)DisabledEventKeys_SettingChanged);
			Configuration.EventOrder.add_SettingChanged((EventHandler<ValueChangedEventArgs<List<string>>>)EventOrder_SettingChanged);
			Configuration.DrawInterval.add_SettingChanged((EventHandler<ValueChangedEventArgs<DrawInterval>>)DrawInterval_SettingChanged);
			Configuration.LimitToCurrentMap.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)LimitToCurrentMap_SettingChanged);
			Configuration.AllowUnspecifiedMap.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AllowUnspecifiedMap_SettingChanged);
			Configuration.FontFace.add_SettingChanged((EventHandler<ValueChangedEventArgs<FontFace>>)FontFace_SettingChanged);
			Configuration.CustomFontPath.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)CustomFontPath_SettingChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			Configuration.CompactMode.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)CompactMode_SettingChanged);
			((Control)this).add_Click((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)OnMouseLeft);
			((Control)this).add_MouseWheelScrolled((EventHandler<MouseEventArgs>)OnMouseWheelScrolled);
			Location_SettingChanged(this, null);
			Size_SettingChanged(this, null);
			Opacity_SettingChanged(this, new ValueChangedEventArgs<float>(0f, Configuration.Opacity.get_Value()));
			BackgroundColor_SettingChanged(this, new ValueChangedEventArgs<Color>((Color)null, Configuration.BackgroundColor.get_Value()));
			DrawInterval_SettingChanged(this, new ValueChangedEventArgs<DrawInterval>(Estreya.BlishHUD.EventTable.Models.DrawInterval.INSTANT, Configuration.DrawInterval.get_Value()));
			_getNowAction = getNowAction;
			_getVersion = getVersion;
			_getAccessToken = getAccessToken;
			_getAreaNames = getAreaNames;
			_getDisabledReminderKeys = getDisabledReminderKeys;
			_contentsManager = contentsManager;
			_iconService = iconService;
			_translationService = translationService;
			_eventStateService = eventService;
			_worldbossService = worldbossService;
			_mapchestService = mapchestService;
			_pointOfInterestService = pointOfInterestService;
			_accountService = accountService;
			_chatService = chatService;
			_mapUtil = mapUtil;
			_flurlClient = flurlClient;
			_getSerializer = getSerializer;
			_apiRootUrl = apiRootUrl;
			using Stream defaultFontStream = _contentsManager.GetFileStream("fonts\\Menomonia.ttf") ?? throw new FileNotFoundException("Memonia Font is not included in module ref folder.");
			_defaultFont = FontUtils.FromTrueTypeFont(defaultFontStream.ToByteArray(), 18f, 256, 256).ToBitmapFont();
			if (_worldbossService != null)
			{
				_worldbossService.WorldbossCompleted += Event_Completed;
				_worldbossService.WorldbossRemoved += Event_Removed;
			}
			if (_mapchestService != null)
			{
				_mapchestService.MapchestCompleted += Event_Completed;
				_mapchestService.MapchestRemoved += Event_Removed;
			}
			if (_eventStateService != null)
			{
				_eventStateService.StateAdded += EventService_ServiceAdded;
				_eventStateService.StateRemoved += EventService_ServiceRemoved;
			}
		}

		private void CompactMode_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			this.CompactModeToggled?.Invoke(this);
		}

		private void OnMouseWheelScrolled(object sender, MouseEventArgs e)
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (!Configuration.EnableHistorySplitScrolling.get_Value())
			{
				return;
			}
			if (_tempHistorySplit == -1)
			{
				_tempHistorySplit = Configuration.HistorySplit.get_Value();
			}
			TimeSpan valueOrDefault = _savedDrawInterval.GetValueOrDefault();
			if (!_savedDrawInterval.HasValue)
			{
				valueOrDefault = base.DrawInterval;
				_savedDrawInterval = valueOrDefault;
			}
			base.DrawInterval = TimeSpan.FromMilliseconds(0.0);
			MouseState state = GameService.Input.get_Mouse().get_State();
			int scrollDistance = ((MouseState)(ref state)).get_ScrollWheelValue();
			int scrollValue = Configuration.HistorySplitScrollingSpeed.get_Value() * ((scrollDistance >= 0) ? 1 : (-1));
			(float, float)? range = Configuration.HistorySplit.GetRange<int>();
			if (range.HasValue && range.HasValue)
			{
				if ((float)(_tempHistorySplit + scrollValue) > range.Value.Item2)
				{
					_tempHistorySplit = (int)range.Value.Item2;
				}
				else if ((float)(_tempHistorySplit + scrollValue) < range.Value.Item1)
				{
					_tempHistorySplit = (int)range.Value.Item1;
				}
				else
				{
					_tempHistorySplit += scrollValue;
				}
			}
		}

		private void OnMouseLeft(object sender, MouseEventArgs e)
		{
			_tempHistorySplit = -1;
			if (_savedDrawInterval.HasValue)
			{
				base.DrawInterval = _savedDrawInterval.Value;
				_savedDrawInterval = null;
			}
		}

		private void EventService_ServiceAdded(object sender, ValueEventArgs<EventStateService.VisibleStateInfo> e)
		{
			if (e.get_Value().AreaName == Configuration.Name && e.get_Value().State == EventStateService.EventStates.Hidden)
			{
				ReAddEvents();
			}
		}

		private void EventService_ServiceRemoved(object sender, ValueEventArgs<EventStateService.VisibleStateInfo> e)
		{
			if (e.get_Value().AreaName == Configuration.Name && e.get_Value().State == EventStateService.EventStates.Hidden)
			{
				ReAddEvents();
			}
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			if (Configuration.LimitToCurrentMap.get_Value())
			{
				ReAddEvents();
			}
		}

		private void AllowUnspecifiedMap_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (Configuration.LimitToCurrentMap.get_Value())
			{
				ReAddEvents();
			}
		}

		private void FontFace_SettingChanged(object sender, ValueChangedEventArgs<FontFace> e)
		{
			_fonts?.Clear();
		}

		private void CustomFontPath_SettingChanged(object sender, ValueChangedEventArgs<string> e)
		{
			if (Configuration.FontFace.get_Value() == FontFace.Custom)
			{
				_fonts?.Clear();
			}
		}

		private void LimitToCurrentMap_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			ReAddEvents();
		}

		private void DrawInterval_SettingChanged(object sender, ValueChangedEventArgs<DrawInterval> e)
		{
			base.DrawInterval = TimeSpan.FromMilliseconds((double)e.get_NewValue());
		}

		private void TimeSpan_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			ReAddEvents();
		}

		private void EventOrder_SettingChanged(object sender, ValueChangedEventArgs<List<string>> e)
		{
			ReAddEvents();
		}

		private void EnabledKeybinding_Activated(object sender, EventArgs e)
		{
			Configuration.Enabled.set_Value(!Configuration.Enabled.get_Value());
		}

		private void DisabledEventKeys_SettingChanged(object sender, ValueChangedEventArgs<List<string>> e)
		{
			ReAddEvents();
		}

		private void BuildDirection_SettingChanged(object sender, ValueChangedEventArgs<BuildDirection> e)
		{
			Location_SettingChanged(this, null);
		}

		private void UseFiller_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			ReAddEvents();
		}

		public async Task UpdateAllEventsAsync(List<EventCategory> allEvents)
		{
			_logger.Debug("Receiving new events..");
			await (_updateFillerTask ?? Task.CompletedTask);
			_lastCheckForNewEventsUpdate = double.MinValue;
			_lastFillerUpdate.Value = double.MinValue;
			base.SkipDraw = true;
			using (await _eventLock.LockAsync())
			{
				_receiving = true;
				_allEvents.Clear();
				JsonSerializer serializer = _getSerializer();
				List<EventCategory> copy = serializer.DeserializeObject<List<EventCategory>>(serializer.SerializeObject(allEvents));
				_allEvents.AddRange(copy);
				_allEvents.ForEach(delegate(EventCategory ec)
				{
					ec.Load(_getNowAction, _translationService);
				});
				_receiving = false;
			}
			ReAddEvents(setSkipDraw: false);
			base.SkipDraw = false;
			_logger.Debug("Finished Receiving new events..");
		}

		private void Event_Removed(object sender, string apiCode)
		{
			List<Estreya.BlishHUD.EventTable.Models.Event> events = new List<Estreya.BlishHUD.EventTable.Models.Event>();
			using (_eventLock.Lock())
			{
				events.AddRange(from ev in _allEvents.SelectMany((EventCategory ec) => ec.Events)
					where ev.APICode == apiCode
					where Configuration.EnableLinkedCompletion.get_Value() || !ev.LinkedCompletion
					select ev);
				if (Configuration.EnableLinkedCompletion.get_Value())
				{
					events.AddRange(from ev in _allEvents.SelectMany((EventCategory ec) => ec.Events)
						where events.Any((Estreya.BlishHUD.EventTable.Models.Event ce) => ce.LinkedCompletionKeys?.Contains(ev.SettingKey) ?? false)
						select ev);
				}
			}
			events.ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
			{
				_logger.Info("Event \"" + ev.SettingKey + "\" no longer marked completed via api.");
				_eventStateService.Remove(Configuration.Name, ev.SettingKey);
			});
		}

		private void Event_Completed(object sender, string apiCode)
		{
			List<Estreya.BlishHUD.EventTable.Models.Event> events = new List<Estreya.BlishHUD.EventTable.Models.Event>();
			using (_eventLock.Lock())
			{
				events.AddRange(from ev in _allEvents.SelectMany((EventCategory ec) => ec.Events)
					where ev.APICode == apiCode
					where Configuration.EnableLinkedCompletion.get_Value() || !ev.LinkedCompletion
					select ev);
				if (Configuration.EnableLinkedCompletion.get_Value())
				{
					events.AddRange(from ev in _allEvents.SelectMany((EventCategory ec) => ec.Events)
						where events.Any((Estreya.BlishHUD.EventTable.Models.Event ce) => ce.LinkedCompletionKeys?.Contains(ev.SettingKey) ?? false)
						select ev);
				}
			}
			events.ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
			{
				Instant nextReset = GetNextReset(ev);
				_logger.Info($"Event \"{ev.SettingKey}\" marked completed via api until: {nextReset}");
				if (Configuration.DisabledCompletionActionForEvents.get_Value().Contains(ev.SettingKey))
				{
					_logger.Info("Event \"" + ev.SettingKey + "\" completion action is disabled. Abort.");
				}
				else
				{
					FinishEvent(ev, nextReset);
				}
			});
		}

		private void BackgroundColor_SettingChanged(object sender, ValueChangedEventArgs<Color> e)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Color backgroundColor = Color.get_Transparent();
			if (e.get_NewValue() != null && e.get_NewValue().get_Id() != 1)
			{
				backgroundColor = ColorExtensions.ToXnaColor(e.get_NewValue().get_Cloth());
			}
			((Control)this).set_BackgroundColor(backgroundColor * Configuration.Opacity.get_Value());
		}

		private void ReportNewHeight(int height)
		{
			if (base.Height != height)
			{
				base.Height = height;
				Configuration.Size.Y.set_Value(height);
				Location_SettingChanged(this, null);
			}
		}

		private void Opacity_SettingChanged(object sender, ValueChangedEventArgs<float> e)
		{
			BackgroundColor_SettingChanged(this, new ValueChangedEventArgs<Color>((Color)null, Configuration.BackgroundColor.get_Value()));
		}

		private void Location_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			bool buildFromBottom = Configuration.BuildDirection.get_Value() == BuildDirection.Bottom;
			((Control)this).set_Location(buildFromBottom ? new Point(Configuration.Location.X.get_Value(), Configuration.Location.Y.get_Value() - base.Height) : new Point(Configuration.Location.X.get_Value(), Configuration.Location.Y.get_Value()));
		}

		private void Size_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(Configuration.Size.X.get_Value(), base.Height);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)30;
		}

		private List<IGrouping<string, string>> GetActiveEventKeysGroupedByCategory()
		{
			List<string> activeEventKeys = GetActiveEventKeys();
			List<string> order = GetEventCategoryOrdering();
			return (from x in activeEventKeys
				orderby order.IndexOf(x.Split('_')[0])
				select x into aek
				group aek by aek.Split('_')[0]).ToList();
		}

		private List<string> GetEventCategoryOrdering()
		{
			return Configuration.EventOrder.get_Value().ToList();
		}

		private List<string> GetActiveEventKeys()
		{
			using (_eventLock.Lock())
			{
				return (from e in _allEvents.SelectMany((EventCategory ae) => ae.Events)
					where !e.Filler && !EventDisabled(e)
					select e.SettingKey into sk
					where !Configuration.DisabledEventKeys.get_Value().Contains(sk)
					select sk).ToList();
			}
		}

		private int GetWidth()
		{
			return base.Width - DrawXOffset;
		}

		private BitmapFont GetFont()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = _fonts.GetOrAdd(Configuration.FontSize.get_Value(), (Func<FontSize, BitmapFont>)delegate(FontSize fontSize)
			{
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					if (Configuration.FontFace.get_Value() == FontFace.Custom)
					{
						string value = Configuration.CustomFontPath.get_Value();
						string extension = Path.GetExtension(value);
						if (extension == ".ttf")
						{
							FileStream fileStream = FileUtil.ReadStream(value);
							SpriteFont spriteFont = FontUtils.FromTrueTypeFont(fileStream?.ToByteArray(), (float)fontSize, 256, 256);
							fileStream.Dispose();
							return spriteFont.ToBitmapFont();
						}
						if (extension == ".fnt")
						{
							return FontUtils.FromBMFont(value).ToBitmapFont();
						}
						return null;
					}
					Stream fileStream2 = _contentsManager.GetFileStream("fonts\\" + Configuration.FontFace.get_Value().ToString() + ".ttf");
					SpriteFont spriteFont2 = FontUtils.FromTrueTypeFont(fileStream2?.ToByteArray(), (float)fontSize, 256, 256);
					fileStream2.Dispose();
					return spriteFont2.ToBitmapFont();
				}
				catch (Exception ex)
				{
					_logger.Warn(ex, "Could not load font file.");
					return null;
				}
			});
			if (font == null)
			{
				font = _defaultFont;
			}
			return font;
		}

		private void ReAddEvents(bool setSkipDraw = true)
		{
			if (setSkipDraw)
			{
				base.SkipDraw = true;
			}
			_clearing = true;
			ClearEventControls();
			if (setSkipDraw)
			{
				base.SkipDraw = false;
			}
			_clearing = false;
			_eventCategoryOrdering = null;
			_lastFillerUpdate.Value = _updateFillersInterval.TotalMilliseconds;
			_lastCheckForNewEventsUpdate = double.MinValue;
			CheckForNewEventsForScreen();
			_lastCheckForNewEventsUpdate = 0.0;
		}

		private (Instant Now, Instant Min, Instant Max) GetTimes()
		{
			Instant now = _getNowAction();
			Instant min = now.Minus(Duration.FromMinutes((float)Configuration.TimeSpan.get_Value() * GetTimeSpanRatio()));
			Instant max = now.Plus(Duration.FromMinutes((float)Configuration.TimeSpan.get_Value() * (1f - GetTimeSpanRatio())));
			return (now, min, max);
		}

		private float GetTimeSpanRatio()
		{
			int historySplit = ((_tempHistorySplit != -1) ? _tempHistorySplit : Configuration.HistorySplit.get_Value());
			return 0.5f + ((float)historySplit / 100f - 0.5f);
		}

		private async Task UpdateFillersAsync()
		{
			if (_clearing || _receiving)
			{
				return;
			}
			(Instant, Instant, Instant) times = GetTimes();
			new List<Task>();
			List<string> activeEventKeys = GetActiveEventKeys();
			ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>> fillers = await GetFillers(times.Item1, times.Item2, times.Item3, activeEventKeys);
			using (await _eventLock.LockAsync())
			{
				foreach (EventCategory ec in _allEvents)
				{
					if (fillers.TryGetValue(ec.Key, out var categoryFillers))
					{
						categoryFillers.ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event cf)
						{
							cf.Load(ec, _getNowAction, _translationService);
						});
					}
					ec.UpdateFillers(categoryFillers);
				}
			}
		}

		private async Task<ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>>> GetFillers(Instant now, Instant min, Instant max, List<string> activeEventKeys)
		{
			try
			{
				if (!Configuration.UseFiller.get_Value() || activeEventKeys == null || activeEventKeys.Count == 0)
				{
					return new ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>>();
				}
				_logger.Info("Load fillers...");
				IFlurlRequest flurlRequest = ((!Configuration.CompactMode.get_Value()) ? _flurlClient.Request(_apiRootUrl, "fillers") : _flurlClient.Request(_apiRootUrl, "compact", "fillers"));
				string accessToken = _getAccessToken();
				if (!string.IsNullOrWhiteSpace(accessToken))
				{
					_logger.Info("Include custom event fillers...");
					flurlRequest.WithOAuthBearerToken(accessToken);
				}
				List<Estreya.BlishHUD.EventTable.Models.Event> activeEvents = new List<Estreya.BlishHUD.EventTable.Models.Event>();
				using (_eventLock.Lock())
				{
					activeEvents.AddRange((from ev in _allEvents.SelectMany((EventCategory a) => a.Events)
						where ev.Filler || activeEventKeys.Any((string aeg) => aeg == ev.SettingKey)
						select ev).ToList());
				}
				IEnumerable<string> eventKeys = activeEvents.Select((Estreya.BlishHUD.EventTable.Models.Event a) => a.SettingKey).Distinct();
				_logger.Debug("Fetch fillers with active keys: " + string.Join(", ", eventKeys.ToArray()));
				List<KeyValuePair<string, OnlineFillerEvent[]>> fillerList = (await (await flurlRequest.PostJsonAsync(new OnlineFillerRequest
				{
					Module = new OnlineFillerRequest.OnlineFillerRequestModule
					{
						Version = ((object)_getVersion()).ToString()
					},
					Times = new OnlineFillerRequest.OnlineFillerRequestTimes
					{
						Now_UTC_ISO = now.InUtc().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture),
						Min_UTC_ISO = min.InUtc().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture),
						Max_UTC_ISO = max.InUtc().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture)
					},
					EventKeys = activeEvents.Select((Estreya.BlishHUD.EventTable.Models.Event a) => a.SettingKey).ToArray()
				}, default(CancellationToken), (HttpCompletionOption)0)).GetJsonAsync<Dictionary<string, OnlineFillerEvent[]>>(_getSerializer())).ToList();
				ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>> parsedFillers = new ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>>(activeEvents.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Filler).GroupBy(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
				{
					ev.Category.TryGetTarget(out var target);
					return target.Key;
				}).ToDictionary((IGrouping<string, Estreya.BlishHUD.EventTable.Models.Event> group) => group.Key, (IGrouping<string, Estreya.BlishHUD.EventTable.Models.Event> group) => group.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Filler).ToList())
					.ToList());
				for (int i = 0; i < fillerList.Count; i++)
				{
					KeyValuePair<string, OnlineFillerEvent[]> currentCategory = fillerList[i];
					OnlineFillerEvent[] value = currentCategory.Value;
					foreach (OnlineFillerEvent fillerItem in value)
					{
						Estreya.BlishHUD.EventTable.Models.Event filler = new Estreya.BlishHUD.EventTable.Models.Event
						{
							Name = (fillerItem.Name ?? ""),
							Duration = fillerItem.Duration,
							Filler = true
						};
						fillerItem.Occurences.ToList().ForEach(delegate(Instant o)
						{
							filler.Occurences.Add(o);
						});
						parsedFillers.GetOrAdd(currentCategory.Key, (string _) => new List<Estreya.BlishHUD.EventTable.Models.Event>()).Add(filler);
					}
				}
				return parsedFillers;
			}
			catch (FlurlHttpException ex2)
			{
				string error = await ex2.GetResponseStringAsync();
				_logger.Warn($"Could not load fillers from {ex2.Call.Request.get_RequestUri()}: {error}");
			}
			catch (Exception ex)
			{
				_logger.Warn(ex, "Could not load fillers.");
			}
			return new ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>>();
		}

		private bool EventDisabled(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			return (!ev.Filler && EventDisabled(ev.SettingKey)) | EventTemporaryDisabled(ev);
		}

		private bool EventTemporaryDisabled(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			bool disabled = false;
			if (!ev.Filler && Configuration.LimitToCurrentMap.get_Value() && GameService.Gw2Mumble.get_IsAvailable())
			{
				int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				if (!ev.MapIds.Contains(mapId) && (!Configuration.AllowUnspecifiedMap.get_Value() || ev.MapIds.Length != 0))
				{
					disabled = true;
				}
			}
			return disabled;
		}

		private bool EventDisabled(string settingKey)
		{
			return !(!Configuration.DisabledEventKeys.get_Value().Contains(settingKey) & !_eventStateService.Contains(Configuration.Name, settingKey, EventStateService.EventStates.Hidden));
		}

		private void UpdateEventsOnScreen(SpriteBatch spriteBatch)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			if (_clearing || _receiving)
			{
				return;
			}
			(Instant, Instant, Instant) times = GetTimes();
			_activeEvent = null;
			int y = DrawYOffset;
			_drawXOffset = 0;
			List<List<(Instant, Event)>> orderedControlEvents = OrderedControlEvents;
			if (Configuration.ShowCategoryNames.get_Value())
			{
				foreach (List<(Instant, Event)> controlEventPairs2 in orderedControlEvents)
				{
					if (controlEventPairs2.Count > 0 && controlEventPairs2.First().Item2.Model.Category.TryGetTarget(out var eventCategory2))
					{
						_drawXOffset = Math.Max((int)GetFont().MeasureString(eventCategory2.Name).Width + 10, _drawXOffset);
					}
				}
				if (Configuration.CategoryNameBackgroundColor.get_Value().get_Id() != 1)
				{
					spriteBatch.Draw(Textures.get_Pixel(), new RectangleF(0f, 0f, (float)_drawXOffset, (float)_heightFromLastDraw), ColorExtensions.ToXnaColor(Configuration.CategoryNameBackgroundColor.get_Value().get_Cloth()) * Configuration.CategoryNameBackgroundOpacity.get_Value());
				}
			}
			RectangleF renderRect = default(RectangleF);
			foreach (List<(Instant, Event)> controlEventPairs in orderedControlEvents)
			{
				if (controlEventPairs.Count == 0)
				{
					continue;
				}
				if (Configuration.ShowCategoryNames.get_Value() && controlEventPairs.First().Item2.Model.Category.TryGetTarget(out var eventCategory))
				{
					Color color = ((Configuration.CategoryNameColor.get_Value().get_Id() == 1) ? Color.get_Black() : ColorExtensions.ToXnaColor(Configuration.CategoryNameColor.get_Value().get_Cloth()));
					int textHeight = (int)GetFont().MeasureString(eventCategory.Name).Height;
					int yOffset = Configuration.EventHeight.get_Value() / 2 - textHeight / 2;
					BitmapFontExtensions.DrawString(spriteBatch, GetFont(), eventCategory.Name, new Vector2(5f, (float)(y + yOffset)), color * Configuration.CategoryNameOpacity.get_Value(), (Rectangle?)null);
				}
				List<(Instant, Event)> toDelete = new List<(Instant, Event)>();
				foreach (var controlEvent in controlEventPairs)
				{
					if (EventDisabled(controlEvent.Item2.Model))
					{
						toDelete.Add(controlEvent);
						continue;
					}
					float width = (float)controlEvent.Item2.Model.CalculateWidth(controlEvent.Item1, times.Item2, GetWidth(), PixelPerMinute);
					if (width <= 0f)
					{
						toDelete.Add(controlEvent);
						continue;
					}
					float x = (float)controlEvent.Item2.Model.CalculateXPosition(controlEvent.Item1, times.Item2, PixelPerMinute);
					x = ((x < 0f) ? 0f : x) + (float)DrawXOffset;
					((RectangleF)(ref renderRect))._002Ector(x, (float)y, width, (float)Configuration.EventHeight.get_Value());
					controlEvent.Item2.Render(spriteBatch, renderRect);
					RectangleF val = renderRect.ToBounds(RectangleF.op_Implicit(((Control)this).get_AbsoluteBounds()));
					if (((RectangleF)(ref val)).Contains(Point2.op_Implicit(GameService.Input.get_Mouse().get_Position())))
					{
						_activeEvent = controlEvent.Item2;
					}
				}
				foreach (var delete in toDelete)
				{
					_logger.Debug("Deleted event " + delete.Item2.Model.Name);
					RemoveEventHooks(delete.Item2);
					delete.Item2.Dispose();
					controlEventPairs.Remove(delete);
				}
				y += Configuration.EventHeight.get_Value();
			}
			_heightFromLastDraw = ((y == 0) ? 1 : y);
			if (_activeEvent != null && _lastActiveEvent?.Model?.Key != _activeEvent.Model.Key)
			{
				bool valueOrDefault = (_activeEvent?.Model?.Filler).GetValueOrDefault();
				Tooltip tooltip = ((Control)this).get_Tooltip();
				if (tooltip != null)
				{
					((Control)tooltip).Dispose();
				}
				((Control)this).set_Tooltip((Tooltip)null);
				ContextMenuStrip menu = ((Control)this).get_Menu();
				if (menu != null && !((Control)menu).get_Visible())
				{
					ContextMenuStrip menu2 = ((Control)this).get_Menu();
					if (menu2 != null)
					{
						((Control)menu2).Dispose();
					}
					((Control)this).set_Menu((ContextMenuStrip)null);
				}
				if (!valueOrDefault)
				{
					((Control)this).set_Tooltip((!Configuration.ShowTooltips.get_Value()) ? null : _activeEvent?.BuildTooltip());
					((Control)this).set_Menu(_activeEvent?.BuildContextMenu(_getAreaNames, Configuration.Name, _getDisabledReminderKeys));
				}
				_lastActiveEvent = _activeEvent;
			}
			else
			{
				if (_activeEvent != null)
				{
					return;
				}
				_lastActiveEvent = null;
				Tooltip tooltip2 = ((Control)this).get_Tooltip();
				if (tooltip2 != null)
				{
					((Control)tooltip2).Dispose();
				}
				((Control)this).set_Tooltip((Tooltip)null);
				ContextMenuStrip menu3 = ((Control)this).get_Menu();
				if (menu3 != null && !((Control)menu3).get_Visible())
				{
					ContextMenuStrip menu4 = ((Control)this).get_Menu();
					if (menu4 != null)
					{
						((Control)menu4).Dispose();
					}
					((Control)this).set_Menu((ContextMenuStrip)null);
				}
			}
		}

		private void CheckForNewEventsForScreen()
		{
			if (_clearing || _receiving)
			{
				return;
			}
			(Instant Now, Instant Min, Instant Max) times = GetTimes();
			foreach (IGrouping<string, string> activeEventGroup in GetActiveEventKeysGroupedByCategory())
			{
				string categoryKey = activeEventGroup.Key;
				EventCategory validCategory = null;
				if (_eventLock.IsFree())
				{
					using (_eventLock.Lock())
					{
						validCategory = _allEvents?.Find((EventCategory ec) => ec.Key == categoryKey);
					}
				}
				else
				{
					_logger.Debug("Event lock is busy. Can't update category " + categoryKey);
				}
				List<Estreya.BlishHUD.EventTable.Models.Event> events = validCategory?.Events.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => activeEventGroup.Any((string aeg) => aeg == ev.SettingKey) || (Configuration.UseFiller.get_Value() && ev.Filler)).ToList();
				if (events == null || events.Count == 0)
				{
					continue;
				}
				using (_controlLock.Lock())
				{
					if (_controlEvents.TryAdd(categoryKey, new List<(Instant, Event)>()))
					{
						_orderedControlEvents = null;
					}
				}
				foreach (Estreya.BlishHUD.EventTable.Models.Event ev2 in events.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Occurences.Any((Instant oc) => oc.Plus(ev.Duration) >= times.Min && oc <= times.Max)))
				{
					if (EventDisabled(ev2))
					{
						continue;
					}
					foreach (Instant occurence in ev2.Occurences.Where((Instant oc) => oc.Plus(ev2.Duration) >= times.Min && oc <= times.Max))
					{
						using (_controlLock.Lock())
						{
							if (_controlEvents.TryGetValue(categoryKey, out var controlEvent) && controlEvent.Any<(Instant, Event)>(((Instant Occurence, Event Event) addedEvent) => addedEvent.Occurence == occurence))
							{
								continue;
							}
						}
						float num = (float)ev2.CalculateXPosition(occurence, times.Min, PixelPerMinute);
						float width = (float)ev2.CalculateWidth(occurence, times.Min, GetWidth(), PixelPerMinute);
						if (num > (float)GetWidth() || width <= 0f)
						{
							continue;
						}
						Event newEventControl = new Event(ev2, _iconService, _translationService, _getNowAction, occurence, occurence.Plus(ev2.Duration), GetFont, () => !ev2.Filler && Configuration.DrawBorders.get_Value(), delegate
						{
							EventCompletedAction value3 = Configuration.CompletionAction.get_Value();
							return ((value3 == EventCompletedAction.Crossout || value3 == EventCompletedAction.CrossoutAndChangeOpacity) ? true : false) && _eventStateService.Contains(Configuration.Name, ev2.SettingKey, EventStateService.EventStates.Completed);
						}, delegate
						{
							//IL_0000: Unknown result type (might be due to invalid IL or missing references)
							//IL_0005: Unknown result type (might be due to invalid IL or missing references)
							//IL_0068: Unknown result type (might be due to invalid IL or missing references)
							//IL_006f: Unknown result type (might be due to invalid IL or missing references)
							//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
							//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
							//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
							//IL_020d: Unknown result type (might be due to invalid IL or missing references)
							//IL_020f: Unknown result type (might be due to invalid IL or missing references)
							Color black = Color.get_Black();
							Color val = ((!ev2.Filler) ? ((Configuration.TextColor.get_Value().get_Id() == 1) ? black : ColorExtensions.ToXnaColor(Configuration.TextColor.get_Value().get_Cloth())) : ((Configuration.FillerTextColor.get_Value().get_Id() == 1) ? black : ColorExtensions.ToXnaColor(Configuration.FillerTextColor.get_Value().get_Cloth())));
							float num2 = (ev2.Filler ? Configuration.FillerTextOpacity.get_Value() : Configuration.EventTextOpacity.get_Value());
							EventCompletedAction value2 = Configuration.CompletionAction.get_Value();
							bool flag2 = (((uint)(value2 - 3) <= 1u) ? true : false);
							if (flag2 && _eventStateService.Contains(Configuration.Name, ev2.SettingKey, EventStateService.EventStates.Completed))
							{
								if (Configuration.CompletedEventsInvertTextColor.get_Value())
								{
									((Color)(ref val))._002Ector(((Color)(ref val)).get_PackedValue() ^ 0xFFFFFFu);
								}
								num2 = Configuration.CompletedEventsTextOpacity.get_Value();
							}
							return val * num2;
						}, delegate
						{
							//IL_001b: Unknown result type (might be due to invalid IL or missing references)
							//IL_0020: Unknown result type (might be due to invalid IL or missing references)
							//IL_019d: Unknown result type (might be due to invalid IL or missing references)
							//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
							//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
							//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
							//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
							//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
							if (ev2.Filler)
							{
								return (Color[])(object)new Color[1] { Color.get_Transparent() };
							}
							float alpha = Configuration.EventBackgroundOpacity.get_Value();
							EventCompletedAction value = Configuration.CompletionAction.get_Value();
							bool flag = (((uint)(value - 3) <= 1u) ? true : false);
							if (flag && _eventStateService.Contains(Configuration.Name, ev2.SettingKey, EventStateService.EventStates.Completed))
							{
								alpha = Configuration.CompletedEventsBackgroundOpacity.get_Value();
							}
							if (!Configuration.EnableColorGradients.get_Value() || ev2.BackgroundColorGradientCodes == null || ev2.BackgroundColorGradientCodes.Length == 0)
							{
								if (string.IsNullOrWhiteSpace(ev2.BackgroundColorCode))
								{
									return (Color[])(object)new Color[1] { Color.get_White() * alpha };
								}
								Color color = ColorTranslator.FromHtml(ev2.BackgroundColorCode);
								return (Color[])(object)new Color[1] { new Color((int)color.R, (int)color.G, (int)color.B) * alpha };
							}
							return ev2.BackgroundColorGradientCodes.Select(delegate(string cc)
							{
								//IL_001c: Unknown result type (might be due to invalid IL or missing references)
								//IL_0027: Unknown result type (might be due to invalid IL or missing references)
								Color color2 = ColorTranslator.FromHtml(cc);
								return new Color((int)color2.R, (int)color2.G, (int)color2.B) * alpha;
							}).ToArray();
						}, () => (!ev2.Filler) ? Configuration.DrawShadows.get_Value() : Configuration.DrawShadowsForFiller.get_Value(), () => (!ev2.Filler) ? (((Configuration.ShadowColor.get_Value().get_Id() == 1) ? Color.get_Black() : ColorExtensions.ToXnaColor(Configuration.ShadowColor.get_Value().get_Cloth())) * Configuration.ShadowOpacity.get_Value()) : (((Configuration.FillerShadowColor.get_Value().get_Id() == 1) ? Color.get_Black() : ColorExtensions.ToXnaColor(Configuration.FillerShadowColor.get_Value().get_Cloth())) * Configuration.FillerShadowOpacity.get_Value()), () => Configuration.EventAbsoluteTimeFormatString.get_Value(), () => (Configuration.EventTimespanDaysFormatString.get_Value(), Configuration.EventTimespanHoursFormatString.get_Value(), Configuration.EventTimespanMinutesFormatString.get_Value()));
						AddEventHooks(newEventControl);
						_logger.Debug($"Added event {ev2.Name} with occurence {occurence}");
						using (_controlLock.Lock())
						{
							if (_controlEvents.ContainsKey(categoryKey))
							{
								_controlEvents[categoryKey].Add((occurence, newEventControl));
							}
						}
					}
				}
			}
		}

		private async void OnLeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			try
			{
				Event currentEvent = _activeEvent;
				if (currentEvent == null || currentEvent.Model.Filler)
				{
					return;
				}
				string waypoint = currentEvent.Model?.GetWaypoint(_accountService.Account);
				switch (Configuration.LeftClickAction.get_Value())
				{
				case LeftClickAction.CopyWaypoint:
				{
					if (string.IsNullOrWhiteSpace(waypoint))
					{
						break;
					}
					string eventChatFormat = currentEvent.Model.GetChatText(Configuration.EventChatFormat.get_Value(), currentEvent.StartTime, _accountService.Account);
					if ((int)GameService.Input.get_Keyboard().get_ActiveModifiers() == 1)
					{
						try
						{
							await _chatService.ChangeChannel(ChatChannel.Squad);
							await _chatService.ChangeChannel(Configuration.WaypointSendingChannel.get_Value(), Configuration.WaypointSendingGuild.get_Value(), GameService.Gw2Mumble.get_PlayerCharacter().get_Name());
							await _chatService.Send(eventChatFormat);
						}
						catch (Exception ex2)
						{
							_logger.Warn(ex2, "Could not paste waypoint into chat. Event: " + currentEvent.Model.SettingKey);
							ScreenNotification.ShowNotification(new string[2] { "Waypoint could not be pasted in chat.", "See log for more information." }, ScreenNotification.NotificationType.Error, null, 5);
						}
					}
					else
					{
						await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(eventChatFormat);
						ScreenNotification.ShowNotification(new string[2]
						{
							currentEvent.Model.Name,
							"Copied to clipboard!"
						});
					}
					break;
				}
				case LeftClickAction.NavigateToWaypoint:
				{
					if (string.IsNullOrWhiteSpace(waypoint))
					{
						break;
					}
					if (_pointOfInterestService.Loading)
					{
						ScreenNotification.ShowNotification("PointOfInterestService is still loading!", ScreenNotification.NotificationType.Error);
						break;
					}
					PointOfInterest poi = _pointOfInterestService.GetPointOfInterest(waypoint);
					if (poi == null)
					{
						ScreenNotification.ShowNotification(waypoint + " not found!", ScreenNotification.NotificationType.Error);
						break;
					}
					Task.Run(async delegate
					{
						MapUtil.NavigationResult result = await (_mapUtil?.NavigateToPosition(poi, Configuration.AcceptWaypointPrompt.get_Value()) ?? Task.FromResult(new MapUtil.NavigationResult(success: false, "Variable null.")));
						if (result.Success)
						{
							if (Configuration.HideAfterWaypointNavigation.get_Value())
							{
								Configuration.Enabled.set_Value(false);
							}
						}
						else
						{
							ScreenNotification.ShowNotification("Navigation failed: " + (result.Message ?? "Unknown"), ScreenNotification.NotificationType.Error);
						}
					});
					break;
				}
				}
			}
			catch (Exception ex)
			{
				_logger.Warn(ex, "Could not handle left mouse click.");
			}
		}

		public bool CalculateUIVisibility()
		{
			bool show = true;
			if (Configuration.HideOnOpenMap.get_Value())
			{
				show &= !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			if (Configuration.HideOnMissingMumbleTicks.get_Value())
			{
				show &= GameService.Gw2Mumble.get_TimeSinceTick().TotalSeconds < 0.5;
			}
			if (Configuration.HideInCombat.get_Value())
			{
				show &= !GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat();
			}
			if (Configuration.HideInPvE_OpenWorld.get_Value())
			{
				MapType[] array = new MapType[4];
				RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
				MapType[] pveOpenWorldMapTypes = (MapType[])(object)array;
				show &= GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !pveOpenWorldMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type()) || MapInfo.MAP_IDS_PVE_COMPETETIVE.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			}
			if (Configuration.HideInPvE_Competetive.get_Value())
			{
				MapType[] pveCompetetiveMapTypes = (MapType[])(object)new MapType[1] { (MapType)4 };
				show &= GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !pveCompetetiveMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type()) || !MapInfo.MAP_IDS_PVE_COMPETETIVE.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			}
			if (Configuration.HideInWvW.get_Value())
			{
				MapType[] array2 = new MapType[5];
				RuntimeHelpers.InitializeArray(array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
				MapType[] wvwMapTypes = (MapType[])(object)array2;
				show &= !GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !wvwMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type());
			}
			if (Configuration.HideInPvP.get_Value())
			{
				MapType[] pvpMapTypes = (MapType[])(object)new MapType[2]
				{
					(MapType)2,
					(MapType)6
				};
				show &= !GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !pvpMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type());
			}
			return show;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			_updateFillerTask = UpdateUtil.UpdateAsync(UpdateFillersAsync, gameTime, _updateFillersInterval.TotalMilliseconds, _lastFillerUpdate).Unwrap();
			UpdateUtil.Update(CheckForNewEventsForScreen, gameTime, _checkForNewEventsInterval.TotalMilliseconds, ref _lastCheckForNewEventsUpdate);
			ReportNewHeight(_heightFromLastDraw);
		}

		protected override void DoPaint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Configuration.TopTimelineLinesInBackground.get_Value())
			{
				DrawTopTimeLine(spriteBatch);
			}
			UpdateEventsOnScreen(spriteBatch);
			if (!Configuration.TopTimelineLinesInBackground.get_Value())
			{
				DrawTopTimeLine(spriteBatch);
			}
			DrawTimeLine(spriteBatch);
		}

		private void DrawTopTimeLine(SpriteBatch spriteBatch)
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			_drawYOffset = 0;
			if (!Configuration.ShowTopTimeline.get_Value())
			{
				return;
			}
			float width = GetWidth();
			(Instant, Instant, Instant) times = GetTimes();
			RectangleF rect = default(RectangleF);
			((RectangleF)(ref rect))._002Ector((float)DrawXOffset, 0f, width, 30f);
			Color backgroundColor = ((Configuration.TopTimelineBackgroundColor.get_Value().get_Id() == 1) ? Color.get_Transparent() : ColorExtensions.ToXnaColor(Configuration.TopTimelineBackgroundColor.get_Value().get_Cloth())) * Configuration.TopTimelineBackgroundOpacity.get_Value();
			spriteBatch.DrawRectangle(Textures.get_Pixel(), rect, backgroundColor);
			int timeInterval = 15;
			int timeSteps = (int)Math.Floor((times.Item3 - times.Item2).TotalMinutes) / timeInterval;
			float timeStepLineHeight = (Configuration.TopTimelineLinesOverWholeHeight.get_Value() ? ((float)base.Height) : rect.Height);
			Color lineColor = ((Configuration.TopTimelineLineColor.get_Value().get_Id() == 1) ? Color.get_Black() : ColorExtensions.ToXnaColor(Configuration.TopTimelineLineColor.get_Value().get_Cloth())) * Configuration.TopTimelineLineOpacity.get_Value();
			Color timeColor = ((Configuration.TopTimelineTimeColor.get_Value().get_Id() == 1) ? Color.get_Red() : ColorExtensions.ToXnaColor(Configuration.TopTimelineTimeColor.get_Value().get_Cloth())) * Configuration.TopTimelineTimeOpacity.get_Value();
			List<Duration> stepTimes = new List<Duration>();
			ZonedDateTime minUtc = times.Item2.InUtc();
			int minutes = (int)Math.Ceiling((double)minUtc.Minute / (double)timeInterval) * timeInterval;
			DateTime first = new DateTime(minUtc.Year, minUtc.Month, minUtc.Day, minUtc.Hour, 0, 0, DateTimeKind.Utc).AddMinutes(minutes);
			for (int j = 0; j < timeSteps; j++)
			{
				stepTimes.Add(first.AddMinutes(j * timeInterval).ToInstant().Minus(times.Item2));
			}
			RectangleF timeStepRect = default(RectangleF);
			for (int i = 0; i < stepTimes.Count; i++)
			{
				Duration stepTime = stepTimes[i];
				float x = (float)(PixelPerMinute * stepTime.TotalMinutes + (double)DrawXOffset);
				((RectangleF)(ref timeStepRect))._002Ector(x, 0f, 2f, timeStepLineHeight);
				ZonedDateTime time = times.Item2.Plus(stepTime).InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault());
				spriteBatch.DrawLine(Textures.get_Pixel(), timeStepRect, lineColor);
				string formattedString = "FORMAT";
				try
				{
					formattedString = time.ToString(Configuration.TopTimelineTimeFormatString.get_Value(), CultureInfo.InvariantCulture);
				}
				catch
				{
				}
				spriteBatch.DrawString(formattedString, GetFont(), new RectangleF(timeStepRect.X + 5f, 5f, (float)PixelPerMinute * (float)timeInterval, 20f), timeColor, wrap: false, 1f, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			_drawYOffset = (int)rect.Height;
		}

		private void DrawTimeLine(SpriteBatch spriteBatch)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			float middleLineX = (float)GetWidth() * GetTimeSpanRatio() + (float)DrawXOffset;
			float width = 2f;
			SpriteBatchExtensions.DrawLine(spriteBatch, Textures.get_Pixel(), new RectangleF(middleLineX - width / 2f, 0f, width, (float)base.Height), Color.get_LightGray() * Configuration.TimeLineOpacity.get_Value());
		}

		private void ClearEventControls()
		{
			using (_eventLock.Lock())
			{
				_allEvents?.ForEach(delegate(EventCategory a)
				{
					if (!a.FromContext)
					{
						a.UpdateFillers(new List<Estreya.BlishHUD.EventTable.Models.Event>());
					}
				});
			}
			using (_controlLock.Lock())
			{
				_controlEvents?.Clear();
			}
			_orderedControlEvents = null;
			_logger.Debug("Cleared filler and controls.");
		}

		private void AddEventHooks(Event ev)
		{
			ev.HideClicked += Ev_HideClicked;
			ev.ToggleFinishClicked += Ev_ToggleFinishClicked;
			ev.DisableClicked += Ev_DisableClicked;
			ev.CopyToAreaClicked += Ev_CopyToAreaClicked;
			ev.MoveToAreaClicked += Ev_MoveToAreaClicked;
			ev.EnableReminderClicked += Ev_EnableReminderClicked;
			ev.DisableReminderClicked += Ev_DisableReminderClicked;
		}

		private void RemoveEventHooks(Event ev)
		{
			ev.HideClicked -= Ev_HideClicked;
			ev.ToggleFinishClicked -= Ev_ToggleFinishClicked;
			ev.DisableClicked -= Ev_DisableClicked;
			ev.CopyToAreaClicked -= Ev_CopyToAreaClicked;
			ev.MoveToAreaClicked -= Ev_MoveToAreaClicked;
			ev.EnableReminderClicked -= Ev_EnableReminderClicked;
			ev.DisableReminderClicked -= Ev_DisableReminderClicked;
		}

		private void Ev_DisableReminderClicked(object sender, EventArgs e)
		{
			Event ev = sender as Event;
			this.DisableReminderClicked?.Invoke(this, ev.Model.SettingKey);
		}

		private void Ev_EnableReminderClicked(object sender, EventArgs e)
		{
			Event ev = sender as Event;
			this.EnableReminderClicked?.Invoke(this, ev.Model.SettingKey);
		}

		private void Ev_MoveToAreaClicked(object sender, string e)
		{
			Event ev = sender as Event;
			this.MoveToAreaClicked?.Invoke(this, (ev.Model.SettingKey, e));
		}

		private void Ev_CopyToAreaClicked(object sender, string e)
		{
			Event ev = sender as Event;
			this.CopyToAreaClicked?.Invoke(this, (ev.Model.SettingKey, e));
		}

		private void Ev_ToggleFinishClicked(object sender, EventArgs e)
		{
			Event ev3 = sender as Event;
			List<Estreya.BlishHUD.EventTable.Models.Event> events = new List<Estreya.BlishHUD.EventTable.Models.Event> { ev3.Model };
			using (_eventLock.Lock())
			{
				if (!string.IsNullOrWhiteSpace(ev3.Model.APICode))
				{
					events.AddRange(from ev2 in _allEvents.SelectMany((EventCategory ec) => ec.Events)
						where ev2.SettingKey != ev3.Model.SettingKey && ev2.APICode == ev3.Model.APICode
						select ev2 into ev
						where Configuration.EnableLinkedCompletion.get_Value() || !ev.LinkedCompletion
						select ev);
				}
				if (Configuration.EnableLinkedCompletion.get_Value())
				{
					events.AddRange(from ev2 in _allEvents.SelectMany((EventCategory ec) => ec.Events)
						where ev2.SettingKey != ev3.Model.SettingKey && events.Any((Estreya.BlishHUD.EventTable.Models.Event ce) => ce.LinkedCompletionKeys?.Contains(ev2.SettingKey) ?? false)
						select ev2);
				}
			}
			events.ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
			{
				Instant nextReset = GetNextReset(ev);
				_logger.Info($"Event \"{ev.SettingKey}\" marked completed manually until: {nextReset}");
				ToggleFinishEvent(ev, nextReset);
			});
		}

		private void ToggleFinishEvent(Estreya.BlishHUD.EventTable.Models.Event ev, Instant until)
		{
			switch (Configuration.CompletionAction.get_Value())
			{
			case EventCompletedAction.Crossout:
			case EventCompletedAction.ChangeOpacity:
			case EventCompletedAction.CrossoutAndChangeOpacity:
				if (_eventStateService.Contains(Configuration.Name, ev.SettingKey, EventStateService.EventStates.Completed))
				{
					_eventStateService.Remove(Configuration.Name, ev.SettingKey);
				}
				else
				{
					_eventStateService.Add(Configuration.Name, ev.SettingKey, until, EventStateService.EventStates.Completed);
				}
				break;
			case EventCompletedAction.Hide:
				HideEvent(ev, until);
				break;
			}
		}

		private void FinishEvent(Estreya.BlishHUD.EventTable.Models.Event ev, Instant until)
		{
			switch (Configuration.CompletionAction.get_Value())
			{
			case EventCompletedAction.Crossout:
			case EventCompletedAction.ChangeOpacity:
			case EventCompletedAction.CrossoutAndChangeOpacity:
				_eventStateService.Add(Configuration.Name, ev.SettingKey, until, EventStateService.EventStates.Completed);
				break;
			case EventCompletedAction.Hide:
				HideEvent(ev, until);
				break;
			}
		}

		private void HideEvent(Estreya.BlishHUD.EventTable.Models.Event ev, Instant until)
		{
			_eventStateService.Add(Configuration.Name, ev.SettingKey, until, EventStateService.EventStates.Hidden);
		}

		private void Ev_HideClicked(object sender, EventArgs e)
		{
			Event ev = sender as Event;
			HideEvent(ev.Model, GetNextReset(ev.Model));
		}

		private void Ev_DisableClicked(object sender, EventArgs e)
		{
			Event ev = sender as Event;
			DisableEvent(ev.Model.SettingKey);
		}

		public void EnableEvent(string eventSettingKey)
		{
			if (Configuration.DisabledEventKeys.get_Value().Contains(eventSettingKey))
			{
				Configuration.DisabledEventKeys.set_Value(new List<string>(from dek in Configuration.DisabledEventKeys.get_Value()
					where dek != eventSettingKey
					select dek));
			}
		}

		public void DisableEvent(string eventSettingKey)
		{
			if (!Configuration.DisabledEventKeys.get_Value().Contains(eventSettingKey))
			{
				Configuration.DisabledEventKeys.set_Value(new List<string>(Configuration.DisabledEventKeys.get_Value()) { eventSettingKey });
			}
		}

		private Instant GetNextReset(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			ZonedDateTime nowUTC = _getNowAction().InUtc();
			TimeSpan addDuration = TimeSpan.FromDays(1.0);
			_ = ev.Duration.TotalMinutes;
			_ = addDuration.TotalMinutes;
			return Instant.FromUtc(nowUTC.Year, nowUTC.Month, nowUTC.Day, 0, 0, 0).Plus(Duration.FromDays(Math.Ceiling(addDuration.TotalDays)));
		}

		protected override void InternalDispose()
		{
			ClearEventControls();
			if (_worldbossService != null)
			{
				_worldbossService.WorldbossCompleted -= Event_Completed;
				_worldbossService.WorldbossRemoved -= Event_Removed;
			}
			if (_mapchestService != null)
			{
				_mapchestService.MapchestCompleted -= Event_Completed;
				_mapchestService.MapchestRemoved -= Event_Removed;
			}
			if (_eventStateService != null)
			{
				_eventStateService.StateAdded -= EventService_ServiceAdded;
				_eventStateService.StateRemoved -= EventService_ServiceRemoved;
			}
			if (_fonts != null)
			{
				_fonts.Clear();
				_fonts = null;
			}
			_iconService = null;
			_worldbossService = null;
			_mapchestService = null;
			_eventStateService = null;
			_translationService = null;
			_mapUtil = null;
			_pointOfInterestService = null;
			_accountService = null;
			_contentsManager = null;
			_flurlClient = null;
			_apiRootUrl = null;
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			((Control)this).remove_MouseLeft((EventHandler<MouseEventArgs>)OnMouseLeft);
			((Control)this).remove_MouseWheelScrolled((EventHandler<MouseEventArgs>)OnMouseWheelScrolled);
			Configuration.EnabledKeybinding.get_Value().remove_Activated((EventHandler<EventArgs>)EnabledKeybinding_Activated);
			Configuration.Size.X.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)Size_SettingChanged);
			Configuration.Size.Y.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)Size_SettingChanged);
			Configuration.Location.X.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)Location_SettingChanged);
			Configuration.Location.Y.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)Location_SettingChanged);
			Configuration.Opacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)Opacity_SettingChanged);
			Configuration.BackgroundColor.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)BackgroundColor_SettingChanged);
			Configuration.UseFiller.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UseFiller_SettingChanged);
			Configuration.BuildDirection.remove_SettingChanged((EventHandler<ValueChangedEventArgs<BuildDirection>>)BuildDirection_SettingChanged);
			Configuration.EventOrder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<List<string>>>)EventOrder_SettingChanged);
			Configuration.DrawInterval.remove_SettingChanged((EventHandler<ValueChangedEventArgs<DrawInterval>>)DrawInterval_SettingChanged);
			Configuration.LimitToCurrentMap.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)LimitToCurrentMap_SettingChanged);
			Configuration.AllowUnspecifiedMap.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AllowUnspecifiedMap_SettingChanged);
			Configuration.FontFace.remove_SettingChanged((EventHandler<ValueChangedEventArgs<FontFace>>)FontFace_SettingChanged);
			Configuration.CustomFontPath.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)CustomFontPath_SettingChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			Configuration = null;
			using (_eventLock.Lock())
			{
				_allEvents?.Clear();
				_allEvents = null;
			}
		}
	}
}
