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
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Newtonsoft.Json;
using SemVer;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class EventArea : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<EventArea>();

		private static TimeSpan _updateEventOccurencesInterval = TimeSpan.FromMinutes(15.0);

		private AsyncRef<double> _lastEventOccurencesUpdate = new AsyncRef<double>(0.0);

		private static TimeSpan _updateEventInterval = TimeSpan.FromMilliseconds(250.0);

		private double _lastEventUpdate;

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

		private List<EventCategory> _allEvents = new List<EventCategory>();

		private ConcurrentDictionary<string, List<(DateTime Occurence, Event Event)>> _controlEvents = new ConcurrentDictionary<string, List<(DateTime, Event)>>();

		private bool _clearing;

		public bool Enabled => Configuration?.Enabled.get_Value() ?? false;

		private double PixelPerMinute => (double)((Control)this).get_Size().X / (double)Configuration.TimeSpan.get_Value();

		public EventAreaConfiguration Configuration { get; private set; }

		public EventArea(EventAreaConfiguration configuration, IconState iconState, TranslationState translationState, EventState eventState, WorldbossState worldbossState, MapchestState mapchestState, PointOfInterestState pointOfInterestState, MapUtil mapUtil, IFlurlClient flurlClient, string apiRootUrl, Func<DateTime> getNowAction, Func<Version> getVersion)
			: this()
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
			Location_SettingChanged(this, null);
			Size_SettingChanged(this, null);
			Opacity_SettingChanged(this, new ValueChangedEventArgs<float>(0f, Configuration.Opacity.get_Value()));
			BackgroundColor_SettingChanged(this, new ValueChangedEventArgs<Color>((Color)null, Configuration.BackgroundColor.get_Value()));
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
			_allEvents.Clear();
			_allEvents.AddRange(JsonConvert.DeserializeObject<List<EventCategory>>(JsonConvert.SerializeObject(allEvents)));
			GetTimes();
			_allEvents.ForEach(delegate(EventCategory ec)
			{
				ec.Load(_translationState);
			});
			ReAddEvents();
		}

		private void Event_Removed(object sender, string apiCode)
		{
			(from ev in _allEvents.SelectMany((EventCategory ec) => ec.Events)
				where ev.APICode == apiCode
				select ev).ToList().ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
			{
				_eventState.Remove(Configuration.Name, ev.SettingKey);
			});
		}

		private void Event_Completed(object sender, string apiCode)
		{
			DateTime until = GetNextReset();
			(from ev in _allEvents.SelectMany((EventCategory ec) => ec.Events)
				where ev.APICode == apiCode
				select ev).ToList().ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
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
			if (((Control)this).get_Height() != height)
			{
				((Control)this).set_Height(height);
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
			((Control)this).set_Location(buildFromBottom ? new Point(Configuration.Location.X.get_Value(), Configuration.Location.Y.get_Value() - ((Control)this).get_Height()) : new Point(Configuration.Location.X.get_Value(), Configuration.Location.Y.get_Value()));
		}

		private void Size_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(Configuration.Size.X.get_Value(), ((Control)this).get_Height()));
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
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
			return (from e in _allEvents.SelectMany((EventCategory ae) => ae.Events)
				where !e.Filler
				select e.SettingKey into sk
				where !Configuration.DisabledEventKeys.get_Value().Contains(sk)
				select sk).ToList();
		}

		private void ReAddEvents()
		{
			_clearing = true;
			using (((Control)this).SuspendLayoutContext())
			{
				ClearEventControls();
				_lastEventOccurencesUpdate.Value = _updateEventOccurencesInterval.TotalMilliseconds;
				_lastEventUpdate = _updateEventInterval.TotalMilliseconds;
				_clearing = false;
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			float middleLineX = (float)((Control)this).get_Width() * GetTimeSpanRatio();
			float width = 2f;
			SpriteBatchUtil.DrawLineOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new RectangleF(middleLineX - width / 2f, 0f, width, (float)((Control)this).get_Height()), Color.get_LightGray());
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
			ConcurrentDictionary<string, List<Estreya.BlishHUD.EventTable.Models.Event>> fillers = await GetFillers(times.Item1, times.Item2, times.Item3, activeEventKeys.Where((string ev) => !EventDisabled(ev)).ToList());
			foreach (EventCategory ec in _allEvents)
			{
				if (fillers.TryGetValue(ec.Key, out var categoryFillers))
				{
					categoryFillers.ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event cf)
					{
						cf.Load(ec, _translationState);
					});
				}
				ec.UpdateFillers(categoryFillers);
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
				IFlurlRequest request = _flurlClient.Request(_apiRootUrl, "fillers");
				List<Estreya.BlishHUD.EventTable.Models.Event> activeEvents = (from ev in _allEvents.SelectMany((EventCategory a) => a.Events)
					where activeEventKeys.Any((string aeg) => aeg == ev.SettingKey)
					select ev).ToList();
				List<OnlineFillerCategory> fillerList = (await (await request.PostJsonAsync(new OnlineFillerRequest
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
			if (!ev.Filler)
			{
				return EventDisabled(ev.SettingKey);
			}
			return false;
		}

		private bool EventDisabled(string settingKey)
		{
			return !(!Configuration.DisabledEventKeys.get_Value().Contains(settingKey) & !_eventState.Contains(Configuration.Name, settingKey, EventState.EventStates.Hidden));
		}

		private void UpdateEventsOnScreen()
		{
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			if (_clearing)
			{
				return;
			}
			(DateTime Now, DateTime Min, DateTime Max) times = GetTimes();
			int y = 0;
			List<string> order = GetEventCategoryOrdering();
			foreach (List<(DateTime, Event)> controlEventPairs in (from x in _controlEvents
				orderby order.IndexOf(x.Key)
				select x.Value).ToList())
			{
				if (controlEventPairs.Count == 0)
				{
					continue;
				}
				List<(DateTime, Event)> toDelete = new List<(DateTime, Event)>();
				foreach (var controlEvent in controlEventPairs)
				{
					bool disabled = EventDisabled(controlEvent.Item2.Ev);
					if (disabled)
					{
						toDelete.Add(controlEvent);
						continue;
					}
					int x3 = (int)Math.Ceiling(controlEvent.Item2.Ev.CalculateXPosition(controlEvent.Item1, times.Min, PixelPerMinute));
					int width2 = (int)Math.Ceiling(controlEvent.Item2.Ev.CalculateWidth(controlEvent.Item1, times.Min, ((Control)this).get_Width(), PixelPerMinute));
					((Control)controlEvent.Item2).set_Location(new Point((x3 >= 0) ? x3 : 0, y));
					controlEvent.Item2.Size = new Point(width2, Configuration.EventHeight.get_Value());
					if (width2 <= 0 || disabled)
					{
						toDelete.Add(controlEvent);
					}
				}
				foreach (var delete in toDelete)
				{
					Logger.Debug("Deleted event " + delete.Item2.Ev.Name);
					RemoveEventHooks(delete.Item2);
					((Control)delete.Item2).Dispose();
					controlEventPairs.Remove(delete);
				}
				y += Configuration.EventHeight.get_Value();
			}
			y = 0;
			foreach (IGrouping<string, string> activeEventGroup in GetActiveEventKeysGroupedByCategory())
			{
				string categoryKey = activeEventGroup.Key;
				EventCategory eventCategory = _allEvents.Find((EventCategory ec) => ec.Key == categoryKey);
				bool renderedAny = false;
				List<Estreya.BlishHUD.EventTable.Models.Event> events = eventCategory.Events.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => activeEventGroup.Any((string aeg) => aeg == ev.SettingKey) || (Configuration.UseFiller.get_Value() && ev.Filler)).ToList();
				if (events.Count == 0)
				{
					continue;
				}
				_controlEvents.TryAdd(categoryKey, new List<(DateTime, Event)>());
				foreach (Estreya.BlishHUD.EventTable.Models.Event ev2 in events.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Occurences.Any((DateTime oc) => oc.AddMinutes(ev.Duration) >= times.Min && oc <= times.Max)))
				{
					if (EventDisabled(ev2))
					{
						continue;
					}
					foreach (DateTime occurence in ev2.Occurences.Where((DateTime oc) => oc.AddMinutes(ev2.Duration) >= times.Min && oc <= times.Max))
					{
						if (_controlEvents[categoryKey].Any(((DateTime Occurence, Event Event) addedEvent) => addedEvent.Occurence == occurence))
						{
							continue;
						}
						int x2 = (int)Math.Ceiling(ev2.CalculateXPosition(occurence, times.Min, PixelPerMinute));
						int width = (int)Math.Ceiling(ev2.CalculateWidth(occurence, times.Min, ((Control)this).get_Width(), PixelPerMinute));
						if (x2 > ((Control)this).get_Width() || width <= 0)
						{
							continue;
						}
						Event @event = new Event(ev2, _iconState, _translationState, _getNowAction, occurence, occurence.AddMinutes(ev2.Duration), () => _fonts.GetOrAdd(Configuration.FontSize.get_Value(), (Func<FontSize, BitmapFont>)((FontSize fontSize) => GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0))), () => !ev2.Filler && Configuration.DrawBorders.get_Value(), () => _eventState.Contains(Configuration.Name, ev2.SettingKey, EventState.EventStates.Completed), delegate
						{
							//IL_0000: Unknown result type (might be due to invalid IL or missing references)
							//IL_0005: Unknown result type (might be due to invalid IL or missing references)
							//IL_0068: Unknown result type (might be due to invalid IL or missing references)
							//IL_006e: Unknown result type (might be due to invalid IL or missing references)
							//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
							//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
							Color black = Color.get_Black();
							if (!ev2.Filler)
							{
								if (Configuration.TextColor.get_Value().get_Id() != 1)
								{
									return ColorExtensions.ToXnaColor(Configuration.TextColor.get_Value().get_Cloth());
								}
								return black;
							}
							return (Configuration.FillerTextColor.get_Value().get_Id() != 1) ? ColorExtensions.ToXnaColor(Configuration.FillerTextColor.get_Value().get_Cloth()) : black;
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
							return new Color((int)color.R, (int)color.G, (int)color.B) * Configuration.EventOpacity.get_Value();
						}, () => (!ev2.Filler) ? Configuration.DrawShadows.get_Value() : Configuration.DrawShadowsForFiller.get_Value(), delegate
						{
							//IL_0062: Unknown result type (might be due to invalid IL or missing references)
							//IL_0068: Unknown result type (might be due to invalid IL or missing references)
							//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
							//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
							if (!ev2.Filler)
							{
								if (Configuration.ShadowColor.get_Value().get_Id() != 1)
								{
									return ColorExtensions.ToXnaColor(Configuration.ShadowColor.get_Value().get_Cloth());
								}
								return Color.get_Black();
							}
							return (Configuration.FillerShadowColor.get_Value().get_Id() != 1) ? ColorExtensions.ToXnaColor(Configuration.FillerShadowColor.get_Value().get_Cloth()) : Color.get_Black();
						}, () => Configuration.ShowTooltips.get_Value());
						((Control)@event).set_Parent((Container)(object)this);
						((Control)@event).set_Top(y);
						@event.Height = Configuration.EventHeight.get_Value();
						@event.Width = width;
						((Control)@event).set_Left((x2 >= 0) ? x2 : 0);
						((Control)@event).set_ClipsBounds(false);
						Event newEventControl = @event;
						AddEventHooks(newEventControl);
						Logger.Debug($"Added event {ev2.Name} with occurence {occurence}");
						_controlEvents[categoryKey].Add((occurence, newEventControl));
					}
					renderedAny = true;
				}
				if (renderedAny)
				{
					y += Configuration.EventHeight.get_Value();
				}
			}
			ReportNewHeight(y);
		}

		private void EventControl_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			Event eventControl = sender as Event;
			switch (Configuration.LeftClickAction.get_Value())
			{
			case LeftClickAction.CopyWaypoint:
				if (!string.IsNullOrWhiteSpace(eventControl.Ev.Waypoint))
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(eventControl.Ev.Waypoint);
					ScreenNotification.ShowNotification(new string[2]
					{
						eventControl.Ev.Name,
						"Copied to clipboard!"
					});
				}
				break;
			case LeftClickAction.NavigateToWaypoint:
			{
				if (string.IsNullOrWhiteSpace(eventControl.Ev.Waypoint))
				{
					break;
				}
				if (_pointOfInterestState.Loading)
				{
					ScreenNotification.ShowNotification("PointOfInterestState is still loading!", ScreenNotification.NotificationType.Error);
					break;
				}
				PointOfInterest poi = _pointOfInterestState.GetPointOfInterest(eventControl.Ev.Waypoint);
				if (poi == null)
				{
					ScreenNotification.ShowNotification(eventControl.Ev.Waypoint + " not found!", ScreenNotification.NotificationType.Error);
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

		public override void UpdateContainer(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(UpdateEventOccurences, gameTime, _updateEventOccurencesInterval.TotalMilliseconds, _lastEventOccurencesUpdate);
			UpdateUtil.Update(UpdateEventsOnScreen, gameTime, _updateEventInterval.TotalMilliseconds, ref _lastEventUpdate);
		}

		private void ClearEventControls()
		{
			((Container)this).get_Children().ToList().ForEach(delegate(Control child)
			{
				Event ev = child as Event;
				RemoveEventHooks(ev);
				child.Dispose();
			});
			_allEvents.ForEach(delegate(EventCategory a)
			{
				a.UpdateFillers(new List<Estreya.BlishHUD.EventTable.Models.Event>());
			});
			((Container)this).get_Children().Clear();
			_controlEvents.Clear();
		}

		private void AddEventHooks(Event ev)
		{
			((Control)ev).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)EventControl_LeftMouseButtonPressed);
			ev.HideRequested += Ev_HideRequested;
			ev.FinishRequested += Ev_FinishRequested;
			ev.DisableRequested += Ev_DisableRequested;
		}

		private void RemoveEventHooks(Event ev)
		{
			((Control)ev).remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)EventControl_LeftMouseButtonPressed);
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
			ReAddEvents();
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

		protected override void DisposeControl()
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
			_iconState = null;
			_worldbossState = null;
			_mapchestState = null;
			_eventState = null;
			_mapUtil = null;
			_pointOfInterestState = null;
			_flurlClient = null;
			_apiRootUrl = null;
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
			Configuration = null;
		}
	}
}
