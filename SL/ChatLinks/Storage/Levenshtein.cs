using System;
using Microsoft.Data.Sqlite;

namespace SL.ChatLinks.Storage
{
	public class Levenshtein
	{
		public static void RegisterLevenshteinFunction(SqliteConnection connection)
		{
			connection.CreateFunction("LevenshteinDistance", (string s, string t) => LevenshteinDistance(s, t));
		}

		private static int LevenshteinDistance(string a, string b)
		{
			if (string.IsNullOrEmpty(a))
			{
				if (!string.IsNullOrEmpty(b))
				{
					return b.Length;
				}
				return 0;
			}
			if (string.IsNullOrEmpty(b))
			{
				return a.Length;
			}
			int[,] costs = new int[a.Length + 1, b.Length + 1];
			for (int j = 0; j <= a.Length; j++)
			{
				costs[j, 0] = j;
			}
			for (int l = 0; l <= b.Length; l++)
			{
				costs[0, l] = l;
			}
			for (int i = 1; i <= a.Length; i++)
			{
				for (int k = 1; k <= b.Length; k++)
				{
					int cost = ((b[k - 1] != a[i - 1]) ? 1 : 0);
					costs[i, k] = Math.Min(Math.Min(costs[i - 1, k] + 1, costs[i, k - 1] + 1), costs[i - 1, k - 1] + cost);
				}
			}
			return costs[a.Length, b.Length];
		}
	}
}
