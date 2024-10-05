using System.Collections.Generic;
using Blish_HUD.Gw2Mumble;
using Gw2Sharp.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class MumbleExtension
	{
		public static bool IsCommonMap(this CurrentMap map)
		{
			MapType type = map.Type;
			if (type == MapType.Public || type == MapType.Tutorial || type == MapType.PublicMini)
			{
				return true;
			}
			return false;
		}

		public static bool IsPvpMap(this CurrentMap map)
		{
			return new List<int>(15)
			{
				549, 1305, 1171, 554, 795, 1163, 900, 894, 875, 984,
				1011, 1201, 1328, 1275, 1200
			}.Contains(map.Id);
		}

		public static bool IsWvWMap(this CurrentMap map)
		{
			return new List<int>(6) { 38, 95, 96, 1099, 899, 968 }.Contains(map.Id);
		}

		public static bool IsCompetitiveMap(this CurrentMap map)
		{
			if (!map.IsPvpMap())
			{
				return map.IsWvWMap();
			}
			return true;
		}
	}
}
