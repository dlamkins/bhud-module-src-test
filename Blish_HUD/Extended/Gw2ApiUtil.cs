using System.Text.RegularExpressions;

namespace Blish_HUD.Extended
{
	public static class Gw2ApiUtil
	{
		public static bool HasCorrectFormat(string apiKey)
		{
			if (!string.IsNullOrWhiteSpace(apiKey))
			{
				return Regex.IsMatch(apiKey, "^[A-F0-9]{8}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{20}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{12}$");
			}
			return false;
		}
	}
}
