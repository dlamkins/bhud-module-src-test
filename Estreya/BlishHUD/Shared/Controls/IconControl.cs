using Blish_HUD.Content;
using Blish_HUD.Controls;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class IconControl : Control
	{
		private readonly AsyncTexture2D _texture;

		private readonly Texture2D _fallbackTexture;

		public IconControl(AsyncTexture2D texture, Texture2D fallbackTexture)
			: this()
		{
			_texture = texture;
			_fallbackTexture = fallbackTexture;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Texture2D texture = (_texture.get_HasSwapped() ? _texture.get_Texture() : _fallbackTexture);
			spriteBatch.DrawOnCtrl((Control)(object)this, texture, RectangleF.op_Implicit(bounds), Color.get_White() * ((Control)this).AbsoluteOpacity());
		}
	}
}
