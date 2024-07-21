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
	public class CurrencyTooltipView : View, ICountTooltipView, ITooltipView, IView
	{
		private TradeableItemNode Node { get; set; }

		private Label UnitCountLabel { get; set; }

		private Label TotalUnitCountLabel { get; set; }

		private CurrencyQuantity Quantity { get; set; }

		private IList<Control> _controls { get; set; } = new List<Control>();


		public Point IconSize { get; set; } = new Point(30, 30);


		public int RequiredQuantity { get; set; }

		public CurrencyTooltipView(CurrencyQuantity quantity, TradeableItemNode node)
			: this()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Quantity = quantity;
			Node = node;
			BuildControls();
		}

		private void BuildControls()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Expected O, but got Unknown
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Expected O, but got Unknown
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Expected O, but got Unknown
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Expected O, but got Unknown
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Expected O, but got Unknown
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Expected O, but got Unknown
			if (Quantity.Currency?.Icon != null)
			{
				AsyncTexture2D texture = ServiceContainer.TextureRepository.GetTexture(Quantity.Currency.Icon);
				if (texture != null)
				{
					IList<Control> controls = _controls;
					Image val = new Image(texture);
					((Control)val).set_Size(IconSize);
					((Control)val).set_Location(new Point(0, 0));
					controls.Add((Control)val);
				}
			}
			IList<Control> controls2 = _controls;
			Label val2 = new Label();
			val2.set_Text(Quantity.Currency?.Name);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val2).set_Location(new Point(40, 5));
			val2.set_TextColor(ColorHelper.CurrencyName);
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			controls2.Add((Control)val2);
			Label val3 = new Label();
			val3.set_Text(Quantity.Currency?.LocalizedDescription());
			val3.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val3).set_Location(new Point(0, 35));
			val3.set_TextColor(Color.get_White());
			((Control)val3).set_Width(400);
			val3.set_AutoSizeHeight(true);
			val3.set_WrapText(true);
			val3.set_StrokeText(true);
			Label descriptionLabel = val3;
			_controls.Add((Control)(object)descriptionLabel);
			if (Quantity.Currency.Id == 1)
			{
				return;
			}
			CurrencyQuantity walletQuantity = ServiceContainer.WalletService.GetQuantity(Quantity.Currency.Id);
			if (walletQuantity != null)
			{
				int walletCount = walletQuantity.Count;
				int reservedCount = Node.GetReservedQuantity(Quantity.Currency.Id);
				walletCount = Math.Max(0, walletCount - reservedCount);
				int yPosition = 40 + ((Control)descriptionLabel).get_Height();
				Label val4 = new Label();
				val4.set_Text($"{walletCount:N0}");
				val4.set_Font(GameService.Content.get_DefaultFont18());
				((Control)val4).set_Location(new Point(0, yPosition));
				val4.set_TextColor(ColorHelper.FromItemCount(walletCount, Quantity.Count));
				val4.set_StrokeText(true);
				val4.set_AutoSizeWidth(true);
				UnitCountLabel = val4;
				Label val5 = new Label();
				val5.set_Text($"/ {Quantity.Count:N0}");
				val5.set_Font(GameService.Content.get_DefaultFont18());
				((Control)val5).set_Location(new Point(((Control)UnitCountLabel).get_Width(), yPosition));
				val5.set_TextColor(Color.get_White());
				val5.set_StrokeText(true);
				val5.set_AutoSizeWidth(true);
				TotalUnitCountLabel = val5;
				yPosition += ((Control)UnitCountLabel).get_Height() + 5;
				_controls.Add((Control)(object)UnitCountLabel);
				_controls.Add((Control)(object)TotalUnitCountLabel);
				int moreRequired = Quantity.Count - walletQuantity.Count;
				if (moreRequired > 0)
				{
					IList<Control> controls3 = _controls;
					Label val6 = new Label();
					val6.set_Text(string.Format(Recipe.MoreRequired, moreRequired));
					val6.set_Font(GameService.Content.get_DefaultFont16());
					((Control)val6).set_Location(new Point(0, yPosition));
					val6.set_TextColor(Color.get_LightGray());
					val6.set_ShowShadow(true);
					val6.set_AutoSizeWidth(true);
					controls3.Add((Control)val6);
					yPosition += 25;
				}
				if (reservedCount > 0)
				{
					IList<Control> controls4 = _controls;
					Label val7 = new Label();
					val7.set_Text(string.Format(Recipe.WalletQuantityReserved, reservedCount));
					val7.set_Font(GameService.Content.get_DefaultFont16());
					((Control)val7).set_Location(new Point(0, yPosition));
					val7.set_TextColor(Color.get_LightGray());
					val7.set_ShowShadow(true);
					val7.set_AutoSizeWidth(true);
					controls4.Add((Control)val7);
					yPosition += 25;
				}
				yPosition += 5;
				IList<Control> controls5 = _controls;
				Label val8 = new Label();
				val8.set_Text(Recipe.WalletLabel);
				val8.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val8).set_Location(new Point(0, yPosition));
				val8.set_TextColor(Color.get_LightGray());
				val8.set_ShowShadow(true);
				val8.set_AutoSizeWidth(true);
				controls5.Add((Control)val8);
			}
		}

		public void UpdateReservedQuantity(int nodeIndex)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			CurrencyQuantity walletQuantity = ServiceContainer.WalletService.GetQuantity(Quantity.Currency.Id);
			if (walletQuantity != null)
			{
				int walletCount = walletQuantity.Count;
				int reservedCount = Node.GetReservedQuantity(Quantity.Currency.Id, nodeIndex);
				walletCount = Math.Max(0, walletCount - reservedCount);
				UnitCountLabel.set_Text(walletCount.ToString("N0"));
				UnitCountLabel.set_TextColor(ColorHelper.FromItemCount(walletCount, Quantity.Count));
				((Control)TotalUnitCountLabel).set_Left(((Control)UnitCountLabel).get_Width());
			}
		}

		protected override void Build(Container buildPanel)
		{
			foreach (Control control in _controls.ToList())
			{
				if (control != null)
				{
					control.set_Parent(buildPanel);
				}
			}
		}

		protected override void Unload()
		{
			foreach (Control control in _controls)
			{
				control.Dispose();
			}
			_controls?.Clear();
			Quantity = null;
			((View<IPresenter>)this).Unload();
		}

		public void UpdateLinkedNodes()
		{
		}
	}
}
