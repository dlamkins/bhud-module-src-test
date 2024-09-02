using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;

namespace FarmingTracker
{
	public class ApiStatDetails
	{
		public string Name { get; set; } = string.Empty;


		public string Description { get; set; } = string.Empty;


		public ItemRarity Rarity { get; set; }

		public ItemType Type { get; set; }

		public ApiFlags<ItemFlag> ItemFlags { get; set; } = new ApiFlags<ItemFlag>((IEnumerable<ApiEnum<ItemFlag>>)new List<ApiEnum<ItemFlag>> { ApiEnum<ItemFlag>.op_Implicit((ItemFlag)0) });


		public int IconAssetId { get; set; }

		public long VendorValueInCopper { get; set; }

		public long SellsUnitPriceInCopper { get; set; }

		public long BuysUnitPriceInCopper { get; set; }

		public string WikiSearchTerm { get; set; } = string.Empty;


		public bool HasWikiSearchTerm => !string.IsNullOrWhiteSpace(WikiSearchTerm);

		public ApiStatDetailsState State { get; set; }

		public bool IsCustomCoinStat
		{
			get
			{
				if (State != ApiStatDetailsState.GoldCoinCustomStat && State != ApiStatDetailsState.SilveCoinCustomStat)
				{
					return State == ApiStatDetailsState.CopperCoinCustomStat;
				}
				return true;
			}
		}
	}
}
