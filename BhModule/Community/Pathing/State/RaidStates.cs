using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.State
{
	public class RaidStates : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<RaidStates>();

		private const double INTERVAL_CHECKACHIEVEMENTS = 300010.0;

		private double _lastRaidCheck = 300010.0;

		private HashSet<string> _completedRaids;

		public RaidStates(IRootPackState rootPackState)
			: base(rootPackState)
		{
		}

		public override Task Reload()
		{
			return Task.CompletedTask;
		}

		public override void Update(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateAsyncWithCadence(UpdateRaids, gameTime, 300010.0, ref _lastRaidCheck);
		}

		protected override Task<bool> Initialize()
		{
			PathingModule.Instance.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			return Task.FromResult(result: true);
		}

		public bool AreRaidsComplete(IEnumerable<string> raids)
		{
			if (_completedRaids != null)
			{
				return _completedRaids.IsSupersetOf(raids);
			}
			return false;
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			_completedRaids = null;
			_lastRaidCheck = 300010.0;
		}

		private async Task UpdateRaids(GameTime gameTime)
		{
			if (!base.Running)
			{
				return;
			}
			try
			{
				if (PathingModule.Instance.Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
				{
					(TokenPermission)1,
					(TokenPermission)6
				}))
				{
					Logger.Debug("Getting user raids from the API.");
					_completedRaids = ((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)PathingModule.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
						.get_Raids()).GetAsync(default(CancellationToken)))).Select((string raid) => raid.ToLowerInvariant()).ToHashSet();
					Logger.Debug("Loaded {raidCount} completed raids from the API.", new object[1] { _completedRaids.Count() });
				}
				else
				{
					Logger.Debug("Skipping raid progress from the API - API key does not give us permission.");
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load account raid progress.");
			}
		}

		public override Task Unload()
		{
			PathingModule.Instance.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			return Task.CompletedTask;
		}
	}
}
