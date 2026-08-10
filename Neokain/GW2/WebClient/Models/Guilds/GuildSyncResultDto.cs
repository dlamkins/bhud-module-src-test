namespace Neokain.GW2.WebClient.Models.Guilds
{
	internal class GuildSyncResultDto
	{
		public bool Success { get; set; }

		public int StashTabsImported { get; set; }

		public int StashItemsImported { get; set; }

		public int StashChangesDetected { get; set; }

		public int TreasuryItemsImported { get; set; }

		public int StorageItemsImported { get; set; }

		public int StorageChangesDetected { get; set; }

		public string? Error { get; set; }
	}
}
