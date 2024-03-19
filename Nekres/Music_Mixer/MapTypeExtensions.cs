using Gw2Sharp.Models;

namespace Nekres.Music_Mixer
{
	internal static class MapTypeExtensions
	{
		public static bool IsWvW(this MapType type)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected I4, but got Unknown
			switch (type - 9)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 5:
			case 6:
			case 9:
				return true;
			default:
				return false;
			}
		}

		public static bool IsInstance(this MapType type)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			if ((int)type == 4 || (int)type == 7 || (int)type == 13)
			{
				return true;
			}
			return false;
		}

		public static bool IsPublic(this MapType type)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)type == 5 || (int)type == 16)
			{
				return true;
			}
			return false;
		}

		public static bool IsTournament(this MapType type)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			if ((int)type == 6 || (int)type == 8)
			{
				return true;
			}
			return false;
		}

		public static bool IsPvP(this MapType type)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Invalid comparison between Unknown and I4
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (type - 2 <= 1)
			{
				return true;
			}
			return type.IsTournament();
		}
	}
}
