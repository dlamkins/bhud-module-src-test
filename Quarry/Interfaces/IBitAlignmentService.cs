using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gw2Sharp.WebApi.V2.Models;
using Quarry.WikiData.Achievement;

namespace Quarry.Interfaces
{
	public interface IBitAlignmentService
	{
		event Action<int> AlignmentLoaded;

		Task<IReadOnlyDictionary<int, Achievement>> GetAchievementsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default(CancellationToken));

		bool TryGetCachedAchievement(int achievementId, out Achievement achievement);

		Task PrefetchAsync(int achievementId, AchievementTableEntry achievement, CancellationToken cancellationToken = default(CancellationToken));

		int MapRowToBit(int achievementId, int rowIndex);

		int MapBitToRow(int achievementId, int bit);

		Task RunValidationAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
