using System;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class LineMeterMaker : MeterMaker<LineMeter>
	{
		public LineMeterMaker(IMeasurer measurer)
			: base(measurer)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			base.Meter.Padding = new Vector2(2f, 2f);
		}

		public void TackOn(bool right, float spacing = 5f, float width = 20f)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			int side = (right ? 1 : 0);
			base.Meter.AnchorMin = new Vector2((float)side, 0f);
			base.Meter.AnchorMax = new Vector2((float)side, 1f);
			base.Meter.Pivot = new Vector2((float)(1 - side), 0f);
			base.Meter.Position = new Vector2(spacing * (float)(right ? 1 : (-1)), 0f);
			base.Meter.SizeDelta = new Vector2(width, 0f);
		}

		public LineMeterZone AddWaterZone(Projection projection)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			return base.Meter.AddChild(new LineMeterZone
			{
				Projection = projection,
				Start = double.NaN,
				End = -40.0,
				Clamp = true,
				Color = MeterMaker.WaterColor
			});
		}

		public LineMeterZone AddUpSpeedZone(Projection projection)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			return base.Meter.AddChild(new LineMeterZone
			{
				Projection = projection,
				Start = double.NaN,
				Clamp = true,
				Color = MeterMaker.UpSpeedColor
			}).WithUpdate(delegate(LineMeterZone x)
			{
				x.End = Math.Max(base.Measurer.Speed.UpSpeed, 0f);
			});
		}

		public LineMeterZone AddDownSpeedZone(Projection projection)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Projection projection2 = projection;
			return base.Meter.AddChild(new LineMeterZone
			{
				Projection = projection2,
				End = double.NaN,
				Clamp = true,
				Color = MeterMaker.DownSpeedColor
			}).WithUpdate(delegate(LineMeterZone x)
			{
				x.Start = projection2.MaxValue - (double)Math.Max(base.Measurer.Speed.DownSpeed, 0f);
			});
		}

		public LineMeterNeedle AddNeedle(Projection projection, Color color)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			return base.Meter.AddChild(new LineMeterNeedle
			{
				Projection = projection,
				Clamp = true,
				Thickness = 2f,
				Color = color
			});
		}

		public LineMeterNeedle AddWaterLevelNeedle(Projection projection)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return AddNeedle(projection, MeterMaker.WaterColor).WithUpdate(delegate(LineMeterNeedle x)
			{
				x.Value = -40.0;
			});
		}

		public LineMeterNeedle AddHeightNeedle(Projection projection)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return AddNeedle(projection, MeterMaker.DefaultColor).WithUpdate(delegate(LineMeterNeedle x)
			{
				x.Value = base.Measurer.Pos.HeightIn;
			});
		}

		public RectAnchor AddLevelScroll(Projection projection, RectAnchor container)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Projection projection2 = projection;
			SimpleRectangle black = container.AddChild(new SimpleRectangle
			{
				FillColor = Color.get_Black()
			});
			SimpleRectangle white = container.AddChild(new SimpleRectangle
			{
				FillColor = Color.get_White()
			});
			return base.Meter.AddChild(container).WithUpdate(delegate
			{
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				double anchorLevel = projection2.GetAnchorLevel();
				SimpleRectangle simpleRectangle3;
				SimpleRectangle simpleRectangle4;
				if ((int)anchorLevel % 2 != 0)
				{
					SimpleRectangle simpleRectangle = white;
					SimpleRectangle simpleRectangle2 = black;
					simpleRectangle3 = simpleRectangle;
					simpleRectangle4 = simpleRectangle2;
				}
				else
				{
					SimpleRectangle simpleRectangle5 = black;
					SimpleRectangle simpleRectangle2 = white;
					simpleRectangle3 = simpleRectangle5;
					simpleRectangle4 = simpleRectangle2;
				}
				float num = 1f - (float)(anchorLevel % 1.0);
				simpleRectangle4.AnchorMax = new Vector2(1f, 1f);
				simpleRectangle4.AnchorMin = new Vector2(0f, num);
				simpleRectangle3.AnchorMax = new Vector2(1f, num);
				simpleRectangle3.AnchorMin = new Vector2(0f, 0f);
			});
		}

		public SizedTextLabel AddLevelText(Projection projection, Vector2 anchor, Vector2 pivot)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Projection projection2 = projection;
			return AddText(anchor, pivot).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"L{(int)projection2.GetAnchorLevel()}";
			});
		}

		public SimpleRectangle AddOutline()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return base.Meter.AddChild(new SimpleRectangle
			{
				Color = MeterMaker.DefaultColor,
				Thickness = 2f
			});
		}
	}
}
