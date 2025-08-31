using System;
using Microsoft.Xna.Framework;

namespace Nekres.Screenshot_Manager
{
	internal static class RectangleExtensions
	{
		public static Rectangle ScaleTo(this Rectangle source, Rectangle bounds, float scaleFactor = 1f, bool center = false)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			if (((Rectangle)(ref source)).Equals(bounds) || source.Width == 0 || source.Height == 0)
			{
				return source;
			}
			float scale = Math.Min((float)bounds.Width / (float)source.Width, (float)bounds.Height / (float)source.Height);
			scale = Math.Min(scale * scaleFactor, 1f);
			int newWidth = (int)((float)source.Width * scale);
			int newHeight = (int)((float)source.Height * scale);
			Rectangle scaledSrc = default(Rectangle);
			((Rectangle)(ref scaledSrc))._002Ector(source.X, source.Y, newWidth, newHeight);
			if (!center)
			{
				return scaledSrc;
			}
			return scaledSrc.Center(bounds);
		}

		public static Rectangle Center(this Rectangle source, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (((Rectangle)(ref source)).Equals(bounds))
			{
				return source;
			}
			int num = (bounds.Width - source.Width) / 2;
			int newY = (bounds.Height - source.Height) / 2;
			return new Rectangle(num, newY, source.Width, source.Height);
		}
	}
}
