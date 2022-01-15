using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	[Serializable]
	public class EventCategory
	{
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("showCombined")]
		public bool ShowCombined { get; set; }

		[JsonProperty("events")]
		public List<Event> Events { get; set; }

		public List<KeyValuePair<DateTime, Event>> GetEventOccurences(List<SettingEntry<bool>> eventSettings, DateTime now, DateTime max, DateTime min, bool fillGaps)
		{
			List<Event> activeEvents = Events.Where((Event e) => !e.IsDisabled()).ToList();
			List<KeyValuePair<DateTime, Event>> activeEventStarts = new List<KeyValuePair<DateTime, Event>>();
			foreach (Event activeEvent in activeEvents)
			{
				activeEvent.GetStartOccurences(now, max, min).ForEach(delegate(DateTime eo)
				{
					activeEventStarts.Add(new KeyValuePair<DateTime, Event>(eo, activeEvent));
				});
			}
			activeEventStarts = activeEventStarts.OrderBy((KeyValuePair<DateTime, Event> aes) => aes.Key).ToList();
			if (!fillGaps)
			{
				return activeEventStarts.ToList();
			}
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
						Filler = true
					};
					modifiedEventStarts.Insert(i + 1, new KeyValuePair<DateTime, Event>(currentEnd, filler));
				}
			}
			if (activeEventStarts.Count > 1)
			{
				KeyValuePair<DateTime, Event> firstEvent = activeEventStarts.FirstOrDefault();
				KeyValuePair<DateTime, Event> lastEvent = activeEventStarts.LastOrDefault();
				KeyValuePair<DateTime, Event> nextEvent4 = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.GetStartOccurences(now, max.AddDays(2.0), lastEvent.Key, addTimezoneOffset: true, limitsBetweenRanges: true).FirstOrDefault(), ae) into aeo
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
						Duration = (int)(nextStart3 - lastEvent.Key).TotalMinutes
					};
					modifiedEventStarts.Add(new KeyValuePair<DateTime, Event>(lastEvent.Key + TimeSpan.FromMinutes(lastEvent.Value.Duration), filler6));
				}
				KeyValuePair<DateTime, Event> prevEvent3 = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.GetStartOccurences(now, firstEvent.Key, min.AddDays(-2.0), addTimezoneOffset: true, limitsBetweenRanges: true).LastOrDefault(), ae) into aeo
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
						Duration = (int)(firstEvent.Key - prevEnd3).TotalMinutes
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
					select new KeyValuePair<DateTime, Event>(ae.GetStartOccurences(now, max.AddDays(2.0), currentEnd2, addTimezoneOffset: true, limitsBetweenRanges: true).FirstOrDefault(), ae) into aeo
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
						Duration = (int)(nextStart2 - currentEnd2).TotalMinutes
					};
					modifiedEventStarts.Add(new KeyValuePair<DateTime, Event>(currentEnd2, filler4));
				}
				KeyValuePair<DateTime, Event> prevEvent2 = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.GetStartOccurences(now, currentStart, min.AddDays(-2.0), addTimezoneOffset: true, limitsBetweenRanges: true).LastOrDefault(), ae) into aeo
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
						Duration = (int)(currentStart - prevEnd2).TotalMinutes
					};
					modifiedEventStarts.Add(new KeyValuePair<DateTime, Event>(prevEnd2, filler3));
				}
			}
			else if (activeEventStarts.Count == 0 && activeEvents.Count >= 1)
			{
				KeyValuePair<DateTime, Event> prevEvent = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.GetStartOccurences(now, now, min.AddDays(-2.0), addTimezoneOffset: true, limitsBetweenRanges: true).LastOrDefault(), ae) into aeo
					orderby aeo.Key
					select aeo).Last();
				KeyValuePair<DateTime, Event> nextEvent2 = (from ae in activeEvents
					select new KeyValuePair<DateTime, Event>(ae.GetStartOccurences(now, max.AddDays(2.0), now, addTimezoneOffset: true, limitsBetweenRanges: true).FirstOrDefault(), ae) into aeo
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
					Filler = true
				};
				modifiedEventStarts.Add(new KeyValuePair<DateTime, Event>(prevEnd, filler2));
			}
			return modifiedEventStarts.OrderBy((KeyValuePair<DateTime, Event> mes) => mes.Key).ToList();
		}
	}
}
