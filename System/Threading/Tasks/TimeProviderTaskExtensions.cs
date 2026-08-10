namespace System.Threading.Tasks
{
	internal static class TimeProviderTaskExtensions
	{
		private sealed class DelayState : TaskCompletionSource<bool>
		{
			public ITimer Timer { get; set; }

			public CancellationToken CancellationToken { get; }

			public CancellationTokenRegistration Registration { get; set; }

			public DelayState(CancellationToken cancellationToken)
			{
				CancellationToken = cancellationToken;
			}
		}

		private sealed class WaitAsyncState : TaskCompletionSource<bool>
		{
			public readonly CancellationTokenSource ContinuationCancellation = new CancellationTokenSource();

			public CancellationTokenRegistration Registration;

			public ITimer Timer;

			public CancellationToken CancellationToken { get; }

			public WaitAsyncState(CancellationToken cancellationToken)
				: base(TaskCreationOptions.RunContinuationsAsynchronously)
			{
				CancellationToken = cancellationToken;
			}
		}

		public static Task Delay(this TimeProvider timeProvider, TimeSpan delay, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (timeProvider == TimeProvider.System)
			{
				return Task.Delay(delay, cancellationToken);
			}
			if (timeProvider == null)
			{
				throw new ArgumentNullException("timeProvider");
			}
			if (delay != Timeout.InfiniteTimeSpan && delay < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("delay");
			}
			if (delay == TimeSpan.Zero)
			{
				return Task.CompletedTask;
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			DelayState delayState2 = new DelayState(cancellationToken);
			delayState2.Timer = timeProvider.CreateTimer(delegate(object delayState)
			{
				DelayState delayState5 = (DelayState)delayState;
				delayState5.TrySetResult(result: true);
				delayState5.Registration.Dispose();
				delayState5.Timer?.Dispose();
			}, delayState2, delay, Timeout.InfiniteTimeSpan);
			delayState2.Registration = cancellationToken.Register(delegate(object delayState)
			{
				DelayState delayState3 = (DelayState)delayState;
				ThreadPool.UnsafeQueueUserWorkItem(delegate(object state)
				{
					DelayState delayState4 = (DelayState)state;
					delayState4.TrySetCanceled(delayState4.CancellationToken);
				}, delayState3);
				delayState3.Registration.Dispose();
				delayState3.Timer?.Dispose();
			}, delayState2);
			if (delayState2.Task.IsCompleted)
			{
				delayState2.Registration.Dispose();
				delayState2.Timer.Dispose();
			}
			return delayState2.Task;
		}

		public static Task WaitAsync(this Task task, TimeSpan timeout, TimeProvider timeProvider, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (task == null)
			{
				throw new ArgumentNullException("task");
			}
			if (timeout != Timeout.InfiniteTimeSpan && timeout < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			if (timeProvider == null)
			{
				throw new ArgumentNullException("timeProvider");
			}
			if (task.IsCompleted)
			{
				return task;
			}
			if (timeout == Timeout.InfiniteTimeSpan && !cancellationToken.CanBeCanceled)
			{
				return task;
			}
			if (timeout == TimeSpan.Zero)
			{
				Task.FromException(new TimeoutException());
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			WaitAsyncState waitAsyncState = new WaitAsyncState(cancellationToken);
			waitAsyncState.Timer = timeProvider.CreateTimer(delegate(object s)
			{
				WaitAsyncState waitAsyncState4 = (WaitAsyncState)s;
				waitAsyncState4.TrySetException(new TimeoutException());
				waitAsyncState4.Registration.Dispose();
				waitAsyncState4.Timer?.Dispose();
				waitAsyncState4.ContinuationCancellation.Cancel();
			}, waitAsyncState, timeout, Timeout.InfiniteTimeSpan);
			task.ContinueWith(delegate(Task t, object s)
			{
				WaitAsyncState waitAsyncState3 = (WaitAsyncState)s;
				if (t.IsFaulted)
				{
					waitAsyncState3.TrySetException(t.Exception.InnerExceptions);
				}
				else if (t.IsCanceled)
				{
					waitAsyncState3.TrySetCanceled();
				}
				else
				{
					waitAsyncState3.TrySetResult(result: true);
				}
				waitAsyncState3.Registration.Dispose();
				waitAsyncState3.Timer?.Dispose();
			}, waitAsyncState, waitAsyncState.ContinuationCancellation.Token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
			waitAsyncState.Registration = cancellationToken.Register(delegate(object s)
			{
				WaitAsyncState waitAsyncState2 = (WaitAsyncState)s;
				waitAsyncState2.TrySetCanceled(waitAsyncState2.CancellationToken);
				waitAsyncState2.Timer?.Dispose();
				waitAsyncState2.ContinuationCancellation.Cancel();
			}, waitAsyncState);
			if (waitAsyncState.Task.IsCompleted)
			{
				waitAsyncState.Registration.Dispose();
				waitAsyncState.Timer.Dispose();
			}
			return waitAsyncState.Task;
		}

		public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout, TimeProvider timeProvider, CancellationToken cancellationToken = default(CancellationToken))
		{
			await ((Task)task).WaitAsync(timeout, timeProvider, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return task.Result;
		}

		public static CancellationTokenSource CreateCancellationTokenSource(this TimeProvider timeProvider, TimeSpan delay)
		{
			if (timeProvider == null)
			{
				throw new ArgumentNullException("timeProvider");
			}
			if (delay != Timeout.InfiniteTimeSpan && delay < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("delay");
			}
			if (timeProvider == TimeProvider.System)
			{
				return new CancellationTokenSource(delay);
			}
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			ITimer state = timeProvider.CreateTimer(delegate(object s)
			{
				try
				{
					((CancellationTokenSource)s).Cancel();
				}
				catch (ObjectDisposedException)
				{
				}
			}, cancellationTokenSource, delay, Timeout.InfiniteTimeSpan);
			cancellationTokenSource.Token.Register(delegate(object t)
			{
				((ITimer)t).Dispose();
			}, state);
			return cancellationTokenSource;
		}
	}
}
