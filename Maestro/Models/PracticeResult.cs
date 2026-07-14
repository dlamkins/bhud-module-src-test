namespace Maestro.Models
{
	public class PracticeResult
	{
		public int PerfectCount { get; set; }

		public int GoodCount { get; set; }

		public int MissCount { get; set; }

		public int WrongCount { get; set; }

		public int MaxCombo { get; set; }

		public int TotalNotes => PerfectCount + GoodCount + MissCount;

		public double AccuracyPercent
		{
			get
			{
				if (TotalNotes != 0)
				{
					return ((double)PerfectCount * 100.0 + (double)GoodCount * 60.0) / (double)TotalNotes;
				}
				return 0.0;
			}
		}
	}
}
