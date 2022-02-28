using System;
using System.Collections.Generic;
using System.Linq;

namespace Nekres.Mistwar
{
	internal static class EnumerableExtensions
	{
		public static T MaxBy<T, U>(this IEnumerable<T> data, Func<T, U> f) where U : IComparable
		{
			return data.Aggregate((T i1, T i2) => (f(i1).CompareTo(f(i2)) <= 0) ? i2 : i1);
		}

		public static T MinBy<T, U>(this IEnumerable<T> data, Func<T, U> f) where U : IComparable
		{
			return data.MinBy(f, null);
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			comparer = comparer ?? Comparer<TKey>.Default;
			using IEnumerator<TSource> sourceIterator = source.GetEnumerator();
			if (!sourceIterator.MoveNext())
			{
				throw new InvalidOperationException("Sequence contains no elements");
			}
			TSource min = sourceIterator.Current;
			TKey minKey = selector(min);
			while (sourceIterator.MoveNext())
			{
				TSource candidate = sourceIterator.Current;
				TKey candidateProjected = selector(candidate);
				if (comparer.Compare(candidateProjected, minKey) < 0)
				{
					min = candidate;
					minKey = candidateProjected;
				}
			}
			return min;
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> data)
		{
			return data?.Any() ?? false;
		}
	}
}
