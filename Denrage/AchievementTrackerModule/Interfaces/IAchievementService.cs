using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Gw2Sharp.WebApi.V2.Models;

namespace Denrage.AchievementTrackerModule.Interfaces
{
	public interface IAchievementService
	{
		IReadOnlyList<AchievementTableEntry> Achievements { get; }

		IReadOnlyList<CollectionAchievementTable> AchievementDetails { get; }

		IEnumerable<AchievementGroup> AchievementGroups { get; }

		IEnumerable<AchievementCategory> AchievementCategories { get; }

		event Action PlayerAchievementsLoaded;

		event Action ApiAchievementsLoaded;

		Task<string> GetDirectImageLink(string imagePath, CancellationToken cancellationToken = default(CancellationToken));

		AsyncTexture2D GetImage(string imageUrl, Action beforeSwap);

		AsyncTexture2D GetImageFromIndirectLink(string imagePath, Action beforeSwap);

		bool HasFinishedAchievement(int achievementId);

		bool HasFinishedAchievementBit(int achievementId, int positionIndex);

		Task LoadPlayerAchievements(bool forceRefresh = false, CancellationToken cancellationToken = default(CancellationToken));
	}
}
