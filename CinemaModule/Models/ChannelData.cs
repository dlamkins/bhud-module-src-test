using System;
using Blish_HUD.Content;
using CinemaModule.Models.Location;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class ChannelData : StreamDataBase
	{
		private static readonly string[] OnDemandExtensions = new string[5] { ".mp4", ".webm", ".mkv", ".avi", ".mov" };

		private static readonly string[] OnDemandPathSegments = new string[2] { "/vod/", "/video/" };

		[JsonProperty("id")]
		public override string Id { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonIgnore]
		public override string Name => Title;

		[JsonProperty("type")]
		public override string TypeString { get; set; } = "video";


		[JsonProperty("url")]
		public override string Url { get; set; }

		[JsonProperty("infoUrl")]
		public override string InfoUrl { get; set; }

		[JsonProperty("youtubeUrl")]
		public string YoutubeUrl { get; set; }

		[JsonProperty("waypoint")]
		public string Waypoint { get; set; }

		[JsonProperty("avatar")]
		public string Avatar { get; set; }

		[JsonProperty("staticImage")]
		public override string StaticImage { get; set; }

		[JsonProperty("twitchName")]
		public string TwitchName { get; set; }

		[JsonProperty("youtubePlaylistUrl")]
		public string YoutubePlaylistUrl { get; set; }

		[JsonProperty("youtubeChannelId")]
		public string YoutubeChannelId { get; set; }

		[JsonProperty("youtubePlaylistCount")]
		public int YoutubePlaylistCount { get; set; }

		[JsonProperty("position")]
		public WorldPosition3D Position { get; set; }

		[JsonProperty("screenWidth")]
		public float? ScreenWidth { get; set; }

		[JsonProperty("asylumInfo")]
		public bool AsylumInfo { get; set; }

		[JsonIgnore]
		public bool IsTwitchChannel => string.Equals(TypeString, "twitch", StringComparison.OrdinalIgnoreCase);

		[JsonIgnore]
		public bool IsYouTubePlaylist => string.Equals(TypeString, "youtube_playlist", StringComparison.OrdinalIgnoreCase);

		[JsonIgnore]
		public bool HasYouTubePlaylistSource
		{
			get
			{
				if (string.IsNullOrEmpty(YoutubeChannelId))
				{
					return !string.IsNullOrEmpty(YoutubePlaylistUrl);
				}
				return true;
			}
		}

		[JsonIgnore]
		public bool HasWorldPosition
		{
			get
			{
				if (Position != null && ScreenWidth.HasValue)
				{
					return ScreenWidth.Value > 0f;
				}
				return false;
			}
		}

		[JsonIgnore]
		public bool IsOnDemand => IsOnDemandUrl(Url);

		[JsonIgnore]
		public AsyncTexture2D AvatarTexture { get; set; }

		private static bool IsOnDemandUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return false;
			}
			string lower = url.ToLowerInvariant();
			string[] onDemandExtensions = OnDemandExtensions;
			foreach (string ext in onDemandExtensions)
			{
				if (lower.EndsWith(ext))
				{
					return true;
				}
			}
			onDemandExtensions = OnDemandPathSegments;
			foreach (string segment in onDemandExtensions)
			{
				if (lower.Contains(segment))
				{
					return true;
				}
			}
			return false;
		}

		public StreamPresetData ToStreamPresetData()
		{
			return new StreamPresetData
			{
				Id = Id,
				NameValue = Title,
				TypeString = TypeString,
				Url = Url,
				InfoUrl = InfoUrl,
				StaticImage = StaticImage,
				StaticImageTexture = base.StaticImageTexture,
				AsylumInfo = AsylumInfo
			};
		}
	}
}
