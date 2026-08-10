using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Microsoft.Extensions.Primitives
{
	[DebuggerDisplay("HasChanged = {HasChanged}")]
	internal class CompositeChangeToken : IChangeToken
	{
		private static readonly Action<object> _onChangeDelegate = OnChange;

		private readonly object _callbackLock = new object();

		private CancellationTokenSource _cancellationTokenSource;

		private List<IDisposable> _disposables;

		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "_cancellationTokenSource")]
		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "_disposables")]
		private bool RegisteredCallbackProxy
		{
			[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "_cancellationTokenSource")]
			[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "_disposables")]
			get;
			[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "_cancellationTokenSource")]
			[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "_disposables")]
			set;
		}

		public IReadOnlyList<IChangeToken> ChangeTokens { get; }

		public bool HasChanged
		{
			get
			{
				if (_cancellationTokenSource != null && _cancellationTokenSource.Token.IsCancellationRequested)
				{
					return true;
				}
				for (int i = 0; i < ChangeTokens.Count; i++)
				{
					if (ChangeTokens[i].HasChanged)
					{
						OnChange(this);
						return true;
					}
				}
				return false;
			}
		}

		public bool ActiveChangeCallbacks { get; }

		public CompositeChangeToken(IReadOnlyList<IChangeToken> changeTokens)
		{
			if (changeTokens == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.changeTokens);
			}
			ChangeTokens = changeTokens;
			for (int i = 0; i < ChangeTokens.Count; i++)
			{
				if (ChangeTokens[i].ActiveChangeCallbacks)
				{
					ActiveChangeCallbacks = true;
					break;
				}
			}
		}

		public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
		{
			EnsureCallbacksInitialized();
			return _cancellationTokenSource.Token.Register(callback, state);
		}

		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNull("_cancellationTokenSource")]
		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNull("_disposables")]
		private void EnsureCallbacksInitialized()
		{
			if (RegisteredCallbackProxy)
			{
				return;
			}
			lock (_callbackLock)
			{
				if (RegisteredCallbackProxy)
				{
					return;
				}
				_cancellationTokenSource = new CancellationTokenSource();
				_disposables = new List<IDisposable>();
				for (int i = 0; i < ChangeTokens.Count; i++)
				{
					if (ChangeTokens[i].ActiveChangeCallbacks)
					{
						IDisposable disposable = ChangeTokens[i].RegisterChangeCallback(_onChangeDelegate, this);
						if (_cancellationTokenSource.IsCancellationRequested)
						{
							disposable.Dispose();
							break;
						}
						_disposables.Add(disposable);
					}
				}
				RegisteredCallbackProxy = true;
			}
		}

		private static void OnChange(object state)
		{
			CompositeChangeToken compositeChangeToken = (CompositeChangeToken)state;
			if (compositeChangeToken._cancellationTokenSource == null)
			{
				return;
			}
			lock (compositeChangeToken._callbackLock)
			{
				if (compositeChangeToken._cancellationTokenSource.IsCancellationRequested)
				{
					return;
				}
				try
				{
					compositeChangeToken._cancellationTokenSource.Cancel();
				}
				catch
				{
				}
			}
			List<IDisposable> disposables = compositeChangeToken._disposables;
			for (int i = 0; i < disposables.Count; i++)
			{
				disposables[i].Dispose();
			}
		}
	}
}
