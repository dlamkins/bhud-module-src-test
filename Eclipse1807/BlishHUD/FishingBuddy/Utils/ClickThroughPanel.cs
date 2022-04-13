using Blish_HUD.Controls;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class ClickThroughPanel : Panel
	{
		public bool Capture { get; set; }

		public ClickThroughPanel(bool captureInput = false)
			: this()
		{
			Capture = captureInput;
		}

		protected override CaptureType CapturesInput()
		{
			if (Capture)
			{
				return (CaptureType)4;
			}
			return (CaptureType)0;
		}
	}
}
