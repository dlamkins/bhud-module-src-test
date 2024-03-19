using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LiteDB.Engine
{
	internal class DiskWriterQueue : IDisposable
	{
		private readonly Stream _stream;

		private Task _task;

		private readonly ConcurrentQueue<PageBuffer> _queue = new ConcurrentQueue<PageBuffer>();

		private int _running;

		public int Length => _queue.Count;

		public DiskWriterQueue(Stream stream)
		{
			_stream = stream;
		}

		public void EnqueuePage(PageBuffer page)
		{
			Constants.ENSURE(page.Origin == FileOrigin.Log, "async writer must use only for Log file");
			_queue.Enqueue(page);
		}

		public void Run()
		{
			lock (_queue)
			{
				if (_queue.Count != 0 && Interlocked.CompareExchange(ref _running, 1, 0) == 0)
				{
					_task = Task.Run((Action)ExecuteQueue);
				}
			}
		}

		public void Wait()
		{
			lock (_queue)
			{
				if (_task != null)
				{
					_task.Wait();
				}
				Run();
			}
			Constants.ENSURE(_queue.Count == 0, "queue should be empty after wait() call");
		}

		private void ExecuteQueue()
		{
			while (true)
			{
				if (_queue.TryDequeue(out var page))
				{
					WritePageToStream(page);
				}
				while (page == null)
				{
					_stream.FlushToDisk();
					Volatile.Write(ref _running, 0);
					if (!_queue.Any() || Interlocked.CompareExchange(ref _running, 1, 0) == 1)
					{
						return;
					}
					_queue.TryDequeue(out page);
					WritePageToStream(page);
				}
			}
		}

		private void WritePageToStream(PageBuffer page)
		{
			if (page != null)
			{
				Constants.ENSURE(page.ShareCounter > 0, "page must be shared at least 1");
				_stream.Position = page.Position;
				_stream.Write(page.Array, page.Offset, 8192);
				page.Release();
			}
		}

		public void Dispose()
		{
			Wait();
		}
	}
}
