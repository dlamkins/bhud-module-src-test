using System.ComponentModel;

namespace Estreya.BlishHUD.EventTable.Models
{
	public enum DrawInterval
	{
		[Description("0ms")]
		INSTANT = 0,
		[Description("100ms")]
		FASTEST = 100,
		[Description("250ms")]
		FAST = 250,
		[Description("500ms")]
		NORMAL = 500,
		[Description("750ms")]
		SLOW = 500,
		[Description("1000ms")]
		SLOWEST = 1000
	}
}
