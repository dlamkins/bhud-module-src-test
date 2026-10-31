using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class OverlayBackground : Control
	{
		public float BackgroundOpacity { get; set; } = 0.75f;


		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		protected override void Paint(SpriteBatch batch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(batch, (Control)(object)this, Textures.get_Pixel(), bounds, (OverlayTheme.Light ? Color.get_White() : Color.get_Black()) * BackgroundOpacity);
		}

		public OverlayBackground()
			: this()
		{
		}
	}
}
