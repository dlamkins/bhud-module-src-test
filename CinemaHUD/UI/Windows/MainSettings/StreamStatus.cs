using Microsoft.Xna.Framework;

namespace CinemaHUD.UI.Windows.MainSettings
{
	public class StreamStatus
	{
		private static readonly Color AvailableColor = new Color(100, 200, 100);

		public bool IsOnline { get; set; }

		public string Subtitle { get; set; }

		public Color SubtitleColor { get; set; } = Color.get_Gray();


		public string AvatarUrl { get; set; }

		public int ViewerCount { get; set; }

		public static StreamStatus Offline()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return new StreamStatus
			{
				IsOnline = false,
				Subtitle = "Offline",
				SubtitleColor = Color.get_Gray()
			};
		}

		public static StreamStatus Available()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return new StreamStatus
			{
				IsOnline = true,
				Subtitle = "Available",
				SubtitleColor = AvailableColor
			};
		}

		public static StreamStatus OnDemand()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			return new StreamStatus
			{
				IsOnline = true,
				Subtitle = "Playback Ready",
				SubtitleColor = new Color(180, 200, 220)
			};
		}

		public static StreamStatus Live(string gameName, int viewerCount)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			return new StreamStatus
			{
				IsOnline = true,
				Subtitle = string.Format("LIVE: {0} - {1:N0} viewers", gameName ?? "Streaming", viewerCount),
				SubtitleColor = AvailableColor,
				ViewerCount = viewerCount
			};
		}
	}
}
