using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace FarmingTracker
{
	public class OpenSettingsButton : StandardButton
	{
		private readonly WindowTabSelector _windowTabSelector;

		public OpenSettingsButton(string buttonText, WindowTabSelector windowTabSelector, Container parent)
			: this()
		{
			_windowTabSelector = windowTabSelector;
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
			_windowTabSelector.SelectWindowTab(WindowTab.Settings, WindowVisibility.Show);
		}
	}
}
