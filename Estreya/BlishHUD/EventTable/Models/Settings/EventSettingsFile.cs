using System.Collections.Generic;
using Estreya.BlishHUD.EventTable.Json;
using Newtonsoft.Json;
using SemVer;

namespace Estreya.BlishHUD.EventTable.Models.Settings
{
	public class EventSettingsFile
	{
		[JsonProperty("version")]
		[JsonConverter(typeof(SemanticVersionConverter))]
		public Version Version { get; set; } = new Version(0, 0, 0);


		[JsonProperty("eventCategories")]
		public List<EventCategory> EventCategories { get; set; }
	}
}
