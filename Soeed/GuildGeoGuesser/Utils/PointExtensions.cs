using Microsoft.Xna.Framework;

namespace Soeed.GuildGeoGuesser.Utils
{
	public static class PointExtensions
	{
		public static Point Half(this Point point)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return new Point(point.X / 2, point.Y / 2);
		}

		public static Point Scale(this Point point, float scale)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return new Point((int)((float)point.X * scale), (int)((float)point.Y * scale));
		}

		public static Vector2 ToVector2(this Point point)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)point.X, (float)point.Y);
		}
	}
}
