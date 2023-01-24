using System;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

		public static RectAnchor Construct()
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			RectAnchor rectAnchor = new RectAnchor();
			ArcMeterMaker arc = new ArcMeterMaker();
			Projection speedP = arc.Meter.AddProjection(Projection.ZeroTo(2500.0)).WithSoftMax(1800.0);
			Projection angleP = arc.Meter.AddProjection(Projection.Symmetric(1.2222222222222223));
			arc.AddSoftMaxSpeedIndicator(speedP);
			rectAnchor.AddChild(arc.Meter);
			arc.AddZone(speedP, Color.get_Goldenrod(), 800.0);
			arc.AddZone(speedP, Color.get_MediumPurple(), 1000.0);
			arc.AddSoftMaxSpeedZone(speedP);
			arc.AddOrbMarkers(angleP);
			arc.AddCameraOrb(angleP);
			arc.AddForwardOrb(angleP);
			arc.AddArc(speedP);
			arc.AddSpeedNeedle(speedP);
			arc.AddSpeedText(speedP);
			arc.AddAccelText(speedP);
			arc.AddSlopeAngleText();
			arc.AddCameraAngleText();
			arc.AddForwardAngleText();
			LineMeterMaker left = new LineMeterMaker();
			Projection driftHoldP = left.Meter.AddProjection(Projection.ZeroTo(1.2));
			left.TackOn(right: false);
			rectAnchor.AddChild(left.Meter);
			LineMeterZone driftHold = left.Meter.AddChild(new LineMeterZone
			{
				Projection = driftHoldP,
				Start = double.NaN,
				Clamp = true
			});
			left.AddOutline();
			SizedTextLabel driftHoldText = left.AddText(new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
			DateTime? pressed = null;
			left.Meter.WithUpdate(delegate
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Invalid comparison between Unknown and I4
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				bool flag = (int)RacingModule.Settings.BeetleDriftKeyValue > 0;
				left.Meter.Visible = flag;
				if (flag)
				{
					KeyboardState state = GameService.Input.get_Keyboard().get_State();
					if (((KeyboardState)(ref state)).IsKeyUp(RacingModule.Settings.BeetleDriftKeyValue))
					{
						pressed = null;
					}
					else if (!pressed.HasValue)
					{
						pressed = DateTime.UtcNow;
					}
					TimeSpan timeSpan = (pressed.HasValue ? (DateTime.UtcNow - pressed.Value) : TimeSpan.Zero);
					driftHold.End = timeSpan.TotalSeconds;
					driftHold.Color = ((timeSpan.TotalSeconds <= 1.0) ? DriftHoldColor : ((timeSpan.TotalSeconds <= 1.2) ? DriftHoldWarningColor : DriftHoldMaxColor));
					if (pressed.HasValue || string.IsNullOrEmpty(driftHoldText.Text))
					{
						driftHoldText.Text = $"{timeSpan.TotalSeconds:0.0}";
					}
				}
			});
			return rectAnchor;
		}
	}
}
