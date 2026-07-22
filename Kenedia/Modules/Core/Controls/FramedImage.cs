using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class FramedImage : Control
	{
		private Rectangle _textureBounds;

		private Rectangle _iconFrameBounds;

		private Rectangle _rightIconFrameBounds;

		public AsyncTexture2D Texture { get; set; }

		public Rectangle TextureRegion { get; set; }

		public AsyncTexture2D IconFrame { get; set; } = AsyncTexture2D.FromAssetId(1414041);


		public Rectangle FrameTextureRegion { get; set; }

		public Point? TextureSize { get; set; }

		public FramedImage()
		{
			base.BackgroundColor = Color.Black * 0.1f;
		}

		public FramedImage(int assetId)
			: this()
		{
			Texture = AsyncTexture2D.FromAssetId(assetId);
			base.Size = Texture.Bounds.Size;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int xOffset = (int)((double)base.Width * 0.15);
			int yOffset = (int)((double)base.Height * 0.15);
			Point size = TextureSize ?? base.Size;
			Point padding = new Point((base.Width - size.X) / 2, (base.Height - size.Y) / 2);
			_textureBounds = new Rectangle(xOffset / 2 + padding.X, yOffset / 2 + padding.Y, size.X - xOffset, size.Y - yOffset);
			_iconFrameBounds = new Rectangle(0, yOffset, base.Width - xOffset, base.Height - yOffset);
			_rightIconFrameBounds = new Rectangle(xOffset, 0, base.Width - xOffset, base.Height - yOffset);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			spriteBatch.DrawOnCtrl(this, IconFrame, _iconFrameBounds, (FrameTextureRegion == Rectangle.Empty) ? IconFrame.Bounds : FrameTextureRegion, Color.White);
			spriteBatch.DrawOnCtrl(this, IconFrame, _rightIconFrameBounds, (FrameTextureRegion == Rectangle.Empty) ? IconFrame.Bounds : FrameTextureRegion, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
			if (Texture != null)
			{
				spriteBatch.DrawOnCtrl(this, Texture, _textureBounds, (TextureRegion == Rectangle.Empty) ? Texture.Bounds : TextureRegion, Color.White);
			}
		}
	}
}
