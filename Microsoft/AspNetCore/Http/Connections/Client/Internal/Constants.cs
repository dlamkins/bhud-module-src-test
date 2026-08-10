using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal static class Constants
	{
		public static readonly string UserAgent = (OperatingSystem.IsBrowser() ? "X-SignalR-User-Agent" : "User-Agent");

		public static readonly string UserAgentHeader = GetUserAgentHeader();

		private static string GetUserAgentHeader()
		{
			AssemblyInformationalVersionAttribute assemblyInformationalVersionAttribute = typeof(Constants).Assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>().FirstOrDefault();
			string runtime = ".NET";
			string frameworkDescription = RuntimeInformation.FrameworkDescription;
			return ConstructUserAgent(typeof(Constants).Assembly.GetName().Version, assemblyInformationalVersionAttribute.InformationalVersion, GetOS(), runtime, frameworkDescription);
		}

		private static string GetOS()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return "Windows NT";
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return "macOS";
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				return "Linux";
			}
			return "";
		}

		public static string ConstructUserAgent(Version version, string detailedVersion, string os, string runtime, string runtimeVersion)
		{
			string text = $"Microsoft SignalR/{version.Major}.{version.Minor} (";
			text = (string.IsNullOrEmpty(detailedVersion) ? (text + "Unknown Version") : (text + detailedVersion));
			text = (string.IsNullOrEmpty(os) ? (text + "; Unknown OS") : (text + "; " + os));
			text = text + "; " + runtime;
			text = (string.IsNullOrEmpty(runtimeVersion) ? (text + "; Unknown Runtime Version") : (text + "; " + runtimeVersion));
			return text + ")";
		}
	}
}
