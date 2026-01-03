using System.Text.Json.Serialization;

namespace LoreBridge.Translation.Models
{
	public sealed class DeepLResponseError
	{
		public sealed class ResponseError
		{
			[JsonPropertyName("message")]
			public string Message { get; set; }
		}

		[JsonPropertyName("error")]
		public ResponseError Error { get; set; }
	}
}
