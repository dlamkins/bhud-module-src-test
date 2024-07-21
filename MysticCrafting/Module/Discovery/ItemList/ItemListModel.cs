using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListModel
	{
		private readonly IItemRepository _itemRepository;

		public IList<string> Breadcrumbs { get; set; }

		public MysticItemFilter Filter { get; set; }

		public int ItemLimit { get; set; } = 100;


		public ItemListModel(IItemRepository itemRepository)
		{
			_itemRepository = itemRepository;
		}

		public IEnumerable<Item> GetFilteredItems()
		{
			return _itemRepository.FilterItems(Filter).Take(ItemLimit);
		}
	}
}
