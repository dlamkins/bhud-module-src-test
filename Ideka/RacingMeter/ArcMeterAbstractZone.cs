using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public abstract class ArcMeterAbstractZone : ArcMeterChild
	{
		public float Thickness { get; set; }

		public Color Color { get; set; }

		protected void DrawZone(RectTarget target, double from, double to)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			double start = (double.IsNaN(from) ? base.Meter.MinArcAngle : base.Projection.Point(base.Meter, from));
			double amplitude = (double.IsNaN(to) ? (1.0 - base.Meter.MinArcAngle) : base.Projection.Point(base.Meter, to)) - start;
			target.SpriteBatch.DrawArc(base.Center, base.Radii, start, amplitude, GetSides(amplitude), Color, Thickness);
		}
	}
}
