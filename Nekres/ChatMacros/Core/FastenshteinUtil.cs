using System;
using System.Collections.Generic;
using System.Linq;
using Fastenshtein;

namespace Nekres.ChatMacros.Core
{
	internal static class FastenshteinUtil
	{
		public static IEnumerable<T> FindMatchesBy<T>(string needle, IEnumerable<T> list, Func<T, string> expression)
		{
			return FindMatchesBy(needle, list, (T item) => new List<string> { expression(item) });
		}

		public static IEnumerable<T> FindMatchesBy<T>(string needle, IEnumerable<T> list, Func<T, IEnumerable<string>> expression)
		{
			if (string.IsNullOrWhiteSpace(needle) || list == null)
			{
				return Enumerable.Empty<T>();
			}
			return list.Where((T o) => expression(o)?.Any((string str) => str?.ToLowerInvariant().Contains(needle.ToLowerInvariant()) ?? false) ?? false);
		}

		public static string FindClosestMatch(string needle, bool ignoreCase = true, params string[] list)
		{
			return FindClosestMatchBy(needle, list, (string str) => new List<string> { str }, ignoreCase);
		}

		public static T FindClosestMatchBy<T>(string needle, IEnumerable<T> list, Func<T, string> expression, bool ignoreCase = true)
		{
			return FindClosestMatchBy(needle, list, (T item) => new List<string> { expression(item) }, ignoreCase);
		}

		public static T FindClosestMatchBy<T>(string needle, IEnumerable<T> list, Func<T, IEnumerable<string>> expression, bool ignoreCase = true)
		{
			if (string.IsNullOrWhiteSpace(needle) || needle.Length == 0)
			{
				return default(T);
			}
			list = FindMatchesBy(needle, list, expression);
			needle = (ignoreCase ? needle.ToLowerInvariant() : needle);
			T result = default(T);
			int minScore = 20000;
			Levenshtein lev = new Levenshtein(needle);
			foreach (T item in list)
			{
				List<string> property = expression(item)?.ToList();
				if (property == null || !property.Any())
				{
					continue;
				}
				foreach (string str in property)
				{
					string testStr = (ignoreCase ? str.ToLowerInvariant() : str);
					if (IsCloser(lev, testStr, ref minScore))
					{
						result = item;
					}
				}
			}
			return result;
		}

		private static bool IsCloser(Levenshtein lev, string test, ref int minScore)
		{
			int score = lev.DistanceFrom(test);
			if (score < minScore)
			{
				minScore = score;
				return true;
			}
			return false;
		}
	}
}
