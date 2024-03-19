using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiteDB.Engine
{
	internal class SortContainer : IDisposable
	{
		private readonly Collation _collation;

		private readonly int _size;

		private int _remaining;

		private int _count;

		private bool _isEOF;

		private int _readPosition;

		private BufferReader _reader;

		public KeyValuePair<BsonValue, PageAddress> Current;

		public bool IsEOF => _isEOF;

		public long Position { get; set; } = -1L;


		public int Count => _count;

		public SortContainer(Collation collation, int size)
		{
			_collation = collation;
			_size = size;
		}

		public void Insert(IEnumerable<KeyValuePair<BsonValue, PageAddress>> items, int order, BufferSlice buffer)
		{
			IOrderedEnumerable<KeyValuePair<BsonValue, PageAddress>> obj = ((order == 1) ? items.OrderBy((KeyValuePair<BsonValue, PageAddress> x) => x.Key, _collation) : items.OrderByDescending((KeyValuePair<BsonValue, PageAddress> x) => x.Key, _collation));
			int offset = 0;
			foreach (KeyValuePair<BsonValue, PageAddress> item in obj)
			{
				buffer.WriteIndexKey(item.Key, offset);
				int keyLength = IndexNode.GetKeyLength(item.Key, recalc: false);
				if (keyLength > 1023)
				{
					throw LiteException.InvalidIndexKey($"Sort key must be less than {1023} bytes.");
				}
				offset += keyLength;
				buffer.Write(item.Value, offset);
				offset += 5;
				_remaining++;
			}
			_count = _remaining;
		}

		public void InitializeReader(Stream stream, BufferSlice buffer, bool utcDate)
		{
			if (stream != null)
			{
				_reader = new BufferReader(GetSourceFromStream(stream), utcDate);
			}
			else
			{
				_reader = new BufferReader(buffer, utcDate);
			}
			MoveNext();
		}

		public bool MoveNext()
		{
			if (_remaining == 0)
			{
				_isEOF = true;
				return false;
			}
			BsonValue key = _reader.ReadIndexKey();
			PageAddress value = _reader.ReadPageAddress();
			Current = new KeyValuePair<BsonValue, PageAddress>(key, value);
			_remaining--;
			return true;
		}

		private IEnumerable<BufferSlice> GetSourceFromStream(Stream stream)
		{
			byte[] bytes = BufferPool.Rent(8192);
			BufferSlice buffer = new BufferSlice(bytes, 0, 8192);
			while (_readPosition < _size)
			{
				stream.Position = Position + _readPosition;
				stream.Read(bytes, 0, 8192);
				_readPosition += 8192;
				yield return buffer;
			}
			BufferPool.Return(bytes);
		}

		public void Dispose()
		{
			_reader?.Dispose();
		}
	}
}
