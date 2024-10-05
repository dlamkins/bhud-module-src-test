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
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			base.BackgroundColor = Color.get_Black() * 0.1f;
		}

		public FramedImage(int assetId)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Texture = AsyncTexture2D.FromAssetId(assetId);
			Rectangle bounds = Texture.Bounds;
			base.Size = ((Rectangle)(ref bounds)).get_Size();
		}

		public override void RecalculateLayout()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = (int)((double)base.Width * 0.15);
			int yOffset = (int)((double)base.Height * 0.15);
			Point size = (Point)(((_003F?)TextureSize) ?? base.Size);
			Point padding = default(Point);
			((Point)(ref padding))._002Ector((base.Width - size.X) / 2, (base.Height - size.Y) / 2);
			_textureBounds = new Rectangle(xOffset / 2 + padding.X, yOffset / 2 + padding.Y, size.X - xOffset, size.Y - yOffset);
			_iconFrameBounds = new Rectangle(0, yOffset, base.Width - xOffset, base.Height - yOffset);
			_rightIconFrameBounds = new Rectangle(xOffset, 0, base.Width - xOffset, base.Height - yOffset);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(this, IconFrame, _iconFrameBounds, (FrameTextureRegion == Rectangle.get_Empty()) ? IconFrame.Bounds : FrameTextureRegion, Color.get_White());
			spriteBatch.DrawOnCtrl(this, IconFrame, _rightIconFrameBounds, (FrameTextureRegion == Rectangle.get_Empty()) ? IconFrame.Bounds : FrameTextureRegion, Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)3);
			if (Texture != null)
			{
				spriteBatch.DrawOnCtrl(this, Texture, _textureBounds, (TextureRegion == Rectangle.get_Empty()) ? Texture.Bounds : TextureRegion, Color.get_White());
			}
		}
	}
}
