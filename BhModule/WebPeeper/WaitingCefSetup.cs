using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.WebPeeper
{
	internal class WaitingCefSetup : Control
	{
		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, new Rectangle(((Control)this).get_Size().X / 2 - 50, ((Control)this).get_Size().Y / 2 - 50, 100, 100));
		}

		public WaitingCefSetup()
			: this()
		{
		}
	}
}
