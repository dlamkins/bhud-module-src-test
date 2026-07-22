using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class Separator : Control
	{
		private readonly DetailedTexture _headerSeparator = new DetailedTexture(155900);

		public Color Color { get; set; } = Color.White;


		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			for (int i = 0; i < (int)Math.Ceiling((double)base.Width / (double)_headerSeparator.Size.X); i++)
			{
				SpriteBatchExtensions.DrawOnCtrl(destinationRectangle: new Rectangle(i * _headerSeparator.Size.X, -_headerSeparator.Size.Y / 2, _headerSeparator.Size.X, _headerSeparator.Size.Y), spriteBatch: spriteBatch, ctrl: this, texture: _headerSeparator.Texture, sourceRectangle: _headerSeparator.TextureRegion, color: Color);
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
		}
	}
}
