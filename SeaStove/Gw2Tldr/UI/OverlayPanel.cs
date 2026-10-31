using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaStove.Gw2Tldr.UI
{
	internal class OverlayPanel : Panel
	{
		public bool DrawBackground { get; set; } = true;


		public float BackgroundOpacity { get; set; } = 0.75f;


		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (DrawBackground)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (OverlayTheme.Light ? Color.get_White() : Color.get_Black()) * BackgroundOpacity);
			}
		}

		public OverlayPanel()
			: this()
		{
		}
	}
}
