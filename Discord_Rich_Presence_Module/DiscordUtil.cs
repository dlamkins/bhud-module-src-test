using System.Text.RegularExpressions;

namespace Discord_Rich_Presence_Module
{
	public static class DiscordUtil
	{
		public static string TruncateLength(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			if (value.Length > maxLength)
			{
				return value.Substring(0, maxLength);
			}
			return value;
		}

		public static string GetDiscordSafeString(string text)
		{
			return Regex.Replace(text.Replace(":", "").Trim(), "[^a-zA-Z]+", "_").ToLowerInvariant();
		}
	}
}
