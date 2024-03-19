using Blish_HUD;

namespace Nekres.ChatMacros.Core
{
	internal static class Gw2Util
	{
		public static bool IsInGame()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
			{
				return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return false;
		}
	}
}
