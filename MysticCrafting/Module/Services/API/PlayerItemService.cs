using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Models.Account;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Services.Recurring;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Services.API
{
	public class PlayerItemService : RecurringService, IPlayerItemService, IRecurringService
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		public override string Name => Common.LoadingPlayerItems;

		private IList<ItemAmount> Materials { get; set; }

		private IList<ItemAmount> BankItems { get; set; }

		private IList<ItemAmount> SharedInventoryItems { get; set; }

		public IDictionary<string, IList<ItemAmount>> CharacterInventoryItems { get; set; }

		public PlayerItemService(Gw2ApiManager apiManager)
		{
			_gw2ApiManager = apiManager;
		}

		public override async Task<string> LoadAsync()
		{
			if (_gw2ApiManager.HasPermissions(new TokenPermission[3]
			{
				TokenPermission.Account,
				TokenPermission.Inventories,
				TokenPermission.Characters
			}))
			{
				Task<IApiV2ObjectList<AccountMaterial>> apiMaterials = _gw2ApiManager.Gw2ApiClient.V2.Account.Materials.GetAsync();
				Task<IApiV2ObjectList<AccountItem>> bankItems = _gw2ApiManager.Gw2ApiClient.V2.Account.Bank.GetAsync();
				Task<IApiV2ObjectList<AccountItem>> sharedInventoryItems = _gw2ApiManager.Gw2ApiClient.V2.Account.Inventory.GetAsync();
				Task<IApiV2ObjectList<string>> characterNames = _gw2ApiManager.Gw2ApiClient.V2.Characters.IdsAsync();
				await Task.WhenAll(apiMaterials, bankItems, sharedInventoryItems, characterNames);
				Materials = (await apiMaterials).Select(ItemAmount.From).ToList();
				BankItems = (await bankItems).Select(ItemAmount.From)?.ToList();
				SharedInventoryItems = (await sharedInventoryItems).Select(ItemAmount.From).ToList();
				await LoadCharacterInventoriesAsync(await characterNames);
				return $"{Materials.Count()} materials, {BankItems.Count()} bank items, {SharedInventoryItems.Count()} shared inventory items, {CharacterInventoryItems.Count()} characters loaded";
			}
			throw new Exception("One or more of the required permissions are missing: Account, Inventories, Characters.");
		}

		public async Task LoadCharacterInventoriesAsync(IEnumerable<string> characterNames)
		{
			if (_gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Characters
			}))
			{
				Dictionary<string, IList<ItemAmount>> characterInventoryItems = (Dictionary<string, IList<ItemAmount>>)(CharacterInventoryItems = (from r in await Task.WhenAll(characterNames.Select(async (string id) => new
					{
						Id = id,
						Inventory = await _gw2ApiManager.Gw2ApiClient.V2.Characters[id].Inventory.GetAsync()
					}))
					where !string.IsNullOrEmpty(r.Id) && r.Inventory?.Bags != null
					select new KeyValuePair<string, IList<ItemAmount>>(r.Id, r.Inventory.Bags.SelectItemAmounts().ToList())).ToDictionary((KeyValuePair<string, IList<ItemAmount>> x) => x.Key, (KeyValuePair<string, IList<ItemAmount>> x) => x.Value));
			}
		}

		public int GetItemCount(int itemId)
		{
			int materialItemCount = GetMaterialItemCount(itemId);
			int bankCount = GetBankItemCount(itemId);
			int sharedInventoryCount = GetSharedInventoryCount(itemId);
			int characterInventoryCount = GetCharacterInventoryItemCount(itemId).Sum((KeyValuePair<string, int> c) => c.Value);
			return materialItemCount + bankCount + sharedInventoryCount + characterInventoryCount;
		}

		public int GetMaterialItemCount(int itemId)
		{
			return (Materials?.Where((ItemAmount m) => m.ItemId == itemId)?.Sum((ItemAmount a) => a.Count)).GetValueOrDefault();
		}

		public int GetBankItemCount(int itemId)
		{
			return (BankItems?.Where((ItemAmount m) => m != null && m.ItemId == itemId)?.Sum((ItemAmount a) => a.Count)).GetValueOrDefault();
		}

		public int GetSharedInventoryCount(int itemId)
		{
			return (SharedInventoryItems?.Where((ItemAmount m) => m != null && m.ItemId == itemId)?.Sum((ItemAmount a) => a.Count)).GetValueOrDefault();
		}

		public IDictionary<string, int> GetCharacterInventoryItemCount(int itemId)
		{
			if (CharacterInventoryItems == null || !CharacterInventoryItems.Any())
			{
				return new Dictionary<string, int>();
			}
			return CharacterInventoryItems.SelectItemAmounts(itemId);
		}
	}
}
