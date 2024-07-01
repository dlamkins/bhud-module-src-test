using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class ProgressMenuItem : OnelineMenuItem
	{
		private static readonly Color FinishedColor = Color.get_Yellow() * 0.1f;

		private static readonly Color ProgressColor = Color.get_Red() * 0.2f;

		public double Progress { get; set; }

		public ProgressMenuItem(string text)
			: base(text)
		{
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(Point.get_Zero(), Size.op_Implicit(new Size((int)((double)((Control)this).get_Size().X * Progress), ((Control)this).get_Size().Y))), (Progress < 1.0) ? ProgressColor : FinishedColor);
			base.PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}
