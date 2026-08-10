using System;
using Neokain.GW2.WebClient.Models.Tasks;

namespace Neokain.GW2.WebClient
{
	public sealed class TaskCompletedEventArgs : EventArgs
	{
		public Guid TaskId { get; }

		public TaskDto Task { get; }

		public TaskCompletedEventArgs(Guid taskId, TaskDto task)
		{
			TaskId = taskId;
			Task = task;
		}
	}
}
