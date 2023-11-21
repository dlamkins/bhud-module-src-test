using System;
using System.Collections.Generic;
using System.Linq;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class IEnumerableExtensions
	{
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
	}
}
