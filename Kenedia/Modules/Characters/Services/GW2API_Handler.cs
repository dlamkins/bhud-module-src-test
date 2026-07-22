using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Utility;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters.Services
{
	public class GW2API_Handler
	{
		private const int InventoryFetchConcurrency = 8;

		private readonly Logger _logger = Logger.GetLogger(typeof(GW2API_Handler));

		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly Action<IApiV2ObjectList<Character>> _callBack;

		private readonly Action<string, int?> _inventorySlotsCallBack;

		private readonly Data _data;

		private readonly Func<NotificationBadge> _notificationBadge;

		private readonly Func<LoadingSpinner> _getSpinner;

		private readonly PathCollection _paths;

		private readonly string _accountFilePath;

		private double _lastApiCheck = double.MinValue;

		private StatusType _apiStatus;

		private StatusType _mapStatus;

		private CancellationTokenSource _cancellationTokenSource;

		private Exception _lastException;

		public MainWindow MainWindow { get; set; }

		public Account Account
		{
			[CompilerGenerated]
			get
			{
				return _003CAccount_003Ek__BackingField;
			}
			set
			{
				Account temp = _003CAccount_003Ek__BackingField;
				if (Common.SetProperty(_003CAccount_003Ek__BackingField, value, delegate(Account v)
				{
					_003CAccount_003Ek__BackingField = v;
				}, this.AccountChanged, triggerOnUpdate: true, "Account"))
				{
					BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info("Account changed from " + (temp?.Name ?? "No Account") + " to " + (value?.Name ?? "No Account") + "!");
				}
			}
		}

		public event PropertyChangedEventHandler AccountChanged;

		public GW2API_Handler(Gw2ApiManager gw2ApiManager, Action<IApiV2ObjectList<Character>> callBack, Action<string, int?> inventorySlotsCallBack, Func<LoadingSpinner> getSpinner, PathCollection paths, Data data, Func<NotificationBadge> notificationBadge)
		{
			_gw2ApiManager = gw2ApiManager;
			_callBack = callBack;
			_inventorySlotsCallBack = inventorySlotsCallBack;
			_getSpinner = getSpinner;
			_paths = paths;
			_accountFilePath = paths.ModulePath + "\\accounts.json";
			_data = data;
			_notificationBadge = notificationBadge;
		}

		private void UpdateAccountsList(Account account, IApiV2ObjectList<Character> characters)
		{
			Account account2 = account;
			try
			{
				List<AccountSummary> accounts = new List<AccountSummary>();
				AccountSummary accountEntry;
				if (System.IO.File.Exists(_accountFilePath))
				{
					accounts = JsonConvert.DeserializeObject<List<AccountSummary>>(System.IO.File.ReadAllText(_accountFilePath), SerializerSettings.Default);
					accountEntry = accounts.Find((AccountSummary e) => e.AccountName == account2.Name);
					if (accountEntry != null)
					{
						accountEntry.AccountName = account2.Name;
						accountEntry.CharacterNames = new List<string>();
						characters.ToList().ForEach(delegate(Character c)
						{
							accountEntry.CharacterNames.Add(c.Name);
						});
					}
					else
					{
						List<AccountSummary> list = accounts;
						AccountSummary obj = new AccountSummary
						{
							AccountName = account2.Name,
							CharacterNames = new List<string>()
						};
						AccountSummary item = obj;
						accountEntry = obj;
						list.Add(item);
						characters.ToList().ForEach(delegate(Character c)
						{
							accountEntry.CharacterNames.Add(c.Name);
						});
					}
				}
				else
				{
					List<AccountSummary> list2 = accounts;
					AccountSummary obj2 = new AccountSummary
					{
						AccountName = account2.Name,
						CharacterNames = new List<string>()
					};
					AccountSummary item = obj2;
					accountEntry = obj2;
					list2.Add(item);
					characters.ToList().ForEach(delegate(Character c)
					{
						accountEntry.CharacterNames.Add(c.Name);
					});
				}
				string json = JsonConvert.SerializeObject(accounts, SerializerSettings.Default);
				System.IO.File.WriteAllText(_accountFilePath, json);
			}
			catch
			{
			}
		}

		private void Reset(CancellationToken cancellationToken, bool hideSpinner = false)
		{
			if (hideSpinner)
			{
				_getSpinner?.Invoke()?.Hide();
			}
			if (cancellationToken.IsCancellationRequested)
			{
				BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info("Canceled API Data fetch!");
			}
			_cancellationTokenSource = null;
		}

		public async Task<bool> CheckAPI()
		{
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			CancellationToken cancellationToken = _cancellationTokenSource.Token;
			_getSpinner?.Invoke()?.Show();
			NotificationBadge notificationBadge = _notificationBadge();
			try
			{
				BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info("Fetching new API Data ...");
				if (_gw2ApiManager.HasPermissions(new TokenPermission[2]
				{
					TokenPermission.Account,
					TokenPermission.Characters
				}))
				{
					Account account = await _gw2ApiManager.Gw2ApiClient.V2.Account.GetAsync(cancellationToken);
					if (cancellationToken.IsCancellationRequested)
					{
						Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
						return false;
					}
					Account = account;
					BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info("Fetching characters for '" + Account.Name + "' ...");
					IApiV2ObjectList<Character> characters = await _gw2ApiManager.Gw2ApiClient.V2.Characters.AllAsync(cancellationToken);
					if (cancellationToken.IsCancellationRequested)
					{
						Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
						return false;
					}
					UpdateAccountsList(account, characters);
					_callBack?.Invoke(characters);
					_getSpinner?.Invoke()?.Hide();
					_apiStatus = StatusType.Success;
					FetchFreeInventorySlots(characters.ToList(), cancellationToken, _cancellationTokenSource);
					return true;
				}
				if (!cancellationToken.IsCancellationRequested)
				{
					BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Warn(strings.Error_InvalidPermissions);
					MainWindow?.SendAPIPermissionNotification();
					Task<Func<string>> text3 = HandleAPIExceptions(new Gw2ApiInvalidPermissionsException());
					_apiStatus = StatusType.Error;
					notificationBadge?.AddNotification(new ConditionalNotification(await text3, () => _apiStatus == StatusType.Success));
				}
				Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
				return false;
			}
			catch (UnexpectedStatusException ex2)
			{
				Task<Func<string>> text2 = HandleAPIExceptions(ex2);
				MainWindow?.SendAPITimeoutNotification();
				BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Warn(ex2, strings.APITimeoutNotification);
				Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
				_apiStatus = StatusType.Error;
				notificationBadge?.AddNotification(new ConditionalNotification(await text2, () => _apiStatus == StatusType.Success));
				return false;
			}
			catch (Exception ex)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					_logger.Warn(ex, strings.Error_FailedAPIFetch);
				}
				_apiStatus = StatusType.Error;
				Task<Func<string>> text = HandleAPIExceptions(ex);
				notificationBadge?.AddNotification(new ConditionalNotification(await text, () => _apiStatus == StatusType.Success));
				Reset(cancellationToken, !cancellationToken.IsCancellationRequested);
				return false;
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

		private async Task FetchFreeInventorySlots(IReadOnlyList<Character> characters, CancellationToken cancellationToken, CancellationTokenSource cancellationTokenSource)
		{
			if (!_gw2ApiManager.HasPermissions(new TokenPermission[1] { TokenPermission.Inventories }))
			{
				BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info("No permission to fetch inventory data. Skipping free inventory slots fetch.");
				ClearCompletedCancellationSource(cancellationTokenSource);
				return;
			}
			BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info($"Fetching free inventory slots for {characters.Count} characters with {8} parallel requests.");
			SemaphoreSlim throttler = new SemaphoreSlim(8);
			try
			{
				List<Task> tasks = characters.Select((Character character) => FetchFreeInventorySlots(character, throttler, cancellationToken)).ToList();
				try
				{
					await Task.WhenAll(tasks);
				}
				catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
				{
					BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info("Canceled inventory data fetch.");
				}
				catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
				{
					_logger.Warn(exception, "Failed while fetching free inventory slots.");
				}
				finally
				{
					ClearCompletedCancellationSource(cancellationTokenSource);
				}
			}
			finally
			{
				if (throttler != null)
				{
					((IDisposable)throttler).Dispose();
				}
			}
		}

		private async Task FetchFreeInventorySlots(Character character, SemaphoreSlim throttler, CancellationToken cancellationToken)
		{
			await throttler.WaitAsync(cancellationToken);
			try
			{
				BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info("Fetching inventory for '" + character.Name + "' ...");
				CharactersInventory inventory = await _gw2ApiManager.Gw2ApiClient.V2.Characters[character.Name].Inventory.GetAsync(cancellationToken);
				_inventorySlotsCallBack?.Invoke(character.Name, CountFreeInventorySlots(inventory));
			}
			catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
			{
				_logger.Warn(exception, "Failed to fetch inventory for '" + character.Name + "'.");
			}
			finally
			{
				throttler.Release();
			}
		}

		private void ClearCompletedCancellationSource(CancellationTokenSource cancellationTokenSource)
		{
			if (_cancellationTokenSource == cancellationTokenSource)
			{
				_cancellationTokenSource = null;
			}
		}

		private static int? CountFreeInventorySlots(CharactersInventory inventory)
		{
			if (inventory?.Bags == null)
			{
				return null;
			}
			int freeSlots = 0;
			foreach (CharacterInventoryBag bag in inventory.Bags.Where((CharacterInventoryBag b) => b != null))
			{
				int usedSlots = bag.Inventory?.Count((AccountItem item) => item != null) ?? 0;
				freeSlots += Math.Max(0, bag.Size - usedSlots);
			}
			return freeSlots;
		}

		private async Task<Func<string>> HandleAPIExceptions(Exception ex)
		{
			Exception ex2 = ex;
			if (ex2 is Gw2ApiInvalidPermissionsException)
			{
				ex2 = (await TestAPI()) ?? ex2;
			}
			Func<string> result = ((ex2 is ServiceUnavailableException) ? ((Func<string>)(() => strings_common.GW2API_Unavailable + GetExceptionMessage(ex2))) : ((ex2 is RequestException) ? ((Func<string>)(() => strings_common.GW2API_RequestFailed + GetExceptionMessage(ex2))) : ((ex2 is RequestException<string>) ? ((Func<string>)(() => strings_common.GW2API_RequestFailed + GetExceptionMessage(ex2))) : ((!(ex2 is Gw2ApiInvalidPermissionsException)) ? ((Func<string>)(() => GetExceptionMessage(ex2) ?? "")) : ((Func<string>)(() => strings.Error_InvalidPermissions + "\nIf you have a valid API Key added there are probably issues with the API currently."))))));
			_lastException = ex2;
			return result;
		}

		private async Task<Exception> TestAPI()
		{
			try
			{
				_lastApiCheck = Common.Now;
				await _gw2ApiManager.Gw2ApiClient.V2.Build.GetAsync();
				return null;
			}
			catch (Exception result)
			{
				return result;
			}
		}
	}
}
