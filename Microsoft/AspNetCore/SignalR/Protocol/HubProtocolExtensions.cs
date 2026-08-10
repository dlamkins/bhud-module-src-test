using Microsoft.AspNetCore.Internal;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal static class HubProtocolExtensions
	{
		public static byte[] GetMessageBytes(this IHubProtocol hubProtocol, HubMessage message)
		{
			MemoryBufferWriter memoryBufferWriter = MemoryBufferWriter.Get();
			try
			{
				hubProtocol.WriteMessage(message, memoryBufferWriter);
				return memoryBufferWriter.ToArray();
			}
			finally
			{
				MemoryBufferWriter.Return(memoryBufferWriter);
			}
		}
	}
}
