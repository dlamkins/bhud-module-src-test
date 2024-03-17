using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class WaitHandleExtensions
	{
		public static Task<bool> WaitOneAsync(this WaitHandle waitHandle, TimeSpan timeout, CancellationToken cancellationToken)
		{
			if (waitHandle == null)
			{
				throw new ArgumentNullException("waitHandle");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromResult(result: true);
			}
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			RegisteredWaitHandle registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(waitHandle, delegate(object state, bool timedOut)
			{
				tcs.TrySetResult(!timedOut);
			}, null, timeout, executeOnlyOnce: true);
			cancellationToken.Register(delegate
			{
				if (registeredWaitHandle.Unregister(null))
				{
					tcs.SetCanceled();
				}
			});
			return tcs.Task.ContinueWith(delegate(Task<bool> continuationTask)
			{
				registeredWaitHandle.Unregister(null);
				try
				{
					return continuationTask.Result;
				}
				catch
				{
					return false;
				}
			});
		}
	}
}
