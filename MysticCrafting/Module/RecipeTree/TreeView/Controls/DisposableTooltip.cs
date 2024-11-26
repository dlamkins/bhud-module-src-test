using System.Reflection;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
{
	public class DisposableTooltip : Tooltip
	{
		public DisposableTooltip(ITooltipView tooltipView)
			: this(tooltipView)
		{
		}

		private void RemoveFromCollection()
		{
			FieldInfo field = typeof(Tooltip).GetField("_allTooltips", BindingFlags.Static | BindingFlags.NonPublic);
			if (!(field == null))
			{
				((ControlCollection<Tooltip>)field.GetValue(null))?.Remove((Tooltip)(object)this);
			}
		}

		protected override void DisposeControl()
		{
			((Tooltip)this).DisposeControl();
			RemoveFromCollection();
		}
	}
}
