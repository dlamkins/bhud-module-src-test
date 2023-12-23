using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace felix.BlishEmotes
{
	public abstract class RadialBase
	{
		[JsonIgnore]
		public Texture2D Texture { get; set; }

		[JsonIgnore]
		public bool Locked { get; set; }

		[JsonIgnore]
		public abstract string Label { get; }
	}
}
