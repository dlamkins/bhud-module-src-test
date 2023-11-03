using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts.Controls
{
	internal class RadialThing
	{
		public double AngleBegin { get; set; }

		public double AngleEnd { get; set; }

		public Thing Thing { get; set; }

		public Texture2D Texture { get; set; }

		public int ImageX { get; set; }

		public int ImageY { get; set; }

		public bool Selected { get; set; }

		public bool Default { get; internal set; }
	}
}
