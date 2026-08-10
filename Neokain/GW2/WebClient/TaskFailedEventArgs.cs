using System;
using Neokain.GW2.WebClient.Models.Tasks;

namespace Neokain.GW2.WebClient
{
	public sealed class TaskFailedEventArgs : EventArgs
	{
		public Guid TaskId { get; }

		public TaskDto Task { get; }

		public TaskFailedEventArgs(Guid taskId, TaskDto task)
		{
			TaskId = taskId;
			Task = task;
		}
	}
}
