using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Blish_HUD;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services
{
	public class DebugLogger
	{
		private static readonly Logger Logger = Logger.GetLogger<DebugLogger>();

		private const string DEBUG_FOLDER = "C:\\git\\perso\\Maestro\\SongsDebug";

		private readonly StringBuilder _log = new StringBuilder();

		private readonly Stopwatch _stopwatch = new Stopwatch();

		private long _lastEventMs;

		private bool _enabled;

		private bool _hasLoggedNotes;

		private string _songName;

		[Conditional("DEBUG")]
		public void Start(string songName)
		{
			_log.Clear();
			_songName = songName;
			_enabled = true;
			_hasLoggedNotes = false;
			_lastEventMs = 0L;
			_stopwatch.Restart();
		}

		[Conditional("DEBUG")]
		public void Stop()
		{
			_stopwatch.Stop();
			_enabled = false;
			if (!_hasLoggedNotes || _log.Length == 0)
			{
				return;
			}
			_hasLoggedNotes = false;
			try
			{
				if (!Directory.Exists("C:\\git\\perso\\Maestro\\SongsDebug"))
				{
					Directory.CreateDirectory("C:\\git\\perso\\Maestro\\SongsDebug");
				}
				string logPath = GetUniqueLogPath(SanitizeFileName(_songName));
				string header = "=== Debug Log for: " + _songName + " ===\n" + $"=== Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n\n";
				File.WriteAllText(logPath, header + _log);
				Logger.Info("Debug log written to: " + logPath);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to write debug log");
			}
		}

		private static string GetUniqueLogPath(string baseName)
		{
			string basePath = Path.Combine("C:\\git\\perso\\Maestro\\SongsDebug", baseName + ".txt");
			if (!File.Exists(basePath))
			{
				return basePath;
			}
			int counter = 1;
			string newPath;
			do
			{
				newPath = Path.Combine("C:\\git\\perso\\Maestro\\SongsDebug", $"{baseName} - {counter}.txt");
				counter++;
			}
			while (File.Exists(newPath));
			return newPath;
		}

		private static string SanitizeFileName(string name)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			StringBuilder sanitized = new StringBuilder(name);
			char[] array = invalidFileNameChars;
			foreach (char c in array)
			{
				sanitized.Replace(c, '_');
			}
			return sanitized.ToString();
		}

		[Conditional("DEBUG")]
		public void Log(string message)
		{
			if (_enabled)
			{
				_log.AppendLine(message);
			}
		}

		[Conditional("DEBUG")]
		public void LogNote(Keys key, Keys targetKey)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (_enabled)
			{
				_hasLoggedNotes = true;
				long currentMs = _stopwatch.ElapsedMilliseconds;
				long deltaMs = currentMs - _lastEventMs;
				_lastEventMs = currentMs;
				_log.AppendLine(string.Format("[+{0,4}ms] NOTE: {1} -> {2}", deltaMs, key, FormatKey(targetKey)));
			}
		}

		[Conditional("DEBUG")]
		public void LogSharp(Keys key, KeyBinding binding)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			if (_enabled)
			{
				_hasLoggedNotes = true;
				long currentMs = _stopwatch.ElapsedMilliseconds;
				long deltaMs = currentMs - _lastEventMs;
				_lastEventMs = currentMs;
				_log.AppendLine(string.Format("[+{0,4}ms] SHARP: {1} -> {2}+{3}", deltaMs, key, FormatModifiers(binding.get_ModifierKeys()), FormatKey(binding.get_PrimaryKey())));
			}
		}

		[Conditional("DEBUG")]
		public void LogDelay(int delayMs)
		{
			if (_enabled)
			{
				_log.AppendLine($"         DELAY: {delayMs}ms");
			}
		}

		private static string FormatKey(Keys key)
		{
			string name = ((object)(Keys)(ref key)).ToString();
			if (name.StartsWith("D") && name.Length == 2 && char.IsDigit(name[1]))
			{
				return name[1].ToString();
			}
			if (name.StartsWith("NumPad"))
			{
				return "Num" + name.Substring(6);
			}
			return name;
		}

		private static string FormatModifiers(ModifierKeys mods)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			List<string> parts = new List<string>();
			if (((Enum)mods).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				parts.Add("Ctrl");
			}
			if (((Enum)mods).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				parts.Add("Alt");
			}
			if (((Enum)mods).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				parts.Add("Shift");
			}
			if (parts.Count <= 0)
			{
				return "";
			}
			return string.Join("+", parts);
		}
	}
}
