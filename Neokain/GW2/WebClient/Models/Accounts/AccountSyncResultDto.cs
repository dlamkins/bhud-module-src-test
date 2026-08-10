namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class AccountSyncResultDto
	{
		public bool Success { get; set; }

		public int BankTabsImported { get; set; }

		public int BankItemsImported { get; set; }

		public int BankChangesDetected { get; set; }

		public int MaterialsImported { get; set; }

		public int MaterialChangesDetected { get; set; }

		public int CharactersImported { get; set; }

		public int BagsImported { get; set; }

		public int CharacterItemsImported { get; set; }

		public int EquipmentImported { get; set; }

		public int CharacterChangesDetected { get; set; }

		public string? Error { get; set; }
	}
}
