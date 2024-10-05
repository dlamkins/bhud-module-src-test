using System;
using System.Collections.Generic;
using System.Linq;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ListExtension
	{
		public static bool ContainsAny<T>(this IEnumerable<T> sequence, params T[] matches)
		{
			IEnumerable<T> sequence2 = sequence;
			return matches.Any((T value) => sequence2.Contains(value));
		}

		public static bool ContainsAll<T>(this IEnumerable<T> sequence, params T[] matches)
		{
			IEnumerable<T> sequence2 = sequence;
			return matches.All((T value) => sequence2.Contains(value));
		}

		public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			foreach (T item in list)
			{
				action(item);
			}
		}

		public static void ForEach<T>(this IList<T> list, Action<T> action)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			foreach (T item in list)
			{
				action(item);
			}
		}

		public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
		{
			return (from x in Enumerable.Select(source, (T x, int i) => new
				{
					Index = i,
					Value = x
				})
				group x by x.Index / chunkSize into x
				select x.Select(v => v.Value).ToList()).ToList();
		}
	}
}
