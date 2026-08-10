namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IProtocolErrorCodeFeature
	{
		long Error { get; set; }
	}
}
