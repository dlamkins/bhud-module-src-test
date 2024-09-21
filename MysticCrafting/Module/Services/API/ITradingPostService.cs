using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Services.API
{
	public interface ITradingPostService : IApiService
	{
		event EventHandler<ItemPriceChangedEventArgs> ItemPriceChanged;

		Task<IList<Item>> UpdatePricesSafeAsync(IList<Item> items);
	}
}
