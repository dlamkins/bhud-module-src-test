using System;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public abstract class ArcMeterChild : MeterChild<ArcMeter>
	{
		protected Vector2 Center => base.Meter.Center;

		protected Vector2 Radii => base.Meter.Radii;

		protected int GetSides(double amplitude)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return SpriteBatchExtensions.GetSidesFor(Radii, amplitude);
		}

		protected override Vector2 Point(double value, bool clamp)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			double radians = base.Projection.Point(base.Meter, value, clamp) * Math.PI;
			return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
		}
	}
}
