using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Taskmaster.Models;

namespace Taskmaster.Services
{
	public static class TabShare
	{
		private class Payload
		{
			public int Taskmaster;

			public string Tab;

			public List<TodoTask> Tasks;
		}

		public const int PayloadVersion = 1;

		public static string Export(TodoTab tab)
		{
			return JsonConvert.SerializeObject((object)new Payload
			{
				Taskmaster = 1,
				Tab = tab.Name,
				Tasks = tab.Tasks
			}, (Formatting)1);
		}

		public static TabShareImportResult TryImport(string json)
		{
			TabShareImportResult fail = new TabShareImportResult
			{
				Outcome = TabShareImportOutcome.NotATabExport
			};
			if (string.IsNullOrWhiteSpace(json))
			{
				return fail;
			}
			Payload payload;
			try
			{
				payload = JsonConvert.DeserializeObject<Payload>(json);
			}
			catch
			{
				return fail;
			}
			if (payload == null || payload.Taskmaster == 0 || payload.Tasks == null)
			{
				return fail;
			}
			if (payload.Taskmaster > 1)
			{
				return new TabShareImportResult
				{
					Outcome = TabShareImportOutcome.VersionTooNew
				};
			}
			TodoTab tab = new TodoTab
			{
				Id = Guid.NewGuid(),
				Name = (payload.Tab ?? "Imported")
			};
			tab.Tasks.AddRange(payload.Tasks);
			foreach (TodoTask task in tab.Tasks)
			{
				Sanitize(task);
			}
			return new TabShareImportResult
			{
				Outcome = TabShareImportOutcome.Success,
				Tab = tab
			};
		}

		private static void Sanitize(TodoTask task)
		{
			task.Id = Guid.NewGuid();
			task.CurrentCount = 0;
			task.LastCompletedUtc = null;
			task.LastActivityUtc = null;
			task.EnsureDurationAnchor(DateTime.UtcNow);
			if (task.Subtasks == null)
			{
				task.Subtasks = new List<TodoTask>();
			}
			foreach (TodoTask subtask in task.Subtasks)
			{
				Sanitize(subtask);
			}
		}
	}
}
