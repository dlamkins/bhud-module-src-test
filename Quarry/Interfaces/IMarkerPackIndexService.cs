using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Quarry.Models.Markers;

namespace Quarry.Interfaces
{
	public interface IMarkerPackIndexService
	{
		bool Ready { get; }

		event Action Changed;

		Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken));

		bool TryGet(int achievementId, out AchievementRoute route);

		IReadOnlyCollection<int> AchievementsOnMap(int mapId);
	}
}
