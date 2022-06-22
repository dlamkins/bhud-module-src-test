using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.EventTable.State
{
	public class AccountState : APIState<Account>
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

		public AccountState(Gw2ApiManager apiManager)
			: base(apiManager, new List<TokenPermission> { (TokenPermission)1 }, (TimeSpan?)null, awaitLoad: true, -1)
		{
			base.FetchAction = async delegate(Gw2ApiManager apiManager)
			{
				Account account = await ((IBlobClient<Account>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken));
				return new List<Account> { account };
			};
		}

		public override Task DoClear()
		{
			return Task.CompletedTask;
		}

		protected override void DoUnload()
		{
		}

		protected override Task Save()
		{
			return Task.CompletedTask;
		}
	}
}
