namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class SetMaintenanceModeRequest
	{
		public bool Enabled { get; set; }

		public string? Message { get; set; }
	}
}
