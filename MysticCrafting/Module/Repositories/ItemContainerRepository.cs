using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class ItemContainerRepository : IItemContainerRepository
	{
		private IList<MysticItemContainer> ItemContainers { get; set; }

		public bool LocalOnly => false;

		public bool Loaded { get; private set; }

		public string FileName => "item_container_data.json";

		public ItemContainerRepository(IDataService dataService)
		{
			ItemContainers = new List<MysticItemContainer>
			{
				new MysticItemContainer
				{
					ItemId = 123456699,
					ContainedItemId = 102929,
					Name = "Map completion of Lowland Shore"
				},
				new MysticItemContainer
				{
					ItemId = 123456698,
					ContainedItemId = 102958,
					Name = "Map completion of Janthir Syntri"
				}
			};
		}

		public IList<MysticItemContainer> GetItemContainers(int itemId)
		{
			return ItemContainers?.Where((MysticItemContainer v) => v.ContainedItemId == itemId)?.ToList();
		}
	}
}
