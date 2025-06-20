using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class HelpScreen
	{
		[JsonProperty("version")]
		public string Version { get; set; } = "0.0.0";


		[JsonProperty("schema_version")]
		public int SchemaVersion { get; set; }

		[JsonProperty("help_screens")]
		public List<HelpScreenPanel> Screens { get; set; } = new List<HelpScreenPanel>();

	}
}
