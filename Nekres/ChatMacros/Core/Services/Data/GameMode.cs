using System;

namespace Nekres.ChatMacros.Core.Services.Data
{
	[Flags]
	public enum GameMode
	{
		None = 0x0,
		PvE = 0x1,
		WvW = 0x2,
		PvP = 0x4
	}
}
