using System;
using System.Collections.Generic;

namespace Quarry.Interfaces
{
	public interface IHereExclusionService
	{
		IReadOnlyCollection<int> HiddenAchievementIds { get; }

		IReadOnlyDictionary<int, DateTime> SnoozedUntilUtc { get; }

		int TotalExcludedCount { get; }

		event Action Changed;

		void Hide(int achievementId);

		void Snooze(int achievementId);

		void Unhide(int achievementId);

		bool IsExcluded(int achievementId, DateTime nowUtc);

		bool IsSnoozed(int achievementId, DateTime nowUtc);

		void Load(IPersistenceService persistenceService);
	}
}
