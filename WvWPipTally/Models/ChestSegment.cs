namespace WvWPipTally.Models
{
	public sealed class ChestSegment
	{
		public string Id { get; }

		public string Division { get; }

		public int SegmentIndex { get; }

		public int Pips { get; }

		public int Tickets { get; }

		public ChestSegment(string id, string division, int segmentIndex, int pips, int tickets)
		{
			Id = id;
			Division = division;
			SegmentIndex = segmentIndex;
			Pips = pips;
			Tickets = tickets;
		}
	}
}
