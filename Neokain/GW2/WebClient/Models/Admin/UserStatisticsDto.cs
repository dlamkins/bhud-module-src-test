namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class UserStatisticsDto
	{
		public int TotalUsers { get; set; }

		public int ActiveUsers { get; set; }

		public int BlockedUsers { get; set; }

		public int NewUsersToday { get; set; }

		public int NewUsersThisWeek { get; set; }

		public int NewUsersThisMonth { get; set; }
	}
}
