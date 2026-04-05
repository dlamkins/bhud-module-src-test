using System.Collections.Generic;
using Newtonsoft.Json;

namespace SongbookOfTyria.Models
{
	public class PracticeSections
	{
		[JsonProperty("bars_per_row")]
		public int BarsPerRow { get; set; }

		[JsonProperty("sections")]
		public List<PracticeSection> Sections { get; set; }
	}
}
