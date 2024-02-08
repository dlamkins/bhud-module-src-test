using System.Collections.Generic;
using MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Repositories
{
	public interface IItemRepository : IRepository
	{
		MysticItem GetItem(int itemId);

		IList<MysticItem> GetItems();

		IEnumerable<MysticItem> FilterItems(MysticItemFilter filter);
	}
}
