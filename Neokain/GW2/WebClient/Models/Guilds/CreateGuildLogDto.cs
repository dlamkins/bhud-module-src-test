namespace Neokain.GW2.WebClient.Models.Guilds
{
	internal class CreateGuildLogDto
	{
		public string Type { get; set; } = string.Empty;


		public string? User { get; set; }

		public string Message { get; set; } = string.Empty;

	}
}
