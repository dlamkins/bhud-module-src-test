using System;

namespace Neokain.GW2.WebClient
{
	public sealed class RetryScheduledEventArgs : EventArgs
	{
		public DateTimeOffset NextAttemptUtc { get; }

		public RetryScheduledEventArgs(DateTimeOffset nextAttemptUtc)
		{
			NextAttemptUtc = nextAttemptUtc;
		}
	}
}
