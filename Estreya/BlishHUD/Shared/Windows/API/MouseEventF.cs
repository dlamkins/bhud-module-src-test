using System;

namespace Estreya.BlishHUD.Shared.Windows.API
{
	[Flags]
	public enum MouseEventF : uint
	{
		ABSOLUTE = 0x8000u,
		HWHEEL = 0x1000u,
		MOVE = 0x1u,
		MOVE_NOCOALESCE = 0x2000u,
		LEFTDOWN = 0x2u,
		LEFTUP = 0x4u,
		RIGHTDOWN = 0x8u,
		RIGHTUP = 0x10u,
		MIDDLEDOWN = 0x20u,
		MIDDLEUP = 0x40u,
		VIRTUALDESK = 0x4000u,
		WHEEL = 0x800u,
		XDOWN = 0x80u,
		XUP = 0x100u
	}
}
