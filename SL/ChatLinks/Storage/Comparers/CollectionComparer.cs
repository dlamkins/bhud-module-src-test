using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SL.ChatLinks.Storage.Comparers
{
	public sealed class CollectionComparer<T> : ValueComparer<IReadOnlyCollection<T>>
	{
		public CollectionComparer()
			: base((Expression<Func<IReadOnlyCollection<T>, IReadOnlyCollection<T>, bool>>)((IReadOnlyCollection<T> left, IReadOnlyCollection<T> right) => left.SequenceEqual(right)), (Expression<Func<IReadOnlyCollection<T>, int>>)((IReadOnlyCollection<T> collection) => collection.GetHashCode()), (Expression<Func<IReadOnlyCollection<T>, IReadOnlyCollection<T>>>)((IReadOnlyCollection<T> collection) => GetSnapshot(collection)))
		{
		}

		private static IReadOnlyCollection<T> GetSnapshot(IReadOnlyCollection<T> collection)
		{
			int num = 0;
			T[] array = new T[collection.Count];
			foreach (T item in collection)
			{
				T val = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<T>(array);
		}
	}
}
