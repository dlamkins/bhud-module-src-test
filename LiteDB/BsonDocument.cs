using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LiteDB.Engine;

namespace LiteDB
{
	public class BsonDocument : BsonValue, IDictionary<string, BsonValue>, ICollection<KeyValuePair<string, BsonValue>>, IEnumerable<KeyValuePair<string, BsonValue>>, IEnumerable
	{
		private int _length;

		public new IDictionary<string, BsonValue> RawValue => base.RawValue as IDictionary<string, BsonValue>;

		internal PageAddress RawId { get; set; } = PageAddress.Empty;


		public override BsonValue this[string key]
		{
			get
			{
				return RawValue.GetOrDefault(key, BsonValue.Null);
			}
			set
			{
				RawValue[key] = value ?? BsonValue.Null;
			}
		}

		public ICollection<string> Keys => RawValue.Keys;

		public ICollection<BsonValue> Values => RawValue.Values;

		public int Count => RawValue.Count;

		public bool IsReadOnly => false;

		public BsonDocument()
			: base(BsonType.Document, new Dictionary<string, BsonValue>(StringComparer.OrdinalIgnoreCase))
		{
		}

		public BsonDocument(ConcurrentDictionary<string, BsonValue> dict)
			: this()
		{
			if (dict == null)
			{
				throw new ArgumentNullException("dict");
			}
			foreach (KeyValuePair<string, BsonValue> element in dict)
			{
				Add(element);
			}
		}

		public BsonDocument(IDictionary<string, BsonValue> dict)
			: this()
		{
			if (dict == null)
			{
				throw new ArgumentNullException("dict");
			}
			foreach (KeyValuePair<string, BsonValue> element in dict)
			{
				Add(element);
			}
		}

		public override int CompareTo(BsonValue other)
		{
			if (other.Type != BsonType.Document)
			{
				return base.Type.CompareTo(other.Type);
			}
			string[] thisKeys = Keys.ToArray();
			int thisLength = thisKeys.Length;
			BsonDocument otherDoc = other.AsDocument;
			int otherLength = otherDoc.Keys.ToArray().Length;
			int result = 0;
			int i = 0;
			int stop = Math.Min(thisLength, otherLength);
			while (result == 0 && i < stop)
			{
				result = this[thisKeys[i]].CompareTo(otherDoc[thisKeys[i]]);
				i++;
			}
			if (result != 0)
			{
				return result;
			}
			if (i == thisLength)
			{
				if (i != otherLength)
				{
					return -1;
				}
				return 0;
			}
			return 1;
		}

		public bool ContainsKey(string key)
		{
			return RawValue.ContainsKey(key);
		}

		public IEnumerable<KeyValuePair<string, BsonValue>> GetElements()
		{
			if (RawValue.TryGetValue("_id", out var id))
			{
				yield return new KeyValuePair<string, BsonValue>("_id", id);
			}
			foreach (KeyValuePair<string, BsonValue> item in RawValue.Where((KeyValuePair<string, BsonValue> x) => x.Key != "_id"))
			{
				yield return item;
			}
		}

		public void Add(string key, BsonValue value)
		{
			RawValue.Add(key, value ?? BsonValue.Null);
		}

		public bool Remove(string key)
		{
			return RawValue.Remove(key);
		}

		public void Clear()
		{
			RawValue.Clear();
		}

		public bool TryGetValue(string key, out BsonValue value)
		{
			return RawValue.TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<string, BsonValue> item)
		{
			Add(item.Key, item.Value);
		}

		public bool Contains(KeyValuePair<string, BsonValue> item)
		{
			return RawValue.Contains(item);
		}

		public bool Remove(KeyValuePair<string, BsonValue> item)
		{
			return Remove(item.Key);
		}

		public IEnumerator<KeyValuePair<string, BsonValue>> GetEnumerator()
		{
			return RawValue.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return RawValue.GetEnumerator();
		}

		public void CopyTo(KeyValuePair<string, BsonValue>[] array, int arrayIndex)
		{
			RawValue.CopyTo(array, arrayIndex);
		}

		public void CopyTo(BsonDocument other)
		{
			using IEnumerator<KeyValuePair<string, BsonValue>> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, BsonValue> element = enumerator.Current;
				other[element.Key] = element.Value;
			}
		}

		internal override int GetBytesCount(bool recalc)
		{
			if (!recalc && _length > 0)
			{
				return _length;
			}
			int length = 5;
			foreach (KeyValuePair<string, BsonValue> element in RawValue)
			{
				length += GetBytesCountElement(element.Key, element.Value);
			}
			return _length = length;
		}
	}
}
