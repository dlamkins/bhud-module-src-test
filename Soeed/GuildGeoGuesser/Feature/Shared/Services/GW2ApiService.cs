using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public static class GW2ApiService
	{
		private static string? _currentSubtoken;

		private static DateTime _subtokenExpiry = DateTime.MinValue;

		private static readonly List<TokenPermission> NecessaryApiTokenPermissions = new List<TokenPermission>(1) { (TokenPermission)1 };

		public static async Task<List<Guild>> QueryGuilds()
		{
			if (Service.Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)NecessaryApiTokenPermissions))
			{
				Logger.GetLogger<Module>().Info("Requesting guild info from server");
				return await Service.GeoServerWrapper.GetGuildsInfoAsync();
			}
			Logger.GetLogger<Module>().Warn("Missing necessary API permissions for guild loading");
			return new List<Guild>();
		}

		public static async Task<Account?> QueryAccount()
		{
			if (Service.Gw2ApiManager.HasPermission((TokenPermission)1))
			{
				Account acct = await ((IBlobClient<Account>)(object)Service.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken));
				(await ((IBlobClient<CreateSubtoken>)(object)Service.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_CreateSubtoken()
					.WithPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[1] { (TokenPermission)1 })
					.Expires((DateTimeOffset)DateTime.Now.AddDays(1.0))).GetAsync(default(CancellationToken))).get_Subtoken();
				return acct;
			}
			return null;
		}

		public static async Task<string?> GenerateSubtoken()
		{
			try
			{
				if (!Service.Gw2ApiManager.HasPermission((TokenPermission)1))
				{
					return null;
				}
				_currentSubtoken = (await ((IBlobClient<CreateSubtoken>)(object)Service.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_CreateSubtoken()
					.WithPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[1] { (TokenPermission)1 })
					.Expires((DateTimeOffset)DateTime.Now.AddDays(1.0))).GetAsync(default(CancellationToken))).get_Subtoken();
				_subtokenExpiry = DateTime.Now.AddDays(1.0);
				return _currentSubtoken;
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Error(ex, "Failed to generate subtoken");
				return null;
			}
		}

		public static async Task<string?> GetValidSubtoken()
		{
			if (!string.IsNullOrEmpty(_currentSubtoken) && DateTime.Now < _subtokenExpiry)
			{
				return _currentSubtoken;
			}
			return await GenerateSubtoken();
		}

		public static void ClearSubtoken()
		{
			_currentSubtoken = null;
			_subtokenExpiry = DateTime.MinValue;
		}
	}
}
