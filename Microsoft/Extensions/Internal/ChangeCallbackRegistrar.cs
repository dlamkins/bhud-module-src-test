using System;
using System.Threading;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.Internal
{
	internal static class ChangeCallbackRegistrar
	{
		internal static IDisposable UnsafeRegisterChangeCallback<T>(Action<object> callback, object state, CancellationToken token, Action<T> onFailure, T onFailureState)
		{
			bool flag = false;
			if (!ExecutionContext.IsFlowSuppressed())
			{
				ExecutionContext.SuppressFlow();
				flag = true;
			}
			try
			{
				return token.Register(callback, state);
			}
			catch (ObjectDisposedException)
			{
				onFailure(onFailureState);
			}
			finally
			{
				if (flag)
				{
					ExecutionContext.RestoreFlow();
				}
			}
			return EmptyDisposable.Instance;
		}
	}
}
