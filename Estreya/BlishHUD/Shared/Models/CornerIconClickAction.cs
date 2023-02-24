using System.ComponentModel;

namespace Estreya.BlishHUD.Shared.Models
{
	public enum CornerIconClickAction
	{
		None,
		[Description("Toggle Settingswindow")]
		Settings,
		[Description("Toggle Visibility")]
		Visibility
	}
}
