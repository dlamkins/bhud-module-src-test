using System;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class LineMeterZone : LineMeterChild
	{
		public double Start { get; set; }

		public double End { get; set; }

		public bool Clamp { get; set; }

		public Color Color { get; set; }

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			Vector2 start = base.Origin + (double.IsNaN(Start) ? Point(base.Projection.MinValue, clamp: false) : Point(Start, Clamp));
			Vector2 end = base.Origin + (double.IsNaN(End) ? Point(base.Projection.MaxValue, clamp: false) : Point(End, Clamp));
			ShapeExtensions.DrawLine(target.SpriteBatch, start, end, Color, Math.Max(base.Meter.Breadth.X, base.Meter.Breadth.Y), 0f);
		}
	}
}
