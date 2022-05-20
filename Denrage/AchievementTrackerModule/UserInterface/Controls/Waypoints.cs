using System.Text.Json.Serialization;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class Waypoints
	{
		public class Continent
		{
			[JsonPropertyName("name")]
			public string Name { get; set; }

			[JsonPropertyName("coord")]
			public double[] Coordinates { get; set; }

			[JsonPropertyName("id")]
			public int Id { get; set; }

			[JsonPropertyName("chat_link")]
			public string ChatLink { get; set; }

			[JsonPropertyName("floors")]
			public int[] Floors { get; set; }
		}

		[JsonPropertyName("1")]
		public Continent[] Tyria { get; set; }

		[JsonPropertyName("2")]
		public Continent[] Mists { get; set; }
	}
}
