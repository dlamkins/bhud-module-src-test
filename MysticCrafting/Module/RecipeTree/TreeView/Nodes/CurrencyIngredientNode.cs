using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Presenters;
using MysticCrafting.Module.RecipeTree.TreeView.Tooltips;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public sealed class CurrencyIngredientNode : IngredientNode
	{
		public CurrencyQuantity CurrencyQuantity { get; set; }

		public CoinsControl CoinsControl { get; set; }

		public CurrencyIngredientNode(CurrencyQuantity currencyQuantity, Container parent, int? index = null, bool loadingChildren = false)
			: base(currencyQuantity.Currency.Id, currencyQuantity.Count, parent, index, showUnitCount: true, loadingChildren)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			CurrencyQuantity = currencyQuantity;
			base.Name = (currencyQuantity.Currency.LocalizedName() ?? "").Truncate(34);
			base.NameLabelColor = ColorHelper.CurrencyName;
			base.IconTexture = ServiceContainer.TextureRepository.GetTexture(currencyQuantity.Currency.Icon);
			base.HidePriceControls = true;
			VendorNode vendorNode = parent as VendorNode;
			if (vendorNode != null)
			{
				OrderUnitCount = vendorNode.OrderUnitCount;
				VendorPriceUnitCount = vendorNode.VendorGroup.VendorItem.ItemQuantity;
			}
			UpdatePlayerUnitCount();
			base.LoadingChildren = loadingChildren;
		}

		public override void Build(Container parent)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			base.Build(parent);
			base.VendorPrice = new List<CurrencyQuantity> { CurrencyQuantity };
			if (CurrencyQuantity.Currency.Id == 1)
			{
				((Control)base.UnitCountLabel).set_Visible(false);
				((Control)base.NameLabel).set_Visible(false);
				CoinsControl obj = new CoinsControl((Container)(object)this)
				{
					UnitPrice = TotalUnitCount
				};
				((Control)obj).set_Location(new Point(70, 10));
				CoinsControl = obj;
			}
			BuildItemTooltip();
		}

		public void BuildItemTooltip()
		{
			base.PlayerCountTooltipView = new AdvancedCurrencyTooltipView(this);
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
			MenuStrip = menuStripPresenter.BuildMenuStrip(CurrencyQuantity.Currency, this);
		}

		public override bool UpdatePlayerUnitCount()
		{
			if (CurrencyQuantity == null || CurrencyQuantity.Currency.Id == 1)
			{
				return false;
			}
			int newPlayerCount = ServiceContainer.WalletService.GetQuantity(CurrencyQuantity.Currency.Id).Count;
			if (newPlayerCount == base.PlayerUnitCount)
			{
				return false;
			}
			base.PlayerUnitCount = newPlayerCount;
			return true;
		}
	}
}
