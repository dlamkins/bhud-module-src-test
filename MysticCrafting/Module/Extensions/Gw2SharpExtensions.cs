using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Models.Account;

namespace MysticCrafting.Module.Extensions
{
	public static class Gw2SharpExtensions
	{
		public static IEnumerable<ItemAmount> SelectItemAmounts(this IReadOnlyList<CharacterInventoryBag> bags)
		{
			return bags.Where((CharacterInventoryBag b) => ((b != null) ? b.get_Inventory() : null) != null).SelectMany((CharacterInventoryBag b) => (from i in b.get_Inventory()
				where i != null
				select i).Select(ItemAmount.From).ToList());
		}

		public static int GetItemCountSum(this IList<ItemAmount> itemAmounts, int itemId)
		{
			if (itemAmounts == null)
			{
				return 0;
			}
			return itemAmounts.Where((ItemAmount v) => v != null && v.ItemId == itemId)?.Sum((ItemAmount a) => a.Count) ?? 0;
		}

		public static IDictionary<string, int> SelectItemAmounts(this IDictionary<string, IList<ItemAmount>> characterInventoryItems, int itemId)
		{
			if (characterInventoryItems == null)
			{
				return new Dictionary<string, int>();
			}
			return characterInventoryItems.Select((KeyValuePair<string, IList<ItemAmount>> i) => new KeyValuePair<string, int>(i.Key, i.Value.GetItemCountSum(itemId)))?.ToDictionary((KeyValuePair<string, int> x) => x.Key, (KeyValuePair<string, int> x) => x.Value) ?? new Dictionary<string, int>();
		}
	}
}
