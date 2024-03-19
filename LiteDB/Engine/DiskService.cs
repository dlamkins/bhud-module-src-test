using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LiteDB.Engine
{
	internal class DiskService : IDisposable
	{
		private readonly MemoryCache _cache;

		private readonly Lazy<DiskWriterQueue> _queue;

		private IStreamFactory _dataFactory;

		private readonly IStreamFactory _logFactory;

		private StreamPool _dataPool;

		private readonly StreamPool _logPool;

		private long _dataLength;

		private long _logLength;

		public DiskWriterQueue Queue => _queue.Value;

		public MemoryCache Cache => _cache;

		public DiskService(EngineSettings settings, int[] memorySegmentSizes)
		{
			_cache = new MemoryCache(memorySegmentSizes);
			_dataFactory = settings.CreateDataFactory();
			_logFactory = settings.CreateLogFactory();
			_dataPool = new StreamPool(_dataFactory, appendOnly: false);
			_logPool = new StreamPool(_logFactory, appendOnly: true);
			bool num = _dataFactory.GetLength() == 0;
			_queue = new Lazy<DiskWriterQueue>(() => new DiskWriterQueue(_logPool.Writer));
			if (num)
			{
				Initialize(_dataPool.Writer, settings.Collation, settings.InitialSize);
			}
			if (!settings.ReadOnly)
			{
				_ = _dataPool.Writer.CanRead;
			}
			_dataLength = _dataFactory.GetLength() - 8192;
			if (_logFactory.Exists())
			{
				_logLength = _logFactory.GetLength() - 8192;
			}
			else
			{
				_logLength = -8192L;
			}
		}

		public StreamPool GetPool(FileOrigin origin)
		{
			if (origin != FileOrigin.Data)
			{
				return _logPool;
			}
			return _dataPool;
		}

		private void Initialize(Stream stream, Collation collation, long initialSize)
		{
			PageBuffer buffer = new PageBuffer(new byte[8192], 0, 0);
			HeaderPage headerPage = new HeaderPage(buffer, 0u);
			headerPage.Pragmas.Set("COLLATION", (collation ?? Collation.Default).ToString(), validate: false);
			headerPage.UpdateBuffer();
			stream.Write(buffer.Array, buffer.Offset, 8192);
			if (initialSize > 0)
			{
				if (stream is AesStream)
				{
					throw LiteException.InitialSizeCryptoNotSupported();
				}
				if (initialSize % 8192 != 0L)
				{
					throw LiteException.InvalidInitialSize();
				}
				stream.SetLength(initialSize);
			}
			stream.FlushToDisk();
		}

		public DiskReader GetReader()
		{
			return new DiskReader(_cache, _dataPool, _logPool);
		}

		public void DiscardDirtyPages(IEnumerable<PageBuffer> pages)
		{
			foreach (PageBuffer page in pages)
			{
				_cache.DiscardPage(page);
			}
		}

		public void DiscardCleanPages(IEnumerable<PageBuffer> pages)
		{
			foreach (PageBuffer page in pages)
			{
				if (!_cache.TryMoveToReadable(page))
				{
					_cache.DiscardPage(page);
				}
			}
		}

		public PageBuffer NewPage()
		{
			return _cache.NewPage();
		}

		public int WriteAsync(IEnumerable<PageBuffer> pages)
		{
			int count = 0;
			foreach (PageBuffer page in pages)
			{
				Constants.ENSURE(page.ShareCounter == Constants.BUFFER_WRITABLE, "to enqueue page, page must be writable");
				page.Position = Interlocked.Add(ref _logLength, 8192L);
				page.Origin = FileOrigin.Log;
				PageBuffer readable = _cache.MoveToReadable(page);
				_queue.Value.EnqueuePage(readable);
				count++;
			}
			_queue.Value.Run();
			return count;
		}

		public long GetVirtualLength(FileOrigin origin)
		{
			if (origin == FileOrigin.Log)
			{
				return _logLength + 8192;
			}
			return _dataLength + 8192;
		}

		public void ChangePassword(string password, EngineSettings settings)
		{
			if (!(settings.Password == password))
			{
				SetLength(0L, FileOrigin.Data);
				_dataPool.Dispose();
				_dataFactory.Delete();
				settings.Password = password;
				_dataFactory = settings.CreateDataFactory();
				_dataPool = new StreamPool(_dataFactory, appendOnly: false);
				_dataLength = -8192L;
			}
		}

		public IEnumerable<PageBuffer> ReadFull(FileOrigin origin)
		{
			byte[] buffer = new byte[8192];
			StreamPool pool = ((origin == FileOrigin.Log) ? _logPool : _dataPool);
			Stream stream = pool.Rent();
			try
			{
				long length = GetVirtualLength(origin);
				stream.Position = 0L;
				while (stream.Position < length)
				{
					long position = stream.Position;
					int bytesRead = stream.Read(buffer, 0, 8192);
					Constants.ENSURE(bytesRead == 8192, $"ReadFull must read PAGE_SIZE bytes [{bytesRead}]");
					yield return new PageBuffer(buffer, 0, 0)
					{
						Position = position,
						Origin = origin,
						ShareCounter = 0
					};
				}
			}
			finally
			{
				pool.Return(stream);
			}
		}

		public void Write(IEnumerable<PageBuffer> pages, FileOrigin origin)
		{
			Constants.ENSURE(origin == FileOrigin.Data);
			Stream stream = ((origin == FileOrigin.Data) ? _dataPool.Writer : _logPool.Writer);
			foreach (PageBuffer page in pages)
			{
				Constants.ENSURE(page.ShareCounter == 0, "this page can't be shared to use sync operation - do not use cached pages");
				_dataLength = Math.Max(_dataLength, page.Position);
				stream.Position = page.Position;
				stream.Write(page.Array, page.Offset, 8192);
			}
			stream.FlushToDisk();
		}

		public void SetLength(long length, FileOrigin origin)
		{
			Stream obj = ((origin == FileOrigin.Log) ? _logPool.Writer : _dataPool.Writer);
			if (origin == FileOrigin.Log)
			{
				Constants.ENSURE(_queue.Value.Length == 0, "queue must be empty before set new length");
				Interlocked.Exchange(ref _logLength, length - 8192);
			}
			else
			{
				Interlocked.Exchange(ref _dataLength, length - 8192);
			}
			obj.SetLength(length);
		}

		public string GetName(FileOrigin origin)
		{
			if (origin != FileOrigin.Data)
			{
				return _logFactory.Name;
			}
			return _dataFactory.Name;
		}

		public void Dispose()
		{
			if (_queue.IsValueCreated)
			{
				_queue.Value.Dispose();
			}
			bool num = _logFactory.Exists() && _logPool.Writer.Length == 0;
			_dataPool.Dispose();
			_logPool.Dispose();
			if (num)
			{
				_logFactory.Delete();
			}
			_cache.Dispose();
		}
	}
}
