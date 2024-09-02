using System;

namespace FarmingTracker
{
	public class Helper
	{
		public static string ConvertEnumValueToTextWithBlanks(string textWithUpperCasedWords)
		{
			string textWithBlanks = "";
			foreach (char character in textWithUpperCasedWords)
			{
				if (character != '_')
				{
					textWithBlanks += (char.IsUpper(character) ? $" {character}" : $"{character}");
				}
			}
			return textWithBlanks;
		}

		public static string CreateSwitchCaseNotFoundMessage<T>(T enumValue, string enumName, string usedFallbackText) where T : Enum
		{
			return $"Fallback: {usedFallbackText}. Because switch case missing or should not be be handled here: {enumName}.{enumValue}.";
		}
	}
}
