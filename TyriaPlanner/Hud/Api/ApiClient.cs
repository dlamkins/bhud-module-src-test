using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json;

namespace TyriaPlanner.Hud.Api
{
	public sealed class ApiClient : IDisposable
	{
		private class ExchangeResponse
		{
			public string access_token { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<ApiClient>();

		private readonly HttpClient _http;

		public ApiClient()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			val.set_Timeout(TimeSpan.FromSeconds(15.0));
			_http = val;
			_http.get_DefaultRequestHeaders().get_UserAgent().ParseAdd("TyriaPlanner.Hud/0.2 (Blish HUD)");
			_http.get_DefaultRequestHeaders().get_Accept().Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<string> ExchangeAsync(string baseUrl, string gw2ApiKey, CancellationToken cancel)
		{
			if (string.IsNullOrWhiteSpace(gw2ApiKey) || string.IsNullOrWhiteSpace(baseUrl))
			{
				return null;
			}
			string body = JsonConvert.SerializeObject(new
			{
				gw2_api_key = gw2ApiKey,
				client_id = "tyria-hud-blish",
				label = "Blish HUD"
			});
			HttpRequestMessage req = new HttpRequestMessage(HttpMethod.get_Post(), baseUrl.TrimEnd('/') + "/api/addon/exchange");
			try
			{
				req.set_Content((HttpContent)new StringContent(body, Encoding.UTF8, "application/json"));
				HttpResponseMessage res = await _http.SendAsync(req, (HttpCompletionOption)1, cancel).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					if (res.get_StatusCode() == HttpStatusCode.Unauthorized || res.get_StatusCode() == HttpStatusCode.BadRequest)
					{
						string errBody = await res.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
						Logger.Warn("Tyria Planner addon exchange rejected: {0} {1}", new object[2]
						{
							(int)res.get_StatusCode(),
							errBody
						});
						return null;
					}
					res.EnsureSuccessStatusCode();
					return JsonConvert.DeserializeObject<ExchangeResponse>(await res.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false))?.access_token;
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

		public async Task<UpcomingResponse> FetchBrowseAsync(string baseUrl, string bearer, CancellationToken cancel)
		{
			if (string.IsNullOrWhiteSpace(bearer))
			{
				return null;
			}
			HttpRequestMessage req = new HttpRequestMessage(HttpMethod.get_Get(), baseUrl.TrimEnd('/') + "/api/addon/browse");
			try
			{
				req.get_Headers().set_Authorization(new AuthenticationHeaderValue("Bearer", bearer));
				HttpResponseMessage res = await _http.SendAsync(req, (HttpCompletionOption)1, cancel).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					if (res.get_StatusCode() == HttpStatusCode.Unauthorized || res.get_StatusCode() == HttpStatusCode.Forbidden)
					{
						return null;
					}
					res.EnsureSuccessStatusCode();
					return JsonConvert.DeserializeObject<UpcomingResponse>(await res.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
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

		public async Task<UpcomingResponse> FetchUpcomingAsync(string baseUrl, string bearer, DateTimeOffset? since, CancellationToken cancel)
		{
			if (string.IsNullOrWhiteSpace(bearer))
			{
				return null;
			}
			string url = baseUrl.TrimEnd('/') + "/api/addon/upcoming";
			if (since.HasValue)
			{
				url = url + "?since=" + Uri.EscapeDataString(since.Value.UtcDateTime.ToString("o"));
			}
			HttpRequestMessage req = new HttpRequestMessage(HttpMethod.get_Get(), url);
			try
			{
				req.get_Headers().set_Authorization(new AuthenticationHeaderValue("Bearer", bearer));
				HttpResponseMessage res = await _http.SendAsync(req, (HttpCompletionOption)1, cancel).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					if (res.get_StatusCode() == HttpStatusCode.Unauthorized || res.get_StatusCode() == HttpStatusCode.Forbidden)
					{
						return null;
					}
					res.EnsureSuccessStatusCode();
					return JsonConvert.DeserializeObject<UpcomingResponse>(await res.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
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

		public async Task<bool> CheckinAsync(string baseUrl, string bearer, string eventId, CancellationToken cancel)
		{
			if (string.IsNullOrWhiteSpace(bearer) || string.IsNullOrWhiteSpace(eventId))
			{
				return false;
			}
			string body = JsonConvert.SerializeObject(new
			{
				event_id = eventId
			});
			HttpRequestMessage req = new HttpRequestMessage(HttpMethod.get_Post(), baseUrl.TrimEnd('/') + "/api/addon/checkin");
			try
			{
				req.set_Content((HttpContent)new StringContent(body, Encoding.UTF8, "application/json"));
				req.get_Headers().set_Authorization(new AuthenticationHeaderValue("Bearer", bearer));
				HttpResponseMessage res = await ((HttpMessageInvoker)_http).SendAsync(req, cancel).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					return res.get_IsSuccessStatusCode();
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

		public async Task<PendingApprovalsResponse> FetchApprovalsAsync(string baseUrl, string bearer, CancellationToken cancel)
		{
			if (string.IsNullOrWhiteSpace(bearer))
			{
				return null;
			}
			HttpRequestMessage req = new HttpRequestMessage(HttpMethod.get_Get(), baseUrl.TrimEnd('/') + "/api/addon/approvals");
			try
			{
				req.get_Headers().set_Authorization(new AuthenticationHeaderValue("Bearer", bearer));
				HttpResponseMessage res = await ((HttpMessageInvoker)_http).SendAsync(req, cancel).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					if (!res.get_IsSuccessStatusCode())
					{
						return null;
					}
					return JsonConvert.DeserializeObject<PendingApprovalsResponse>(await res.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
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

		public async Task<(bool ok, int status)> DecideApprovalAsync(string baseUrl, string bearer, string signupId, string decision, CancellationToken cancel)
		{
			if (string.IsNullOrWhiteSpace(bearer) || string.IsNullOrWhiteSpace(signupId))
			{
				return (false, 0);
			}
			string body = JsonConvert.SerializeObject(new { decision });
			string url = baseUrl.TrimEnd('/') + "/api/addon/approvals/" + signupId + "/decide";
			try
			{
				HttpRequestMessage req = new HttpRequestMessage(HttpMethod.get_Post(), url);
				try
				{
					req.set_Content((HttpContent)new StringContent(body, Encoding.UTF8, "application/json"));
					req.get_Headers().set_Authorization(new AuthenticationHeaderValue("Bearer", bearer));
					HttpResponseMessage res = await ((HttpMessageInvoker)_http).SendAsync(req, cancel).ConfigureAwait(continueOnCapturedContext: false);
					try
					{
						int code = (int)res.get_StatusCode();
						if (!res.get_IsSuccessStatusCode())
						{
							string errBody = string.Empty;
							try
							{
								errBody = await res.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
							}
							catch
							{
							}
							Logger.Warn("decide failed · {0} {1} · {2}", new object[3]
							{
								code,
								res.get_ReasonPhrase(),
								errBody
							});
							return (false, code);
						}
						return (true, code);
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
			catch (Exception ex)
			{
				Logger.Warn(ex, "decide threw for {0}", new object[1] { signupId });
				return (false, 0);
			}
		}

		public void Dispose()
		{
			HttpClient http = _http;
			if (http != null)
			{
				((HttpMessageInvoker)http).Dispose();
			}
		}
	}
}
