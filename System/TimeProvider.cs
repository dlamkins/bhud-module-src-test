using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
	internal abstract class TimeProvider
	{
		private sealed class SystemTimeProviderTimer : ITimer, IDisposable, IAsyncDisposable
		{
			private sealed class TimerState
			{
				public TimerCallback Callback { get; }

				public object State { get; }

				public Timer Timer { get; set; }

				public TimerState(TimerCallback callback, object state)
				{
					Callback = callback;
					State = state;
					base._002Ector();
				}
			}

			private readonly Timer _timer;

			public SystemTimeProviderTimer(TimeSpan dueTime, TimeSpan period, TimerCallback callback, object state)
			{
				TimerState timerState = new TimerState(callback, state);
				timerState.Timer = (_timer = new Timer(delegate(object s)
				{
					TimerState timerState2 = (TimerState)s;
					timerState2.Callback(timerState2.State);
				}, timerState, dueTime, period));
			}

			public bool Change(TimeSpan dueTime, TimeSpan period)
			{
				try
				{
					return _timer.Change(dueTime, period);
				}
				catch (ObjectDisposedException)
				{
					return false;
				}
			}

			public void Dispose()
			{
				_timer.Dispose();
			}

			public ValueTask DisposeAsync()
			{
				_timer.Dispose();
				return default(ValueTask);
			}
		}

		private sealed class SystemTimeProvider : TimeProvider
		{
			internal SystemTimeProvider()
			{
			}
		}

		private static readonly long s_minDateTicks = DateTime.MinValue.Ticks;

		private static readonly long s_maxDateTicks = DateTime.MaxValue.Ticks;

		public static TimeProvider System { get; } = new SystemTimeProvider();


		public virtual TimeZoneInfo LocalTimeZone => TimeZoneInfo.Local;

		public virtual long TimestampFrequency => Stopwatch.Frequency;

		public virtual DateTimeOffset GetUtcNow()
		{
			return DateTimeOffset.UtcNow;
		}

		public DateTimeOffset GetLocalNow()
		{
			DateTimeOffset utcNow = GetUtcNow();
			TimeZoneInfo localTimeZone = LocalTimeZone;
			if (localTimeZone == null)
			{
				throw new InvalidOperationException(_003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003ESR.InvalidOperation_TimeProviderNullLocalTimeZone);
			}
			TimeSpan utcOffset = localTimeZone.GetUtcOffset(utcNow);
			if (utcOffset.Ticks == 0L)
			{
				return utcNow;
			}
			long num = utcNow.Ticks + utcOffset.Ticks;
			if ((ulong)num > (ulong)s_maxDateTicks)
			{
				num = ((num < s_minDateTicks) ? s_minDateTicks : s_maxDateTicks);
			}
			return new DateTimeOffset(num, utcOffset);
		}

		public virtual long GetTimestamp()
		{
			return Stopwatch.GetTimestamp();
		}

		public TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp)
		{
			long timestampFrequency = TimestampFrequency;
			if (timestampFrequency <= 0)
			{
				throw new InvalidOperationException(_003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003ESR.InvalidOperation_TimeProviderInvalidTimestampFrequency);
			}
			return new TimeSpan((long)((double)(endingTimestamp - startingTimestamp) * (10000000.0 / (double)timestampFrequency)));
		}

		public TimeSpan GetElapsedTime(long startingTimestamp)
		{
			return GetElapsedTime(startingTimestamp, GetTimestamp());
		}

		public virtual ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			return new SystemTimeProviderTimer(dueTime, period, callback, state);
		}
	}
}
