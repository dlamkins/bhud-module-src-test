using System;

namespace Microsoft.AspNetCore.Http.Connections.Client
{
	internal class TransportFailedException : Exception
	{
		public string TransportType { get; }

		public TransportFailedException(string transportType, string message, Exception? innerException = null)
			: base(transportType + " failed: " + message, innerException)
		{
			TransportType = transportType;
		}
	}
}
