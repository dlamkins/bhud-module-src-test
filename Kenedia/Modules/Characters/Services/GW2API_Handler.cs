using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters.Services
{
	public class GW2API_Handler
	{
		private readonly Logger _logger = Logger.GetLogger(typeof(GW2API_Handler));

		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly Action<IApiV2ObjectList<Character>> _callBack;

		private readonly Data _data;

		private readonly Func<LoadingSpinner> _getSpinner;

		private readonly PathCollection _paths;

		private readonly string _accountFilePath;

		private CancellationTokenSource _cancellationTokenSource;

		private Account _account;

		public Account Account
		{
			get
			{
				return _account;
			}
			set
			{
				_paths.AccountName = value.get_Name();
				_account = value;
			}
		}

		public GW2API_Handler(Gw2ApiManager gw2ApiManager, Action<IApiV2ObjectList<Character>> callBack, Func<LoadingSpinner> getSpinner, PathCollection paths, Data data)
		{
			_gw2ApiManager = gw2ApiManager;
			_callBack = callBack;
			_getSpinner = getSpinner;
			_paths = paths;
			_accountFilePath = paths.ModulePath + "\\accounts.json";
			_data = data;
		}

		private void UpdateAccountsList(Account account, IApiV2ObjectList<Character> characters)
		{
			try
			{
				List<AccountSummary> accounts = new List<AccountSummary>();
				AccountSummary accountEntry;
				if (File.Exists(_accountFilePath))
				{
					accounts = JsonConvert.DeserializeObject<List<AccountSummary>>(File.ReadAllText(_accountFilePath));
					accountEntry = accounts.Find((AccountSummary e) => e.AccountName == account.get_Name());
					if (accountEntry != null)
					{
						accountEntry.AccountName = account.get_Name();
						accountEntry.CharacterNames = new List<string>();
						((IEnumerable<Character>)characters).ToList().ForEach(delegate(Character c)
						{
							accountEntry.CharacterNames.Add(c.get_Name());
						});
					}
					else
					{
						List<AccountSummary> list = accounts;
						AccountSummary obj = new AccountSummary
						{
							AccountName = account.get_Name(),
							CharacterNames = new List<string>()
						};
						AccountSummary item = obj;
						accountEntry = obj;
						list.Add(item);
						((IEnumerable<Character>)characters).ToList().ForEach(delegate(Character c)
						{
							accountEntry.CharacterNames.Add(c.get_Name());
						});
					}
				}
				else
				{
					List<AccountSummary> list2 = accounts;
					AccountSummary obj2 = new AccountSummary
					{
						AccountName = account.get_Name(),
						CharacterNames = new List<string>()
					};
					AccountSummary item = obj2;
					accountEntry = obj2;
					list2.Add(item);
					((IEnumerable<Character>)characters).ToList().ForEach(delegate(Character c)
					{
						accountEntry.CharacterNames.Add(c.get_Name());
					});
				}
				string json = JsonConvert.SerializeObject((object)accounts, (Formatting)1);
				File.WriteAllText(_accountFilePath, json);
			}
			catch
			{
			}
		}

		private void Reset(CancellationToken cancellationToken, bool hideSpinner = false)
		{
			if (hideSpinner)
			{
				Func<LoadingSpinner> getSpinner = _getSpinner;
				if (getSpinner != null)
				{
					LoadingSpinner obj = getSpinner();
					if (obj != null)
					{
						((Control)obj).Hide();
					}
				}
			}
			if (cancellationToken.IsCancellationRequested)
			{
				BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info("Canceled API Data fetch!");
			}
			_cancellationTokenSource = null;
		}

		public async Task<bool> CheckAPI()
		{
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			CancellationToken cancellationToken = _cancellationTokenSource.Token;
			Func<LoadingSpinner> getSpinner = _getSpinner;
			if (getSpinner != null)
			{
				LoadingSpinner obj = getSpinner();
				if (obj != null)
				{
					((Control)obj).Show();
				}
			}
			try
			{
				BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info("Fetching new API Data ...");
				if (_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
				{
					(TokenPermission)1,
					(TokenPermission)3
				}))
				{
					Account account = await ((IBlobClient<Account>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(cancellationToken);
					if (cancellationToken.IsCancellationRequested)
					{
						Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
						return false;
					}
					Account = account;
					IApiV2ObjectList<Character> characters = await ((IAllExpandableClient<Character>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(cancellationToken);
					if (cancellationToken.IsCancellationRequested)
					{
						Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
						return false;
					}
					UpdateAccountsList(account, characters);
					_callBack?.Invoke(characters);
					Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
					return true;
				}
				if (!cancellationToken.IsCancellationRequested)
				{
					ScreenNotification.ShowNotification("[Characters]: " + strings.Error_InvalidPermissions, (NotificationType)2, (Texture2D)null, 4);
					BaseModule<Characters, MainWindow, SettingsModel>.Logger.Error(strings.Error_InvalidPermissions);
				}
				Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
				return false;
			}
			catch (Exception ex)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					BaseModule<Characters, MainWindow, SettingsModel>.Logger.Warn(ex, strings.Error_FailedAPIFetch);
				}
				Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
				return false;
			}
		}

		public async Task FetchLocale(Locale? locale = null, bool force = false)
		{
			locale.GetValueOrDefault();
			if (!locale.HasValue)
			{
				Locale value = GameService.Overlay.get_UserLocale().get_Value();
				locale = value;
			}
			if (force || _data.Maps.Count == 0 || !_data.Maps.FirstOrDefault().Value.Names.TryGetValue(locale.Value, out var name) || string.IsNullOrEmpty(name))
			{
				BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info($"No data for {locale.Value} loaded yet. Fetching new data from the API.");
				await GetMaps();
			}
		}

		public async Task GetMaps()
		{
			Dictionary<int, Map> _maps = _data.Maps;
			foreach (Map i in (IEnumerable<Map>)(await ((IAllExpandableClient<Map>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).AllAsync(default(CancellationToken))))
			{
				Map map;
				bool num = _maps.TryGetValue(i.get_Id(), out map);
				if (map == null)
				{
					map = new Map(i);
				}
				map.Name = i.get_Name();
				if (!num)
				{
					_maps.Add(i.get_Id(), map);
				}
			}
			string json = JsonConvert.SerializeObject((object)_maps, (Formatting)1);
			File.WriteAllText(_paths.ModuleDataPath + "\\Maps.json", json);
		}
	}
}
