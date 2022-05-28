using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.SubModules.ItemDesctruction.Controls
{
	public class DeleteIndicator : Control
	{
		public Texture2D Texture;

		public DeleteIndicator()
			: this()
		{
			((Control)this).set_ClipsBounds(false);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			if (Texture != null && ((Control)this).get_Parent() != null)
			{
				((Control)this).set_BackgroundColor(Color.get_Magenta());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), Texture, new Rectangle(((Control)this).get_Location(), ((Control)this).get_Size()), (Rectangle?)Texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		protected override void OnShown(EventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnShown(e);
			((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
				.Add(new Point(5, 5)));
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
