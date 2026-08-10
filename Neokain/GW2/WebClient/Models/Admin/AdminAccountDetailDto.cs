namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminAccountDetailDto : AdminAccountDto
	{
		public int Age { get; set; }

		public string? Commander { get; set; }

		public int FractalLevel { get; set; }

		public int WvwRank { get; set; }

		public int DailyAp { get; set; }

		public int MonthlyAp { get; set; }

		public string? OwnerId { get; set; }
	}
}
