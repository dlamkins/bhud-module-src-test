using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class ArcMeterOrb : ArcMeterChild
	{
		public double Value { get; set; }

		public bool Clamp { get; set; }

		public float Distance { get; set; }

		public float Radius { get; set; }

		public float Thickness { get; set; }

		public Color Color { get; set; }

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			Vector2 point = Point(Value, Clamp);
			Vector2 center = base.Center - new Vector2((base.Radii.X + Distance) * point.X, (base.Radii.Y + Distance) * point.Y);
			ShapeExtensions.DrawCircle(target.SpriteBatch, center, Radius, SpriteBatchExtensions.GetSidesFor(Radius, 2.0), Color, Thickness, 0f);
		}
	}
}
