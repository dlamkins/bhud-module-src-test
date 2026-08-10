namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class CancelInvocationMessage : HubInvocationMessage
	{
		public CancelInvocationMessage(string invocationId)
			: base(invocationId)
		{
		}
	}
}
