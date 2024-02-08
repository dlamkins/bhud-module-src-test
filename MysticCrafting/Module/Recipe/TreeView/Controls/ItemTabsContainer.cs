using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Nodes;

namespace MysticCrafting.Module.Recipe.TreeView.Controls
{
	[DebuggerDisplay("Name = {ItemSource.DisplayName}")]
	public class ItemTabsContainer : FlowPanel
	{
		public IList<IItemSource> ItemSources { get; set; }

		public IList<ItemTab> Tabs { get; private set; }

		public event EventHandler<CheckChangedEvent> Selected;

		public ItemTabsContainer(Container parent)
		{
			base.Parent = parent;
			base.FlowDirection = ControlFlowDirection.RightToLeft;
			base.ControlPadding = new Vector2(3f, 0f);
			base.Height = 40;
		}

		public void Build(IList<IItemSource> itemSources, TreeNodeBase node)
		{
			ItemSources = itemSources;
			int paddingLeft = 0;
			DisposeTabs();
			List<ItemTab> tabs = new List<ItemTab>();
			using (IEnumerator<IItemSource> enumerator = itemSources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemTab tab = new ItemTab(enumerator.Current)
					{
						Parent = this,
						Size = new Point(32, 32)
					};
					tab.Activated += Tab_ActiveChanged;
					tabs.Add(tab);
					paddingLeft += 35;
				}
			}
			IngredientNode ingredientNode = node as IngredientNode;
			if (ingredientNode != null && ingredientNode.Ingredient.GameIds != null && ingredientNode.Ingredient.GameIds.Any())
			{
				ItemTab swapTab = new ItemTab(new SwapItemSource($"swap_{ingredientNode.Ingredient.GameId}")
				{
					SwappableItemIds = ingredientNode.Ingredient.GameIds.ToList()
				})
				{
					Parent = this,
					Size = new Point(32, 32)
				};
				tabs.Add(swapTab);
				paddingLeft += 35;
			}
			Tabs = tabs;
			base.Width = paddingLeft;
		}

		private void Tab_ActiveChanged(object sender, CheckChangedEvent e)
		{
			ItemTab sourceTab = sender as ItemTab;
			if (sourceTab == null || !sourceTab.Active)
			{
				return;
			}
			foreach (ItemTab item in Tabs?.Where((ItemTab t) => !t.ItemSource.UniqueId.Equals(sourceTab.ItemSource.UniqueId)))
			{
				item.Active = false;
			}
		}

		public new void Dispose()
		{
			DisposeTabs();
			base.Dispose();
		}

		public void DisposeTabs()
		{
			if (Tabs == null)
			{
				return;
			}
			foreach (ItemTab tab in Tabs)
			{
				tab.Dispose();
			}
		}
	}
}
