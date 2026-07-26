using System;
using System.Collections.Generic;
using System.Linq;
using Taskmaster.Models;

namespace Taskmaster.Services
{
	public static class TaskPresetService
	{
		public const string PsnaName = "Pact Supply Network Agents";

		public static TodoTask CreatePsna(int order)
		{
			TodoTask parent = new TodoTask
			{
				Name = "Pact Supply Network Agents",
				Order = order,
				Schedule = ResetScheduleType.Psna,
				PresetType = TaskPresetType.PactSupplyNetworkAgents,
				Notes = "Locations rotate daily at 08:00 UTC."
			};
			int childOrder = 0;
			foreach (TaskPresetSlot slot in PsnaRotation.Slots)
			{
				parent.Subtasks.Add(new TodoTask
				{
					Name = slot.ToString(),
					Order = childOrder++,
					Schedule = ResetScheduleType.Psna,
					PresetType = TaskPresetType.PactSupplyNetworkAgents,
					PresetSlot = slot
				});
			}
			return parent;
		}

		public static string ResolveName(TodoTask task, DateTime nowUtc)
		{
			if (task == null)
			{
				return "";
			}
			if (task.PresetType != TaskPresetType.PactSupplyNetworkAgents)
			{
				return task.Name;
			}
			if (task.PresetSlot == TaskPresetSlot.None)
			{
				return "Pact Supply Network Agents";
			}
			PsnaLocation location = PsnaRotation.GetLocation(task.PresetSlot, nowUtc);
			if (location != null)
			{
				return location.Region + ": " + location.Location;
			}
			return task.Name;
		}

		public static string ResolveClipboardContent(TodoTask task, DateTime nowUtc)
		{
			if (task == null)
			{
				return null;
			}
			if (task.PresetType != TaskPresetType.PactSupplyNetworkAgents)
			{
				return task.ClipboardContent;
			}
			if (task.PresetSlot != 0)
			{
				return PsnaRotation.GetLocation(task.PresetSlot, nowUtc)?.MapLink;
			}
			return string.Join(" ", from slot in PsnaRotation.Slots
				select PsnaRotation.GetLocation(slot, nowUtc)?.MapLink into link
				where !string.IsNullOrEmpty(link)
				select link);
		}

		public static bool ContainsPreset(TodoTab tab, TaskPresetType presetType)
		{
			return tab?.Tasks.Any((TodoTask task) => task.PresetType == presetType && task.PresetSlot == TaskPresetSlot.None) ?? false;
		}

		public static bool CanMoveTo(IEnumerable<TodoTask> tasks, TodoTab destination)
		{
			if (destination == null)
			{
				return false;
			}
			return (from task in tasks ?? Enumerable.Empty<TodoTask>()
				where task.IsManagedPresetParent
				select task.PresetType).Distinct().All((TaskPresetType type) => !ContainsPreset(destination, type));
		}

		public static bool ValidateTab(TodoTab tab)
		{
			if (tab?.Tasks == null)
			{
				return false;
			}
			HashSet<TaskPresetType> presetTypes = new HashSet<TaskPresetType>();
			foreach (TodoTask task in tab.Tasks)
			{
				if (task == null)
				{
					return false;
				}
				if (task.IsManagedPresetParent)
				{
					if (!presetTypes.Add(task.PresetType) || !ValidatePreset(task))
					{
						return false;
					}
				}
				else if (!ValidateOrdinaryTask(task))
				{
					return false;
				}
			}
			return true;
		}

		private static bool ValidateOrdinaryTask(TodoTask task)
		{
			if (task == null || task.PresetType != 0 || task.PresetSlot != 0)
			{
				return false;
			}
			if (task.Subtasks != null)
			{
				return task.Subtasks.All(ValidateOrdinaryTask);
			}
			return true;
		}

		private static bool ValidatePreset(TodoTask parent)
		{
			if (parent.PresetType != TaskPresetType.PactSupplyNetworkAgents || parent.PresetSlot != 0 || parent.Schedule != ResetScheduleType.Psna || parent.TargetCount != 1 || parent.LocalResetTime.HasValue || parent.ResetDuration.HasValue || parent.Subtasks == null || parent.Subtasks.Count != PsnaRotation.Slots.Count || parent.Subtasks.Any((TodoTask child) => child == null))
			{
				return false;
			}
			List<TodoTask> children = parent.Subtasks.OrderBy((TodoTask child) => child.Order).ToList();
			for (int index = 0; index < children.Count; index++)
			{
				TodoTask child2 = children[index];
				if (child2.PresetType != parent.PresetType || child2.PresetSlot != PsnaRotation.Slots[index] || child2.Schedule != ResetScheduleType.Psna || child2.TargetCount != 1 || child2.LocalResetTime.HasValue || child2.ResetDuration.HasValue || child2.IsOptional || child2.HasSubtasks || child2.Order != index)
				{
					return false;
				}
			}
			return true;
		}
	}
}
