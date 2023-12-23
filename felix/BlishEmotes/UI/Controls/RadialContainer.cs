using Microsoft.Xna.Framework.Graphics;

namespace felix.BlishEmotes.UI.Controls
{
	internal class RadialContainer<T> where T : RadialBase
	{
		public double StartAngle { get; set; }

		public double EndAngle { get; set; }

		public T Value { get; set; }

		public Texture2D Texture { get; set; }

		public string Text { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public bool Selected { get; set; }

		public bool Locked { get; set; }
	}
}
