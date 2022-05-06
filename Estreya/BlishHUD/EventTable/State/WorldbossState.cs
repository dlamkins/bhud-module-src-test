using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Helpers;
using Estreya.BlishHUD.EventTable.Utils;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.State
{
	public class WorldbossState : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<WorldbossState>();

		private TimeSpan updateInterval = TimeSpan.FromMinutes(5.0).Add(TimeSpan.FromMilliseconds(100.0));

		private double timeSinceUpdate;

		private List<string> completedWorldbosses = new List<string>();

		private Gw2ApiManager ApiManager { get; set; }

		public event EventHandler<string> WorldbossCompleted;

		public WorldbossState(Gw2ApiManager apiManager)
		{
			ApiManager = apiManager;
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

		public override async Task InternalReload()
		{
			await UpdateCompletedWorldbosses(null);
		}

		private async Task UpdateCompletedWorldbosses(GameTime gameTime)
		{
			Logger.Info("Check for completed worldbosses.");
			try
			{
				List<string> oldCompletedWorldbosses;
				lock (completedWorldbosses)
				{
					oldCompletedWorldbosses = completedWorldbosses.ToArray().ToList();
					completedWorldbosses.Clear();
				}
				if (!ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
				{
					(TokenPermission)1,
					(TokenPermission)6
				}))
				{
					return;
				}
				List<string> bosses = ((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_WorldBosses()).GetAsync(default(CancellationToken)))).ToList();
				lock (completedWorldbosses)
				{
					completedWorldbosses.AddRange(bosses);
				}
				foreach (string boss in bosses)
				{
					if (!oldCompletedWorldbosses.Contains(boss))
					{
						Logger.Info("Completed worldboss: " + boss);
						try
						{
							this.WorldbossCompleted?.Invoke(this, boss);
						}
						catch (Exception ex2)
						{
							Logger.Error("Error handling complete worldboss event: " + ex2.Message);
						}
					}
				}
			}
			catch (MissingScopesException val)
			{
				MissingScopesException msex = val;
				Logger.Warn("Could not update completed worldbosses due to missing scopes: " + ((Exception)(object)msex).Message);
			}
			catch (InvalidAccessTokenException val2)
			{
				InvalidAccessTokenException iatex = val2;
				Logger.Warn("Could not update completed worldbosses due to invalid access token: " + ((Exception)(object)iatex).Message);
			}
			catch (Exception ex)
			{
				Logger.Warn("Error updating completed worldbosses: " + ex.Message);
			}
		}

		protected override Task Initialize()
		{
			ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)ApiManager_SubtokenUpdated);
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)ApiManager_SubtokenUpdated);
			AsyncHelper.RunSync(Clear);
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(UpdateCompletedWorldbosses, gameTime, updateInterval.TotalMilliseconds, ref timeSinceUpdate);
		}

		protected override async Task Load()
		{
			await UpdateCompletedWorldbosses(null);
		}

		protected override Task Save()
		{
			return Task.CompletedTask;
		}

		public override Task Clear()
		{
			lock (completedWorldbosses)
			{
				completedWorldbosses.Clear();
			}
			return Task.CompletedTask;
		}
	}
}
