using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class SearchResultPanel : Panel
	{
		public override void PaintBeforeChildren(SpriteBatch batch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(batch, (Control)(object)this, Textures.get_Pixel(), bounds, OverlayTheme.Light ? new Color(220, 224, 228, 235) : new Color(32, 35, 40, 220));
		}

		public SearchResultPanel()
			: this()
		{
		}
	}
}
