using System;
using Microsoft.Xna.Framework;

namespace Nekres.Screenshot_Manager
{
	internal static class RectangleExtensions
	{
		public static Rectangle Fit(this Rectangle source, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			if (((Rectangle)(ref source)).Equals(bounds))
			{
				return source;
			}
			float scale = Math.Min((float)bounds.Width / (float)source.Width, (float)bounds.Height / (float)source.Height);
			int newWidth = Convert.ToInt32((float)source.Width * scale);
			int newHeight = Convert.ToInt32((float)source.Height * scale);
			return new Rectangle((bounds.Width - newWidth) / 2, (bounds.Height - newHeight) / 2, newWidth, newHeight);
		}
	}
}
