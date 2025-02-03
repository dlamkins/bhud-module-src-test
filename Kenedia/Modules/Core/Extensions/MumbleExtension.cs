using System.Collections.Generic;
using Blish_HUD.Gw2Mumble;
using Gw2Sharp.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class MumbleExtension
	{
		public static bool IsCommonMap(this CurrentMap map)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			MapType type = map.Type;
			if ((int)type == 5 || (int)type == 7 || (int)type == 16)
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
