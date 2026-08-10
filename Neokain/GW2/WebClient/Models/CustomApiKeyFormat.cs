using System;

namespace Neokain.GW2.WebClient.Models
{
	internal static class CustomApiKeyFormat
	{
		public const string KeyPrefix = "am_";

		public static bool IsCustomApiKey(string? token)
		{
			if (token == null || string.IsNullOrWhiteSpace(token))
			{
				return false;
			}
			Guid result;
			if (token!.StartsWith("am_", StringComparison.OrdinalIgnoreCase) && token!.Length == "am_".Length + 32)
			{
				return Guid.TryParse(token!.Substring("am_".Length), out result);
			}
			return false;
		}
	}
}
