using System.ComponentModel;

namespace Estreya.BlishHUD.Shared.Models
{
	public enum CornerIconLeftClickAction
	{
		None,
		[Description("Toggle Settingswindow")]
		Settings,
		[Description("Toggle Visibility")]
		Visibility
	}
}
