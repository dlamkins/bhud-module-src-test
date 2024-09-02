using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace FarmingTracker
{
	public class OpenSettingsButton : StandardButton
	{
		private readonly FarmingTrackerWindow _farmingTrackerWindow;

		public OpenSettingsButton(string buttonText, FarmingTrackerWindow farmingTrackerWindow, Container parent)
			: this()
		{
			_farmingTrackerWindow = farmingTrackerWindow;
			((StandardButton)this).set_Text(buttonText);
			((Control)this).set_BasicTooltipText("Open farming tracker settings");
			((Control)this).set_Width(300);
			((Control)this).set_Parent(parent);
			((Control)this).add_Click((EventHandler<MouseEventArgs>)OnSettingsButtonClick);
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)OnSettingsButtonClick);
			((Control)this).DisposeControl();
		}

		private void OnSettingsButtonClick(object sender, MouseEventArgs e)
		{
			_farmingTrackerWindow.ShowWindowAndSelectSettingsTab();
		}
	}
}
