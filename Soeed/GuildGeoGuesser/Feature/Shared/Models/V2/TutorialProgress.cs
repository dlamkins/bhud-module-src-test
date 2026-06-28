using System;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class TutorialProgress
	{
		[JsonProperty("publicUnlocked")]
		public bool PublicUnlocked { get; set; }

		[JsonProperty("optedOut")]
		public bool OptedOut { get; set; }
	}
}
