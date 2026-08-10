using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AuditLogStatisticsDto
	{
		public int TotalLogs { get; set; }

		public int LogsToday { get; set; }

		public int LogsThisWeek { get; set; }

		public List<ActionCountDto> TopActions { get; set; } = new List<ActionCountDto>();


		public List<EntityCountDto> TopEntities { get; set; } = new List<EntityCountDto>();

	}
}
