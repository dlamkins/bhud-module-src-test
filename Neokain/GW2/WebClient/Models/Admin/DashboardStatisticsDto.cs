namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class DashboardStatisticsDto
	{
		public int TotalUsers { get; set; }

		public int TotalGuilds { get; set; }

		public int TotalAlliances { get; set; }

		public int TotalAccounts { get; set; }

		public int ActiveUsersToday { get; set; }

		public int NewUsersThisWeek { get; set; }

		public string SystemHealth { get; set; } = "healthy";

	}
}
