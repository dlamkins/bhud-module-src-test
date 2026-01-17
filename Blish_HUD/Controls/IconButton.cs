using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Controls
{
	public class IconButton : Control
	{
		private readonly Texture2D _iconTexture;

		private Rectangle _destIconRect;

		private Rectangle _destIconHoveredRect;

		private Rectangle _sourceIconRect;

		private Vector2 _originPos;

		public float IconRotation;

		private readonly int _sourceSize;

		public IconButton(Texture2D iconTexture, int buttonSize)
			: this(iconTexture, buttonSize, 0)
		{
		}

		public IconButton(Texture2D iconTexture, int buttonSize, int iconPadding)
			: this(iconTexture, buttonSize, iconPadding, 1.1f)
		{
		}

		public IconButton(Texture2D iconTexture, int buttonSize, float hoverScale)
			: this(iconTexture, buttonSize, 0, hoverScale)
		{
		}

		public IconButton(Texture2D iconTexture, int buttonSize, int iconPadding, float hoverScale)
			: this()
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			_iconTexture = iconTexture;
			int num = buttonSize - 2 * iconPadding;
			int num2 = buttonSize - 2 * iconPadding;
			int num3 = (int)((float)num * hoverScale);
			int num4 = (int)((float)num2 * hoverScale);
			int num5 = iconPadding - (num3 - num) / 2;
			int num6 = iconPadding - (num4 - num2) / 2;
			_sourceSize = MathHelper.Max(_iconTexture.get_Width(), _iconTexture.get_Height());
			_destIconRect = new Rectangle(iconPadding + num / 2, iconPadding + num2 / 2, num, num2);
			_destIconHoveredRect = new Rectangle(num5 + num3 / 2, num6 + num4 / 2, num3, num4);
			_sourceIconRect = new Rectangle(0, 0, _sourceSize, _sourceSize);
			_originPos = new Vector2((float)(_iconTexture.get_Width() / 2), (float)(_iconTexture.get_Height() / 2));
			((Control)this).set_Size(new Point(buttonSize, buttonSize));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _iconTexture, ((Control)this).get_MouseOver() ? _destIconHoveredRect : _destIconRect, (Rectangle?)_sourceIconRect, Color.get_White(), IconRotation, _originPos, (SpriteEffects)0);
		}
	}
}
