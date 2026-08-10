using System;

namespace Microsoft.Extensions.DependencyInjection
{
	internal class DefaultServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
	{
		private readonly ServiceProviderOptions _options;

		public DefaultServiceProviderFactory()
			: this(ServiceProviderOptions.Default)
		{
		}

		public DefaultServiceProviderFactory(ServiceProviderOptions options)
		{
			_options = options ?? throw new ArgumentNullException("options");
		}

		public IServiceCollection CreateBuilder(IServiceCollection services)
		{
			return services;
		}

		public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
		{
			return containerBuilder.BuildServiceProvider(_options);
		}
	}
}
