using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	internal class ItemTabsPresenter
	{
		private readonly IItemSourceService _itemSourceService;

		private readonly IChoiceRepository _choiceRepository;

		public ItemTabsPresenter(IItemSourceService itemSourceService, IChoiceRepository choiceRepository)
		{
			_itemSourceService = itemSourceService;
			_choiceRepository = choiceRepository;
		}

		public IList<ItemTab> BuildTabs(Container parent, MysticItem item, Point location)
		{
			List<IItemSource> itemSources = _itemSourceService.GetItemSources(item)?.ToList();
			if (itemSources == null || !itemSources.Any())
			{
				return new List<ItemTab>();
			}
			ItemTabsContainer tabsContainer = new ItemTabsContainer(parent)
			{
				Location = location
			};
			TreeNodeBase node = parent as TreeNodeBase;
			if (node != null)
			{
				tabsContainer.Build(itemSources, node);
			}
			tabsContainer.Location = new Point(location.X - tabsContainer.Width, location.Y);
			return tabsContainer.Tabs;
		}

		public ItemTab AutoActivateTab(TreeNodeBase node, MysticItem item, IList<ItemTab> tabs)
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
