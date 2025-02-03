using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class ImageTextureToggle : Control
	{
		private bool _active;

		public AsyncTexture2D ActiveTexture { get; set; }

		public AsyncTexture2D InactiveTexture { get; set; }

		public string ActiveText { get; set; }

		public string InactiveText { get; set; }

		public Rectangle TextureRectangle { get; set; }

		public Color ColorHovered { get; set; } = new Color(255, 255, 255, 255);


		public Color ColorActive { get; set; } = new Color(200, 200, 200, 255);


		public Color ColorInactive { get; set; } = new Color(200, 200, 200, 255);


		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			if (ActiveTexture != null)
			{
				AsyncTexture2D texture = (_active ? ActiveTexture : (InactiveTexture ?? ActiveTexture));
				spriteBatch.DrawOnCtrl(this, texture, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Height, bounds.Height), (TextureRectangle == Rectangle.get_Empty()) ? texture.Bounds : TextureRectangle, base.MouseOver ? ColorHovered : (_active ? ColorActive : ColorInactive), 0f, default(Vector2), (SpriteEffects)0);
			}
			if (ActiveText != null)
			{
				string text = (_active ? ActiveText : (InactiveText ?? ActiveText));
				spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont14, new Rectangle(((Rectangle)(ref bounds)).get_Left() + bounds.Height + 3, ((Rectangle)(ref bounds)).get_Top(), bounds.Width - bounds.Height - 3, bounds.Height), Color.get_White(), wrap: false, stroke: false, 0);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			_active = !_active;
		}
	}
}
