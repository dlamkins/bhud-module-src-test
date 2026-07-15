using System;
using System.Collections.Generic;
using Taskmaster.Models;

namespace Taskmaster.Services
{
	public static class ResetEngine
	{
		private static readonly TimeSpan DailyAt = TimeSpan.Zero;

		private static readonly TimeSpan PsnaAt = new TimeSpan(8, 0, 0);

		private static readonly TimeSpan WeeklyAt = new TimeSpan(7, 30, 0);

		private static readonly TimeSpan MapBonusAt = new TimeSpan(20, 0, 0);

		private static readonly TimeSpan WvwEuAt = new TimeSpan(18, 0, 0);

		private static readonly TimeSpan WvwNaAt = new TimeSpan(2, 0, 0);

		public static DateTime? LastBoundary(TodoTask task, DateTime nowUtc, TimeZoneInfo localTz = null)
		{
			switch (task.Schedule)
			{
			case ResetScheduleType.DailyServer:
				return LastDaily(nowUtc, DailyAt);
			case ResetScheduleType.Psna:
				return LastDaily(nowUtc, PsnaAt);
			case ResetScheduleType.WeeklyServer:
				return LastWeekly(nowUtc, DayOfWeek.Monday, WeeklyAt);
			case ResetScheduleType.MapBonus:
				return LastWeekly(nowUtc, DayOfWeek.Thursday, MapBonusAt);
			case ResetScheduleType.WvwEu:
				return LastWeekly(nowUtc, DayOfWeek.Friday, WvwEuAt);
			case ResetScheduleType.WvwNa:
				return LastWeekly(nowUtc, DayOfWeek.Saturday, WvwNaAt);
			case ResetScheduleType.LocalTime:
				return LastLocalDaily(nowUtc, task.LocalResetTime ?? TimeSpan.Zero, localTz ?? TimeZoneInfo.Local);
			case ResetScheduleType.Duration:
			{
				DateTime? reference = task.LastCompletedUtc ?? task.LastActivityUtc;
				if (!reference.HasValue || !task.ResetDuration.HasValue)
				{
					return null;
				}
				return reference.Value + task.ResetDuration.Value;
			}
			default:
				return null;
			}
		}

		public static DateTime? NextBoundary(TodoTask task, DateTime nowUtc, TimeZoneInfo localTz = null)
		{
			switch (task.Schedule)
			{
			case ResetScheduleType.DailyServer:
				return LastDaily(nowUtc, DailyAt).AddDays(1.0);
			case ResetScheduleType.Psna:
				return LastDaily(nowUtc, PsnaAt).AddDays(1.0);
			case ResetScheduleType.WeeklyServer:
				return LastWeekly(nowUtc, DayOfWeek.Monday, WeeklyAt).AddDays(7.0);
			case ResetScheduleType.MapBonus:
				return LastWeekly(nowUtc, DayOfWeek.Thursday, MapBonusAt).AddDays(7.0);
			case ResetScheduleType.WvwEu:
				return LastWeekly(nowUtc, DayOfWeek.Friday, WvwEuAt).AddDays(7.0);
			case ResetScheduleType.WvwNa:
				return LastWeekly(nowUtc, DayOfWeek.Saturday, WvwNaAt).AddDays(7.0);
			case ResetScheduleType.LocalTime:
				return NextLocalDaily(nowUtc, task.LocalResetTime ?? TimeSpan.Zero, localTz ?? TimeZoneInfo.Local);
			case ResetScheduleType.Duration:
			{
				DateTime? b = LastBoundary(task, nowUtc);
				if (!b.HasValue || !(b.Value > nowUtc))
				{
					return null;
				}
				return b;
			}
			default:
				return null;
			}
		}

		public static int ApplyResets(IEnumerable<TodoTab> tabs, DateTime nowUtc, TimeZoneInfo localTz = null)
		{
			int applied = 0;
			foreach (TodoTab tab in tabs)
			{
				foreach (TodoTask task in tab.Tasks)
				{
					applied += ApplyResetRecursive(task, nowUtc, localTz);
				}
			}
			return applied;
		}

		private static int ApplyResetRecursive(TodoTask task, DateTime nowUtc, TimeZoneInfo localTz)
		{
			if (task.LastCompletedUtc.HasValue && task.LastCompletedUtc.Value > nowUtc)
			{
				task.LastCompletedUtc = nowUtc;
			}
			if (task.LastActivityUtc.HasValue && task.LastActivityUtc.Value > nowUtc)
			{
				task.LastActivityUtc = nowUtc;
			}
			if (task.HasSubtasks)
			{
				int applied = 0;
				foreach (TodoTask s in task.Subtasks)
				{
					applied += ApplyResetRecursive(s, nowUtc, localTz);
				}
				if (applied > 0)
				{
					task.SyncGroupAnchor(nowUtc);
				}
				return applied;
			}
			DateTime? boundary = LastBoundary(task, nowUtc, localTz);
			if (!boundary.HasValue)
			{
				return 0;
			}
			DateTime? reference = task.LastCompletedUtc ?? task.LastActivityUtc;
			if (!reference.HasValue)
			{
				return 0;
			}
			if (!((task.Schedule == ResetScheduleType.Duration) ? (nowUtc >= boundary.Value) : (reference.Value < boundary.Value)) || (task.CurrentCount == 0 && !task.LastCompletedUtc.HasValue))
			{
				return 0;
			}
			task.CurrentCount = 0;
			task.LastCompletedUtc = null;
			task.LastActivityUtc = null;
			return 1;
		}

		private static DateTime LastDaily(DateTime nowUtc, TimeSpan at)
		{
			DateTime candidate = nowUtc.Date + at;
			if (!(candidate <= nowUtc))
			{
				return candidate.AddDays(-1.0);
			}
			return candidate;
		}

		private static DateTime LastWeekly(DateTime nowUtc, DayOfWeek day, TimeSpan at)
		{
			int diff = (nowUtc.DayOfWeek - day + 7) % 7;
			DateTime candidate = nowUtc.Date.AddDays(-diff) + at;
			if (!(candidate <= nowUtc))
			{
				return candidate.AddDays(-7.0);
			}
			return candidate;
		}

		private static DateTime LastLocalDaily(DateTime nowUtc, TimeSpan localAt, TimeZoneInfo tz)
		{
			DateTime nowLocal = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, tz);
			DateTime candidateLocal = nowLocal.Date + localAt;
			if (candidateLocal > nowLocal)
			{
				candidateLocal = candidateLocal.AddDays(-1.0);
			}
			DateTime utc = SafeLocalToUtc(candidateLocal, tz);
			if (!(utc <= nowUtc))
			{
				return SafeLocalToUtc(candidateLocal.AddDays(-1.0), tz);
			}
			return utc;
		}

		private static DateTime NextLocalDaily(DateTime nowUtc, TimeSpan localAt, TimeZoneInfo tz)
		{
			DateTime nowLocal = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, tz);
			DateTime candidateLocal = nowLocal.Date + localAt;
			if (candidateLocal <= nowLocal)
			{
				candidateLocal = candidateLocal.AddDays(1.0);
			}
			DateTime utc = SafeLocalToUtc(candidateLocal, tz);
			if (!(utc > nowUtc))
			{
				return SafeLocalToUtc(candidateLocal.AddDays(1.0), tz);
			}
			return utc;
		}

		private static DateTime SafeLocalToUtc(DateTime local, TimeZoneInfo tz)
		{
			local = DateTime.SpecifyKind(local, DateTimeKind.Unspecified);
			if (tz.IsInvalidTime(local))
			{
				local = local.AddHours(1.0);
			}
			if (tz.IsAmbiguousTime(local))
			{
				TimeSpan[] ambiguousTimeOffsets = tz.GetAmbiguousTimeOffsets(local);
				TimeSpan max = ambiguousTimeOffsets[0];
				TimeSpan[] array = ambiguousTimeOffsets;
				foreach (TimeSpan o in array)
				{
					if (o > max)
					{
						max = o;
					}
				}
				return DateTime.SpecifyKind(local - max, DateTimeKind.Utc);
			}
			return TimeZoneInfo.ConvertTimeToUtc(local, tz);
		}
	}
}
