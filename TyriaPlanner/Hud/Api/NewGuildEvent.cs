using System;
using Newtonsoft.Json;

namespace TyriaPlanner.Hud.Api
{
	public sealed class NewGuildEvent : EventBase
	{
		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }
	}
}
