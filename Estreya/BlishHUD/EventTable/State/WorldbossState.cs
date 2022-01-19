using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Utils;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.State
{
	internal class WorldbossState : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<WorldbossState>();

		private TimeSpan updateInterval = TimeSpan.FromMinutes(5.0).Add(TimeSpan.FromMilliseconds(100.0));

		private double timeSinceUpdate;

		private List<string> completedWorldbosses = new List<string>();

		private Gw2ApiManager ApiManager { get; set; }

		public WorldbossState(Gw2ApiManager apiManager)
		{
			ApiManager = apiManager;
			ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)ApiManager_SubtokenUpdated);
		}

		private void ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Task.Run(async delegate
			{
				await Reload();
			});
		}

		public bool IsCompleted(string apiCode)
		{
			return completedWorldbosses.Contains(apiCode);
		}

		public override async Task Reload()
		{
			await UpdateCompletedWorldbosses(null);
		}

		private async Task UpdateCompletedWorldbosses(GameTime gameTime)
		{
			try
			{
				lock (completedWorldbosses)
				{
					completedWorldbosses.Clear();
				}
				if (ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
				{
					(TokenPermission)1,
					(TokenPermission)6
				}))
				{
					IApiV2ObjectList<string> bosses = await ((IBlobClient<IApiV2ObjectList<string>>)(object)ApiManager.get_Gw2ApiClient().get_V2().get_Account()
						.get_WorldBosses()).GetAsync(default(CancellationToken));
					lock (completedWorldbosses)
					{
						completedWorldbosses.AddRange((IEnumerable<string>)bosses);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Error updating completed worldbosses: " + ex.Message);
			}
		}

		protected override Task Initialize()
		{
			return Task.CompletedTask;
		}

		protected override Task InternalUnload()
		{
			lock (completedWorldbosses)
			{
				completedWorldbosses.Clear();
			}
			return Task.CompletedTask;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateAsyncWithCadence(UpdateCompletedWorldbosses, gameTime, updateInterval.TotalMilliseconds, ref timeSinceUpdate);
		}

		protected override async Task Load()
		{
			await UpdateCompletedWorldbosses(null);
		}

		protected override Task Save()
		{
			return Task.CompletedTask;
		}
	}
}
