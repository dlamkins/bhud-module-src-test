using System.ComponentModel;

namespace RaidClears.Settings
{
	public enum Orientation
	{
		[Description("Wings in a vertical column")]
		Vertical,
		[Description("Wings in a horizontal row")]
		Horizontal,
		[Description("A single row")]
		SingleRow
	}
}
