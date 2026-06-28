using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Soeed.GuildGeoGuesser
{
	internal static class ModuleVersion
	{
		private const string FallbackVersion = "0.0.0";

		public static string Value { get; } = ReadFromEmbeddedManifest();


		private static string ReadFromEmbeddedManifest()
		{
			Assembly assembly = typeof(ModuleVersion).Assembly;
			string resourceName = assembly.GetManifestResourceNames().FirstOrDefault((string name) => name.EndsWith(".manifest.json", StringComparison.Ordinal));
			if (resourceName == null)
			{
				return "0.0.0";
			}
			using Stream stream = assembly.GetManifestResourceStream(resourceName);
			if (stream == null)
			{
				return "0.0.0";
			}
			using StreamReader reader = new StreamReader(stream);
			try
			{
				return JObject.Parse(reader.ReadToEnd())["version"]?.ToString() ?? "0.0.0";
			}
			catch
			{
				return "0.0.0";
			}
		}
	}
}
