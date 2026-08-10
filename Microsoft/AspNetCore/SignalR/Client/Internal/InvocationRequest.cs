using System;
using System.CodeDom.Compiler;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.SignalR.Client.Internal
{
	internal abstract class InvocationRequest : IDisposable
	{
		private sealed class Streaming : InvocationRequest
		{
			private readonly Channel<object> _channel = Channel.CreateUnbounded<object>();

			public ChannelReader<object> Result => _channel.Reader;

			public Streaming(CancellationToken cancellationToken, Type resultType, string invocationId, ILoggerFactory loggerFactory, HubConnection hubConnection)
				: base(cancellationToken, resultType, invocationId, loggerFactory.CreateLogger<Streaming>(), hubConnection)
			{
			}

			public override void Complete(CompletionMessage completionMessage)
			{
				Log.InvocationCompleted(base.Logger, base.InvocationId);
				if (completionMessage.Result != null)
				{
					Log.ReceivedUnexpectedComplete(base.Logger, base.InvocationId);
					_channel.Writer.TryComplete(new InvalidOperationException("Server provided a result in a completion response to a streamed invocation."));
				}
				if (!string.IsNullOrEmpty(completionMessage.Error))
				{
					Fail(new HubException(completionMessage.Error));
				}
				else
				{
					_channel.Writer.TryComplete();
				}
			}

			public override void Fail(Exception exception)
			{
				Log.InvocationFailed(base.Logger, base.InvocationId);
				_channel.Writer.TryComplete(exception);
			}

			public override async ValueTask<bool> StreamItem(object item)
			{
				try
				{
					while (!_channel.Writer.TryWrite(item))
					{
						if (!(await _channel.Writer.WaitToWriteAsync().ConfigureAwait(continueOnCapturedContext: false)))
						{
							return false;
						}
					}
				}
				catch (Exception exception)
				{
					Log.ErrorWritingStreamItem(base.Logger, base.InvocationId, exception);
				}
				return true;
			}

			protected override void Cancel()
			{
				_channel.Writer.TryComplete(new OperationCanceledException());
			}
		}

		private sealed class NonStreaming : InvocationRequest
		{
			private readonly TaskCompletionSource<object> _completionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

			public Task<object> Result => _completionSource.Task;

			public NonStreaming(CancellationToken cancellationToken, Type resultType, string invocationId, ILoggerFactory loggerFactory, HubConnection hubConnection)
				: base(cancellationToken, resultType, invocationId, loggerFactory.CreateLogger<NonStreaming>(), hubConnection)
			{
			}

			public override void Complete(CompletionMessage completionMessage)
			{
				if (!string.IsNullOrEmpty(completionMessage.Error))
				{
					Fail(new HubException(completionMessage.Error));
					return;
				}
				Log.InvocationCompleted(base.Logger, base.InvocationId);
				_completionSource.TrySetResult(completionMessage.Result);
			}

			public override void Fail(Exception exception)
			{
				Log.InvocationFailed(base.Logger, base.InvocationId);
				_completionSource.TrySetException(exception);
			}

			public override ValueTask<bool> StreamItem(object item)
			{
				Log.StreamItemOnNonStreamInvocation(base.Logger, base.InvocationId);
				_completionSource.TrySetException(new InvalidOperationException("Streaming hub methods must be invoked with the 'HubConnection.StreamAsChannelAsync' method."));
				return new ValueTask<bool>(result: true);
			}

			protected override void Cancel()
			{
				_completionSource.TrySetCanceled();
			}
		}

		private static class Log
		{
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __InvocationCreatedCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(1, "InvocationCreated"), "Invocation {InvocationId} created.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __InvocationDisposedCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(2, "InvocationDisposed"), "Invocation {InvocationId} disposed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __InvocationCompletedCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(3, "InvocationCompleted"), "Invocation {InvocationId} marked as completed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __InvocationFailedCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(4, "InvocationFailed"), "Invocation {InvocationId} marked as failed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ErrorWritingStreamItemCallback = LoggerMessage.Define<string>(LogLevel.Error, new EventId(5, "ErrorWritingStreamItem"), "Invocation {InvocationId} caused an error trying to write a stream item.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ReceivedUnexpectedCompleteCallback = LoggerMessage.Define<string>(LogLevel.Error, new EventId(6, "ReceivedUnexpectedComplete"), "Invocation {InvocationId} received a completion result, but was invoked as a streaming invocation.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __StreamItemOnNonStreamInvocationCallback = LoggerMessage.Define<string>(LogLevel.Error, new EventId(7, "StreamItemOnNonStreamInvocation"), "Invocation {InvocationId} received stream item but was invoked as a non-streamed invocation.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[LoggerMessage(1, LogLevel.Trace, "Invocation {InvocationId} created.", EventName = "InvocationCreated")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void InvocationCreated(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__InvocationCreatedCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(2, LogLevel.Trace, "Invocation {InvocationId} disposed.", EventName = "InvocationDisposed")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void InvocationDisposed(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__InvocationDisposedCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(3, LogLevel.Trace, "Invocation {InvocationId} marked as completed.", EventName = "InvocationCompleted")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void InvocationCompleted(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__InvocationCompletedCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(4, LogLevel.Trace, "Invocation {InvocationId} marked as failed.", EventName = "InvocationFailed")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void InvocationFailed(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__InvocationFailedCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(5, LogLevel.Error, "Invocation {InvocationId} caused an error trying to write a stream item.", EventName = "ErrorWritingStreamItem")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorWritingStreamItem(ILogger logger, string invocationId, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorWritingStreamItemCallback(logger, invocationId, exception);
				}
			}

			[LoggerMessage(6, LogLevel.Error, "Invocation {InvocationId} received a completion result, but was invoked as a streaming invocation.", EventName = "ReceivedUnexpectedComplete")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedUnexpectedComplete(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ReceivedUnexpectedCompleteCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(7, LogLevel.Error, "Invocation {InvocationId} received stream item but was invoked as a non-streamed invocation.", EventName = "StreamItemOnNonStreamInvocation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void StreamItemOnNonStreamInvocation(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__StreamItemOnNonStreamInvocationCallback(logger, invocationId, null);
				}
			}
		}

		private readonly CancellationTokenRegistration _cancellationTokenRegistration;

		protected ILogger Logger { get; }

		public Type ResultType { get; }

		public CancellationToken CancellationToken { get; }

		public string InvocationId { get; }

		public HubConnection HubConnection { get; private set; }

		protected InvocationRequest(CancellationToken cancellationToken, Type resultType, string invocationId, ILogger logger, HubConnection hubConnection)
		{
			_cancellationTokenRegistration = cancellationToken.Register(delegate(object self)
			{
				((InvocationRequest)self).Cancel();
			}, this);
			InvocationId = invocationId;
			CancellationToken = cancellationToken;
			ResultType = resultType;
			Logger = logger;
			HubConnection = hubConnection;
			Log.InvocationCreated(Logger, InvocationId);
		}

		public static InvocationRequest Invoke(CancellationToken cancellationToken, Type resultType, string invocationId, ILoggerFactory loggerFactory, HubConnection hubConnection, out Task<object?> result)
		{
			NonStreaming nonStreaming = new NonStreaming(cancellationToken, resultType, invocationId, loggerFactory, hubConnection);
			result = nonStreaming.Result;
			return nonStreaming;
		}

		public static InvocationRequest Stream(CancellationToken cancellationToken, Type resultType, string invocationId, ILoggerFactory loggerFactory, HubConnection hubConnection, out ChannelReader<object?> result)
		{
			Streaming streaming = new Streaming(cancellationToken, resultType, invocationId, loggerFactory, hubConnection);
			result = streaming.Result;
			return streaming;
		}

		public abstract void Fail(Exception exception);

		public abstract void Complete(CompletionMessage message);

		public abstract ValueTask<bool> StreamItem(object? item);

		protected abstract void Cancel();

		public virtual void Dispose()
		{
			Log.InvocationDisposed(Logger, InvocationId);
			Cancel();
			_cancellationTokenRegistration.Dispose();
		}
	}
}
