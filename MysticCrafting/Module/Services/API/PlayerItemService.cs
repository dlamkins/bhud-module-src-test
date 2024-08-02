using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Models.Account;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Services.API
{
	public class PlayerItemService : ApiService, IPlayerItemService, IApiService
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		public override string Name => Common.LoadingPlayerItems;

		private IList<ItemAmount> Materials { get; set; }

		private IList<ItemAmount> BankItems { get; set; }

		private IList<ItemAmount> SharedInventoryItems { get; set; }

		public IDictionary<string, IList<ItemAmount>> CharacterInventoryItems { get; set; }

		public override List<TokenPermission> Permissions => new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)5,
			(TokenPermission)3
		};

		public PlayerItemService(Gw2ApiManager apiManager)
			: base(apiManager)
		{
			_gw2ApiManager = apiManager;
		}

		public override async Task<string> LoadAsync()
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)Permissions))
			{
				return string.Empty;
			}
			Task<IApiV2ObjectList<AccountMaterial>> apiMaterials = ((IBlobClient<IApiV2ObjectList<AccountMaterial>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Materials()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<AccountItem>> bankItems = ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Bank()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<AccountItem>> sharedInventoryItems = ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Inventory()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<string>> characterNames = ((IBulkExpandableClient<Character, string>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).IdsAsync(default(CancellationToken));
			await Task.WhenAll(apiMaterials, bankItems, sharedInventoryItems, characterNames);
			Materials = ((IEnumerable<AccountMaterial>)(await apiMaterials)).Select(ItemAmount.From).ToList();
			BankItems = ((IEnumerable<AccountItem>)(await bankItems)).Select(ItemAmount.From)?.ToList();
			SharedInventoryItems = ((IEnumerable<AccountItem>)(await sharedInventoryItems)).Select(ItemAmount.From).ToList();
			await LoadCharacterInventoriesAsync((IEnumerable<string>)(await characterNames));
			return $"{Materials.Count()} materials, {BankItems.Count()} bank items, {SharedInventoryItems.Count()} shared inventory items, {CharacterInventoryItems.Count()} characters loaded";
		}

		public async Task LoadCharacterInventoriesAsync(IEnumerable<string> characterNames)
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)3
			}))
			{
				return;
			}
			Dictionary<string, IList<ItemAmount>> characterInventoryItems = (Dictionary<string, IList<ItemAmount>>)(CharacterInventoryItems = (from r in (await Task.WhenAll(characterNames.Select(async (string id) => new
				{
					Id = id,
					Inventory = await ((IBlobClient<CharactersInventory>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()
						.get_Item(id)
						.get_Inventory()).GetAsync(default(CancellationToken))
				}))).Where(r =>
				{
					if (!string.IsNullOrEmpty(r.Id))
					{
						CharactersInventory inventory = r.Inventory;
						return ((inventory != null) ? inventory.get_Bags() : null) != null;
					}
					return false;
				})
				select new KeyValuePair<string, IList<ItemAmount>>(r.Id, r.Inventory.get_Bags().SelectItemAmounts().ToList())).ToDictionary((KeyValuePair<string, IList<ItemAmount>> x) => x.Key, (KeyValuePair<string, IList<ItemAmount>> x) => x.Value));
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
