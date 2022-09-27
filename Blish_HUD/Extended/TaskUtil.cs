using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Blish_HUD.Extended
{
	public class TaskUtil
	{
		private static Logger Logger = Logger.GetLogger<TaskUtil>();

		public static void CallActionWithTimeout(Action action, Action error, int timeout)
		{
			CancellationTokenSource cancelToken = new CancellationTokenSource();
			CancellationToken token = cancelToken.Token;
			Task<Exception> task = Task.Run(delegate
			{
				try
				{
					Thread currentThread = Thread.CurrentThread;
					using (token.Register(currentThread.Abort))
					{
						action();
					}
					return null;
				}
				catch (Exception result)
				{
					return result;
				}
			}, token);
			if (Task.WaitAny(task, Task.Delay(timeout)) != 0)
			{
				cancelToken.Cancel();
				error();
			}
			else if (task.Result != null)
			{
				Logger.Error(task.Result.Message, new object[1] { task.Result });
			}
		}

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

		public static async Task<(bool, T)> GetJsonResponse<T>(string request, int timeOutSeconds = 10)
		{
			try
			{
				T result;
				return (TryParseJson<T>(await GeneratedExtensions.GetStringAsync(SettingsExtensions.WithTimeout<IFlurlRequest>(SettingsExtensions.AllowHttpStatus<IFlurlRequest>(GeneratedExtensions.AllowHttpStatus(request, new HttpStatusCode[1] { HttpStatusCode.NotFound }), "200"), timeOutSeconds), default(CancellationToken), (HttpCompletionOption)0), out result), result);
			}
			catch (FlurlHttpTimeoutException val)
			{
				FlurlHttpTimeoutException ex4 = val;
				Logger.Warn((Exception)(object)ex4, "Request '" + request + "' timed out.");
			}
			catch (FlurlHttpException val2)
			{
				FlurlHttpException ex3 = val2;
				Logger.Warn((Exception)(object)ex3, "Request '" + request + "' was not successful.");
			}
			catch (JsonReaderException val3)
			{
				JsonReaderException ex2 = val3;
				Logger.Warn((Exception)(object)ex2, "Failed to deserialize requested content from \"" + request + "\"\n" + ((Exception)(object)ex2).StackTrace);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Unexpected error while requesting '" + request + "'.");
			}
			return (false, default(T));
		}
	}
}
