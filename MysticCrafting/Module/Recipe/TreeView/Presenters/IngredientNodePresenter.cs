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
using MysticCrafting.Module.Strings;

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

		private readonly List<int> _excludedItems = new List<int> { 19675, 96978, 79410, 20797, 19925, 86093 };

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
			if (recipe?.Ingredients == null || (!recipe.HasBaseIngredients && !recipe.IsMysticForgeRecipe) || parent == null)
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
				if (!_excludedItems.Contains(node.Item.GameId))
				{
					ItemTab selectedTab = BuildTabs(node, node.Item);
					if (selectedTab != null)
					{
						BuildChildren(selectedTab.ItemSource, node);
					}
					else
					{
						node.BuildMissingTabsLabel();
					}
				}
				else
				{
					node.BuildMissingTabsLabel();
				}
				node.LoadingChildren = false;
				node.OnChildrenLoaded();
			}
		}

		public IngredientNode BuildNode(MysticItem item, Container parent, bool expandable = false, bool isPrimaryNode = false)
		{
			IngredientNode node = new IngredientNode(new MysticIngredient
			{
				GameId = item.GameId,
				Quantity = 1,
				Index = 0,
				Name = item.Name,
				Item = item
			}, item, parent, loadingChildren: true)
			{
				Width = parent.Width - (isPrimaryNode ? 50 : 25),
				PanelHeight = 45,
				PanelExtensionHeight = 0,
				Expandable = expandable
			};
			node.Build(parent);
			node.OnPanelClick += delegate
			{
				_recipeDetailsPresenter.SaveScrollDistance();
			};
			node.Toggle();
			ItemTab selectedTab = BuildTabs(node, item);
			if (selectedTab != null)
			{
				BuildChildren(selectedTab.ItemSource, node);
			}
			else
			{
				node.BuildMissingTabsLabel();
			}
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
			ingredientNode.Build(parent);
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
				IEnumerable<int> recipeSheetIds = recipeSource.Recipe.RecipeSheetIds;
				int num;
				if (recipeSheetIds == null)
				{
					num = 0;
				}
				else
				{
					recipeSheetIds.FirstOrDefault();
					num = 1;
				}
				if (num != 0)
				{
					_recipeSheetPresenter.BuildNode(recipeSource.Recipe, parent);
				}
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
			ItemContainerSource containerSource = itemSource as ItemContainerSource;
			if (containerSource == null)
			{
				return;
			}
			if (containerSource.Container != null)
			{
				IngredientNode parentNode = parent as IngredientNode;
				if (parentNode != null)
				{
					if (containerSource.Container.ContainedChoiceItemIds != null && containerSource.Container.ContainedChoiceItemIds.Contains(parentNode.Item.GameId))
					{
						new LabelNode(MysticCrafting.Module.Strings.Recipe.ChoiceOfItem, parent)
						{
							Width = 300,
							TextColor = Color.Red
						};
					}
					if (containerSource.Container.ContainedChanceItemIds != null && containerSource.Container.ContainedChanceItemIds.Contains(parentNode.Item.GameId))
					{
						new LabelNode(MysticCrafting.Module.Strings.Recipe.RandomItemChance, parent)
						{
							Width = 300,
							TextColor = Color.Red
						};
					}
				}
			}
			if (containerSource.ContainerItem != null)
			{
				BuildNode(containerSource.ContainerItem, parent, expandable: true);
			}
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

		private void Event_LayoutChanged(object sender, CheckChangedEvent e)
		{
			_recipeDetailsPresenter.SaveScrollDistance();
		}
	}
}
