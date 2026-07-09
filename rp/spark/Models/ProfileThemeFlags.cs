using System;

namespace rp.spark.Models
{
	[Flags]
	public enum ProfileThemeFlags
	{
		None = 0x0,
		Comedy = 0x1,
		Combat = 0x2,
		Romance = 0x4,
		SliceOfLife = 0x8,
		All = 0xF
	}
}
