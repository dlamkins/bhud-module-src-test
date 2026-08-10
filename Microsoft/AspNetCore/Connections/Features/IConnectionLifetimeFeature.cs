using System.Threading;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionLifetimeFeature
	{
		CancellationToken ConnectionClosed { get; set; }

		void Abort();
	}
}
