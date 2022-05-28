using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.SubModules.ItemDesctruction.Controls
{
	public class CursorIcon : Control
	{
		public Texture2D Texture;

		public CursorIcon()
			: this()
		{
			((Control)this).set_ClipsBounds(false);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			if (Texture != null && ((Control)this).get_Parent() != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), Texture, new Rectangle(Control.get_Input().get_Mouse().get_Position()
					.X + ((Control)this).get_Size().Y, Control.get_Input().get_Mouse().get_Position()
					.Y + ((Control)this).get_Size().Y, ((Control)this).get_Size().X, ((Control)this).get_Size().Y), (Rectangle?)Texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			Texture2D texture = Texture;
			if (texture != null)
			{
				((GraphicsResource)texture).Dispose();
			}
		}
	}
}
