using Blish_HUD;
using Gw2Sharp.Models;
using Nekres.ChatMacros.Core.Services.Data;

namespace Nekres.ChatMacros.Core
{
	internal static class MapUtil
	{
		public static GameMode GetCurrentGameMode()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected I4, but got Unknown
			MapType type = GameService.Gw2Mumble.get_CurrentMap().get_Type();
			switch (type - 2)
			{
			case 0:
			case 1:
			case 4:
			case 6:
			case 13:
				return GameMode.PvP;
			case 2:
			case 3:
			case 5:
			case 14:
				if (GameService.Gw2Mumble.get_CurrentMap().get_Id() != 350)
				{
					return GameMode.PvE;
				}
				return GameMode.PvP;
			case 7:
			case 8:
			case 9:
			case 10:
			case 12:
			case 16:
				return GameMode.WvW;
			default:
				return GameMode.None;
			}
		}
	}
}
