using System;
using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public interface IItemRepository : IDisposable
	{
		void Initialize(IDataService service);

		Item GetItem(int itemId);

		IList<int> GetItemIds();

		IEnumerable<Item> FilterItems(MysticItemFilter filter);
	}
}
