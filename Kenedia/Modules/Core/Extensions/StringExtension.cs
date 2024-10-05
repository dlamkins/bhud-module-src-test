using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Kenedia.Modules.Core.Extensions
{
	public static class StringExtension
	{
		private static readonly Regex s_diacritics = new Regex("\\p{M}");

		private static readonly Regex s_namingConventionConvert = new Regex("\r\n                (?<=[A-Z])(?=[A-Z][a-z]) |\r\n                 (?<=[^A-Z])(?=[A-Z]) |\r\n                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

		public static string RemoveDiacritics(this string s)
		{
			string result = s.Normalize(NormalizationForm.FormD);
			return s_diacritics.Replace(result, string.Empty).ToString().Replace("Æ", "Ae")
				.Replace("æ", "ae")
				.Replace("œ", "oe")
				.Replace("Œ", "Oe");
		}

		public static string ToLowercaseNamingConvention(this string s)
		{
			return s_namingConventionConvert.Replace(s.Replace("_", ""), " ");
		}

		public static string RemoveLeadingNumbers(this string input)
		{
			return Regex.Replace(input, "^\\d+", "");
		}

		public static string RemoveSpaces(this string input)
		{
			return input.Replace(" ", "");
		}

		public static string SplitStringOnUppercase(this string input)
		{
			StringBuilder result = new StringBuilder();
			foreach (char c in input)
			{
				if (char.IsUpper(c) && result.Length > 0)
				{
					result.Append(' ');
				}
				result.Append(c);
			}
			return result.ToString();
		}

		public static int LevenshteinDistance(this string source, string target)
		{
			if (string.IsNullOrEmpty(source))
			{
				if (!string.IsNullOrEmpty(target))
				{
					return target.Length;
				}
				return 0;
			}
			if (string.IsNullOrEmpty(target))
			{
				return source.Length;
			}
			if (source.Length > target.Length)
			{
				string text = target;
				target = source;
				source = text;
			}
			int l = target.Length;
			int m = source.Length;
			int[,] distance = new int[2, l + 1];
			for (int k = 1; k <= l; k++)
			{
				distance[0, k] = k;
			}
			int currentRow = 0;
			for (int i = 1; i <= m; i++)
			{
				currentRow = i & 1;
				distance[currentRow, 0] = i;
				int previousRow = currentRow ^ 1;
				for (int j = 1; j <= l; j++)
				{
					int cost = ((target[j - 1] != source[i - 1]) ? 1 : 0);
					distance[currentRow, j] = Math.Min(Math.Min(distance[previousRow, j] + 1, distance[currentRow, j - 1] + 1), distance[previousRow, j - 1] + cost);
				}
			}
			return distance[currentRow, l];
		}

		public static bool ContainsAny(this string haystack, params string[] needles)
		{
			foreach (string needle in needles)
			{
				if (haystack.Contains(needle))
				{
					return true;
				}
			}
			return false;
		}

		public static bool ContainsAnyTrimmed(this string haystack, params string[] needles)
		{
			foreach (string needle in needles)
			{
				if (haystack.Contains(needle.Trim()))
				{
					return true;
				}
			}
			return false;
		}
	}
}
