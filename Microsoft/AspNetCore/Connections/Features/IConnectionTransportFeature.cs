using System.IO.Pipelines;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionTransportFeature
	{
		IDuplexPipe Transport { get; set; }
	}
}
