using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Recipe.TreeView.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public class IngredientNodePresenter : IIngredientNodePresenter
	{
		private readonly IRecipeDetailsViewPresenter _recipeDetailsPresenter;

		private readonly ItemTabsPresenter _tabsPresenter = new ItemTabsPresenter(ServiceContainer.ItemSourceService, ServiceContainer.ChoiceRepository);

		private readonly TradingPostPresenter _tradingPostPresenter = new TradingPostPresenter(ServiceContainer.ChoiceRepository);

		private readonly VendorPresenter _vendorPresenter = new VendorPresenter(ServiceContainer.ChoiceRepository);

		private readonly IRecipeSheetNodePresenter _recipeSheetPresenter;

		private readonly IItemRepository _itemRepository;

		private readonly IChoiceRepository _choiceRepository;

		private readonly List<int> _excludedItems = new List<int> { 19675, 96978, 79410, 20797, 19925, 86093, 68063 };

		public IngredientNodePresenter(IRecipeDetailsViewPresenter recipeDetailsPresenter, IItemRepository itemRepository, IChoiceRepository choiceRepository)
		{
			_recipeDetailsPresenter = recipeDetailsPresenter;
			_tradingPostPresenter.SelectChanged += Event_LayoutChanged;
			_vendorPresenter.SelectChanged += Event_LayoutChanged;
			_itemRepository = itemRepository;
			_choiceRepository = choiceRepository;
			_recipeSheetPresenter = new RecipeSheetNodePresenter(_recipeDetailsPresenter, this);
		}

		public void BuildNodes(MysticRecipe recipe, Container parent)
		{
			if (recipe?.Ingredients == null || (recipe.HasBaseIngredients != "t" && recipe.MysticForgeId == 0) || parent == null)
			{
				return;
			}
			foreach (MysticIngredient ingredient in recipe.Ingredients.OrderBy((MysticIngredient i) => i.Index))
			{
				IngredientNode node = BuildNode(ingredient, parent, loadingChildren: true);
				if (node == null)
				{
					continue;
				}
				if (!_excludedItems.Contains(node.Item.Id))
				{
					ItemTab selectedTab = BuildTabs(node, node.Item);
					if (selectedTab != null)
					{
						BuildChildren(selectedTab.ItemSource, node);
					}
				}
				node.LoadingChildren = false;
				node.OnChildrenLoaded();
			}
		}

		public IngredientNode BuildNode(MysticItem item, Container parent, bool expandable = false)
		{
			IngredientNode node = new IngredientNode(new MysticIngredient
			{
				GameId = item.Id,
				Quantity = 1,
				Index = 0,
				Name = item.Name,
				Item = item
			}, item, parent, loadingChildren: true)
			{
				Width = parent.Width - 50,
				PanelHeight = 45,
				PanelExtensionHeight = 0,
				Expandable = expandable
			};
			node.OnPanelClick += delegate
			{
				_recipeDetailsPresenter.SaveScrollDistance();
			};
			node.Toggle();
			ItemTab selectedTab = BuildTabs(node, item);
			BuildChildren(selectedTab.ItemSource, node);
			node.LoadingChildren = false;
			node.OnChildrenLoaded();
			return node;
		}

		public IngredientNode BuildNode(MysticIngredient ingredient, Container parent, bool loadingChildren = false, bool expandable = false)
		{
			MysticItem item = _itemRepository.GetItem(ingredient.GameId);
			if (item == null)
			{
				return null;
			}
			IngredientNode ingredientNode = new IngredientNode(ingredient, item, parent, loadingChildren);
			ingredientNode.Width = parent.Width - 25;
			ingredientNode.PanelHeight = 45;
			ingredientNode.PanelExtensionHeight = 0;
			ingredientNode.OnPanelClick += delegate
			{
				_recipeDetailsPresenter.SaveScrollDistance();
			};
			return ingredientNode;
		}

		public void BuildChildren(IItemSource itemSource, Container parent)
		{
			(parent as TreeNodeBase)?.ClearChildNodes();
			(parent as IIngredientContainer)?.ClearChildIngredientNodes();
			RecipeSource recipeSource = itemSource as RecipeSource;
			if (recipeSource != null && recipeSource.Recipe != null)
			{
				BuildNodes(recipeSource.Recipe, parent);
			}
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
			WizardsVaultSource wizardsVaultSource = itemSource as WizardsVaultSource;
			if (wizardsVaultSource == null)
			{
				return;
			}
			IngredientNode parentNode = parent as IngredientNode;
			if (parentNode != null)
			{
				if (parentNode.Item.Id == 19673 || parentNode.Item.Id == 19672)
				{
					new LabelNode("50% chance to get this item.", parent)
					{
						Width = 200,
						TextColor = Color.Yellow
					};
				}
				if (parentNode.Item.Id == 96054)
				{
					new LabelNode("Limit 1 per account.", parent)
					{
						Width = 200,
						TextColor = Color.Yellow
					};
				}
			}
			BuildNode(wizardsVaultSource.Item, parent, expandable: true);
		}

		public ItemTab BuildTabs(TreeNodeBase node, MysticItem item)
		{
			_recipeDetailsPresenter.SaveScrollDistance();
			IList<ItemTab> tabs = _tabsPresenter.BuildTabs(node, item, new Point(node.Width - 5, 7));
			if (!tabs.Any())
			{
				return null;
			}
			ItemTab selectedTab = _tabsPresenter.AutoActivateTab(node, item, tabs);
			foreach (ItemTab item2 in tabs)
			{
				item2.Activated += ItemSource_OnTabSelected;
			}
			return selectedTab;
		}

		private void ItemSource_OnTabSelected(object sender, CheckChangedEvent e)
		{
			_recipeDetailsPresenter.SaveScrollDistance();
			ItemTab tab = sender as ItemTab;
			if (tab == null || !tab.Active)
			{
				return;
			}
			Container parent = tab.Parent.Parent;
			(parent as ITradeableItemNode)?.ResetPrices();
			BuildChildren(tab.ItemSource, parent);
			IngredientNode node = parent as IngredientNode;
			if (node != null)
			{
				_choiceRepository.SaveChoice(node.FullPath, tab.ItemSource.UniqueId, ChoiceType.ItemSource);
				node.TreeView.IngredientNodes.Reindex(node.NodeIndex);
				if (!node.Expanded)
				{
					node.Toggle();
				}
			}
		}

		private void BuildRecipeSheet(MysticRecipe recipe, Container parent)
		{
			if (recipe.RecipeSheetIds == null || !recipe.RecipeSheetIds.Any())
			{
				return;
			}
			int sheetId = recipe.RecipeSheetIds.FirstOrDefault();
			MysticItem item = ServiceContainer.ItemRepository.GetItem(sheetId);
			if (item != null)
			{
				new LabelNode("Learned from", parent).Width = 200;
				IngredientNode node = new IngredientNode(new MysticIngredient
				{
					Quantity = 1,
					GameId = recipe.RecipeSheetIds.FirstOrDefault(),
					Item = item,
					Index = 0,
					Name = item.Name
				}, item, parent)
				{
					Parent = parent,
					Width = parent.Width - 25,
					PanelHeight = 45,
					PanelExtensionHeight = 0
				};
				node.OnPanelClick += delegate
				{
					_recipeDetailsPresenter.SaveScrollDistance();
				};
				BuildTabs(node, node.Item);
			}
		}

		private void Event_LayoutChanged(object sender, CheckChangedEvent e)
		{
			_recipeDetailsPresenter.SaveScrollDistance();
		}
	}
}
