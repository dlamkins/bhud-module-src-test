using System;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Helpers
{
	public static class MathHelper
	{
		public static double CalculeAngle(Point start, Point arrival)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return (float)Math.Atan2(arrival.Y - start.Y, arrival.X - start.X);
		}

		public static double CalculeDistance(Point start, Point arrival)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			double deltaX = Math.Pow(arrival.X - start.X, 2.0);
			return Math.Sqrt(Math.Pow(arrival.Y - start.Y, 2.0) + deltaX);
		}
	}
}
