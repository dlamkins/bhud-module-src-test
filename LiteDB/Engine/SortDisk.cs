using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace LiteDB.Engine
{
	internal class SortDisk : IDisposable
	{
		private readonly IStreamFactory _factory;

		private readonly StreamPool _pool;

		private readonly ConcurrentBag<long> _freePositions = new ConcurrentBag<long>();

		private long _lastContainerPosition;

		private readonly int _containerSize;

		private readonly EnginePragmas _pragmas;

		public int ContainerSize => _containerSize;

		public SortDisk(IStreamFactory factory, int containerSize, EnginePragmas pragmas)
		{
			Constants.ENSURE(containerSize % 8192 == 0, "size must be PAGE_SIZE multiple");
			_factory = factory;
			_containerSize = containerSize;
			_pragmas = pragmas;
			_lastContainerPosition = -containerSize;
			_pool = new StreamPool(_factory, appendOnly: false);
		}

		public Stream GetReader()
		{
			return _pool.Rent();
		}

		public void Return(Stream stream)
		{
			_pool.Return(stream);
		}

		public void Return(long position)
		{
			_freePositions.Add(position);
		}

		public long GetContainerPosition()
		{
			if (_freePositions.TryTake(out var position))
			{
				return position;
			}
			return Interlocked.Add(ref _lastContainerPosition, _containerSize);
		}

		public void Write(long position, BufferSlice buffer)
		{
			Stream writer = _pool.Writer;
			lock (writer)
			{
				for (int i = 0; i < _containerSize / 8192; i++)
				{
					writer.Position = position + i * 8192;
					writer.Write(buffer.Array, buffer.Offset + i * 8192, 8192);
				}
			}
		}

		public void Dispose()
		{
			_pool.Dispose();
			_factory.Delete();
		}
	}
}
