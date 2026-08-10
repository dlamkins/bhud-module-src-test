using System;

namespace Neokain.GW2.WebClient.Models.Tasks
{
	internal class CreateTaskResponseDto
	{
		public Guid Id { get; set; }

		public string TaskType { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public bool Finished { get; set; }

		public int? Progress { get; set; }
	}
}
