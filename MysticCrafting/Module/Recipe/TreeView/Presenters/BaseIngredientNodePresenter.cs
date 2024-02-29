using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public class BaseIngredientNodePresenter : IBaseIngredientNodePresenter
	{
		private readonly IItemRepository _itemRepository;

		private readonly IIngredientNodePresenter _ingredientPresenter;

		private readonly IItemSourceService _itemSourceService;

		private readonly ItemTabsPresenter _tabsPresenter = new ItemTabsPresenter(ServiceContainer.ItemSourceService, ServiceContainer.ChoiceRepository);

		private readonly TradingPostPresenter _tradingPostPresenter = new TradingPostPresenter(ServiceContainer.ChoiceRepository);

		private readonly VendorPresenter _vendorPresenter = new VendorPresenter(ServiceContainer.ChoiceRepository);

		private IRecipeDetailsViewPresenter TreeView { get; }

		public BaseIngredientNodePresenter(IRecipeDetailsViewPresenter treeView, IItemRepository itemRepository, IIngredientNodePresenter ingredientPresenter, IItemSourceService itemSourceService)
		{
			TreeView = treeView;
			_itemRepository = itemRepository;
			_ingredientPresenter = ingredientPresenter;
			_itemSourceService = itemSourceService;
		}

		public void BuildNodes(MysticRecipe recipe, Container primaryParent)
		{
			BuildNodes(recipe, primaryParent, null);
			foreach (BaseIngredientNode node in primaryParent.Children.OfType<BaseIngredientNode>())
			{
				if (node.Levels.Count < 2)
				{
					BuildTabs(node, node.Item);
				}
			}
		}

		public void BuildNodes(MysticRecipe recipe, Container primaryParent, MysticIngredientLevel parentIngredientLevel = null)
		{
			IEnumerable<BaseIngredientNode> existingNodes = primaryParent.Children.OfType<BaseIngredientNode>();
			if (recipe?.Ingredients == null || (!recipe.HasBaseIngredients && !recipe.IsMysticForgeRecipe))
			{
				return;
			}
			foreach (MysticIngredient ingredient in recipe.Ingredients)
			{
				ingredient.Item = _itemRepository.GetItem(ingredient.GameId);
				MysticIngredientLevel level = new MysticIngredientLevel(ingredient);
				if (parentIngredientLevel != null)
				{
					parentIngredientLevel.Child = level;
				}
				else
				{
					level.Parent = primaryParent as IngredientNode;
				}
				IItemSource itemSource = _itemSourceService.GetPreferredItemSource(level.GetFullPath(), ingredient.GameId);
				if (itemSource != null)
				{
					RecipeSource recipeSource = itemSource as RecipeSource;
					if (recipeSource != null)
					{
						BuildNodes(recipeSource.Recipe, primaryParent, level);
						continue;
					}
				}
				BaseIngredientNode node = existingNodes.FirstOrDefault((BaseIngredientNode b) => b.Item.GameId == ingredient.GameId);
				if (node == null)
				{
					node = BuildNode(ingredient.Item, primaryParent);
				}
				node.Levels.Add(level);
				if (node.TotalRequiredQuantity == 0)
				{
					node.Hide();
					continue;
				}
				node.BuildItemCountControls();
				node.BuildItemCountTooltip();
			}
		}

		private BaseIngredientNode BuildNode(MysticItem item, Container parent)
		{
			BaseIngredientNode baseIngredientNode = new BaseIngredientNode(item);
			baseIngredientNode.Parent = parent;
			baseIngredientNode.Width = parent.Width - 50;
			baseIngredientNode.PanelHeight = 65;
			baseIngredientNode.PanelExtensionHeight = 40;
			baseIngredientNode.OnPanelClick += delegate
			{
				TreeView.SaveScrollDistance();
			};
			return baseIngredientNode;
		}

		public ItemTab BuildTabs(BaseIngredientNode node, MysticItem item)
		{
			TreeView.SaveScrollDistance();
			IList<ItemTab> tabs = _tabsPresenter.BuildTabs(node, item, new Point(0, 70));
			foreach (ItemTab item2 in tabs)
			{
				item2.Activated += ItemSource_OnTabSelected;
			}
			return _tabsPresenter.AutoActivateTab(node, item, tabs);
		}

		private void ItemSource_OnTabSelected(object sender, CheckChangedEvent e)
		{
			TreeView.SaveScrollDistance();
			ItemTab tab = sender as ItemTab;
			if (tab != null && tab.Active)
			{
				(tab.Parent as ITradeableItemNode)?.ResetPrices();
				BuildChildren(tab.ItemSource, tab.Parent);
			}
		}

		public void BuildChildren(IItemSource itemSource, Container parent)
		{
			(parent as TreeNodeBase)?.ClearChildNodes();
			TradingPostSource tradingPostSource = itemSource as TradingPostSource;
			if (tradingPostSource != null)
			{
				_tradingPostPresenter.Build(parent, tradingPostSource);
			}
			VendorSource vendorSource = itemSource as VendorSource;
			if (vendorSource != null)
			{
				_vendorPresenter.Build(parent, vendorSource);
			}
		}
	}
}
