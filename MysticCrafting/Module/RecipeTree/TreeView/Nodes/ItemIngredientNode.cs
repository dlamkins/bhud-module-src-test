using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Presenters;
using MysticCrafting.Module.RecipeTree.TreeView.Tooltips;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public class ItemIngredientNode : IngredientNode
	{
		public Item Item { get; set; }

		public ItemIngredientNode(int id, int quantity, Container parent)
			: base(id, quantity, parent)
		{
		}

		public ItemIngredientNode(Item item, int quantity, Container parent, int? index = null, bool showUnitCount = true, bool loadingChildren = false)
			: base(item.Id, quantity, parent, index, showUnitCount, loadingChildren)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			Item = item;
			base.Name = (Item.LocalizedName() ?? "").Truncate(34);
			base.NameLabelColor = ColorHelper.FromRarity(Item.Rarity.ToString());
			base.IconTexture = ServiceContainer.TextureRepository.GetTexture(item.Icon);
			UpdatePlayerUnitCount();
			base.LoadingChildren = loadingChildren;
			if (parent is TreeView)
			{
				BackgroundOpaqueColor = ColorHelper.FromRarity(item.Rarity.ToString()) * 0.2f;
				FrameColor = ColorHelper.FromRarity(item.Rarity.ToString()) * 0.8f;
			}
		}

		public ItemIngredientNode(Ingredient ingredient, Container parent, int? index = null, bool showUnitCount = true, bool loadingChildren = false)
			: this(ingredient.Item, ingredient.Quantity, parent, index, showUnitCount, loadingChildren)
		{
		}

		public override void Build(Container parent)
		{
			base.Build(parent);
			if (Item.Rarity == ItemRarity.Legendary)
			{
				base.MaxEditValue = GetLegendaryMaxCount();
			}
			BuildItemTooltip();
		}

		private int GetLegendaryMaxCount()
		{
			int maxItemCount = Item.GetMaxCount().GetValueOrDefault(int.MaxValue);
			if (Item.Rarity == ItemRarity.Legendary)
			{
				int unlockedCount = ServiceContainer.PlayerUnlocksService.LegendaryUnlockedCount(Item.Id);
				return maxItemCount - unlockedCount;
			}
			return maxItemCount;
		}

		public void BuildItemTooltip()
		{
			base.PlayerCountTooltipView = new ItemNodeTooltipView(this);
			if (base.PlayerCountTooltipView != null)
			{
				base.PlayerCountTooltip = (Tooltip)(object)new DisposableTooltip((ITooltipView)base.PlayerCountTooltipView);
				if (base.NameLabel != null)
				{
					((Control)base.NameLabel).set_Tooltip(base.PlayerCountTooltip);
				}
				if (base.Icon != null)
				{
					((Control)base.Icon).set_Tooltip(base.PlayerCountTooltip);
				}
				if (base.IconChain != null)
				{
					((Control)base.IconChain).set_Tooltip(base.PlayerCountTooltip);
				}
				if (base.UnitCountLabel != null)
				{
					((Control)base.UnitCountLabel).set_Tooltip(base.PlayerCountTooltip);
				}
			}
		}

		protected override void BuildMenuStrip()
		{
			ContextMenuPresenter menuStripPresenter = new ContextMenuPresenter();
			MenuStrip = menuStripPresenter.BuildMenuStrip(Item, this);
		}

		public override bool UpdatePlayerUnitCount()
		{
			if (Item == null)
			{
				return false;
			}
			if (base.IsTopIngredient)
			{
				if (base.PlayerUnitCount == 0)
				{
					return false;
				}
				base.PlayerUnitCount = 0;
				return true;
			}
			int newPlayerCount = ServiceContainer.PlayerItemService.GetItemCount(Item.Id);
			if (newPlayerCount == base.PlayerUnitCount)
			{
				return false;
			}
			base.PlayerUnitCount = newPlayerCount;
			return true;
		}
	}
}
