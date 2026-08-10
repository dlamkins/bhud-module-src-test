using System.Buffers;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal sealed class RawResult
	{
		public ReadOnlySequence<byte> RawSerializedData { get; private set; }

		public RawResult(ReadOnlySequence<byte> rawBytes)
		{
			RawSerializedData = new ReadOnlySequence<byte>(BuffersExtensions.ToArray(in rawBytes));
		}
	}
}
