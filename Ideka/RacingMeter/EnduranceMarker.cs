using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class EnduranceMarker : AnchoredRect
	{
		public double Percentage { get; set; }

		public Color Color { get; set; }

		public float Thickness { get; set; }

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			var (start, end) = UIOverlay.GetEndurancePoints(Percentage);
			ShapeExtensions.DrawLine(target.SpriteBatch, start, end, Color, Thickness, 0f);
		}
	}
}
