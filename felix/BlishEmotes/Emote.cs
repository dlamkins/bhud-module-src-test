using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace felix.BlishEmotes
{
	public class Emote
	{
		[JsonProperty("id", Required = Required.Always)]
		public string Id { get; set; }

		[JsonProperty("command", Required = Required.Always)]
		public string Command { get; set; }

		[JsonIgnore]
		public bool Locked { get; set; }

		[JsonIgnore]
		public Texture2D Texture { get; set; }

		[JsonIgnore]
		public string TextureRef => Id + ".png";
	}
}
