using System.ComponentModel;

namespace RaidClears.Settings.Enums
{
	public enum Layout
	{
		[Description("Vertical")]
		Vertical,
		[Description("Horizontal")]
		Horizontal,
		[Description("A single row")]
		SingleRow,
		[Description("A single column")]
		SingleColumn
	}
}
