using Blish_HUD.Controls;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class ClickThroughImage : Image
	{
		public bool Capture { get; set; }

		public ClickThroughImage(bool captureInput = false)
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
			return (CaptureType)22;
		}
	}
}
