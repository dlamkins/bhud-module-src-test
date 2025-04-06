using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SL.ChatLinks.Storage.Comparers
{
	public class ValueObjectComparer<T> : ValueComparer<T>
	{
		public ValueObjectComparer(Expression<Func<T, T>> snapshotExpression)
			: base((Expression<Func<T, T, bool>>)((T left, T right) => object.Equals(left, right)), (Expression<Func<T, int>>)((T obj) => obj.GetHashCode()), snapshotExpression)
		{
		}
	}
}
