using System.IO.Pipes;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionNamedPipeFeature
	{
		NamedPipeServerStream NamedPipe { get; }
	}
}
