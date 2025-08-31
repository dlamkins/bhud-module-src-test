using Gw2Sharp.Models;

namespace Blish_HUD.Extended
{
	public static class MapTypeExtensions
	{
		public static bool IsWvW(this MapType type)
		{
			switch (type)
			{
			case MapType.Center:
			case MapType.BlueHome:
			case MapType.GreenHome:
			case MapType.RedHome:
			case MapType.JumpPuzzle:
			case MapType.EdgeOfTheMists:
			case MapType.WvwLounge:
				return true;
			default:
				return false;
			}
		}

		public static bool IsWvWMatch(this MapType type)
		{
			if ((uint)(type - 9) <= 3u)
			{
				return true;
			}
			return false;
		}

		public static bool IsInstance(this MapType type)
		{
			if (type == MapType.Instance || type == MapType.Tutorial || type == MapType.FortunesVale)
			{
				return true;
			}
			return false;
		}

		public static bool IsPublic(this MapType type)
		{
			if (type == MapType.Public || type == MapType.PublicMini)
			{
				return true;
			}
			return false;
		}

		public static bool IsTournament(this MapType type)
		{
			if (type == MapType.Tournament || type == MapType.UserTournament)
			{
				return true;
			}
			return false;
		}

		public static bool IsPvP(this MapType type)
		{
			if ((uint)(type - 2) <= 1u)
			{
				return true;
			}
			return type.IsTournament();
		}
	}
}
