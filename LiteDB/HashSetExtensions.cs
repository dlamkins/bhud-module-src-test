using System.Collections.Generic;

namespace LiteDB
{
	internal static class HashSetExtensions
	{
		public static HashSet<T> AddRange<T>(this HashSet<T> hash, IEnumerable<T> items)
		{
			if (items == null)
			{
				return hash;
			}
			foreach (T item in items)
			{
				hash.Add(item);
			}
			return hash;
		}
	}
}
