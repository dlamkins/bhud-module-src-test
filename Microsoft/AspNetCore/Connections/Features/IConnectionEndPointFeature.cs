using System.Net;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionEndPointFeature
	{
		EndPoint? LocalEndPoint { get; set; }

		EndPoint? RemoteEndPoint { get; set; }
	}
}
