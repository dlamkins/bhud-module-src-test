using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoreBridge.Translation.Models
{
	public class LibreTranslateResponse
	{
		[JsonPropertyName("alternatives")]
		public List<string> Alternatives { get; set; }

		[JsonPropertyName("detectedLanguage")]
		public DetectedLanguage DetectedLanguage { get; set; }

		[JsonPropertyName("translatedText")]
		public string TranslatedText { get; set; }
	}
}
