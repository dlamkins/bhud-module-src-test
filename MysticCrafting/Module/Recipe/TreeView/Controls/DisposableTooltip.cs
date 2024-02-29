using System.Reflection;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;

namespace MysticCrafting.Module.Recipe.TreeView.Controls
{
	public class DisposableTooltip : Tooltip
	{
		public DisposableTooltip(ITooltipView tooltipView)
			: base(tooltipView)
		{
		}

		private void RemoveFromCollection()
		{
			FieldInfo field = typeof(Tooltip).GetField("_allTooltips", BindingFlags.Static | BindingFlags.NonPublic);
			if (!(field == null))
			{
				((ControlCollection<Tooltip>)field.GetValue(null))?.Remove(this);
			}
		}

		protected override void DisposeControl()
		{
			RemoveFromCollection();
			base.DisposeControl();
		}
	}
}
