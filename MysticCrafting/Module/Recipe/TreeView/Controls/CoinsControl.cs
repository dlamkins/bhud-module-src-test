using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Controls
{
	public class CoinsControl : Control
	{
		private int _unitPrice;

		public int UnitPrice
		{
			get
			{
				return _unitPrice;
			}
			set
			{
				_unitPrice = value;
				base.Width = CalculateWidth();
			}
		}

		public string SuffixText { get; set; }

		public int Gold => UnitPrice / 10000;

		public int Silver => UnitPrice % 10000 / 100;

		public int Copper => UnitPrice % 100;

		public Point IconSize { get; set; } = new Point(20, 20);


		public BitmapFont TextFont { get; set; } = GameService.Content.DefaultFont16;


		public CoinsControl(Container parent)
		{
			base.Parent = parent;
			new Label().ShowShadow = true;
		}

		public int CalculateWidth()
		{
			int width = 0;
			if (Gold != 0)
			{
				width += CalculateWidth(Gold);
			}
			if (Gold != 0 || Silver != 0)
			{
				width += CalculateWidth(Silver);
			}
			if (Gold != 0 || Silver != 0 || Copper != 0)
			{
				width += CalculateWidth(Copper);
			}
			return width;
		}

		public int CalculateWidth(int coin)
		{
			string text = coin.ToString("N0");
			return (int)Math.Ceiling(TextFont.MeasureString(text).Width) + IconSize.X + 5;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (UnitPrice != 0)
			{
				Rectangle position = new Rectangle(bounds.X, bounds.Y, base.Width, base.Height);
				if (Gold != 0)
				{
					position.X += PaintCoin(Gold, ServiceContainer.TextureRepository.Textures.CoinGold, new Color(210, 177, 66), spriteBatch, position);
				}
				if (Gold != 0 || Silver != 0)
				{
					position.X += PaintCoin(Silver, ServiceContainer.TextureRepository.Textures.CoinSilver, new Color(184, 184, 184), spriteBatch, position);
				}
				if (Gold != 0 || Silver != 0 || Copper != 0)
				{
					position.X += PaintCoin(Copper, ServiceContainer.TextureRepository.Textures.CoinCopper, new Color(187, 102, 34), spriteBatch, position);
				}
				base.Size = new Point(position.X, base.Size.Y);
				spriteBatch.DrawStringOnCtrl(this, SuffixText, GameService.Content.DefaultFont16, new Rectangle(position.X, position.Y, 50, 20), Color.White * 0.7f);
			}
		}

		private int PaintCoin(int amount, AsyncTexture2D texture, Color color, SpriteBatch spriteBatch, Rectangle position)
		{
			string text = amount.ToString("N0");
			int textWidth = (int)Math.Ceiling(TextFont.MeasureString(text).Width);
			spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont16, new Rectangle(position.X, position.Y, textWidth, 20).OffsetBy(1, 1), Color.Black);
			spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont16, new Rectangle(position.X, position.Y, textWidth, 20), color);
			spriteBatch.DrawOnCtrl(this, texture, new Rectangle(position.X + textWidth, position.Y + 3, IconSize.X, IconSize.Y));
			return textWidth + IconSize.X + 2;
		}
	}
}
