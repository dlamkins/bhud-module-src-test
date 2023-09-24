using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Manlaan.CommanderMarkers.Library.Models
{
	public class CommunitySets
	{
		[JsonProperty("lastEdit")]
		public DateTime LastEdit { get; set; } = DateTime.UtcNow;


		[JsonProperty("categories")]
		public List<CommunityCategory> Categories { get; set; } = new List<CommunityCategory>();

	}
}
