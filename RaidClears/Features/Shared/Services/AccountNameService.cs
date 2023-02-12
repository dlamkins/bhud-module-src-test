using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace RaidClears.Features.Shared.Services
{
	public static class AccountNameService
	{
		public static string DEFAULT_ACCOUNT_NAME = "default";

		private static readonly List<TokenPermission> NecessaryApiTokenPermissions = new List<TokenPermission> { (TokenPermission)1 };

		public static async Task<string> UpdateAccountName()
		{
			Gw2ApiManager gw2ApiManager = Service.Gw2ApiManager;
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)NecessaryApiTokenPermissions))
			{
				return DEFAULT_ACCOUNT_NAME;
			}
			try
			{
				return (await ((IBlobClient<Account>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken))).get_Name();
			}
			catch (Exception)
			{
				return DEFAULT_ACCOUNT_NAME;
			}
		}
	}
}
