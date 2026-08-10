namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class HandshakeResponseMessage : HubMessage
	{
		public static readonly HandshakeResponseMessage Empty = new HandshakeResponseMessage(null);

		public string? Error { get; }

		public HandshakeResponseMessage(string? error)
		{
			Error = error;
		}
	}
}
