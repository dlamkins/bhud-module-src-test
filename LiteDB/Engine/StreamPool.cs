using System;
using System.Collections.Concurrent;
using System.IO;

namespace LiteDB.Engine
{
	internal class StreamPool : IDisposable
	{
		private readonly ConcurrentBag<Stream> _pool = new ConcurrentBag<Stream>();

		private readonly Lazy<Stream> _writer;

		private readonly IStreamFactory _factory;

		public Stream Writer => _writer.Value;

		public StreamPool(IStreamFactory factory, bool appendOnly)
		{
			StreamPool streamPool = this;
			_factory = factory;
			_writer = new Lazy<Stream>(() => streamPool._factory.GetStream(canWrite: true, appendOnly), isThreadSafe: true);
		}

		public Stream Rent()
		{
			if (!_pool.TryTake(out var stream))
			{
				return _factory.GetStream(canWrite: false, sequencial: false);
			}
			return stream;
		}

		public void Return(Stream stream)
		{
			_pool.Add(stream);
		}

		public void Dispose()
		{
			if (!_factory.CloseOnDispose)
			{
				return;
			}
			foreach (Stream item in _pool)
			{
				item.Dispose();
			}
			if (_writer.IsValueCreated)
			{
				_writer.Value.Dispose();
			}
		}
	}
}
