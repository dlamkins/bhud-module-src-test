using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
{
	public class ItemTabsContainer : FlowPanel
	{
		public IList<IItemSource> ItemSources { get; set; }

		public IList<ItemTab> Tabs => ((IEnumerable)((Container)this).get_Children()).OfType<ItemTab>().ToList();

		public ItemTabsContainer(Container parent)
			: this()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)4);
			((FlowPanel)this).set_ControlPadding(new Vector2(3f, 0f));
			((Control)this).set_Height(40);
		}

		public void Build(IList<IItemSource> itemSources, TreeNodeBase node)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			ItemSources = itemSources;
			int paddingLeft = 0;
			foreach (IItemSource itemSource in itemSources)
			{
				ItemTab itemTab = new ItemTab(itemSource);
				((Control)itemTab).set_Parent((Container)(object)this);
				((Control)itemTab).set_Size(new Point(32, 32));
				itemTab.Activated += Tab_ActiveChanged;
				paddingLeft += 35;
			}
			IngredientNode ingredientNode = node as IngredientNode;
			if (ingredientNode != null && !ingredientNode.IsTopIngredient)
			{
				BuildIgnoreTab();
				paddingLeft += 35;
			}
			((Control)this).set_Width(paddingLeft);
		}

		private ItemTab BuildIgnoreTab()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			ItemTab itemTab = new ItemTab(new IgnoreSource("ignore"));
			((Control)itemTab).set_Parent((Container)(object)this);
			((Control)itemTab).set_Size(new Point(32, 32));
			((Control)itemTab).set_BasicTooltipText(Recipe.IgnoreItemSource);
			itemTab.Activated += Tab_ActiveChanged;
			return itemTab;
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

		protected override void DisposeControl()
		{
			Tooltip tooltip = ((Control)this).get_Tooltip();
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			foreach (ItemTab tab in Tabs)
			{
				tab.Activated -= Tab_ActiveChanged;
			}
			((FlowPanel)this).DisposeControl();
		}
	}
}
