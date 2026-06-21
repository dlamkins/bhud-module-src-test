using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class MarkerPlacementHelper
	{
		private struct POINT
		{
			public int X;

			public int Y;
		}

		private struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		[DllImport("user32.dll")]
		private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

		[DllImport("user32.dll")]
		private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll")]
		private static extern bool SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		private static extern bool GetCursorPos(out POINT lpPoint);

		public static bool TryGetGameClientOrigin(out int left, out int top)
		{
			left = 0;
			top = 0;
			try
			{
				Process[] processes = Process.GetProcessesByName("Gw2-64");
				if (processes.Length == 0)
				{
					processes = Process.GetProcessesByName("Gw2");
				}
				Process[] array = processes;
				foreach (Process process in array)
				{
					try
					{
						IntPtr hwnd = process.MainWindowHandle;
						if (!(hwnd == IntPtr.Zero) && GetClientRect(hwnd, out var rect))
						{
							POINT pOINT = default(POINT);
							pOINT.X = rect.Left;
							pOINT.Y = rect.Top;
							POINT pt = pOINT;
							if (ClientToScreen(hwnd, ref pt))
							{
								left = pt.X;
								top = pt.Y;
								return true;
							}
						}
					}
					finally
					{
						process.Dispose();
					}
				}
			}
			catch
			{
			}
			return false;
		}

		public static Point BlishToPlacementPosition(Vector2 blishCoord, float uiScaleMultiplier)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			float x = blishCoord.X * uiScaleMultiplier;
			float y = blishCoord.Y * uiScaleMultiplier;
			if (TryGetGameClientOrigin(out var gameLeft, out var gameTop))
			{
				return new Point(gameLeft + (int)x, gameTop + (int)y);
			}
			return new Point((int)x, (int)y);
		}

		public static void SetPlacementMousePosition(Point placementPosition, bool useScreenCoordinates)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (useScreenCoordinates)
			{
				SetCursorPos(placementPosition.X, placementPosition.Y);
			}
			else
			{
				Mouse.SetPosition(placementPosition.X, placementPosition.Y);
			}
		}

		public static Point GetPlacementCursorPosition(bool useScreenCoordinates)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if (useScreenCoordinates && GetCursorPos(out var pt))
			{
				return new Point(pt.X, pt.Y);
			}
			MouseState state = Mouse.GetState();
			return new Point(((MouseState)(ref state)).get_X(), ((MouseState)(ref state)).get_Y());
		}

		public static bool UseScreenCoordinatesForPlacement()
		{
			int left;
			int top;
			return TryGetGameClientOrigin(out left, out top);
		}
	}
}
