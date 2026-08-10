using System;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal static class ProtocolHelper
	{
		internal static Type TryGetReturnType(IInvocationBinder binder, string invocationId)
		{
			try
			{
				return binder.GetReturnType(invocationId);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
