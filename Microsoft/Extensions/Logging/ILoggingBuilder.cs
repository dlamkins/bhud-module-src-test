using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
{
	internal interface ILoggingBuilder
	{
		IServiceCollection Services { get; }
	}
}
