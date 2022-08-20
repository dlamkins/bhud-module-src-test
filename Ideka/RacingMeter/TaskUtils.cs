using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;

namespace Ideka.RacingMeter
{
	internal static class TaskUtils
	{
		public class TaskState
		{
			public bool Completed { get; }

			public bool Canceled { get; }

			public bool Faulted { get; }

			public bool Success
			{
				get
				{
					if (Completed && !Canceled)
					{
						return !Faulted;
					}
					return false;
				}
			}

			public Exception Exception { get; }

			public TaskState(Task task, CancellationTokenSource src)
			{
				Completed = task.IsCompleted;
				Canceled = task.IsCanceled || (src?.IsCancellationRequested ?? false);
				Faulted = task.IsFaulted;
				Exception = task.Exception;
			}
		}

		public static bool IsRunning(CancellationTokenSource src)
		{
			if (src == null)
			{
				return false;
			}
			return !src.IsCancellationRequested;
		}

		public static void Cancel(ref CancellationTokenSource src)
		{
			src?.Cancel();
			src = null;
		}

		public static CancellationToken New(ref CancellationTokenSource src)
		{
			src = new CancellationTokenSource();
			return src.Token;
		}

		public static Task<TaskState> Done(this Task task, Logger logger, string failMessage, CancellationTokenSource src = null)
		{
			return task.ContinueWith(delegate
			{
				TaskState taskState = new TaskState(task, src);
				src?.Cancel();
				if (!taskState.Canceled && (taskState.Faulted || !taskState.Completed))
				{
					FriendlyError.Report(logger, failMessage, taskState.Exception);
				}
				return taskState;
			});
		}
	}
}
