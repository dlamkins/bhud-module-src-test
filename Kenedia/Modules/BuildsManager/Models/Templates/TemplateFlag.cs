using System;

namespace Kenedia.Modules.BuildsManager.Models.Templates
{
	[Flags]
	public enum TemplateFlag
	{
		None = 0x0,
		Favorite = 0x1,
		Pve = 0x2,
		Pvp = 0x4,
		Wvw = 0x8,
		OpenWorld = 0x10,
		Dungeons = 0x20,
		Fractals = 0x40,
		Raids = 0x80,
		Power = 0x100,
		Condition = 0x200,
		Tank = 0x400,
		Support = 0x800,
		Heal = 0x1000,
		Quickness = 0x2000,
		Alacrity = 0x4000,
		WorldCompletion = 0x8000,
		Leveling = 0x10000,
		Farming = 0x20000
	}
}
