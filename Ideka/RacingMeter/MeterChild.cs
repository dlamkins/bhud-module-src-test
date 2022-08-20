using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public abstract class MeterChild<T> : RectAnchor where T : RectAnchor, IMeter
	{
		public Projection Projection { get; set; }

		protected T Meter => (T)base.Parent;

		protected abstract Vector2 Point(double value, bool clamp);
	}
}
