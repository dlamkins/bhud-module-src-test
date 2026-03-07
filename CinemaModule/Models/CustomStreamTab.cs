using System;
using CinemaModule.Models.Location;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class CustomStreamTab
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		[JsonConstructor]
		public CustomStreamTab()
		{
		}

		public CustomStreamTab(string name)
		{
			Id = IdGenerator.Generate();
			Name = name;
			CreatedAt = DateTime.UtcNow;
		}
	}
}
