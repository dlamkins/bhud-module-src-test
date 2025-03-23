using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;

namespace gw2stacks_blish.reader
{
	internal class Gw2Api
	{
		private Gw2ApiManager manager;

		public Gw2Api(Gw2ApiManager manager_)
		{
			manager = manager_;
		}

		private async Task<Account> get_account_name()
		{
			return await manager.Gw2ApiClient.V2.Account.GetAsync();
		}

		public async Task<IApiV2ObjectList<AccountItem>> shared_inventory()
		{
			return await manager.Gw2ApiClient.V2.Account.Inventory.GetAsync();
		}

		public async Task<CharactersInventory> character_inventory(string name_)
		{
			return await manager.Gw2ApiClient.V2.Characters[name_].Inventory.GetAsync();
		}

		public async Task<IApiV2ObjectList<AccountItem>> bank()
		{
			return await manager.Gw2ApiClient.V2.Account.Bank.GetAsync();
		}

		public async Task<IApiV2ObjectList<AccountMaterial>> material_storage()
		{
			return await manager.Gw2ApiClient.V2.Account.Materials.GetAsync();
		}

		public async Task<IApiV2ObjectList<Character>> characters()
		{
			return await manager.Gw2ApiClient.V2.Characters.AllAsync();
		}

		public async Task<CommercePrices> item_price(int id_)
		{
			return await manager.Gw2ApiClient.V2.Commerce.Prices.GetAsync(id_);
		}

		public async Task<IReadOnlyList<CommercePrices>> item_prices(List<int> ids_)
		{
			return await manager.Gw2ApiClient.V2.Commerce.Prices.ManyAsync(ids_);
		}

		public async Task<IReadOnlyList<int>> item_price_ids()
		{
			return await manager.Gw2ApiClient.V2.Commerce.Prices.IdsAsync();
		}

		public async Task<Itemstat> item_information(int id_)
		{
			return await manager.Gw2ApiClient.V2.Itemstats.GetAsync(id_);
		}

		public async Task<IReadOnlyList<Item>> item_information_bulk(List<int> ids_)
		{
			return await manager.Gw2ApiClient.V2.Items.ManyAsync(ids_);
		}

		public async Task<IApiV2ObjectList<int>> recipe_ids()
		{
			return await manager.Gw2ApiClient.V2.Recipes.IdsAsync();
		}

		public async Task<IReadOnlyList<Recipe>> recipes(List<int> ids_)
		{
			return await manager.Gw2ApiClient.V2.Recipes.ManyAsync(ids_);
		}
	}
}
