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

		public Color Color { get; set; } = Color.get_White();


		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			Rectangle r = default(Rectangle);
			for (int i = 0; i < (int)Math.Ceiling((double)base.Width / (double)_headerSeparator.Size.X); i++)
			{
				((Rectangle)(ref r))._002Ector(i * _headerSeparator.Size.X, -_headerSeparator.Size.Y / 2, _headerSeparator.Size.X, _headerSeparator.Size.Y);
				spriteBatch.DrawOnCtrl(this, _headerSeparator.Texture, r, _headerSeparator.TextureRegion, Color);
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
		}
	}
}
