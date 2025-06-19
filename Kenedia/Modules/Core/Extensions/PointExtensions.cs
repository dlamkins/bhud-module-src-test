using System;
using Blish_HUD;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class PointExtensions
	{
		public static int Distance2D(this Point p1, Point p2)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			return (int)Math.Sqrt(Math.Pow(p2.X - p1.X, 2.0) + Math.Pow(p2.Y - p1.Y, 2.0));
		}

		public static float Distance3D(this Vector3 p1, Vector3 p2)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			float num = p2.X - p1.X;
			float deltaY = p2.Y - p1.Y;
			float deltaZ = p2.Z - p1.Z;
			return (float)Math.Sqrt(num * num + deltaY * deltaY + deltaZ * deltaZ);
		}

		public static Point Add(this Point b, Point p)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return new Point(b.X + p.X, b.Y + p.Y);
		}

		public static Point Substract(this Point b, Point p)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return new Point(b.X - p.X, b.Y - p.Y);
		}

		public static Point Scale(this Point p, double factor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return new Point((int)((double)p.X * factor), (int)((double)p.Y * factor));
		}

		public static string ConvertToString(this Point p)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return $"X: {p.X}, Y: {p.Y}";
		}

		public static Point ClientToScreenPos(this Point p, bool scaleToUi = false)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (scaleToUi)
			{
				p = p.ScaleToUi();
			}
			IntPtr gw2WindowHandle = GameService.GameIntegration.Gw2Instance.Gw2WindowHandle;
			User32Dll.POINT point = new User32Dll.POINT
			{
				X = p.X,
				Y = p.Y
			};
			User32Dll.ClientToScreen(gw2WindowHandle, ref point);
			return new Point(point.X, point.Y);
		}
	}
}
