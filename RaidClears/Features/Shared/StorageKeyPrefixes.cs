using System;

namespace RaidClears.Features.Shared
{
	public static class StorageKeyPrefixes
	{
		public const string Priority = "priority_";

		public const string Tomorrow = "tomorrow_";

		public static string NormalizeStorageKey(string key)
		{
			if (key == "priority" || key == "priority_tomorrow")
			{
				return key;
			}
			if (key.StartsWith("priority_", StringComparison.Ordinal))
			{
				return key.Substring("priority_".Length);
			}
			if (key.StartsWith("tomorrow_", StringComparison.Ordinal))
			{
				return key.Substring("tomorrow_".Length);
			}
			return key;
		}
	}
}
