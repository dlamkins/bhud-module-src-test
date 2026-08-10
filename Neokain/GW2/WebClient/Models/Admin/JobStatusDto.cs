using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class JobStatusDto
	{
		public string Name { get; set; }

		public string Status { get; set; } = "idle";


		public DateTimeOffset? LastRun { get; set; }

		public DateTimeOffset? NextRun { get; set; }

		public string? LastError { get; set; }

		public int RunCount { get; set; }
	}
}
