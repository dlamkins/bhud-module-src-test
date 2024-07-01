using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class ArcMeterNeedle : ArcMeterChild
	{
		public double Value { get; set; }

		public float Distance { get; set; }

		public bool Clamp { get; set; }

		public float Length { get; set; }

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
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			Vector2 point = Point(Value, Clamp);
			Vector2 center = base.Center - new Vector2((base.Radii.X + Distance) * point.X, (base.Radii.Y + Distance) * point.Y);
			Vector2 halfLength = default(Vector2);
			((Vector2)(ref halfLength))._002Ector(Length * point.X / 2f, Length * point.Y / 2f);
			ShapeExtensions.DrawLine(target.SpriteBatch, center - halfLength, center + halfLength, Color, Thickness, 0f);
		}
	}
}
