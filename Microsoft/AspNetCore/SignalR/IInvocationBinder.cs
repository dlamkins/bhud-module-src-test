using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.SignalR
{
	internal interface IInvocationBinder
	{
		Type GetReturnType(string invocationId);

		IReadOnlyList<Type> GetParameterTypes(string methodName);

		Type GetStreamItemType(string streamId);
	}
}
