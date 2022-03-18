using System;
using System.Drawing;

namespace Nekres.Screenshot_Manager
{
	internal static class SizeExtensions
	{
		public static Size Fit(this Size source, Size size)
		{
			if (source.Equals(size))
			{
				return source;
			}
			float scale = Math.Min((float)size.Width / (float)source.Width, (float)size.Height / (float)source.Height);
			return new Size(Convert.ToInt32((float)source.Width * scale), Convert.ToInt32((float)source.Height * scale));
		}
	}
}
