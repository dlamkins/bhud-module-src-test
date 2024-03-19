using System;
using Newtonsoft.Json;

namespace Nekres.Music_Mixer.Core.Services.YtDlp
{
	public class MetaData
	{
		[JsonIgnore]
		public bool IsError { get; private set; }

		[JsonProperty("id")]
		public string Id { get; private set; }

		[JsonProperty("title")]
		public string Title { get; private set; }

		[JsonProperty("webpage_url")]
		public string Url { get; private set; }

		[JsonProperty("uploader")]
		public string Uploader { get; private set; }

		[JsonProperty("duration")]
		[JsonConverter(typeof(TimeSpanFromSecondsConverter))]
		public TimeSpan Duration { get; private set; }

		public MetaData(string id, string title, string url, string uploader, TimeSpan duration)
		{
			Id = id ?? string.Empty;
			Title = title ?? string.Empty;
			Url = url ?? string.Empty;
			Uploader = uploader ?? string.Empty;
			Duration = duration;
			IsError = string.IsNullOrWhiteSpace(Id) || !Url.IsWebLink() || Duration.TotalSeconds < 1.0;
		}
	}
}
