using System;
using Blish_HUD.Controls;

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
			base.Click += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
			};
			base.MouseMoved += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
				Module.Logger.Debug("Mouse Moved!");
			};
			base.MouseEntered += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
			};
			base.MouseLeft += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
			};
			base.Shown += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
			};
			base.Hidden += delegate
			{
				base.Opacity = 1f;
			};
		}
	}
}
