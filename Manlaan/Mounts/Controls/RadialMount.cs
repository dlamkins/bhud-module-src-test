using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts.Controls
{
	internal class RadialMount
	{
		public double AngleBegin { get; set; }

		public double AngleEnd { get; set; }

		public Mount Mount { get; set; }

		public Texture Texture { get; set; }

		public int ImageX { get; set; }

		public int ImageY { get; set; }

		public bool Selected { get; set; }

		public bool Default { get; internal set; }
	}
}
