using System;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
{
	public class CurrencyControl : Control
	{
		private CurrencyQuantity _quantity;

		public CurrencyQuantity Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				_quantity = value;
				WalletQuantity = ServiceContainer.WalletService.GetQuantity(_quantity.Currency.Id);
				Icon = ServiceContainer.TextureRepository.GetTexture(_quantity.Currency.Icon);
				((Control)this).set_Width(CalculateWidth());
			}
		}

		public CurrencyQuantity WalletQuantity { get; set; }

		public Point IconSize { get; set; } = new Point(20, 20);


		public BitmapFont TextFont { get; set; } = GameService.Content.get_DefaultFont16();


		public AsyncTexture2D Icon { get; set; }

		public CurrencyControl(Container parent)
			: this()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
		}

		private string AmountToText(long amount)
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
			string text = AmountToText(Quantity.Count);
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
			if (Quantity != null && Quantity.Count != 0L)
			{
				PaintCurrency(Quantity.Count, Icon, spriteBatch, bounds);
			}
		}

		private int PaintCurrency(long amount, AsyncTexture2D texture, SpriteBatch spriteBatch, Rectangle position)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			if (amount == 0L)
			{
				return 0;
			}
			string text = AmountToText(amount);
			int textWidth = (int)Math.Ceiling(TextFont.MeasureString(text).Width);
			Color color = Color.get_LightYellow();
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, GameService.Content.get_DefaultFont16(), RectangleExtension.OffsetBy(new Rectangle(position.X, position.Y, textWidth, 20), 1, 1), Color.get_Black(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, GameService.Content.get_DefaultFont16(), new Rectangle(position.X, position.Y, textWidth, 20), color, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			if (texture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(texture), new Rectangle(position.X + textWidth, position.Y, IconSize.X, IconSize.Y));
			}
			return textWidth + IconSize.X;
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
