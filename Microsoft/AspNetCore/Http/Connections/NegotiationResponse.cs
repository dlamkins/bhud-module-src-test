using System.Collections.Generic;

namespace Microsoft.AspNetCore.Http.Connections
{
	internal class NegotiationResponse
	{
		public string? Url { get; set; }

		public string? AccessToken { get; set; }

		public string? ConnectionId { get; set; }

		public string? ConnectionToken { get; set; }

		public int Version { get; set; }

		public IList<AvailableTransport>? AvailableTransports { get; set; }

		public string? Error { get; set; }

		public bool UseStatefulReconnect { get; set; }
	}
}
