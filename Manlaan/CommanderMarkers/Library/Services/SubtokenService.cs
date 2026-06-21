using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Manlaan.CommanderMarkers.Library.Services
{
	public class SubtokenService
	{
		private string? _cachedSubtoken;

		private DateTime _subtokenExpiry = DateTime.MinValue;

		public event EventHandler? SubtokenRefreshed;

		public async Task<string?> GetValidSubtokenAsync()
		{
			if (!string.IsNullOrEmpty(_cachedSubtoken) && DateTime.UtcNow < _subtokenExpiry)
			{
				return _cachedSubtoken;
			}
			return await GenerateSubtokenAsync();
		}

		public async Task<string?> GenerateSubtokenAsync()
		{
			try
			{
				if (Service.Gw2ApiManager == null || !Service.Gw2ApiManager.HasPermission((TokenPermission)1))
				{
					return null;
				}
				_cachedSubtoken = (await ((IBlobClient<CreateSubtoken>)(object)Service.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_CreateSubtoken()
					.WithPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[1] { (TokenPermission)1 })
					.Expires((DateTimeOffset)DateTime.UtcNow.AddDays(1.0))).GetAsync(default(CancellationToken))).get_Subtoken();
				_subtokenExpiry = DateTime.UtcNow.AddDays(1.0);
				this.SubtokenRefreshed?.Invoke(this, EventArgs.Empty);
				return _cachedSubtoken;
			}
			catch (Exception ex)
			{
				Logger.GetLogger<SubtokenService>().Warn(ex, "Failed to generate GW2 subtoken.");
				return null;
			}
		}

		public void ClearSubtoken()
		{
			_cachedSubtoken = null;
			_subtokenExpiry = DateTime.MinValue;
		}

		public async Task RefreshAccountNameAsync()
		{
			_ = 1;
			try
			{
				if (Service.Gw2ApiManager == null || !Service.Gw2ApiManager.HasPermission((TokenPermission)1))
				{
					Service.AccountDisplayName = null;
					return;
				}
				Service.AccountDisplayName = (await ((IBlobClient<Account>)(object)Service.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken))).get_Name();
				await GenerateSubtokenAsync();
			}
			catch (Exception ex)
			{
				Logger.GetLogger<SubtokenService>().Warn(ex, "Failed to refresh GW2 account name.");
			}
		}
	}
}
