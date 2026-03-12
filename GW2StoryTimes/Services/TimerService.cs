using System;
using System.Diagnostics;

namespace GW2StoryTimes.Services
{
	public class TimerService : IDisposable
	{
		private readonly Stopwatch _stopwatch = new Stopwatch();

		public TimeSpan Elapsed => _stopwatch.Elapsed;

		public bool IsRunning => _stopwatch.IsRunning;

		public string FormattedElapsed
		{
			get
			{
				TimeSpan ts = _stopwatch.Elapsed;
				if (!(ts.TotalHours >= 1.0))
				{
					return $"{ts.Minutes:D2}:{ts.Seconds:D2}";
				}
				return $"{(int)ts.TotalHours}:{ts.Minutes:D2}:{ts.Seconds:D2}";
			}
		}

		public void Start()
		{
			_stopwatch.Start();
		}

		public void Stop()
		{
			_stopwatch.Stop();
		}

		public void Reset()
		{
			_stopwatch.Reset();
		}

		public void Toggle()
		{
			if (_stopwatch.IsRunning)
			{
				_stopwatch.Stop();
			}
			else
			{
				_stopwatch.Start();
			}
		}

		public void Dispose()
		{
			_stopwatch.Stop();
		}
	}
}
