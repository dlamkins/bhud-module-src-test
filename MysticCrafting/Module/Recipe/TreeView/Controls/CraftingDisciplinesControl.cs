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
	public class CraftingDisciplinesControl : Control
	{
		public int DisciplineCount { get; set; }

		public Point IconSize { get; set; } = new Point(20, 20);


		public BitmapFont TextFont { get; set; } = GameService.Content.DefaultFont16;


		public AsyncTexture2D Icon { get; set; } = ServiceContainer.TextureRepository.Textures.CraftingIcon;


		public CraftingDisciplinesControl(Container parent)
		{
			base.Parent = parent;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (DisciplineCount != 0)
			{
				string text = "400";
				int textWidth = (int)Math.Ceiling(TextFont.MeasureString(text).Width);
				spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont16, new Rectangle(0, 0, 20, 20).OffsetBy(1, 1), Color.Black);
				spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont16, new Rectangle(0, 0, 20, 20), Color.LightYellow);
				spriteBatch.DrawOnCtrl(this, Icon, new Rectangle(textWidth, 0, IconSize.X, IconSize.Y), Color.LightYellow);
			}
		}
	}
}
