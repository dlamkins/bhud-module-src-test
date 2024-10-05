using System;

namespace Kenedia.Modules.Core.DataModels
{
	[Flags]
	public enum AttunementType
	{
		None = 0x0,
		Fire = 0x1,
		Water = 0x2,
		Air = 0x4,
		Earth = 0x8
	}
}
