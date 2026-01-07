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

		private const string DEBUG_FOLDER = "C:\\git\\Maestro\\Debug";

		private readonly StringBuilder _log = new StringBuilder();

		private bool _enabled;

		private string _songName;

		[Conditional("DEBUG")]
		public void Start(string songName)
		{
			_log.Clear();
			_songName = songName;
			_enabled = true;
		}

		[Conditional("DEBUG")]
		public void Stop()
		{
			_enabled = false;
			if (_log.Length == 0)
			{
				return;
			}
			try
			{
				if (!Directory.Exists("C:\\git\\Maestro\\Debug"))
				{
					Directory.CreateDirectory("C:\\git\\Maestro\\Debug");
				}
				string safeFileName = SanitizeFileName(_songName);
				string logPath = Path.Combine("C:\\git\\Maestro\\Debug", safeFileName + ".txt");
				string header = "=== Debug Log for: " + _songName + " ===\n" + $"=== Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n\n";
				File.WriteAllText(logPath, header + _log);
				Logger.Info("Debug log written to: " + logPath);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to write debug log");
			}
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
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (_enabled)
			{
				_log.AppendLine($"NOTE: {key} -> {FormatKey(targetKey)}");
			}
		}

		[Conditional("DEBUG")]
		public void LogSharp(Keys key, KeyBinding binding)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (_enabled)
			{
				_log.AppendLine($"SHARP: {key} -> {FormatModifiers(binding.ModifierKeys)}+{FormatKey(binding.PrimaryKey)}");
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
			List<string> parts = new List<string>();
			if (mods.HasFlag(ModifierKeys.Ctrl))
			{
				parts.Add("Ctrl");
			}
			if (mods.HasFlag(ModifierKeys.Alt))
			{
				parts.Add("Alt");
			}
			if (mods.HasFlag(ModifierKeys.Shift))
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
