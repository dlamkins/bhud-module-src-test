namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class SystemHealthDto
	{
		public string Status { get; set; } = "healthy";


		public long UptimeSeconds { get; set; }

		public string? GitCommit { get; set; }

		public string? BuildTime { get; set; }

		public ComponentHealthDto Database { get; set; } = new ComponentHealthDto();


		public ComponentHealthDto Cache { get; set; } = new ComponentHealthDto();


		public ComponentHealthDto Gw2Api { get; set; } = new ComponentHealthDto();


		public int RunningTasks { get; set; }

		public bool IsShuttingDown { get; set; }
	}
}
