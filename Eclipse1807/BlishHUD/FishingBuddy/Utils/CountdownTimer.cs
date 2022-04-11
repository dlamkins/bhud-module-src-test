using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	public class CountdownTimer : IDisposable
	{
		public Stopwatch _stopWatch = new Stopwatch();

		public Action TimeChanged;

		public Action CountdownFinished;

		private Timer timer = new Timer();

		private TimeSpan _max = TimeSpan.FromMilliseconds(30000.0);

		public bool IsRunning => timer.Enabled;

		public int StepMs
		{
			get
			{
				return timer.Interval;
			}
			set
			{
				timer.Interval = value;
			}
		}

		public TimeSpan TimeLeft
		{
			get
			{
				if (!(_max.TotalMilliseconds - (double)_stopWatch.ElapsedMilliseconds > 0.0))
				{
					return TimeSpan.FromMilliseconds(0.0);
				}
				return TimeSpan.FromMilliseconds(_max.TotalMilliseconds - (double)_stopWatch.ElapsedMilliseconds);
			}
		}

		private bool _mustStop => _max.TotalMilliseconds - (double)_stopWatch.ElapsedMilliseconds < 0.0;

		public string TimeLeftStr => TimeLeft.ToString("mm':'ss");

		public string TimeLeftMsStr => TimeLeft.ToString("mm':'ss'.'fff");

		private void TimerTick(object sender, EventArgs e)
		{
			TimeChanged?.Invoke();
			if (_mustStop)
			{
				CountdownFinished?.Invoke();
				_stopWatch.Stop();
				timer.Enabled = false;
			}
		}

		public CountdownTimer(int min, int sec)
		{
			SetTime(min, sec);
			Init();
		}

		public CountdownTimer(TimeSpan ts)
		{
			if (!(ts == TimeSpan.Zero))
			{
				SetTime(ts);
				Init();
			}
		}

		public CountdownTimer()
		{
			Init();
		}

		private void Init()
		{
			StepMs = 1000;
			timer.Tick += TimerTick;
		}

		public void SetTime(TimeSpan ts)
		{
			_max = ts;
			TimeChanged?.Invoke();
		}

		public void SetTime(int min, int sec = 0)
		{
			SetTime(TimeSpan.FromSeconds(min * 60 + sec));
		}

		public void Start()
		{
			timer.Start();
			_stopWatch.Start();
		}

		public void Pause()
		{
			timer.Stop();
			_stopWatch.Stop();
		}

		public void Stop()
		{
			Reset();
			Pause();
		}

		public void Reset()
		{
			_stopWatch.Reset();
		}

		public void Restart()
		{
			_stopWatch.Reset();
			timer.Start();
		}

		public void Dispose()
		{
			timer.Dispose();
		}
	}
}
