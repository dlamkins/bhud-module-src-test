using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListModel
	{
		public MysticItemFilter Filter = new MysticItemFilter
		{
			IsFavorite = true
		};

		public EventHandler FilterChanged;

		private readonly IItemRepository _itemRepository;

		public IList<string> Breadcrumbs { get; set; }

		public int ItemLimit => 300;

		public ItemListModel(IItemRepository itemRepository)
		{
			_itemRepository = itemRepository;
		}

		public IEnumerable<Item> GetFilteredItems()
		{
			return _itemRepository.FilterItems(Filter).Take(ItemLimit);
		}

		public void InvokeFilterChanged()
		{
			FilterChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
