using System.Linq;
using Kenedia.Modules.Core.Controls;

namespace Kenedia.Modules.QoL.Controls
{
	public class ModuleHotbar : Hotbar
	{
		protected override void SortButtons()
		{
			switch (base.SortType)
			{
			case SortType.ActivesFirst:
				ItemsPanel.SortChildren((ModuleButton a, ModuleButton b) => b.Checked.CompareTo(a.Checked));
				break;
			case SortType.ByModuleName:
				ItemsPanel.SortChildren((ModuleButton a, ModuleButton b) => a.Module.Name.CompareTo(b.Module.Name));
				break;
			}
		}

		public override void SetButtonsExpanded()
		{
			foreach (ModuleButton c in ItemsPanel.Children.OfType<ModuleButton>())
			{
				c.Visible = c.Module.ShowInHotbar.Value && (base.ExpandBar || c.Checked);
			}
		}
	}
}
