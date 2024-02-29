using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class ItemContainerRepository : IItemContainerRepository, IRepository
	{
		private readonly IDataService _dataService;

		private IList<MysticItemContainer> ItemContainers { get; set; }

		public bool LocalOnly => false;

		public bool Loaded { get; private set; }

		public string FileName => "item_container_data.json";

		public ItemContainerRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			ItemContainers = (await _dataService.LoadFromFileAsync<List<MysticItemContainer>>(FileName)) ?? new List<MysticItemContainer>();
			Loaded = true;
			return $"{ItemContainers.Count} item containers loaded";
		}

		public IList<MysticItemContainer> GetItemContainers(int itemId)
		{
			return ItemContainers.Where((MysticItemContainer v) => v.ContainedItemId == itemId).ToList();
		}
	}
}
