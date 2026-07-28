using System;
using System.Net;
using System.Reflection;

namespace Manlaan.CommanderMarkers.Library.Services
{
	public static class ModuleHttp
	{
		public const string UserAgentPrefix = "COMM-MARKERS-Blish";

		private static string? _cachedVersion;

		public static string UserAgent => "COMM-MARKERS-Blish/" + ResolveVersion();

		public static WebClient CreateClient()
		{
			return new WebClient
			{
				Headers = { [HttpRequestHeader.UserAgent] = UserAgent }
			};
		}

		private static string ResolveVersion()
		{
			if (!string.IsNullOrEmpty(_cachedVersion))
			{
				return _cachedVersion;
			}
			try
			{
				Module module = Service.ModuleInstance;
				if (module != null)
				{
					string value = ((object)module).GetType().GetProperty("Version", BindingFlags.Instance | BindingFlags.Public)?.GetValue(module)?.ToString();
					if (!string.IsNullOrWhiteSpace(value))
					{
						_cachedVersion = value;
						return _cachedVersion;
					}
				}
			}
			catch (Exception)
			{
			}
			_cachedVersion = "0.0.0";
			return _cachedVersion;
		}
	}
}
