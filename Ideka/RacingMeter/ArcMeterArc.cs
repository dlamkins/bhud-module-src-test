using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class ArcMeterArc : ArcMeterChild
	{
		public float Thickness { get; set; }

		public Color Color { get; set; }

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			target.SpriteBatch.DrawArc(base.Center, base.Radii, base.Meter.MinArcAngle, base.Meter.ArcAmplitude, GetSides(base.Meter.ArcAmplitude), Color, Thickness);
		}
	}
}
