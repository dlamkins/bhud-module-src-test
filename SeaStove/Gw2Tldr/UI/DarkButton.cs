using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class DarkButton : Label
	{
		public DarkButton()
			: this()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Height(26);
			((Label)this).set_TextColor(Color.get_White());
			((Label)this).set_HorizontalAlignment((HorizontalAlignment)1);
			((Label)this).set_VerticalAlignment((VerticalAlignment)1);
			((Label)this).set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)14, (FontStyle)0));
		}

		protected override void Paint(SpriteBatch batch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(batch, (Control)(object)this, Textures.get_Pixel(), bounds, OverlayTheme.Light ? new Color(205, 210, 215, 235) : new Color(40, 44, 50, 220));
			((LabelBase)this).Paint(batch, bounds);
		}
	}
}
