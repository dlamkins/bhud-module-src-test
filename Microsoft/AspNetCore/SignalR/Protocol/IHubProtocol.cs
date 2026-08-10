using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Connections;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal interface IHubProtocol
	{
		string Name { get; }

		int Version { get; }

		TransferFormat TransferFormat { get; }

		bool TryParseMessage(ref ReadOnlySequence<byte> input, IInvocationBinder binder, [_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ENotNullWhen(true)] out HubMessage? message);

		void WriteMessage(HubMessage message, IBufferWriter<byte> output);

		ReadOnlyMemory<byte> GetMessageBytes(HubMessage message);

		bool IsVersionSupported(int version);
	}
}
