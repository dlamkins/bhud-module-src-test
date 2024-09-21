using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Currencies;
using Atzie.MysticCrafting.Models.Items;
using Atzie.MysticCrafting.Models.Vendor;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public class IngredientNodePresenter : IIngredientNodePresenter
	{
		private readonly IRecipeDetailsViewPresenter _recipeDetailsPresenter;

		private readonly ItemTabsPresenter _tabsPresenter = new ItemTabsPresenter(ServiceContainer.ItemSourceService);

		private readonly TradingPostPresenter _tradingPostPresenter = new TradingPostPresenter(ServiceContainer.ChoiceRepository);

		private readonly VendorPresenter _vendorPresenter = new VendorPresenter(ServiceContainer.ChoiceRepository);

		private readonly CurrencyNodePresenter _currencyNodePresenter = new CurrencyNodePresenter();

		private readonly IRecipeSheetNodePresenter _recipeSheetPresenter;

		private readonly IChoiceRepository _choiceRepository;

		private readonly List<int> _excludedItems = new List<int> { 19675, 79410, 20797, 19925, 86093 };

		public IngredientNodePresenter(IRecipeDetailsViewPresenter recipeDetailsPresenter, IChoiceRepository choiceRepository)
		{
			_recipeDetailsPresenter = recipeDetailsPresenter;
			_tradingPostPresenter.SelectChanged += Event_LayoutChanged;
			_vendorPresenter.SelectChanged += Event_LayoutChanged;
			_choiceRepository = choiceRepository;
			_recipeSheetPresenter = new RecipeSheetNodePresenter(_recipeDetailsPresenter, this);
		}

		public void BuildNodes(Atzie.MysticCrafting.Models.Crafting.Recipe recipe, Container parent)
		{
			if (recipe?.Ingredients == null || (!recipe.HasBaseIngredients && !recipe.IsMysticForgeRecipe) || parent == null)
			{
				return;
			}
			foreach (Ingredient ingredient in recipe.Ingredients.OrderBy((Ingredient i) => i.Index))
			{
				IngredientNode node = ((ingredient.Item != null) ? ((IngredientNode)BuildItemNode(ingredient, parent, loadingChildren: true)) : ((IngredientNode)BuildUnknownNode(ingredient, parent, loadingChildren: true)));
				if (node == null)
				{
					continue;
				}
				if (!_excludedItems.Contains(node.Id))
				{
					ItemIngredientNode itemNode = node as ItemIngredientNode;
					if (itemNode != null)
					{
						ItemTab selectedTab = BuildTabs(node, itemNode.Item);
						if (selectedTab != null)
						{
							BuildChildren(selectedTab.ItemSource, (Container)(object)node);
						}
						else
						{
							node.BuildMissingTabsLabel();
						}
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

		public ItemIngredientNode BuildItemNode(Item item, Container parent, bool expandable = false, bool isPrimaryNode = false, bool openByDefault = true, int quantity = 1)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			ItemIngredientNode itemIngredientNode = new ItemIngredientNode(item, quantity, parent, 0);
			((Control)itemIngredientNode).set_Width(parent.get_ContentRegion().Width - (isPrimaryNode ? 50 : 25));
			itemIngredientNode.PanelHeight = 40;
			itemIngredientNode.PanelExtensionHeight = 0;
			itemIngredientNode.Expandable = expandable;
			ItemIngredientNode node = itemIngredientNode;
			node.Build(parent);
			node.OnPanelClick += delegate
			{
				_recipeDetailsPresenter.SaveScrollDistance();
			};
			if (openByDefault)
			{
				node.Toggle();
			}
			ItemTab selectedTab = BuildTabs(node, item);
			if (selectedTab != null)
			{
				BuildChildren(selectedTab.ItemSource, (Container)(object)node);
			}
			else
			{
				node.BuildMissingTabsLabel();
			}
			node.LoadingChildren = false;
			node.OnChildrenLoaded();
			return node;
		}

		public UnknownIngredientNode BuildUnknownNode(Ingredient ingredient, Container parent, bool loadingChildren = false, bool expandable = false)
		{
			if (ingredient.Item != null)
			{
				return null;
			}
			UnknownIngredientNode unknownIngredientNode = new UnknownIngredientNode(ingredient, parent, null, loadingChildren);
			((Control)unknownIngredientNode).set_Width(((Control)parent).get_Width() - 25);
			unknownIngredientNode.PanelHeight = 40;
			unknownIngredientNode.PanelExtensionHeight = 0;
			unknownIngredientNode.Build(parent);
			unknownIngredientNode.OnPanelClick += delegate
			{
				_recipeDetailsPresenter.SaveScrollDistance();
			};
			return unknownIngredientNode;
		}

		public ItemIngredientNode BuildItemNode(Ingredient ingredient, Container parent, bool loadingChildren = false, bool expandable = false)
		{
			if (ingredient.Item == null)
			{
				return null;
			}
			ItemIngredientNode itemIngredientNode = new ItemIngredientNode(ingredient, parent, null, loadingChildren);
			((Control)itemIngredientNode).set_Width(((Control)parent).get_Width() - 25);
			itemIngredientNode.PanelHeight = 40;
			itemIngredientNode.PanelExtensionHeight = 0;
			itemIngredientNode.Build(parent);
			itemIngredientNode.OnPanelClick += delegate
			{
				_recipeDetailsPresenter.SaveScrollDistance();
			};
			return itemIngredientNode;
		}

		private bool IsCircularReference(TreeNodeBase node)
		{
			int itemId = 0;
			ItemIngredientNode ingredientNode = node as ItemIngredientNode;
			if (ingredientNode != null)
			{
				itemId = ingredientNode.Item.Id;
			}
			VendorNode vendorNode = node as VendorNode;
			if (vendorNode != null)
			{
				itemId = vendorNode.VendorGroup.VendorItem.ItemId;
			}
			TreeNodeBase parentNode = ((Control)node).get_Parent() as TreeNodeBase;
			if (parentNode != null)
			{
				return IsCircularReference(parentNode, itemId);
			}
			return false;
		}

		private bool IsCircularReference(TreeNodeBase node, int itemId)
		{
			ItemIngredientNode ingredientNode = node as ItemIngredientNode;
			if (ingredientNode != null && ingredientNode.Item.Id == itemId)
			{
				return true;
			}
			VendorNode vendorNode = node as VendorNode;
			if (vendorNode != null && vendorNode.VendorGroup.VendorItem.ItemId == itemId)
			{
				return true;
			}
			TreeNodeBase parentNode = ((Control)node).get_Parent() as TreeNodeBase;
			if (parentNode != null)
			{
				return IsCircularReference(parentNode, itemId);
			}
			return false;
		}

		public void BuildChildren(IItemSource itemSource, Container parent)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			TreeNodeBase node = parent as TreeNodeBase;
			if (node != null)
			{
				node.ClearChildNodes();
				if (IsCircularReference(node))
				{
					return;
				}
			}
			(parent as IIngredientContainer)?.ClearChildIngredientNodes();
			RecipeSource recipeSource = itemSource as RecipeSource;
			if (recipeSource != null && recipeSource.Recipe != null)
			{
				if (recipeSource.Recipe.RecipeSheets != null && recipeSource.Recipe.RecipeSheets.Any() && !ServiceContainer.PlayerUnlocksService.RecipeUnlocked(recipeSource.Recipe))
				{
					((Control)new LabelNode(MysticCrafting.Module.Strings.Recipe.RecipeSheetLabel, parent)
					{
						TextColor = Color.get_White()
					}).set_Width(((Control)parent).get_Width() - 25);
					_recipeSheetPresenter.BuildNode(recipeSource.Recipe, parent);
				}
				((Control)new LabelNode((recipeSource.Source == RecipeType.MysticForge) ? MysticCrafting.Module.Strings.Recipe.ForgingItemsLabel : MysticCrafting.Module.Strings.Recipe.CraftingItemsLabel, parent)
				{
					TextColor = Color.get_White()
				}).set_Width(((Control)parent).get_Width() - 25);
				BuildNodes(recipeSource.Recipe, parent);
			}
			TradingPostSource tradingPostSource = itemSource as TradingPostSource;
			if (tradingPostSource != null)
			{
				((Control)new LabelNode(MysticCrafting.Module.Strings.Recipe.BuyingItemsLabel, parent)
				{
					TextColor = Color.get_White()
				}).set_Width(((Control)parent).get_Width() - 25);
				_tradingPostPresenter.Build(parent, tradingPostSource);
			}
			VendorSource vendorSource = itemSource as VendorSource;
			if (vendorSource != null)
			{
				((Control)new LabelNode(MysticCrafting.Module.Strings.Recipe.SellerItemsLabel, parent)
				{
					TextColor = Color.get_White()
				}).set_Width(((Control)parent).get_Width() - 25);
				foreach (VendorNode vendorNode in _vendorPresenter.Build(parent, vendorSource))
				{
					((Control)new LabelNode(MysticCrafting.Module.Strings.Recipe.SellerItemCostsLabel, (Container)(object)vendorNode)
					{
						TextColor = Color.get_White()
					}).set_Width(((Control)vendorNode).get_Width() - 25);
					vendorNode.OnPanelClick += delegate
					{
						_recipeDetailsPresenter.SaveScrollDistance();
					};
					foreach (VendorSellsItemCost itemCost in vendorNode.VendorGroup.ItemCosts.Where((VendorSellsItemCost c) => c.Item != null))
					{
						BuildItemNode(itemCost.Item, (Container)(object)vendorNode, expandable: true, isPrimaryNode: false, openByDefault: false, itemCost.UnitPrice);
					}
					foreach (VendorSellsItemCost currencyCost in vendorNode.VendorGroup.ItemCosts.Where((VendorSellsItemCost c) => c.Currency != null))
					{
						_currencyNodePresenter.BuildNode((Container)(object)vendorNode, new CurrencyQuantity(currencyCost));
					}
				}
			}
			ItemContainerSource containerSource = itemSource as ItemContainerSource;
			if (containerSource == null)
			{
				return;
			}
			if (containerSource.Container != null)
			{
				ItemIngredientNode parentNode = parent as ItemIngredientNode;
				if (parentNode != null)
				{
					if (containerSource.Container.ContainedChoiceItemIds != null && containerSource.Container.ContainedChoiceItemIds.Contains(parentNode.Item.Id))
					{
						LabelNode labelNode = new LabelNode(MysticCrafting.Module.Strings.Recipe.ChoiceOfItem, parent);
						((Control)labelNode).set_Width(300);
						labelNode.TextColor = Color.get_Red();
					}
					if (containerSource.Container.ContainedChanceItemIds != null && containerSource.Container.ContainedChanceItemIds.Contains(parentNode.Item.Id))
					{
						LabelNode labelNode2 = new LabelNode(MysticCrafting.Module.Strings.Recipe.RandomItemChance, parent);
						((Control)labelNode2).set_Width(300);
						labelNode2.TextColor = Color.get_Red();
					}
				}
			}
			if (containerSource.ContainerItem != null)
			{
				BuildItemNode(containerSource.ContainerItem, parent, expandable: true);
			}
		}

		public ItemTab BuildTabs(TreeNodeBase node, Item item)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			_recipeDetailsPresenter.SaveScrollDistance();
			IList<ItemTab> tabs = _tabsPresenter.BuildTabs((Container)(object)node, item, new Point(((Control)node).get_Width() - 5, 3));
			if (!tabs.Any())
			{
				return null;
			}
			ItemTab selectedTab = _tabsPresenter.AutoActivateTab(node, tabs);
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
			Container parent = ((Control)((Control)tab).get_Parent()).get_Parent();
			(parent as ITradeableItemNode)?.ResetPrices();
			BuildChildren(tab.ItemSource, parent);
			IngredientNode node = parent as IngredientNode;
			if (node != null)
			{
				_choiceRepository.SaveChoice(node.FullPath, tab.ItemSource.UniqueId, ChoiceType.ItemSource);
				node.TreeView.ReIndex();
				UpdateTooltips(node);
				if (!node.Expanded)
				{
					node.Toggle();
				}
			}
		}

		private void UpdateTooltips(TradeableItemNode node)
		{
			node.UpdateReservedQuantityTooltip();
			VendorNode vNode = node as VendorNode;
			vNode?.PriceContainer.Update(vNode.PriceContainer.Costs);
			foreach (TradeableItemNode childNode in ((IEnumerable)((Container)node).get_Children()).OfType<TradeableItemNode>())
			{
				UpdateTooltips(childNode);
			}
		}

		private void Event_LayoutChanged(object sender, CheckChangedEvent e)
		{
			_recipeDetailsPresenter.SaveScrollDistance();
		}
	}
}
