using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Hedging;
using Polly.Retry;
using Polly.Timeout;

namespace SL.ChatLinks.Integrations
{
	internal static class Resiliency
	{
		public static readonly TimeoutStrategyOptions TotalTimeoutStrategy = new TimeoutStrategyOptions
		{
			Timeout = TimeSpan.FromMinutes(3.0)
		};

		public static readonly RetryStrategyOptions<HttpResponseMessage> RetryStrategy = new RetryStrategyOptions<HttpResponseMessage>
		{
			MaxRetryAttempts = 10,
			Delay = TimeSpan.FromSeconds(10.0),
			BackoffType = DelayBackoffType.Constant,
			UseJitter = true,
			ShouldHandle = async delegate(RetryPredicateArguments<HttpResponseMessage> attempt)
			{
				Outcome<HttpResponseMessage> outcome2 = attempt.Outcome;
				Exception exception2 = outcome2.Exception;
				bool result6;
				if (!(exception2 is OperationCanceledException))
				{
					if (!(exception2 is HttpRequestException))
					{
						if (!(exception2 is TimeoutRejectedException))
						{
							if (!(exception2 is BrokenCircuitException))
							{
								HttpResponseMessage result5 = outcome2.Result;
								if (result5 != null)
								{
									switch (result5.StatusCode)
									{
									case HttpStatusCode.RequestTimeout:
										goto IL_0134;
									case (HttpStatusCode)422:
										goto IL_013b;
									case HttpStatusCode.InternalServerError:
										goto IL_0142;
									case HttpStatusCode.BadGateway:
										goto IL_0149;
									case HttpStatusCode.ServiceUnavailable:
										goto IL_01b1;
									case HttpStatusCode.GatewayTimeout:
										goto IL_01c8;
									}
									if (!result5.IsSuccessStatusCode)
									{
										HttpContent content3 = result5.Content;
										if (content3 != null)
										{
											HttpContentHeaders headers3 = content3.Headers;
											if (headers3 != null)
											{
												long? contentLength3 = headers3.ContentLength;
												if (contentLength3.HasValue && contentLength3.GetValueOrDefault() == 0L)
												{
													result6 = true;
													goto IL_0287;
												}
											}
										}
										bool flag3;
										switch (await GetText(attempt.Outcome))
										{
										case "endpoint requires authentication":
										case "unknown error":
										case "ErrBadData":
										case "ErrTimeout":
											flag3 = true;
											break;
										default:
											flag3 = false;
											break;
										}
										result6 = flag3;
										goto IL_0287;
									}
								}
								result6 = false;
							}
							else
							{
								result6 = true;
							}
						}
						else
						{
							result6 = true;
						}
					}
					else
					{
						result6 = true;
					}
				}
				else
				{
					result6 = !attempt.Context.CancellationToken.IsCancellationRequested;
				}
				goto IL_0287;
				IL_0149:
				result6 = true;
				goto IL_0287;
				IL_013b:
				result6 = true;
				goto IL_0287;
				IL_0142:
				result6 = true;
				goto IL_0287;
				IL_0134:
				result6 = true;
				goto IL_0287;
				IL_0287:
				return result6;
				IL_01c8:
				result6 = true;
				goto IL_0287;
				IL_01b1:
				result6 = await GetText(attempt.Outcome) != "API not active";
				goto IL_0287;
			}
		};

		public static readonly CircuitBreakerStrategyOptions<HttpResponseMessage> CircuitBreakerStrategy = new CircuitBreakerStrategyOptions<HttpResponseMessage>
		{
			ShouldHandle = async delegate(CircuitBreakerPredicateArguments<HttpResponseMessage> attempt)
			{
				Outcome<HttpResponseMessage> outcome = attempt.Outcome;
				Exception exception = outcome.Exception;
				bool result4;
				if (!(exception is OperationCanceledException))
				{
					if (!(exception is HttpRequestException))
					{
						if (!(exception is TimeoutRejectedException))
						{
							HttpResponseMessage result3 = outcome.Result;
							if (result3 != null)
							{
								switch (result3.StatusCode)
								{
								case HttpStatusCode.RequestTimeout:
									goto IL_0121;
								case (HttpStatusCode)422:
									goto IL_0128;
								case HttpStatusCode.InternalServerError:
									goto IL_012f;
								case HttpStatusCode.BadGateway:
									goto IL_0136;
								case HttpStatusCode.ServiceUnavailable:
									goto IL_019e;
								case HttpStatusCode.GatewayTimeout:
									goto IL_01b5;
								}
								if (!result3.IsSuccessStatusCode)
								{
									HttpContent content2 = result3.Content;
									if (content2 != null)
									{
										HttpContentHeaders headers2 = content2.Headers;
										if (headers2 != null)
										{
											long? contentLength2 = headers2.ContentLength;
											if (contentLength2.HasValue && contentLength2.GetValueOrDefault() == 0L)
											{
												result4 = true;
												goto IL_0274;
											}
										}
									}
									bool flag2;
									switch (await GetText(attempt.Outcome))
									{
									case "endpoint requires authentication":
									case "unknown error":
									case "ErrBadData":
									case "ErrTimeout":
										flag2 = true;
										break;
									default:
										flag2 = false;
										break;
									}
									result4 = flag2;
									goto IL_0274;
								}
							}
							result4 = false;
						}
						else
						{
							result4 = true;
						}
					}
					else
					{
						result4 = true;
					}
				}
				else
				{
					result4 = !attempt.Context.CancellationToken.IsCancellationRequested;
				}
				goto IL_0274;
				IL_012f:
				result4 = true;
				goto IL_0274;
				IL_0121:
				result4 = true;
				goto IL_0274;
				IL_0274:
				return result4;
				IL_0128:
				result4 = true;
				goto IL_0274;
				IL_01b5:
				result4 = true;
				goto IL_0274;
				IL_019e:
				result4 = await GetText(attempt.Outcome) != "API not active";
				goto IL_0274;
				IL_0136:
				result4 = true;
				goto IL_0274;
			}
		};

		public static readonly HedgingStrategyOptions<HttpResponseMessage> HedgingStrategy = new HedgingStrategyOptions<HttpResponseMessage>
		{
			Delay = TimeSpan.FromSeconds(10.0),
			ShouldHandle = async delegate(HedgingPredicateArguments<HttpResponseMessage> attempt)
			{
				HttpResponseMessage result = attempt.Outcome.Result;
				bool result2;
				if (result != null)
				{
					switch (result.StatusCode)
					{
					case HttpStatusCode.RequestTimeout:
						goto IL_009f;
					case HttpStatusCode.InternalServerError:
						goto IL_00a6;
					case HttpStatusCode.BadGateway:
						goto IL_00ad;
					case HttpStatusCode.GatewayTimeout:
						goto IL_00b4;
					}
					if (!result.IsSuccessStatusCode)
					{
						HttpContent content = result.Content;
						if (content != null)
						{
							HttpContentHeaders headers = content.Headers;
							if (headers != null)
							{
								long? contentLength = headers.ContentLength;
								if (contentLength.HasValue && contentLength.GetValueOrDefault() == 0L)
								{
									result2 = true;
									goto IL_0173;
								}
							}
						}
						bool flag;
						switch (await GetText(attempt.Outcome))
						{
						case "endpoint requires authentication":
						case "unknown error":
						case "ErrBadData":
						case "ErrTimeout":
							flag = true;
							break;
						default:
							flag = false;
							break;
						}
						result2 = flag;
						goto IL_0173;
					}
				}
				result2 = false;
				goto IL_0173;
				IL_0173:
				return result2;
				IL_00b4:
				result2 = true;
				goto IL_0173;
				IL_00ad:
				result2 = true;
				goto IL_0173;
				IL_00a6:
				result2 = true;
				goto IL_0173;
				IL_009f:
				result2 = true;
				goto IL_0173;
			}
		};

		public static readonly TimeoutStrategyOptions AttemptTimeoutStrategy = new TimeoutStrategyOptions
		{
			Timeout = TimeSpan.FromSeconds(30.0)
		};

		private static async Task<string?> GetText(Outcome<HttpResponseMessage> attempt)
		{
			if (attempt.Result == null)
			{
				return null;
			}
			if (attempt.Result!.Content.Headers.ContentType?.MediaType != "application/json")
			{
				return null;
			}
			await attempt.Result!.Content.LoadIntoBufferAsync();
			Stream content = await attempt.Result!.Content.ReadAsStreamAsync();
			try
			{
				using JsonDocument json = await JsonDocument.ParseAsync(content);
				JsonElement text;
				return json.RootElement.TryGetProperty("text", out text) ? text.GetString() : null;
			}
			finally
			{
				content.Position = 0L;
			}
		}
	}
}
