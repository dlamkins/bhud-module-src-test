using Blish_HUD.Controls;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class ClickThroughPanel : Panel
	{
		public bool capture { get; set; }

		public ClickThroughPanel(bool captureInput = false)
			: this()
		{
			capture = captureInput;
		}

		protected override CaptureType CapturesInput()
		{
			if (capture)
			{
				return (CaptureType)4;
			}
			return (CaptureType)0;
		}
	}
}
