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
	public class MapchestState : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<MapchestState>();

		private TimeSpan updateInterval = TimeSpan.FromMinutes(5.0).Add(TimeSpan.FromMilliseconds(100.0));

		private double timeSinceUpdate;

		private List<string> completedMapchests = new List<string>();

		private Gw2ApiManager ApiManager { get; set; }

		public event EventHandler<string> MapchestCompleted;

		public MapchestState(Gw2ApiManager apiManager)
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
			return completedMapchests.Contains(apiCode);
		}

		public override async Task InternalReload()
		{
			await UpdatedCompletedMapchests(null);
		}

		private async Task UpdatedCompletedMapchests(GameTime gameTime)
		{
			Logger.Info("Check for completed mapchests.");
			try
			{
				List<string> oldCompletedMapchests;
				lock (completedMapchests)
				{
					oldCompletedMapchests = completedMapchests.ToArray().ToList();
					completedMapchests.Clear();
				}
				if (!ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
				{
					(TokenPermission)1,
					(TokenPermission)6
				}))
				{
					return;
				}
				IApiV2ObjectList<string> mapchests = await ((IBlobClient<IApiV2ObjectList<string>>)(object)ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_MapChests()).GetAsync(default(CancellationToken));
				lock (completedMapchests)
				{
					completedMapchests.AddRange((IEnumerable<string>)mapchests);
				}
				foreach (string mapchest in (IEnumerable<string>)mapchests)
				{
					if (!oldCompletedMapchests.Contains(mapchest))
					{
						Logger.Info("Completed mapchest: " + mapchest);
						try
						{
							this.MapchestCompleted?.Invoke(this, mapchest);
						}
						catch (Exception ex2)
						{
							Logger.Error("Error handling complete mapchest event: " + ex2.Message);
						}
					}
				}
			}
			catch (MissingScopesException val)
			{
				MissingScopesException msex = val;
				Logger.Warn("Could not update completed mapchests due to missing scopes: " + ((Exception)(object)msex).Message);
			}
			catch (InvalidAccessTokenException val2)
			{
				InvalidAccessTokenException iatex = val2;
				Logger.Warn("Could not update completed mapchests due to invalid access token: " + ((Exception)(object)iatex).Message);
			}
			catch (Exception ex)
			{
				Logger.Warn("Error updating completed mapchests: " + ex.Message);
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
			UpdateUtil.UpdateAsync(UpdatedCompletedMapchests, gameTime, updateInterval.TotalMilliseconds, ref timeSinceUpdate);
		}

		protected override async Task Load()
		{
			await UpdatedCompletedMapchests(null);
		}

		protected override Task Save()
		{
			return Task.CompletedTask;
		}

		public override Task Clear()
		{
			lock (completedMapchests)
			{
				completedMapchests.Clear();
			}
			return Task.CompletedTask;
		}
	}
}
