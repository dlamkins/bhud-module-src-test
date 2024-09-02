using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class ScreenBoundariesService
	{
		public static Point AdjustLocationToKeepControlInsideScreenBoundaries(Point windowAnchorCoordinates, Point controlSize, Point screenSize, WindowAnchor windowAnchor)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			bool xIsControlLocation = windowAnchor == WindowAnchor.TopLeft || windowAnchor == WindowAnchor.BottomLeft;
			bool yIsControlLocation = windowAnchor == WindowAnchor.TopLeft || windowAnchor == WindowAnchor.TopRight;
			int num = AdjustCoordinateToKeepControlInsideScreenBoundaries(windowAnchorCoordinates.X, controlSize.X, screenSize.X, xIsControlLocation);
			int y = AdjustCoordinateToKeepControlInsideScreenBoundaries(windowAnchorCoordinates.Y, controlSize.Y, screenSize.Y, yIsControlLocation);
			return new Point(num, y);
		}

		private static int AdjustCoordinateToKeepControlInsideScreenBoundaries(int windowAnchorCoordinate, int controlHeightOrWidth, int screenHeightOrWidth, bool isControlLocation)
		{
			int controlTopOrLeftCoordinate = (isControlLocation ? windowAnchorCoordinate : (windowAnchorCoordinate - controlHeightOrWidth));
			int controlBottomOrRight = controlTopOrLeftCoordinate + controlHeightOrWidth;
			if (controlHeightOrWidth >= screenHeightOrWidth)
			{
				if (!isControlLocation)
				{
					return screenHeightOrWidth;
				}
				return 0;
			}
			if (controlTopOrLeftCoordinate <= 0)
			{
				if (!isControlLocation)
				{
					return controlHeightOrWidth;
				}
				return 0;
			}
			if (controlBottomOrRight > screenHeightOrWidth)
			{
				if (!isControlLocation)
				{
					return screenHeightOrWidth;
				}
				return screenHeightOrWidth - controlHeightOrWidth;
			}
			return windowAnchorCoordinate;
		}
	}
}
