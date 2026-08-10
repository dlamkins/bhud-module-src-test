namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminAuditLogDetailDto : AdminAuditLogDto
	{
		public string? OldValues { get; set; }

		public string? NewValues { get; set; }

		public string? IpAddress { get; set; }
	}
}
