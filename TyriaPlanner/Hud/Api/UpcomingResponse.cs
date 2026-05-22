using System;
using Newtonsoft.Json;

namespace TyriaPlanner.Hud.Api
{
	public sealed class UpcomingResponse
	{
		[JsonProperty("mySignups")]
		public MySignup[] MySignups { get; set; } = Array.Empty<MySignup>();


		[JsonProperty("newGuildEvents")]
		public NewGuildEvent[] NewGuildEvents { get; set; } = Array.Empty<NewGuildEvent>();


		[JsonProperty("newAnnouncements")]
		public Announcement[] NewAnnouncements { get; set; } = Array.Empty<Announcement>();


		[JsonProperty("serverTime")]
		public DateTime ServerTime { get; set; }
	}
}
