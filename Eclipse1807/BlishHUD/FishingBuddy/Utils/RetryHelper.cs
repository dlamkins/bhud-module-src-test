using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Blish_HUD;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	public static class RetryHelper
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(RetryHelper));

		public static async Task RetryAsync<TType>(int maxAttempts, Task<TType> task, Func<Task> operation, Func<bool> resultExpression)
		{
			if (maxAttempts <= 0)
			{
				throw new ArgumentOutOfRangeException("maxAttempts");
			}
			int attempt = 1;
			do
			{
				Logger.Debug("Attempt #" + attempt);
				try
				{
					await operation();
					if (resultExpression())
					{
						return;
					}
				}
				catch (Exception)
				{
					if (attempt == maxAttempts)
					{
						throw;
					}
				}
				await CreateDelay(attempt, maxAttempts);
				attempt++;
			}
			while (attempt != maxAttempts);
		}

		private static Task CreateDelay(int attempt, int maxAttempts)
		{
			int delaytime = IncreasingDelayInSeconds(attempt);
			Logger.Debug($"Failed result expression on attempt {attempt} of {maxAttempts}. Will retry after sleeping for {delaytime}.");
			return Task.Delay(delaytime);
		}

		public static async Task RetryOnExceptionAsync(int times, Func<Task> operation)
		{
			await RetryOnExceptionAsync<Exception>(times, operation);
		}

		public static async Task RetryOnExceptionAsync<TException>(int maxAttempts, Func<Task> operation) where TException : Exception
		{
			if (maxAttempts <= 0)
			{
				throw new ArgumentOutOfRangeException("maxAttempts");
			}
			int attempt = 1;
			object obj;
			while (true)
			{
				int num;
				try
				{
					await operation();
					return;
				}
				catch (TException val)
				{
					obj = (TException)val;
					num = 1;
				}
				if (num == 1)
				{
					TException ex = (TException)obj;
					if (attempt == maxAttempts)
					{
						Exception obj2 = obj as Exception;
						if (obj2 == null)
						{
							break;
						}
						ExceptionDispatchInfo.Capture(obj2).Throw();
					}
					await CreateDelayForException(attempt, maxAttempts, ex);
				}
				attempt++;
			}
			throw obj;
		}

		private static Task CreateDelayForException(int attemptNumber, int maxAttempts, Exception ex)
		{
			int delaytime = IncreasingDelayInSeconds(attemptNumber);
			Logger.Debug($"Exception on attempt {attemptNumber} of {maxAttempts}. Will retry after sleeping for {delaytime}.", new object[1] { ex });
			return Task.Delay(delaytime);
		}

		private static int IncreasingDelayInSeconds(int failedAttempt)
		{
			int delaySeconds = 10;
			if (failedAttempt <= 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return failedAttempt * delaySeconds;
		}
	}
}
