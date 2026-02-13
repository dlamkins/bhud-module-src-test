using System;

namespace CinemaModule.Player
{
	public class VideoPlayerOptions
	{
		public bool EnableHardwareAcceleration { get; set; } = true;


		public int NetworkCachingMs { get; set; } = 1000;


		public int MaxWidth { get; set; } = 1920;


		public int MaxHeight { get; set; } = 1080;


		public int PreferredResolution { get; set; } = 1080;


		public string[] AdditionalLibVlcOptions { get; set; } = Array.Empty<string>();


		public static VideoPlayerOptions Default => new VideoPlayerOptions();
	}
}
