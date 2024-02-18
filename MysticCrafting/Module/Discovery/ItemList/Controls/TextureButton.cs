using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.Discovery.ItemList.Controls
{
	public class TextureButton : Control
	{
		public bool HasActiveState = true;

		public bool Active { get; set; }

		public AsyncTexture2D Texture { get; set; }

		protected override void OnClick(MouseEventArgs e)
		{
			Active = !Active;
			base.OnClick(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			Color tint = (HasActiveState ? (Color.White * 0.3f) : Color.LightYellow);
			if (Active)
			{
				tint = Color.LightYellow;
			}
			if (base.MouseOver)
			{
				tint = Color.LightYellow * 0.6f;
			}
			spriteBatch.DrawOnCtrl(this, Texture, new Rectangle((int)base.Padding.Left, (int)base.Padding.Top, base.Size.X, base.Size.Y), tint);
		}
	}
}
