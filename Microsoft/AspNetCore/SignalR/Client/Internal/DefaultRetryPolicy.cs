using System;

namespace Microsoft.AspNetCore.SignalR.Client.Internal
{
	internal sealed class DefaultRetryPolicy : IRetryPolicy
	{
		internal static TimeSpan?[] DEFAULT_RETRY_DELAYS_IN_MILLISECONDS = new TimeSpan?[5]
		{
			TimeSpan.Zero,
			TimeSpan.FromSeconds(2.0),
			TimeSpan.FromSeconds(10.0),
			TimeSpan.FromSeconds(30.0),
			null
		};

		private readonly TimeSpan?[] _retryDelays;

		public DefaultRetryPolicy()
		{
			_retryDelays = DEFAULT_RETRY_DELAYS_IN_MILLISECONDS;
		}

		public DefaultRetryPolicy(TimeSpan[] retryDelays)
		{
			_retryDelays = new TimeSpan?[retryDelays.Length + 1];
			for (int i = 0; i < retryDelays.Length; i++)
			{
				_retryDelays[i] = retryDelays[i];
			}
		}

		public TimeSpan? NextRetryDelay(RetryContext retryContext)
		{
			return _retryDelays[retryContext.PreviousRetryCount];
		}
	}
}
