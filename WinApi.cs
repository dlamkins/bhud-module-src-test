using System;
using System.Drawing;
using System.Runtime.InteropServices;

internal static class WinApi
{
	[DllImport("user32.dll")]
	private static extern bool GetCursorInfo(ref CursorInfo pci);

	[DllImport("user32.dll")]
	private static extern bool GetCursorClip(ref RECT rec);

	[DllImport("user32.dll")]
	private static extern bool ClipCursor(ref RECT rec);

	[DllImport("user32.dll")]
	private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

	[DllImport("user32.dll")]
	private static extern bool GetClientRect(IntPtr hWnd, ref RECT lpRect);

	[DllImport("user32.dll")]
	private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

	[DllImport("user32.dll")]
	private static extern bool GetCursorPos(IntPtr hWnd, ref POINT lpPoint);

	internal static CursorInfo GetCursorInfo()
	{
		CursorInfo cursorInfo = default(CursorInfo);
		cursorInfo.CbSize = Marshal.SizeOf(typeof(CursorInfo));
		CursorInfo pci = cursorInfo;
		GetCursorInfo(ref pci);
		return pci;
	}

	internal static Rectangle? GetCursorClip()
	{
		RECT rect = default(RECT);
		if (!GetCursorClip(ref rect))
		{
			return null;
		}
		return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
	}

	internal static Rectangle? GetWindowRect(IntPtr hWnd)
	{
		RECT rect = default(RECT);
		if (!GetWindowRect(hWnd, ref rect))
		{
			return null;
		}
		return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
	}

	internal static Rectangle? GetClientRect(IntPtr hWnd)
	{
		RECT rect = default(RECT);
		if (!GetClientRect(hWnd, ref rect))
		{
			return null;
		}
		return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
	}

	internal static Point? ClientToScreen(IntPtr hWnd)
	{
		POINT point = default(POINT);
		if (!ClientToScreen(hWnd, ref point))
		{
			return null;
		}
		return new Point(point.x, point.y);
	}

	internal static Point? GetCursorPos(IntPtr hWnd)
	{
		POINT point = default(POINT);
		if (!GetCursorPos(hWnd, ref point))
		{
			return null;
		}
		return new Point(point.x, point.y);
	}
}
