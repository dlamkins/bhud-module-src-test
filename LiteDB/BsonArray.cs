using System;
using System.Collections;
using System.Collections.Generic;

namespace LiteDB
{
	public class BsonArray : BsonValue, IList<BsonValue>, ICollection<BsonValue>, IEnumerable<BsonValue>, IEnumerable
	{
		private int _length;

		public new IList<BsonValue> RawValue => (IList<BsonValue>)base.RawValue;

		public override BsonValue this[int index]
		{
			get
			{
				return RawValue[index];
			}
			set
			{
				RawValue[index] = value ?? BsonValue.Null;
			}
		}

		public int Count => RawValue.Count;

		public bool IsReadOnly => false;

		public BsonArray()
			: base(BsonType.Array, new List<BsonValue>())
		{
		}

		public BsonArray(List<BsonValue> array)
			: this()
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			AddRange(array);
		}

		public BsonArray(params BsonValue[] array)
			: this()
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			AddRange(array);
		}

		public BsonArray(IEnumerable<BsonValue> items)
			: this()
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			AddRange(items);
		}

		public void Add(BsonValue item)
		{
			RawValue.Add(item ?? BsonValue.Null);
		}

		public void AddRange<TCollection>(TCollection collection) where TCollection : ICollection<BsonValue>
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			List<BsonValue> list = (List<BsonValue>)base.RawValue;
			if (list.Capacity - list.Count < collection.Count)
			{
				list.Capacity += collection.Count;
			}
			foreach (BsonValue bsonValue in collection)
			{
				list.Add(bsonValue ?? BsonValue.Null);
			}
		}

		public void AddRange(IEnumerable<BsonValue> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			foreach (BsonValue item in items)
			{
				Add(item ?? BsonValue.Null);
			}
		}

		public void Clear()
		{
			RawValue.Clear();
		}

		public bool Contains(BsonValue item)
		{
			return RawValue.Contains(item ?? BsonValue.Null);
		}

		public void CopyTo(BsonValue[] array, int arrayIndex)
		{
			RawValue.CopyTo(array, arrayIndex);
		}

		public IEnumerator<BsonValue> GetEnumerator()
		{
			return RawValue.GetEnumerator();
		}

		public int IndexOf(BsonValue item)
		{
			return RawValue.IndexOf(item ?? BsonValue.Null);
		}

		public void Insert(int index, BsonValue item)
		{
			RawValue.Insert(index, item ?? BsonValue.Null);
		}

		public bool Remove(BsonValue item)
		{
			return RawValue.Remove(item);
		}

		public void RemoveAt(int index)
		{
			RawValue.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (BsonValue item in RawValue)
			{
				yield return item;
			}
		}

		public override int CompareTo(BsonValue other)
		{
			if (other.Type != BsonType.Array)
			{
				return base.Type.CompareTo(other.Type);
			}
			BsonArray otherArray = other.AsArray;
			int result = 0;
			int i = 0;
			int stop = Math.Min(Count, otherArray.Count);
			while (result == 0 && i < stop)
			{
				result = this[i].CompareTo(otherArray[i]);
				i++;
			}
			if (result != 0)
			{
				return result;
			}
			if (i == Count)
			{
				if (i != otherArray.Count)
				{
					return -1;
				}
				return 0;
			}
			return 1;
		}

		internal override int GetBytesCount(bool recalc)
		{
			if (!recalc && _length > 0)
			{
				return _length;
			}
			int length = 5;
			IList<BsonValue> array = RawValue;
			for (int i = 0; i < array.Count; i++)
			{
				length += GetBytesCountElement(i.ToString(), array[i]);
			}
			return _length = length;
		}
	}
}
