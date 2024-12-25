using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule.CustomControls
{
	public class BorderPanel : Panel
	{
		private readonly AsyncTexture2D _textureWindowCorner = AsyncTexture2D.FromAssetId(156008);

		protected Rectangle ResizeHandleBoundsRight { get; private set; } = Rectangle.get_Empty();


		protected Rectangle ResizeHandleBoundsLeft { get; private set; } = Rectangle.get_Empty();


		public override void RecalculateLayout()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			ResizeHandleBoundsRight = new Rectangle(((Control)this).get_Width() - _textureWindowCorner.get_Width() + 2, ((Control)this).get_Height() - _textureWindowCorner.get_Height() + 3, _textureWindowCorner.get_Width(), _textureWindowCorner.get_Height());
			ResizeHandleBoundsLeft = new Rectangle(-2, ((Control)this).get_Height() - _textureWindowCorner.get_Height() + 3, _textureWindowCorner.get_Width(), _textureWindowCorner.get_Height());
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			PaintCorner(spriteBatch);
		}

		private void PaintCorner(SpriteBatch spriteBatch)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureWindowCorner), ResizeHandleBoundsRight);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureWindowCorner), ResizeHandleBoundsLeft, (Rectangle?)null, Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)1);
		}

		public BorderPanel()
			: this()
		{
		}//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)

	}
}
