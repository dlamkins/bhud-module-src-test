using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using TyriaPlanner.Hud.Settings;

namespace TyriaPlanner.Hud.Services
{
	public sealed class StreamSubscriber : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<StreamSubscriber>();

		private readonly ModuleSettings _settings;

		private readonly Action _onRefresh;

		private CancellationTokenSource _cancel;

		private Task _loop;

		public StreamSubscriber(ModuleSettings settings, Action onRefresh)
		{
			_settings = settings;
			_onRefresh = onRefresh;
		}

		public void Start()
		{
			Stop();
			_cancel = new CancellationTokenSource();
			_loop = Task.Run(() => RunAsync(_cancel.Token));
		}

		public void Stop()
		{
			if (_cancel != null)
			{
				_cancel.Cancel();
				_cancel.Dispose();
				_cancel = null;
			}
			_loop = null;
		}

		private async Task RunAsync(CancellationToken cancel)
		{
			int backoffSeconds = 5;
			while (!cancel.IsCancellationRequested)
			{
				try
				{
					await ListenOnceAsync(cancel).ConfigureAwait(continueOnCapturedContext: false);
					backoffSeconds = 5;
				}
				catch (TaskCanceledException)
				{
					return;
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Stream disconnected; reconnecting in {0}s", new object[1] { backoffSeconds });
				}
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(backoffSeconds), cancel).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (TaskCanceledException)
				{
					return;
				}
				backoffSeconds = Math.Min(backoffSeconds * 2, 300);
			}
		}

		private async Task ListenOnceAsync(CancellationToken cancel)
		{
			string baseUrl = _settings.ApiBaseUrl.get_Value();
			string bearer = _settings.CachedBearer.get_Value();
			if (string.IsNullOrWhiteSpace(bearer) || string.IsNullOrWhiteSpace(baseUrl))
			{
				await Task.Delay(TimeSpan.FromSeconds(15.0), cancel).ConfigureAwait(continueOnCapturedContext: false);
				return;
			}
			HttpClient val = new HttpClient();
			val.set_Timeout(Timeout.InfiniteTimeSpan);
			HttpClient http = val;
			try
			{
				http.get_DefaultRequestHeaders().get_UserAgent().ParseAdd("TyriaPlanner.Hud-Stream/0.9 (Blish HUD)");
				http.get_DefaultRequestHeaders().set_Authorization(new AuthenticationHeaderValue("Bearer", bearer));
				HttpRequestMessage req = new HttpRequestMessage(HttpMethod.get_Get(), baseUrl.TrimEnd('/') + "/api/addon/stream");
				try
				{
					req.get_Headers().get_Accept().Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
					HttpResponseMessage res = await http.SendAsync(req, (HttpCompletionOption)1, cancel).ConfigureAwait(continueOnCapturedContext: false);
					try
					{
						if (res.get_StatusCode() == HttpStatusCode.Unauthorized || res.get_StatusCode() == HttpStatusCode.Forbidden)
						{
							Logger.Warn("Stream auth rejected ({0}); polling fallback will keep working.", new object[1] { res.get_StatusCode() });
							await Task.Delay(TimeSpan.FromSeconds(60.0), cancel).ConfigureAwait(continueOnCapturedContext: false);
							return;
						}
						res.EnsureSuccessStatusCode();
						using Stream stream = await res.get_Content().ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
						using StreamReader reader = new StreamReader(stream);
						string currentEvent = null;
						string line = default(string);
						while (true)
						{
							bool flag = !cancel.IsCancellationRequested;
							if (flag)
							{
								flag = (line = await reader.ReadLineAsync().ConfigureAwait(continueOnCapturedContext: false)) != null;
							}
							if (!flag)
							{
								break;
							}
							if (line.StartsWith(":"))
							{
								continue;
							}
							if (line.StartsWith("event:"))
							{
								currentEvent = line.Substring(6).Trim();
							}
							else
							{
								if (!line.StartsWith("data:"))
								{
									continue;
								}
								if (currentEvent == "refresh")
								{
									try
									{
										_onRefresh?.Invoke();
									}
									catch (Exception ex)
									{
										Logger.Warn(ex, "Stream callback threw.");
									}
								}
								currentEvent = null;
							}
						}
					}
					finally
					{
						((IDisposable)res)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)req)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)http)?.Dispose();
			}
		}

		public void Dispose()
		{
			Stop();
		}
	}
}
