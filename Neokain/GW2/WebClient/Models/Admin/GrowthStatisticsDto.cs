using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class GrowthStatisticsDto
	{
		public List<DateCountDto> UserGrowth { get; set; } = new List<DateCountDto>();


		public List<DateCountDto> GuildGrowth { get; set; } = new List<DateCountDto>();


		public List<DateCountDto> AccountGrowth { get; set; } = new List<DateCountDto>();

	}
}
