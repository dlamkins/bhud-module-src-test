using System;

namespace Neokain.GW2.WebClient.Models.Tasks
{
	public class TaskDto
	{
		public Guid Id { get; set; }

		public string TaskType { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? StartedAt { get; set; }

		public DateTimeOffset? CompletedAt { get; set; }

		public Guid? AccountId { get; set; }

		public Guid? GuildId { get; set; }

		public string? WebUserId { get; set; }

		public bool Finished { get; set; }

		public bool? Success { get; set; }

		public string? Error { get; set; }

		public int? Progress { get; set; }

		public string? CurrentAction { get; set; }

		public string? CurrentPhase { get; set; }

		public bool Cancelled { get; set; }

		public int Priority { get; set; }

		public int RetryCount { get; set; }

		public int MaxRetries { get; set; }

		public string? Result { get; set; }

		public DateTimeOffset? NextRetryAt { get; set; }
	}
}
