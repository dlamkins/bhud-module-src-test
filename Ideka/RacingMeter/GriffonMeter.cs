using System;
using Ideka.BHUDCommon.AnchoredRect;
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

		public static AnchoredRect Construct(IMeasurer measurer)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			IMeasurer measurer2 = measurer;
			AnchoredRect anchoredRect = new AnchoredRect();
			ArcMeterMaker arc = new ArcMeterMaker(measurer2);
			Projection speedP = arc.Meter.AddProjection(Projection.ZeroTo(2500.0)).WithSoftMax(2000.0);
			anchoredRect.AddChild(arc.Meter);
			arc.AddSoftMaxSpeedIndicator(speedP);
			arc.AddZone(speedP, Color.get_MediumPurple(), 1000.0);
			arc.AddZone(speedP, Color.get_DarkGreen(), 1310.0);
			arc.AddSoftMaxSpeedZone(speedP);
			arc.AddArc(speedP);
			arc.AddSpeedNeedle(speedP);
			arc.AddSpeed3DNeedle(speedP);
			arc.AddSpeed3DText(speedP);
			arc.AddAccel3DText(speedP);
			LineMeterMaker right = new LineMeterMaker(measurer2);
			Projection heightP = right.Meter.AddProjection(new Projection()).WithAnchors(new ProjectionAnchors(() => measurer2.Pos.HeightIn));
			Projection vSpeedP = right.Meter.AddProjection(Projection.ZeroTo(2000.0));
			right.TackOn(right: true);
			anchoredRect.AddChild(right.Meter);
			right.AddWaterZone(heightP);
			right.AddUpSpeedZone(vSpeedP);
			right.AddDownSpeedZone(vSpeedP);
			right.AddWaterLevelNeedle(heightP);
			LineMeterNeedle diveHeight = heightP.AddAnchor(right.AddNeedle(heightP, Color.get_DarkRed()));
			right.AddHeightNeedle(heightP);
			right.AddOutline();
			right.AddLevelScroll(heightP, new AnchoredRect
			{
				AnchorMin = new Vector2(1f, 0f),
				AnchorMax = new Vector2(1f, 1f),
				Position = new Vector2(5f, 0f),
				SizeDelta = new Vector2(5f, 0f)
			});
			right.AddLevelText(heightP, new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
			anchoredRect.AddChild(new EnduranceMarker
			{
				Percentage = 0.76,
				Color = Color.get_Black(),
				Thickness = 2f
			});
			bool preClimb = false;
			bool diving = false;
			bool climbing = false;
			anchoredRect.DebugData.Add(("3ips", () => $"{measurer2.Speed.Speed3D:0}"));
			anchoredRect.DebugData.Add(("preclimb", () => $"{preClimb}"));
			anchoredRect.DebugData.Add(("diving", () => $"{diving}"));
			anchoredRect.DebugData.Add(("climbing", () => $"{climbing}"));
			anchoredRect.WithUpdate(delegate
			{
				float speed2D = measurer2.Speed.Speed2D;
				float speed3D = measurer2.Speed.Speed3D;
				float accel2D = measurer2.Accel.Accel2D;
				float upSpeed = measurer2.Speed.UpSpeed;
				float downSpeed = measurer2.Speed.DownSpeed;
				float downAccel = measurer2.Accel.DownAccel;
				float heightIn = measurer2.Pos.HeightIn;
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
			return anchoredRect;
		}
	}
}
