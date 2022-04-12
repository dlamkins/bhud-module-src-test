using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Kenedia.Modules.Characters
{
	public class FilterWindow : BasicContainer
	{
		public DateTime lastInput;

		public int Alpha = 255;

		public HeadedFlowRegion Utility;

		public HeadedFlowRegion Crafting;

		public HeadedFlowRegion Profession;

		public HeadedFlowRegion Specialization;

		public HeadedFlowRegion CustomTags;

		public StandardButton toggleSpecsButton;

		public ToggleIcon visibleToggle;

		public ToggleIcon birthdayToggle;

		public FilterWindow()
		{
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
			});
			((Control)this).add_MouseMoved((EventHandler<MouseEventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
				Module.Logger.Debug("Mouse Moved!");
			});
			((Control)this).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
			});
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
			});
			((Control)this).add_Shown((EventHandler<EventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
			});
			((Control)this).add_Hidden((EventHandler<EventArgs>)delegate
			{
				((Control)this).set_Opacity(1f);
			});
		}
	}
}
