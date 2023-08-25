using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace felix.BlishEmotes
{
	public class Emote
	{
		[JsonProperty("id", Required = Required.Always)]
		public string Id { get; set; }

		[JsonProperty("command", Required = Required.Always)]
		public string Command { get; set; }

		[JsonProperty("locked", DefaultValueHandling = DefaultValueHandling.Populate)]
		public bool Locked { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty("category", Required = Required.Always)]
		public Category Category { get; set; }

		[JsonIgnore]
		public Texture2D Texture { get; set; }

		[JsonIgnore]
		public string TextureRef => Id + ".png";
	}
}
