using System;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Utils
{
	public class Common
	{
		public static string GetRandomGUID()
		{
			return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
		}
	}
}
