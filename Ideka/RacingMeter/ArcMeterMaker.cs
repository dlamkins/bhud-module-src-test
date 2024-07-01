using Blish_HUD;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class ArcMeterMaker : MeterMaker<ArcMeter>
	{
		public ArcMeterMaker(IMeasurer measurer)
			: base(measurer)
		{
		}

		public ArcMeterZone AddSoftMaxSpeedIndicator(Projection projection)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Projection projection2 = projection;
			return base.Meter.AddChild(new ArcMeterZone
			{
				Start = double.NaN,
				End = double.NaN,
				Thickness = 15f,
				Color = MeterMaker.SoftMaxColor
			}).WithUpdate(delegate(ArcMeterZone x)
			{
				x.Visible = projection2.OverSoftMax(base.Measurer.Speed.Speed2D);
			});
		}

		public ArcMeterZone AddSoftMaxSpeedZone(Projection projection)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return AddZone(projection, MeterMaker.SoftMaxColor, projection.SoftMax);
		}

		public ArcMeterZone AddZone(Projection projection, Color color, double start, double end = double.NaN)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			return base.Meter.AddChild(new ArcMeterZone
			{
				Projection = projection,
				Start = start,
				End = end,
				Thickness = 10f,
				Color = color
			});
		}

		public void AddArc(Projection projectionForMargins)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			base.Meter.AddChild(new ArcMeterArc
			{
				Thickness = 2f,
				Color = MeterMaker.DefaultColor
			});
			if (projectionForMargins != null)
			{
				base.Meter.AddChild(new ArcMeterMargins
				{
					Projection = projectionForMargins,
					Thickness = 5f,
					Color = MeterMaker.MarginColor
				});
			}
		}

		public ArcMeterNeedle AddNeedle(Projection projection, Color color, float length = 20f)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return base.Meter.AddChild(new ArcMeterNeedle
			{
				Projection = projection,
				Clamp = true,
				Color = color,
				Length = length,
				Thickness = 2f
			});
		}

		public ArcMeterNeedle AddSpeedNeedle(Projection projection)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return AddNeedle(projection, MeterMaker.DefaultColor).WithUpdate(delegate(ArcMeterNeedle x)
			{
				x.Value = base.Measurer.Speed.Speed2D;
			});
		}

		public ArcMeterNeedle AddSpeed3DNeedle(Projection projection)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return AddNeedle(projection, MeterMaker.Default3DColor).WithUpdate(delegate(ArcMeterNeedle x)
			{
				x.Value = base.Measurer.Speed.Speed3D;
			});
		}

		public ArcMeterNeedle AddAngleNeedle(Projection projection, float length, Color color)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			return base.Meter.AddChild(new ArcMeterNeedle
			{
				Projection = projection,
				Distance = 15f,
				Length = length,
				Color = color,
				Thickness = 2f
			});
		}

		public ArcMeterNeedle AddAngleNeedleMarker(Projection projection, double degrees)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return AddAngleNeedle(projection, 7.5f, MeterMaker.DefaultColor).With(delegate(ArcMeterNeedle x)
			{
				x.Value = degrees * 0.005555555555555556;
			});
		}

		public ArcMeterNeedle[] AddAngleNeedleMarkers(Projection projection, double guideDegrees = 45.0)
		{
			return new ArcMeterNeedle[2]
			{
				AddAngleNeedleMarker(projection, guideDegrees),
				AddAngleNeedleMarker(projection, 0.0 - guideDegrees)
			};
		}

		public ArcMeterNeedle AddForwardAngleNeedle(Projection projection)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return AddAngleNeedle(projection, 5f, MeterMaker.ForwardOrbColor).WithUpdate(delegate(ArcMeterNeedle x)
			{
				x.Value = base.Measurer.Speed.FwdMovementYaw;
			});
		}

		public ArcMeterOrb AddAngleOrb(Projection projection, float radius, Color color)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			return base.Meter.AddChild(new ArcMeterOrb
			{
				Projection = projection,
				Distance = 15f,
				Radius = radius,
				Color = color,
				Thickness = 2f
			});
		}

		public ArcMeterOrb AddAngleOrbMarker(Projection projection, double degrees)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return AddAngleOrb(projection, 5f, MeterMaker.DefaultColor).With(delegate(ArcMeterOrb x)
			{
				x.Value = degrees * 0.005555555555555556;
			});
		}

		public ArcMeterOrb[] AddAngleOrbMarkers(Projection projection, double guideDegrees = 45.0)
		{
			return new ArcMeterOrb[2]
			{
				AddAngleOrbMarker(projection, guideDegrees),
				AddAngleOrbMarker(projection, 0.0 - guideDegrees)
			};
		}

		public ArcMeterOrb AddCameraAngleOrb(Projection projection)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return AddAngleOrb(projection, 15f, MeterMaker.DefaultColor).WithUpdate(delegate(ArcMeterOrb x)
			{
				x.Value = base.Measurer.Speed.CamMovementYaw;
			});
		}

		public ArcMeterOrb AddForwardAngleOrb(Projection projection)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return AddAngleOrb(projection, 10f, MeterMaker.ForwardOrbColor).WithUpdate(delegate(ArcMeterOrb x)
			{
				x.Value = base.Measurer.Speed.FwdMovementYaw;
			});
		}

		public SizedTextLabel AddSpeedText(Projection projection)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Projection projection2 = projection;
			return AddText(new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), GameService.Content.get_DefaultFont32()).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"{projection2.GetBP(base.Measurer.Speed.Speed2D)}";
			});
		}

		public SizedTextLabel AddSpeed3DText(Projection projection)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Projection projection2 = projection;
			return AddText(new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), GameService.Content.get_DefaultFont32()).With(delegate(SizedTextLabel x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				x.Color = MeterMaker.Default3DColor;
			}).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"{projection2.GetBP(base.Measurer.Speed.Speed3D)}";
			});
		}

		public SizedTextLabel AddAccelText(Projection projection)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Projection projection2 = projection;
			return AddText(new Vector2(0.6f, 0.4f), new Vector2(0f, 0.5f)).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"Accel.\n{projection2.GetBP(base.Measurer.Accel.Accel2D)}";
			});
		}

		public SizedTextLabel AddAccel3DText(Projection projection)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Projection projection2 = projection;
			return AddText(new Vector2(0.6f, 0.4f), new Vector2(0f, 0.5f)).With(delegate(SizedTextLabel x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				x.Color = MeterMaker.Default3DColor;
			}).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"Accel.\n{projection2.GetBP(base.Measurer.Accel.Accel3D)}";
			});
		}

		public SizedTextLabel AddSlopeAngleText()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return AddText(new Vector2(0.6f, 0.8f), new Vector2(0f, 0.5f)).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"Slope.\n{MeterMaker.RoundedDegrees(base.Measurer.Speed.SlopeAngle)}°";
			});
		}

		public SizedTextLabel AddCameraAngleText()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return AddText(new Vector2(0.4f, 0.4f), new Vector2(1f, 0.5f)).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"Cam.\n{MeterMaker.RoundedDegrees(base.Measurer.Speed.CamMovementYaw)}°";
			});
		}

		public SizedTextLabel AddForwardAngleText()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return AddText(new Vector2(0.4f, 0.4f), new Vector2(0f, 0.5f)).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"Fwd.\n{MeterMaker.RoundedDegrees(base.Measurer.Speed.FwdMovementYaw)}°";
			});
		}
	}
}
