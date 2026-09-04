using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Oberyn.AnglerAssociate.Models;

namespace Oberyn.AnglerAssociate.Services
{
	public class AchievementProgressService
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(AchievementProgressService));

		private readonly Dictionary<int, HashSet<int>> _completedBits = new Dictionary<int, HashSet<int>>();

		private readonly Dictionary<int, bool> _achievementDone = new Dictionary<int, bool>();

		public bool IsLoaded { get; private set; }

		public async Task RefreshAsync()
		{
			Gw2ApiManager apiManager = AnglerAssociateModule.Instance.Gw2ApiManager;
			if (!apiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)6
			}))
			{
				Logger.Debug("Skipping achievement refresh - missing account/progression permission.");
				IsLoaded = false;
				return;
			}
			try
			{
				IApiV2ObjectList<AccountAchievement> achievements = await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Achievements()).GetAsync(default(CancellationToken));
				_completedBits.Clear();
				_achievementDone.Clear();
				foreach (AccountAchievement achievement in (IEnumerable<AccountAchievement>)achievements)
				{
					_achievementDone[achievement.get_Id()] = achievement.get_Done();
					_completedBits[achievement.get_Id()] = ((achievement.get_Bits() != null) ? new HashSet<int>(achievement.get_Bits()) : new HashSet<int>());
				}
				IsLoaded = true;
				Logger.Debug("Loaded {0} account achievements.", new object[1] { ((IReadOnlyCollection<AccountAchievement>)achievements).Count });
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load account achievements.");
				IsLoaded = false;
			}
		}

		public bool IsCollectionDone(int? collectionId)
		{
			bool done = default(bool);
			return collectionId.HasValue && _achievementDone.TryGetValue(collectionId.Value, out done) && done;
		}

		public bool IsFishCaught(Fish fish)
		{
			if (!fish.CollectionId.HasValue || !fish.BitIndex.HasValue)
			{
				return false;
			}
			return IsBitCaught(fish.CollectionId.Value, fish.BitIndex.Value);
		}

		public bool IsFishCaughtForAvid(Fish fish)
		{
			if (!fish.AvidCollectionId.HasValue || !fish.BitIndex.HasValue)
			{
				return false;
			}
			return IsBitCaught(fish.AvidCollectionId.Value, fish.BitIndex.Value);
		}

		private bool IsBitCaught(int collectionId, int bitIndex)
		{
			if (_achievementDone.TryGetValue(collectionId, out var done) && done)
			{
				return true;
			}
			if (_completedBits.TryGetValue(collectionId, out var bits))
			{
				return bits.Contains(bitIndex);
			}
			return false;
		}
	}
}
