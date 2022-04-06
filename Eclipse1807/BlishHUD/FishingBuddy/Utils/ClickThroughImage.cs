using Blish_HUD.Controls;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class ClickThroughImage : Image
	{
		public bool capture { get; set; }

		public ClickThroughImage(bool captureInput = false)
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
			return (CaptureType)22;
		}
	}
}
