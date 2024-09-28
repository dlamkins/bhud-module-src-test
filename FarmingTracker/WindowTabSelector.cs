namespace FarmingTracker
{
	public class WindowTabSelector
	{
		private FarmingTrackerWindow? _farmingTrackerWindow;

		public void Init(FarmingTrackerWindow farmingTrackerWindow)
		{
			_farmingTrackerWindow = farmingTrackerWindow;
		}

		public void SelectWindowTab(WindowTab windowTab, WindowVisibility windowVisibility)
		{
			if (_farmingTrackerWindow == null)
			{
				Module.Logger.Error("Cannot select tab because window field is not set yet.");
			}
			else
			{
				_farmingTrackerWindow!.SelectWindowTab(windowTab, windowVisibility);
			}
		}
	}
}
