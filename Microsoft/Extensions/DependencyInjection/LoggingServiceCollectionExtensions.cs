using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class LoggingServiceCollectionExtensions
	{
		public static IServiceCollection AddLogging(this IServiceCollection services)
		{
			return services.AddLogging(delegate
			{
			});
		}

		public static IServiceCollection AddLogging(this IServiceCollection services, Action<ILoggingBuilder> configure)
		{
			_003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EThrowHelper.ThrowIfNull(services, "services");
			services.AddOptions();
			services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
			services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
			services.TryAddEnumerable(ServiceDescriptor.Singleton((IConfigureOptions<LoggerFilterOptions>)new DefaultLoggerLevelConfigureOptions(LogLevel.Information)));
			configure(new LoggingBuilder(services));
			return services;
		}
	}
}
