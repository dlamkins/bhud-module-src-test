using System.ComponentModel;

namespace Settings.Enums
{
	public enum DungeonLabel
	{
		[Description("Abbreviate the dungeon names")]
		Abbreviation,
		[Description("Hide the dungeon names")]
		NoLabel
	}
}
