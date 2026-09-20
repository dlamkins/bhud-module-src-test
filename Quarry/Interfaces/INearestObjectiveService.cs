using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Quarry.Models;

namespace Quarry.Interfaces
{
	public interface INearestObjectiveService
	{
		IReadOnlyList<RemainingObjective> GetRemaining(int achievementId, int mapId, Vector3 player);

		GuidanceInfo GetGuidance(int achievementId, int mapId);

		bool HasAnyObjectives(int achievementId);

		IReadOnlyList<string> Waypoints(int achievementId);

		WaypointSuggestion NearestWaypoint(int achievementId, int mapId, Vector3 player);
	}
}
