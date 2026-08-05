using Microsoft.Xna.Framework;

namespace Frtal.Wayfinder.Util
{
	public static class MapProjection
	{
		public static Vector2 ContinentToScreen(Vector2 continent, Vector2 mapCenter, double mapScale, int screenWidth, int screenHeight)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (mapScale <= 0.0)
			{
				mapScale = 1.0;
			}
			float num = (float)((double)(continent.X - mapCenter.X) / mapScale) + (float)screenWidth * 0.5f;
			float sy = (float)((double)(continent.Y - mapCenter.Y) / mapScale) + (float)screenHeight * 0.5f;
			return new Vector2(num, sy);
		}
	}
}
