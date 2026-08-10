using System;

namespace Microsoft.AspNetCore.SignalR.Client
{
	internal interface IRetryPolicy
	{
		TimeSpan? NextRetryDelay(RetryContext retryContext);
	}
}
