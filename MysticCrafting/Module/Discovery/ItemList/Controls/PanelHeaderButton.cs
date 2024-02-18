using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Discovery.ItemList.Controls
{
	public class PanelHeaderButton : Container
	{
		private AsyncTexture2D DefaultTexture = ServiceContainer.TextureRepository.Textures.PanelHeaderBg;

		private AsyncTexture2D ActiveTexture = ServiceContainer.TextureRepository.Textures.PanelHeaderBgActive;

		public BitmapFont TextFont { get; set; } = GameService.Content.DefaultFont18;


		public string Text { get; set; }

		public IList<string> Breadcrumbs { get; set; }

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (base.MouseOver)
			{
				spriteBatch.DrawOnCtrl(this, ActiveTexture, new Rectangle(0, 0, base.Width, base.Height));
			}
			else
			{
				spriteBatch.DrawOnCtrl(this, DefaultTexture, new Rectangle(0, 0, base.Width, base.Height));
			}
			spriteBatch.DrawOnCtrl(this, ServiceContainer.TextureRepository.Textures.BackButton, new Rectangle(10, 0, 35, 35));
			if (!string.IsNullOrWhiteSpace(Text))
			{
				spriteBatch.DrawStringOnCtrl(this, Text, TextFont, new Rectangle(60, 0, 150, 35), Color.White, wrap: false, stroke: true);
			}
			int xPosition = 60;
			foreach (string breadcrumb in Breadcrumbs)
			{
				spriteBatch.DrawStringOnCtrl(this, breadcrumb, TextFont, new Rectangle(xPosition, 0, 150, 35), Color.White, wrap: false, stroke: true);
				int textWidth = (int)Math.Ceiling(TextFont.MeasureString(breadcrumb).Width);
				xPosition += textWidth + 10;
				if (Breadcrumbs.IndexOf(breadcrumb) + 1 != Breadcrumbs.Count())
				{
					spriteBatch.DrawOnCtrl(this, ServiceContainer.TextureRepository.Textures.BreadcrumbArrow, new Rectangle(xPosition, 19, 25, 25), null, Color.White, (float)Math.PI, new Vector2(16f, 16f));
					xPosition += 15;
				}
			}
			base.PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}
