namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class PingMessage : HubMessage
	{
		public static readonly PingMessage Instance = new PingMessage();

		private PingMessage()
		{
		}
	}
}
