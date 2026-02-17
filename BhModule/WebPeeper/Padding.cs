using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.WebPeeper
{
	internal class Padding : Control
	{
		public string message = "";

		public Padding(int height = 16)
			: this()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(0, height));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Width(((Control)((Control)this).get_Parent()).get_Width());
			if (!(message == ""))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, message, GameService.Content.get_DefaultFont14(), new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()), Color.get_Red(), false, false, 1, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}
	}
}
