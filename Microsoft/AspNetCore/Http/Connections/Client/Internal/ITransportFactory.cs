namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal interface ITransportFactory
	{
		ITransport CreateTransport(HttpTransportType availableServerTransports, bool useStatefulReconnect);
	}
}
