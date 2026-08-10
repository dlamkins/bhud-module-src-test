namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class ComponentHealthDto
	{
		public string Status { get; set; } = "healthy";


		public string? Message { get; set; }

		public long? ResponseTimeMs { get; set; }
	}
}
