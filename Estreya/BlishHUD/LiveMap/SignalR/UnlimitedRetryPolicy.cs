using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Estreya.BlishHUD.LiveMap.SignalR
{
	public class UnlimitedRetryPolicy : IRetryPolicy
	{
		private readonly TimeSpan _retryTime;

		public UnlimitedRetryPolicy(TimeSpan retryTime)
		{
			_retryTime = retryTime;
		}

		public TimeSpan? NextRetryDelay(RetryContext retryContext)
		{
			return _retryTime;
		}
	}
}
