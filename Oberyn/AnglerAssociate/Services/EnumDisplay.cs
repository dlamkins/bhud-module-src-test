using System.Text.RegularExpressions;

namespace Oberyn.AnglerAssociate.Services
{
	public static class EnumDisplay
	{
		private static readonly string[] LowercaseWords = new string[3] { "Of", "And", "The" };

		public static string Format(object enumValue)
		{
			string spaced = Regex.Replace(enumValue.ToString(), "(?<=[a-z])(?=[A-Z])", " ");
			string[] lowercaseWords = LowercaseWords;
			foreach (string word in lowercaseWords)
			{
				spaced = Regex.Replace(spaced, "\\b" + word + "\\b", word.ToLowerInvariant());
			}
			return spaced;
		}
	}
}
