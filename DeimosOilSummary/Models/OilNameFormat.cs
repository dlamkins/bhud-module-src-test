namespace DeimosOilSummary.Models
{
	internal static class OilNameFormat
	{
		public static string Display(string characterName, string accountName)
		{
			bool hasCharacter = !string.IsNullOrWhiteSpace(characterName);
			bool hasAccount = !string.IsNullOrWhiteSpace(accountName);
			if (hasCharacter && hasAccount)
			{
				return characterName + " \"" + accountName + "\"";
			}
			if (hasCharacter)
			{
				return characterName;
			}
			if (hasAccount)
			{
				return "\"" + accountName + "\"";
			}
			return "Unknown";
		}
	}
}
