using System;
using System.Collections;
using System.Linq;
using Blish_HUD.Controls;
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
				((FlowPanel)ItemsPanel).SortChildren<ModuleButton>((Comparison<ModuleButton>)((ModuleButton a, ModuleButton b) => b.Checked.CompareTo(a.Checked)));
				break;
			case SortType.ByModuleName:
				((FlowPanel)ItemsPanel).SortChildren<ModuleButton>((Comparison<ModuleButton>)((ModuleButton a, ModuleButton b) => a.Module.Name.CompareTo(b.Module.Name)));
				break;
			}
		}

		public override void SetButtonsExpanded()
		{
			foreach (ModuleButton c in ((IEnumerable)((Container)ItemsPanel).get_Children()).OfType<ModuleButton>())
			{
				((Control)c).set_Visible(c.Module.ShowInHotbar.get_Value() && (base.ExpandBar || c.Checked));
			}
		}
	}
}
