using System;

namespace Kenedia.Modules.BuildsManager.Models.Templates
{
	[Flags]
	public enum SkillSlotType
	{
		None = 0x0,
		Active = 0x1,
		Inactive = 0x2,
		Terrestrial = 0x4,
		Aquatic = 0x8,
		Heal = 0x10,
		Utility_1 = 0x20,
		Utility_2 = 0x40,
		Utility_3 = 0x80,
		Elite = 0x100
	}
}
