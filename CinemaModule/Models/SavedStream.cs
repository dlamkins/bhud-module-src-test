using System;
using CinemaModule.Models.Location;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class SavedStream
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("sourceType")]
		public StreamSourceType SourceType { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("tabId")]
		public string TabId { get; set; }

		[JsonIgnore]
		public bool IsYouTubeChannelOrPlaylist
		{
			get
			{
				if (SourceType != StreamSourceType.YouTubeChannel)
				{
					return SourceType == StreamSourceType.YouTubePlaylist;
				}
				return true;
			}
		}

		[JsonConstructor]
		public SavedStream()
		{
		}

		public SavedStream(string name, StreamSourceType sourceType, string value, string tabId = null)
		{
			Id = IdGenerator.Generate();
			CreatedAt = DateTime.UtcNow;
			Name = name;
			SourceType = sourceType;
			Value = value;
			TabId = tabId;
		}
	}
}
