using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BhModule.WebPeeper
{
	internal class NugetIndex
	{
		[JsonPropertyName("version")]
		public string Version { get; set; }

		[JsonPropertyName("resources")]
		public List<NugetResource> Resources { get; set; }
	}
}
