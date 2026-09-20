using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gw2Sharp.WebApi.V2.Models;
using Quarry.WikiData.Achievement;

namespace Quarry.Interfaces
{
	public interface IAchievementService : IDisposable
	{
		IReadOnlyList<AchievementTableEntry> Achievements { get; }

		DateTime AchievementTablesSnapshotDate { get; }

		IEnumerable<AchievementGroup> AchievementGroups { get; }

		IEnumerable<AchievementCategory> AchievementCategories { get; }

		IEnumerable<AccountAchievement> PlayerAchievements { get; }

		IReadOnlyDictionary<int, AccountAchievement> PlayerAchievementsById { get; }

		IReadOnlyDictionary<int, AchievementTableEntry> AchievementsById { get; }

		event Action PlayerAchievementsLoaded;

		event Action ApiAchievementsLoaded;

		Task<IReadOnlyList<CollectionAchievementTable>> GetAchievementDetailsAsync();

		bool HasFinishedAchievement(int achievementId);

		bool HasFinishedAchievementBit(int achievementId, int positionIndex);

		bool HasFinishedBitIndex(int achievementId, int bit);

		Task LoadPlayerAchievements(bool forceRefresh = false, CancellationToken cancellationToken = default(CancellationToken));

		void ToggleManualCompleteStatus(int achievementId, int bit);
	}
}
