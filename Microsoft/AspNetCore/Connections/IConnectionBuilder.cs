using System;

namespace Microsoft.AspNetCore.Connections
{
	internal interface IConnectionBuilder
	{
		IServiceProvider ApplicationServices { get; }

		IConnectionBuilder Use(Func<ConnectionDelegate, ConnectionDelegate> middleware);

		ConnectionDelegate Build();
	}
}
