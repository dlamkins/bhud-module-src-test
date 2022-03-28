using Gw2Sharp.Models;

namespace Kenedia.Modules.Characters
{
	internal static class MapTypeExtensions
	{
		public static bool IsWorldVsWorld(this MapType type)
		{
			if ((uint)(type - 9) <= 3u || type == MapType.EdgeOfTheMists)
			{
				return true;
			}
			return false;
		}

		public static bool IsCompetitive(this MapType type)
		{
			switch (type)
			{
			case MapType.Pvp:
			case MapType.Tournament:
			case MapType.UserTournament:
			case MapType.RedHome:
				return true;
			default:
				return false;
			}
		}
	}
}
