using Blish_HUD;
using Gw2Sharp.WebApi;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class BlishHUDUtils
	{
		public static string GetLocaleAsISO639_1()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected I4, but got Unknown
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			return (value - 1) switch
			{
				1 => "de", 
				0 => "es", 
				2 => "fr", 
				_ => "en", 
			};
		}
	}
}
