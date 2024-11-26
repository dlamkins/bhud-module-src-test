using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Discovery.ItemList;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Menu;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.Menu
{
	public class WeightClassesMenuPanel : MenuPanel
	{
		private readonly Menu _weightClassMenu;

		private readonly ItemListModel _itemsListModel;

		public WeightClassesMenuPanel(ItemListModel model)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			_itemsListModel = model ?? throw new ArgumentNullException("model");
			MaxSelected = 5;
			CurrentSelected = 5;
			Menu val = new Menu();
			((Control)val).set_Parent((Container)(object)this);
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_CanSelect(true);
			_weightClassMenu = val;
			FilterMenuItem filterMenuItem = new FilterMenuItem(MysticCrafting.Module.Strings.Discovery.WeightClassesPanelAllOptions);
			((Control)filterMenuItem).set_Parent((Container)(object)_weightClassMenu);
			((MenuItem)filterMenuItem).set_CanCheck(true);
			((MenuItem)filterMenuItem).set_Checked(true);
			filterMenuItem.IsSelectAllOption = true;
			((MenuItem)filterMenuItem).add_CheckedChanged((EventHandler<CheckChangedEvent>)WeightClassMenuItemOnCheckChanged);
			((MenuItem)_weightClassMenu.AddWeightClass(WeightClass.Heavy)).add_CheckedChanged((EventHandler<CheckChangedEvent>)WeightClassMenuItemOnCheckChanged);
			((MenuItem)_weightClassMenu.AddWeightClass(WeightClass.Medium)).add_CheckedChanged((EventHandler<CheckChangedEvent>)WeightClassMenuItemOnCheckChanged);
			((MenuItem)_weightClassMenu.AddWeightClass(WeightClass.Light)).add_CheckedChanged((EventHandler<CheckChangedEvent>)WeightClassMenuItemOnCheckChanged);
			((MenuItem)_weightClassMenu.AddWeightClass(WeightClass.Clothing)).add_CheckedChanged((EventHandler<CheckChangedEvent>)WeightClassMenuItemOnCheckChanged);
			((MenuItem)_weightClassMenu.AddWeightClass(WeightClass.Unknown)).add_CheckedChanged((EventHandler<CheckChangedEvent>)WeightClassMenuItemOnCheckChanged);
			List<EnumFilterMenuItem<WeightClass>> weightClasses = (from c in _weightClassMenu.Items().OfType<EnumFilterMenuItem<WeightClass>>()
				where !c.IsSelectAllOption
				select c).ToList();
			_itemsListModel.Filter.WeightClasses = (from m in weightClasses
				where ((MenuItem)m).get_Checked()
				select m into c
				select c.Value).ToList();
		}

		public void WeightClassMenuItemOnCheckChanged(object sender, CheckChangedEvent e)
		{
			List<EnumFilterMenuItem<WeightClass>> menuItems = (from c in _weightClassMenu.Items().OfType<EnumFilterMenuItem<WeightClass>>()
				where !c.IsSelectAllOption
				select c).ToList();
			_itemsListModel.Filter.WeightClasses = (from m in menuItems
				where ((MenuItem)m).get_Checked()
				select m into c
				select c.Value).ToList();
			FilterMenuItem selectAllOption = ((IEnumerable)((Container)_weightClassMenu).get_Children()).OfType<FilterMenuItem>().FirstOrDefault((FilterMenuItem m) => m.IsSelectAllOption);
			if (selectAllOption != null)
			{
				if (e.get_Checked() && menuItems.All((EnumFilterMenuItem<WeightClass> c) => ((MenuItem)c).get_Checked()))
				{
					selectAllOption.SetCheckedSilently(@checked: true);
				}
				else if (menuItems.Any((EnumFilterMenuItem<WeightClass> c) => !((MenuItem)c).get_Checked()))
				{
					selectAllOption.SetCheckedSilently(@checked: false);
				}
			}
			CurrentSelected = menuItems.Sum((EnumFilterMenuItem<WeightClass> i) => ((MenuItem)i).get_Checked() ? 1 : 0);
			_itemsListModel.InvokeFilterChanged();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			Menu weightClassMenu = _weightClassMenu;
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)weightClassMenu).set_Size(((Rectangle)(ref contentRegion)).get_Size());
		}
	}
}
