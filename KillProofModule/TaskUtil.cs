using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;

namespace KillProofModule
{
	internal static class TaskUtil
	{
		public static async Task<(bool, T)> GetJsonResponse<T>(string request)
		{
			try
			{
				return (true, JsonConvert.DeserializeObject<T>(await GeneratedExtensions.GetStringAsync(GeneratedExtensions.AllowHttpStatus(request, new HttpStatusCode[1] { HttpStatusCode.NotFound }), default(CancellationToken), (HttpCompletionOption)0)));
			}
			catch (FlurlHttpTimeoutException val)
			{
				FlurlHttpTimeoutException ex4 = val;
				KillProofModule.Logger.Warn((Exception)(object)ex4, "Request '" + request + "' timed out.");
			}
			catch (FlurlHttpException val2)
			{
				FlurlHttpException ex3 = val2;
				KillProofModule.Logger.Warn((Exception)(object)ex3, "Request '" + request + "' was not successful.");
			}
			catch (JsonReaderException val3)
			{
				JsonReaderException ex2 = val3;
				KillProofModule.Logger.Warn((Exception)(object)ex2, "Failed to read JSON response returned by request '" + request + "'.");
			}
			catch (Exception ex)
			{
				KillProofModule.Logger.Error(ex, "Unexpected error while requesting '" + request + "'.");
			}
			return (false, default(T));
		}
	}
}
