using System;
using Atzie.MysticCrafting.Models.Currencies;
using Atzie.MysticCrafting.Models.Vendor;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Tooltips;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
{
	public class VendorCostControl : Control
	{
		private VendorSellsItemCost _cost;

		private string Text;

		private int TextWidth;

		private Color TextColor;

		public VendorSellsItemCost Cost
		{
			get
			{
				return _cost;
			}
			set
			{
				_cost = value;
				Update();
			}
		}

		public Point IconSize { get; set; } = new Point(20, 20);


		public BitmapFont TextFont { get; set; } = GameService.Content.get_DefaultFont16();


		public int PlayerCount { get; set; }

		public AsyncTexture2D Icon { get; set; }

		public VendorCostControl(Container parent)
			: this()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
		}

		public void Update()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			Text = AmountToText(Cost.UnitPrice);
			TextColor = ColorHelper.FromItemCount(PlayerCount, Cost.UnitPrice);
			Size2 textSize = TextFont.MeasureString(Text);
			TextWidth = (int)Math.Ceiling(textSize.Width);
			if (_cost.Currency != null)
			{
				PlayerCount = (int)(ServiceContainer.WalletService.GetQuantity(_cost.CurrencyId)?.Count ?? 0);
				Icon = ServiceContainer.TextureRepository.GetTexture(_cost.Currency.Icon);
			}
			else if (_cost.Item != null)
			{
				PlayerCount = ServiceContainer.PlayerItemService.GetItemCount(_cost.ItemId);
				Icon = ServiceContainer.TextureRepository.GetTexture(_cost.Item.Icon);
			}
			UpdateTooltip();
			((Control)this).set_Width(CalculateWidth());
		}

		public void UpdateTooltip()
		{
			if (((Control)this).get_Tooltip() == null)
			{
				if (Cost.Currency != null)
				{
					((Control)this).set_Tooltip((Tooltip)(object)new DisposableTooltip((ITooltipView)(object)new VendorCurrencyTooltipView(new CurrencyQuantity(Cost))));
				}
				else if (Cost.Item != null)
				{
					((Control)this).set_Tooltip((Tooltip)(object)new DisposableTooltip((ITooltipView)(object)new ItemTooltipView(Cost.Item, Cost.UnitPrice)));
				}
			}
		}

		private string AmountToText(int amount)
		{
			return amount.ToString("N0");
		}

		public int CalculateWidth()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			string text = AmountToText(Cost.UnitPrice);
			int width = (int)Math.Ceiling(TextFont.MeasureString(text).Width) + IconSize.X + 3;
			if (((Control)this).get_Size().X != width)
			{
				((Control)this).set_Size(new Point(width, ((Control)this).get_Size().Y));
				((Control)this).RecalculateLayout();
			}
			return width;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			if (Cost != null || Cost.UnitPrice != 0)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Text, GameService.Content.get_DefaultFont16(), RectangleExtension.OffsetBy(new Rectangle(bounds.X, bounds.Y, TextWidth, 20), 1, 1), Color.get_Black(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Text, GameService.Content.get_DefaultFont16(), new Rectangle(bounds.X, bounds.Y, TextWidth, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				if (Icon != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(Icon), new Rectangle(bounds.X + TextWidth, bounds.Y, IconSize.X, IconSize.Y));
				}
			}
		}

		protected override void DisposeControl()
		{
			Tooltip tooltip = ((Control)this).get_Tooltip();
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			((Control)this).DisposeControl();
		}
	}
}
