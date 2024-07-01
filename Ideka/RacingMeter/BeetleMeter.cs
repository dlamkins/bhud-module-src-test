using System;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public static class BeetleMeter
	{
		public const double TrickIPS = 800.0;

		public const double DriftIPS = 1000.0;

		public const double BoostIPS = 1800.0;

		public const double MaxIPS = 2500.0;

		public const double AngleAmplitude = 1.2222222222222223;

		public const double MaxDriftHoldSeconds = 1.2;

		public const double DriftHoldWarningThreshold = 1.0;

		public static readonly Color DriftHoldColor = Color.get_LightBlue();

		public static readonly Color DriftHoldWarningColor = Color.get_DarkOrange();

		public static readonly Color DriftHoldMaxColor = Color.get_DarkRed();

		public static AnchoredRect Construct(IMeasurer measurer, Func<bool?> isDriftKeyDown)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			Func<bool?> isDriftKeyDown2 = isDriftKeyDown;
			IMeasurer measurer2 = measurer;
			AnchoredRect anchoredRect = new AnchoredRect();
			ArcMeterMaker arc = new ArcMeterMaker(measurer2);
			Projection speedP = arc.Meter.AddProjection(Projection.ZeroTo(2500.0)).WithSoftMax(1800.0);
			Projection angleP = arc.Meter.AddProjection(Projection.Symmetric(1.2222222222222223));
			arc.AddSoftMaxSpeedIndicator(speedP);
			anchoredRect.AddChild(arc.Meter);
			arc.AddZone(speedP, Color.get_Goldenrod(), 800.0);
			arc.AddZone(speedP, Color.get_MediumPurple(), 1000.0);
			arc.AddSoftMaxSpeedZone(speedP);
			arc.AddAngleOrbMarker(angleP, 0.0);
			arc.AddAngleOrbMarkers(angleP);
			arc.AddCameraAngleOrb(angleP);
			arc.AddForwardAngleOrb(angleP);
			arc.AddArc(speedP);
			arc.AddSpeedNeedle(speedP);
			arc.AddSpeedText(speedP);
			arc.AddAccelText(speedP);
			arc.AddSlopeAngleText();
			arc.AddCameraAngleText();
			arc.AddForwardAngleText();
			LineMeterMaker left = new LineMeterMaker(measurer2);
			Projection driftHoldP = left.Meter.AddProjection(Projection.ZeroTo(1.2));
			left.TackOn(right: false);
			anchoredRect.AddChild(left.Meter);
			LineMeterZone driftHold = left.Meter.AddChild(new LineMeterZone
			{
				Projection = driftHoldP,
				Start = double.NaN,
				Clamp = true
			});
			left.AddOutline();
			SizedTextLabel driftHoldTime = left.AddText(new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
			SizedTextLabel driftHoldSpeed = left.AddText(new Vector2(0.5f, 1f), new Vector2(0.5f, 0f));
			DateTime? pressed = null;
			float startSpeed = 0f;
			left.Meter.WithUpdate(delegate
			{
				//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				bool? flag = isDriftKeyDown2();
				if (flag.HasValue)
				{
					bool valueOrDefault = flag.GetValueOrDefault();
					left.Meter.Visible = true;
					if (!valueOrDefault)
					{
						pressed = null;
						startSpeed = 0f;
					}
					else if (!pressed.HasValue)
					{
						pressed = DateTime.UtcNow;
						startSpeed = measurer2.Speed.Speed2D;
					}
					TimeSpan timeSpan = (pressed.HasValue ? (DateTime.UtcNow - pressed.Value) : TimeSpan.Zero);
					driftHold.End = timeSpan.TotalSeconds;
					driftHold.Color = ((timeSpan.TotalSeconds <= 1.0) ? DriftHoldColor : ((timeSpan.TotalSeconds <= 1.2) ? DriftHoldWarningColor : DriftHoldMaxColor));
					if (pressed.HasValue || string.IsNullOrEmpty(driftHoldTime.Text))
					{
						int bP = speedP.GetBP(measurer2.Speed.Speed2D - startSpeed);
						driftHoldTime.Text = $"{timeSpan.TotalSeconds:0.0}";
						driftHoldSpeed.Text = string.Format("{0}{1}", (bP > 0) ? "+" : "", bP);
					}
				}
				else
				{
					left.Meter.Visible = false;
				}
			});
			return anchoredRect;
		}
	}
}
