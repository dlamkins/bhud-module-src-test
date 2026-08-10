using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Neokain.GW2.WebClient
{
	internal sealed class InfiniteRetryPolicy : IRetryPolicy
	{
		internal static readonly TimeSpan[] Delays = new TimeSpan[5]
		{
			TimeSpan.Zero,
			TimeSpan.FromSeconds(2.0),
			TimeSpan.FromSeconds(10.0),
			TimeSpan.FromSeconds(30.0),
			TimeSpan.FromSeconds(60.0)
		};

		internal static TimeSpan[]? DelaysOverride;

		internal static TimeSpan[] EffectiveDelays => DelaysOverride ?? Delays;

		public TimeSpan? NextRetryDelay(RetryContext retryContext)
		{
			try
			{
				TimeSpan[] delays = Delays;
				long num = retryContext?.PreviousRetryCount ?? long.MaxValue;
				return (num >= delays.Length) ? delays[delays.Length - 1] : delays[(int)num];
			}
			catch
			{
				return TimeSpan.FromSeconds(60.0);
			}
		}
	}
}
