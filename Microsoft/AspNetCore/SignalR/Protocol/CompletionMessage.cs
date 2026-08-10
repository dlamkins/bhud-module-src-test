using System;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class CompletionMessage : HubInvocationMessage
	{
		public string? Error { get; }

		public object? Result { get; }

		public bool HasResult { get; }

		public CompletionMessage(string invocationId, string? error, object? result, bool hasResult)
			: base(invocationId)
		{
			if (error != null && hasResult)
			{
				throw new ArgumentException("Expected either 'error' or 'result' to be provided, but not both");
			}
			Error = error;
			Result = result;
			HasResult = hasResult;
		}

		public override string ToString()
		{
			string text = ((Error == null) ? "<<null>>" : ("\"" + Error + "\""));
			string text2 = (HasResult ? string.Format(", {0}: {1}", "Result", Result ?? "<<null>>") : string.Empty);
			return "Completion { InvocationId: \"" + base.InvocationId + "\", Error: " + text + text2 + " }";
		}

		public static CompletionMessage WithError(string invocationId, string? error)
		{
			return new CompletionMessage(invocationId, error, null, hasResult: false);
		}

		public static CompletionMessage WithResult(string invocationId, object? payload)
		{
			return new CompletionMessage(invocationId, null, payload, hasResult: true);
		}

		public static CompletionMessage Empty(string invocationId)
		{
			return new CompletionMessage(invocationId, null, null, hasResult: false);
		}
	}
}
