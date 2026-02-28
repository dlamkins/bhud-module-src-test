using System;
using Microsoft.Xna.Framework;

internal struct CursorInfo
{
	public int CbSize;

	public CursorFlags Flags;

	public IntPtr HCursor;

	public Point ScreenPosition;
}
