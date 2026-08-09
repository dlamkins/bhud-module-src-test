namespace WvWPipTally.Models
{
	public sealed class PipResults
	{
		public int PipsPerTick { get; set; }

		public int RemainingPips { get; set; }

		public int RemainingTickets { get; set; }

		public int FinishedPips { get; set; }

		public int EarnedTickets { get; set; }

		public int TotalPips { get; set; }

		public int TotalTickets { get; set; }

		public int PipPercent { get; set; }

		public int TicketPercent { get; set; }

		public int TicksRemaining { get; set; }

		public int HoursRemaining { get; set; }

		public int MinutesRemaining { get; set; }
	}
}
