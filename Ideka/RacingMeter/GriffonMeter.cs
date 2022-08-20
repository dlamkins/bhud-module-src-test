using System;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public static class GriffonMeter
	{
		public const double DiveIPS = 300.0;

		public const double LandIPS = 500.0;

		public const double AirIPS = 600.0;

		public const double ClimbIPS = 1000.0;

		public const double MaxDiveIPS = 1310.0;

		public const double DownflapIPS = 2000.0;

		public const double MaxIPS = 2500.0;

		public const double TopDivingIPS = 1234.0;

		public const double TopFlapVSpeed = 680.0;

		public const double TopFreefallSpeed = 426.0;

		public const double TopFreedropSpeed = 1067.0;

		public const double VSpeedRange = 2000.0;

		public static RectAnchor Construct()
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			RectAnchor meter = new RectAnchor();
			ArcMeterMaker arc = new ArcMeterMaker();
			Projection speedP = arc.Meter.AddProjection(Projection.ZeroTo(2500.0)).WithSoftMax(2000.0);
			meter.AddChild(arc.Meter);
			arc.AddSoftMaxSpeedIndicator(speedP);
			arc.AddZone(speedP, Color.get_MediumPurple(), 1000.0);
			arc.AddZone(speedP, Color.get_DarkGreen(), 1310.0);
			arc.AddSoftMaxSpeedZone(speedP);
			arc.AddArc(speedP);
			arc.AddSpeedNeedle(speedP);
			arc.AddSpeed3DNeedle(speedP);
			arc.AddSpeed3DText(speedP);
			arc.AddAccel3DText(speedP);
			LineMeterMaker right = new LineMeterMaker();
			Projection heightP = right.Meter.AddProjection(new Projection()).WithAnchors(new ProjectionAnchors(() => RacingModule.Measurer.Pos.HeightIn));
			Projection vSpeedP = right.Meter.AddProjection(Projection.ZeroTo(2000.0));
			right.TackOn(right: true);
			meter.AddChild(right.Meter);
			right.AddWaterZone(heightP);
			right.AddUpSpeedZone(vSpeedP);
			right.AddDownSpeedZone(vSpeedP);
			right.AddWaterLevelNeedle(heightP);
			LineMeterNeedle diveHeight = heightP.AddAnchor(right.AddNeedle(heightP, Color.get_DarkRed()));
			right.AddHeightNeedle(heightP);
			right.AddOutline();
			right.AddLevelScroll(heightP, new RectAnchor
			{
				AnchorMin = new Vector2(1f, 0f),
				AnchorMax = new Vector2(1f, 1f),
				Position = new Vector2(5f, 0f),
				SizeDelta = new Vector2(5f, 0f)
			});
			right.AddLevelText(heightP, new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
			meter.AddChild(new EnduranceMarker
			{
				Percentage = 0.76,
				Color = Color.get_Black(),
				Thickness = 2f
			});
			bool preClimb = false;
			bool diving = false;
			bool climbing = false;
			Speedometer.ExtraDebug.Add((meter, "3ips", () => $"{RacingModule.Measurer.Speed.Speed3D:0}"));
			Speedometer.ExtraDebug.Add((meter, "preclimb", () => $"{preClimb}"));
			Speedometer.ExtraDebug.Add((meter, "diving", () => $"{diving}"));
			Speedometer.ExtraDebug.Add((meter, "climbing", () => $"{climbing}"));
			meter.WithUpdate(delegate
			{
				float speed2D = RacingModule.Measurer.Speed.Speed2D;
				float speed3D = RacingModule.Measurer.Speed.Speed3D;
				float accel2D = RacingModule.Measurer.Accel.Accel2D;
				float upSpeed = RacingModule.Measurer.Speed.UpSpeed;
				float downSpeed = RacingModule.Measurer.Speed.DownSpeed;
				float downAccel = RacingModule.Measurer.Accel.DownAccel;
				float heightIn = RacingModule.Measurer.Pos.HeightIn;
				diving = ((double)speed2D >= 300.0 || (diving && (double)speed3D > 300.0)) && downSpeed > 0f && (downAccel > 1300f || (diving && downAccel > 700f) || (diving && downSpeed > 500f));
				climbing = (climbing || preClimb) && upSpeed > 500f;
				preClimb = !climbing && !diving && (double)speed2D >= 1000.0 && (Math.Abs(accel2D) > 500f || (preClimb && accel2D > 0f) || (preClimb && upSpeed > 0f && accel2D < -100f));
				if (diving && diveHeight.Value < (double)heightIn)
				{
					diveHeight.Value = heightIn;
					diveHeight.Visible = true;
				}
				else if ((double)speed2D <= 600.0 && !preClimb && !climbing && !diving)
				{
					diveHeight.Value = 0.0;
					diveHeight.Visible = false;
				}
			});
			return meter;
		}
	}
}
