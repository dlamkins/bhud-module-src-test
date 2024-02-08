using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Controls
{
	public class CurrencyControl : Control
	{
		private MysticCurrencyQuantity _quantity;

		public MysticCurrencyQuantity Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				_quantity = value;
				WalletQuantity = ServiceContainer.WalletService.GetQuantity(_quantity.Currency.GameId);
				base.Width = CalculateWidth();
			}
		}

		public MysticCurrencyQuantity WalletQuantity { get; set; }

		public Point IconSize { get; set; } = new Point(20, 20);


		public BitmapFont TextFont { get; set; } = GameService.Content.DefaultFont16;


		public AsyncTexture2D Icon { get; set; }

		public CurrencyControl(Container parent)
		{
			base.Parent = parent;
		}

		private string AmountToText(int amount)
		{
			return amount.ToString("N0");
		}

		public int CalculateWidth()
		{
			string text = AmountToText(Quantity.Count);
			return (int)Math.Ceiling(TextFont.MeasureString(text).Width) + IconSize.X;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Quantity != null && Quantity.Count != 0)
			{
				if (Icon == null)
				{
					Icon = ServiceContainer.TextureRepository.GetTexture(Quantity.Currency.Icon);
				}
				PaintCurrency(Quantity.Count, Icon, spriteBatch, bounds);
				int width = CalculateWidth();
				if (base.Size.X != width)
				{
					base.Size = new Point(width, base.Size.Y);
					RecalculateLayout();
				}
			}
		}

		private int PaintCurrency(int amount, AsyncTexture2D texture, SpriteBatch spriteBatch, Rectangle position)
		{
			if (amount == 0)
			{
				return 0;
			}
			string text = AmountToText(amount);
			int textWidth = (int)Math.Ceiling(TextFont.MeasureString(text).Width);
			Color color = Color.White;
			if (WalletQuantity != null)
			{
				color = ColorHelper.FromItemCount(WalletQuantity.Count, Quantity.Count);
			}
			spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont16, new Rectangle(position.X, position.Y, textWidth, 20).OffsetBy(1, 1), Color.Black);
			spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont16, new Rectangle(position.X, position.Y, textWidth, 20), color);
			spriteBatch.DrawOnCtrl(this, texture, new Rectangle(position.X + textWidth, position.Y, IconSize.X, IconSize.Y));
			return textWidth + IconSize.X;
		}
	}
}
