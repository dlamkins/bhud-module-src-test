using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;

namespace Ideka.RacingMeter
{
	internal static class TaskUtils
	{
		public readonly struct TaskState
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

			public TaskState(Task task, CancellationTokenSource? src)
			{
				Completed = task.IsCompleted;
				Canceled = task.IsCanceled || (src?.IsCancellationRequested ?? false);
				Faulted = task.IsFaulted;
				Exception = task.Exception;
			}
		}

		public static bool IsRunning(CancellationTokenSource? src)
		{
			if (src == null)
			{
				return false;
			}
			return !src!.IsCancellationRequested;
		}

		public static void Cancel(ref CancellationTokenSource? src)
		{
			src?.Cancel();
			src = null;
		}

		public static CancellationToken New(out CancellationTokenSource? src)
		{
			src = new CancellationTokenSource();
			return src!.Token;
		}

		public static Task<TaskState> Done(this Task task, Logger logger, string? failMessage, CancellationTokenSource? src = null)
		{
			Task task2 = task;
			CancellationTokenSource src2 = src;
			Logger logger2 = logger;
			string failMessage2 = failMessage;
			return task2.ContinueWith(delegate
			{
				TaskState result = new TaskState(task2, src2);
				src2?.Cancel();
				if (!result.Canceled && (result.Faulted || !result.Completed))
				{
					FriendlyError.Report(logger2, failMessage2, result.Exception);
				}
				return result;
			});
		}
	}
}
