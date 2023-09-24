using System.Collections.Generic;
using Newtonsoft.Json;

namespace Manlaan.CommanderMarkers.Library.Models
{
	public class CommunityCategory
	{
		[JsonProperty("categoryName")]
		public string CategoryName { get; set; } = "Community Created";


		[JsonProperty("markerSets")]
		public List<CommunityMarkerSet> MarkerSets { get; set; } = new List<CommunityMarkerSet>();

	}
}
