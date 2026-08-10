namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionInherentKeepAliveFeature
	{
		bool HasInherentKeepAlive { get; }
	}
}
