using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace CinemaModule.Services.WatchParty
{
	public sealed class Gw2IdentityService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<Gw2IdentityService>();

		private static readonly TimeSpan FetchCooldown = TimeSpan.FromSeconds(5.0);

		private static readonly TokenPermission[] RequiredPermissions = (TokenPermission[])(object)new TokenPermission[1] { (TokenPermission)1 };

		private readonly Gw2ApiManager _gw2ApiManager;

		private string _localGw2Name;

		private bool _isDisposed;

		private DateTime _lastFetchAttempt = DateTime.MinValue;

		public string AccountName => _localGw2Name;

		public bool IsAvailable => !string.IsNullOrEmpty(_localGw2Name);

		public event EventHandler ApiAvailabilityChanged;

		public Gw2IdentityService(Gw2ApiManager gw2ApiManager)
		{
			_gw2ApiManager = gw2ApiManager;
			_gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
			FetchAccountNameFireAndForget();
		}

		private void OnSubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			if (!_isDisposed)
			{
				FetchAccountNameFireAndForget();
			}
		}

		private async void FetchAccountNameFireAndForget()
		{
			try
			{
				await FetchAccountNameAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Fire-and-forget fetch failed");
			}
		}

		public async Task FetchAccountNameAsync(bool ignoreCooldown = false)
		{
			if (_isDisposed || (!ignoreCooldown && DateTime.UtcNow - _lastFetchAttempt < FetchCooldown))
			{
				return;
			}
			_lastFetchAttempt = DateTime.UtcNow;
			bool wasAvailable = IsAvailable;
			try
			{
				IGw2WebApiClient client = _gw2ApiManager.get_Gw2ApiClient();
				bool hasPermission = _gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)RequiredPermissions);
				if (client == null || !hasPermission)
				{
					ClearAccountName(wasAvailable);
					return;
				}
				Account account = await ((IBlobClient<Account>)(object)client.get_V2().get_Account()).GetAsync(default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
				if (!_isDisposed)
				{
					_localGw2Name = account.get_Name();
					Logger.Info("Resolved GW2 account: " + _localGw2Name);
					if (!wasAvailable)
					{
						RaiseApiAvailabilityChanged();
					}
				}
			}
			catch (AuthorizationRequiredException val)
			{
				AuthorizationRequiredException ex3 = val;
				HandleFetchError(wasAvailable, (Exception)(object)ex3, "API token unauthorized");
			}
			catch (RequestException val2)
			{
				RequestException ex2 = val2;
				HandleFetchError(wasAvailable, (Exception)(object)ex2, "GW2 API request failed");
			}
			catch (Exception ex)
			{
				HandleFetchError(wasAvailable, ex, "Unexpected error fetching account name");
			}
		}

		private void ClearAccountName(bool wasAvailable)
		{
			_localGw2Name = null;
			if (wasAvailable)
			{
				RaiseApiAvailabilityChanged();
			}
		}

		private void RaiseApiAvailabilityChanged()
		{
			try
			{
				this.ApiAvailabilityChanged?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Error in ApiAvailabilityChanged handler");
			}
		}

		private void HandleFetchError(bool wasAvailable, Exception ex, string message)
		{
			if (!_isDisposed)
			{
				Logger.Warn(ex, message);
				ClearAccountName(wasAvailable);
			}
		}

		public async Task<bool> EnsureAvailableAsync()
		{
			if (IsAvailable)
			{
				return true;
			}
			await FetchAccountNameAsync(ignoreCooldown: true).ConfigureAwait(continueOnCapturedContext: false);
			return IsAvailable;
		}

		public string GetDiagnosticInfo()
		{
			bool hasAccountPermission = _gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)RequiredPermissions);
			return "AccountName: " + (_localGw2Name ?? "(not set)") + ", " + $"HasClient: {_gw2ApiManager.get_Gw2ApiClient() != null}, " + $"HasAccountPerm: {hasAccountPermission}";
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
			}
		}
	}
}
