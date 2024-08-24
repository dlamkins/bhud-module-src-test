using System.Collections.Generic;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public static class SkimmerMeter
	{
		public const double LandIPS = 550.0;

		public const double LandHoverIPS = 608.0;

		public const double WaterIPS = 850.0;

		public const double WaterHoverIPS = 935.0;

		public const double UnderwaterIPS = 550.0;

		public const double UnderwaterBoostedIPS = 637.0;

		public const double MaxIPS = 1100.0;

		public const double TopVIPS = 773.0;

		public const double AngleAmplitude = 1.2222222222222223;

		public const double VSpeedRange = 1024.0;

		public static AnchoredRect Construct(IMeasurer measurer)
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			IMeasurer measurer2 = measurer;
			AnchoredRect meter = new AnchoredRect();
			List<AnchoredRect> airComponents = new List<AnchoredRect>();
			List<AnchoredRect> waterComponents = new List<AnchoredRect>();
			ArcMeterMaker arc = new ArcMeterMaker(measurer2);
			Projection speedP = arc.Meter.AddProjection(Projection.ZeroTo(1100.0)).WithSoftMax(935.0);
			Projection angleP = arc.Meter.AddProjection(Projection.Symmetric(1.2222222222222223));
			meter.AddChild(arc.Meter);
			arc.AddSoftMaxSpeedIndicator(speedP);
			airComponents.Add(arc.AddZone(speedP, Color.get_Brown(), 550.0, 608.0));
			airComponents.Add(arc.AddZone(speedP, Color.get_MediumPurple(), 850.0));
			waterComponents.Add(arc.AddZone(speedP, Color.get_MediumPurple(), 550.0));
			waterComponents.Add(arc.AddZone(speedP, Color.get_SaddleBrown(), 637.0));
			arc.AddSoftMaxSpeedZone(speedP);
			airComponents.Add(arc.AddAngleOrbMarker(angleP, 0.0));
			airComponents.AddRange(arc.AddAngleOrbMarkers(angleP));
			airComponents.Add(arc.AddCameraAngleOrb(angleP));
			airComponents.Add(arc.AddForwardAngleOrb(angleP));
			arc.AddArc(speedP);
			arc.AddSpeed3DNeedle(speedP);
			arc.AddSpeedNeedle(speedP);
			arc.AddSpeedText(speedP);
			arc.AddAccelText(speedP);
			arc.AddSlopeAngleText();
			LineMeterMaker right = new LineMeterMaker(measurer2);
			Projection heightP = right.Meter.AddProjection(new Projection()).WithAnchors(new ProjectionAnchors(() => measurer2.Pos.HeightIn));
			Projection vSpeedP = right.Meter.AddProjection(Projection.ZeroTo(1024.0));
			right.TackOn(right: true);
			meter.AddChild(right.Meter);
			right.AddWaterZone(heightP);
			right.AddUpSpeedZone(vSpeedP);
			right.AddDownSpeedZone(vSpeedP);
			heightP.AddAnchor(right.AddWaterLevelNeedle(heightP));
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
			LineMeterMaker left = new LineMeterMaker(measurer2);
			Projection pitch = left.Meter.AddProjection(Projection.Symmetric(7.0 / 9.0));
			left.TackOn(right: false);
			waterComponents.Add(meter.AddChild(left.Meter));
			left.AddNeedle(pitch, Color.get_DarkRed()).With(delegate(LineMeterNeedle x)
			{
				x.Value = -0.16666666666666669;
			});
			left.AddNeedle(pitch, Color.get_DarkRed()).With(delegate(LineMeterNeedle x)
			{
				x.Value = 0.16666666666666669;
			});
			left.AddNeedle(pitch, Color.get_White()).WithUpdate(delegate(LineMeterNeedle x)
			{
				x.Value = measurer2.Pos.CameraPitch;
			});
			left.AddOutline();
			left.AddText(new Vector2(0.5f, 0f), new Vector2(0.5f, 1f)).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"{MeterMaker.RoundedDegrees(measurer2.Pos.CameraPitch)}°";
			});
			meter.WithUpdate(delegate
			{
				bool flag = (double)measurer2.Pos.HeightIn < -40.0;
				foreach (AnchoredRect item in airComponents)
				{
					item.Visible = !flag;
				}
				foreach (AnchoredRect item2 in waterComponents)
				{
					item2.Visible = flag;
				}
			});
			meter.DebugData.Add(("3ips", () => $"{measurer2.Speed.Speed3D:0}"));
			return meter;
		}
	}
}
