namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class HandshakeRequestMessage : HubMessage
	{
		public string Protocol { get; }

		public int Version { get; }

		public HandshakeRequestMessage(string protocol, int version)
		{
			Protocol = protocol;
			Version = version;
		}
	}
}
