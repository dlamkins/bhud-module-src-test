using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule.CustomControls.CustomTab
{
	public class CustomLoadingSpinner : LoadingSpinner
	{
		private const int DRAWLENGTH = 32;

		public CustomLoadingSpinner()
			: this()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(32, 32));
		}

		public void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, bounds);
		}
	}
}
