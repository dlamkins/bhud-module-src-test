using System;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ColorExtension
	{
		public static string ToHex(this Color col)
		{
			Color.FromArgb(((Color)(ref col)).get_A(), ((Color)(ref col)).get_R(), ((Color)(ref col)).get_G(), ((Color)(ref col)).get_B());
			return $"#{((Color)(ref col)).get_A():X2}{((Color)(ref col)).get_R():X2}{((Color)(ref col)).get_G():X2}{((Color)(ref col)).get_B():X2}";
		}

		public static bool ColorFromHex(this string col, out Color outColor)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Color c = ColorTranslator.FromHtml(col);
				outColor = new Color(c.R, c.G, c.B, c.A);
				return true;
			}
			catch (Exception)
			{
			}
			outColor = Color.get_Transparent();
			return false;
		}
	}
}
