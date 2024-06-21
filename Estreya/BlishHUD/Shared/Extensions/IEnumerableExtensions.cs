using System;
using System.Collections.Generic;
using System.Linq;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<TResult> SelectWithIndex<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TSource>, TResult> selector)
		{
			return source.SelectWithIndex((TSource element, int index, IEnumerable<TSource> sourceList, bool first, bool last) => selector(element, index, sourceList));
		}

		public static IEnumerable<TResult> SelectWithIndex<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TSource>, bool, bool, TResult> selector)
		{
			List<TResult> newList = new List<TResult>();
			List<TSource> sourceList = source.ToList();
			for (int i = 0; i < sourceList.Count; i++)
			{
				bool first = i == 0;
				bool last = i == sourceList.Count - 1;
				newList.Add(selector(sourceList[i], i, source, first, last));
			}
			return newList.AsEnumerable();
		}

		public static IEnumerable<TResult> SelectManyWithIndex<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TSource>, IEnumerable<TResult>> selector)
		{
			return source.SelectManyWithIndex((TSource element, int index, IEnumerable<TSource> sourceList, bool first, bool last) => selector(element, index, sourceList));
		}

		public static IEnumerable<TResult> SelectManyWithIndex<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TSource>, bool, bool, IEnumerable<TResult>> selector)
		{
			List<TResult> newList = new List<TResult>();
			List<TSource> sourceList = source.ToList();
			for (int i = 0; i < sourceList.Count; i++)
			{
				bool first = i == 0;
				bool last = i == sourceList.Count - 1;
				newList.AddRange(selector(sourceList[i], i, source, first, last));
			}
			return newList.AsEnumerable();
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> known = new HashSet<TKey>();
			return source.Where((TSource element) => known.Add(keySelector(element)));
		}

		public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
		{
			return from x in source.Select((T x, int i) => new
				{
					Index = i,
					Value = x
				})
				group x by x.Index / chunkSize into x
				select from v in x
					select v.Value;
		}

		public static T PickRandom<T>(this IEnumerable<T> source)
		{
			return source.PickRandom(1).Single();
		}

		public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
		{
			return source.Shuffle().Take(count);
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.OrderBy((T x) => Guid.NewGuid());
		}
	}
}
