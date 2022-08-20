using System.Collections.Generic;
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

		public static RectAnchor Construct()
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			RectAnchor meter = new RectAnchor();
			List<RectAnchor> airComponents = new List<RectAnchor>();
			List<RectAnchor> waterComponents = new List<RectAnchor>();
			ArcMeterMaker arc = new ArcMeterMaker();
			Projection speedP = arc.Meter.AddProjection(Projection.ZeroTo(1100.0)).WithSoftMax(935.0);
			Projection angleP = arc.Meter.AddProjection(Projection.Symmetric(1.2222222222222223));
			meter.AddChild(arc.Meter);
			arc.AddSoftMaxSpeedIndicator(speedP);
			airComponents.Add(arc.AddZone(speedP, Color.get_Brown(), 550.0, 608.0));
			airComponents.Add(arc.AddZone(speedP, Color.get_MediumPurple(), 850.0));
			waterComponents.Add(arc.AddZone(speedP, Color.get_MediumPurple(), 550.0));
			waterComponents.Add(arc.AddZone(speedP, Color.get_SaddleBrown(), 637.0));
			arc.AddSoftMaxSpeedZone(speedP);
			airComponents.AddRange(arc.AddOrbMarkers(angleP));
			airComponents.Add(arc.AddCameraOrb(angleP));
			airComponents.Add(arc.AddForwardOrb(angleP));
			arc.AddArc(speedP);
			arc.AddSpeed3DNeedle(speedP);
			arc.AddSpeedNeedle(speedP);
			arc.AddSpeedText(speedP);
			arc.AddAccelText(speedP);
			arc.AddSlopeAngleText();
			LineMeterMaker right = new LineMeterMaker();
			Projection heightP = right.Meter.AddProjection(new Projection().WithAnchors(new ProjectionAnchors(() => RacingModule.Measurer.Pos.HeightIn)));
			Projection vSpeedP = right.Meter.AddProjection(Projection.ZeroTo(1024.0));
			right.TackOn(right: true);
			meter.AddChild(right.Meter);
			right.AddWaterZone(heightP);
			right.AddUpSpeedZone(vSpeedP);
			right.AddDownSpeedZone(vSpeedP);
			heightP.AddAnchor(right.AddWaterLevelNeedle(heightP));
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
			LineMeterMaker left = new LineMeterMaker();
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
				x.Value = RacingModule.Measurer.Pos.CameraPitch;
			});
			left.AddOutline();
			left.AddText(new Vector2(0.5f, 0f), new Vector2(0.5f, 1f)).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"{MeterMaker.RoundedDegrees(RacingModule.Measurer.Pos.CameraPitch)}°";
			});
			meter.WithUpdate(delegate
			{
				bool flag = (double)RacingModule.Measurer.Pos.HeightIn < -40.0;
				foreach (RectAnchor item in airComponents)
				{
					item.Visible = !flag;
				}
				foreach (RectAnchor item2 in waterComponents)
				{
					item2.Visible = flag;
				}
			});
			Speedometer.ExtraDebug.Add((meter, "3ips", () => $"{RacingModule.Measurer.Speed.Speed3D:0}"));
			return meter;
		}
	}
}
