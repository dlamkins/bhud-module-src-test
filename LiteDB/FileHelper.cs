using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LiteDB
{
	internal static class FileHelper
	{
		public static string GetSufixFile(string filename, string suffix = "-temp", bool checkIfExists = true)
		{
			int count = 0;
			string temp = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + suffix + Path.GetExtension(filename));
			while (checkIfExists && File.Exists(temp))
			{
				string directoryName = Path.GetDirectoryName(filename);
				string[] obj = new string[5]
				{
					Path.GetFileNameWithoutExtension(filename),
					suffix,
					"-",
					null,
					null
				};
				int num = ++count;
				obj[3] = num.ToString();
				obj[4] = Path.GetExtension(filename);
				temp = Path.Combine(directoryName, string.Concat(obj));
			}
			return temp;
		}

		public static string GetLogFile(string filename)
		{
			return GetSufixFile(filename, "-log", checkIfExists: false);
		}

		public static string GetTempFile(string filename)
		{
			return GetSufixFile(filename, "-tmp", checkIfExists: false);
		}

		public static bool IsFileLocked(string filename)
		{
			FileStream stream = null;
			FileInfo file = new FileInfo(filename);
			try
			{
				stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (IOException ex)
			{
				return ex.IsLocked();
			}
			finally
			{
				stream?.Dispose();
			}
			return false;
		}

		public static bool TryExec(Action action, TimeSpan timeout)
		{
			DateTime timer = DateTime.UtcNow.Add(timeout);
			do
			{
				try
				{
					action();
					return true;
				}
				catch (IOException ex)
				{
					ex.WaitIfLocked(25);
				}
			}
			while (DateTime.UtcNow < timer);
			return false;
		}

		public static long ParseFileSize(string size)
		{
			Match match = Regex.Match(size, "^(\\d+)\\s*([tgmk])?(b|byte|bytes)?$", RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				return 0L;
			}
			long num = Convert.ToInt64(match.Groups[1].Value);
			string text = match.Groups[2].Value.ToLower();
			switch (text)
			{
			default:
				if (text.Length != 0)
				{
					break;
				}
				return num;
			case "t":
				return num * 1024 * 1024 * 1024 * 1024;
			case "g":
				return num * 1024 * 1024 * 1024;
			case "m":
				return num * 1024 * 1024;
			case "k":
				return num * 1024;
			case null:
				break;
			}
			return 0L;
		}

		public static string FormatFileSize(long byteCount)
		{
			string[] suf = new string[5] { "B", "KB", "MB", "GB", "TB" };
			if (byteCount == 0L)
			{
				return "0" + suf[0];
			}
			long num2 = Math.Abs(byteCount);
			long place = Convert.ToInt64(Math.Floor(Math.Log(num2, 1024.0)));
			double num = Math.Round((double)num2 / Math.Pow(1024.0, place), 1);
			return (double)Math.Sign(byteCount) * num + suf[place];
		}
	}
}
