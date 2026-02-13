using Blish_HUD.Content;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class WorldLocationPresetData
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("waypoint")]
		public string Waypoint { get; set; }

		[JsonProperty("picture")]
		public string Picture { get; set; }

		[JsonProperty("avatar")]
		public string Avatar { get; set; }

		[JsonProperty("position")]
		public WorldPosition3D Position { get; set; }

		[JsonProperty("screenWidth")]
		public float ScreenWidth { get; set; } = 10f;


		[JsonIgnore]
		public AsyncTexture2D AvatarTexture { get; set; }

		[JsonIgnore]
		public AsyncTexture2D PictureTexture { get; set; }
	}
}
