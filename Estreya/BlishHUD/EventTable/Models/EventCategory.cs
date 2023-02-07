using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Attributes;
using Estreya.BlishHUD.Shared.State;
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

		public async Task LoadAsync(TranslationState translationState = null)
		{
			if (translationState != null)
			{
				Name = translationState.GetTranslation("eventCategory-" + Key + "-name", Name);
			}
			using (await _eventLock.LockAsync())
			{
				await Task.WhenAll(Events.Select((Event ev) => ev.LoadAsync(this, translationState)));
			}
		}
	}
}
