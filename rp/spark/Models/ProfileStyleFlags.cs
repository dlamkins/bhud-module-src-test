using System;

namespace rp.spark.Models
{
	[Flags]
	public enum ProfileStyleFlags
	{
		None = 0x0,
		WalkUpFriendly = 0x1,
		WhisperFirst = 0x2,
		OpenToNewContacts = 0x4,
		LoreFriendly = 0x8,
		FlexibleLore = 0x10,
		All = 0x1F
	}
}
