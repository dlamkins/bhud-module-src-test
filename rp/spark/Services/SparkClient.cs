using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using rp.spark.Models;
using rp.spark.Models.Api;

namespace rp.spark.Services
{
	public class SparkClient
	{
		private sealed class InvalidApiResponseException : Exception
		{
			public HttpStatusCode StatusCode { get; }

			public InvalidApiResponseException(HttpStatusCode statusCode, Exception innerException)
				: base("The API response contained invalid JSON.", innerException)
			{
				StatusCode = statusCode;
			}
		}

		private sealed class ResponseBodyTooLargeException : Exception
		{
			public HttpStatusCode StatusCode { get; }

			public ResponseBodyTooLargeException(HttpStatusCode statusCode)
			{
				StatusCode = statusCode;
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<SparkClient>();

		private const string ServerAction = "connect to the SPARK webserver";

		private const string ServerUnavailableMessage = "Cannot connect to the SPARK webserver.";

		private const string ServerInvalidResponseMessage = "The SPARK webserver returned a response SPARK could not read.";

		private const string ServerEmptyResponseMessage = "The SPARK webserver returned an empty response.";

		private const string ServerTimeoutMessage = "The SPARK webserver did not respond in time.";

		private const string ServerInvalidRequestMessage = "SPARK could not prepare the request.";

		private const string SubtokenHeader = "X-GW2-Subtoken";

		private const int MaxResponseBodySize = 1048576;

		private const int ResponseReadBufferSize = 8192;

		private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(10.0);

		private static readonly HttpClient SharedHttpClient;

		private static readonly JsonSerializerSettings JsonSettings;

		private Uri _baseUri;

		private static readonly char[] LogPathSeparators;

		public bool IsConfigured => _baseUri != null;

		public SparkClient(string baseUrl)
		{
			SetBaseUrl(baseUrl);
		}

		public void SetBaseUrl(string baseUrl)
		{
			_baseUri = ((!string.IsNullOrWhiteSpace(baseUrl) && Uri.TryCreate(baseUrl.Trim().TrimEnd('/') + "/", UriKind.Absolute, out var uri)) ? uri : null);
		}

		public Task<ApiResult<bool>> PublishPresenceResultAsync(PlayerPresence presence, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			return PostAsync("presence/", new PresencePublishRequest
			{
				Presence = presence
			}, cancellationToken, gw2Subtoken);
		}

		public Task<ApiResult<PresenceListResponse>> ListPresenceResultAsync(ProfileRegion region, bool includeMature, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			string path = "presence/?region=" + Uri.EscapeDataString(region.ToString()) + "&includeMature=" + includeMature.ToString().ToLowerInvariant();
			return GetAsync<PresenceListResponse>(path, cancellationToken, gw2Subtoken);
		}

		public Task<ApiResult<bool>> UploadProfileResultAsync(CharacterProfile profile, PlayerPresence presence, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			return PostAsync("profiles/", ProfileUploadRequest.FromProfile(profile, presence), cancellationToken, gw2Subtoken);
		}

		public Task<ApiResult<ProfileDownload>> DownloadProfileResultAsync(string accountName, string officialCharacterName, string profileId, bool includeMature, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			string path = "profiles/?account=" + Uri.EscapeDataString(accountName ?? string.Empty) + "&character=" + Uri.EscapeDataString(officialCharacterName ?? string.Empty) + "&profileId=" + Uri.EscapeDataString(profileId ?? string.Empty) + "&includeMature=" + includeMature.ToString().ToLowerInvariant();
			return GetAsync<ProfileDownload>(path, cancellationToken, gw2Subtoken);
		}

		public Task<ApiResult<bool>> BlockAccountResultAsync(string accountName, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			return PostAsync("blocks/", new AccountBlockRequest
			{
				AccountName = (accountName ?? string.Empty)
			}, cancellationToken, gw2Subtoken);
		}

		public Task<ApiResult<bool>> UnblockAccountResultAsync(string accountName, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			string path = "blocks/?account=" + Uri.EscapeDataString(accountName ?? string.Empty);
			return DeleteAsync(path, cancellationToken, gw2Subtoken);
		}

		public Task<ApiResult<bool>> ReplaceBlocklistResultAsync(IEnumerable<string> accountNames, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			return PutAsync("blocks/", new BlocklistRequest
			{
				AccountNames = ((accountNames == null) ? new List<string>() : new List<string>(accountNames))
			}, cancellationToken, gw2Subtoken);
		}

		public Task<ApiResult<ProfileReportResponse>> ReportProfileResultAsync(ProfileReportRequest report, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			return PostAsync<ProfileReportRequest, ProfileReportResponse>("reports/", report, cancellationToken, gw2Subtoken);
		}

		private async Task<ApiResult<T>> GetAsync<T>(string relativePath, CancellationToken cancellationToken, string gw2Subtoken) where T : class
		{
			if (!IsConfigured)
			{
				return ApiResult<T>.Failure("SPARK webserver is misconfigured or down.", null, ApiFailure.NotConfigured);
			}
			return await ExecuteRequestAsync(HttpMethod.get_Get(), relativePath, cancellationToken, async delegate(CancellationToken requestToken)
			{
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Get(), GetUri(relativePath));
				try
				{
					AddSubtokenHeader(request, gw2Subtoken);
					HttpResponseMessage response = await SharedHttpClient.SendAsync(request, (HttpCompletionOption)1, requestToken);
					try
					{
						return await ReadJsonResponseAsync<T>(HttpMethod.get_Get(), relativePath, response, requestToken);
					}
					finally
					{
						((IDisposable)response)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
			});
		}

		private async Task<ApiResult<bool>> PostAsync<T>(string relativePath, T payload, CancellationToken cancellationToken, string gw2Subtoken)
		{
			if (!IsConfigured)
			{
				return ApiResult<bool>.Failure("SPARK webserver is not configured.", null, ApiFailure.NotConfigured);
			}
			if (payload == null)
			{
				return ApiResult<bool>.Failure("SPARK request is empty.", null, ApiFailure.InvalidRequest);
			}
			return await ExecuteRequestAsync(HttpMethod.get_Post(), relativePath, cancellationToken, async delegate(CancellationToken requestToken)
			{
				string json = JsonConvert.SerializeObject((object)payload, (Formatting)0, JsonSettings);
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Post(), GetUri(relativePath));
				try
				{
					StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
					try
					{
						request.set_Content((HttpContent)(object)content);
						AddSubtokenHeader(request, gw2Subtoken);
						HttpResponseMessage response = await SharedHttpClient.SendAsync(request, (HttpCompletionOption)1, requestToken);
						try
						{
							return await ReadStatusOnlyResponseAsync(HttpMethod.get_Post(), relativePath, response, requestToken);
						}
						finally
						{
							((IDisposable)response)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)content)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
			});
		}

		private async Task<ApiResult<TResponse>> PostAsync<TPayload, TResponse>(string relativePath, TPayload payload, CancellationToken cancellationToken, string gw2Subtoken) where TResponse : class
		{
			if (!IsConfigured)
			{
				return ApiResult<TResponse>.Failure("SPARK webserver is not configured.", null, ApiFailure.NotConfigured);
			}
			if (payload == null)
			{
				return ApiResult<TResponse>.Failure("SPARK request is empty.", null, ApiFailure.InvalidRequest);
			}
			return await ExecuteRequestAsync(HttpMethod.get_Post(), relativePath, cancellationToken, async delegate(CancellationToken requestToken)
			{
				string json = JsonConvert.SerializeObject((object)payload, (Formatting)0, JsonSettings);
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Post(), GetUri(relativePath));
				try
				{
					StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
					try
					{
						request.set_Content((HttpContent)(object)content);
						AddSubtokenHeader(request, gw2Subtoken);
						HttpResponseMessage response = await SharedHttpClient.SendAsync(request, (HttpCompletionOption)1, requestToken);
						try
						{
							return await ReadJsonResponseAsync<TResponse>(HttpMethod.get_Post(), relativePath, response, requestToken);
						}
						finally
						{
							((IDisposable)response)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)content)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
			});
		}

		private async Task<ApiResult<bool>> PutAsync<T>(string relativePath, T payload, CancellationToken cancellationToken, string gw2Subtoken)
		{
			if (!IsConfigured)
			{
				return ApiResult<bool>.Failure("SPARK webserver is not configured.", null, ApiFailure.NotConfigured);
			}
			if (payload == null)
			{
				return ApiResult<bool>.Failure("SPARK request is empty.", null, ApiFailure.InvalidRequest);
			}
			return await ExecuteRequestAsync(HttpMethod.get_Put(), relativePath, cancellationToken, async delegate(CancellationToken requestToken)
			{
				string json = JsonConvert.SerializeObject((object)payload, (Formatting)0, JsonSettings);
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Put(), GetUri(relativePath));
				try
				{
					StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
					try
					{
						request.set_Content((HttpContent)(object)content);
						AddSubtokenHeader(request, gw2Subtoken);
						HttpResponseMessage response = await SharedHttpClient.SendAsync(request, (HttpCompletionOption)1, requestToken);
						try
						{
							return await ReadStatusOnlyResponseAsync(HttpMethod.get_Put(), relativePath, response, requestToken);
						}
						finally
						{
							((IDisposable)response)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)content)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
			});
		}

		private async Task<ApiResult<bool>> DeleteAsync(string relativePath, CancellationToken cancellationToken, string gw2Subtoken)
		{
			if (!IsConfigured)
			{
				return ApiResult<bool>.Failure("SPARK webserver is not configured.", null, ApiFailure.NotConfigured);
			}
			return await ExecuteRequestAsync(HttpMethod.get_Delete(), relativePath, cancellationToken, async delegate(CancellationToken requestToken)
			{
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Delete(), GetUri(relativePath));
				try
				{
					AddSubtokenHeader(request, gw2Subtoken);
					HttpResponseMessage response = await SharedHttpClient.SendAsync(request, (HttpCompletionOption)1, requestToken);
					try
					{
						return await ReadStatusOnlyResponseAsync(HttpMethod.get_Delete(), relativePath, response, requestToken);
					}
					finally
					{
						((IDisposable)response)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
			});
		}

		private static async Task<ApiResult<T>> ExecuteRequestAsync<T>(HttpMethod method, string relativePath, CancellationToken cancellationToken, Func<CancellationToken, Task<ApiResult<T>>> requestAsync)
		{
			string logPath = GetEndpointPathForLog(relativePath);
			using CancellationTokenSource requestTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			requestTimeout.CancelAfter(RequestTimeout);
			try
			{
				return await requestAsync(requestTimeout.Token);
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				throw;
			}
			catch (ResponseBodyTooLargeException ex3)
			{
				Logger.Warn("SPARK {method} {path} exceeded the {limit} byte response limit.", new object[3]
				{
					method.get_Method(),
					logPath,
					1048576
				});
				return ApiResult<T>.Failure("The SPARK webserver returned a response SPARK could not read.", ex3.StatusCode, ApiFailure.InvalidResponse);
			}
			catch (InvalidApiResponseException ex4)
			{
				Logger.Warn("SPARK {method} {path} returned invalid JSON ({errorType}).", new object[3]
				{
					method.get_Method(),
					logPath,
					(ex4.InnerException ?? ex4).GetType().Name
				});
				return ApiResult<T>.Failure("The SPARK webserver returned a response SPARK could not read.", ex4.StatusCode, ApiFailure.InvalidResponse);
			}
			catch (JsonException val)
			{
				JsonException ex5 = val;
				Logger.Warn("SPARK {method} {path} could not serialize the request ({errorType}).", new object[3]
				{
					method.get_Method(),
					logPath,
					((Exception)(object)ex5).GetType().Name
				});
				return ApiResult<T>.Failure("SPARK could not prepare the request.", null, ApiFailure.InvalidRequest);
			}
			catch (OperationCanceledException ex2)
			{
				Logger.Warn("SPARK {method} {path} timed out ({errorType}).", new object[3]
				{
					method.get_Method(),
					logPath,
					ex2.GetType().Name
				});
				return ApiResult<T>.Failure("The SPARK webserver did not respond in time.", null, ApiFailure.Timeout);
			}
			catch (Exception ex)
			{
				BlishWarnings.HttpBlocked(ex, "connect to the SPARK webserver");
				Logger.Warn("SPARK {method} {path} failed ({errorType}).", new object[3]
				{
					method.get_Method(),
					logPath,
					ex.GetType().Name
				});
				ApiFailure failureKind = (BlishWarnings.IsHttpBlocked(ex) ? ApiFailure.BlockedByWindows : ApiFailure.Network);
				return ApiResult<T>.Failure("Cannot connect to the SPARK webserver.", null, failureKind);
			}
		}

		private static async Task<ApiResult<T>> ReadJsonResponseAsync<T>(HttpMethod method, string relativePath, HttpResponseMessage response, CancellationToken cancellationToken) where T : class
		{
			string responseBody = await ReadResponseBodyAsync(response, cancellationToken);
			if (!response.get_IsSuccessStatusCode())
			{
				return CreateStatusFailure<T>(method, relativePath, response, responseBody);
			}
			if (string.IsNullOrWhiteSpace(responseBody))
			{
				return ApiResult<T>.Failure("The SPARK webserver returned an empty response.", response.get_StatusCode());
			}
			try
			{
				T value = JsonConvert.DeserializeObject<T>(responseBody, JsonSettings);
				return (value == null) ? ApiResult<T>.Failure("The SPARK webserver returned a response SPARK could not read.", response.get_StatusCode(), ApiFailure.InvalidResponse) : ApiResult<T>.Success(value, response.get_StatusCode());
			}
			catch (JsonException val)
			{
				JsonException ex = val;
				throw new InvalidApiResponseException(response.get_StatusCode(), (Exception)(object)ex);
			}
		}

		private static async Task<ApiResult<bool>> ReadStatusOnlyResponseAsync(HttpMethod method, string relativePath, HttpResponseMessage response, CancellationToken cancellationToken)
		{
			if (response.get_IsSuccessStatusCode())
			{
				return ApiResult<bool>.Success(value: true, response.get_StatusCode());
			}
			return CreateStatusFailure<bool>(method, relativePath, response, await ReadResponseBodyAsync(response, cancellationToken));
		}

		public Task<ApiResult<bool>> PublishNearbyPresenceResultAsync(NearbyPresence nearby, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			return PostAsync("nearby/", new NearbyPresencePublishRequest
			{
				Nearby = nearby
			}, cancellationToken, gw2Subtoken);
		}

		public Task<ApiResult<NearbyPresenceListResponse>> SearchNearbyPresenceResultAsync(NearbyPresenceSearchRequest query, string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			return PostAsync<NearbyPresenceSearchRequest, NearbyPresenceListResponse>("nearby/search/", query, cancellationToken, gw2Subtoken);
		}

		public Task<ApiResult<bool>> RemoveNearbyPresenceResultAsync(string gw2Subtoken, CancellationToken cancellationToken = default(CancellationToken))
		{
			return DeleteAsync("nearby/", cancellationToken, gw2Subtoken);
		}

		private static ApiResult<T> CreateStatusFailure<T>(HttpMethod method, string relativePath, HttpResponseMessage response, string responseBody)
		{
			Logger.Warn("SPARK {method} {path} failed with status {status}.", new object[3]
			{
				method.get_Method(),
				GetEndpointPathForLog(relativePath),
				response.get_StatusCode()
			});
			return ApiResult<T>.Failure(GetServerStatusMessage(response.get_StatusCode(), responseBody), response.get_StatusCode());
		}

		private static async Task<string> ReadResponseBodyAsync(HttpResponseMessage response, CancellationToken cancellationToken)
		{
			if (((response != null) ? response.get_Content() : null) == null)
			{
				return string.Empty;
			}
			long? contentLength = response.get_Content().get_Headers().get_ContentLength();
			if (contentLength.HasValue && contentLength.Value > 1048576)
			{
				throw new ResponseBodyTooLargeException(response.get_StatusCode());
			}
			using Stream stream = await response.get_Content().ReadAsStreamAsync();
			using MemoryStream memory = new MemoryStream();
			byte[] buffer = new byte[8192];
			int bytesRead;
			while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
			{
				if (memory.Length + bytesRead > 1048576)
				{
					throw new ResponseBodyTooLargeException(response.get_StatusCode());
				}
				memory.Write(buffer, 0, bytesRead);
			}
			return Encoding.UTF8.GetString(memory.ToArray());
		}

		private static string GetEndpointPathForLog(string relativePath)
		{
			string path = relativePath?.Trim() ?? string.Empty;
			int index = path.IndexOfAny(LogPathSeparators);
			if (index >= 0)
			{
				return path.Substring(0, index);
			}
			return path;
		}

		private static void AddSubtokenHeader(HttpRequestMessage request, string gw2Subtoken)
		{
			if (request != null && !string.IsNullOrWhiteSpace(gw2Subtoken))
			{
				((HttpHeaders)request.get_Headers()).TryAddWithoutValidation("X-GW2-Subtoken", gw2Subtoken.Trim());
			}
		}

		private Uri GetUri(string relativePath)
		{
			return new Uri(_baseUri, relativePath ?? string.Empty);
		}

		private static string GetServerStatusMessage(HttpStatusCode statusCode, string responseBody = "")
		{
			string serverDetail = GetServerDetail(responseBody);
			if (!string.IsNullOrWhiteSpace(serverDetail))
			{
				return serverDetail;
			}
			return $"SPARK status: {(int)statusCode}.";
		}

		private static string GetServerDetail(string responseBody)
		{
			if (string.IsNullOrWhiteSpace(responseBody))
			{
				return string.Empty;
			}
			try
			{
				Dictionary<string, object> payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody, JsonSettings);
				if (payload == null || !payload.TryGetValue("detail", out var detail))
				{
					return string.Empty;
				}
				string detailText = detail?.ToString()?.Trim() ?? string.Empty;
				return (detailText.Length <= 160) ? detailText : string.Empty;
			}
			catch
			{
				return string.Empty;
			}
		}

		static SparkClient()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_0059: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			val.set_Timeout(Timeout.InfiniteTimeSpan);
			SharedHttpClient = val;
			JsonSerializerSettings val2 = new JsonSerializerSettings();
			val2.set_NullValueHandling((NullValueHandling)1);
			val2.set_Converters((IList<JsonConverter>)new List<JsonConverter> { (JsonConverter)new StringEnumConverter() });
			JsonSettings = val2;
			LogPathSeparators = new char[2] { '?', '#' };
		}
	}
}
