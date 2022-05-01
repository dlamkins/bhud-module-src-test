using System;

namespace Universal_Search_Module
{
	public static class StringUtil
	{
		public static int ComputeLevenshteinDistance(string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];
			if (n == 0)
			{
				return m;
			}
			if (m == 0)
			{
				return n;
			}
			int j = 0;
			while (j <= n)
			{
				d[j, 0] = j++;
			}
			int l = 0;
			while (l <= m)
			{
				d[0, l] = l++;
			}
			for (int i = 1; i <= n; i++)
			{
				for (int k = 1; k <= m; k++)
				{
					int cost = ((t[k - 1] != s[i - 1]) ? 1 : 0);
					d[i, k] = Math.Min(Math.Min(d[i - 1, k] + 1, d[i, k - 1] + 1), d[i - 1, k - 1] + cost);
				}
			}
			return d[n, m];
		}

		public static string SanitizeTraitDescription(string description)
		{
			for (int indexOfClosingBracket = description.IndexOf('>'); indexOfClosingBracket != -1; indexOfClosingBracket = description.IndexOf('>'))
			{
				description = description.Remove(description.IndexOf('<'), indexOfClosingBracket - description.IndexOf('<') + 1);
			}
			return description;
		}
	}
}
