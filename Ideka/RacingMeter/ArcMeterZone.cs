namespace Ideka.RacingMeter
{
	public class ArcMeterZone : ArcMeterAbstractZone
	{
		public double Start { get; set; }

		public double End { get; set; }

		protected override void EarlyDraw(RectTarget target)
		{
			DrawZone(target, Start, End);
		}
	}
}
