using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts.Controls
{
	internal class DrawMouseCursor : Control
	{
		private Texture2D _mouseTexture;

		public DrawMouseCursor(TextureCache textureCache)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Visible(false);
			((Control)this).set_Padding(Thickness.Zero);
			_mouseTexture = textureCache.GetImgFile(textureCache.MouseTexture);
			((Control)this).set_Size(new Point(_mouseTexture.get_Width(), _mouseTexture.get_Height()));
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_ZIndex(int.MaxValue);
			Rectangle rect = default(Rectangle);
			((Rectangle)(ref rect))._002Ector(0, 0, ((Control)this).get_Size().X, ((Control)this).get_Size().Y);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _mouseTexture, rect);
		}
	}
}
