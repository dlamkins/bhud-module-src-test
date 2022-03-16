using System.Globalization;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace Nekres.Stream_Out
{
	public static class OverlayServiceExtensions
	{
		public static CultureInfo CultureInfo(this OverlayService overlay)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected I4, but got Unknown
			Locale value = overlay.get_UserLocale().get_Value();
			return (int)value switch
			{
				0 => new CultureInfo("en-US"), 
				1 => new CultureInfo("es-ES"), 
				2 => new CultureInfo("de-DE"), 
				3 => new CultureInfo("fr-FR"), 
				4 => new CultureInfo("ko-KR"), 
				5 => new CultureInfo("zh-CN"), 
				_ => System.Globalization.CultureInfo.CurrentCulture, 
			};
		}
	}
}
