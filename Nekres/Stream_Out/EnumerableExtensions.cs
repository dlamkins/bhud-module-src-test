using System;
using System.Collections.Generic;
using System.Linq;

namespace Nekres.Stream_Out
{
	public static class EnumerableExtensions
	{
		public static T MaxBy<T, U>(this IEnumerable<T> data, Func<T, U> f) where U : IComparable
		{
			return data.Aggregate((T i1, T i2) => (f(i1).CompareTo(f(i2)) > 0) ? i1 : i2);
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> data)
		{
			return data != null && data.Count() <= 0;
		}
	}
}
