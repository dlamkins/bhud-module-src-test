using System.ComponentModel;

namespace Estreya.BlishHUD.EventTable.Models
{
	public enum EventCompletedAction
	{
		Crossout,
		Hide,
		[Description("Change Opacity")]
		ChangeOpacity,
		[Description("Crossout & Change Opacity")]
		CrossoutAndChangeOpacity
	}
}
