using System;

namespace Neokain.GW2.WebClient.Models.Tasks
{
	internal class CreateTaskRequestDto
	{
		public string TaskType { get; set; }

		public Guid? AccountId { get; set; }

		public object? Payload { get; set; }

		public int Priority { get; set; }
	}
}
