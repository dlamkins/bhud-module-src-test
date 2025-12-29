using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomeDesigner
{
	public class InputBlocker : Control
	{
		public InputBlocker()
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Location(new Point(0, 0));
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			((Control)this).set_ZIndex(0);
			((Control)this).set_Visible(false);
			((Control)this).set_BackgroundColor(Color.get_White() * 0f);
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)onResized);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
		}

		public void onResized(object sender, ResizedEventArgs e)
		{
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
		}
	}
}
