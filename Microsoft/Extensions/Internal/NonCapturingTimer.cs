using System;
using System.Threading;
using Microsoft.AspNetCore.Shared;

namespace Microsoft.Extensions.Internal
{
	internal static class NonCapturingTimer
	{
		public static Timer Create(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
		{
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(callback, "callback");
			bool flag = false;
			try
			{
				if (!ExecutionContext.IsFlowSuppressed())
				{
					ExecutionContext.SuppressFlow();
					flag = true;
				}
				return new Timer(callback, state, dueTime, period);
			}
			finally
			{
				if (flag)
				{
					ExecutionContext.RestoreFlow();
				}
			}
		}
	}
}
