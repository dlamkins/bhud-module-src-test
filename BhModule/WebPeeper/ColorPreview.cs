using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

namespace BhModule.WebPeeper
{
	public class ColorPreview : Control
	{
		private static readonly TextureRegion2D _colorTextureRegion = Control.TextureAtlasControl.GetRegion("colorpicker/cp-clr-v1");

		private static readonly TextureRegion2D _borderTextureRegion = Control.TextureAtlasControl.GetRegion("colorpicker/cp-clr-active");

		public Color Color = Color.get_Transparent();

		public ColorPreview()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(Size.op_Implicit(new Size(28, 28)));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _colorTextureRegion, bounds, Color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _borderTextureRegion, bounds, Color.get_Black());
		}
	}
}
