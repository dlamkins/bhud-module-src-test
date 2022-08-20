namespace Ideka.RacingMeter
{
	public class ArcMeterMargins : ArcMeterAbstractZone
	{
		protected override void EarlyDraw(RectTarget target)
		{
			DrawZone(target, double.NaN, base.Projection.MinValue);
			DrawZone(target, base.Projection.MaxValue, double.NaN);
		}
	}
}
