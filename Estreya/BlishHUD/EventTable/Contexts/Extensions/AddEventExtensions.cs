using System;
using System.Collections.Generic;

namespace Estreya.BlishHUD.EventTable.Contexts.Extensions
{
	public static class AddEventExtensions
	{
		public static AddEvent GenerateEventFiller(this AddEvent currentEvent, AddEvent nextEvent, int fromOccurenceIndex = 0, int toOccurenceIndex = 0)
		{
			if (string.IsNullOrWhiteSpace(currentEvent.CategoryKey))
			{
				throw new ArgumentException("CategoryKey of the current event is empty or null.");
			}
			if (string.IsNullOrWhiteSpace(nextEvent.CategoryKey))
			{
				throw new ArgumentException("CategoryKey of the next event is empty or null.");
			}
			if (currentEvent.CategoryKey != nextEvent.CategoryKey)
			{
				throw new ArgumentException("CategoryKey of events is not the same.");
			}
			if (string.IsNullOrWhiteSpace(currentEvent.Key))
			{
				throw new ArgumentException("Key of the current event is empty or null.");
			}
			if (string.IsNullOrWhiteSpace(nextEvent.Key))
			{
				throw new ArgumentException("Key of the next event is empty or null.");
			}
			if (currentEvent.Occurences == null || currentEvent.Occurences.Count < toOccurenceIndex)
			{
				throw new ArgumentOutOfRangeException("fromOccurenceIndex", $"Current event has no occurence at index {fromOccurenceIndex}");
			}
			if (nextEvent.Occurences == null || nextEvent.Occurences.Count < toOccurenceIndex)
			{
				throw new ArgumentOutOfRangeException("toOccurenceIndex", $"Next event has no occurence at index {toOccurenceIndex}");
			}
			AddEvent result = new AddEvent();
			result.CategoryKey = currentEvent.CategoryKey;
			result.Key = $"{currentEvent.Key}-{nextEvent.Key}_{Guid.NewGuid()}";
			result.Name = currentEvent.Name + " - " + nextEvent.Name;
			result.Duration = Math.Max(0, (int)(nextEvent.Occurences[toOccurenceIndex] - currentEvent.Occurences[fromOccurenceIndex]).TotalMinutes);
			result.Filler = true;
			result.Occurences = new List<DateTime> { currentEvent.Occurences[fromOccurenceIndex].AddMinutes(currentEvent.Duration) };
			return result;
		}

		public static AddEvent GenerateEventFiller(this AddEvent currentEvent, int fromOccurenceIndex = 0, int toOccurenceIndex = 1)
		{
			return currentEvent.GenerateEventFiller(currentEvent, fromOccurenceIndex, toOccurenceIndex);
		}

		public static IEnumerable<AddEvent> GenerateEventFillers(this AddEvent currentEvent)
		{
			List<AddEvent> filler = new List<AddEvent>();
			if (currentEvent.Occurences == null)
			{
				return filler;
			}
			for (int i = 0; i < currentEvent.Occurences.Count - 1; i++)
			{
				int fromIndex = i;
				int toIndex = i + 1;
				filler.Add(currentEvent.GenerateEventFiller(fromIndex, toIndex));
			}
			return filler;
		}
	}
}
