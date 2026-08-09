namespace WvWPipTally.Models
{
	public sealed class ScanResult
	{
		public bool Success { get; set; }

		public string Message { get; set; }

		public string RawText { get; set; }

		public string Division { get; set; }

		public int? RewardNumber { get; set; }

		public int? RewardsInDivision { get; set; }

		public int? StartingSegmentIndex { get; set; }

		public int? TickPrediction { get; set; }

		public int? PreviousTick { get; set; }
	}
}
