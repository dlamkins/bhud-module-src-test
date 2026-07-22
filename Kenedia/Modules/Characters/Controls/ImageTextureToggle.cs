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
			if (ActiveTexture != null)
			{
				AsyncTexture2D texture = (_active ? ActiveTexture : (InactiveTexture ?? ActiveTexture));
				spriteBatch.DrawOnCtrl(this, texture, new Rectangle(bounds.Left, bounds.Top, bounds.Height, bounds.Height), (TextureRectangle == Rectangle.Empty) ? texture.Bounds : TextureRectangle, base.MouseOver ? ColorHovered : (_active ? ColorActive : ColorInactive), 0f, default(Vector2));
			}
			if (ActiveText != null)
			{
				string text = (_active ? ActiveText : (InactiveText ?? ActiveText));
				spriteBatch.DrawStringOnCtrl(this, text, GameService.Content.DefaultFont14, new Rectangle(bounds.Left + bounds.Height + 3, bounds.Top, bounds.Width - bounds.Height - 3, bounds.Height), Color.White, wrap: false, stroke: false, 0);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			_active = !_active;
		}
	}
}
