using System;
using System.Threading;

namespace SL.ChatLinks
{
	internal sealed class Clock : IDisposable
	{
		private readonly Timer _minuteEnded;

		private readonly Timer _hourEnded;

		public event EventHandler? MinuteStarted;

		public event EventHandler? HourStarted;

		public Clock()
		{
			DateTime now = DateTime.Now;
			DateTime nextMinute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Local).AddMinutes(1.0);
			DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Local).AddHours(1.0);
			TimeSpan minuteDue = nextMinute - now;
			TimeSpan hourDue = dateTime - now;
			_minuteEnded = new Timer(OnMinuteStart, null, minuteDue, TimeSpan.FromMinutes(1.0));
			_hourEnded = new Timer(OnHourStart, null, hourDue, TimeSpan.FromHours(1.0));
		}

		private void OnMinuteStart(object state)
		{
			this.MinuteStarted?.Invoke(this, EventArgs.Empty);
		}

		private void OnHourStart(object state)
		{
			this.HourStarted?.Invoke(this, EventArgs.Empty);
		}

		public void Dispose()
		{
			this.MinuteStarted = null;
			this.HourStarted = null;
			_minuteEnded.Dispose();
			_hourEnded.Dispose();
		}
	}
}
