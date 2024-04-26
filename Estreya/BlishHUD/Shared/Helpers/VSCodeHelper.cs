using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Estreya.BlishHUD.Shared.Helpers
{
	public static class VSCodeHelper
	{
		public const string SYSTEM_INSTALL_FOLDER = "C:\\Program Files\\Microsoft VS Code";

		public const string USER_INSTALL_FOLDER = "%USERPROFILE%\\AppData\\Local\\Programs\\Microsoft VS Code";

		public const string EXE_NAME = "code.exe";

		public const string SYSTEM_EXE = "C:\\Program Files\\Microsoft VS Code\\code.exe";

		public const string USER_EXE = "%USERPROFILE%\\AppData\\Local\\Programs\\Microsoft VS Code\\code.exe";

		public static Task Diff(string filePath1, string filePath2)
		{
			return Task.Run(delegate
			{
				if (string.IsNullOrEmpty(filePath1) || !File.Exists(filePath1))
				{
					throw new FileNotFoundException("Path \"" + filePath1 + "\" does not exist.");
				}
				if (string.IsNullOrEmpty(filePath2) || !File.Exists(filePath2))
				{
					throw new FileNotFoundException("Path \"" + filePath2 + "\" does not exist.");
				}
				object obj = GetExePath();
				if (string.IsNullOrWhiteSpace((string)obj))
				{
					throw new FileNotFoundException("Could not find VS Code installation.");
				}
				if (obj == null)
				{
					obj = "";
				}
				Process.Start((string)obj, "--wait --diff \"" + filePath1 + "\" \"" + filePath2 + "\"").WaitForExit();
			});
		}

		public static Task EditAsync(string filePath)
		{
			return Task.Run(delegate
			{
				if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
				{
					throw new FileNotFoundException("Path \"" + filePath + "\" does not exist.");
				}
				object obj = GetExePath();
				if (string.IsNullOrWhiteSpace((string)obj))
				{
					throw new FileNotFoundException("Could not find VS Code installation.");
				}
				if (obj == null)
				{
					obj = "";
				}
				Process.Start((string)obj, "--wait \"" + filePath + "\"").WaitForExit();
			});
		}

		public static string GetExePath()
		{
			string userExe = Environment.ExpandEnvironmentVariables("%USERPROFILE%\\AppData\\Local\\Programs\\Microsoft VS Code\\code.exe");
			if (!File.Exists("C:\\Program Files\\Microsoft VS Code\\code.exe"))
			{
				if (!File.Exists(userExe))
				{
					return null;
				}
				return userExe;
			}
			return "C:\\Program Files\\Microsoft VS Code\\code.exe";
		}
	}
}