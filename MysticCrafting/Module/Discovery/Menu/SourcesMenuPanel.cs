using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Crafting;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Discovery.ItemList;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Menu;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.Menu
{
	public class SourcesMenuPanel : MenuPanel
	{
		private readonly Menu _sourcesMenu;

		private readonly ItemListModel _itemsListModel;

		public SourcesMenuPanel(ItemListModel model)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			_itemsListModel = model ?? throw new ArgumentNullException("model");
			MaxSelected = 11;
			CurrentSelected = 11;
			Menu val = new Menu();
			((Control)val).set_Parent((Container)(object)this);
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_CanSelect(true);
			_sourcesMenu = val;
			FilterMenuItem filterMenuItem = new FilterMenuItem(MysticCrafting.Module.Strings.Discovery.SourcesPanelAllOptions);
			((Control)filterMenuItem).set_Parent((Container)(object)_sourcesMenu);
			((MenuItem)filterMenuItem).set_CanCheck(true);
			((MenuItem)filterMenuItem).set_Checked(true);
			filterMenuItem.IsSelectAllOption = true;
			((MenuItem)filterMenuItem).add_CheckedChanged((EventHandler<CheckChangedEvent>)AllSourcesMenuItemOnCheckedChanged);
			((MenuItem)_sourcesMenu.AddCustomMenuItem(MysticCrafting.Module.Strings.Recipe.TradingPost, ServiceContainer.TextureRepository.Textures.TradingPostIcon)).add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent e)
			{
				_itemsListModel.Filter.IsTradeable = e.get_Checked();
				SourceMenuItemOnCheckChanged(sender, e);
			});
			_itemsListModel.Filter.IsTradeable = true;
			((MenuItem)_sourcesMenu.AddCustomMenuItem(MysticCrafting.Module.Strings.Recipe.Vendor, ServiceContainer.TextureRepository.Textures.VendorIcon)).add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent e)
			{
				_itemsListModel.Filter.SoldByVendor = e.get_Checked();
				SourceMenuItemOnCheckChanged(sender, e);
			});
			_itemsListModel.Filter.SoldByVendor = true;
			((MenuItem)_sourcesMenu.AddCustomMenuItem(MysticCrafting.Module.Strings.Recipe.MysticForge, ServiceContainer.TextureRepository.Textures.MysticForgeIcon)).add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent e)
			{
				_itemsListModel.Filter.HasMysticForgeRecipe = e.get_Checked();
				SourceMenuItemOnCheckChanged(sender, e);
			});
			_itemsListModel.Filter.HasMysticForgeRecipe = true;
			((MenuItem)_sourcesMenu.AddDiscipline(Discipline.Armorsmith)).add_CheckedChanged((EventHandler<CheckChangedEvent>)DisciplineMenuItemOnCheckChanged);
			((MenuItem)_sourcesMenu.AddDiscipline(Discipline.Artificer)).add_CheckedChanged((EventHandler<CheckChangedEvent>)DisciplineMenuItemOnCheckChanged);
			((MenuItem)_sourcesMenu.AddDiscipline(Discipline.Chef)).add_CheckedChanged((EventHandler<CheckChangedEvent>)DisciplineMenuItemOnCheckChanged);
			((MenuItem)_sourcesMenu.AddDiscipline(Discipline.Huntsman)).add_CheckedChanged((EventHandler<CheckChangedEvent>)DisciplineMenuItemOnCheckChanged);
			((MenuItem)_sourcesMenu.AddDiscipline(Discipline.Jeweler)).add_CheckedChanged((EventHandler<CheckChangedEvent>)DisciplineMenuItemOnCheckChanged);
			((MenuItem)_sourcesMenu.AddDiscipline(Discipline.Scribe)).add_CheckedChanged((EventHandler<CheckChangedEvent>)DisciplineMenuItemOnCheckChanged);
			((MenuItem)_sourcesMenu.AddDiscipline(Discipline.Tailor)).add_CheckedChanged((EventHandler<CheckChangedEvent>)DisciplineMenuItemOnCheckChanged);
			((MenuItem)_sourcesMenu.AddDiscipline(Discipline.Weaponsmith)).add_CheckedChanged((EventHandler<CheckChangedEvent>)DisciplineMenuItemOnCheckChanged);
			List<EnumFilterMenuItem<Discipline>> menuItems = (from c in _sourcesMenu.Items().OfType<EnumFilterMenuItem<Discipline>>()
				where !c.IsSelectAllOption
				select c).ToList();
			_itemsListModel.Filter.Disciplines = (from m in menuItems
				where ((MenuItem)m).get_Checked()
				select m into c
				select c.Value).ToList();
		}

		public void AllSourcesMenuItemOnCheckedChanged(object sender, CheckChangedEvent e)
		{
			_itemsListModel.Filter.IsTradeable = e.get_Checked();
			_itemsListModel.Filter.SoldByVendor = e.get_Checked();
			_itemsListModel.Filter.HasMysticForgeRecipe = e.get_Checked();
			DisciplineMenuItemOnCheckChanged(sender, e);
		}

		public void SourceMenuItemOnCheckChanged(object sender, CheckChangedEvent e)
		{
			List<FilterMenuItem> menuItems = (from c in _sourcesMenu.Items()
				where !c.IsSelectAllOption
				select c).ToList();
			CurrentSelected = menuItems.Sum((FilterMenuItem i) => ((MenuItem)i).get_Checked() ? 1 : 0);
			_itemsListModel.InvokeFilterChanged();
		}

		public void DisciplineMenuItemOnCheckChanged(object sender, CheckChangedEvent e)
		{
			List<EnumFilterMenuItem<Discipline>> menuItems = (from c in _sourcesMenu.Items().OfType<EnumFilterMenuItem<Discipline>>()
				where !c.IsSelectAllOption
				select c).ToList();
			_itemsListModel.Filter.Disciplines = (from m in menuItems
				where ((MenuItem)m).get_Checked()
				select m into c
				select c.Value).ToList();
			CurrentSelected = (from c in _sourcesMenu.Items()
				where !c.IsSelectAllOption
				select c).Sum((FilterMenuItem i) => ((MenuItem)i).get_Checked() ? 1 : 0);
			_itemsListModel.InvokeFilterChanged();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			Menu sourcesMenu = _sourcesMenu;
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)sourcesMenu).set_Size(((Rectangle)(ref contentRegion)).get_Size());
		}
	}
}
