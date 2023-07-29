using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nekres.ProofLogix.Core.Utils
{
	internal static class HttpUtil
	{
		public static bool TryParseJson<T>(string json, out T result)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			bool success = true;
			JsonSerializerSettings val = new JsonSerializerSettings();
			val.set_Error((EventHandler<ErrorEventArgs>)delegate(object _, ErrorEventArgs args)
			{
				success = false;
				args.get_ErrorContext().set_Handled(true);
			});
			val.set_MissingMemberHandling((MissingMemberHandling)1);
			JsonSerializerSettings settings = val;
			result = JsonConvert.DeserializeObject<T>(json, settings);
			return success;
		}

		public static async Task<T> RetryAsync<T>(Func<Task<HttpResponseMessage>> request, int retries = 2, int delayMs = 2000, Logger logger = null)
		{
			return await RetryAsync(HttpResponseWrapper, retries, delayMs, logger);
			async Task<T> HttpResponseWrapper()
			{
				return JsonConvert.DeserializeObject<T>(await (await request()).get_Content().ReadAsStringAsync());
			}
		}

		public static async Task<T> RetryAsync<T>(Func<Task<T>> request, int retries = 2, int delayMs = 2000, Logger logger = null)
		{
			if (logger == null)
			{
				logger = Logger.GetLogger(typeof(HttpUtil));
			}
			try
			{
				return await request();
			}
			catch (Exception e)
			{
				if (retries > 0)
				{
					logger.Warn(e, $"Failed to request data. Retrying in {delayMs / 1000} second(s) (remaining retries: {retries}).");
					await Task.Delay(delayMs);
					return await RetryAsync(request, retries - 1, delayMs, logger);
				}
				if (!(e is FlurlHttpTimeoutException))
				{
					if (!(e is FlurlHttpException))
					{
						if (e is JsonReaderException)
						{
							logger.Warn(e, e.Message);
						}
						else
						{
							logger.Error(e, e.Message);
						}
					}
					else
					{
						logger.Warn(e, e.Message);
					}
				}
				else
				{
					logger.Warn(e, e.Message);
				}
			}
			return default(T);
		}
	}
}
