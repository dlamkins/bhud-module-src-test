using Ideka.NetCommon;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public static class SkiffMeter
	{
		public const double Tier1IPS = 300.0;

		public const double Tier2IPS = 600.0;

		public const double Tier3IPS = 800.0;

		public const double BoostIPS = 1000.0;

		public const double MaxIPS = 1200.0;

		public const double AngleAmplitude = 1.2222222222222223;

		public static float DistanceToZero(double ips)
		{
			return (float)(-4.06707E-09 * MathUtils.Biquadrated(ips) + 8.34847E-06 * MathUtils.Cubed(ips) - 0.000873353 * MathUtils.Squared(ips) + 0.241738 * ips - 1.43113);
		}

		public static RectAnchor Construct()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			ArcMeterMaker arcMeterMaker = new ArcMeterMaker();
			Projection speedP = arcMeterMaker.Meter.AddProjection(Projection.ZeroTo(1200.0)).WithSoftMax(1000.0);
			Projection angleP = arcMeterMaker.Meter.AddProjection(Projection.Symmetric(1.2222222222222223));
			arcMeterMaker.Meter.AddChild(new WorldCircle
			{
				Color = MeterMaker.DefaultColor,
				Thickness = 2f
			}).WithUpdate(delegate(WorldCircle x)
			{
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				float speed2D = RacingModule.Measurer.Speed.Speed2D;
				x.Visible = (double)speed2D > 330.0;
				if (x.Visible)
				{
					Vector3 inches = RacingModule.Measurer.Pos.Inches;
					Vector3 front = RacingModule.Measurer.Pos.Front;
					float num = DistanceToZero(speed2D);
					Vector3 val = inches + front * num;
					val.Z = 0f;
					x.WorldPosition = val * 0.0254f;
				}
			});
			arcMeterMaker.AddSoftMaxSpeedIndicator(speedP);
			arcMeterMaker.AddZone(speedP, Color.get_Green(), 300.0);
			arcMeterMaker.AddZone(speedP, Color.get_YellowGreen(), 600.0);
			arcMeterMaker.AddZone(speedP, Color.get_OrangeRed(), 800.0);
			arcMeterMaker.AddSoftMaxSpeedZone(speedP);
			arcMeterMaker.AddOrbMarkers(angleP);
			arcMeterMaker.AddCameraOrb(angleP);
			arcMeterMaker.AddForwardOrb(angleP);
			arcMeterMaker.AddArc(speedP);
			arcMeterMaker.AddSpeedNeedle(speedP);
			arcMeterMaker.AddSpeedText(speedP);
			arcMeterMaker.AddAccelText(speedP);
			arcMeterMaker.AddCameraAngleText();
			arcMeterMaker.AddForwardAngleText();
			return arcMeterMaker.Meter;
		}
	}
}
