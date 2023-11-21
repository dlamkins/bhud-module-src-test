using Estreya.BlishHUD.Shared.Attributes;

namespace Estreya.BlishHUD.Shared.Models
{
	public enum CornerIconClickAction
	{
		[Translation("cornerIconClickAction-none", "None")]
		None,
		[Translation("cornerIconClickAction-settings", "Toggle Settingswindow")]
		Settings,
		[Translation("cornerIconClickAction-visibility", "Toggle Visibility")]
		Visibility
	}
}
