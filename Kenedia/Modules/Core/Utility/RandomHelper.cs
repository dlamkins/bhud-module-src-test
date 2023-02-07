using System;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Utility
{
	public static class RandomHelper
	{
		public static readonly Random Rnd = new Random();

		public static Color RandomColor(Range? r = null, Range? g = null, Range? b = null, Range? a = null)
		{
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			Range red = ((!r.HasValue) ? new Range(0, 255) : r.Value);
			Range green = ((!g.HasValue) ? new Range(0, 255) : g.Value);
			Range blue = ((!b.HasValue) ? new Range(0, 255) : b.Value);
			Range alpha = ((!a.HasValue) ? new Range(255, 255) : a.Value);
			return new Color(Rnd.Next(red.Start, red.End), Rnd.Next(green.Start, green.End), Rnd.Next(blue.Start, blue.End), Rnd.Next(alpha.Start, alpha.End));
		}
	}
}
