namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IStreamDirectionFeature
	{
		bool CanRead { get; }

		bool CanWrite { get; }
	}
}
