using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Mistwar
{
	internal static class GameUtil
	{
		private static readonly IReadOnlyList<int> EmergencyWayPoints = new List<int>
		{
			2244, 2100, 2195, 2091, 2209, 2248, 2207, 2347, 2337, 2350,
			2325, 2343, 2338, 2345, 2351, 2322, 2328, 2324, 2339, 2348,
			2341, 2228, 2293, 2243, 2252, 2109, 2263, 2145, 2280, 2154,
			2103, 2267, 2253, 2275, 2217, 2148, 2134
		};

		public static bool IsUiAvailable()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return false;
		}

		public static bool IsEmergencyWayPoint(ContinentFloorRegionMapPoi waypoint)
		{
			if (waypoint.get_Type() == ApiEnum<PoiType>.op_Implicit((PoiType)2))
			{
				return EmergencyWayPoints.Contains(waypoint.get_Id());
			}
			return false;
		}
	}
}
