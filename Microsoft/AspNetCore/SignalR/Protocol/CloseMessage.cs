namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class CloseMessage : HubMessage
	{
		public static readonly CloseMessage Empty = new CloseMessage(null, allowReconnect: false);

		public string? Error { get; }

		public bool AllowReconnect { get; }

		public CloseMessage(string? error)
			: this(error, allowReconnect: false)
		{
		}

		public CloseMessage(string? error, bool allowReconnect)
		{
			Error = error;
			AllowReconnect = allowReconnect;
		}
	}
}
