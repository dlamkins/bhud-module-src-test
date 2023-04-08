using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using KpRefresher.Domain;
using KpRefresher.Extensions;

namespace KpRefresher.Services
{
	public class Gw2ApiService
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly Logger _logger;

		private List<Token> _tokens { get; set; }

		public Gw2ApiService(Gw2ApiManager gw2ApiManager, Logger logger)
		{
			_gw2ApiManager = gw2ApiManager;
			_logger = logger;
			_tokens = Enum.GetValues(typeof(Token)).Cast<Token>().ToList();
		}

		public async Task<string> GetAccountName()
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)_gw2ApiManager.get_Permissions()))
			{
				_logger.Warn("Permissions not granted.");
				return string.Empty;
			}
			try
			{
				Account obj = await ((IBlobClient<Account>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken));
				return (obj != null) ? obj.get_Name() : null;
			}
			catch (Exception ex)
			{
				_logger.Warn("Error while getting account name : " + ex.Message);
				return null;
			}
		}

		public async Task<List<string>> GetClears()
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)_gw2ApiManager.get_Permissions()))
			{
				_logger.Warn("Permissions not granted.");
				return null;
			}
			try
			{
				return ((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Raids()).GetAsync(default(CancellationToken))))?.ToList();
			}
			catch (Exception ex)
			{
				_logger.Warn("Error while getting raid clears : " + ex.Message);
				return null;
			}
		}

		public async Task<string> ScanAccountForKp()
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)_gw2ApiManager.get_Permissions()))
			{
				_logger.Warn("Permissions not granted.");
				return string.Empty;
			}
			string res = string.Empty;
			List<int> tokensId = _tokens.Select((Token c) => (int)c).ToList();
			try
			{
				IApiV2ObjectList<AccountItem> bankItems = await ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Bank()).GetAsync(default(CancellationToken));
				if (bankItems != null)
				{
					bool bankHasData = false;
					string bankData = string.Empty;
					foreach (AccountItem item3 in (IEnumerable<AccountItem>)bankItems)
					{
						if (item3 != null && tokensId.Contains(item3.get_Id()))
						{
							bankHasData = true;
							bankData = $"{bankData}{item3.get_Count()}   {((Token)item3.get_Id()).GetDisplayName()}\n";
						}
					}
					if (bankHasData)
					{
						res = res + "[Bank]\n" + bankData + "\n";
					}
				}
				else
				{
					_logger.Warn("Failed to retrieve bank items.");
				}
			}
			catch (Exception ex3)
			{
				_logger.Warn($"Failed to retrieve bank items : {ex3}");
			}
			try
			{
				IApiV2ObjectList<AccountItem> sharedInventoryItems = await ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Inventory()).GetAsync(default(CancellationToken));
				if (sharedInventoryItems != null)
				{
					bool sharedInventoryHasData = false;
					string sharedInventoryData = string.Empty;
					foreach (AccountItem item2 in (IEnumerable<AccountItem>)sharedInventoryItems)
					{
						if (item2 != null && tokensId.Contains(item2.get_Id()))
						{
							sharedInventoryHasData = true;
							sharedInventoryData = $"{sharedInventoryData}{item2.get_Count()}   {((Token)item2.get_Id()).GetDisplayName()}\n";
						}
					}
					if (sharedInventoryHasData)
					{
						res = res + "[Shared Slots]\n" + sharedInventoryData + "\n";
					}
				}
				else
				{
					_logger.Warn("Failed to retrieve shared inventory items.");
				}
			}
			catch (Exception ex2)
			{
				_logger.Warn($"Failed to retrieve shared inventory items : {ex2}");
			}
			try
			{
				IApiV2ObjectList<Character> characters = await ((IAllExpandableClient<Character>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken));
				if (characters != null)
				{
					bool characterHasData = false;
					string characterData = string.Empty;
					foreach (Character character in (IEnumerable<Character>)characters)
					{
						if (character.get_Bags() != null)
						{
							foreach (CharacterInventoryBag bag in character.get_Bags())
							{
								if (bag == null)
								{
									continue;
								}
								foreach (AccountItem item in bag.get_Inventory())
								{
									if (item != null && tokensId.Contains(item.get_Id()))
									{
										characterHasData = true;
										characterData = $"{characterData}{item.get_Count()}   {((Token)item.get_Id()).GetDisplayName()}\n";
									}
								}
							}
						}
						else
						{
							_logger.Warn("Failed to retrieve character bags");
						}
						if (characterHasData)
						{
							res = res + "[" + character.get_Name() + "]\n" + characterData + "\n";
							characterHasData = false;
							characterData = string.Empty;
						}
					}
				}
				else
				{
					_logger.Warn("Failed to retrieve characters.");
				}
			}
			catch (Exception ex)
			{
				_logger.Warn($"Failed to retrieve characters : {ex}");
			}
			return res;
		}
	}
}
