using System.Collections.Generic;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal abstract class HubInvocationMessage : HubMessage
	{
		public IDictionary<string, string>? Headers { get; set; }

		public string? InvocationId { get; }

		protected HubInvocationMessage(string? invocationId)
		{
			InvocationId = invocationId;
		}
	}
}
