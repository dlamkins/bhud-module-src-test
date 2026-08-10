using System;

namespace Neokain.GW2.WebClient
{
	public sealed class TaskCancelledEventArgs : EventArgs
	{
		public Guid TaskId { get; }

		public TaskCancelledEventArgs(Guid taskId)
		{
			TaskId = taskId;
		}
	}
}
