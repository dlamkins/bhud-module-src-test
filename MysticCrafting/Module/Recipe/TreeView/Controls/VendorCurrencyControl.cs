using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Controls
{
	public class VendorCurrencyControl : Control
	{
		public IList<MysticCurrencyQuantity> Price { get; set; }

		public Point IconSize { get; set; } = new Point(20, 20);


		public BitmapFont TextFont { get; set; } = GameService.Content.DefaultFont16;


		public AsyncTexture2D Icon { get; set; } = ServiceContainer.TextureRepository.Textures.VendorIcon;


		public VendorCurrencyControl(Container parent)
		{
			base.Parent = parent;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Price != null && Price.Any())
			{
				string text = $"{Price.Count}";
				int textWidth = (int)Math.Ceiling(TextFont.MeasureString(text).Width);
				spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont16, new Rectangle(0, 0, 20, 20).OffsetBy(1, 1), Color.Black);
				spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont16, new Rectangle(0, 0, 20, 20), Color.LightYellow);
				spriteBatch.DrawOnCtrl(this, Icon, new Rectangle(textWidth, 0, IconSize.X, IconSize.Y), Color.LightYellow);
			}
		}

		private void DrawOutline(SpriteBatch spriteBatch)
		{
			Color lineColor = Color.Yellow * 0.5f;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, 0, base.Width, 1), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, base.Height - 2, base.Width, 1), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, 0, 1, base.Height - 1), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(base.Width - 1, 0, 1, base.Height - 1), lineColor);
		}
	}
}
