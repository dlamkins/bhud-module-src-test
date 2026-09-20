using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Quarry.Models;

namespace Quarry.Interfaces
{
	public interface ICurrentMapService : IDisposable
	{
		int MapId { get; }

		string MapName { get; }

		event Action Changed;

		bool TryContinentToWorld(int mapId, double continentX, double continentY, out Vector2 world);

		bool TryGetWaypoints(int mapId, out IReadOnlyList<MapWaypoint> waypoints);

		bool TryGetSectors(int mapId, out IReadOnlyList<MapSector> sectors);

		bool TryGetMapName(int mapId, out string name);
	}
}
