using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.State
{
	public class MapchestState : APIState<string>
	{
		private readonly AccountState _accountState;

		public event EventHandler<string> MapchestCompleted;

		public event EventHandler<string> MapchestRemoved;

		public MapchestState(APIStateConfiguration configuration, Gw2ApiManager apiManager, AccountState accountState)
			: base(apiManager, configuration)
		{
			_accountState = accountState;
			base.APIObjectAdded += APIState_APIObjectAdded;
			base.APIObjectRemoved += APIState_APIObjectRemoved;
		}

		private void APIState_APIObjectRemoved(object sender, string e)
		{
			this.MapchestRemoved?.Invoke(this, e);
		}

		private void APIState_APIObjectAdded(object sender, string e)
		{
			this.MapchestCompleted?.Invoke(this, e);
		}

		public bool IsCompleted(string apiCode)
		{
			return base.APIObjectList.Contains(apiCode);
		}

		protected override void DoUnload()
		{
			base.APIObjectAdded -= APIState_APIObjectAdded;
			base.APIObjectRemoved -= APIState_APIObjectRemoved;
		}

		protected override async Task<List<string>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress)
		{
			Account account = _accountState.Account;
			DateTime obj = ((account != null) ? account.get_LastModified().UtcDateTime : DateTime.MinValue);
			DateTime now = DateTime.UtcNow;
			DateTime lastResetUTC = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
			if (obj < lastResetUTC)
			{
				Logger.Warn("Account has not been modified after reset.");
				return new List<string>();
			}
			return ((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_MapChests()).GetAsync(default(CancellationToken)))).ToList();
		}
	}
}
