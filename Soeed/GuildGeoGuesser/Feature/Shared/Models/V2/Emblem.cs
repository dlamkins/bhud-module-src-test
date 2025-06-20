using System.Collections.Generic;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class Emblem
	{
		[JsonProperty("background_id")]
		public int BackgroundId { get; set; } = 1;


		[JsonProperty("foreground_id")]
		public int ForegroundId { get; set; } = 1;


		[JsonProperty("flags")]
		public List<string> Flags { get; set; } = new List<string>();


		[JsonProperty("background_color_id")]
		public int BackgroundColorId { get; set; } = 1;


		[JsonProperty("foreground_primary_color_id")]
		public int ForegroundPrimaryColorId { get; set; } = 1;


		[JsonProperty("foreground_secondary_color_id")]
		public int ForegroundSecondaryColorId { get; set; } = 1;

	}
}
