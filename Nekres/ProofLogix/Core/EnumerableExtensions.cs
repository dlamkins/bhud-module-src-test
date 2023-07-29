using System;
using System.Collections.Generic;
using System.Linq;

namespace Nekres.ProofLogix.Core
{
	public static class EnumerableExtensions
	{
		public static double Median<TColl, TValue>(this IEnumerable<TColl> source, Func<TColl, TValue> selector)
		{
			return source.Select(selector).Median();
		}

		public static double Median<T>(this IEnumerable<T> source)
		{
			if (Nullable.GetUnderlyingType(typeof(T)) != null)
			{
				source = source.Where((T x) => x != null);
			}
			int num = source.Count();
			if (num == 0)
			{
				throw new InvalidOperationException("Sequence contains no elements.");
			}
			source = source.OrderBy((T n) => n);
			int midpoint = num / 2;
			if (num % 2 == 0)
			{
				return (Convert.ToDouble(source.ElementAt(midpoint - 1)) + Convert.ToDouble(source.ElementAt(midpoint))) / 2.0;
			}
			return Convert.ToDouble(source.ElementAt(midpoint));
		}

		public static void RemoveAll<T>(this IList<T> collection, Func<T, bool> condition)
		{
			for (int i = collection.Count - 1; i >= 0; i--)
			{
				if (condition(collection[i]))
				{
					collection.RemoveAt(i);
				}
			}
		}

		public static void RemoveAll<T>(this IList<T> collection, T obj) where T : IEquatable<T>
		{
			for (int i = collection.Count - 1; i >= 0; i--)
			{
				if (collection[i].Equals(obj))
				{
					collection.RemoveAt(i);
				}
			}
		}

		public static void RemoveAll<T>(this IList<T> collection, Enum @enum)
		{
			for (int i = collection.Count - 1; i >= 0; i--)
			{
				if (collection[i].Equals(@enum))
				{
					collection.RemoveAt(i);
				}
			}
		}
	}
}
