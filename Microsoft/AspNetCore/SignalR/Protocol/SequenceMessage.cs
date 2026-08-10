namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal sealed class SequenceMessage : HubMessage
	{
		public long SequenceId { get; set; }

		public SequenceMessage(long sequenceId)
		{
			SequenceId = sequenceId;
		}
	}
}
