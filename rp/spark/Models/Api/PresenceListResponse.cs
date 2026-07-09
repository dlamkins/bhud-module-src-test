using System;
using System.Collections.Generic;

namespace rp.spark.Models.Api
{
	public class PresenceListResponse
	{
		public DateTime ServerTime { get; set; } = DateTime.UtcNow;


		public List<PlayerPresence> Entries { get; set; } = new List<PlayerPresence>();

	}
}
