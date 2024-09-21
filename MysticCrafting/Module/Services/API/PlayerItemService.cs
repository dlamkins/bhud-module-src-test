using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Account;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Models.Account;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Services.API
{
	public class PlayerItemService : ApiService, IPlayerItemService, IApiService, IDisposable
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		private string _currentCharacterName;

		public override string Name => Common.LoadingPlayerItems;

		private IList<ItemAmount> Materials { get; set; }

		private IList<ItemAmount> BankItems { get; set; }

		private IList<ItemAmount> SharedInventoryItems { get; set; }

		public Dictionary<string, IList<ItemAmount>> CharacterInventoryItems { get; set; } = new Dictionary<string, IList<ItemAmount>>();


		private List<string> _allCharacterNames { get; set; } = new List<string>();


		private List<CharacterActivity> _characterActivity { get; set; } = new List<CharacterActivity>();


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
			_currentCharacterName = GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacterOnNameChanged);
		}

		public override async Task<string> LoadAsync()
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)Permissions))
			{
				return string.Empty;
			}
			Task<IApiV2ObjectList<AccountMaterial>> apiMaterialsTask = ((IBlobClient<IApiV2ObjectList<AccountMaterial>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Materials()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<AccountItem>> backItemsTask = ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Bank()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<AccountItem>> sharedInventoryItemsTask = ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Inventory()).GetAsync(default(CancellationToken));
			List<Task> tasks = new List<Task> { apiMaterialsTask, backItemsTask, sharedInventoryItemsTask };
			if (!_allCharacterNames.Any())
			{
				tasks.Add(LoadAllCharacterInventoriesAsync());
			}
			else if (_characterActivity.Any())
			{
				tasks.Add(LoadActiveCharacterInventoriesAsync());
			}
			await Task.WhenAll(tasks);
			Convert((IEnumerable<AccountMaterial>)(await apiMaterialsTask), ItemStorageType.MaterialStorage).ToList();
			Materials = ((IEnumerable<AccountMaterial>)(await apiMaterialsTask)).Select(ItemAmount.From).ToList();
			BankItems = ((IEnumerable<AccountItem>)(await backItemsTask)).Select(ItemAmount.From)?.ToList();
			SharedInventoryItems = ((IEnumerable<AccountItem>)(await sharedInventoryItemsTask)).Select(ItemAmount.From).ToList();
			return $"{Materials.Count()} materials, {BankItems.Count()} bank items, {SharedInventoryItems.Count()} shared inventory items";
		}

		public List<PlayerItem> Convert(IEnumerable<AccountMaterial> items, ItemStorageType type)
		{
			return items.Select((AccountMaterial i) => new PlayerItem
			{
				ItemId = i.get_Id(),
				Count = i.get_Count(),
				StorageType = type
			}).ToList();
		}

		private async Task LoadAllCharacterInventoriesAsync()
		{
			_allCharacterNames = ((IEnumerable<string>)(await ((IBulkExpandableClient<Character, string>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).IdsAsync(default(CancellationToken)))).ToList();
			await LoadCharacterInventoriesAsync(_allCharacterNames);
		}

		private Task LoadActiveCharacterInventoriesAsync()
		{
			List<string> activeCharacterNames = (from a in _characterActivity
				where DateTime.Now > a.LastDataUpdate.AddMinutes(3.0) && DateTime.Now < a.LastActive.AddMinutes(10.0)
				select a.CharacterName).ToList();
			return LoadCharacterInventoriesAsync(activeCharacterNames);
		}

		public async Task LoadCharacterInventoriesAsync(IList<string> characterNames)
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)3
			}))
			{
				return;
			}
			Dictionary<string, IList<ItemAmount>> characterInventoryItems = (from r in (await Task.WhenAll(characterNames.Select(async (string characterName) => new
				{
					CharacterName = characterName,
					Inventory = await ((IBlobClient<CharactersInventory>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()
						.get_Item(characterName)
						.get_Inventory()).GetAsync(default(CancellationToken))
				}))).Where(r =>
				{
					if (!string.IsNullOrEmpty(r.CharacterName))
					{
						CharactersInventory inventory = r.Inventory;
						return ((inventory != null) ? inventory.get_Bags() : null) != null;
					}
					return false;
				})
				select new KeyValuePair<string, IList<ItemAmount>>(r.CharacterName, r.Inventory.get_Bags().SelectItemAmounts().ToList())).ToDictionary((KeyValuePair<string, IList<ItemAmount>> x) => x.Key, (KeyValuePair<string, IList<ItemAmount>> x) => x.Value);
			CharacterInventoryItems = CharacterInventoryItems.Where((KeyValuePair<string, IList<ItemAmount>> kv) => !characterNames.Contains(kv.Key)).Concat(characterInventoryItems).ToDictionary((KeyValuePair<string, IList<ItemAmount>> kv) => kv.Key, (KeyValuePair<string, IList<ItemAmount>> kv) => kv.Value);
			if (!_characterActivity.Any())
			{
				return;
			}
			foreach (string name in characterNames)
			{
				CharacterActivity characterActivity = _characterActivity.FirstOrDefault((CharacterActivity a) => a.CharacterName.Equals(name));
				if (characterActivity != null)
				{
					characterActivity.LastDataUpdate = DateTime.Now;
				}
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

		private void PlayerCharacterOnNameChanged(object sender, ValueEventArgs<string> e)
		{
			_currentCharacterName = e.get_Value();
			CharacterActivity characterActivity = _characterActivity.FirstOrDefault((CharacterActivity a) => a.CharacterName.Equals(_currentCharacterName));
			if (characterActivity != null)
			{
				characterActivity.LastActive = DateTime.Now;
				return;
			}
			_characterActivity.Add(new CharacterActivity
			{
				CharacterName = _currentCharacterName,
				LastActive = DateTime.Now
			});
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacterOnNameChanged);
		}
	}
}
