namespace CinemaModule.Models
{
	public class TwitchStreamQuality
	{
		public string DisplayName { get; }

		public string StreamUrl { get; }

		public TwitchStreamQuality(string displayName, string streamUrl)
		{
			DisplayName = displayName;
			StreamUrl = streamUrl;
		}
	}
}
