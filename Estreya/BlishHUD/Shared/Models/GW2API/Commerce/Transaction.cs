using System;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Models.GW2API.Items;
using Humanizer;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.GW2API.Commerce
{
	public class Transaction
	{
		private static readonly Logger Logger = Logger.GetLogger<Transaction>();

		public int ItemId { get; set; }

		public int Price { get; set; }

		public int Quantity { get; set; }

		public DateTime Created { get; set; }

		public TransactionType Type { get; set; }

		[JsonIgnore]
		public Item Item { get; set; }

		public override string ToString()
		{
			return $"Item-ID: {ItemId} - Type: {Type.Humanize()} - Quantity: {Quantity} - Unit Price: {Price}";
		}

		public override bool Equals(object obj)
		{
			Transaction transaction = obj as Transaction;
			if (transaction != null && ItemId == transaction.ItemId)
			{
				return Type == transaction.Type;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((((17 * 23 + ItemId.GetHashCode()) * 23 + Price.GetHashCode()) * 23 + Quantity.GetHashCode()) * 23 + Created.GetHashCode()) * 23 + Type.GetHashCode();
		}
	}
}
