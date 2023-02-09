using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Kenedia.Modules.Core.Extensions
{
	public static class StringExtension
	{
		private static readonly Regex s_diacritics = new Regex("\\p{M}");

		public static string RemoveDiacritics(this string s)
		{
			string result = s.Normalize(NormalizationForm.FormD);
			return s_diacritics.Replace(result, string.Empty).ToString().Replace("Æ", "Ae")
				.Replace("æ", "ae")
				.Replace("œ", "oe")
				.Replace("Œ", "Oe");
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
				string text2 = source;
				source = text;
				target = text2;
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
	}
}