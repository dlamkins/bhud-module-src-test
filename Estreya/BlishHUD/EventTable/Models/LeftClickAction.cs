using Estreya.BlishHUD.Shared.Attributes;

namespace Estreya.BlishHUD.EventTable.Models
{
	public enum LeftClickAction
	{
		[Translation("leftClickAction-none", "None")]
		None,
		[Translation("leftClickAction-copyWaypoint", "Copy Waypoint")]
		CopyWaypoint,
		[Translation("leftClickAction-navigateToWaypoint", "Navigate to Waypoint")]
		NavigateToWaypoint
	}
}
