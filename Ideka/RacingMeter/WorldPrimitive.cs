using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public abstract class WorldPrimitive : RectAnchor
	{
		public Vector3 WorldPosition { get; set; }

		public float Scale { get; set; } = 1f;


		public float Thickness { get; set; }

		public Color Color { get; set; }

		public abstract Primitive Primitive { get; }

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Matrix trs = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(WorldPosition);
			Primitive.Transformed(trs).ToScreen().Draw(target.SpriteBatch, Color, Thickness);
		}
	}
}
