using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Extended;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.Services
{
	internal class Gw2WebApiService : IDisposable
	{
		public readonly TokenPermission[] Requires;

		public Gw2WebApiService()
		{
			TokenPermission[] array = new TokenPermission[5];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			Requires = (TokenPermission[])(object)array;
			base._002Ector();
		}

		public void Dispose()
		{
		}

		public async Task<List<string>> GetClears()
		{
			return ((IEnumerable<string>)(((object)(await TaskUtil.TryAsync(() => ((IBlobClient<IApiV2ObjectList<string>>)(object)ProofLogix.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Raids()).GetAsync(default(CancellationToken))))) ?? ((object)Enumerable.Empty<string>()))).ToList();
		}

		public async Task<List<AccountItem>> GetBank()
		{
			return FilterProofs((IEnumerable<AccountItem>)(await TaskUtil.TryAsync(() => ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)ProofLogix.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Bank()).GetAsync(default(CancellationToken))))).ToList();
		}

		public async Task<List<AccountItem>> GetSharedBags()
		{
			return FilterProofs((IEnumerable<AccountItem>)(await TaskUtil.TryAsync(() => ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)ProofLogix.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Inventory()).GetAsync(default(CancellationToken))))).ToList();
		}

		public async Task<Dictionary<Character, List<AccountItem>>> GetBagsByCharacter()
		{
			IEnumerable<Character> obj = await GetCharacters();
			Dictionary<Character, List<AccountItem>> bagsByCharacter = new Dictionary<Character, List<AccountItem>>();
			foreach (Character character in obj)
			{
				if (character.get_Bags() != null)
				{
					IEnumerable<CharacterInventoryBag> bags = from bag in character.get_Bags()
						where bag != null
						select bag;
					List<AccountItem> filtered = FilterProofs(bags.SelectMany((CharacterInventoryBag bag) => bag.get_Inventory())).ToList();
					if (filtered.Any())
					{
						bagsByCharacter.Add(character, filtered);
					}
				}
			}
			return bagsByCharacter;
		}

		public async Task<IReadOnlyList<Item>> GetItems(params int[] itemIds)
		{
			return (await TaskUtil.TryAsync(() => ((IBulkExpandableClient<Item, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
				.get_Items()).ManyAsync((IEnumerable<int>)itemIds, default(CancellationToken)))) ?? Enumerable.Empty<Item>().ToList();
		}

		public async Task<IReadOnlyList<Map>> GetMaps(params int[] mapIds)
		{
			return (await TaskUtil.TryAsync(() => ((IBulkExpandableClient<Map, int>)(object)ProofLogix.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).ManyAsync((IEnumerable<int>)mapIds, default(CancellationToken)))) ?? Enumerable.Empty<Map>().ToList();
		}

		private async Task<IEnumerable<Character>> GetCharacters()
		{
			return (IEnumerable<Character>)(((object)(await TaskUtil.TryAsync(() => ((IAllExpandableClient<Character>)(object)ProofLogix.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken))))) ?? ((object)Enumerable.Empty<Character>()));
		}

		private IEnumerable<AccountItem> FilterProofs(IEnumerable<AccountItem> items)
		{
			List<Resource> resources = ProofLogix.Instance.Resources.GetItems();
			return items?.Where((AccountItem item) => item != null && resources.Select((Resource res) => res.Id).Contains(item.get_Id())) ?? Enumerable.Empty<AccountItem>();
		}
	}
}
