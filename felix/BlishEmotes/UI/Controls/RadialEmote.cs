using Microsoft.Xna.Framework.Graphics;

namespace felix.BlishEmotes.UI.Controls
{
	internal class RadialEmote
	{
		public double StartAngle { get; set; }

		public double EndAngle { get; set; }

		public Emote Emote { get; set; }

		public Texture2D Texture { get; set; }

		public string Text { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public bool Selected { get; set; }
	}
}
