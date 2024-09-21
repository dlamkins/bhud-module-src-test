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
	public class RaritiesMenuPanel : MenuPanel
	{
		private readonly Menu _raritiesMenu;

		private readonly ItemListModel _itemsListModel;

		public RaritiesMenuPanel(ItemListModel model)
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
			MaxSelected = 7;
			CurrentSelected = 7;
			Menu val = new Menu();
			((Control)val).set_Parent((Container)(object)this);
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_CanSelect(true);
			_raritiesMenu = val;
			FilterMenuItem filterMenuItem = new FilterMenuItem(MysticCrafting.Module.Strings.Discovery.RaritiesPanelAllOptions);
			((Control)filterMenuItem).set_Parent((Container)(object)_raritiesMenu);
			((MenuItem)filterMenuItem).set_CanCheck(true);
			((MenuItem)filterMenuItem).set_Checked(true);
			filterMenuItem.IsSelectAllOption = true;
			((MenuItem)filterMenuItem).add_CheckedChanged((EventHandler<CheckChangedEvent>)RarityMenuItemOnCheckChanged);
			((MenuItem)_raritiesMenu.AddRarity(ItemRarity.Legendary)).add_CheckedChanged((EventHandler<CheckChangedEvent>)RarityMenuItemOnCheckChanged);
			((MenuItem)_raritiesMenu.AddRarity(ItemRarity.Ascended)).add_CheckedChanged((EventHandler<CheckChangedEvent>)RarityMenuItemOnCheckChanged);
			((MenuItem)_raritiesMenu.AddRarity(ItemRarity.Exotic)).add_CheckedChanged((EventHandler<CheckChangedEvent>)RarityMenuItemOnCheckChanged);
			((MenuItem)_raritiesMenu.AddRarity(ItemRarity.Rare)).add_CheckedChanged((EventHandler<CheckChangedEvent>)RarityMenuItemOnCheckChanged);
			((MenuItem)_raritiesMenu.AddRarity(ItemRarity.Masterwork)).add_CheckedChanged((EventHandler<CheckChangedEvent>)RarityMenuItemOnCheckChanged);
			((MenuItem)_raritiesMenu.AddRarity(ItemRarity.Fine)).add_CheckedChanged((EventHandler<CheckChangedEvent>)RarityMenuItemOnCheckChanged);
			((MenuItem)_raritiesMenu.AddRarity(ItemRarity.Basic)).add_CheckedChanged((EventHandler<CheckChangedEvent>)RarityMenuItemOnCheckChanged);
			List<EnumFilterMenuItem<ItemRarity>> menuItems = (from c in _raritiesMenu.Items().OfType<EnumFilterMenuItem<ItemRarity>>()
				where !c.IsSelectAllOption
				select c).ToList();
			_itemsListModel.Filter.Rarities = (from m in menuItems
				where ((MenuItem)m).get_Checked()
				select m into c
				select c.Value).ToList();
		}

		public void RarityMenuItemOnCheckChanged(object sender, CheckChangedEvent e)
		{
			List<EnumFilterMenuItem<ItemRarity>> menuItems = (from c in _raritiesMenu.Items().OfType<EnumFilterMenuItem<ItemRarity>>()
				where !c.IsSelectAllOption
				select c).ToList();
			_itemsListModel.Filter.Rarities = (from m in menuItems
				where ((MenuItem)m).get_Checked()
				select m into c
				select c.Value).ToList();
			FilterMenuItem selectAllOption = ((IEnumerable)((Container)_raritiesMenu).get_Children()).OfType<FilterMenuItem>().FirstOrDefault((FilterMenuItem m) => m.IsSelectAllOption);
			if (selectAllOption != null)
			{
				if (e.get_Checked() && menuItems.All((EnumFilterMenuItem<ItemRarity> c) => ((MenuItem)c).get_Checked()))
				{
					selectAllOption.SetCheckedSilently(@checked: true);
				}
				else if (menuItems.Any((EnumFilterMenuItem<ItemRarity> c) => !((MenuItem)c).get_Checked()))
				{
					selectAllOption.SetCheckedSilently(@checked: false);
				}
			}
			CurrentSelected = menuItems.Sum((EnumFilterMenuItem<ItemRarity> i) => ((MenuItem)i).get_Checked() ? 1 : 0);
			_itemsListModel.InvokeFilterChanged();
		}

		protected override void DisposeControl()
		{
			((FlowPanel)this).DisposeControl();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			Menu raritiesMenu = _raritiesMenu;
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)raritiesMenu).set_Size(((Rectangle)(ref contentRegion)).get_Size());
		}
	}
}
