using System.Drawing;
using Microsoft.Xna.Framework;

namespace Nekres.Regions_Of_Tyria
{
	public static class ColorExtensions
	{
		public static Microsoft.Xna.Framework.Color ToXnaColor(this System.Drawing.Color color)
		{
			return new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
		}
	}
}
