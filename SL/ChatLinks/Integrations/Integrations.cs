using System;
using System.Net.Http;
using System.Threading;
using GuildWars2;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace SL.ChatLinks.Integrations
{
	internal static class Integrations
	{
		internal static void AddGw2Client(this IServiceCollection services)
		{
			IHttpClientBuilder builder = services.AddHttpClient<Gw2Client>(delegate(HttpClient httpClient)
			{
				httpClient.set_Timeout(Timeout.InfiniteTimeSpan);
			});
			builder.ConfigurePrimaryHttpMessageHandler((Func<HttpMessageHandler>)delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				HttpClientHandler val = new HttpClientHandler();
				val.set_MaxConnectionsPerServer(int.MaxValue);
				return (HttpMessageHandler)val;
			});
			builder.AddHttpMessageHandler(() => (DelegatingHandler)(object)new ResilienceHandler(new ResiliencePipelineBuilder<HttpResponseMessage>().AddTimeout(Resiliency.TotalTimeoutStrategy).AddRetry(Resiliency.RetryStrategy).AddCircuitBreaker(Resiliency.CircuitBreakerStrategy)
				.AddHedging(Resiliency.HedgingStrategy)
				.AddTimeout(Resiliency.AttemptTimeoutStrategy)
				.Build()));
		}
	}
}
