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
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.Services
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
			ProofLogix.Instance.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
		}

		public void Dispose()
		{
			ProofLogix.Instance.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
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
				ProofLogix.Logger.Warn(e, "Failed to check API status.");
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
				string missing = string.Join(", ", ProofLogix.Instance.Gw2WebApi.MissingPermissions);
				ScreenNotification.ShowNotification("Insufficient API permissions.\nRequired: " + missing, (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			return true;
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
			return (await TaskUtil.TryAsync(() => ((IBulkExpandableClient<Map, int>)(object)ProofLogix.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).ManyAsync((IEnumerable<int>)mapIds, default(CancellationToken)))) ?? Enumerable.Empty<Map>().ToList();
		}

		private async Task<IEnumerable<Character>> GetCharacters()
		{
			return (IEnumerable<Character>)(((object)(await TaskUtil.TryAsync(() => ((IAllExpandableClient<Character>)(object)ProofLogix.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken))))) ?? ((object)Enumerable.Empty<Character>()));
		}

		private void OnSubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			HasSubtoken = true;
			MissingPermissions = _requires.Except(e.get_Value()).ToList();
		}

		private IEnumerable<AccountItem> FilterProofs(IEnumerable<AccountItem> items)
		{
			List<Resource> resources = ProofLogix.Instance.Resources.GetItems();
			return items?.Where((AccountItem item) => item != null && resources.Select((Resource res) => res.Id).Contains(item.get_Id())) ?? Enumerable.Empty<AccountItem>();
		}
	}
}
