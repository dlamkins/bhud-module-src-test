using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public static class BeetleMeter
	{
		public const double DriftIPS = 1000.0;

		public const double BoostIPS = 1800.0;

		public const double MaxIPS = 2500.0;

		public const double AngleAmplitude = 1.2222222222222223;

		public static RectAnchor Construct()
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			ArcMeterMaker arcMeterMaker = new ArcMeterMaker();
			Projection speedP = arcMeterMaker.Meter.AddProjection(Projection.ZeroTo(2500.0)).WithSoftMax(1800.0);
			Projection angleP = arcMeterMaker.Meter.AddProjection(Projection.Symmetric(1.2222222222222223));
			arcMeterMaker.AddSoftMaxSpeedIndicator(speedP);
			arcMeterMaker.AddZone(speedP, Color.get_MediumPurple(), 1000.0);
			arcMeterMaker.AddSoftMaxSpeedZone(speedP);
			arcMeterMaker.AddOrbMarkers(angleP);
			arcMeterMaker.AddCameraOrb(angleP);
			arcMeterMaker.AddForwardOrb(angleP);
			arcMeterMaker.AddArc(speedP);
			arcMeterMaker.AddSpeedNeedle(speedP);
			arcMeterMaker.AddSpeedText(speedP);
			arcMeterMaker.AddAccelText(speedP);
			arcMeterMaker.AddSlopeAngleText();
			arcMeterMaker.AddCameraAngleText();
			arcMeterMaker.AddForwardAngleText();
			return arcMeterMaker.Meter;
		}
	}
}
