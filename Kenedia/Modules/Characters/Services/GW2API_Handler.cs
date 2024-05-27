using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Utility;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters.Services
{
	public class GW2API_Handler
	{
		private readonly Logger _logger = Logger.GetLogger(typeof(GW2API_Handler));

		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly Action<IApiV2ObjectList<Character>> _callBack;

		private readonly Data _data;

		private readonly Func<NotificationBadge> _notificationBadge;

		private readonly Func<LoadingSpinner> _getSpinner;

		private readonly PathCollection _paths;

		private readonly string _accountFilePath;

		private double _lastApiCheck = double.MinValue;

		private StatusType _apiStatus;

		private StatusType _mapStatus;

		private CancellationTokenSource _cancellationTokenSource;

		private Account _account;

		private Exception _lastException;

		public MainWindow MainWindow { get; set; }

		public Account Account
		{
			get
			{
				return _account;
			}
			set
			{
				Account temp = _account;
				if (Common.SetProperty(ref _account, value, this.AccountChanged, triggerOnUpdate: true, "Account"))
				{
					_paths.AccountName = ((value != null) ? value.get_Name() : null);
					BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Account changed from " + (((temp != null) ? temp.get_Name() : null) ?? "No Account") + " to " + (((value != null) ? value.get_Name() : null) ?? "No Account") + "!");
				}
			}
		}

		public event PropertyChangedEventHandler AccountChanged;

		public GW2API_Handler(Gw2ApiManager gw2ApiManager, Action<IApiV2ObjectList<Character>> callBack, Func<LoadingSpinner> getSpinner, PathCollection paths, Data data, Func<NotificationBadge> notificationBadge)
		{
			_gw2ApiManager = gw2ApiManager;
			_callBack = callBack;
			_getSpinner = getSpinner;
			_paths = paths;
			_accountFilePath = paths.ModulePath + "\\accounts.json";
			_data = data;
			_notificationBadge = notificationBadge;
		}

		private void UpdateAccountsList(Account account, IApiV2ObjectList<Character> characters)
		{
			try
			{
				List<AccountSummary> accounts = new List<AccountSummary>();
				AccountSummary accountEntry;
				if (File.Exists(_accountFilePath))
				{
					accounts = JsonConvert.DeserializeObject<List<AccountSummary>>(File.ReadAllText(_accountFilePath), SerializerSettings.Default);
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
				string json = JsonConvert.SerializeObject((object)accounts, SerializerSettings.Default);
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
					LoadingSpinner loadingSpinner = getSpinner();
					if (loadingSpinner != null)
					{
						((Control)loadingSpinner).Hide();
					}
				}
			}
			if (cancellationToken.IsCancellationRequested)
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Canceled API Data fetch!");
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
				LoadingSpinner loadingSpinner = getSpinner();
				if (loadingSpinner != null)
				{
					((Control)loadingSpinner).Show();
				}
			}
			NotificationBadge notificationBadge = _notificationBadge();
			bool result = default(bool);
			object obj;
			int num;
			try
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Fetching new API Data ...");
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
						result = false;
						return result;
					}
					Account = account;
					BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Fetching characters for '" + Account.get_Name() + "' ...");
					IApiV2ObjectList<Character> characters = await ((IAllExpandableClient<Character>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(cancellationToken);
					if (cancellationToken.IsCancellationRequested)
					{
						Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
						result = false;
						return result;
					}
					UpdateAccountsList(account, characters);
					_callBack?.Invoke(characters);
					Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
					_apiStatus = StatusType.Success;
					result = true;
					return result;
				}
				if (!cancellationToken.IsCancellationRequested)
				{
					BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Warn(strings.Error_InvalidPermissions);
					MainWindow?.SendAPIPermissionNotification();
					Task<Func<string>> text3 = HandleAPIExceptions(new Gw2ApiInvalidPermissionsException());
					_apiStatus = StatusType.Error;
					notificationBadge?.AddNotification(new ConditionalNotification(await text3, () => _apiStatus == StatusType.Success));
				}
				Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
				result = false;
				return result;
			}
			catch (UnexpectedStatusException val)
			{
				obj = (object)val;
				num = 1;
			}
			catch (Exception ex2)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					_logger.Warn(ex2, strings.Error_FailedAPIFetch);
				}
				_apiStatus = StatusType.Error;
				Task<Func<string>> text2 = HandleAPIExceptions(ex2);
				notificationBadge?.AddNotification(new ConditionalNotification(await text2, () => _apiStatus == StatusType.Success));
				Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
				return false;
			}
			if (num != 1)
			{
				return result;
			}
			UnexpectedStatusException ex = (UnexpectedStatusException)obj;
			Task<Func<string>> text = HandleAPIExceptions((Exception)(object)ex);
			MainWindow?.SendAPITimeoutNotification();
			BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Warn((Exception)(object)ex, strings.APITimeoutNotification);
			Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
			_apiStatus = StatusType.Error;
			notificationBadge?.AddNotification(new ConditionalNotification(await text, () => _apiStatus == StatusType.Success));
			return false;
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
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info($"No data for {locale.Value} loaded yet. Fetching new data from the API.");
				await GetMaps();
			}
		}

		public async Task GetMaps()
		{
			NotificationBadge notificationBadge = _notificationBadge();
			try
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
				string json = JsonConvert.SerializeObject((object)_maps, SerializerSettings.Default);
				File.WriteAllText(_paths.ModuleDataPath + "\\Maps.json", json);
				_mapStatus = StatusType.Success;
			}
			catch (Exception ex)
			{
				_logger.Warn("Failed to fetch armory items.");
				_logger.Warn($"{ex}");
				Task<Func<string>> text = HandleAPIExceptions(ex);
				_mapStatus = StatusType.Error;
				notificationBadge?.AddNotification(new ConditionalNotification(await text, () => _mapStatus == StatusType.Success));
			}
		}

		private static string? GetExceptionMessage(Exception ex)
		{
			string lineBreakPattern = "<\\/h[0-9]>";
			string lineBreakReplacement = Environment.NewLine;
			string result = Regex.Replace(ex?.Message ?? string.Empty, lineBreakPattern, lineBreakReplacement);
			string pattern = "<[^>]+>";
			string replacement = "";
			result = Regex.Replace(result, pattern, replacement);
			if (!string.IsNullOrEmpty(result))
			{
				return "\n\n" + result;
			}
			return null;
		}

		private async Task<Func<string>> HandleAPIExceptions(Exception ex)
		{
			if (ex is Gw2ApiInvalidPermissionsException)
			{
				ex = (await TestAPI()) ?? ex;
			}
			Func<string> result = ((ex is ServiceUnavailableException) ? ((Func<string>)(() => strings_common.GW2API_Unavailable + GetExceptionMessage(ex))) : ((ex is RequestException) ? ((Func<string>)(() => strings_common.GW2API_RequestFailed + GetExceptionMessage(ex))) : ((ex is RequestException<string>) ? ((Func<string>)(() => strings_common.GW2API_RequestFailed + GetExceptionMessage(ex))) : ((!(ex is Gw2ApiInvalidPermissionsException)) ? ((Func<string>)(() => GetExceptionMessage(ex) ?? "")) : ((Func<string>)(() => strings.Error_InvalidPermissions + "\nIf you have a valid API Key added there are probably issues with the API currently."))))));
			_lastException = ex;
			return result;
		}

		private async Task<Exception> TestAPI()
		{
			try
			{
				_lastApiCheck = Common.Now;
				await ((IBlobClient<Build>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Build()).GetAsync(default(CancellationToken));
				return null;
			}
			catch (Exception result)
			{
				return result;
			}
		}
	}
}
