using System.Text.Json.Serialization;

namespace LoreBridge.Translation.Models
{
	public class DetectedLanguage
	{
		[JsonPropertyName("confidence")]
		public double Confidence { get; set; }

		[JsonPropertyName("language")]
		public string Language { get; set; }
	}
}
