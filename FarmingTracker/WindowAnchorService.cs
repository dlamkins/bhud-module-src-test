using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class WindowAnchorService
	{
		public static Point ConvertBetweenControlAndWindowAnchorLocation(Point location, Point controlSize, ConvertLocation convertLocation, WindowAnchor windowAnchor)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			bool xIsControlLocation = windowAnchor == WindowAnchor.TopLeft || windowAnchor == WindowAnchor.BottomLeft;
			bool yIsControlLocation = windowAnchor == WindowAnchor.TopLeft || windowAnchor == WindowAnchor.TopRight;
			int num = ConvertCoordinate(location.X, controlSize.X, xIsControlLocation, convertLocation);
			int y = ConvertCoordinate(location.Y, controlSize.Y, yIsControlLocation, convertLocation);
			return new Point(num, y);
		}

		private static int ConvertCoordinate(int coordinate, int controlWidthOrHeight, bool isControlLocation, ConvertLocation convertLocation)
		{
			if (isControlLocation)
			{
				return coordinate;
			}
			if (convertLocation != ConvertLocation.ToWindowAnchorLocation)
			{
				return coordinate - controlWidthOrHeight;
			}
			return coordinate + controlWidthOrHeight;
		}
	}
}
