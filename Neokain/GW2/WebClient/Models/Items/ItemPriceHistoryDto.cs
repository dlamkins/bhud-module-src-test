namespace Neokain.GW2.WebClient.Models.Items
{
	internal class ItemPriceHistoryDto
	{
		public int BuyPrice { get; set; }

		public int BuyQuantity { get; set; }

		public int SellPrice { get; set; }

		public int SellQuantity { get; set; }

		public string RecordedAt { get; set; } = string.Empty;

	}
}
