using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class CurrenciesTooltipView : View, ITooltipView, IView
	{
		public IList<CurrencyQuantity> Quantities { get; set; }

		private TradeableItemNode Node { get; set; }

		private IList<Control> Controls { get; set; } = new List<Control>();


		public Point IconSize { get; set; } = new Point(30, 30);


		public CurrenciesTooltipView(TradeableItemNode node)
			: this()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Node = node;
			BuildControls(node.TotalVendorPrice);
		}

		public CurrenciesTooltipView(IList<CurrencyQuantity> quantities)
			: this()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			BuildControls(quantities);
		}

		public void BuildControls(IList<CurrencyQuantity> quantities)
		{
			Quantities = quantities;
			foreach (Control control in Controls)
			{
				control.Dispose();
			}
			int paddingTop = 5;
			foreach (CurrencyQuantity quantity in quantities)
			{
				BuildControls(quantity, ref paddingTop);
			}
		}

		private void BuildControls(CurrencyQuantity quantity, ref int paddingTop)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Expected O, but got Unknown
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Expected O, but got Unknown
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Expected O, but got Unknown
			if (quantity.Currency.Id == 1)
			{
				return;
			}
			if (quantity.Currency?.Icon != null)
			{
				AsyncTexture2D texture = ServiceContainer.TextureRepository.GetTexture(quantity.Currency.Icon);
				if (texture != null)
				{
					IList<Control> controls = Controls;
					Image val = new Image(texture);
					((Control)val).set_Size(IconSize);
					((Control)val).set_Location(new Point(0, paddingTop));
					controls.Add((Control)val);
				}
			}
			CurrencyQuantity walletQuantity = ServiceContainer.WalletService.GetQuantity(quantity.Currency.Id);
			if (walletQuantity != null)
			{
				int walletCount = walletQuantity.Count;
				int reservedCount = 0;
				IngredientNode ingredientNode = Node as IngredientNode;
				if (ingredientNode != null)
				{
					reservedCount = ingredientNode.GetReservedQuantity(quantity.Currency.Id);
					walletCount = Math.Max(0, walletCount - reservedCount);
				}
				paddingTop += 5;
				Label val2 = new Label();
				val2.set_Text(walletCount.ToString("N0"));
				val2.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val2).set_Location(new Point(40, paddingTop));
				val2.set_TextColor(ColorHelper.FromItemCount(walletCount, quantity.Count));
				val2.set_StrokeText(true);
				val2.set_AutoSizeWidth(true);
				Label textLabel = val2;
				Label val3 = new Label();
				val3.set_Text($"/{quantity.Count:N0}");
				val3.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val3).set_Location(new Point(((Control)textLabel).get_Right(), paddingTop));
				val3.set_TextColor(Color.get_White());
				val3.set_StrokeText(true);
				val3.set_AutoSizeWidth(true);
				Label totalLabel = val3;
				Controls.Add((Control)(object)textLabel);
				Controls.Add((Control)(object)totalLabel);
				int paddingLeft = ((Control)totalLabel).get_Right() + 5;
				Label val4 = new Label();
				val4.set_Text(quantity.Currency.LocalizedName());
				val4.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val4).set_Location(new Point(paddingLeft, paddingTop));
				val4.set_TextColor(ColorHelper.CurrencyName);
				val4.set_StrokeText(true);
				val4.set_AutoSizeWidth(true);
				Label nameLabel = val4;
				Controls.Add((Control)(object)nameLabel);
				int moreRequired = quantity.Count - walletCount;
				if (moreRequired > 0)
				{
					string text = ((reservedCount > 0) ? (", " + string.Format(Recipe.WalletQuantityReservedShort, reservedCount)) : "");
					IList<Control> controls2 = Controls;
					Label val5 = new Label();
					val5.set_Text("(" + string.Format(Recipe.MoreRequired, moreRequired) + text + ")");
					val5.set_Font(GameService.Content.get_DefaultFont14());
					((Control)val5).set_Location(new Point(((Control)nameLabel).get_Right() + 10, paddingTop));
					val5.set_TextColor(Color.get_LightGray());
					val5.set_ShowShadow(true);
					val5.set_AutoSizeWidth(true);
					controls2.Add((Control)val5);
				}
				paddingTop += 35;
			}
		}

		protected override void Build(Container buildPanel)
		{
			foreach (Control control in Controls.ToList())
			{
				if (control != null)
				{
					control.set_Parent(buildPanel);
				}
			}
		}

		protected override void Unload()
		{
			Controls?.SafeDispose();
			Controls?.Clear();
			Quantities = null;
			((View<IPresenter>)this).Unload();
		}
	}
}
