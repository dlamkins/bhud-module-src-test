using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class Guild
	{
		[JsonProperty("GuildId")]
		public string Id { get; set; } = "";


		[JsonProperty("Name")]
		public string Name { get; set; } = "";


		[JsonProperty("Tag")]
		public string Tag { get; set; } = "";


		[JsonProperty("Emblem")]
		public Emblem Emblem { get; set; } = new Emblem();


		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; } = DateTime.Now;


		[JsonProperty("Puzzles")]
		public List<Puzzle> Puzzles { get; set; } = new List<Puzzle>();

	}
}
