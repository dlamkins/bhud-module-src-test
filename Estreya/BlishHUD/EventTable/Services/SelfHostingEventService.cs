using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models.SelfHosting;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Services
{
	public class SelfHostingEventService : APIService<SelfHostingEventEntry>
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly IFlurlClient _flurlClient;

		private readonly string _apiBaseUrl;

		private readonly AccountService _accountService;

		private readonly BlishHudApiService _blishHudApiService;

		private static TimeSpan _lastServerAddressCheckInterval = TimeSpan.FromSeconds(10.0);

		private AsyncRef<double> _lastServerAddressCheck = new AsyncRef<double>(_lastServerAddressCheckInterval.TotalMilliseconds);

		private string _lastServerAddress;

		private bool _lastServerAddressFirstCheck = true;

		public List<SelfHostingEventEntry> Events
		{
			get
			{
				using (_apiObjectListLock.Lock())
				{
					return new List<SelfHostingEventEntry>(base.APIObjectList);
				}
			}
		}

		public SelfHostingEventService(APIServiceConfiguration configuration, Gw2ApiManager apiManager, IFlurlClient flurlClient, string apiBaseUrl, AccountService accountService, BlishHudApiService blishHudApiService)
			: base(apiManager, configuration)
		{
			_flurlClient = flurlClient;
			_apiBaseUrl = apiBaseUrl;
			_accountService = accountService;
			_blishHudApiService = blishHudApiService;
		}

		protected override void DoUpdate(GameTime gameTime)
		{
			base.DoUpdate(gameTime);
			UpdateUtil.UpdateAsync(CheckServerAddress, gameTime, _lastServerAddressCheckInterval.TotalMilliseconds, _lastServerAddressCheck, doLogging: false);
		}

		private async Task CheckServerAddress()
		{
			try
			{
				string currentServerAddress = GameService.Gw2Mumble.get_Info().get_ServerAddress();
				if (!_lastServerAddressFirstCheck && _lastServerAddress != currentServerAddress && HasSelfHostingEntry())
				{
					Logger.Info("Instance IP changed from " + _lastServerAddress + " to " + currentServerAddress + ". Deleting self hosting entry.");
					await DeleteEntry();
				}
				_lastServerAddressFirstCheck = false;
				_lastServerAddress = currentServerAddress;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Could not check server address.");
			}
		}

		protected override async Task<List<SelfHostingEventEntry>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress, CancellationToken cancellationToken)
		{
			return await _flurlClient.Request(_apiBaseUrl, "/self-hosting").GetJsonAsync<List<SelfHostingEventEntry>>(default(CancellationToken), (HttpCompletionOption)0);
		}

		public async Task<double> GetMaxHostingDuration()
		{
			return (double)((dynamic)(await _flurlClient.Request(_apiBaseUrl, "/self-hosting/duration").GetJsonAsync(default(CancellationToken), (HttpCompletionOption)0))).duration;
		}

		public async Task<List<SelfHostingCategoryDefinition>> GetDefinitions()
		{
			return await _flurlClient.Request(_apiBaseUrl, "/self-hosting/definitions").GetJsonAsync<List<SelfHostingCategoryDefinition>>(default(CancellationToken), (HttpCompletionOption)0);
		}

		public async Task<List<SelfHostingCategoryDefinition>> GetCategories()
		{
			return await _flurlClient.Request(_apiBaseUrl, "/self-hosting/categories").GetJsonAsync<List<SelfHostingCategoryDefinition>>(default(CancellationToken), (HttpCompletionOption)0);
		}

		public async Task<SelfHostingCategoryDefinition> GetCategory(string categoryKey)
		{
			return await _flurlClient.Request(_apiBaseUrl, "/self-hosting/categories/" + categoryKey).GetJsonAsync<SelfHostingCategoryDefinition>(default(CancellationToken), (HttpCompletionOption)0);
		}

		public async Task<List<SelfHostingZoneDefinition>> GetCategoryZones(string categoryKey)
		{
			return await _flurlClient.Request(_apiBaseUrl, "/self-hosting/categories/" + categoryKey + "/zones").GetJsonAsync<List<SelfHostingZoneDefinition>>(default(CancellationToken), (HttpCompletionOption)0);
		}

		public async Task<List<SelfHostingEventDefinition>> GetCategoryZoneEvents(string categoryKey, string zoneKey)
		{
			return await _flurlClient.Request(_apiBaseUrl, "/self-hosting/categories/" + categoryKey + "/zones/" + zoneKey + "/events").GetJsonAsync<List<SelfHostingEventDefinition>>(default(CancellationToken), (HttpCompletionOption)0);
		}

		public bool HasSelfHostingEntry()
		{
			Account account = _accountService.Account;
			string accountName = ((account != null) ? account.get_Name() : null);
			if (!string.IsNullOrWhiteSpace(accountName))
			{
				return Events.Any((SelfHostingEventEntry e) => e.AccountName == accountName);
			}
			return false;
		}

		public async Task DeleteEntry()
		{
			await DeleteEntry(reload: true);
		}

		private async Task DeleteEntry(bool reload)
		{
			if (string.IsNullOrWhiteSpace(_blishHudApiService.AccessToken))
			{
				throw new InvalidOperationException("Not logged in to Estreya BlishHUD.");
			}
			await _flurlClient.Request(_apiBaseUrl, "/self-hosting").WithOAuthBearerToken(_blishHudApiService.AccessToken).DeleteAsync(default(CancellationToken), (HttpCompletionOption)0);
			if (reload)
			{
				await Reload();
			}
		}

		public async Task AddEntry(string categoryKey, string zoneKey, string eventKey, DateTimeOffset startTime, int duration)
		{
			if (string.IsNullOrWhiteSpace(categoryKey))
			{
				throw new ArgumentNullException("categoryKey");
			}
			if (string.IsNullOrWhiteSpace(zoneKey))
			{
				throw new ArgumentNullException("zoneKey");
			}
			if (string.IsNullOrWhiteSpace(eventKey))
			{
				throw new ArgumentNullException("eventKey");
			}
			int accountServiceTimeoutSec = 5;
			if (!(await _accountService.WaitForCompletion(TimeSpan.FromSeconds(accountServiceTimeoutSec))))
			{
				throw new InvalidOperationException($"Account Service did not respond within {accountServiceTimeoutSec} seconds.");
			}
			Account account = _accountService.Account;
			string accountName = ((account != null) ? account.get_Name() : null);
			if (string.IsNullOrWhiteSpace(accountName))
			{
				throw new InvalidOperationException("Account Service did not return a valid account.");
			}
			if (string.IsNullOrWhiteSpace(_blishHudApiService.AccessToken))
			{
				throw new InvalidOperationException("Not logged in to Estreya BlishHUD.");
			}
			string instanceIP = GameService.Gw2Mumble.get_Info().get_ServerAddress();
			if (string.IsNullOrWhiteSpace(instanceIP))
			{
				throw new InvalidOperationException("Could not find valid server ip address.");
			}
			await _flurlClient.Request(_apiBaseUrl, "/self-hosting/categories/" + categoryKey + "/zones/" + zoneKey + "/events/" + eventKey).WithOAuthBearerToken(_blishHudApiService.AccessToken).PostJsonAsync(new SelfHostingEventEntry
			{
				AccountName = accountName,
				InstanceIP = instanceIP,
				Duration = duration,
				StartTime = startTime.ToUniversalTime()
			}, default(CancellationToken), (HttpCompletionOption)0);
			await Reload();
		}

		public async Task ReportHost(string accountName, SelfHostingReportType type, string reason)
		{
			if (string.IsNullOrWhiteSpace(_blishHudApiService.AccessToken))
			{
				throw new InvalidOperationException("Not logged in to Estreya BlishHUD.");
			}
			await _flurlClient.Request(_apiBaseUrl, "/self-hosting/report").WithOAuthBearerToken(_blishHudApiService.AccessToken).PostJsonAsync(new
			{
				accountName = accountName,
				type = type.ToString(),
				reason = reason
			}, default(CancellationToken), (HttpCompletionOption)0);
		}

		protected override void DoUnload()
		{
			base.DoUnload();
			try
			{
				AsyncHelper.RunSync(() => DeleteEntry(reload: false));
			}
			catch (Exception)
			{
			}
		}
	}
}
