namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class StreamItemMessage : HubInvocationMessage
	{
		public object? Item { get; set; }

		public StreamItemMessage(string invocationId, object? item)
			: base(invocationId)
		{
			Item = item;
		}

		public override string ToString()
		{
			return string.Format("StreamItem {{ {0}: \"{1}\", {2}: {3} }}", "InvocationId", base.InvocationId, "Item", Item ?? "<<null>>");
		}
	}
}
