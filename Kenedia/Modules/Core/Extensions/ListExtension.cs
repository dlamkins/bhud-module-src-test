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
	}
}
