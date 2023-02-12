using Microsoft.Xna.Framework;
using RaidClears.Settings.Models;

namespace RaidClears.Utils
{
	public static class StringExtensions
	{
		public static Color HexToXnaColor(this string s)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return new ColorHelper(s).XnaColor;
		}
	}
}
