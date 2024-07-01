using System;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class ArcMeter : AnchoredRectMeter
	{
		public double ArcAmplitude { get; set; } = 0.6;


		public double MinArcAngle => (1.0 - ArcAmplitude) / 2.0;

		public double MeterMargin { get; set; } = 0.018;


		public override double FullPortion { get; protected set; }

		public override double MinProjected { get; protected set; }

		public Vector2 Center { get; private set; }

		public Vector2 Radii { get; private set; }

		public static void GetArc(RectangleF target, double min, out Vector2 center, out Vector2 radii)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			double minRadians = min * Math.PI;
			float rx = (float)((double)target.Width / Math.Cos(minRadians) / 2.0);
			float ry = (float)((double)target.Height / (1.0 - Math.Sin(minRadians)));
			center = Point2.op_Implicit(((RectangleF)(ref target)).get_TopLeft() + new Vector2(target.Width / 2f, ry));
			radii = new Vector2(rx, ry);
		}

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			FullPortion = ArcAmplitude - MeterMargin * 2.0;
			MinProjected = (1.0 - FullPortion) / 2.0;
			GetArc(target.Rect, MinArcAngle, out var center, out var radii);
			Center = center;
			Radii = radii;
		}
	}
}
