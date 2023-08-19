using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estreya.BlishHUD.Shared.Utils
{
	public class AsyncLock
	{
		private class Releaser : IDisposable
		{
			private readonly SemaphoreSlim _semaphore;

			public Releaser(SemaphoreSlim semaphore)
			{
				_semaphore = semaphore;
			}

			public void Dispose()
			{
				_semaphore.Release();
			}
		}

		public class LockBusyException : Exception
		{
			public LockBusyException()
				: this(null)
			{
			}

			public LockBusyException(string message)
				: base(message ?? "The lock is currently busy and can't be entered.")
			{
			}
		}

		private readonly IDisposable _releaser;

		private readonly Task<IDisposable> _releaserTask;

		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

		public AsyncLock()
		{
			_releaser = new Releaser(_semaphore);
			_releaserTask = Task.FromResult(_releaser);
		}

		public IDisposable Lock()
		{
			_semaphore.Wait();
			return _releaser;
		}

		public bool IsFree()
		{
			return _semaphore.CurrentCount > 0;
		}

		public void ThrowIfBusy(string message = null)
		{
			if (!IsFree())
			{
				throw new LockBusyException(message);
			}
		}

		public Task<IDisposable> LockAsync()
		{
			Task waitTask = _semaphore.WaitAsync();
			if (!waitTask.IsCompleted)
			{
				return waitTask.ContinueWith((Task _, object releaser) => (IDisposable)releaser, _releaser, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
			}
			return _releaserTask;
		}
	}
}
