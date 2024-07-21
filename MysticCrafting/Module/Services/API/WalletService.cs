using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Services.Recurring;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Services.API
{
	public class WalletService : RecurringService, IWalletService, IRecurringService
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly ICurrencyRepository _currencyRepository;

		private readonly IPlayerItemService _playerItemService;

		public override string Name => Common.LoadingWallet;

		public IList<CurrencyQuantity> CurrencyQuantities { get; set; } = new List<CurrencyQuantity>();


		public WalletService(Gw2ApiManager apiManager, ICurrencyRepository currencyRepository, IPlayerItemService playerItemService)
		{
			_gw2ApiManager = apiManager;
			_currencyRepository = currencyRepository;
			_playerItemService = playerItemService;
		}

		public IEnumerable<CurrencyQuantity> GetQuantities()
		{
			return CurrencyQuantities.Concat(GetItemCurrencies());
		}

		public CurrencyQuantity GetQuantity(int id)
		{
			return CurrencyQuantities.FirstOrDefault((CurrencyQuantity i) => i?.Currency != null && i.Currency.Id == id) ?? GetItemCurrency(id);
		}

		public override async Task<string> LoadAsync()
		{
			if (_gw2ApiManager == null)
			{
				throw new Exception("Gw2ApiManager object is null.");
			}
			if (_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)10
			}))
			{
				CurrencyQuantities = ((IEnumerable<AccountCurrency>)(await ((IBlobClient<IApiV2ObjectList<AccountCurrency>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Wallet()).GetAsync(default(CancellationToken)))).Select((AccountCurrency c) => CurrencyQuantity.From(c, _currencyRepository.GetCurrency(c.get_Id()))).ToList();
				return $"{CurrencyQuantities.Count} currencies loaded";
			}
			throw new Exception("One or more of the required permissions are missing: Tradingpost.");
		}

		private IEnumerable<CurrencyQuantity> GetItemCurrencies()
		{
			if (!_playerItemService.Loaded)
			{
				return new List<CurrencyQuantity>();
			}
			return _currencyRepository.GetCurrencies().Select(GetItemCurrency);
		}

		private CurrencyQuantity GetItemCurrency(int itemCurrencyId)
		{
			Currency itemCurrency = _currencyRepository.GetCurrency(itemCurrencyId);
			return GetItemCurrency(itemCurrency);
		}

		private CurrencyQuantity GetItemCurrency(Currency itemCurrency)
		{
			int quantity = _playerItemService.GetItemCount(itemCurrency.Id);
			return new CurrencyQuantity
			{
				Count = quantity,
				Currency = itemCurrency
			};
		}
	}
}
