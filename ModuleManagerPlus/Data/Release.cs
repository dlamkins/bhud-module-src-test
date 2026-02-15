using Newtonsoft.Json;
using SemVer;

namespace ModuleManagerPlus.Data
{
	internal class Release
	{
		private Version _typedVersion;

		[JsonProperty("Version")]
		public string Version { get; set; }

		public bool IsPrerelease { get; set; }

		public Version TypedVersion => _typedVersion ?? (_typedVersion = new Version(Version));

		public string DownloadUrl { get; set; }
	}
}
