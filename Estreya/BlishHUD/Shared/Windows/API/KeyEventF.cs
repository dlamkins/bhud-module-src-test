using System;

namespace Estreya.BlishHUD.Shared.Windows.API
{
	[Flags]
	public enum KeyEventF : uint
	{
		EXTENDEDKEY = 0x1u,
		KEYUP = 0x2u,
		SCANCODE = 0x8u,
		UNICODE = 0x4u
	}
}
