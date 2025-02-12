using System;
using System.Net.Http;
using System.Threading;
using GuildWars2;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using SL.ChatLinks.StaticFiles;
using SL.ChatLinks.Storage;

namespace SL.ChatLinks.Integrations
{
	public static class Integrations
	{
		public static void AddDatabase(this IServiceCollection services, Action<DatabaseOptions> configureOptions)
		{
			services.Configure(configureOptions);
			services.AddSingleton<IDbContextFactory, SqliteDbContextFactory>();
		}

		public static void AddGw2Client(this IServiceCollection services)
		{
			IHttpClientBuilder builder = services.AddHttpClient<Gw2Client>(delegate(HttpClient httpClient)
			{
				httpClient.BaseAddress = BaseAddress.DefaultUri;
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

		public static void AddStaticDataClient(this IServiceCollection services)
		{
			IHttpClientBuilder builder = services.AddHttpClient<StaticDataClient>(delegate(HttpClient httpClient)
			{
				httpClient.BaseAddress = new Uri("https://bhm.blishhud.com/sliekens.chat_links/");
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
