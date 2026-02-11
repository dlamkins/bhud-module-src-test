using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	[Serializable]
	public class BossSlotRotation
	{
		[JsonProperty("slot")]
		public int Slot { get; set; }

		[JsonProperty("offset")]
		public int Offset { get; set; }

		[JsonProperty("encounters")]
		public List<string> Encounters { get; set; } = new List<string>();

	}
}
