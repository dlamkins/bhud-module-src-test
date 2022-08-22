using System;
using System.Collections.Generic;
using System.Linq;

namespace Nekres.Mistwar
{
	internal static class EnumerableExtensions
	{
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				return true;
			}
			return !source.Any();
		}

		public static IEnumerable<(T, T)> By2<T>(this IEnumerable<T> source)
		{
			return source.Zip(source.Skip(1));
		}

		public static IEnumerable<(TA, TB)> Zip<TA, TB>(this IEnumerable<TA> source, IEnumerable<TB> other)
		{
			return source.Zip(other, (TA a, TB b) => (a, b));
		}

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, null);
		}

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			return source.MostBy(selector, comparer, max: true);
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, null);
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			return source.MostBy(selector, comparer, max: false);
		}

		private static TSource MostBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, bool max)
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
				if (comparer.Compare(candidateProjected, mostKey) * factor < 0)
				{
					most = candidate;
					mostKey = candidateProjected;
				}
			}
			return most;
		}

		public static T GetItem<T>(this List<T> array, int index)
		{
			if (index >= array.Count)
			{
				return array[index % array.Count];
			}
			if (index < 0)
			{
				return array[index % array.Count + array.Count];
			}
			return array[index];
		}
	}
}
