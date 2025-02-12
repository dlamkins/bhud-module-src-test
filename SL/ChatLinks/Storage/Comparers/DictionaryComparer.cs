using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SL.ChatLinks.Storage.Comparers
{
	public sealed class DictionaryComparer<TKey, TValue> : ValueComparer<IDictionary<TKey, TValue>>
	{
		public DictionaryComparer()
			: base((Expression<Func<IDictionary<TKey, TValue>, IDictionary<TKey, TValue>, bool>>)((IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right) => DictionaryEquals(left, right)), (Expression<Func<IDictionary<TKey, TValue>, int>>)((IDictionary<TKey, TValue> d) => GetDictionaryHashCode(d)), (Expression<Func<IDictionary<TKey, TValue>, IDictionary<TKey, TValue>>>)((IDictionary<TKey, TValue> d) => GetSnapshot(d)))
		{
		}

		private static bool DictionaryEquals(IDictionary<TKey, TValue>? left, IDictionary<TKey, TValue>? right)
		{
			IDictionary<TKey, TValue> right2 = right;
			if (left == right2)
			{
				return true;
			}
			if (left == null || right2 == null)
			{
				return false;
			}
			if (left!.Count != right2.Count)
			{
				return false;
			}
			TValue value;
			return left.All<KeyValuePair<TKey, TValue>>((KeyValuePair<TKey, TValue> pair) => right2.TryGetValue(pair.Key, out value) && object.Equals(pair.Value, value));
		}

		private static int GetDictionaryHashCode(IDictionary<TKey, TValue>? dictionary)
		{
			return dictionary?.Aggregate(0, (int hash, KeyValuePair<TKey, TValue> pair) => HashCode.Combine(hash, pair.Key, pair.Value)) ?? 0;
		}

		private static IDictionary<TKey, TValue> GetSnapshot(IDictionary<TKey, TValue> dictionary)
		{
			return new Dictionary<TKey, TValue>(dictionary);
		}
	}
}
