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

namespace RaidClears.Features.Raids.Services
{
	public static class GetCurrentClearsService
	{
		private static readonly List<TokenPermission> NecessaryApiTokenPermissions = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)6
		};

		public static async Task<List<string>> GetClearsFromApi()
		{
			Gw2ApiManager gw2ApiManager = Service.Gw2ApiManager;
			Logger logger = Logger.GetLogger<Module>();
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)NecessaryApiTokenPermissions))
			{
				logger.Warn("HasPermissions() returned false. Possible reasons: API subToken does not have the necessary permissions: " + string.Join(", ", NecessaryApiTokenPermissions) + ". Or module did not get API subToken from Blish yet. Or API key is missing.");
				return new List<string>();
			}
			try
			{
				return ((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Raids()).GetAsync(default(CancellationToken)))).ToList();
			}
			catch (Exception e)
			{
				logger.Warn(e, "Could not get current clears from API");
				return new List<string>();
			}
		}
	}
}
