using System;
using System.Collections.Generic;
using System.Linq;
using Taskmaster.Models;

namespace Taskmaster.UI
{
	public static class TaskEditValidation
	{
		public static string Validate(string name, ResetScheduleType schedule, string localTime, string duration, string count, bool hasSubtasks, IEnumerable<string> subtaskNames)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return "Enter a task name.";
			}
			if (schedule == ResetScheduleType.LocalTime && (!TimeSpan.TryParse(localTime, out var localAt) || localAt < TimeSpan.Zero || localAt >= TimeSpan.FromDays(1.0)))
			{
				return "Local time needs to use HH:mm, for example 19:30.";
			}
			if (schedule == ResetScheduleType.Duration && (!TimeSpan.TryParse(duration, out var cooldown) || cooldown <= TimeSpan.Zero))
			{
				return "Cooldown needs to be greater than zero, for example 1:00:00.";
			}
			if (!hasSubtasks && (!int.TryParse(count, out var targetCount) || targetCount < 1 || targetCount > 999))
			{
				return "Count needs to be a whole number from 1 to 999.";
			}
			if (subtaskNames != null && subtaskNames.Any(string.IsNullOrWhiteSpace))
			{
				return "Each subtask needs a name. Enter a name or delete the empty row.";
			}
			return null;
		}
	}
}
