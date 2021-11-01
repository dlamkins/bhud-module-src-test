using System.Drawing;
using Microsoft.Xna.Framework;

namespace Nekres.Regions_Of_Tyria
{
	public static class ColorExtensions
	{
		public static Color ToXnaColor(this Color color)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			return new Color(color.R, color.G, color.B, color.A);
		}
	}
}
