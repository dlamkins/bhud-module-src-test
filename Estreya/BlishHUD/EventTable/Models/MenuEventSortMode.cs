using System.ComponentModel;

namespace Estreya.BlishHUD.EventTable.Models
{
	public enum MenuEventSortMode
	{
		[Description("Default")]
		Default,
		[Description("Alphabetical (A-Z)")]
		Alphabetical,
		[Description("Alphabetical (Z-A)")]
		AlphabeticalDesc
	}
}
