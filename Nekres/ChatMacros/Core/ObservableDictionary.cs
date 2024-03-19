using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Nekres.ChatMacros.Core
{
	internal class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, INotifyPropertyChanged
	{
		private Dictionary<TKey, TValue> _nameValues;

		public int Count => _nameValues.Count;

		public TValue this[TKey key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				_nameValues.TryGetValue(key, out var value);
				return value;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				_nameValues.TryGetValue(key, out var oldValue);
				if (object.Equals(oldValue, null) || !object.Equals(oldValue, value))
				{
					_nameValues[key] = value;
					FireDictionaryChanged();
				}
			}
		}

		public bool IsReadOnly => false;

		public ICollection<TKey> Keys => _nameValues.Keys;

		public ICollection<TValue> Values => _nameValues.Values;

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableDictionary()
		{
			_nameValues = new Dictionary<TKey, TValue>();
		}

		public void Add(TKey key, TValue val)
		{
			if (key == null || val == null)
			{
				throw new ArgumentNullException(object.Equals(key, null) ? "key" : "val");
			}
			_nameValues.Add(key, val);
			FireDictionaryChanged();
		}

		public void Clear()
		{
			if (_nameValues.Count > 0)
			{
				_nameValues.Clear();
				FireDictionaryChanged();
			}
		}

		public bool ContainsKey(TKey key)
		{
			return _nameValues.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			bool num = _nameValues.Remove(key);
			if (num)
			{
				FireDictionaryChanged();
			}
			return num;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _nameValues.GetEnumerator();
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>)_nameValues).GetEnumerator();
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return _nameValues.TryGetValue(key, out value);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)_nameValues).Add(pair);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_nameValues).Contains(pair);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_nameValues).Remove(pair);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] target, int startIndex)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (startIndex < 0 || startIndex > target.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			((ICollection<KeyValuePair<TKey, TValue>>)_nameValues).CopyTo(target, startIndex);
		}

		private void FireDictionaryChanged()
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(null));
			}
		}
	}
}
