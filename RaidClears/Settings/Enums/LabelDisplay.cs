using System.ComponentModel;

namespace RaidClears.Settings.Enums
{
	public enum LabelDisplay
	{
		[Description("Show numbers")]
		WingNumber,
		[Description("Abbreviate the names")]
		Abbreviation,
		[Description("Hide the names")]
		NoLabel
	}
}
