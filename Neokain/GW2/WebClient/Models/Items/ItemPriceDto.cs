namespace Neokain.GW2.WebClient.Models.Items
{
	public class ItemPriceDto
	{
		public int ItemId { get; set; }

		public int BuyPrice { get; set; }

		public int BuyQuantity { get; set; }

		public int SellPrice { get; set; }

		public int SellQuantity { get; set; }

		public bool Whitelisted { get; set; }

		public string FetchedAt { get; set; } = string.Empty;

	}
}
