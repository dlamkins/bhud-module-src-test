using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Blish_HUD.Extended
{
	public class BrowserUtil
	{
		private static Logger Logger = Logger.GetLogger<BrowserUtil>();

		private static int WM_SYSCOMMAND = 274;

		private static int SC_RESTORE = 61728;

		private const int SC_MAXIMIZE = 61488;

		private const int SC_MINIMIZE = 61472;

		private const uint SW_SHOW = 5u;

		public static async Task Open(string url)
		{
			if (Uri.TryCreate(url, UriKind.Absolute, out var uri) && uri.IsFile)
			{
				return;
			}
			url = url.Trim().Trim('"').Trim();
			string command = GetDefaultBrowserCommand(url);
			if (string.IsNullOrEmpty(command))
			{
				Process.Start(url);
				return;
			}
			string[] args = CommandLineToArgs(command);
			string exe = args[0];
			string argString = string.Join(" ", from s in args.Skip(1)
				select (!s.Equals("%1")) ? s : s.Replace("%1", url));
			ProcessStartInfo psi = new ProcessStartInfo(exe, argString)
			{
				WorkingDirectory = (Path.GetDirectoryName(exe) ?? Directory.GetCurrentDirectory())
			};
			try
			{
				Process.Start(psi);
			}
			catch (Exception ex) when (ex is IOException || ex is Win32Exception)
			{
				Logger.Warn("Failed to run '" + exe + "' with arguments '" + argString + "': " + ex.Message);
				Process.Start(url);
				return;
			}
			string title = await TryFetchWebPageTitle(url);
			Process proc = GetProcessWithWindowByName(Path.GetFileNameWithoutExtension(psi.FileName), title);
			if (proc != null)
			{
				ForceForegroundWindow(proc.MainWindowHandle);
			}
		}

		private static Process GetProcessWithWindowByName(string name, string windowTitle = null)
		{
			List<Process> processes = (from p in Process.GetProcessesByName(name)
				where !p.MainWindowHandle.Equals(IntPtr.Zero)
				select p).ToList();
			if (processes.Count == 0)
			{
				return null;
			}
			if (windowTitle == null)
			{
				return processes[0];
			}
			return processes.FirstOrDefault((Process p) => p.MainWindowTitle.Contains(windowTitle)) ?? processes[0];
		}

		private static async Task<string> TryFetchWebPageTitle(string url)
		{
			WebRequest webRequest = WebRequest.Create(url);
			webRequest.UseDefaultCredentials = true;
			return await webRequest.GetResponseAsync().ContinueWith(async delegate(Task<WebResponse> task)
			{
				if (task.IsFaulted)
				{
					return string.Empty;
				}
				WebResponse response = task.Result;
				if (response.Headers.AllKeys.Contains("Content-Type") && response.Headers["Content-Type"].StartsWith("text/html"))
				{
					string page = await new WebClient
					{
						UseDefaultCredentials = true
					}.DownloadStringTaskAsync(url).ContinueWith((Task<string> t) => (!t.IsFaulted) ? t.Result : string.Empty);
					return new Regex("(?<=<title.*>)([\\s\\S]*)(?=</title>)", RegexOptions.IgnoreCase).Match(page).Value.Trim();
				}
				return string.Empty;
			}).Unwrap();
		}

		private static void ForceForegroundWindow(IntPtr hWnd)
		{
			if (!(hWnd == (IntPtr)0) && !hWnd.Equals(IntPtr.Zero))
			{
				uint foreThread = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
				uint appThread = GetCurrentThreadId();
				if (foreThread != appThread)
				{
					AttachThreadInput(foreThread, appThread, fAttach: true);
					BringWindowToTop(hWnd);
					ShowWindow(hWnd, 5u);
					AttachThreadInput(foreThread, appThread, fAttach: false);
				}
				else
				{
					BringWindowToTop(hWnd);
					ShowWindow(hWnd, 5u);
				}
				SendMessage(hWnd, WM_SYSCOMMAND, 61488, 0);
			}
		}

		private static string GetDefaultBrowserCommand(string url)
		{
			string protocol = Uri.UriSchemeHttp;
			if (Regex.IsMatch(url, "^[a-z]+" + Regex.Escape(Uri.SchemeDelimiter), RegexOptions.IgnoreCase))
			{
				int schemeEnd = url.IndexOf(Uri.SchemeDelimiter, StringComparison.Ordinal);
				if (schemeEnd > -1)
				{
					protocol = url.Substring(0, schemeEnd).ToLowerInvariant();
				}
			}
			object userProtocol;
			using (RegistryKey userDefBrowserKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\" + protocol + "\\UserChoice"))
			{
				if (userDefBrowserKey == null || (userProtocol = userDefBrowserKey.GetValue("Progid")) == null)
				{
					return string.Empty;
				}
			}
			using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\" + userProtocol?.ToString() + "\\shell\\open\\command"))
			{
				object command;
				if (registryKey != null && (command = registryKey.GetValue(null)) != null)
				{
					return (string)command;
				}
			}
			using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\" + userProtocol?.ToString() + "\\shell\\open\\command"))
			{
				object command;
				if (registryKey2 != null && (command = registryKey2.GetValue(null)) != null)
				{
					return (string)command;
				}
			}
			using (RegistryKey defBrowserKey = Registry.ClassesRoot.OpenSubKey(userProtocol?.ToString() + "\\shell\\open\\command"))
			{
				object command;
				if (defBrowserKey != null && (command = defBrowserKey.GetValue(null)) != null)
				{
					return (string)command;
				}
			}
			return string.Empty;
		}

		private static string[] CommandLineToArgs(string commandLine)
		{
			int count;
			IntPtr argsPtr = CommandLineToArgvW(commandLine, out count);
			if (argsPtr == IntPtr.Zero)
			{
				return Array.Empty<string>();
			}
			try
			{
				string[] args = new string[count];
				for (int i = 0; i < args.Length; i++)
				{
					IntPtr p = Marshal.ReadIntPtr(argsPtr, i * IntPtr.Size);
					args[i] = Marshal.PtrToStringUni(p);
				}
				return args;
			}
			finally
			{
				Marshal.FreeHGlobal(argsPtr);
			}
		}

		[DllImport("shell32.dll", SetLastError = true)]
		private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

		[DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

		[DllImport("kernel32.dll")]
		private static extern uint GetCurrentThreadId();

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool BringWindowToTop(IntPtr hWnd);

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

		[DllImport("user32.dll")]
		private static extern bool SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
	}
}
