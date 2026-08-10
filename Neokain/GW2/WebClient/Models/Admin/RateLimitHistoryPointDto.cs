using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal sealed class RateLimitHistoryPointDto
	{
		public DateTimeOffset Timestamp { get; set; }

		public int? LowWaterTokens { get; set; }

		public long Waits { get; set; }
	}
}
