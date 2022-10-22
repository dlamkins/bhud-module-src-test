using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using RaidClears.Raids.Model;

namespace RaidClears.Raids.Services
{
	public static class GetCurrentClearsService
	{
		public static readonly List<TokenPermission> NECESSARY_API_TOKEN_PERMISSIONS = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)6
		};

		public static async Task<(ApiRaids, bool apiAccessFailed)> GetClearsFromApi(Gw2ApiManager gw2ApiManager, Logger logger)
		{
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)NECESSARY_API_TOKEN_PERMISSIONS))
			{
				logger.Warn("HasPermissions() returned false. Possible reasons: API subToken does not have the necessary permissions: " + string.Join(", ", NECESSARY_API_TOKEN_PERMISSIONS) + ". Or module did not get API subToken from Blish yet. Or API key is missing.");
				return (new ApiRaids(), true);
			}
			try
			{
				return (await GetCurrentClearsFromApi(gw2ApiManager, logger), false);
			}
			catch (Exception e)
			{
				logger.Warn(e, "Could not get current clears from API");
				return (new ApiRaids(), true);
			}
		}

		private static async Task<ApiRaids> GetCurrentClearsFromApi(Gw2ApiManager gw2ApiManager, Logger logger)
		{
			return new ApiRaids(((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Raids()).GetAsync(default(CancellationToken)))).ToList());
		}
	}
}
