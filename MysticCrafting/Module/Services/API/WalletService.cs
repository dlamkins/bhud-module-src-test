using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Models.Commerce;
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

		public IList<MysticCurrencyQuantity> CurrencyQuantities { get; set; } = new List<MysticCurrencyQuantity>();


		public WalletService(Gw2ApiManager apiManager, ICurrencyRepository currencyRepository, IPlayerItemService playerItemService)
		{
			_gw2ApiManager = apiManager;
			_currencyRepository = currencyRepository;
			_playerItemService = playerItemService;
		}

		public IEnumerable<MysticCurrencyQuantity> GetQuantities()
		{
			return CurrencyQuantities.Concat(GetItemCurrencies());
		}

		public MysticCurrencyQuantity GetQuantity(int id)
		{
			return CurrencyQuantities.FirstOrDefault((MysticCurrencyQuantity i) => i?.Currency != null && i.Currency.GameId == id) ?? GetItemCurrency(id);
		}

		public override async Task<string> LoadAsync()
		{
			if (_gw2ApiManager == null)
			{
				throw new Exception("Gw2ApiManager object is null.");
			}
			if (_gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Wallet
			}))
			{
				CurrencyQuantities = (await _gw2ApiManager.Gw2ApiClient.V2.Account.Wallet.GetAsync()).Select((AccountCurrency c) => MysticCurrencyQuantity.From(c, _currencyRepository.GetCurrency(c.Id))).ToList();
				return $"{CurrencyQuantities.Count} currencies loaded";
			}
			throw new Exception("One or more of the required permissions are missing: Tradingpost.");
		}

		private IEnumerable<MysticCurrencyQuantity> GetItemCurrencies()
		{
			if (!_playerItemService.Loaded)
			{
				return new List<MysticCurrencyQuantity>();
			}
			return (from c in _currencyRepository.GetCurrencies()
				where c.IsItem
				select c).Select(GetItemCurrency);
		}

		private MysticCurrencyQuantity GetItemCurrency(int itemCurrencyId)
		{
			MysticCurrency itemCurrency = _currencyRepository.GetCurrency(itemCurrencyId);
			return GetItemCurrency(itemCurrency);
		}

		private MysticCurrencyQuantity GetItemCurrency(MysticCurrency itemCurrency)
		{
			int quantity = _playerItemService.GetItemCount(itemCurrency.GameId);
			return new MysticCurrencyQuantity
			{
				Count = quantity,
				Currency = itemCurrency
			};
		}
	}
}
