using System;
using System.Collections.Generic;
using System.Linq;

namespace Estreya.BlishHUD.EventTable.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> known = new HashSet<TKey>();
			return source.Where((TSource element) => known.Add(keySelector(element)));
		}
	}
}
