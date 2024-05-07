using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Ideka.CustomCombatText
{
	public class AreasMenu : SelectablesMenu<AreaView, AreaView, AreasMenuItem>
	{
		private AreaView? _opened;

		public event Action<AreaView>? DoubleClicked;

		protected override AreaView ExtractId(AreaView item)
		{
			return item;
		}

		protected override AreasMenuItem Construct(AreasMenuItem? item, AreaView area)
		{
			AreasMenuItem item2 = item;
			if (item2 == null)
			{
				item2 = new AreasMenuItem(area, _opened);
				((Control)item2).add_Click((EventHandler<MouseEventArgs>)delegate(object _, MouseEventArgs args)
				{
					Select(item2.Area);
					if (args.get_IsDoubleClick())
					{
						this.DoubleClicked?.Invoke(item2.Area);
					}
				});
				((Control)item2).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					item2.UpdateTooltip();
				});
			}
			else
			{
				item2.UpdateVisuals(area, _opened);
			}
			return item2;
		}

		public void UpdateName(AreaView target)
		{
			if (base.Items.TryGetValue(target, out var item))
			{
				item.UpdateVisuals(target, _opened);
			}
		}

		public void SetOpened(AreaView? opened)
		{
			AreaView opened2 = opened;
			Repopulate(delegate
			{
				IEnumerable<AreaView> enumerable = ((opened2 == null) ? CTextModule.LocalData.RootAreaViews : opened2.GetChildren());
				_opened = opened2;
				if (!enumerable.Any() && opened2 == null)
				{
					Placeholder("Nothing");
				}
				else
				{
					IEnumerable<AreaView> enumerable2;
					if (opened2 != null)
					{
						enumerable2 = enumerable.Prepend(opened2);
					}
					else
					{
						IEnumerable<AreaView> enumerable3 = enumerable;
						enumerable2 = enumerable3;
					}
					foreach (AreaView current in enumerable2)
					{
						SetSelectable(current, current);
					}
				}
			});
		}
	}
}
