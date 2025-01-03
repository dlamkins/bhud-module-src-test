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
				httpClient.Timeout = Timeout.InfiniteTimeSpan;
			});
			builder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				MaxConnectionsPerServer = int.MaxValue
			});
			builder.AddHttpMessageHandler(() => new ResilienceHandler(new ResiliencePipelineBuilder<HttpResponseMessage>().AddTimeout(Resiliency.TotalTimeoutStrategy).AddRetry(Resiliency.RetryStrategy).AddCircuitBreaker(Resiliency.CircuitBreakerStrategy)
				.AddHedging(Resiliency.HedgingStrategy)
				.AddTimeout(Resiliency.AttemptTimeoutStrategy)
				.Build()));
		}
	}
}
