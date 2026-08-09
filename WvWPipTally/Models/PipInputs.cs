namespace WvWPipTally.Models
{
	public sealed class PipInputs
	{
		public int StartingSegmentIndex { get; set; }

		public Placement Placement { get; set; }

		public RankTier Rank { get; set; }

		public bool Commitment { get; set; }

		public bool Commander { get; set; }

		public bool PublicCommander { get; set; }

		public int? ScannedPipsPerTick { get; set; }
	}
}
