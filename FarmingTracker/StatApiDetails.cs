using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;

namespace FarmingTracker
{
	public class StatApiDetails
	{
		public string Name { get; set; } = string.Empty;


		public string Description { get; set; } = string.Empty;


		public ItemRarity Rarity { get; set; }

		public ItemType Type { get; set; }

		public ApiFlags<ItemFlag> ItemFlags { get; set; } = new ApiFlags<ItemFlag>((IEnumerable<ApiEnum<ItemFlag>>)new List<ApiEnum<ItemFlag>> { ApiEnum<ItemFlag>.op_Implicit((ItemFlag)0) });


		public int IconAssetId { get; set; }

		public long Unsigned_VendorValueInCopper { get; set; }

		public long Unsigned_SellsUnitPriceInCopper { get; set; }

		public long Unsigned_BuysUnitPriceInCopper { get; set; }

		public string WikiSearchTerm { get; set; } = string.Empty;


		public string ChatLink { get; set; } = string.Empty;


		public bool HasWikiSearchTerm => !string.IsNullOrWhiteSpace(WikiSearchTerm);

		public StatApiDetailsState State { get; set; }

		public bool IsCustomCoinStat
		{
			get
			{
				if (State != StatApiDetailsState.GoldCoinCustomStat && State != StatApiDetailsState.SilveCoinCustomStat)
				{
					return State == StatApiDetailsState.CopperCoinCustomStat;
				}
				return true;
			}
		}
	}
}
