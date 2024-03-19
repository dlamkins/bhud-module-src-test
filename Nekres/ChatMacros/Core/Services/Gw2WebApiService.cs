using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Flurl.Http;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ChatMacros.Core.Services
{
	internal class Gw2WebApiService : IDisposable
	{
		private readonly IReadOnlyList<TokenPermission> _requires = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)6,
			(TokenPermission)5,
			(TokenPermission)3,
			(TokenPermission)2
		};

		public bool HasSubtoken;

		public IReadOnlyList<TokenPermission> MissingPermissions;

		private Regex _apiKeyPattern = new Regex("^[A-F0-9]{8}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{20}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{12}$");

		private string _baseApiUrl = "https://api.guildwars2.com/";

		public Gw2WebApiService()
		{
			MissingPermissions = new List<TokenPermission>();
			ChatMacros.Instance.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
		}

		public void Dispose()
		{
			ChatMacros.Instance.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
		}

		public bool IsApiDown(out string message)
		{
			try
			{
				HttpResponseMessage response = GeneratedExtensions.GetAsync(SettingsExtensions.AllowHttpStatus<IFlurlRequest>(GeneratedExtensions.AllowHttpStatus(_baseApiUrl, new HttpStatusCode[1] { HttpStatusCode.ServiceUnavailable }), new HttpStatusCode[1] { HttpStatusCode.InternalServerError }), default(CancellationToken), (HttpCompletionOption)1).Result;
				try
				{
					if (response.get_StatusCode() == HttpStatusCode.InternalServerError)
					{
						message = "GW2 API is down. Please, try again later.";
						return true;
					}
					if (response.get_StatusCode() == HttpStatusCode.ServiceUnavailable)
					{
						string body = response.get_Content().ReadAsStringAsync().Result;
						message = body.GetTextBetweenTags("p");
						return true;
					}
				}
				finally
				{
					((IDisposable)response)?.Dispose();
				}
			}
			catch (Exception e)
			{
				ChatMacros.Logger.Warn(e, "Failed to check API status.");
			}
			message = string.Empty;
			return false;
		}

		public bool IsApiAvailable()
		{
			if (IsApiDown(out var message))
			{
				ScreenNotification.ShowNotification(message, (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			return true;
		}

		public bool IsAuthorizedApiAvailable()
		{
			if (!IsApiAvailable())
			{
				return false;
			}
			if (string.IsNullOrWhiteSpace(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()))
			{
				ScreenNotification.ShowNotification("API unavailable. Please, login to a character.", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			if (!HasSubtoken)
			{
				ScreenNotification.ShowNotification("Missing API key. Please, add an API key to BlishHUD.", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			if (MissingPermissions.Any())
			{
				string missing = string.Join(", ", MissingPermissions);
				ScreenNotification.ShowNotification("Insufficient API permissions.\nRequired: " + missing, (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			return true;
		}

		public async Task<List<string>> GetClears()
		{
			return ((IEnumerable<string>)(((object)(await TaskUtil.TryAsync(() => ((IBlobClient<IApiV2ObjectList<string>>)(object)ChatMacros.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Raids()).GetAsync(default(CancellationToken))))) ?? ((object)Enumerable.Empty<string>()))).ToList();
		}

		public async Task<List<AccountItem>> GetBank()
		{
			return ((IEnumerable<AccountItem>)(await TaskUtil.TryAsync(() => ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)ChatMacros.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Bank()).GetAsync(default(CancellationToken))))).ToList();
		}

		public async Task<List<AccountItem>> GetSharedBags()
		{
			return ((IEnumerable<AccountItem>)(await TaskUtil.TryAsync(() => ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)ChatMacros.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
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
					List<AccountItem> filtered = (from bag in character.get_Bags()
						where bag != null
						select bag).SelectMany((CharacterInventoryBag bag) => bag.get_Inventory()).ToList();
					if (filtered.Any())
					{
						bagsByCharacter.Add(character, filtered);
					}
				}
			}
			return bagsByCharacter;
		}

		public bool HasCorrectFormat(string apiKey)
		{
			if (!string.IsNullOrWhiteSpace(apiKey))
			{
				return _apiKeyPattern.IsMatch(apiKey);
			}
			return false;
		}

		public async Task<IReadOnlyList<Item>> GetItems(params int[] itemIds)
		{
			return (await TaskUtil.TryAsync(() => ((IBulkExpandableClient<Item, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
				.get_Items()).ManyAsync((IEnumerable<int>)itemIds, default(CancellationToken)))) ?? Enumerable.Empty<Item>().ToList();
		}

		public async Task<IReadOnlyList<Map>> GetMaps(params int[] mapIds)
		{
			return (await TaskUtil.TryAsync(() => ((IBulkExpandableClient<Map, int>)(object)ChatMacros.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).ManyAsync((IEnumerable<int>)mapIds, default(CancellationToken)))) ?? Enumerable.Empty<Map>().ToList();
		}

		public async Task<IReadOnlyList<Map>> GetMaps()
		{
			return ((IEnumerable<Map>)(await TaskUtil.TryAsync(() => ((IAllExpandableClient<Map>)(object)ChatMacros.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).AllAsync(default(CancellationToken)))))?.ToList() ?? Enumerable.Empty<Map>().ToList();
		}

		public async Task<IReadOnlyList<ContinentFloorRegionMap>> GetRegionMap(Map map)
		{
			List<ContinentFloorRegionMap> regionMaps = new List<ContinentFloorRegionMap>();
			foreach (int floor in map.get_Floors())
			{
				regionMaps.Add(await TaskUtil.TryAsync(() => ((IBlobClient<ContinentFloorRegionMap>)(object)ChatMacros.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Continents()
					.get_Item(map.get_ContinentId())
					.get_Floors()
					.get_Item(floor)
					.get_Regions()
					.get_Item(map.get_RegionId())
					.get_Maps()
					.get_Item(map.get_Id())).GetAsync(default(CancellationToken))));
			}
			return regionMaps;
		}

		public async Task<List<ContinentFloorRegionMapSector>> GetMapSectors(Map map)
		{
			List<ContinentFloorRegionMapSector> result = new List<ContinentFloorRegionMapSector>();
			foreach (int floor in map.get_Floors())
			{
				IApiV2ObjectList<ContinentFloorRegionMapSector> sectors = await TaskUtil.RetryAsync(() => ((IAllExpandableClient<ContinentFloorRegionMapSector>)(object)ChatMacros.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Continents()
					.get_Item(map.get_ContinentId())
					.get_Floors()
					.get_Item(floor)
					.get_Regions()
					.get_Item(map.get_RegionId())
					.get_Maps()
					.get_Item(map.get_Id())
					.get_Sectors()).AllAsync(default(CancellationToken)));
				if (sectors != null && ((IEnumerable<ContinentFloorRegionMapSector>)sectors).Any())
				{
					result.AddRange(((IEnumerable<ContinentFloorRegionMapSector>)sectors).DistinctBy((ContinentFloorRegionMapSector sector) => sector.get_Id()));
				}
			}
			return result;
		}

		private async Task<IEnumerable<Character>> GetCharacters()
		{
			return (IEnumerable<Character>)(((object)(await TaskUtil.TryAsync(() => ((IAllExpandableClient<Character>)(object)ChatMacros.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken))))) ?? ((object)Enumerable.Empty<Character>()));
		}

		private void OnSubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			HasSubtoken = true;
			MissingPermissions = _requires.Except(e.get_Value()).ToList();
		}
	}
}
