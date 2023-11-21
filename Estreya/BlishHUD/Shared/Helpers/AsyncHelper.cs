using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estreya.BlishHUD.Shared.Helpers
{
	public static class AsyncHelper
	{
		private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

		public static TResult RunSync<TResult>(Func<Task<TResult>> func)
		{
			return _myTaskFactory.StartNew(func).Unwrap().GetAwaiter()
				.GetResult();
		}

		public static void RunSync(Func<Task> func)
		{
			_myTaskFactory.StartNew(func).Unwrap().GetAwaiter()
				.GetResult();
		}

		public static async Task WaitUntil(Func<bool> func, TimeSpan timeout, int waitInterval = 1000)
		{
			await WaitUntil(() => Task.FromResult(func()), timeout, waitInterval);
		}

		public static async Task WaitUntil(Func<Task<bool>> func, TimeSpan timeout, int waitInterval = 1000)
		{
			DateTimeOffset start = DateTimeOffset.UtcNow;
			bool condition;
			do
			{
				condition = await func();
				if (!condition)
				{
					await Task.Delay(waitInterval);
				}
			}
			while (!condition && start + timeout >= DateTimeOffset.UtcNow);
			if (!condition && start + timeout >= DateTimeOffset.UtcNow)
			{
				throw new TimeoutException("The condition has not been resolved in the given timespan.");
			}
		}

		public static async Task<T> WaitForReference<T>(Func<T> func, TimeSpan timeout, int waitInterval = 1000) where T : class
		{
			return await WaitForReference(() => Task.FromResult(func()), timeout, waitInterval);
		}

		public static async Task<T> WaitForReference<T>(Func<Task<T>> func, TimeSpan timeout, int waitInterval = 1000) where T : class
		{
			DateTimeOffset start = DateTimeOffset.UtcNow;
			T reference;
			do
			{
				reference = await func();
				if (reference == null)
				{
					await Task.Delay(waitInterval);
				}
			}
			while (reference == null && start + timeout >= DateTimeOffset.UtcNow);
			if (reference == null && start + timeout >= DateTimeOffset.UtcNow)
			{
				throw new TimeoutException("The reference has not been resolved in the given timespan.");
			}
			return reference;
		}
	}
}
