using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal class InvocationMessage : HubMethodInvocationMessage
	{
		public InvocationMessage(string target, object?[] arguments)
			: this(null, target, arguments)
		{
		}

		public InvocationMessage(string? invocationId, string target, object?[] arguments)
			: base(invocationId, target, arguments)
		{
		}

		public InvocationMessage(string? invocationId, string target, object?[] arguments, string[]? streamIds)
			: base(invocationId, target, arguments, streamIds)
		{
		}

		public override string ToString()
		{
			string text;
			try
			{
				text = ((base.Arguments == null) ? string.Empty : string.Join(", ", base.Arguments.Select((object a) => a?.ToString())));
			}
			catch (Exception ex)
			{
				text = "Error: " + ex.Message;
			}
			string text2;
			try
			{
				IEnumerable<string> values;
				if (base.StreamIds == null)
				{
					IEnumerable<string> enumerable = Array.Empty<string>();
					values = enumerable;
				}
				else
				{
					values = base.StreamIds.Select((string id) => id?.ToString());
				}
				text2 = string.Join(", ", values);
			}
			catch (Exception ex2)
			{
				text2 = "Error: " + ex2.Message;
			}
			return "InvocationMessage { InvocationId: \"" + base.InvocationId + "\", Target: \"" + base.Target + "\", Arguments: [ " + text + " ], StreamIds: [ " + text2 + " ] }";
		}
	}
}
