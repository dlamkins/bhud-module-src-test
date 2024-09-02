using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class ConvertCoordinatesService
	{
		public static Point ConvertRelativeToAbsoluteCoordinates(FloatPoint relative, Point screenSize)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return ConvertRelativeToAbsoluteCoordinates(relative.X, relative.Y, screenSize.X, screenSize.Y);
		}

		public static Point ConvertRelativeToAbsoluteCoordinates(float xRelative, float yRelative, int screenWidth, int screenHeight)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			int num = (int)((float)screenWidth * xRelative);
			int yAbsolute = (int)((float)screenHeight * yRelative);
			return new Point(num, yAbsolute);
		}

		public static FloatPoint ConvertAbsoluteToRelativeCoordinates(Point absolute, Point screenSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			(float, float) relative = ConvertAbsoluteToRelativeCoordinates(absolute.X, absolute.Y, screenSize.X, screenSize.Y);
			return new FloatPoint(relative.Item1, relative.Item2);
		}

		public static (float xRelative, float yRelative) ConvertAbsoluteToRelativeCoordinates(int xAbsolute, int yAbsolute, int screenWidth, int screenHeight)
		{
			float item = ConvertAbsoluteToRelativeCoordinate(xAbsolute, screenWidth);
			float yRelative = ConvertAbsoluteToRelativeCoordinate(yAbsolute, screenHeight);
			return (item, yRelative);
		}

		private static float ConvertAbsoluteToRelativeCoordinate(int absolute, int screenWidthOrHeight)
		{
			if (screenWidthOrHeight <= 0)
			{
				return 0f;
			}
			return (float)absolute / (float)screenWidthOrHeight;
		}
	}
}
