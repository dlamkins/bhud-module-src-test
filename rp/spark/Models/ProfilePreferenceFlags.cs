using System;

namespace rp.spark.Models
{
	[Flags]
	public enum ProfilePreferenceFlags
	{
		None = 0x0,
		Casual = 0x1,
		OneShot = 0x2,
		LongTerm = 0x4,
		EventRoleplay = 0x8,
		SmallGroup = 0x10,
		LargeGroup = 0x20,
		All = 0x3F
	}
}
