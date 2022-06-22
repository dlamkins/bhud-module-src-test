using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.EventTable.Resources;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.EventTable.Utils;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	[Serializable]
	public class EventCategory
	{
		private static readonly Logger Logger = Logger.GetLogger<EventCategory>();

		private readonly TimeSpan updateInterval = TimeSpan.FromMinutes(15.0);

		private double timeSinceUpdate;

		[JsonProperty("events")]
		private List<Event> _originalEvents = new List<Event>();

		[JsonIgnore]
		private List<Event> _fillerEvents = new List<Event>();

		[JsonIgnore]
		private AsyncLock _eventLock = new AsyncLock();

		[JsonIgnore]
		private bool? _isDisabled;

		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		[JsonProperty("showCombined")]
		public bool ShowCombined { get; set; }

		[JsonIgnore]
		public List<Event> Events
		{
			get
			{
				return _originalEvents.Concat(_fillerEvents).ToList();
			}
			set
			{
				_originalEvents = value;
			}
		}

		[JsonIgnore]
		public bool IsDisabled
		{
			get
			{
				if (!_isDisabled.HasValue)
				{
					_isDisabled = EventTableModule.ModuleInstance.EventState.Contains(Key, EventState.EventStates.Hidden);
				}
				return _isDisabled.Value;
			}
		}

		public EventCategory()
		{
			timeSinceUpdate = updateInterval.TotalMilliseconds;
		}

		private void ModuleSettings_EventSettingChanged(object sender, ModuleSettings.EventSettingsChangedEventArgs e)
		{
			List<Event> changedEvents = _originalEvents.Where((Event ev) => ev.SettingKey.ToLowerInvariant() == e.Name.ToLowerInvariant()).ToList();
			foreach (Event item in changedEvents)
			{
				item.ResetCachedStates();
			}
			if (changedEvents.Count > 0)
			{
				UpdateEventOccurences(null);
			}
		}

		private void UpdateEventOccurences(GameTime gameTime)
		{
			lock (_fillerEvents)
			{
				_fillerEvents.Clear();
			}
			List<Event> activeEvents = _originalEvents.Where((Event e) => !e.IsDisabled).ToList();
			List<KeyValuePair<DateTime, Event>> activeEventStarts = new List<KeyValuePair<DateTime, Event>>();
			foreach (Event activeEvent in activeEvents)
			{
				activeEvent.Occurences.Where((DateTime oc) => (oc >= EventTableModule.ModuleInstance.EventTimeMin || oc.AddMinutes(activeEvent.Duration) >= EventTableModule.ModuleInstance.EventTimeMin) && oc <= EventTableModule.ModuleInstance.EventTimeMax).ToList().ForEach(delegate(DateTime eo)
				{
					activeEventStarts.Add(new KeyValuePair<DateTime, Event>(eo, activeEvent));
				});
			}
			activeEventStarts = activeEventStarts.OrderBy((KeyValuePair<DateTime, Event> aes) => aes.Key).ToList();
			List<KeyValuePair<DateTime, Event>> modifiedEventStarts = activeEventStarts.ToList();
			for (int i = 0; i < activeEventStarts.Count - 1; i++)
			{
				KeyValuePair<DateTime, Event> currentEvent = activeEventStarts.ElementAt(i);
				KeyValuePair<DateTime, Event> nextEvent = activeEventStarts.ElementAt(i + 1);
				DateTime currentEnd = currentEvent.Key + TimeSpan.FromMinutes(currentEvent.Value.Duration);
				TimeSpan gap = nextEvent.Key - currentEnd;
				if (gap > TimeSpan.Zero)
				{
					Event filler = new Event
					{
						Name = currentEvent.Value.Name + " - " + nextEvent.Value.Name,
						Duration = (int)gap.TotalMinutes,
						Filler = true,
						EventCategory = this
					};
					modifiedEventStarts.Insert(i + 1, new KeyValuePair<DateTime, Event>(currentEnd, filler));
				}
			}
			if (activeEventStarts.Count > 1)
			{
				KeyValuePair<DateTime, Event> firstEvent = activeEventStarts.FirstOrDefault();
				KeyValuePair<DateTime, Event> lastEvent = activeEventStarts.LastOrDefault();
				KeyValuePair<DateTime, Event> nextEvent4 = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.Occurences.Where((DateTime oc) => oc > lastEvent.Key && oc < EventTableModule.ModuleInstance.EventTimeMax.AddDays(2.0)).FirstOrDefault(), ae) into aeo
					orderby aeo.Key
					select aeo).First();
				KeyValuePair<DateTime, Event> nextEventMapping3 = new KeyValuePair<DateTime, Event>(nextEvent4.Key, nextEvent4.Value);
				DateTime nextStart3 = nextEventMapping3.Key;
				_ = nextStart3 + TimeSpan.FromMinutes(nextEventMapping3.Value.Duration);
				if (nextStart3 - lastEvent.Key > TimeSpan.Zero)
				{
					Event filler6 = new Event
					{
						Name = lastEvent.Value.Name + " - " + nextEventMapping3.Value.Name,
						Filler = true,
						EventCategory = this,
						Duration = (int)(nextStart3 - lastEvent.Key).TotalMinutes
					};
					modifiedEventStarts.Add(new KeyValuePair<DateTime, Event>(lastEvent.Key + TimeSpan.FromMinutes(lastEvent.Value.Duration), filler6));
				}
				KeyValuePair<DateTime, Event> prevEvent3 = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.Occurences.Where((DateTime oc) => oc > EventTableModule.ModuleInstance.EventTimeMin.AddDays(-2.0) && oc < firstEvent.Key).LastOrDefault(), ae) into aeo
					orderby aeo.Key
					select aeo).Last();
				KeyValuePair<DateTime, Event> prevEventMapping3 = new KeyValuePair<DateTime, Event>(prevEvent3.Key, prevEvent3.Value);
				DateTime prevEnd3 = prevEventMapping3.Key + TimeSpan.FromMinutes(prevEventMapping3.Value.Duration);
				if (firstEvent.Key - prevEnd3 > TimeSpan.Zero)
				{
					Event filler5 = new Event
					{
						Name = prevEventMapping3.Value.Name + " - " + firstEvent.Value.Name,
						Filler = true,
						Duration = (int)(firstEvent.Key - prevEnd3).TotalMinutes,
						EventCategory = this
					};
					modifiedEventStarts.Add(new KeyValuePair<DateTime, Event>(prevEnd3, filler5));
				}
			}
			else if (activeEventStarts.Count == 1 && activeEvents.Count >= 1)
			{
				KeyValuePair<DateTime, Event> currentEvent2 = activeEventStarts.First();
				DateTime currentStart = currentEvent2.Key;
				DateTime currentEnd2 = currentStart + TimeSpan.FromMinutes(currentEvent2.Value.Duration);
				KeyValuePair<DateTime, Event> nextEvent3 = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.Occurences.Where((DateTime oc) => oc > currentEnd2 && oc < EventTableModule.ModuleInstance.EventTimeMax.AddDays(2.0)).FirstOrDefault(), ae) into aeo
					orderby aeo.Key
					select aeo).First();
				KeyValuePair<DateTime, Event> nextEventMapping2 = new KeyValuePair<DateTime, Event>(nextEvent3.Key, nextEvent3.Value);
				DateTime nextStart2 = nextEventMapping2.Key;
				_ = nextStart2 + TimeSpan.FromMinutes(nextEventMapping2.Value.Duration);
				if (nextStart2 - currentEnd2 > TimeSpan.Zero)
				{
					Event filler4 = new Event
					{
						Name = currentEvent2.Value.Name + " - " + nextEventMapping2.Value.Name,
						Filler = true,
						Duration = (int)(nextStart2 - currentEnd2).TotalMinutes,
						EventCategory = this
					};
					modifiedEventStarts.Add(new KeyValuePair<DateTime, Event>(currentEnd2, filler4));
				}
				KeyValuePair<DateTime, Event> prevEvent2 = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.Occurences.Where((DateTime oc) => oc > EventTableModule.ModuleInstance.EventTimeMin.AddDays(-2.0) && oc < currentStart).LastOrDefault(), ae) into aeo
					orderby aeo.Key
					select aeo).Last();
				KeyValuePair<DateTime, Event> prevEventMapping2 = new KeyValuePair<DateTime, Event>(prevEvent2.Key, prevEvent2.Value);
				DateTime prevEnd2 = prevEventMapping2.Key + TimeSpan.FromMinutes(prevEventMapping2.Value.Duration);
				if (currentStart - prevEnd2 > TimeSpan.Zero)
				{
					Event filler3 = new Event
					{
						Name = prevEventMapping2.Value.Name + " - " + currentEvent2.Value.Name,
						Filler = true,
						Duration = (int)(currentStart - prevEnd2).TotalMinutes,
						EventCategory = this
					};
					modifiedEventStarts.Add(new KeyValuePair<DateTime, Event>(prevEnd2, filler3));
				}
			}
			else if (activeEventStarts.Count == 0 && activeEvents.Count >= 1)
			{
				KeyValuePair<DateTime, Event> prevEvent = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.Occurences.Where((DateTime oc) => oc > EventTableModule.ModuleInstance.EventTimeMin.AddDays(-2.0) && oc < EventTableModule.ModuleInstance.EventTimeMax).LastOrDefault(), ae) into aeo
					orderby aeo.Key
					select aeo).Last();
				KeyValuePair<DateTime, Event> nextEvent2 = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.Occurences.Where((DateTime oc) => oc > EventTableModule.ModuleInstance.EventTimeMin && oc < EventTableModule.ModuleInstance.EventTimeMax.AddDays(2.0)).FirstOrDefault(), ae) into aeo
					orderby aeo.Key
					select aeo).First();
				KeyValuePair<DateTime, Event> prevEventMapping = new KeyValuePair<DateTime, Event>(prevEvent.Key, prevEvent.Value);
				KeyValuePair<DateTime, Event> nextEventMapping = new KeyValuePair<DateTime, Event>(nextEvent2.Key, nextEvent2.Value);
				DateTime prevEnd = prevEventMapping.Key + TimeSpan.FromMinutes(prevEventMapping.Value.Duration);
				DateTime nextStart = nextEventMapping.Key;
				Event filler2 = new Event
				{
					Name = prevEventMapping.Value.Name + " - " + nextEventMapping.Value.Name,
					Duration = (int)(nextStart - prevEnd).TotalMinutes,
					Filler = true,
					EventCategory = this
				};
				modifiedEventStarts.Add(new KeyValuePair<DateTime, Event>(prevEnd, filler2));
			}
			lock (_fillerEvents)
			{
				modifiedEventStarts.Where((KeyValuePair<DateTime, Event> e) => e.Value.Filler).ToList().ForEach(delegate(KeyValuePair<DateTime, Event> modEvent)
				{
					modEvent.Value.Occurences.Add(modEvent.Key);
				});
				IEnumerable<Event> modifiedEvents = from e in modifiedEventStarts
					where e.Value.Filler
					select e.Value;
				_fillerEvents.AddRange(modifiedEvents);
			}
		}

		public void Hide()
		{
			DateTime now = EventTableModule.ModuleInstance.DateTimeNow.ToUniversalTime();
			DateTime until = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1.0);
			EventTableModule.ModuleInstance.EventState.Add(Key, until, EventState.EventStates.Hidden);
		}

		public void Finish()
		{
			DateTime now = EventTableModule.ModuleInstance.DateTimeNow.ToUniversalTime();
			DateTime until = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1.0);
			EventTableModule.ModuleInstance.EventState.Add(Key, until, EventState.EventStates.Completed);
		}

		public void Unfinish()
		{
			EventTableModule.ModuleInstance.EventState.Remove(Key);
		}

		public bool IsFinished()
		{
			return EventTableModule.ModuleInstance.EventState.Contains(Key, EventState.EventStates.Completed);
		}

		public async Task LoadAsync()
		{
			Logger.Debug("Load event category: {0}", new object[1] { Key });
			lock (_originalEvents)
			{
				foreach (Event originalEvent in _originalEvents)
				{
					originalEvent.EventCategory = this;
				}
			}
			if (EventTableModule.ModuleInstance.ModuleSettings.UseEventTranslation.get_Value())
			{
				Name = Strings.ResourceManager.GetString("eventCategory-" + Key) ?? Name;
			}
			EventTableModule.ModuleInstance.ModuleSettings.EventSettingChanged += ModuleSettings_EventSettingChanged;
			EventTableModule.ModuleInstance.EventState.StateAdded += EventState_StateAdded;
			EventTableModule.ModuleInstance.EventState.StateRemoved += EventState_StateRemoved;
			using (await _eventLock.LockAsync())
			{
				await Task.WhenAll(Events.Select((Event ev) => ev.LoadAsync()));
			}
			Logger.Debug("Loaded event category: {0}", new object[1] { Key });
		}

		private void EventState_StateRemoved(object sender, ValueEventArgs<string> e)
		{
			if (e.get_Value() == Key)
			{
				_isDisabled = null;
			}
		}

		private void EventState_StateAdded(object sender, ValueEventArgs<EventState.VisibleStateInfo> e)
		{
			if (e.get_Value().Key == Key && e.get_Value().State == EventState.EventStates.Hidden)
			{
				_isDisabled = null;
			}
		}

		public void Unload()
		{
			Logger.Debug("Unload event category: {0}", new object[1] { Key });
			EventTableModule.ModuleInstance.ModuleSettings.EventSettingChanged -= ModuleSettings_EventSettingChanged;
			Events.ForEach(delegate(Event ev)
			{
				ev.Unload();
			});
			Logger.Debug("Unloaded event category: {0}", new object[1] { Key });
		}

		public void Update(GameTime gameTime)
		{
			Events.ForEach(delegate(Event ev)
			{
				ev.Update(gameTime);
			});
			UpdateUtil.Update(UpdateEventOccurences, gameTime, updateInterval.TotalMilliseconds, ref timeSinceUpdate);
		}
	}
}
