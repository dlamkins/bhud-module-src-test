using System;
using Microsoft.Xna.Framework;

namespace Blish_HUD.Extended
{
	public static class XnaExtensions
	{
		public static Point ScaleTo(this Point size, Point target, bool keepAspect = true, bool enlarge = false)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			if (size.X <= 0 || size.Y <= 0 || target.X <= 0 || target.Y <= 0)
			{
				return Point.get_Zero();
			}
			if (keepAspect)
			{
				float scale = Math.Min((float)target.X / (float)size.X, (float)target.Y / (float)size.Y);
				if (!enlarge)
				{
					scale = Math.Min(1f, scale);
				}
				int num = Math.Min((int)Math.Round((float)size.X * scale), target.X);
				int height2 = Math.Min((int)Math.Round((float)size.Y * scale), target.Y);
				return new Point(num, height2);
			}
			int num2 = (enlarge ? target.X : Math.Min(size.X, target.X));
			int height = (enlarge ? target.Y : Math.Min(size.Y, target.Y));
			return new Point(num2, height);
		}

		public static Rectangle CenterWithin(this Point size, Rectangle target)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (size.X <= 0 || size.Y <= 0)
			{
				return Rectangle.get_Empty();
			}
			int num = target.X + (target.Width - size.X) / 2;
			int y = target.Y + (target.Height - size.Y) / 2;
			return new Rectangle(num, y, size.X, size.Y);
		}

		public static Rectangle GetCenteredFit(this Rectangle bounds, Point size, bool keepAspect = true, bool enlarge = false)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return size.ScaleTo(((Rectangle)(ref bounds)).get_Size(), keepAspect, enlarge).CenterWithin(bounds);
		}
	}
}
