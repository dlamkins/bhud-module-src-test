using System;

namespace Microsoft.AspNetCore.Connections
{
	internal interface IMultiplexedConnectionBuilder
	{
		IServiceProvider ApplicationServices { get; }

		IMultiplexedConnectionBuilder Use(Func<MultiplexedConnectionDelegate, MultiplexedConnectionDelegate> middleware);

		MultiplexedConnectionDelegate Build();
	}
}
