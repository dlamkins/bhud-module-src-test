using System.ComponentModel;

namespace Estreya.BlishHUD.Shared.Models
{
	public enum CornerIconRightClickAction
	{
		None,
		[Description("Toggle Settingswindow")]
		Settings,
		[Description("Toggle Visibility")]
		Visibility
	}
}
