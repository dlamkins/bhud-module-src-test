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
		public Version Version { get; set; } = new Version(0, 0, 0, (string)null, (string)null);


		[JsonProperty("moduleVersion")]
		[JsonConverter(typeof(SemanticRangeConverter))]
		public Range MinimumModuleVersion { get; set; } = new Range(">=0.0.0", false);


		[JsonProperty("eventCategories")]
		public List<EventCategory> EventCategories { get; set; }
	}
}
