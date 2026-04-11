using System;

namespace CinemaModule.Services.YouTube
{
	public class YouTubePlaylistVideo
	{
		public string VideoId { get; set; }

		public string Title { get; set; }

		public string Author { get; set; }

		public string ThumbnailUrl { get; set; }

		public TimeSpan Duration { get; set; }
	}
}
