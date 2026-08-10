using Microsoft.AspNetCore.Shared;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal abstract class HubMethodInvocationMessage : HubInvocationMessage
	{
		public string Target { get; }

		public object?[] Arguments { get; }

		public string[]? StreamIds { get; }

		protected HubMethodInvocationMessage(string? invocationId, string target, object?[] arguments, string[]? streamIds)
			: this(invocationId, target, arguments)
		{
			StreamIds = streamIds;
		}

		protected HubMethodInvocationMessage(string? invocationId, string target, object?[] arguments)
			: base(invocationId)
		{
			ArgumentThrowHelper.ThrowIfNullOrEmpty(target, "target");
			Target = target;
			Arguments = arguments;
		}
	}
}
