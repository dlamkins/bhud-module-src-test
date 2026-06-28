using System;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class TutorialInstructionStep
	{
		[JsonProperty("type")]
		public string Type { get; set; } = "text";


		[JsonProperty("text")]
		public string Text { get; set; } = "";


		public bool IsTextType => string.Equals(Type, "text", StringComparison.OrdinalIgnoreCase);

		public bool IsHintType => string.Equals(Type, "hint", StringComparison.OrdinalIgnoreCase);

		public bool IsWaypointType => string.Equals(Type, "waypoint", StringComparison.OrdinalIgnoreCase);

		public bool IsLocationType => string.Equals(Type, "location", StringComparison.OrdinalIgnoreCase);

		public bool IsScoringRingsType => string.Equals(Type, "scoring_rings", StringComparison.OrdinalIgnoreCase);
	}
}
