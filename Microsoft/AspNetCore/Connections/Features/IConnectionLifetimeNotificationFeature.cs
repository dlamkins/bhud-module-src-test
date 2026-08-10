using System.Threading;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionLifetimeNotificationFeature
	{
		CancellationToken ConnectionClosedRequested { get; set; }

		void RequestClose();
	}
}
