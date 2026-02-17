using System.Text.Json.Serialization;

namespace BhModule.WebPeeper
{
	internal class NugetResource
	{
		[JsonPropertyName("@id")]
		public string Id { get; set; }

		[JsonPropertyName("@type")]
		public string Type { get; set; }

		public string Comment { get; set; }
	}
}
