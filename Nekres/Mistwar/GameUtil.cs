using Blish_HUD;

namespace Nekres.Mistwar
{
	internal static class GameUtil
	{
		public static bool IsUiAvailable()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return false;
		}
	}
}
