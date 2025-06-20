using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models
{
	[Serializable]
	public class HelpScreensModel
	{
		[JsonProperty("help_screen_version")]
		public int HelpScreenVersion { get; set; } = 1;


		[JsonProperty("help_screens")]
		public List<HelpScreenModel> HelpScreens { get; set; } = new List<HelpScreenModel>();

	}
}
