using System;
using Microsoft.Data.Sqlite;
using SL.Common;

namespace SL.ChatLinks.Storage
{
	public static class Levenshtein
	{
		public static void RegisterLevenshteinFunction(SqliteConnection connection)
		{
			ThrowHelper.ThrowIfNull(connection, "connection");
			connection.CreateFunction("LevenshteinDistance", (string s, string t) => LevenshteinDistance(s, t));
		}

		public static int LevenshteinDistance(string a, string b)
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
			int[][] costs = new int[a.Length + 1][];
			for (int k = 0; k <= a.Length; k++)
			{
				costs[k] = new int[b.Length + 1];
			}
			for (int j = 0; j <= a.Length; j++)
			{
				costs[j][0] = j;
			}
			for (int m = 0; m <= b.Length; m++)
			{
				costs[0][m] = m;
			}
			for (int i = 1; i <= a.Length; i++)
			{
				for (int l = 1; l <= b.Length; l++)
				{
					int cost = ((b[l - 1] != a[i - 1]) ? 1 : 0);
					costs[i][l] = Math.Min(Math.Min(costs[i - 1][l] + 1, costs[i][l - 1] + 1), costs[i - 1][l - 1] + cost);
				}
			}
			return costs[a.Length][b.Length];
		}
	}
}
