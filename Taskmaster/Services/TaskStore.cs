using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Newtonsoft.Json;
using Taskmaster.Models;

namespace Taskmaster.Services
{
	public class TaskStore
	{
		private class Envelope
		{
			public int Version = 3;

			public List<TodoTab> Tabs = new List<TodoTab>();
		}

		private static readonly Logger Logger = Logger.GetLogger<TaskStore>();

		public const int CurrentVersion = 3;

		private static readonly TimeSpan DebounceDelay = TimeSpan.FromSeconds(2.0);

		private readonly string _filePath;

		private bool _dirty;

		private DateTime _dirtySinceUtc;

		public List<TodoTab> Tabs { get; private set; } = new List<TodoTab>();


		public bool ReadOnly { get; private set; }

		public TaskStore(string directory)
		{
			_filePath = Path.Combine(directory, "tasks.json");
		}

		public TaskStoreLoadResult Load()
		{
			TaskStoreLoadResult result = new TaskStoreLoadResult();
			if (!File.Exists(_filePath))
			{
				result.Outcome = TaskStoreLoadOutcome.StartedEmpty;
				return result;
			}
			Envelope primary = TryParse(_filePath);
			if (primary != null)
			{
				if (primary.Version > 3)
				{
					Logger.Warn($"tasks.json is version {primary.Version}, newer than supported {3}; read-only mode");
					ReadOnly = true;
					result.Outcome = TaskStoreLoadOutcome.VersionTooNew;
					return result;
				}
				Tabs = primary.Tabs ?? new List<TodoTab>();
				result.Outcome = TaskStoreLoadOutcome.LoadedPrimary;
				return result;
			}
			result.QuarantinedPath = Path.Combine(Path.GetDirectoryName(_filePath), $"tasks.corrupt-{DateTime.UtcNow:yyyyMMdd-HHmmss}.json");
			File.Move(_filePath, result.QuarantinedPath);
			Logger.Warn("tasks.json was corrupt; quarantined to " + result.QuarantinedPath);
			string bakPath = _filePath + ".bak";
			if (File.Exists(bakPath))
			{
				Envelope backup = TryParse(bakPath);
				if (backup != null && backup.Version <= 3)
				{
					Tabs = backup.Tabs ?? new List<TodoTab>();
					result.Outcome = TaskStoreLoadOutcome.LoadedBackup;
					return result;
				}
			}
			result.Outcome = TaskStoreLoadOutcome.StartedEmptyAfterCorruption;
			return result;
		}

		private static Envelope TryParse(string path)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Envelope envelope = JsonConvert.DeserializeObject<Envelope>(File.ReadAllText(path));
				if (envelope != null && envelope.Version <= 3 && envelope.Tabs != null && !envelope.Tabs.All(TaskPresetService.ValidateTab))
				{
					throw new JsonSerializationException("Task data contains invalid managed preset structure.");
				}
				return envelope;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to parse " + path);
				return null;
			}
		}

		public void MarkDirty(DateTime nowUtc)
		{
			if (!_dirty)
			{
				_dirtySinceUtc = nowUtc;
			}
			_dirty = true;
		}

		public void FlushIfDue(DateTime nowUtc)
		{
			if (_dirty && nowUtc - _dirtySinceUtc >= DebounceDelay)
			{
				Save();
			}
		}

		public void Save()
		{
			_dirty = false;
			if (!ReadOnly)
			{
				string json = JsonConvert.SerializeObject((object)new Envelope
				{
					Version = 3,
					Tabs = Tabs
				}, (Formatting)1);
				string tmp = _filePath + ".tmp";
				File.WriteAllText(tmp, json);
				if (File.Exists(_filePath))
				{
					File.Replace(tmp, _filePath, _filePath + ".bak");
				}
				else
				{
					File.Move(tmp, _filePath);
				}
			}
		}
	}
}
