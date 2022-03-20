using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Behavior;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.State
{
	public class AchievementStates : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<AchievementStates>();

		private const double INTERVAL_CHECKACHIEVEMENTS = 300010.0;

		private double _lastAchievementCheck = 300010.0;

		private readonly ConcurrentDictionary<int, AchievementStatus> _achievementStates = new ConcurrentDictionary<int, AchievementStatus>();

		public AchievementStates(IRootPackState rootPackState)
			: base(rootPackState)
		{
		}

		public override Task Reload()
		{
			return Task.CompletedTask;
		}

		public override void Update(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateAsyncWithCadence(UpdateAchievements, gameTime, 300010.0, ref _lastAchievementCheck);
		}

		protected override Task<bool> Initialize()
		{
			PathingModule.Instance.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			return Task.FromResult(result: true);
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			lock (_achievementStates)
			{
				_achievementStates.Clear();
			}
			_lastAchievementCheck = 300010.0;
		}

		public override Task Unload()
		{
			return Task.CompletedTask;
		}

		public bool IsAchievementHidden(int achievementId, int achievementBit)
		{
			AchievementStatus achievement;
			lock (_achievementStates)
			{
				if (!_achievementStates.TryGetValue(achievementId, out achievement))
				{
					return false;
				}
			}
			if (achievement.Done)
			{
				return true;
			}
			return achievement.AchievementBits.Contains(achievementBit);
		}

		private async Task UpdateAchievements(GameTime gameTime)
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
					Logger.Debug("Getting user achievements from the API.");
					IApiV2ObjectList<AccountAchievement> achievements = await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)PathingModule.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
						.get_Achievements()).GetAsync(default(CancellationToken));
					lock (_achievementStates)
					{
						foreach (AccountAchievement achievement in (IEnumerable<AccountAchievement>)achievements)
						{
							_achievementStates.AddOrUpdate(achievement.get_Id(), new AchievementStatus(achievement), (int _, AchievementStatus _) => new AchievementStatus(achievement));
						}
					}
					Logger.Debug("Loaded {achievementCount} player achievements from the API.", new object[1] { ((IReadOnlyCollection<AccountAchievement>)achievements).Count });
				}
				else
				{
					Logger.Debug("Skipping user achievements from the API - API key does not give us permission.");
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load account achievements.");
			}
		}
	}
}
