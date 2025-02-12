using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SL.ChatLinks.Logging
{
	public static class LogProcessor
	{
		private static readonly ConcurrentQueue<Action> Work;

		private static readonly SemaphoreSlim QueueSemaphore;

		static LogProcessor()
		{
			Work = new ConcurrentQueue<Action>();
			QueueSemaphore = new SemaphoreSlim(0);
			Task.Factory.StartNew((Func<Task>)ProcessQueue, TaskCreationOptions.LongRunning);
		}

		public static void Enqueue(Action callback)
		{
			Work.Enqueue(callback);
			QueueSemaphore.Release();
		}

		private static async Task ProcessQueue()
		{
			while (true)
			{
				await QueueSemaphore.WaitAsync();
				Action work;
				while (Work.TryDequeue(out work))
				{
					try
					{
						work?.Invoke();
					}
					catch
					{
					}
				}
			}
		}
	}
}
