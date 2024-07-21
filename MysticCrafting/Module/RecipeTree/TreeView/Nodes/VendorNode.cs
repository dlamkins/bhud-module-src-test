using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Vendor;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Presenters;
using MysticCrafting.Module.RecipeTree.TreeView.Tooltips;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public class VendorNode : SelectableTradeableItemNode
	{
		private Image Icon;

		private Label NameLabel;

		public VendorSellsItemGroup VendorGroup { get; }

		public VendorPriceContainer PriceContainer { get; set; }

		private Label QuantityLabel { get; set; }

		public override string PathName => VendorGroup.VendorSellsItems.FirstOrDefault().FullName;

		public IEnumerable<string> AllVendorPathNames => VendorGroup.VendorSellsItems.Select((VendorSellsItem v) => v.FullName);

		public VendorNode(VendorSellsItemGroup vendorGroup, Container parent)
			: base(parent)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			((Control)this).set_EffectBehind((ControlEffect)new ScrollingHighlightEffect((Control)(object)this));
			VendorGroup = vendorGroup;
			((Control)this).set_Parent(parent);
			VendorPriceUnitCount = VendorGroup.VendorItem.ItemQuantity;
			Active = false;
			IngredientNode node = parent as IngredientNode;
			if (node != null)
			{
				base.TreeView = node.TreeView;
				OrderUnitCount = node.OrderUnitCount;
			}
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				if (PriceContainer != null)
				{
					((Control)PriceContainer).set_Location(new Point(((Control)this).get_Width() - ((Control)PriceContainer).get_Width() - 10, 10));
				}
			});
			Build();
		}

		public override void Build()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Expected O, but got Unknown
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D iconTexture = ServiceContainer.TextureRepository.GetVendorIconTexture(VendorGroup.VendorItem.Vendor.IconFile);
			if (iconTexture != null)
			{
				Image val = new Image(iconTexture);
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Size(new Point(30, 30));
				((Control)val).set_Location(new Point(55, 5));
				Icon = val;
			}
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(GetVendorTitle(25));
			val2.set_TextColor(ColorHelper.VendorName);
			((Control)val2).set_Location(new Point(90, 10));
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			NameLabel = val2;
			DisposableTooltip tooltip = new DisposableTooltip((ITooltipView)(object)new VendorGroupTooltipView(VendorGroup));
			((Control)Icon).set_Tooltip((Tooltip)(object)tooltip);
			((Control)NameLabel).set_Tooltip((Tooltip)(object)tooltip);
			PriceContainer = new VendorPriceContainer((Container)(object)this)
			{
				Costs = VendorGroup.ItemCosts
			};
			string quantityText = "";
			if (VendorGroup.VendorItem.ItemQuantity == 1)
			{
				quantityText = Recipe.PriceEach;
			}
			else if (VendorGroup.VendorItem.ItemQuantity > 1)
			{
				quantityText = string.Format(Recipe.PricePerNumber, VendorGroup.VendorItem.ItemQuantity);
			}
			if (!string.IsNullOrEmpty(quantityText))
			{
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)this);
				val3.set_Text(quantityText);
				((Control)val3).set_Location(new Point(((Control)PriceContainer).get_Right() + 25, 10));
				val3.set_Font(GameService.Content.get_DefaultFont16());
				val3.set_AutoSizeWidth(true);
				val3.set_TextColor(Color.get_LightYellow());
				QuantityLabel = val3;
			}
			if (!string.IsNullOrEmpty(VendorGroup.VendorItem.Requirement))
			{
				Image val4 = new Image();
				((Control)val4).set_Parent((Container)(object)this);
				val4.set_Texture(ServiceContainer.TextureRepository.Textures.ExclamationMark);
				((Control)val4).set_BasicTooltipText(Recipe.Requirement + ": " + VendorGroup.VendorItem.Requirement);
				((Control)val4).set_Size(new Point(30, 30));
				((Control)val4).set_Location(new Point(((Control)NameLabel).get_Right() + 3, 5));
			}
			BuildMenuStrip();
			base.Build();
		}

		private void BuildMenuStrip()
		{
			ContextMenuPresenter menuStripPresenter = new ContextMenuPresenter();
			MenuStrip = menuStripPresenter.BuildMenuStrip(VendorGroup);
		}

		private string GetVendorTitle(int truncate = 0)
		{
			string firstVendorName = VendorGroup.VendorSellsItems.FirstOrDefault()?.Vendor.Name;
			int vendorCount = VendorGroup.VendorSellsItems.Count();
			if (vendorCount == 1)
			{
				return firstVendorName;
			}
			if (truncate != 0)
			{
				firstVendorName = firstVendorName.Truncate(truncate);
			}
			return $"{firstVendorName} (+{vendorCount - 1})";
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			base.OnChildAdded(e);
			IngredientNode node = e.get_ChangedChild() as IngredientNode;
			if (node != null)
			{
				node.Active = Active;
				if (base.TreeView.IngredientNodes != null && node.Active && node.Id != 1)
				{
					base.TreeView.IngredientNodes.Add(node);
					node.UpdateRelatedNodes();
				}
			}
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			IngredientNode node = e.get_ChangedChild() as IngredientNode;
			if (node != null)
			{
				base.TreeView.RemoveNode(node);
			}
			((Container)this).OnChildRemoved(e);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnResized(e);
			if (PriceContainer != null)
			{
				((Control)PriceContainer).set_Location(new Point(((Control)this).get_Width() - ((Control)PriceContainer).get_Width() - ((Control)QuantityLabel).get_Width() - 15, 10));
				((Control)QuantityLabel).set_Location(new Point(((Control)PriceContainer).get_Right() + 8, 8));
			}
		}
	}
}
