using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	internal class ItemTabsPresenter
	{
		private readonly IItemSourceService _itemSourceService;

		public ItemTabsPresenter(IItemSourceService itemSourceService)
		{
			_itemSourceService = itemSourceService;
		}

		public IList<ItemTab> BuildTabs(Container parent, Item item, Point location)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			List<IItemSource> itemSources = _itemSourceService.GetItemSources(item)?.ToList();
			if (itemSources == null || !itemSources.Any())
			{
				return new List<ItemTab>();
			}
			ItemTabsContainer itemTabsContainer = new ItemTabsContainer(parent);
			((Control)itemTabsContainer).set_Location(location);
			ItemTabsContainer tabsContainer = itemTabsContainer;
			TreeNodeBase node = parent as TreeNodeBase;
			if (node != null)
			{
				tabsContainer.Build(itemSources, node);
			}
			((Control)tabsContainer).set_Location(new Point(location.X - ((Control)tabsContainer).get_Width(), location.Y));
			return tabsContainer.Tabs;
		}

		public ItemTab AutoActivateTab(TreeNodeBase node, IList<ItemTab> tabs)
		{
			string itemSourceId = _itemSourceService.GetPreferredItemSource(node.FullPath);
			ItemTab tab = tabs.FirstOrDefault();
			if (itemSourceId != null)
			{
				tab = tabs.FirstOrDefault((ItemTab t) => t.ItemSource.UniqueId.Equals(itemSourceId));
				if (tab != null)
				{
					tab.Active = true;
				}
			}
			if (tab != null)
			{
				tab.Active = true;
			}
			return tab;
		}
	}
}
