using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.MouseCursor.Controls
{
	public class DrawMouseCursor : Container
	{
		public Texture2D Texture;

		public Color Tint = Color.White;

		public bool AboveBlish;

		public DrawMouseCursor()
			: this()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(0, 0));
			((Control)this).set_Visible(true);
			((Control)this).set_Padding(Thickness.Zero);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (AboveBlish)
			{
				((Control)this).set_ZIndex(int.MaxValue);
			}
			else
			{
				((Control)this).set_ZIndex(0);
			}
			if (Texture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Texture, new Rectangle(0, 0, ((Control)this).get_Size().X, ((Control)this).get_Size().Y), (Rectangle?)null, Tint);
			}
		}
	}
}
