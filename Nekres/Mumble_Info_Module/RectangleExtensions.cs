using System;
using Microsoft.Xna.Framework;

namespace Nekres.Mumble_Info_Module
{
	internal static class RectangleExtensions
	{
		public static Rectangle Union(this Rectangle value1, Rectangle value2)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			int x = Math.Min(value1.X, value2.X);
			int y = Math.Min(value1.Y, value2.Y);
			return new Rectangle(x, y, Math.Max(((Rectangle)(ref value1)).get_Right(), ((Rectangle)(ref value2)).get_Right()) - x, Math.Max(((Rectangle)(ref value1)).get_Bottom(), ((Rectangle)(ref value2)).get_Bottom()) - y);
		}

		public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			result.X = Math.Min(value1.X, value2.X);
			result.Y = Math.Min(value1.Y, value2.Y);
			result.Width = Math.Max(((Rectangle)(ref value1)).get_Right(), ((Rectangle)(ref value2)).get_Right()) - result.X;
			result.Height = Math.Max(((Rectangle)(ref value1)).get_Bottom(), ((Rectangle)(ref value2)).get_Bottom()) - result.Y;
		}
	}
}
