using System.Collections.Generic;
using Blish_HUD.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CinemaModule.Models
{
	public class StreamCategory
	{
		private const string RadioTypeString = "radio";

		private const string TwitchTypeString = "twitch";

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		[JsonProperty("infoUrl")]
		public string InfoUrl { get; set; }

		[JsonProperty("type")]
		public string TypeString { get; set; } = "stream";


		[JsonProperty("channels")]
		public JToken ChannelsRaw { get; set; }

		[JsonIgnore]
		public StreamCategoryType CategoryType
		{
			get
			{
				string type = TypeString?.ToLowerInvariant();
				if (type == "radio")
				{
					return StreamCategoryType.Radio;
				}
				if (type == "twitch")
				{
					return StreamCategoryType.Twitch;
				}
				return StreamCategoryType.Stream;
			}
		}

		[JsonIgnore]
		public bool IsTwitch => CategoryType == StreamCategoryType.Twitch;

		[JsonIgnore]
		public List<ChannelData> Channels { get; set; } = new List<ChannelData>();


		[JsonIgnore]
		public List<string> TwitchChannelNames { get; set; } = new List<string>();


		[JsonIgnore]
		public AsyncTexture2D IconTexture { get; set; }

		public void ParseChannels()
		{
			if (ChannelsRaw != null)
			{
				if (IsTwitch)
				{
					TwitchChannelNames = ChannelsRaw.ToObject<List<string>>() ?? new List<string>();
				}
				else
				{
					Channels = ChannelsRaw.ToObject<List<ChannelData>>() ?? new List<ChannelData>();
				}
			}
		}
	}
}
