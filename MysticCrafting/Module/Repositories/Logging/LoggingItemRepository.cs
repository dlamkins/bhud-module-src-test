using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Repositories.Logging
{
	public class LoggingItemRepository : LoggingRepository, IItemRepository, IRepository
	{
		private static readonly Logger Logger = Logger.GetLogger<IItemRepository>();

		private readonly IItemRepository _itemRepository;

		public LoggingItemRepository(IItemRepository itemRepository)
			: base(itemRepository)
		{
			_itemRepository = itemRepository;
		}

		public IEnumerable<MysticItem> FilterItems(MysticItemFilter filter)
		{
			IEnumerable<MysticItem> items = _itemRepository.FilterItems(filter);
			if (items == null || !items.Any())
			{
				Logger.Info("No items were found using selected filter");
			}
			return items;
		}

		public MysticItem GetItem(int itemId)
		{
			MysticItem item = _itemRepository.GetItem(itemId);
			if (item == null)
			{
				Logger.Warn($"Item with ID '{itemId}' was not found.");
			}
			return item;
		}

		public IList<MysticItem> GetItems()
		{
			IList<MysticItem> items = _itemRepository.GetItems();
			if (items == null || !items.Any())
			{
				Logger.Warn("No items were found in the ItemRepository.");
			}
			return items;
		}
	}
}
