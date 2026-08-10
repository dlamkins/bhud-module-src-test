using System.Runtime.ExceptionServices;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class InvocationBindingFailureMessage : HubInvocationMessage
	{
		public ExceptionDispatchInfo BindingFailure { get; }

		public string Target { get; }

		public InvocationBindingFailureMessage(string? invocationId, string target, ExceptionDispatchInfo bindingFailure)
			: base(invocationId)
		{
			Target = target;
			BindingFailure = bindingFailure;
		}
	}
}
