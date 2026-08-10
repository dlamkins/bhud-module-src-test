using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Shared;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Internal
{
	internal sealed class TimerAwaitable : IDisposable, ICriticalNotifyCompletion, INotifyCompletion
	{
		private Timer _timer;

		private Action _callback;

		private static readonly Action _callbackCompleted = delegate
		{
		};

		private readonly TimeSpan _period;

		private readonly TimeSpan _dueTime;

		private readonly object _lockObj = new object();

		private bool _disposed;

		private bool _running = true;

		public bool IsCompleted => (object)_callback == _callbackCompleted;

		public TimerAwaitable(TimeSpan dueTime, TimeSpan period)
		{
			_dueTime = dueTime;
			_period = period;
		}

		public void Start()
		{
			if (_timer != null)
			{
				return;
			}
			lock (_lockObj)
			{
				if (_disposed || _timer != null)
				{
					return;
				}
				_timer = NonCapturingTimer.Create(delegate(object state)
				{
					if (((WeakReference<TimerAwaitable>)state).TryGetTarget(out var target))
					{
						target.Tick();
					}
				}, new WeakReference<TimerAwaitable>(this), _dueTime, _period);
			}
		}

		public TimerAwaitable GetAwaiter()
		{
			return this;
		}

		public bool GetResult()
		{
			_callback = null;
			return _running;
		}

		private void Tick()
		{
			Interlocked.Exchange(ref _callback, _callbackCompleted)?.Invoke();
		}

		public void OnCompleted(Action continuation)
		{
			if ((object)_callback == _callbackCompleted || (object)Interlocked.CompareExchange(ref _callback, continuation, null) == _callbackCompleted)
			{
				Task.Run(continuation);
			}
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			OnCompleted(continuation);
		}

		public void Stop()
		{
			lock (_lockObj)
			{
				_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EObjectDisposedThrowHelper.ThrowIf(_disposed, this);
				_running = false;
			}
			Tick();
		}

		void IDisposable.Dispose()
		{
			lock (_lockObj)
			{
				_disposed = true;
				_timer?.Dispose();
				_timer = null;
			}
		}
	}
}
