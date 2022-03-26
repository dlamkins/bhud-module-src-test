using System.Text.Json.Serialization;

namespace Manlaan.HPGrid.Models
{
	public class GridPhase
	{
		[JsonPropertyName("percent")]
		public int Percent { get; set; }

		[JsonPropertyName("color")]
		public string Color { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }
	}
}
