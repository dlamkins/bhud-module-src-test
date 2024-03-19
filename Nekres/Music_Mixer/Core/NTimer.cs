using System;
using System.Diagnostics;
using System.Timers;

namespace Nekres.Music_Mixer.Core
{
	public class NTimer : IDisposable
	{
		private Timer _timer;

		private Stopwatch _stopWatch;

		private bool _paused;

		private double _remainingTimeBeforePause;

		private bool _disposed;

		public bool Paused => _paused;

		public bool IsRunning => _stopWatch.IsRunning;

		public bool AutoReset
		{
			get
			{
				return _timer.AutoReset;
			}
			set
			{
				_timer.AutoReset = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return _timer.Enabled;
			}
			set
			{
				_timer.Enabled = value;
			}
		}

		public double Interval
		{
			get
			{
				return _timer.Interval;
			}
			set
			{
				_timer.Interval = value;
			}
		}

		public event ElapsedEventHandler Elapsed;

		public NTimer()
		{
			_stopWatch = new Stopwatch();
			_timer = new Timer();
			_timer.AutoReset = false;
			_timer.Elapsed += delegate(object sender, ElapsedEventArgs arguments)
			{
				_stopWatch.Stop();
				this.Elapsed?.Invoke(sender, arguments);
				if (_timer != null && _timer.AutoReset)
				{
					_stopWatch.Restart();
				}
			};
		}

		public NTimer(double interval)
			: this()
		{
			Interval = interval;
		}

		public void Start()
		{
			_timer.Start();
			_stopWatch.Restart();
		}

		public void Restart()
		{
			if (!_timer.Enabled)
			{
				_timer.Start();
			}
			_stopWatch.Restart();
			_timer.Interval = _timer.Interval;
		}

		public void Stop()
		{
			_timer.Stop();
			_stopWatch.Stop();
		}

		public void Pause()
		{
			if (!_paused && _timer.Enabled)
			{
				_paused = true;
				_stopWatch.Stop();
				_timer.Stop();
				_remainingTimeBeforePause = Math.Max(0.0, Interval - (double)_stopWatch.ElapsedMilliseconds);
			}
		}

		public void Resume()
		{
			if (_paused)
			{
				_paused = false;
				if (_remainingTimeBeforePause > 0.0)
				{
					_timer.Interval = _remainingTimeBeforePause;
					_timer.Start();
					_stopWatch.Start();
				}
			}
		}

		public void Dispose()
		{
			if (_timer != null && !_disposed)
			{
				_disposed = true;
				_timer.Dispose();
				_timer = null;
			}
		}

		~NTimer()
		{
			Dispose();
		}
	}
}
