using System;

namespace Microsoft.AspNetCore.Http.Connections.Client
{
	internal class NoTransportSupportedException : Exception
	{
		public NoTransportSupportedException(string message)
			: base(message)
		{
		}
	}
}
