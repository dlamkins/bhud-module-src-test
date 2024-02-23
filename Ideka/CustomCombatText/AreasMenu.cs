using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Ideka.CustomCombatText
{
	public class AreasMenu : SelectablesMenu<AreaView, AreaView>
	{
		public event Action<AreaView>? DoubleClicked;

		protected override AreaView? ExtractId(AreaView? item)
		{
			return item;
		}

		public void UpdateName(AreaView target, bool indent)
		{
			AreaView target2 = target;
			Update(target2, delegate(MenuItem item)
			{
				item.set_Text((indent ? "  " : "") + target2.Model.Describe);
			});
		}

		public void SetOpened(AreaView? opened)
		{
			AreaView opened2 = opened;
			Repopulate(delegate
			{
				IEnumerable<AreaView> enumerable = ((opened2 == null) ? CTextModule.LocalData.RootAreaViews : opened2.GetChildren());
				if (!enumerable.Any() && opened2 == null)
				{
					Placeholder("Nothing");
				}
				else
				{
					_menu.set_CanSelect(true);
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
					foreach (AreaView area in enumerable2)
					{
						string describe = area.Model.Describe;
						string text = ((opened2 == null || opened2 == area) ? describe : ("  " + describe));
						AreasMenu areasMenu = this;
						AreaView id = area;
						OnelineMenuItem onelineMenuItem = new OnelineMenuItem(text);
						((Control)onelineMenuItem).set_Parent((Container)(object)_menu);
						OnelineMenuItem item = areasMenu.SetItem(id, onelineMenuItem);
						((Control)item).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
						{
							OnelineMenuItem onelineMenuItem2 = item;
							string describe2 = area.Model.Describe;
							int count = area.Model.Receivers.Count;
							string text2 = count switch
							{
								1 => "1 receiver.", 
								0 => "No receivers.", 
								_ => $"{count} receivers.", 
							};
							int num = area.GetChildren().Count();
							string text3 = num switch
							{
								1 => "1 area inside.", 
								0 => "No areas inside.", 
								_ => $"{num} areas inside.", 
							};
							((Control)onelineMenuItem2).set_BasicTooltipText("Area: " + describe2 + "\n" + text2 + "\n" + text3 + "\nDouble click to open.");
						});
						((Control)item).add_Click((EventHandler<MouseEventArgs>)delegate(object _, MouseEventArgs args)
						{
							Select(area);
							if (args.get_IsDoubleClick())
							{
								this.DoubleClicked?.Invoke(area);
							}
						});
					}
				}
			});
		}
	}
}
