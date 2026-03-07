using Blish_HUD.Content;
using CinemaModule.Models;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.Windows.MainSettings
{
	public class StreamListItem
	{
		public string Key { get; set; }

		public string Title { get; set; }

		public string Subtitle { get; set; }

		public Color SubtitleColor { get; set; } = Color.get_Gray();


		public AsyncTexture2D AvatarTexture { get; set; }

		public ChannelData ChannelData { get; set; }

		public string TwitchChannel { get; set; }

		public string AvatarUrl { get; set; }

		public bool IsOnline { get; set; }

		public bool IsOnDemand { get; set; }

		public int ViewerCount { get; set; }

		public int Index { get; set; }

		public void ApplyStatus(StreamStatus status)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			IsOnline = status.IsOnline;
			Subtitle = status.Subtitle;
			SubtitleColor = status.SubtitleColor;
			AvatarUrl = (string.IsNullOrEmpty(status.AvatarUrl) ? AvatarUrl : status.AvatarUrl);
			ViewerCount = status.ViewerCount;
		}
	}
}
