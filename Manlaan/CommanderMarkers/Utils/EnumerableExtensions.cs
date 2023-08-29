using System;
using System.Collections.Generic;
using System.Linq;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class EnumerableExtensions
	{
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
		{
			return source.ToDictionary<KeyValuePair<TKey, TValue>, TKey, TValue>((KeyValuePair<TKey, TValue> x) => x.Key, (KeyValuePair<TKey, TValue> x) => x.Value);
		}

		public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> source)
		{
			return source.Select<T, (int, T)>((T t, int i) => (i, t));
		}

		public static IEnumerable<(T, T)> By2<T>(this IEnumerable<T> source)
		{
			return source.Zip(source.Skip(1));
		}

		public static IEnumerable<(TA, TB)> Zip<TA, TB>(this IEnumerable<TA> source, IEnumerable<TB> other)
		{
			return source.Zip<TA, TB, (TA, TB)>(other, (TA a, TB b) => (a, b));
		}

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, null);
		}

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey>? comparer)
		{
			return source.MostBy(selector, comparer, max: true);
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, null);
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey>? comparer)
		{
			return source.MostBy(selector, comparer, max: false);
		}

		private static TSource MostBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey>? comparer, bool max)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				comparer = Comparer<TKey>.Default;
			}
			int factor = ((!max) ? 1 : (-1));
			using IEnumerator<TSource> sourceIterator = source.GetEnumerator();
			if (!sourceIterator.MoveNext())
			{
				throw new InvalidOperationException("Sequence contains no elements");
			}
			TSource most = sourceIterator.Current;
			TKey mostKey = selector(most);
			while (sourceIterator.MoveNext())
			{
				TSource candidate = sourceIterator.Current;
				TKey candidateProjected = selector(candidate);
				if (comparer!.Compare(candidateProjected, mostKey) * factor < 0)
				{
					most = candidate;
					mostKey = candidateProjected;
				}
			}
			return most;
		}
	}
}
