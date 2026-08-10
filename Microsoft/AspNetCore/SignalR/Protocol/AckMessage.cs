namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal sealed class AckMessage : HubMessage
	{
		public long SequenceId { get; set; }

		public AckMessage(long sequenceId)
		{
			SequenceId = sequenceId;
		}
	}
}
