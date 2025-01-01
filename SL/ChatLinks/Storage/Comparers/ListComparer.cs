using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SL.ChatLinks.Storage.Comparers
{
	public sealed class ListComparer<T> : ValueComparer<IReadOnlyList<T>>
	{
		public ListComparer()
			: base((Expression<Func<IReadOnlyList<T>, IReadOnlyList<T>, bool>>)((IReadOnlyList<T> left, IReadOnlyList<T> right) => left.SequenceEqual(right)), (Expression<Func<IReadOnlyList<T>, int>>)((IReadOnlyList<T> list) => list.GetHashCode()), (Expression<Func<IReadOnlyList<T>, IReadOnlyList<T>>>)((IReadOnlyList<T> list) => GetSnapshot(list)))
		{
		}

		private static IReadOnlyList<T> GetSnapshot(IReadOnlyList<T> list)
		{
			int num = 0;
			T[] array = new T[list.Count];
			foreach (T item in list)
			{
				T val = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<T>(array);
		}
	}
}
