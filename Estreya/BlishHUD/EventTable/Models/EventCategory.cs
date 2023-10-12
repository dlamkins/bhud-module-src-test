using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Attributes;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	[Serializable]
	public class EventCategory
	{
		[IgnoreCopy]
		private static readonly Logger Logger = Logger.GetLogger<EventCategory>();

		[JsonIgnore]
		private AsyncLock _eventLock = new AsyncLock();

		[JsonProperty("fillers")]
		public List<Event> FillerEvents { get; private set; } = new List<Event>();


		[JsonProperty("events")]
		public List<Event> OriginalEvents { get; private set; } = new List<Event>();


		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		[JsonProperty("showCombined")]
		public bool ShowCombined { get; set; }

		[JsonProperty("fromContext")]
		internal bool FromContext { get; set; }

		[JsonIgnore]
		public List<Event> Events => OriginalEvents.Concat(FillerEvents).ToList();

		public void UpdateFillers(List<Event> fillers)
		{
			lock (FillerEvents)
			{
				FillerEvents.Clear();
				if (fillers != null)
				{
					FillerEvents.AddRange(fillers);
				}
			}
		}

		public void UpdateOriginalEvents(List<Event> events)
		{
			lock (OriginalEvents)
			{
				OriginalEvents.Clear();
				if (events != null)
				{
					OriginalEvents.AddRange(events);
				}
			}
		}

		public void Load(Func<DateTime> getNowAction, TranslationService translationService = null)
		{
			if (translationService != null)
			{
				Name = translationService.GetTranslation("eventCategory-" + Key + "-name", Name);
			}
			using (_eventLock.Lock())
			{
				Events.ForEach(delegate(Event ev)
				{
					ev.Load(this, getNowAction, translationService);
				});
			}
		}
	}
}
