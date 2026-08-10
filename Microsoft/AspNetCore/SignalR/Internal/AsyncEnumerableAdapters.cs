using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal
{
	internal static class AsyncEnumerableAdapters
	{
		private sealed class ChannelAsyncEnumerator<T> : IAsyncEnumerator<object>, IAsyncDisposable
		{
			private readonly ChannelReader<T> _channel;

			private readonly CancellationToken _cancellationToken;

			public object Current { get; private set; }

			public ChannelAsyncEnumerator(ChannelReader<T> channel, CancellationToken cancellationToken)
			{
				_channel = channel;
				_cancellationToken = cancellationToken;
			}

			public ValueTask<bool> MoveNextAsync()
			{
				if (_channel.TryRead(out var item))
				{
					Current = item;
					return new ValueTask<bool>(result: true);
				}
				return new ValueTask<bool>(MoveNextAsyncAwaited());
			}

			private async Task<bool> MoveNextAsyncAwaited()
			{
				if (await _channel.WaitToReadAsync(_cancellationToken).ConfigureAwait(continueOnCapturedContext: false) && _channel.TryRead(out var item))
				{
					Current = item;
					return true;
				}
				return false;
			}

			public ValueTask DisposeAsync()
			{
				return default(ValueTask);
			}
		}

		private sealed class CancelableTypedAsyncEnumerable<TResult> : IAsyncEnumerable<TResult>
		{
			private sealed class CancelableEnumerator<T> : IAsyncEnumerator<T>, IAsyncDisposable
			{
				private readonly IAsyncEnumerator<T> _asyncEnumerator;

				private readonly CancellationTokenRegistration _cancellationTokenRegistration;

				public T Current => _asyncEnumerator.Current;

				public CancelableEnumerator(IAsyncEnumerator<T> asyncEnumerator, CancellationTokenRegistration registration)
				{
					_asyncEnumerator = asyncEnumerator;
					_cancellationTokenRegistration = registration;
				}

				public ValueTask<bool> MoveNextAsync()
				{
					return _asyncEnumerator.MoveNextAsync();
				}

				public ValueTask DisposeAsync()
				{
					_cancellationTokenRegistration.Dispose();
					return _asyncEnumerator.DisposeAsync();
				}
			}

			private readonly IAsyncEnumerable<TResult> _asyncEnumerable;

			private readonly CancellationTokenSource _cts;

			public CancelableTypedAsyncEnumerable(IAsyncEnumerable<TResult> asyncEnumerable, CancellationTokenSource cts)
			{
				_asyncEnumerable = asyncEnumerable;
				_cts = cts;
			}

			public IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
			{
				IAsyncEnumerator<TResult> asyncEnumerator = _asyncEnumerable.GetAsyncEnumerator(_cts.Token);
				if (cancellationToken.CanBeCanceled)
				{
					CancellationTokenRegistration registration = cancellationToken.Register(delegate(object ctsState)
					{
						((CancellationTokenSource)ctsState).Cancel();
					}, _cts);
					return new CancelableEnumerator<TResult>(asyncEnumerator, registration);
				}
				return asyncEnumerator;
			}
		}

		private sealed class BoxedAsyncEnumerator<T> : IAsyncEnumerator<object>, IAsyncDisposable
		{
			private readonly IAsyncEnumerator<T> _asyncEnumerator;

			public object Current => _asyncEnumerator.Current;

			public BoxedAsyncEnumerator(IAsyncEnumerator<T> asyncEnumerator)
			{
				_asyncEnumerator = asyncEnumerator;
			}

			public ValueTask<bool> MoveNextAsync()
			{
				return _asyncEnumerator.MoveNextAsync();
			}

			public ValueTask DisposeAsync()
			{
				return _asyncEnumerator.DisposeAsync();
			}
		}

		public static IAsyncEnumerator<object?> MakeCancelableAsyncEnumerator<T>(IAsyncEnumerable<T> asyncEnumerable, CancellationToken cancellationToken = default(CancellationToken))
		{
			IAsyncEnumerator<T> asyncEnumerator = asyncEnumerable.GetAsyncEnumerator(cancellationToken);
			return (asyncEnumerator as IAsyncEnumerator<object>) ?? new BoxedAsyncEnumerator<T>(asyncEnumerator);
		}

		public static IAsyncEnumerable<T> MakeCancelableTypedAsyncEnumerable<T>(IAsyncEnumerable<T> asyncEnumerable, CancellationTokenSource cts)
		{
			return new CancelableTypedAsyncEnumerable<T>(asyncEnumerable, cts);
		}

		public static IAsyncEnumerator<object?> MakeAsyncEnumeratorFromChannel<T>(ChannelReader<T> channel, CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ChannelAsyncEnumerator<T>(channel, cancellationToken);
		}
	}
}
