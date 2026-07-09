using System;
using System.Collections.Generic;

namespace rp.spark.Models.Api
{
	public class NearbyPresenceListResponse
	{
		public DateTime ServerTime { get; set; } = DateTime.UtcNow;


		public List<NearbyPresence> Entries { get; set; } = new List<NearbyPresence>();

	}
}
