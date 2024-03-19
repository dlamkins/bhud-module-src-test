using System;
using System.IO;

namespace LiteDB.Engine
{
	internal class DiskReader : IDisposable
	{
		private readonly MemoryCache _cache;

		private readonly StreamPool _dataPool;

		private readonly StreamPool _logPool;

		private readonly Lazy<Stream> _dataStream;

		private readonly Lazy<Stream> _logStream;

		public DiskReader(MemoryCache cache, StreamPool dataPool, StreamPool logPool)
		{
			_cache = cache;
			_dataPool = dataPool;
			_logPool = logPool;
			_dataStream = new Lazy<Stream>(() => _dataPool.Rent());
			_logStream = new Lazy<Stream>(() => _logPool.Rent());
		}

		public PageBuffer ReadPage(long position, bool writable, FileOrigin origin)
		{
			Constants.ENSURE(position % 8192 == 0, "invalid page position");
			Stream stream = ((origin == FileOrigin.Data) ? _dataStream.Value : _logStream.Value);
			if (!writable)
			{
				return _cache.GetReadablePage(position, origin, delegate(long pos, BufferSlice buf)
				{
					ReadStream(stream, pos, buf);
				});
			}
			return _cache.GetWritablePage(position, origin, delegate(long pos, BufferSlice buf)
			{
				ReadStream(stream, pos, buf);
			});
		}

		private void ReadStream(Stream stream, long position, BufferSlice buffer)
		{
			stream.Position = position;
			stream.Read(buffer.Array, buffer.Offset, buffer.Count);
		}

		public PageBuffer NewPage()
		{
			return _cache.NewPage();
		}

		public void Dispose()
		{
			if (_dataStream.IsValueCreated)
			{
				_dataPool.Return(_dataStream.Value);
			}
			if (_logStream.IsValueCreated)
			{
				_logPool.Return(_logStream.Value);
			}
		}
	}
}
