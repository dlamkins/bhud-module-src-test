using System.Buffers;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IMemoryPoolFeature
	{
		MemoryPool<byte> MemoryPool { get; }
	}
}
