using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;

namespace Nekres.Stream_Out
{
	internal static class TaskUtil
	{
		public static async Task<(bool, T)> GetJsonResponse<T>(string request)
		{
			try
			{
				return (true, JsonConvert.DeserializeObject<T>(await request.AllowHttpStatus(HttpStatusCode.NotFound).GetStringAsync(default(CancellationToken), (HttpCompletionOption)0)));
			}
			catch (FlurlHttpTimeoutException ex5)
			{
				FlurlHttpTimeoutException ex4 = ex5;
				StreamOutModule.Logger.Warn((Exception)ex4, "Request '" + request + "' timed out.");
			}
			catch (FlurlHttpException ex6)
			{
				FlurlHttpException ex3 = ex6;
				StreamOutModule.Logger.Warn((Exception)ex3, "Request '" + request + "' was not successful.");
			}
			catch (JsonReaderException val)
			{
				JsonReaderException val2 = val;
				JsonReaderException ex2 = val2;
				StreamOutModule.Logger.Warn((Exception)(object)ex2, "Failed to read JSON response returned by request '" + request + "'.");
			}
			catch (Exception ex7)
			{
				Exception ex = ex7;
				StreamOutModule.Logger.Error(ex, $"Unexpected error while requesting '{request}': '{ex.GetType()}'");
			}
			return (false, default(T));
		}
	}
}
