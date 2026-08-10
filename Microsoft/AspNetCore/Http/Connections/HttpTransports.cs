namespace Microsoft.AspNetCore.Http.Connections
{
	internal static class HttpTransports
	{
		public static readonly HttpTransportType All = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling;
	}
}
