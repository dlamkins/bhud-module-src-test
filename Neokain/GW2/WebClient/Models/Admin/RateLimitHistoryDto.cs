using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal sealed class RateLimitHistoryDto
	{
		public int Capacity { get; set; }

		public List<RateLimitHistoryPointDto> Points { get; set; } = new List<RateLimitHistoryPointDto>();

	}
}
