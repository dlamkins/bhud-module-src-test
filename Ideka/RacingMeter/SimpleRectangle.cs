using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class SimpleRectangle : AnchoredRect
	{
		public float Thickness { get; set; }

		public Color? Color { get; set; }

		public Color? FillColor { get; set; }

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (FillColor.HasValue)
			{
				target.SpriteBatch.DrawRectangleFill(target.Rect, FillColor.Value);
			}
			if (Color.HasValue)
			{
				ShapeExtensions.DrawRectangle(target.SpriteBatch, target.Rect, Color.Value, Thickness, 0f);
			}
		}
	}
}
