using System;

namespace Neokain.GW2.WebClient.Models.Tasks
{
	public class TaskProgressDto
	{
		public Guid TaskId { get; set; }

		public int ProgressPercent { get; set; }

		public string StatusMessage { get; set; } = string.Empty;


		public DateTimeOffset Timestamp { get; set; }
	}
}
