using System.Collections.Generic;
using System.Linq;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ListExtension
	{
		public static bool ContainsAny<T>(this IEnumerable<T> sequence, params T[] matches)
		{
			return matches.Any((T value) => sequence.Contains(value));
		}

		public static bool ContainsAll<T>(this IEnumerable<T> sequence, params T[] matches)
		{
			return matches.All((T value) => sequence.Contains(value));
		}

		public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
		{
			return (from x in source.Select((T x, int i) => new
				{
					Index = i,
					Value = x
				})
				group x by x.Index / chunkSize into x
				select x.Select(v => v.Value).ToList()).ToList();
		}
	}
}
