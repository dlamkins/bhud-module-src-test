using System;

namespace LiteDB
{
	public class SharedDataReader : IBsonDataReader, IDisposable
	{
		private readonly IBsonDataReader _reader;

		private readonly Action _dispose;

		private bool _disposed;

		public BsonValue this[string field] => _reader[field];

		public string Collection => _reader.Collection;

		public BsonValue Current => _reader.Current;

		public bool HasValues => _reader.HasValues;

		public SharedDataReader(IBsonDataReader reader, Action dispose)
		{
			_reader = reader;
			_dispose = dispose;
		}

		public bool Read()
		{
			return _reader.Read();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~SharedDataReader()
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
					_reader.Dispose();
					_dispose();
				}
			}
		}
	}
}
