using System;
using Newtonsoft.Json;

namespace CinemaModule.Models.Location
{
	public class SavedLocation
	{
		private const float DefaultScreenWidth = 10f;

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("position")]
		public WorldPosition3D Position { get; set; }

		[JsonProperty("screenWidth")]
		public float ScreenWidth { get; set; } = 10f;


		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		[JsonConstructor]
		public SavedLocation()
		{
		}

		public SavedLocation(string name, WorldPosition3D position, float screenWidth)
		{
			Id = IdGenerator.Generate();
			CreatedAt = DateTime.UtcNow;
			Name = name;
			Position = position;
			ScreenWidth = screenWidth;
		}
	}
}
