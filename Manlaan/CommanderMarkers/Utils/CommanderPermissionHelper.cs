using Blish_HUD;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class CommanderPermissionHelper
	{
		public static bool HasCommanderPermissions()
		{
			if (Service.LtMode.get_Value())
			{
				return true;
			}
			if (GameService.Gw2Mumble.get_IsAvailable())
			{
				return GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander();
			}
			return false;
		}

		public static bool RequiresCommanderGate()
		{
			if (!Service.Settings._settingOnlyWhenCommander.get_Value())
			{
				return Service.LtMode.get_Value();
			}
			return true;
		}

		public static bool PassesCommanderGate()
		{
			if (!RequiresCommanderGate())
			{
				return true;
			}
			return HasCommanderPermissions();
		}
	}
}
