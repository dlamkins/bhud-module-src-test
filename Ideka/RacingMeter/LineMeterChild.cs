using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public abstract class LineMeterChild : MeterChild<LineMeter>
	{
		protected Vector2 Origin => base.Meter.Origin;

		protected override Vector2 Point(double value, bool clamp = false)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			double size = base.Projection.Point(base.Meter, value, clamp);
			return new Vector2((float)((double)base.Meter.Full.X * size), (float)((double)base.Meter.Full.Y * size));
		}

		public RectangleF ThicknessRect(Vector2 position, float thickness)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			Vector2 breadth = base.Meter.GetBreadth(thickness);
			return new RectangleF(Point2.op_Implicit(position - breadth / 2f), Size2.op_Implicit(breadth));
		}
	}
}
