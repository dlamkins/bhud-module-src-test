using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.SignalR
{
	internal interface ISignalRBuilder
	{
		IServiceCollection Services { get; }
	}
}
