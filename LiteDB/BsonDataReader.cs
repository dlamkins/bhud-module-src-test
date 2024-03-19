using System;
using System.Collections.Generic;

namespace LiteDB
{
	public class BsonDataReader : IBsonDataReader, IDisposable
	{
		private readonly IEnumerator<BsonValue> _source;

		private readonly string _collection;

		private readonly bool _hasValues;

		private BsonValue _current;

		private bool _isFirst;

		private bool _disposed;

		public bool HasValues => _hasValues;

		public BsonValue Current => _current;

		public string Collection => _collection;

		public BsonValue this[string field] => _current.AsDocument[field] ?? BsonValue.Null;

		internal BsonDataReader()
		{
			_hasValues = false;
		}

		internal BsonDataReader(BsonValue value, string collection = null)
		{
			_current = value;
			_isFirst = (_hasValues = true);
			_collection = collection;
		}

		internal BsonDataReader(IEnumerable<BsonValue> values, string collection)
		{
			_source = values.GetEnumerator();
			_collection = collection;
			if (_source.MoveNext())
			{
				_hasValues = (_isFirst = true);
				_current = _source.Current;
			}
		}

		public bool Read()
		{
			if (!_hasValues)
			{
				return false;
			}
			if (_isFirst)
			{
				_isFirst = false;
				return true;
			}
			if (_source != null)
			{
				bool result = _source.MoveNext();
				_current = _source.Current;
				return result;
			}
			return false;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~BsonDataReader()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_disposed = true;
				if (disposing)
				{
					_source?.Dispose();
				}
			}
		}
	}
}
