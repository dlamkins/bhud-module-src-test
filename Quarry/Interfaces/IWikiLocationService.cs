using System.Collections.Generic;
using Quarry.Models.Markers;

namespace Quarry.Interfaces
{
	public interface IWikiLocationService
	{
		bool HasAnyLocations(int achievementId);

		IReadOnlyList<AchievementObjective> GetRemainingOnMap(int achievementId, int mapId);

		bool HasAreaOnlyRemaining(int achievementId, int mapId);
	}
}
