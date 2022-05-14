using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Blish_HUD;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	public static class RetryHelper
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(RetryHelper));

		public static async Task RetryOnExceptionAsync(Func<Task> operation, int maxAttempts = 3)
		{
			await RetryOnExceptionAsync<Exception>(operation, maxAttempts);
		}

		public static async Task RetryOnExceptionAsync<TException>(Func<Task> operation, int maxAttempts = 3) where TException : Exception
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
					Logger.Debug($"Exception on attempt {attempt} of {maxAttempts}.", new object[1] { ex });
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
			return Task.Delay(IncreasingDelayInSeconds(attemptNumber));
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
