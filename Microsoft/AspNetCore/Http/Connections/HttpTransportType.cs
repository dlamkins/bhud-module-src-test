using System;

namespace Microsoft.AspNetCore.Http.Connections
{
	[Flags]
	internal enum HttpTransportType
	{
		None = 0x0,
		WebSockets = 0x1,
		ServerSentEvents = 0x2,
		LongPolling = 0x4
	}
}
