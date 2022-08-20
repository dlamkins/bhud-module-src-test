using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class LineMeterNeedle : LineMeterChild, IProjectionAnchor
	{
		public double Value { get; set; }

		public bool Clamp { get; set; }

		public float Thickness { get; set; }

		public Color Color { get; set; }

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Vector2 point = Point(Value, Clamp);
			Vector2 position = base.Origin + point;
			target.SpriteBatch.DrawRectangleFill(ThicknessRect(position, Thickness), Color);
		}
	}
}
