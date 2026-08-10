using System.Runtime.ExceptionServices;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class StreamBindingFailureMessage : HubMessage
	{
		public string Id { get; }

		public ExceptionDispatchInfo BindingFailure { get; }

		public StreamBindingFailureMessage(string id, ExceptionDispatchInfo bindingFailure)
		{
			Id = id;
			BindingFailure = bindingFailure;
		}
	}
}
