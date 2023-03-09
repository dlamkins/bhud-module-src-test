using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Newtonsoft.Json;
using SemVer;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class EventArea : RenderTargetControl
	{
		private static readonly Logger Logger = Logger.GetLogger<EventArea>();

		private static TimeSpan _updateEventOccurencesInterval = TimeSpan.FromMinutes(15.0);

		private AsyncRef<double> _lastEventOccurencesUpdate = new AsyncRef<double>(0.0);

		private static TimeSpan _checkForNewEventsInterval = TimeSpan.FromMilliseconds(1000.0);

		private double _lastCheckForNewEventsUpdate;

		private static readonly ConcurrentDictionary<FontSize, BitmapFont> _fonts = new ConcurrentDictionary<FontSize, BitmapFont>();

		private IconState _iconState;

		private TranslationState _translationState;

		private EventState _eventState;

		private WorldbossState _worldbossState;

		private MapchestState _mapchestState;

		private PointOfInterestState _pointOfInterestState;

		private MapUtil _mapUtil;

		private IFlurlClient _flurlClient;

		private string _apiRootUrl;

		private Func<DateTime> _getNowAction;

		private readonly Func<Version> _getVersion;

		private AsyncLock _eventLock = new AsyncLock();

		private List<EventCategory> _allEvents = new List<EventCategory>();

		private int _heightFromLastDraw = 1;

		private Event _lastActiveEvent;

		private List<string> _eventCategoryOrdering;

		private List<List<(DateTime Occurence, Event Event)>> _orderedControlEvents;

		private AsyncLock _controlLock = new AsyncLock();

		private ConcurrentDictionary<string, List<(DateTime Occurence, Event Event)>> _controlEvents = new ConcurrentDictionary<string, List<(DateTime, Event)>>();

		private bool _clearing;

		private Event _activeEvent;

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

		private List<List<(DateTime Occurence, Event Event)>> OrderedControlEvents
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

		private double PixelPerMinute => (double)base.Size.X / (double)Configuration.TimeSpan.get_Value();

		public EventAreaConfiguration Configuration { get; private set; }

		public EventArea(EventAreaConfiguration configuration, IconState iconState, TranslationState translationState, EventState eventState, WorldbossState worldbossState, MapchestState mapchestState, PointOfInterestState pointOfInterestState, MapUtil mapUtil, IFlurlClient flurlClient, string apiRootUrl, Func<DateTime> getNowAction, Func<Version> getVersion)
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
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			((Control)this).add_Click((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			Location_SettingChanged(this, null);
			Size_SettingChanged(this, null);
			Opacity_SettingChanged(this, new ValueChangedEventArgs<float>(0f, Configuration.Opacity.get_Value()));
			BackgroundColor_SettingChanged(this, new ValueChangedEventArgs<Color>((Color)null, Configuration.BackgroundColor.get_Value()));
			DrawInterval_SettingChanged(this, new ValueChangedEventArgs<DrawInterval>(Estreya.BlishHUD.EventTable.Models.DrawInterval.INSTANT, Configuration.DrawInterval.get_Value()));
			_getNowAction = getNowAction;
			_getVersion = getVersion;
			_iconState = iconState;
			_translationState = translationState;
			_eventState = eventState;
			_worldbossState = worldbossState;
			_mapchestState = mapchestState;
			_pointOfInterestState = pointOfInterestState;
			_mapUtil = mapUtil;
			_flurlClient = flurlClient;
			_apiRootUrl = apiRootUrl;
			if (_worldbossState != null)
			{
				_worldbossState.WorldbossCompleted += Event_Completed;
				_worldbossState.WorldbossRemoved += Event_Removed;
			}
			if (_mapchestState != null)
			{
				_mapchestState.MapchestCompleted += Event_Completed;
				_mapchestState.MapchestRemoved += Event_Removed;
			}
			if (_eventState != null)
			{
				_eventState.StateAdded += EventState_StateAdded;
				_eventState.StateRemoved += EventState_StateRemoved;
			}
		}

		private void EventState_StateAdded(object sender, ValueEventArgs<EventState.VisibleStateInfo> e)
		{
			if (e.get_Value().AreaName == Configuration.Name && e.get_Value().State == EventState.EventStates.Hidden)
			{
				ReAddEvents();
			}
		}

		private void EventState_StateRemoved(object sender, ValueEventArgs<EventState.VisibleStateInfo> e)
		{
			if (e.get_Value().AreaName == Configuration.Name && e.get_Value().State == EventState.EventStates.Hidden)
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

		public void UpdateAllEvents(List<EventCategory> allEvents)
		{
			using (_eventLock.Lock())
			{
				_allEvents.Clear();
				_allEvents.AddRange(JsonConvert.DeserializeObject<List<EventCategory>>(JsonConvert.SerializeObject(allEvents)));
				GetTimes();
				_allEvents.ForEach(delegate(EventCategory ec)
				{
					ec.Load(_getNowAction, _translationState);
				});
			}
			ReAddEvents();
		}

		private void Event_Removed(object sender, string apiCode)
		{
			List<Estreya.BlishHUD.EventTable.Models.Event> events = new List<Estreya.BlishHUD.EventTable.Models.Event>();
			using (_eventLock.Lock())
			{
				events.AddRange(from ev in _allEvents.SelectMany((EventCategory ec) => ec.Events)
					where ev.APICode == apiCode
					select ev);
			}
			events.ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
			{
				_eventState.Remove(Configuration.Name, ev.SettingKey);
			});
		}

		private void Event_Completed(object sender, string apiCode)
		{
			DateTime until = GetNextReset();
			List<Estreya.BlishHUD.EventTable.Models.Event> events = new List<Estreya.BlishHUD.EventTable.Models.Event>();
			using (_eventLock.Lock())
			{
				events.AddRange(from ev in _allEvents.SelectMany((EventCategory ec) => ec.Events)
					where ev.APICode == apiCode
					select ev);
			}
			events.ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
			{
				FinishEvent(ev, until);
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
			return (CaptureType)22;
		}

		public override Control TriggerMouseInput(MouseEventType mouseEventType, MouseState ms)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (_activeEvent == null)
			{
				return null;
			}
			return ((Control)this).TriggerMouseInput(mouseEventType, ms);
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

		private void ReAddEvents()
		{
			_clearing = true;
			using (((Control)this).SuspendLayoutContext())
			{
				ClearEventControls();
				_eventCategoryOrdering = null;
				_lastEventOccurencesUpdate.Value = _updateEventOccurencesInterval.TotalMilliseconds;
				_lastCheckForNewEventsUpdate = _checkForNewEventsInterval.TotalMilliseconds;
				_clearing = false;
			}
		}

		private (DateTime Now, DateTime Min, DateTime Max) GetTimes()
		{
			DateTime now = _getNowAction();
			DateTime min = now.Subtract(TimeSpan.FromMinutes((float)Configuration.TimeSpan.get_Value() * GetTimeSpanRatio()));
			DateTime max = now.Add(TimeSpan.FromMinutes((float)Configuration.TimeSpan.get_Value() * (1f - GetTimeSpanRatio())));
			return (now, min, max);
		}

		private float GetTimeSpanRatio()
		{
			return 0.5f + ((float)Configuration.HistorySplit.get_Value() / 100f - 0.5f);
		}

		private async Task UpdateEventOccurences()
		{
			(DateTime, DateTime, DateTime) times = GetTimes();
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
							cf.Load(ec, _getNowAction, _translationState);
						});
					}
					ec.UpdateFillers(categoryFillers);
				}
			}
		}

		private async Task<ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>>> GetFillers(DateTime now, DateTime min, DateTime max, List<string> activeEventKeys)
		{
			try
			{
				if (activeEventKeys == null || activeEventKeys.Count == 0)
				{
					return new ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>>();
				}
				IFlurlRequest flurlRequest = _flurlClient.Request(_apiRootUrl, "fillers");
				List<Estreya.BlishHUD.EventTable.Models.Event> activeEvents = new List<Estreya.BlishHUD.EventTable.Models.Event>();
				using (_eventLock.Lock())
				{
					activeEvents.AddRange((from ev in _allEvents.SelectMany((EventCategory a) => a.Events)
						where activeEventKeys.Any((string aeg) => aeg == ev.SettingKey)
						select ev).ToList());
				}
				IEnumerable<string> eventKeys = activeEvents.Select((Estreya.BlishHUD.EventTable.Models.Event a) => a.SettingKey).Distinct();
				Logger.Debug("Fetch fillers with active keys: " + string.Join(", ", eventKeys.ToArray()));
				List<OnlineFillerCategory> fillerList = (await (await flurlRequest.PostJsonAsync(new OnlineFillerRequest
				{
					Module = new OnlineFillerRequest.OnlineFillerRequestModule
					{
						Version = ((object)_getVersion()).ToString()
					},
					Times = new OnlineFillerRequest.OnlineFillerRequestTimes
					{
						Now_UTC_ISO = now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"),
						Min_UTC_ISO = min.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"),
						Max_UTC_ISO = max.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'")
					},
					EventKeys = activeEvents.Select((Estreya.BlishHUD.EventTable.Models.Event a) => a.SettingKey).ToArray()
				}, default(CancellationToken), (HttpCompletionOption)0)).GetJsonAsync<OnlineFillerCategory[]>()).ToList();
				ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>> parsedFillers = new ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>>();
				for (int i = 0; i < fillerList.Count; i++)
				{
					OnlineFillerCategory currentCategory = fillerList[i];
					OnlineFillerEvent[] fillers = currentCategory.Fillers;
					foreach (OnlineFillerEvent fillerItem in fillers)
					{
						Estreya.BlishHUD.EventTable.Models.Event filler = new Estreya.BlishHUD.EventTable.Models.Event
						{
							Name = (fillerItem.Name ?? ""),
							Duration = fillerItem.Duration,
							Filler = true
						};
						fillerItem.Occurences.ToList().ForEach(delegate(DateTimeOffset o)
						{
							filler.Occurences.Add(o.UtcDateTime);
						});
						parsedFillers.GetOrAdd(currentCategory.Key, (string key) => new List<Estreya.BlishHUD.EventTable.Models.Event> { filler }).Add(filler);
					}
				}
				return parsedFillers;
			}
			catch (FlurlHttpException ex)
			{
				string error = await ex.GetResponseStringAsync();
				Logger.Warn($"Could not load fillers from {ex.Call.Request.get_RequestUri()}: {error}");
			}
			return new ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>>();
		}

		private bool EventCategoryDisabled(EventCategory ec)
		{
			return _eventState?.Contains(Configuration.Name, ec.Key, EventState.EventStates.Completed) ?? false;
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
			return !(!Configuration.DisabledEventKeys.get_Value().Contains(settingKey) & !_eventState.Contains(Configuration.Name, settingKey, EventState.EventStates.Hidden));
		}

		private void UpdateEventsOnScreen(SpriteBatch spriteBatch)
		{
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			if (_clearing)
			{
				return;
			}
			(DateTime, DateTime, DateTime) times = GetTimes();
			_activeEvent = null;
			int y = 0;
			RectangleF renderRect = default(RectangleF);
			foreach (List<(DateTime, Event)> controlEventPairs in OrderedControlEvents)
			{
				if (controlEventPairs.Count == 0)
				{
					continue;
				}
				List<(DateTime, Event)> toDelete = new List<(DateTime, Event)>();
				foreach (var controlEvent in controlEventPairs)
				{
					if (EventDisabled(controlEvent.Item2.Ev))
					{
						toDelete.Add(controlEvent);
						continue;
					}
					float width = (float)controlEvent.Item2.Ev.CalculateWidth(controlEvent.Item1, times.Item2, base.Width, PixelPerMinute);
					if (width <= 0f)
					{
						toDelete.Add(controlEvent);
						continue;
					}
					float x = (float)controlEvent.Item2.Ev.CalculateXPosition(controlEvent.Item1, times.Item2, PixelPerMinute);
					((RectangleF)(ref renderRect))._002Ector((x < 0f) ? 0f : x, (float)y, width, (float)Configuration.EventHeight.get_Value());
					controlEvent.Item2.Render(spriteBatch, renderRect);
					RectangleF val = renderRect.ToBounds(RectangleF.op_Implicit(((Control)this).get_AbsoluteBounds()));
					if (((RectangleF)(ref val)).Contains(Point2.op_Implicit(GameService.Input.get_Mouse().get_Position())))
					{
						_activeEvent = controlEvent.Item2;
					}
				}
				foreach (var delete in toDelete)
				{
					Logger.Debug("Deleted event " + delete.Item2.Ev.Name);
					RemoveEventHooks(delete.Item2);
					delete.Item2.Dispose();
					controlEventPairs.Remove(delete);
				}
				y += Configuration.EventHeight.get_Value();
			}
			_heightFromLastDraw = ((y == 0) ? 1 : y);
			if (_activeEvent != null && _lastActiveEvent?.Ev?.Key != _activeEvent.Ev.Key)
			{
				bool valueOrDefault = (_activeEvent?.Ev?.Filler).GetValueOrDefault();
				Tooltip tooltip = ((Control)this).get_Tooltip();
				if (tooltip != null)
				{
					((Control)tooltip).Dispose();
				}
				((Control)this).set_Tooltip((Tooltip)null);
				if (!valueOrDefault)
				{
					((Control)this).set_Tooltip((!Configuration.ShowTooltips.get_Value()) ? null : _activeEvent?.BuildTooltip());
					((Control)this).set_Menu(_activeEvent?.BuildContextMenu());
				}
				_lastActiveEvent = _activeEvent;
			}
			else if (_activeEvent == null)
			{
				_lastActiveEvent = null;
			}
		}

		private void CheckForNewEventsForScreen()
		{
			if (_clearing)
			{
				return;
			}
			(DateTime Now, DateTime Min, DateTime Max) times = GetTimes();
			foreach (IGrouping<string, string> activeEventGroup in GetActiveEventKeysGroupedByCategory())
			{
				string categoryKey = activeEventGroup.Key;
				EventCategory validCategory = null;
				if (_eventLock.IsFree())
				{
					using (_eventLock.Lock())
					{
						validCategory = _allEvents.Find((EventCategory ec) => ec.Key == categoryKey);
					}
				}
				else
				{
					Logger.Debug("Event lock is busy. Can't update category " + categoryKey);
				}
				List<Estreya.BlishHUD.EventTable.Models.Event> events = validCategory?.Events.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => activeEventGroup.Any((string aeg) => aeg == ev.SettingKey) || (Configuration.UseFiller.get_Value() && ev.Filler)).ToList();
				if (events == null || events.Count == 0)
				{
					continue;
				}
				using (_controlLock.Lock())
				{
					if (_controlEvents.TryAdd(categoryKey, new List<(DateTime, Event)>()))
					{
						_orderedControlEvents = null;
					}
				}
				foreach (Estreya.BlishHUD.EventTable.Models.Event ev2 in events.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Occurences.Any((DateTime oc) => oc.AddMinutes(ev.Duration) >= times.Min && oc <= times.Max)))
				{
					if (EventDisabled(ev2))
					{
						continue;
					}
					foreach (DateTime occurence in ev2.Occurences.Where((DateTime oc) => oc.AddMinutes(ev2.Duration) >= times.Min && oc <= times.Max))
					{
						using (_controlLock.Lock())
						{
							if (_controlEvents[categoryKey].Any(((DateTime Occurence, Event Event) addedEvent) => addedEvent.Occurence == occurence))
							{
								continue;
							}
						}
						float num = (float)ev2.CalculateXPosition(occurence, times.Min, PixelPerMinute);
						float width = (float)ev2.CalculateWidth(occurence, times.Min, base.Width, PixelPerMinute);
						if (num > (float)base.Width || width <= 0f)
						{
							continue;
						}
						Event newEventControl = new Event(ev2, _iconState, _translationState, _getNowAction, occurence, occurence.AddMinutes(ev2.Duration), () => _fonts.GetOrAdd(Configuration.FontSize.get_Value(), (Func<FontSize, BitmapFont>)((FontSize fontSize) => GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0))), () => !ev2.Filler && Configuration.DrawBorders.get_Value(), () => _eventState.Contains(Configuration.Name, ev2.SettingKey, EventState.EventStates.Completed), delegate
						{
							//IL_0000: Unknown result type (might be due to invalid IL or missing references)
							//IL_0005: Unknown result type (might be due to invalid IL or missing references)
							//IL_006b: Unknown result type (might be due to invalid IL or missing references)
							//IL_0072: Unknown result type (might be due to invalid IL or missing references)
							//IL_0097: Unknown result type (might be due to invalid IL or missing references)
							//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
							//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
							//IL_011e: Unknown result type (might be due to invalid IL or missing references)
							Color black = Color.get_Black();
							return (!ev2.Filler) ? (((Configuration.TextColor.get_Value().get_Id() == 1) ? black : ColorExtensions.ToXnaColor(Configuration.TextColor.get_Value().get_Cloth())) * Configuration.EventTextOpacity.get_Value()) : (((Configuration.FillerTextColor.get_Value().get_Id() == 1) ? black : ColorExtensions.ToXnaColor(Configuration.FillerTextColor.get_Value().get_Cloth())) * Configuration.FillerTextOpacity.get_Value());
						}, delegate
						{
							//IL_000d: Unknown result type (might be due to invalid IL or missing references)
							//IL_0052: Unknown result type (might be due to invalid IL or missing references)
							//IL_007b: Unknown result type (might be due to invalid IL or missing references)
							if (ev2.Filler)
							{
								return Color.get_Transparent();
							}
							Color color = (string.IsNullOrWhiteSpace(ev2.BackgroundColorCode) ? Color.White : ColorTranslator.FromHtml(ev2.BackgroundColorCode));
							return new Color((int)color.R, (int)color.G, (int)color.B) * Configuration.EventBackgroundOpacity.get_Value();
						}, () => (!ev2.Filler) ? Configuration.DrawShadows.get_Value() : Configuration.DrawShadowsForFiller.get_Value(), () => (!ev2.Filler) ? (((Configuration.ShadowColor.get_Value().get_Id() == 1) ? Color.get_Black() : ColorExtensions.ToXnaColor(Configuration.ShadowColor.get_Value().get_Cloth())) * Configuration.ShadowOpacity.get_Value()) : (((Configuration.FillerShadowColor.get_Value().get_Id() == 1) ? Color.get_Black() : ColorExtensions.ToXnaColor(Configuration.FillerShadowColor.get_Value().get_Cloth())) * Configuration.FillerShadowOpacity.get_Value()));
						AddEventHooks(newEventControl);
						Logger.Debug($"Added event {ev2.Name} with occurence {occurence}");
						using (_controlLock.Lock())
						{
							_controlEvents[categoryKey].Add((occurence, newEventControl));
						}
					}
				}
			}
		}

		private void OnLeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (_activeEvent == null || _activeEvent.Ev.Filler)
			{
				return;
			}
			switch (Configuration.LeftClickAction.get_Value())
			{
			case LeftClickAction.CopyWaypoint:
				if (!string.IsNullOrWhiteSpace(_activeEvent.Ev.Waypoint))
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_activeEvent.Ev.Waypoint);
					ScreenNotification.ShowNotification(new string[2]
					{
						_activeEvent.Ev.Name,
						"Copied to clipboard!"
					});
				}
				break;
			case LeftClickAction.NavigateToWaypoint:
			{
				if (string.IsNullOrWhiteSpace(_activeEvent.Ev.Waypoint))
				{
					break;
				}
				if (_pointOfInterestState.Loading)
				{
					ScreenNotification.ShowNotification("PointOfInterestState is still loading!", ScreenNotification.NotificationType.Error);
					break;
				}
				PointOfInterest poi = _pointOfInterestState.GetPointOfInterest(_activeEvent.Ev.Waypoint);
				if (poi == null)
				{
					ScreenNotification.ShowNotification(_activeEvent.Ev.Waypoint + " not found!", ScreenNotification.NotificationType.Error);
					break;
				}
				Task.Run(async delegate
				{
					MapUtil.NavigationResult result = await (_mapUtil?.NavigateToPosition(poi, Configuration.AcceptWaypointPrompt.get_Value()) ?? Task.FromResult(new MapUtil.NavigationResult(success: false, "Variable null.")));
					if (!result.Success)
					{
						ScreenNotification.ShowNotification("Navigation failed: " + (result.Message ?? "Unknown"), ScreenNotification.NotificationType.Error);
					}
				});
				break;
			}
			}
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(UpdateEventOccurences, gameTime, _updateEventOccurencesInterval.TotalMilliseconds, _lastEventOccurencesUpdate);
			UpdateUtil.Update(CheckForNewEventsForScreen, gameTime, _checkForNewEventsInterval.TotalMilliseconds, ref _lastCheckForNewEventsUpdate);
			ReportNewHeight(_heightFromLastDraw);
		}

		protected override void DoPaint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			UpdateEventsOnScreen(spriteBatch);
			DrawTimeLine(spriteBatch);
		}

		private void DrawTimeLine(SpriteBatch spriteBatch)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			float middleLineX = (float)base.Width * GetTimeSpanRatio();
			float width = 2f;
			SpriteBatchUtil.DrawLine(spriteBatch, Textures.get_Pixel(), new RectangleF(middleLineX - width / 2f, 0f, width, (float)base.Height), Color.get_LightGray() * Configuration.TimeLineOpacity.get_Value());
		}

		private void ClearEventControls()
		{
			using (_eventLock.Lock())
			{
				_allEvents?.ForEach(delegate(EventCategory a)
				{
					a.UpdateFillers(new List<Estreya.BlishHUD.EventTable.Models.Event>());
				});
			}
			using (_controlLock.Lock())
			{
				_controlEvents?.Clear();
			}
			_orderedControlEvents = null;
		}

		private void AddEventHooks(Event ev)
		{
			ev.HideRequested += Ev_HideRequested;
			ev.FinishRequested += Ev_FinishRequested;
			ev.DisableRequested += Ev_DisableRequested;
		}

		private void RemoveEventHooks(Event ev)
		{
			ev.HideRequested -= Ev_HideRequested;
			ev.FinishRequested -= Ev_FinishRequested;
			ev.DisableRequested -= Ev_DisableRequested;
		}

		private void Ev_FinishRequested(object sender, EventArgs e)
		{
			Event ev = sender as Event;
			FinishEvent(ev.Ev, GetNextReset());
		}

		private void FinishEvent(Estreya.BlishHUD.EventTable.Models.Event ev, DateTime until)
		{
			switch (Configuration.CompletionAcion.get_Value())
			{
			case EventCompletedAction.Crossout:
				_eventState.Add(Configuration.Name, ev.SettingKey, until, EventState.EventStates.Completed);
				break;
			case EventCompletedAction.Hide:
				HideEvent(ev, until);
				break;
			}
		}

		private void HideEvent(Estreya.BlishHUD.EventTable.Models.Event ev, DateTime until)
		{
			_eventState.Add(Configuration.Name, ev.SettingKey, until, EventState.EventStates.Hidden);
		}

		private void Ev_HideRequested(object sender, EventArgs e)
		{
			Event ev = sender as Event;
			HideEvent(ev.Ev, GetNextReset());
		}

		private void Ev_DisableRequested(object sender, EventArgs e)
		{
			Event ev = sender as Event;
			if (!Configuration.DisabledEventKeys.get_Value().Contains(ev.Ev.SettingKey))
			{
				Configuration.DisabledEventKeys.set_Value(new List<string>(Configuration.DisabledEventKeys.get_Value()) { ev.Ev.SettingKey });
			}
		}

		private DateTime GetNextReset()
		{
			DateTime nowUTC = _getNowAction().ToUniversalTime();
			return new DateTime(nowUTC.Year, nowUTC.Month, nowUTC.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1.0);
		}

		protected override void InternalDispose()
		{
			ClearEventControls();
			if (_worldbossState != null)
			{
				_worldbossState.WorldbossCompleted -= Event_Completed;
				_worldbossState.WorldbossRemoved -= Event_Removed;
			}
			if (_mapchestState != null)
			{
				_mapchestState.MapchestCompleted -= Event_Completed;
				_mapchestState.MapchestRemoved -= Event_Removed;
			}
			if (_eventState != null)
			{
				_eventState.StateAdded -= EventState_StateAdded;
				_eventState.StateRemoved -= EventState_StateRemoved;
			}
			_iconState = null;
			_worldbossState = null;
			_mapchestState = null;
			_eventState = null;
			_mapUtil = null;
			_pointOfInterestState = null;
			_flurlClient = null;
			_apiRootUrl = null;
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
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
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			Configuration = null;
		}
	}
}
