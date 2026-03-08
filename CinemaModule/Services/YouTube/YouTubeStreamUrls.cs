namespace CinemaModule.Services.YouTube
{
	public readonly struct YouTubeStreamUrls
	{
		public string VideoUrl { get; }

		public string AudioUrl { get; }

		public bool HasSeparateAudio => !string.IsNullOrEmpty(AudioUrl);

		public YouTubeStreamUrls(string videoUrl, string audioUrl = null)
		{
			VideoUrl = videoUrl;
			AudioUrl = audioUrl;
		}
	}
}
