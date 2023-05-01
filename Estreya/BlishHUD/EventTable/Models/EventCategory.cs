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

		[JsonProperty("events")]
		private List<Event> _originalEvents = new List<Event>();

		[JsonIgnore]
		private List<Event> _fillerEvents = new List<Event>();

		[JsonIgnore]
		private AsyncLock _eventLock = new AsyncLock();

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

		public void UpdateFillers(List<Event> fillers)
		{
			lock (_fillerEvents)
			{
				_fillerEvents.Clear();
				if (fillers != null)
				{
					_fillerEvents.AddRange(fillers);
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
