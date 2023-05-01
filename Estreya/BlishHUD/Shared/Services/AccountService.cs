using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Services
{
	public class AccountService : APIService<Account>
	{
		public Account Account
		{
			get
			{
				if (!base.APIObjectList.Any())
				{
					return null;
				}
				return base.APIObjectList.First();
			}
		}

		public AccountService(APIServiceConfiguration configuration, Gw2ApiManager apiManager)
			: base(apiManager, configuration)
		{
		}

		protected override void DoUnload()
		{
		}

		protected override async Task<List<Account>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress)
		{
			Account account = await ((IBlobClient<Account>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken));
			return new List<Account> { account };
		}
	}
}
