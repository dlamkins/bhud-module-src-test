using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public abstract class MeterChild<T> : RectAnchor where T : RectAnchor, IMeter, new()
	{
		private static readonly T EmptyMeter = new T();

		public Projection Projection { get; set; } = new Projection();


		protected T Meter => (base.Parent as T) ?? EmptyMeter;

		protected abstract Vector2 Point(double value, bool clamp);
	}
}
