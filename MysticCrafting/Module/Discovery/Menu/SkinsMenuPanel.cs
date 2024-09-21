using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Discovery.ItemList;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Menu;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.Menu
{
	public class SkinsMenuPanel : MenuPanel
	{
		private readonly Menu _skinsMenu;

		private readonly ItemListModel _itemsListModel;

		public SkinsMenuPanel(ItemListModel model)
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
			MaxSelected = 3;
			CurrentSelected = 3;
			Menu val = new Menu();
			((Control)val).set_Parent((Container)(object)this);
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_CanSelect(true);
			_skinsMenu = val;
			FilterMenuItem filterMenuItem = new FilterMenuItem(MysticCrafting.Module.Strings.Discovery.SkinsPanelAllOptions);
			((Control)filterMenuItem).set_Parent((Container)(object)_skinsMenu);
			((MenuItem)filterMenuItem).set_CanCheck(true);
			((MenuItem)filterMenuItem).set_Checked(true);
			filterMenuItem.IsSelectAllOption = true;
			((MenuItem)filterMenuItem).add_CheckedChanged((EventHandler<CheckChangedEvent>)MenuItemOnCheckChanged);
			((MenuItem)_skinsMenu.AddCustomMenuItem(MysticCrafting.Module.Strings.Discovery.SkinsPanelNoSkins, null)).add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent @event)
			{
				_itemsListModel.Filter.HideSkinLocked = !@event.get_Checked();
				MenuItemOnCheckChanged(sender, @event);
			});
			((MenuItem)_skinsMenu.AddCustomMenuItem(MysticCrafting.Module.Strings.Discovery.SkinsPanelSkinsUnlocked, null)).add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent @event)
			{
				_itemsListModel.Filter.HideSkinUnlocked = !@event.get_Checked();
				MenuItemOnCheckChanged(sender, @event);
			});
			((MenuItem)_skinsMenu.AddCustomMenuItem(MysticCrafting.Module.Strings.Discovery.SkinsPanelSkinsLocked, null)).add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent @event)
			{
				_itemsListModel.Filter.HideSkinLocked = !@event.get_Checked();
				MenuItemOnCheckChanged(sender, @event);
			});
		}

		public void MenuItemOnCheckChanged(object sender, CheckChangedEvent e)
		{
			List<FilterMenuItem> menuItems = (from c in _skinsMenu.Items()
				where !c.IsSelectAllOption
				select c).ToList();
			CurrentSelected = menuItems.Sum((FilterMenuItem i) => ((MenuItem)i).get_Checked() ? 1 : 0);
			_itemsListModel.InvokeFilterChanged();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			Menu skinsMenu = _skinsMenu;
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)skinsMenu).set_Size(((Rectangle)(ref contentRegion)).get_Size());
		}
	}
}
