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

namespace Estreya.BlishHUD.EventTable.State
{
	public class MapchestState : APIState<string>
	{
		private static readonly Logger Logger = Logger.GetLogger<MapchestState>();

		private readonly AccountState _accountState;

		public event EventHandler<string> MapchestCompleted;

		public event EventHandler<string> MapchestRemoved;

		public MapchestState(Gw2ApiManager apiManager, AccountState accountState)
			: base(apiManager, new List<TokenPermission>
			{
				(TokenPermission)1,
				(TokenPermission)6
			}, (TimeSpan?)null, awaitLoad: true, -1)
		{
			_accountState = accountState;
			base.FetchAction = async delegate(Gw2ApiManager apiManager)
			{
				await _accountState.WaitAsync();
				Account account = _accountState.Account;
				DateTime obj = ((account != null) ? account.get_LastModified().UtcDateTime : DateTime.MinValue);
				DateTime now = EventTableModule.ModuleInstance.DateTimeNow.ToUniversalTime();
				DateTime lastResetUTC = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
				return (obj < lastResetUTC) ? new List<string>() : ((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_MapChests()).GetAsync(default(CancellationToken)))).ToList();
			};
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

		protected override Task Save()
		{
			return Task.CompletedTask;
		}

		public override Task DoClear()
		{
			return Task.CompletedTask;
		}

		protected override void DoUnload()
		{
			base.APIObjectAdded -= APIState_APIObjectAdded;
			base.APIObjectRemoved -= APIState_APIObjectRemoved;
		}
	}
}
