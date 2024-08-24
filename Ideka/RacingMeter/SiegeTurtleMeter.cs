using Ideka.BHUDCommon.AnchoredRect;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public static class SiegeTurtleMeter
	{
		public const double UnderwaterIPS = 200.0;

		public const double UnderwaterBoostIPS = 200.0;

		public const double SoftIPS = 382.0;

		public const double ReloadIPS = 450.0;

		public const double LandIPS = 588.0;

		public const double AirIPS = 600.0;

		public const double BoostLandIPS = 649.0;

		public const double BoostAirIPS = 660.0;

		public const double MaxIPS = 800.0;

		public const double AngleAmplitude = 1.2222222222222223;

		public const double VSpeedRange = 1024.0;

		public static AnchoredRect Construct(IMeasurer measurer)
		{
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			AnchoredRect meter = new AnchoredRect();
			ArcMeterMaker arc = new ArcMeterMaker(measurer);
			Projection speedP = arc.Meter.AddProjection(Projection.ZeroTo(800.0)).WithSoftMax(660.0);
			Projection angleP = arc.Meter.AddProjection(Projection.Symmetric(1.2222222222222223));
			meter.AddChild(arc.Meter);
			arc.AddSoftMaxSpeedIndicator(speedP);
			Color startColor = default(Color);
			((Color)(ref startColor))._002Ector(17, 34, 34);
			Color endColor = default(Color);
			((Color)(ref endColor))._002Ector(136, 221, 221);
			double[] thresholds = new double[8] { 200.0, 200.0, 382.0, 450.0, 588.0, 600.0, 649.0, 660.0 };
			foreach (var (i, ips) in thresholds.Enumerate())
			{
				arc.AddZone(speedP, Color.Lerp(startColor, endColor, (float)i / (float)(thresholds.Length - 1)), ips);
			}
			arc.AddSoftMaxSpeedZone(speedP);
			arc.AddAngleOrbMarker(angleP, 0.0);
			arc.AddAngleOrbMarkers(angleP);
			arc.AddCameraAngleOrb(angleP);
			arc.AddForwardAngleOrb(angleP);
			arc.AddArc(speedP);
			arc.AddSpeedNeedle(speedP);
			arc.AddSpeedText(speedP);
			arc.AddAccelText(speedP);
			arc.AddCameraAngleText();
			arc.AddForwardAngleText();
			LineMeterMaker right = new LineMeterMaker(measurer);
			Projection vSpeedP = right.Meter.AddProjection(Projection.ZeroTo(1024.0));
			right.TackOn(right: true);
			meter.AddChild(right.Meter);
			right.AddUpSpeedZone(vSpeedP, (Color?)new Color(51, 68, 136));
			right.AddDownSpeedZone(vSpeedP, (Color?)new Color(34, 34, 102));
			right.AddVSpeedNeedle(vSpeedP, Color.get_White());
			right.AddOutline();
			return meter;
		}
	}
}
