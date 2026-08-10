using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class ActivityStatisticsDto
	{
		public List<DateCountDto> DailyActiveUsers { get; set; } = new List<DateCountDto>();


		public List<DateCountDto> DailyRequests { get; set; } = new List<DateCountDto>();


		public List<FeatureCountDto> TopFeatures { get; set; } = new List<FeatureCountDto>();

	}
}
