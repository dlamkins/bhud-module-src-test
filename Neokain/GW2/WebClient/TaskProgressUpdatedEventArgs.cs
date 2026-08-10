using System;
using Neokain.GW2.WebClient.Models.Tasks;

namespace Neokain.GW2.WebClient
{
	public sealed class TaskProgressUpdatedEventArgs : EventArgs
	{
		public Guid TaskId { get; }

		public TaskProgressDto Progress { get; }

		public TaskProgressUpdatedEventArgs(Guid taskId, TaskProgressDto progress)
		{
			TaskId = taskId;
			Progress = progress;
		}
	}
}
