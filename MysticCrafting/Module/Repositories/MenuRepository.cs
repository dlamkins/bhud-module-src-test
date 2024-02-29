using System.Collections.Generic;
using System.Threading.Tasks;
using MysticCrafting.Models.Menu;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class MenuRepository : IMenuRepository, IRepository
	{
		private readonly IDataService _dataService;

		private IList<MysticMenuItem> MenuItems { get; set; }

		public bool LocalOnly => false;

		public bool Loaded { get; private set; }

		public string FileName => "menu_data.json";

		public MenuRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			MenuItems = (await _dataService.LoadFromFileAsync<List<MysticMenuItem>>(FileName)) ?? new List<MysticMenuItem>();
			Loaded = true;
			return $"{MenuItems.Count} menu items loaded";
		}

		public IList<MysticMenuItem> GetMenuItems()
		{
			return MenuItems;
		}
	}
}
