using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Manlaan.HPGrid.Models
{
	public class Grid
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("map")]
		public int Map { get; set; }

		[JsonPropertyName("fights")]
		public List<GridFight> Fights { get; set; }
	}
}
