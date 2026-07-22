using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class FramedMaskedRegion : MaskedRegion
	{
		public Rectangle MaskedRegion;

		public Color BorderColor { get; set; } = ContentService.Colors.ColonialWhite;


		public RectangleDimensions BorderWidth { get; set; } = new RectangleDimensions(2);


		public FramedMaskedRegion()
		{
			base.Parent = Control.Graphics.SpriteScreen;
			ZIndex = int.MaxValue;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			MaskedRegion = new Rectangle(base.Location.X + BorderWidth.Left, base.Location.Y + BorderWidth.Top, base.Size.X - BorderWidth.Horizontal, base.Size.Y - BorderWidth.Vertical);
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			base.OnMoved(e);
			RecalculateLayout();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.Paint(spriteBatch, MaskedRegion);
			Color? borderColor = BorderColor;
			if (borderColor.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, bounds.Width, BorderWidth.Top), Rectangle.Empty, borderColor.Value * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, bounds.Width, BorderWidth.Top / 2), Rectangle.Empty, borderColor.Value * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Bottom - 2, bounds.Width, BorderWidth.Bottom), Rectangle.Empty, borderColor.Value * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Bottom - 1, bounds.Width, BorderWidth.Bottom / 2), Rectangle.Empty, borderColor.Value * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, BorderWidth.Left, bounds.Height), Rectangle.Empty, borderColor.Value * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, BorderWidth.Left / 2, bounds.Height), Rectangle.Empty, borderColor.Value * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Right - 2, bounds.Top, BorderWidth.Right, bounds.Height), Rectangle.Empty, borderColor.Value * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Right - 1, bounds.Top, BorderWidth.Right / 2, bounds.Height), Rectangle.Empty, borderColor.Value * 0.6f);
			}
		}
	}
}
