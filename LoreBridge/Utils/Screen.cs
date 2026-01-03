using System.Drawing;
using Microsoft.Xna.Framework;

namespace LoreBridge.Utils
{
	internal static class Screen
	{
		public static Bitmap GetScreen(Point pos, Point size)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return GetScreen(new Rectangle(pos.X, pos.Y, size.X, size.Y));
		}

		public static Bitmap GetScreen(Point pos, Size size)
		{
			return GetScreen(new Rectangle(pos.X, pos.Y, size.Width, size.Height));
		}

		public static Bitmap GetScreen(Rectangle rectangle)
		{
			Bitmap screen = new Bitmap(rectangle.Width, rectangle.Height);
			Graphics.FromImage(screen).CopyFromScreen(rectangle.X, rectangle.Y, Point.Empty.X, Point.Empty.Y, screen.Size);
			return screen;
		}
	}
}
