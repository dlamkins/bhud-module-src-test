using System;
using System.Threading.Tasks;
using Gw2Sharp.WebApi.Exceptions;

namespace Blish_HUD.Extended
{
	public class TaskUtil
	{
		public static async Task<T> RetryAsync<T>(Func<Task<T>> func, int retries = 2, int delayMs = 30000, Logger logger = null)
		{
			if (logger == null)
			{
				logger = Logger.GetLogger<TaskUtil>();
			}
			T result = default(T);
			object obj;
			int num;
			try
			{
				result = await func();
				return result;
			}
			catch (Exception ex)
			{
				obj = ex;
				num = 1;
			}
			if (num != 1)
			{
				return result;
			}
			Exception e = (Exception)obj;
			if (e is NotFoundException || e is BadRequestException || e is AuthorizationRequiredException)
			{
				logger.Trace(e, e.Message);
				return default(T);
			}
			if (retries > 0)
			{
				logger.Warn(e, $"Failed to pull data from the GW2 API. Retrying in {delayMs / 1000} second(s) (remaining retries: {retries}).");
				await Task.Delay(delayMs);
				return await RetryAsync(func, retries - 1, delayMs, logger);
			}
			if (!(e is TooManyRequestsException))
			{
				if (e is RequestException || e is RequestException<string>)
				{
					logger.Trace(e, e.Message);
				}
				else
				{
					logger.Error(e, e.Message);
				}
			}
			else
			{
				logger.Warn(e, "After multiple attempts no data could be loaded due to being rate limited by the API.");
			}
			return default(T);
		}

		public static async Task<T> TryAsync<T>(Func<Task<T>> func, Logger logger = null)
		{
			if (logger == null)
			{
				logger = Logger.GetLogger<TaskUtil>();
			}
			try
			{
				return await func();
			}
			catch (Exception e)
			{
				if (!(e is NotFoundException) && !(e is BadRequestException) && !(e is AuthorizationRequiredException))
				{
					if (!(e is TooManyRequestsException))
					{
						if (e is RequestException || e is RequestException<string>)
						{
							logger.Trace(e, e.Message);
						}
						else
						{
							logger.Error(e, e.Message);
						}
					}
					else
					{
						logger.Warn(e, "No data could be loaded due to being rate limited by the API.");
					}
				}
				else
				{
					logger.Trace(e, e.Message);
				}
				return default(T);
			}
		}
	}
}
