using System;
using System.Collections.Generic;

namespace LiteDB
{
	internal static class LinqExtensions
	{
		public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
		{
			using IEnumerator<T> enumerator = source.GetEnumerator();
			while (enumerator.MoveNext())
			{
				yield return YieldBatchElements(enumerator, batchSize - 1);
			}
		}

		private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, int batchSize)
		{
			yield return source.Current;
			for (int i = 0; i < batchSize; i++)
			{
				if (!source.MoveNext())
				{
					break;
				}
				yield return source.Current;
			}
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			return _();
			IEnumerable<TSource> _()
			{
				HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
				foreach (TSource element in source)
				{
					if (knownKeys.Add(keySelector(element)))
					{
						yield return element;
					}
				}
			}
		}

		public static IEnumerable<LastItem<T>> IsLast<T>(this IEnumerable<T> source) where T : class
		{
			T last = null;
			foreach (T item in source)
			{
				if (last != null)
				{
					yield return new LastItem<T>
					{
						Item = last,
						IsLast = false
					};
				}
				last = item;
			}
			if (last != null)
			{
				yield return new LastItem<T>
				{
					Item = last,
					IsLast = true
				};
			}
		}
	}
}
