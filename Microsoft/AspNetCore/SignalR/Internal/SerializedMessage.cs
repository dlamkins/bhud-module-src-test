using System;

namespace Microsoft.AspNetCore.SignalR.Internal
{
	internal readonly struct SerializedMessage
	{
		public string ProtocolName { get; }

		public ReadOnlyMemory<byte> Serialized { get; }

		public SerializedMessage(string protocolName, ReadOnlyMemory<byte> serialized)
		{
			ProtocolName = protocolName;
			Serialized = serialized;
		}
	}
}
