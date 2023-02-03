using System;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class RectangleExtension
	{
		public static int Distance2D_Center(this Rectangle p1, Rectangle p2)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			return (int)Math.Sqrt(Math.Pow(((Rectangle)(ref p2)).get_Left() - ((Rectangle)(ref p1)).get_Right(), 2.0) + Math.Pow(((Rectangle)(ref p2)).get_Top() + p2.Height / 2 - (((Rectangle)(ref p1)).get_Top() + p1.Height / 2), 2.0));
		}

		public static int Distance2D_Middle(this Rectangle p1, Rectangle p2)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			Point pp1 = default(Point);
			((Point)(ref pp1))._002Ector(((Rectangle)(ref p1)).get_Left() + p1.Width / 2, ((Rectangle)(ref p1)).get_Top() + p1.Height / 2);
			Point pp2 = default(Point);
			((Point)(ref pp2))._002Ector(((Rectangle)(ref p2)).get_Left() + p2.Width / 2, ((Rectangle)(ref p2)).get_Top() + p2.Height / 2);
			return (int)Math.Sqrt(Math.Pow(p2.X - pp2.X, 2.0) + Math.Pow(pp2.Y - pp1.Y, 2.0));
		}

		public static Rectangle Scale(this Rectangle b, double factor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle((int)((double)b.X * factor), (int)((double)b.Y * factor), (int)((double)b.Width * factor), (int)((double)b.Height * factor));
		}

		public static Rectangle Add(this Rectangle b, Point p)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(b.X + p.X, b.Y + p.Y, b.Width, b.Height);
		}
	}
}
