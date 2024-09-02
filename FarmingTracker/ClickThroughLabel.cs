using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class ClickThroughLabel : Label
	{
		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public ClickThroughLabel()
			: this()
		{
		}
	}
}
