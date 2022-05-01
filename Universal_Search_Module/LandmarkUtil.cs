using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Universal_Search_Module
{
	public static class LandmarkUtil
	{
		public static ContinentFloorRegionMapPoi GetClosestWaypoint(IEnumerable<ContinentFloorRegionMapPoi> waypoints, ContinentFloorRegionMapPoi landmark)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			ContinentFloorRegionMapPoi closestWaypoint = null;
			float distance = float.MaxValue;
			Vector2 staticPos = default(Vector2);
			((Vector2)(ref staticPos))._002Ector((float)landmark.Coord.X, (float)landmark.Coord.Y);
			Vector2 pos = default(Vector2);
			foreach (ContinentFloorRegionMapPoi waypoint in waypoints)
			{
				((Vector2)(ref pos))._002Ector((float)waypoint.Coord.X, (float)waypoint.Coord.Y);
				float netDistance = Vector2.Distance(staticPos, pos);
				if (netDistance < distance)
				{
					closestWaypoint = waypoint;
					distance = netDistance;
				}
			}
			return closestWaypoint;
		}
	}
}
