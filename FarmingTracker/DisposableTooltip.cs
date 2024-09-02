using System;
using System.Reflection;
using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class DisposableTooltip : Tooltip
	{
		protected override void DisposeControl()
		{
			RemoveFromStaticTooltips();
			((Tooltip)this).DisposeControl();
		}

		private void RemoveFromStaticTooltips()
		{
			try
			{
				FieldInfo allTooltipsField = typeof(Tooltip).GetField("_allTooltips", BindingFlags.Static | BindingFlags.NonPublic);
				if (!(allTooltipsField == null))
				{
					((ControlCollection<Tooltip>)allTooltipsField.GetValue(null))?.Remove((Tooltip)(object)this);
				}
			}
			catch (Exception e)
			{
				Module.Logger.Error(e, "Failed to remove tooltip from static tooltips field with reflection");
			}
		}

		public DisposableTooltip()
			: this()
		{
		}
	}
}
