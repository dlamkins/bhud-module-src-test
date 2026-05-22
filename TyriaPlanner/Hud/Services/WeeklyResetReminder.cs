using System;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using TyriaPlanner.Hud.Settings;
using TyriaPlanner.Hud.Ui;

namespace TyriaPlanner.Hud.Services
{
	public sealed class WeeklyResetReminder : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<WeeklyResetReminder>();

		private readonly ModuleSettings _settings;

		private readonly ToastStack _stack;

		private Timer _timer;

		public WeeklyResetReminder(ModuleSettings settings, ToastStack stack)
		{
			_settings = settings;
			_stack = stack;
		}

		public void Start()
		{
			ScheduleNext();
		}

		private void ScheduleNext()
		{
			TimeSpan delay = TimeUntilNextResetUtc(DateTime.UtcNow);
			_timer?.Dispose();
			_timer = new Timer(delegate
			{
				OnFire();
			}, null, delay, Timeout.InfiniteTimeSpan);
			Logger.Info("Weekly reset reminder scheduled in {0:0.#} h", new object[1] { delay.TotalHours });
		}

		private void OnFire()
		{
			if (_settings.NotifyWeeklyReset.get_Value())
			{
				_stack.Push((Container)(object)new EventToast(_settings, "Weekly raid reset", "Refresh your kill-proofs · new week is live", ToastAccent.NewEvent, "raid", null, "weekly-reset-" + Guid.NewGuid().ToString("N").Substring(0, 8), "https://tyriaplanner.com", null, showSqjoin: false));
			}
			ScheduleNext();
		}

		public static TimeSpan TimeUntilNextResetUtc(DateTime nowUtc)
		{
			int daysUntilMonday = (int)(1 - nowUtc.DayOfWeek + 7) % 7;
			DateTime candidate = new DateTime(nowUtc.Year, nowUtc.Month, nowUtc.Day, 7, 30, 0, DateTimeKind.Utc).AddDays(daysUntilMonday);
			if (candidate <= nowUtc)
			{
				candidate = candidate.AddDays(7.0);
			}
			return candidate - nowUtc;
		}

		public void Dispose()
		{
			_timer?.Dispose();
			_timer = null;
		}
	}
}
