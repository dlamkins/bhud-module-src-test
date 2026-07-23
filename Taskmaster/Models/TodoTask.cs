using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Taskmaster.Models
{
	public class TodoTask
	{
		public Guid Id { get; set; } = Guid.NewGuid();


		public string Name { get; set; } = "";


		public int Order { get; set; }

		public ResetScheduleType Schedule { get; set; } = ResetScheduleType.DailyServer;


		public TaskPresetType PresetType { get; set; }

		public TaskPresetSlot PresetSlot { get; set; }

		public TimeSpan? LocalResetTime { get; set; }

		public TimeSpan? ResetDuration { get; set; }

		public string ClipboardContent { get; set; }

		public string Notes { get; set; }

		public int TargetCount { get; set; } = 1;


		public int CurrentCount { get; set; }

		public DateTime? LastCompletedUtc { get; set; }

		public DateTime? LastActivityUtc { get; set; }

		public List<TodoTask> Subtasks { get; set; } = new List<TodoTask>();


		[JsonIgnore]
		public bool HasSubtasks
		{
			get
			{
				if (Subtasks != null)
				{
					return Subtasks.Count > 0;
				}
				return false;
			}
		}

		[JsonIgnore]
		public bool IsManagedPreset => PresetType != TaskPresetType.None;

		[JsonIgnore]
		public bool IsManagedPresetParent
		{
			get
			{
				if (IsManagedPreset)
				{
					return PresetSlot == TaskPresetSlot.None;
				}
				return false;
			}
		}

		[JsonIgnore]
		public bool IsManagedPresetChild
		{
			get
			{
				if (IsManagedPreset)
				{
					return PresetSlot != TaskPresetSlot.None;
				}
				return false;
			}
		}

		[JsonIgnore]
		public bool IsDone
		{
			get
			{
				if (!HasSubtasks)
				{
					return CurrentCount >= TargetCount;
				}
				return Subtasks.All((TodoTask s) => s.IsDone);
			}
		}

		public void Increment(DateTime nowUtc)
		{
			if (HasSubtasks)
			{
				CompleteAll(nowUtc);
			}
			else if (CurrentCount < TargetCount)
			{
				CurrentCount++;
				LastActivityUtc = nowUtc;
				if (CurrentCount >= TargetCount)
				{
					LastCompletedUtc = nowUtc;
				}
			}
		}

		public void Decrement()
		{
			if (HasSubtasks)
			{
				UncheckAll();
			}
			else if (CurrentCount > 0)
			{
				CurrentCount--;
				if (CurrentCount < TargetCount)
				{
					LastCompletedUtc = null;
				}
			}
		}

		public void CompleteAll(DateTime nowUtc)
		{
			if (HasSubtasks)
			{
				foreach (TodoTask subtask in Subtasks)
				{
					subtask.CompleteAll(nowUtc);
				}
			}
			else if (CurrentCount < TargetCount)
			{
				CurrentCount = TargetCount;
				LastActivityUtc = nowUtc;
				LastCompletedUtc = nowUtc;
			}
		}

		public void UncheckAll()
		{
			if (HasSubtasks)
			{
				foreach (TodoTask subtask in Subtasks)
				{
					subtask.UncheckAll();
				}
			}
			else
			{
				CurrentCount = 0;
				LastCompletedUtc = null;
			}
		}

		public void SyncGroupAnchor(DateTime nowUtc)
		{
			if (HasSubtasks)
			{
				LastActivityUtc = nowUtc;
				LastCompletedUtc = (IsDone ? new DateTime?(nowUtc) : null);
			}
		}

		public void EnsureDurationAnchor(DateTime nowUtc)
		{
			if (Schedule == ResetScheduleType.Duration && !LastCompletedUtc.HasValue && !LastActivityUtc.HasValue)
			{
				LastActivityUtc = nowUtc;
			}
		}
	}
}
