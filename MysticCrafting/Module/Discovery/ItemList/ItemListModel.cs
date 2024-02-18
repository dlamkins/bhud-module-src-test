using System.Collections.Generic;
using System.Linq;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListModel
	{
		private readonly IItemRepository _itemRepository;

		public IList<string> Breadcrumbs { get; set; }

		public MysticItemFilter Filter { get; set; }

		public int ItemLimit { get; set; } = 50;


		public ItemListModel(IItemRepository itemRepository)
		{
			_itemRepository = itemRepository;
		}

		public IEnumerable<MysticItem> GetFilteredItems()
		{
			return _itemRepository.FilterItems(Filter).Take(ItemLimit);
		}
	}
}
