using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlishHudCurrencyViewer.Models;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace BlishHudCurrencyViewer.Services
{
	internal class CurrencyService
	{
		private Gw2ApiManager _apiManager;

		private SettingsManager _settingsManager;

		private Logger _logger;

		private List<Currency> _allInGameCurrencies;

		private List<SettingEntry<bool>> _availableCurrencySettings;

		private List<UserCurrency> _userAccountCurrencies;

		private List<SettingEntry> _selectedCurrencySettings;

		public event EventHandler AllGameCurrencyFetched;

		public CurrencyService(Gw2ApiManager apiManager, SettingsManager settingsManager, Logger logger)
		{
			_apiManager = apiManager;
			_settingsManager = settingsManager;
			_logger = logger;
			_selectedCurrencySettings = ((IEnumerable<SettingEntry>)_settingsManager.get_ModuleSettings()).Where((SettingEntry s) => s.get_EntryKey().StartsWith("currency-setting-") && ((s as SettingEntry<bool>)?.get_Value() ?? false)).ToList();
		}

		public async Task InitializeCurrencySettings()
		{
			try
			{
				_allInGameCurrencies = ((IEnumerable<Currency>)(await ((IAllExpandableClient<Currency>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Currencies()).AllAsync(default(CancellationToken)))).OrderBy((Currency c) => c.get_Name()).ToList();
				_availableCurrencySettings = new List<SettingEntry<bool>>();
				_allInGameCurrencies.ForEach(delegate(Currency c)
				{
					SettingEntry<bool> item = _settingsManager.get_ModuleSettings().DefineSetting<bool>("currency-setting-" + c.get_Id(), false, (Func<string>)(() => c.get_Name()), (Func<string>)null);
					_availableCurrencySettings.Add(item);
				});
			}
			catch (Exception e)
			{
				_logger.Warn(e.Message);
			}
		}

		public async Task<List<UserCurrency>> GetUserCurrencies()
		{
			if (!_apiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)10
			}))
			{
				_logger.Debug("User has incorrect permissions.");
				_userAccountCurrencies = new List<UserCurrency>();
				return _userAccountCurrencies;
			}
			try
			{
				List<AccountCurrency> list = ((IEnumerable<AccountCurrency>)(await ((IBlobClient<IApiV2ObjectList<AccountCurrency>>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Wallet()).GetAsync(default(CancellationToken)))).ToList();
				_userAccountCurrencies = new List<UserCurrency>();
				list.ForEach(delegate(AccountCurrency uc)
				{
					Currency val = _allInGameCurrencies.FirstOrDefault((Currency c) => c.get_Id() == uc.get_Id());
					UserCurrency item = new UserCurrency
					{
						CurrencyId = uc.get_Id(),
						CurrencyName = ((val != null) ? val.get_Name() : null),
						CurrencyQuantity = uc.get_Value()
					};
					_userAccountCurrencies.Add(item);
				});
				_logger.Debug($"Loaded {_userAccountCurrencies.Count()} currencies.");
			}
			catch (Exception e)
			{
				_logger.Warn(e.Message);
				_userAccountCurrencies = new List<UserCurrency>();
			}
			return _userAccountCurrencies;
		}

		public bool Update(GameTime gameTime)
		{
			List<SettingEntry> currentSelectedSettings = ((IEnumerable<SettingEntry>)_settingsManager.get_ModuleSettings()).Where((SettingEntry s) => s.get_EntryKey().StartsWith("currency-setting-") && ((s as SettingEntry<bool>)?.get_Value() ?? false)).ToList();
			if (currentSelectedSettings != null && currentSelectedSettings.Count != _selectedCurrencySettings.Count)
			{
				_selectedCurrencySettings = currentSelectedSettings;
				return true;
			}
			return false;
		}
	}
}
