using System;
using System.Buffers;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Connections.Abstractions;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Shared;
using Microsoft.AspNetCore.SignalR.Client.Internal;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.SignalR.Client
{
	internal class HubConnection : IAsyncDisposable
	{
		private sealed class Subscription : IDisposable
		{
			private readonly InvocationHandler _handler;

			private readonly InvocationHandlerList _handlerList;

			public Subscription(InvocationHandler handler, InvocationHandlerList handlerList)
			{
				_handler = handler;
				_handlerList = handlerList;
			}

			public void Dispose()
			{
				_handlerList.Remove(_handler);
			}
		}

		private sealed class InvocationHandlerList
		{
			private readonly List<InvocationHandler> _invocationHandlers;

			private InvocationHandler[] _copiedHandlers;

			internal InvocationHandlerList(InvocationHandler handler)
			{
				_invocationHandlers = new List<InvocationHandler> { handler };
			}

			internal InvocationHandler[] GetHandlers()
			{
				InvocationHandler[] copiedHandlers = _copiedHandlers;
				if (copiedHandlers == null)
				{
					lock (_invocationHandlers)
					{
						if (_copiedHandlers == null)
						{
							_copiedHandlers = _invocationHandlers.ToArray();
						}
						return _copiedHandlers;
					}
				}
				return copiedHandlers;
			}

			internal void Add(string methodName, InvocationHandler handler)
			{
				lock (_invocationHandlers)
				{
					if (handler.HasResult)
					{
						foreach (InvocationHandler invocationHandler in _invocationHandlers)
						{
							if (invocationHandler.HasResult)
							{
								throw new InvalidOperationException("'" + methodName + "' already has a value returning handler. Multiple return values are not supported.");
							}
						}
					}
					_invocationHandlers.Add(handler);
					_copiedHandlers = null;
				}
			}

			internal void Remove(InvocationHandler handler)
			{
				lock (_invocationHandlers)
				{
					if (_invocationHandlers.Remove(handler))
					{
						_copiedHandlers = null;
					}
				}
			}
		}

		private readonly struct InvocationHandler
		{
			private readonly Func<object[], object, Task> _callback;

			private readonly object _state;

			public Type[] ParameterTypes { get; }

			public bool HasResult => _callback.Method.ReturnType == typeof(Task<object>);

			public InvocationHandler(Type[] parameterTypes, Func<object[], object, Task> callback, object state)
			{
				_callback = callback;
				ParameterTypes = parameterTypes;
				_state = state;
			}

			public Task InvokeAsync(object[] parameters)
			{
				return _callback(parameters, _state);
			}
		}

		private sealed class ConnectionState : IInvocationBinder
		{
			private readonly HubConnection _hubConnection;

			private readonly ILogger _logger;

			private readonly bool _hasInherentKeepAlive;

			private readonly MessageBuffer _messageBuffer;

			private readonly object _lock = new object();

			private readonly Dictionary<string, InvocationRequest> _pendingCalls = new Dictionary<string, InvocationRequest>(StringComparer.Ordinal);

			private TaskCompletionSource<object> _stopTcs;

			private volatile bool _stopping;

			private int _nextInvocationId;

			private long _nextActivationServerTimeout;

			private long _nextActivationSendPing;

			public ConnectionContext Connection { get; }

			public Task ReceiveTask { get; set; }

			public Exception CloseException { get; set; }

			public CancellationToken UploadStreamToken { get; set; }

			public Task InvocationMessageReceiveTask { get; set; }

			public bool Stopping
			{
				get
				{
					return _stopping;
				}
				set
				{
					_stopping = value;
				}
			}

			public ConnectionState(ConnectionContext connection, HubConnection hubConnection)
			{
				Connection = connection;
				_hubConnection = hubConnection;
				_hubConnection._logScope.ConnectionId = connection.ConnectionId;
				_logger = _hubConnection._logger;
				_hasInherentKeepAlive = connection.Features.Get<IConnectionInherentKeepAliveFeature>()?.HasInherentKeepAlive ?? false;
				IStatefulReconnectFeature statefulReconnectFeature = Connection.Features.Get<IStatefulReconnectFeature>();
				if (statefulReconnectFeature != null)
				{
					_messageBuffer = new MessageBuffer(connection, hubConnection._protocol, _hubConnection._serviceProvider.GetService<IOptions<HubConnectionOptions>>()?.Value.StatefulReconnectBufferSize ?? 100000, _logger);
					statefulReconnectFeature.OnReconnected(new Func<PipeWriter, Task>(_messageBuffer.ResendAsync));
				}
			}

			public string GetNextId()
			{
				return (++_nextInvocationId).ToString(CultureInfo.InvariantCulture);
			}

			public void AddInvocation(InvocationRequest irq)
			{
				lock (_lock)
				{
					if (_pendingCalls.ContainsKey(irq.InvocationId))
					{
						Log.InvocationAlreadyInUse(_logger, irq.InvocationId);
						throw new InvalidOperationException("Invocation ID '" + irq.InvocationId + "' is already in use.");
					}
					_pendingCalls.Add(irq.InvocationId, irq);
				}
			}

			public bool TryGetInvocation(string invocationId, [_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003ENotNullWhen(true)] out InvocationRequest irq)
			{
				lock (_lock)
				{
					return _pendingCalls.TryGetValue(invocationId, out irq);
				}
			}

			public bool TryRemoveInvocation(string invocationId, [_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003ENotNullWhen(true)] out InvocationRequest irq)
			{
				lock (_lock)
				{
					if (_pendingCalls.TryGetValue(invocationId, out irq))
					{
						_pendingCalls.Remove(invocationId);
						return true;
					}
					return false;
				}
			}

			public void CancelOutstandingInvocations(Exception exception)
			{
				Log.CancelingOutstandingInvocations(_logger);
				lock (_lock)
				{
					foreach (InvocationRequest value in _pendingCalls.Values)
					{
						Log.RemovingInvocation(_logger, value.InvocationId);
						if (exception != null)
						{
							value.Fail(exception);
						}
						value.Dispose();
					}
					_pendingCalls.Clear();
				}
			}

			public Task StopAsync()
			{
				lock (_lock)
				{
					if (_stopTcs != null)
					{
						return _stopTcs.Task;
					}
					_stopTcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
					return StopAsyncCore();
				}
			}

			private async Task StopAsyncCore()
			{
				Log.Stopping(_logger);
				Log.TerminatingReceiveLoop(_logger);
				Connection.Transport.Input.CancelPendingRead();
				Log.WaitingForReceiveLoopToTerminate(_logger);
				await (ReceiveTask ?? Task.CompletedTask).ConfigureAwait(continueOnCapturedContext: false);
				Log.Stopped(_logger);
				_hubConnection._logScope.ConnectionId = null;
				_stopTcs.TrySetResult(null);
			}

			public void Cleanup()
			{
				_messageBuffer?.Dispose();
			}

			public async Task TimerLoop(TimerAwaitable timer)
			{
				timer.Start();
				ResetTimeout();
				ResetSendPing();
				using (timer)
				{
					while (await timer)
					{
						await RunTimerActions().ConfigureAwait(continueOnCapturedContext: false);
					}
				}
			}

			public ValueTask<FlushResult> WriteAsync(HubMessage message, CancellationToken cancellationToken)
			{
				return _messageBuffer.WriteAsync(message, cancellationToken);
			}

			public bool ShouldProcessMessage(HubMessage message)
			{
				if (UsingAcks() && !_messageBuffer.ShouldProcessMessage(message))
				{
					Log.DroppingMessage(_logger, ((HubInvocationMessage)message).GetType().Name, ((HubInvocationMessage)message).InvocationId);
					return false;
				}
				return true;
			}

			public Task AckAsync(AckMessage ackMessage)
			{
				if (UsingAcks())
				{
					return _messageBuffer.AckAsync(ackMessage);
				}
				return Task.CompletedTask;
			}

			[_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EMemberNotNullWhen(true, "_messageBuffer")]
			public bool UsingAcks()
			{
				return _messageBuffer != null;
			}

			public void ResetSendPing()
			{
				Volatile.Write(ref _nextActivationSendPing, (DateTime.UtcNow + _hubConnection.KeepAliveInterval).Ticks);
			}

			public void ResetTimeout()
			{
				Volatile.Write(ref _nextActivationServerTimeout, (DateTime.UtcNow + _hubConnection.ServerTimeout).Ticks);
			}

			internal async Task RunTimerActions()
			{
				if (_hasInherentKeepAlive)
				{
					return;
				}
				if (DateTime.UtcNow.Ticks > Volatile.Read(ref _nextActivationServerTimeout))
				{
					OnServerTimeout();
				}
				if (DateTime.UtcNow.Ticks <= Volatile.Read(ref _nextActivationSendPing) || Stopping)
				{
					return;
				}
				if (!_hubConnection._state.TryAcquireConnectionLock())
				{
					Log.UnableToAcquireConnectionLockForPing(_logger);
					return;
				}
				Log.AcquiredConnectionLockForPing(_logger);
				try
				{
					if (_hubConnection._state.CurrentConnectionStateUnsynchronized != null)
					{
						await _hubConnection.SendHubMessage(this, PingMessage.Instance).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				catch
				{
				}
				finally
				{
					_hubConnection._state.ReleaseConnectionLock("RunTimerActions", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 2157);
				}
			}

			internal void OnServerTimeout()
			{
				CloseException = new TimeoutException($"Server timeout ({_hubConnection.ServerTimeout.TotalMilliseconds:0.00}ms) elapsed without receiving a message from the server.");
				Connection.Transport.Input.CancelPendingRead();
			}

			Type IInvocationBinder.GetReturnType(string invocationId)
			{
				if (!TryGetInvocation(invocationId, out var irq))
				{
					Log.ReceivedUnexpectedResponse(_logger, invocationId);
					throw new KeyNotFoundException("No invocation with id '" + invocationId + "' could be found.");
				}
				return irq.ResultType;
			}

			Type IInvocationBinder.GetStreamItemType(string invocationId)
			{
				if (!TryGetInvocation(invocationId, out var irq))
				{
					Log.ReceivedUnexpectedResponse(_logger, invocationId);
					throw new KeyNotFoundException("No invocation with id '" + invocationId + "' could be found.");
				}
				return irq.ResultType;
			}

			IReadOnlyList<Type> IInvocationBinder.GetParameterTypes(string methodName)
			{
				if (!_hubConnection._handlers.TryGetValue(methodName, out var value))
				{
					Log.MissingHandler(_logger, methodName);
					return Type.EmptyTypes;
				}
				InvocationHandler[] handlers = value.GetHandlers();
				if (handlers.Length != 0)
				{
					return handlers[0].ParameterTypes;
				}
				throw new InvalidOperationException("There are no callbacks registered for the method '" + methodName + "'");
			}
		}

		private sealed class ReconnectingConnectionState
		{
			private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

			private readonly ILogger _logger;

			public ConnectionState CurrentConnectionStateUnsynchronized { get; set; }

			public HubConnectionState OverallState { get; private set; }

			public CancellationTokenSource StopCts { get; set; } = new CancellationTokenSource();


			public Task ReconnectTask { get; set; } = Task.CompletedTask;


			public ReconnectingConnectionState(ILogger logger)
			{
				_logger = logger;
				StopCts = new CancellationTokenSource();
				ReconnectTask = Task.CompletedTask;
			}

			public void ChangeState(HubConnectionState expectedState, HubConnectionState newState)
			{
				if (!TryChangeState(expectedState, newState))
				{
					Log.StateTransitionFailed(_logger, expectedState, newState, OverallState);
					throw new InvalidOperationException($"The HubConnection failed to transition from the '{expectedState}' state to the '{newState}' state because it was actually in the '{OverallState}' state.");
				}
			}

			public bool TryChangeState(HubConnectionState expectedState, HubConnectionState newState)
			{
				Log.AttemptingStateTransition(_logger, expectedState, newState);
				if (OverallState != expectedState)
				{
					return false;
				}
				OverallState = newState;
				return true;
			}

			[Conditional("DEBUG")]
			public void AssertInConnectionLock([CallerMemberName] string memberName = null, [CallerFilePath] string fileName = null, [CallerLineNumber] int lineNumber = 0)
			{
			}

			[Conditional("DEBUG")]
			public void AssertConnectionValid([CallerMemberName] string memberName = null, [CallerFilePath] string fileName = null, [CallerLineNumber] int lineNumber = 0)
			{
			}

			public Task WaitConnectionLockAsync(CancellationToken token, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
			{
				Log.WaitingOnConnectionLock(_logger, memberName, filePath, lineNumber);
				return _connectionLock.WaitAsync(token);
			}

			public bool TryAcquireConnectionLock()
			{
				if (_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EOperatingSystem.IsBrowser())
				{
					return _connectionLock.WaitAsync(0).Result;
				}
				return _connectionLock.Wait(0);
			}

			public async Task<ConnectionState> WaitForActiveConnectionAsync(string methodName, CancellationToken token)
			{
				await WaitConnectionLockAsync(token, methodName, "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 2284).ConfigureAwait(continueOnCapturedContext: false);
				if (!IsConnectionActive())
				{
					ReleaseConnectionLock(methodName, "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 2288);
					throw new InvalidOperationException("The '" + methodName + "' method cannot be called if the connection is not active");
				}
				return CurrentConnectionStateUnsynchronized;
			}

			[_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EMemberNotNullWhen(true, "CurrentConnectionStateUnsynchronized")]
			public bool IsConnectionActive()
			{
				if (CurrentConnectionStateUnsynchronized != null)
				{
					return !CurrentConnectionStateUnsynchronized.Stopping;
				}
				return false;
			}

			public void ReleaseConnectionLock([CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
			{
				Log.ReleasingConnectionLock(_logger, memberName, filePath, lineNumber);
				_connectionLock.Release();
			}
		}

		private static class Log
		{
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, int, Exception> __PreparingNonBlockingInvocationCallback = LoggerMessage.Define<string, int>(LogLevel.Trace, new EventId(1, "PreparingNonBlockingInvocation"), "Preparing non-blocking invocation of '{Target}', with {ArgumentCount} argument(s).", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, string, int, Exception> __PreparingBlockingInvocationCallback = LoggerMessage.Define<string, string, string, int>(LogLevel.Trace, new EventId(2, "PreparingBlockingInvocation"), "Preparing blocking invocation '{InvocationId}' of '{Target}', with return type '{ReturnType}' and {ArgumentCount} argument(s).", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __RegisteringInvocationCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(3, "RegisteringInvocation"), "Registering Invocation ID '{InvocationId}' for tracking.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, string, string, Exception> __IssuingInvocationCallback = LoggerMessage.Define<string, string, string, string>(LogLevel.Trace, new EventId(4, "IssuingInvocation"), "Issuing Invocation '{InvocationId}': {ReturnType} {MethodName}({Args}).", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, Exception> __SendingMessageCallback = LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(5, "SendingMessage"), "Sending {MessageType} message '{InvocationId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __SendingMessageGenericCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(59, "SendingMessageGeneric"), "Sending {MessageType} message.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, Exception> __MessageSentCallback = LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(6, "MessageSent"), "Sending {MessageType} message '{InvocationId}' completed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __MessageSentGenericCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(60, "MessageSentGeneric"), "Sending {MessageType} message completed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __FailedToSendInvocationCallback = LoggerMessage.Define<string>(LogLevel.Error, new EventId(7, "FailedToSendInvocation"), "Sending Invocation '{InvocationId}' failed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, string, Exception> __ReceivedInvocationCallback = LoggerMessage.Define<string, string, string>(LogLevel.Trace, new EventId(8, "ReceivedInvocation"), "Received Invocation '{InvocationId}': {MethodName}({Args}).", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __DroppedCompletionMessageCallback = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(9, "DroppedCompletionMessage"), "Dropped unsolicited Completion message for invocation '{InvocationId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __DroppedStreamMessageCallback = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(10, "DroppedStreamMessage"), "Dropped unsolicited StreamItem message for invocation '{InvocationId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ShutdownConnectionCallback = LoggerMessage.Define(LogLevel.Trace, new EventId(11, "ShutdownConnection"), "Shutting down connection.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ShutdownWithErrorCallback = LoggerMessage.Define(LogLevel.Error, new EventId(12, "ShutdownWithError"), "Connection is shutting down due to an error.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __RemovingInvocationCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(13, "RemovingInvocation"), "Removing pending invocation {InvocationId}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __MissingHandlerCallback = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(14, "MissingHandler"), "Failed to find handler for '{Target}' method.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ReceivedStreamItemCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(15, "ReceivedStreamItem"), "Received StreamItem for Invocation {InvocationId}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __CancelingStreamItemCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(16, "CancelingStreamItem"), "Canceling dispatch of StreamItem message for Invocation {InvocationId}. The invocation was canceled.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ReceivedStreamItemAfterCloseCallback = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(17, "ReceivedStreamItemAfterClose"), "Invocation {InvocationId} received stream item after channel was closed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ReceivedInvocationCompletionCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(18, "ReceivedInvocationCompletion"), "Received Completion for Invocation {InvocationId}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __CancelingInvocationCompletionCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(19, "CancelingInvocationCompletion"), "Canceling dispatch of Completion message for Invocation {InvocationId}. The invocation was canceled.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __StoppedCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(21, "Stopped"), "HubConnection stopped.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __InvocationAlreadyInUseCallback = LoggerMessage.Define<string>(LogLevel.Critical, new EventId(22, "InvocationAlreadyInUse"), "Invocation ID '{InvocationId}' is already in use.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ReceivedUnexpectedResponseCallback = LoggerMessage.Define<string>(LogLevel.Error, new EventId(23, "ReceivedUnexpectedResponse"), "Unsolicited response received for invocation '{InvocationId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, int, Exception> __HubProtocolCallback = LoggerMessage.Define<string, int>(LogLevel.Information, new EventId(24, "HubProtocol"), "Using HubProtocol '{Protocol} v{Version}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, string, int, Exception> __PreparingStreamingInvocationCallback = LoggerMessage.Define<string, string, string, int>(LogLevel.Trace, new EventId(25, "PreparingStreamingInvocation"), "Preparing streaming invocation '{InvocationId}' of '{Target}', with return type '{ReturnType}' and {ArgumentCount} argument(s).", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ResettingKeepAliveTimerCallback = LoggerMessage.Define(LogLevel.Trace, new EventId(26, "ResettingKeepAliveTimer"), "Resetting keep-alive timer, received a message from the server.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ErrorDuringClosedEventCallback = LoggerMessage.Define(LogLevel.Error, new EventId(27, "ErrorDuringClosedEvent"), "An exception was thrown in the handler for the Closed event.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SendingHubHandshakeCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(28, "SendingHubHandshake"), "Sending Hub Handshake.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReceivedPingCallback = LoggerMessage.Define(LogLevel.Trace, new EventId(31, "ReceivedPing"), "Received a ping message.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ErrorInvokingClientSideMethodCallback = LoggerMessage.Define<string>(LogLevel.Error, new EventId(34, "ErrorInvokingClientSideMethod"), "Invoking client side method '{MethodName}' failed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ErrorReceivingHandshakeResponseCallback = LoggerMessage.Define(LogLevel.Error, new EventId(35, "ErrorReceivingHandshakeResponse"), "The underlying connection closed while processing the handshake response. See exception for details.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __HandshakeServerErrorCallback = LoggerMessage.Define<string>(LogLevel.Error, new EventId(36, "HandshakeServerError"), "Server returned handshake error: {Error}", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReceivedCloseCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(37, "ReceivedClose"), "Received close message.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ReceivedCloseWithErrorCallback = LoggerMessage.Define<string>(LogLevel.Error, new EventId(38, "ReceivedCloseWithError"), "Received close message with an error: {Error}", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __HandshakeCompleteCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(39, "HandshakeComplete"), "Handshake with server complete.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __RegisteringHandlerCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(40, "RegisteringHandler"), "Registering handler for client method '{MethodName}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __RemovingHandlersCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(58, "RemovingHandlers"), "Removing handlers for client method '{MethodName}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __StartingCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(41, "Starting"), "Starting HubConnection.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ErrorStartingConnectionCallback = LoggerMessage.Define(LogLevel.Error, new EventId(43, "ErrorStartingConnection"), "Error starting connection.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __StartedCallback = LoggerMessage.Define(LogLevel.Information, new EventId(44, "Started"), "HubConnection started.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __SendingCancellationCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(45, "SendingCancellation"), "Sending Cancellation for Invocation '{InvocationId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __CancelingOutstandingInvocationsCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(46, "CancelingOutstandingInvocations"), "Canceling all outstanding invocations.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReceiveLoopStartingCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(47, "ReceiveLoopStarting"), "Receive loop starting.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, double, Exception> __StartingServerTimeoutTimerCallback = LoggerMessage.Define<double>(LogLevel.Debug, new EventId(48, "StartingServerTimeoutTimer"), "Starting server timeout timer. Duration: {ServerTimeout:0.00}ms", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __NotUsingServerTimeoutCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(49, "NotUsingServerTimeout"), "Not using server timeout because the transport inherently tracks server availability.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ServerDisconnectedWithErrorCallback = LoggerMessage.Define(LogLevel.Error, new EventId(50, "ServerDisconnectedWithError"), "The server connection was terminated with an error.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __InvokingClosedEventHandlerCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(51, "InvokingClosedEventHandler"), "Invoking the Closed event handler.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __StoppingCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(52, "Stopping"), "Stopping HubConnection.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __TerminatingReceiveLoopCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(53, "TerminatingReceiveLoop"), "Terminating receive loop.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __WaitingForReceiveLoopToTerminateCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(54, "WaitingForReceiveLoopToTerminate"), "Waiting for the receive loop to terminate.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, long, Exception> __ProcessingMessageCallback = LoggerMessage.Define<long>(LogLevel.Debug, new EventId(56, "ProcessingMessage"), "Processing {MessageLength} byte message from server.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, int, Exception> __WaitingOnConnectionLockCallback = LoggerMessage.Define<string, string, int>(LogLevel.Trace, new EventId(42, "WaitingOnConnectionLock"), "Waiting on Connection Lock in {MethodName} ({FilePath}:{LineNumber}).", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, int, Exception> __ReleasingConnectionLockCallback = LoggerMessage.Define<string, string, int>(LogLevel.Trace, new EventId(20, "ReleasingConnectionLock"), "Releasing Connection Lock in {MethodName} ({FilePath}:{LineNumber}).", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __UnableToSendCancellationCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(55, "UnableToSendCancellation"), "Unable to send cancellation for invocation '{InvocationId}'. The connection is inactive.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, Exception> __ArgumentBindingFailureCallback = LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(57, "ArgumentBindingFailure"), "Failed to bind arguments received in invocation '{InvocationId}' of '{MethodName}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __AcquiredConnectionLockForPingCallback = LoggerMessage.Define(LogLevel.Trace, new EventId(61, "AcquiredConnectionLockForPing"), "Acquired the Connection Lock in order to ping the server.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __UnableToAcquireConnectionLockForPingCallback = LoggerMessage.Define(LogLevel.Trace, new EventId(62, "UnableToAcquireConnectionLockForPing"), "Skipping ping because a send is already in progress.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __StartingStreamCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(63, "StartingStream"), "Initiating stream '{StreamId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __SendingStreamItemCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(64, "StreamItemSent"), "Sending item for stream '{StreamId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __CancelingStreamCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(65, "CancelingStream"), "Stream '{StreamId}' has been canceled by client.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __CompletingStreamCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(66, "CompletingStream"), "Sending completion message for stream '{StreamId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, HubConnectionState, HubConnectionState, HubConnectionState, Exception> __StateTransitionFailedCallback = LoggerMessage.Define<HubConnectionState, HubConnectionState, HubConnectionState>(LogLevel.Error, new EventId(67, "StateTransitionFailed"), "The HubConnection failed to transition from the {ExpectedState} state to the {NewState} state because it was actually in the {ActualState} state.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReconnectingCallback = LoggerMessage.Define(LogLevel.Information, new EventId(68, "Reconnecting"), "HubConnection reconnecting.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReconnectingWithErrorCallback = LoggerMessage.Define(LogLevel.Error, new EventId(69, "ReconnectingWithError"), "HubConnection reconnecting due to an error.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, long, TimeSpan, Exception> __ReconnectedCallback = LoggerMessage.Define<long, TimeSpan>(LogLevel.Information, new EventId(70, "Reconnected"), "HubConnection reconnected successfully after {ReconnectAttempts} attempts and {ElapsedTime} elapsed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, long, TimeSpan, Exception> __ReconnectAttemptsExhaustedCallback = LoggerMessage.Define<long, TimeSpan>(LogLevel.Information, new EventId(71, "ReconnectAttemptsExhausted"), "Reconnect retries have been exhausted after {ReconnectAttempts} failed attempts and {ElapsedTime} elapsed. Disconnecting.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, long, TimeSpan, Exception> __AwaitingReconnectRetryDelayCallback = LoggerMessage.Define<long, TimeSpan>(LogLevel.Trace, new EventId(72, "AwaitingReconnectRetryDelay"), "Reconnect attempt number {ReconnectAttempts} will start in {RetryDelay}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReconnectAttemptFailedCallback = LoggerMessage.Define(LogLevel.Trace, new EventId(73, "ReconnectAttemptFailed"), "Reconnect attempt failed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ErrorDuringReconnectingEventCallback = LoggerMessage.Define(LogLevel.Error, new EventId(74, "ErrorDuringReconnectingEvent"), "An exception was thrown in the handler for the Reconnecting event.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ErrorDuringReconnectedEventCallback = LoggerMessage.Define(LogLevel.Error, new EventId(75, "ErrorDuringReconnectedEvent"), "An exception was thrown in the handler for the Reconnected event.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ErrorDuringNextRetryDelayCallback = LoggerMessage.Define(LogLevel.Error, new EventId(76, "ErrorDuringNextRetryDelay"), "An exception was thrown from IRetryPolicy.NextRetryDelay().", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __FirstReconnectRetryDelayNullCallback = LoggerMessage.Define(LogLevel.Warning, new EventId(77, "FirstReconnectRetryDelayNull"), "Connection not reconnecting because the IRetryPolicy returned null on the first reconnect attempt.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReconnectingStoppedDuringRetryDelayCallback = LoggerMessage.Define(LogLevel.Trace, new EventId(78, "ReconnectingStoppedDueToStateChangeDuringRetryDelay"), "Connection stopped during reconnect delay. Done reconnecting.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReconnectingStoppedDuringReconnectAttemptCallback = LoggerMessage.Define(LogLevel.Trace, new EventId(79, "ReconnectingStoppedDueToStateChangeDuringReconnectAttempt"), "Connection stopped during reconnect attempt. Done reconnecting.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, HubConnectionState, HubConnectionState, Exception> __AttemptingStateTransitionCallback = LoggerMessage.Define<HubConnectionState, HubConnectionState>(LogLevel.Trace, new EventId(80, "AttemptingStateTransition"), "The HubConnection is attempting to transition from the {ExpectedState} state to the {NewState} state.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ErrorInvalidHandshakeResponseCallback = LoggerMessage.Define(LogLevel.Error, new EventId(81, "ErrorInvalidHandshakeResponse"), "Received an invalid handshake response.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, double, Exception> __ErrorHandshakeTimedOutCallback = LoggerMessage.Define<double>(LogLevel.Error, new EventId(82, "ErrorHandshakeTimedOut"), "The handshake timed out after {HandshakeTimeoutSeconds} seconds.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ErrorHandshakeCanceledCallback = LoggerMessage.Define(LogLevel.Error, new EventId(83, "ErrorHandshakeCanceled"), "The handshake was canceled by the client.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ErroredStreamCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(84, "ErroredStream"), "Client threw an error for stream '{StreamId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __MissingResultHandlerCallback = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(85, "MissingResultHandler"), "Failed to find a value returning handler for '{Target}' method. Sending error to server.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ResultNotExpectedCallback = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(86, "ResultNotExpected"), "Result given for '{Target}' method but server is not expecting a result.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __CompletingStreamNotSentCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(87, "CompletingStreamNotSent"), "Completion message for stream '{StreamId}' was not sent because the connection is closed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, Exception> __ErrorSendingInvocationResultCallback = LoggerMessage.Define<string, string>(LogLevel.Warning, new EventId(88, "ErrorSendingInvocationResult"), "Error returning result for invocation '{InvocationId}' for method '{Target}' because the underlying connection is closed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ErrorSendingStreamCompletionCallback = LoggerMessage.Define<string>(LogLevel.Trace, new EventId(89, "ErrorSendingStreamCompletion"), "Error sending Completion message for stream '{StreamId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, Exception> __DroppingMessageCallback = LoggerMessage.Define<string, string>(LogLevel.Trace, new EventId(90, "DroppingMessage"), "Dropping {MessageType} with ID '{InvocationId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, long, Exception> __ReceivedAckMessageCallback = LoggerMessage.Define<long>(LogLevel.Trace, new EventId(91, "ReceivedAckMessage"), "Received AckMessage with Sequence ID '{SequenceId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, long, Exception> __ReceivedSequenceMessageCallback = LoggerMessage.Define<long>(LogLevel.Trace, new EventId(92, "ReceivedSequenceMessage"), "Received SequenceMessage with Sequence ID '{SequenceId}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, int, Exception> __DisablingReconnectCallback = LoggerMessage.Define<string, int>(LogLevel.Debug, new EventId(93, "DisablingReconnect"), "HubProtocol '{Protocol} v{Version}' does not support Stateful Reconnect. Disabling the feature.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[LoggerMessage(1, LogLevel.Trace, "Preparing non-blocking invocation of '{Target}', with {ArgumentCount} argument(s).", EventName = "PreparingNonBlockingInvocation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void PreparingNonBlockingInvocation(ILogger logger, string target, int argumentCount)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__PreparingNonBlockingInvocationCallback(logger, target, argumentCount, null);
				}
			}

			[LoggerMessage(2, LogLevel.Trace, "Preparing blocking invocation '{InvocationId}' of '{Target}', with return type '{ReturnType}' and {ArgumentCount} argument(s).", EventName = "PreparingBlockingInvocation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void PreparingBlockingInvocation(ILogger logger, string invocationId, string target, string returnType, int argumentCount)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__PreparingBlockingInvocationCallback(logger, invocationId, target, returnType, argumentCount, null);
				}
			}

			[LoggerMessage(3, LogLevel.Debug, "Registering Invocation ID '{InvocationId}' for tracking.", EventName = "RegisteringInvocation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void RegisteringInvocation(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__RegisteringInvocationCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(4, LogLevel.Trace, "Issuing Invocation '{InvocationId}': {ReturnType} {MethodName}({Args}).", EventName = "IssuingInvocation", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void IssuingInvocation(ILogger logger, string invocationId, string returnType, string methodName, string args)
			{
				__IssuingInvocationCallback(logger, invocationId, returnType, methodName, args, null);
			}

			public static void IssuingInvocation(ILogger logger, string invocationId, string returnType, string methodName, object[] args)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					string args2 = ((args == null) ? string.Empty : string.Join(", ", args.Select((object a) => a?.GetType().FullName ?? "(null)")));
					IssuingInvocation(logger, invocationId, returnType, methodName, args2);
				}
			}

			public static void SendingMessage(ILogger logger, HubMessage message)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					HubInvocationMessage hubInvocationMessage = message as HubInvocationMessage;
					if (hubInvocationMessage != null)
					{
						SendingMessage(logger, message.GetType().Name, hubInvocationMessage.InvocationId);
					}
					else
					{
						SendingMessageGeneric(logger, message.GetType().Name);
					}
				}
			}

			[LoggerMessage(5, LogLevel.Debug, "Sending {MessageType} message '{InvocationId}'.", EventName = "SendingMessage", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void SendingMessage(ILogger logger, string messageType, string invocationId)
			{
				__SendingMessageCallback(logger, messageType, invocationId, null);
			}

			[LoggerMessage(59, LogLevel.Debug, "Sending {MessageType} message.", EventName = "SendingMessageGeneric", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void SendingMessageGeneric(ILogger logger, string messageType)
			{
				__SendingMessageGenericCallback(logger, messageType, null);
			}

			public static void MessageSent(ILogger logger, HubMessage message)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					HubInvocationMessage hubInvocationMessage = message as HubInvocationMessage;
					if (hubInvocationMessage != null)
					{
						MessageSent(logger, message.GetType().Name, hubInvocationMessage.InvocationId);
					}
					else
					{
						MessageSentGeneric(logger, message.GetType().Name);
					}
				}
			}

			[LoggerMessage(6, LogLevel.Debug, "Sending {MessageType} message '{InvocationId}' completed.", EventName = "MessageSent", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void MessageSent(ILogger logger, string messageType, string invocationId)
			{
				__MessageSentCallback(logger, messageType, invocationId, null);
			}

			[LoggerMessage(60, LogLevel.Debug, "Sending {MessageType} message completed.", EventName = "MessageSentGeneric", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void MessageSentGeneric(ILogger logger, string messageType)
			{
				__MessageSentGenericCallback(logger, messageType, null);
			}

			[LoggerMessage(7, LogLevel.Error, "Sending Invocation '{InvocationId}' failed.", EventName = "FailedToSendInvocation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void FailedToSendInvocation(ILogger logger, string invocationId, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__FailedToSendInvocationCallback(logger, invocationId, exception);
				}
			}

			[LoggerMessage(8, LogLevel.Trace, "Received Invocation '{InvocationId}': {MethodName}({Args}).", EventName = "ReceivedInvocation", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void ReceivedInvocation(ILogger logger, string invocationId, string methodName, string args)
			{
				__ReceivedInvocationCallback(logger, invocationId, methodName, args, null);
			}

			public static void ReceivedInvocation(ILogger logger, string invocationId, string methodName, object[] args)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					string args2 = ((args == null) ? string.Empty : string.Join(", ", args.Select((object a) => a?.GetType().FullName ?? "(null)")));
					ReceivedInvocation(logger, invocationId, methodName, args2);
				}
			}

			[LoggerMessage(9, LogLevel.Warning, "Dropped unsolicited Completion message for invocation '{InvocationId}'.", EventName = "DroppedCompletionMessage")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void DroppedCompletionMessage(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__DroppedCompletionMessageCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(10, LogLevel.Warning, "Dropped unsolicited StreamItem message for invocation '{InvocationId}'.", EventName = "DroppedStreamMessage")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void DroppedStreamMessage(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__DroppedStreamMessageCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(11, LogLevel.Trace, "Shutting down connection.", EventName = "ShutdownConnection")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ShutdownConnection(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ShutdownConnectionCallback(logger, null);
				}
			}

			[LoggerMessage(12, LogLevel.Error, "Connection is shutting down due to an error.", EventName = "ShutdownWithError")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ShutdownWithError(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ShutdownWithErrorCallback(logger, exception);
				}
			}

			[LoggerMessage(13, LogLevel.Trace, "Removing pending invocation {InvocationId}.", EventName = "RemovingInvocation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void RemovingInvocation(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__RemovingInvocationCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(14, LogLevel.Warning, "Failed to find handler for '{Target}' method.", EventName = "MissingHandler")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void MissingHandler(ILogger logger, string target)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__MissingHandlerCallback(logger, target, null);
				}
			}

			[LoggerMessage(15, LogLevel.Trace, "Received StreamItem for Invocation {InvocationId}.", EventName = "ReceivedStreamItem")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedStreamItem(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ReceivedStreamItemCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(16, LogLevel.Trace, "Canceling dispatch of StreamItem message for Invocation {InvocationId}. The invocation was canceled.", EventName = "CancelingStreamItem")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void CancelingStreamItem(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__CancelingStreamItemCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(17, LogLevel.Warning, "Invocation {InvocationId} received stream item after channel was closed.", EventName = "ReceivedStreamItemAfterClose")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedStreamItemAfterClose(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__ReceivedStreamItemAfterCloseCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(18, LogLevel.Trace, "Received Completion for Invocation {InvocationId}.", EventName = "ReceivedInvocationCompletion")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedInvocationCompletion(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ReceivedInvocationCompletionCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(19, LogLevel.Trace, "Canceling dispatch of Completion message for Invocation {InvocationId}. The invocation was canceled.", EventName = "CancelingInvocationCompletion")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void CancelingInvocationCompletion(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__CancelingInvocationCompletionCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(21, LogLevel.Debug, "HubConnection stopped.", EventName = "Stopped")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void Stopped(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__StoppedCallback(logger, null);
				}
			}

			[LoggerMessage(22, LogLevel.Critical, "Invocation ID '{InvocationId}' is already in use.", EventName = "InvocationAlreadyInUse")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void InvocationAlreadyInUse(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Critical))
				{
					__InvocationAlreadyInUseCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(23, LogLevel.Error, "Unsolicited response received for invocation '{InvocationId}'.", EventName = "ReceivedUnexpectedResponse")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedUnexpectedResponse(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ReceivedUnexpectedResponseCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(24, LogLevel.Information, "Using HubProtocol '{Protocol} v{Version}'.", EventName = "HubProtocol")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void HubProtocol(ILogger logger, string protocol, int version)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__HubProtocolCallback(logger, protocol, version, null);
				}
			}

			[LoggerMessage(25, LogLevel.Trace, "Preparing streaming invocation '{InvocationId}' of '{Target}', with return type '{ReturnType}' and {ArgumentCount} argument(s).", EventName = "PreparingStreamingInvocation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void PreparingStreamingInvocation(ILogger logger, string invocationId, string target, string returnType, int argumentCount)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__PreparingStreamingInvocationCallback(logger, invocationId, target, returnType, argumentCount, null);
				}
			}

			[LoggerMessage(26, LogLevel.Trace, "Resetting keep-alive timer, received a message from the server.", EventName = "ResettingKeepAliveTimer")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ResettingKeepAliveTimer(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ResettingKeepAliveTimerCallback(logger, null);
				}
			}

			[LoggerMessage(27, LogLevel.Error, "An exception was thrown in the handler for the Closed event.", EventName = "ErrorDuringClosedEvent")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorDuringClosedEvent(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorDuringClosedEventCallback(logger, exception);
				}
			}

			[LoggerMessage(28, LogLevel.Debug, "Sending Hub Handshake.", EventName = "SendingHubHandshake")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendingHubHandshake(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendingHubHandshakeCallback(logger, null);
				}
			}

			[LoggerMessage(31, LogLevel.Trace, "Received a ping message.", EventName = "ReceivedPing")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedPing(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ReceivedPingCallback(logger, null);
				}
			}

			[LoggerMessage(34, LogLevel.Error, "Invoking client side method '{MethodName}' failed.", EventName = "ErrorInvokingClientSideMethod")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorInvokingClientSideMethod(ILogger logger, string methodName, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorInvokingClientSideMethodCallback(logger, methodName, exception);
				}
			}

			[LoggerMessage(35, LogLevel.Error, "The underlying connection closed while processing the handshake response. See exception for details.", EventName = "ErrorReceivingHandshakeResponse")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorReceivingHandshakeResponse(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorReceivingHandshakeResponseCallback(logger, exception);
				}
			}

			[LoggerMessage(36, LogLevel.Error, "Server returned handshake error: {Error}", EventName = "HandshakeServerError")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void HandshakeServerError(ILogger logger, string error)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__HandshakeServerErrorCallback(logger, error, null);
				}
			}

			[LoggerMessage(37, LogLevel.Debug, "Received close message.", EventName = "ReceivedClose")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedClose(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ReceivedCloseCallback(logger, null);
				}
			}

			[LoggerMessage(38, LogLevel.Error, "Received close message with an error: {Error}", EventName = "ReceivedCloseWithError")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedCloseWithError(ILogger logger, string error)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ReceivedCloseWithErrorCallback(logger, error, null);
				}
			}

			[LoggerMessage(39, LogLevel.Debug, "Handshake with server complete.", EventName = "HandshakeComplete")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void HandshakeComplete(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__HandshakeCompleteCallback(logger, null);
				}
			}

			[LoggerMessage(40, LogLevel.Debug, "Registering handler for client method '{MethodName}'.", EventName = "RegisteringHandler")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void RegisteringHandler(ILogger logger, string methodName)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__RegisteringHandlerCallback(logger, methodName, null);
				}
			}

			[LoggerMessage(58, LogLevel.Debug, "Removing handlers for client method '{MethodName}'.", EventName = "RemovingHandlers")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void RemovingHandlers(ILogger logger, string methodName)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__RemovingHandlersCallback(logger, methodName, null);
				}
			}

			[LoggerMessage(41, LogLevel.Debug, "Starting HubConnection.", EventName = "Starting")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void Starting(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__StartingCallback(logger, null);
				}
			}

			[LoggerMessage(43, LogLevel.Error, "Error starting connection.", EventName = "ErrorStartingConnection")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorStartingConnection(ILogger logger, Exception ex)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorStartingConnectionCallback(logger, ex);
				}
			}

			[LoggerMessage(44, LogLevel.Information, "HubConnection started.", EventName = "Started")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void Started(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__StartedCallback(logger, null);
				}
			}

			[LoggerMessage(45, LogLevel.Debug, "Sending Cancellation for Invocation '{InvocationId}'.", EventName = "SendingCancellation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendingCancellation(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendingCancellationCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(46, LogLevel.Debug, "Canceling all outstanding invocations.", EventName = "CancelingOutstandingInvocations")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void CancelingOutstandingInvocations(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__CancelingOutstandingInvocationsCallback(logger, null);
				}
			}

			[LoggerMessage(47, LogLevel.Debug, "Receive loop starting.", EventName = "ReceiveLoopStarting")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceiveLoopStarting(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ReceiveLoopStartingCallback(logger, null);
				}
			}

			[LoggerMessage(48, LogLevel.Debug, "Starting server timeout timer. Duration: {ServerTimeout:0.00}ms", EventName = "StartingServerTimeoutTimer", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void StartingServerTimeoutTimer(ILogger logger, double serverTimeout)
			{
				__StartingServerTimeoutTimerCallback(logger, serverTimeout, null);
			}

			public static void StartingServerTimeoutTimer(ILogger logger, TimeSpan serverTimeout)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					StartingServerTimeoutTimer(logger, serverTimeout.TotalMilliseconds);
				}
			}

			[LoggerMessage(49, LogLevel.Debug, "Not using server timeout because the transport inherently tracks server availability.", EventName = "NotUsingServerTimeout")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void NotUsingServerTimeout(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__NotUsingServerTimeoutCallback(logger, null);
				}
			}

			[LoggerMessage(50, LogLevel.Error, "The server connection was terminated with an error.", EventName = "ServerDisconnectedWithError")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ServerDisconnectedWithError(ILogger logger, Exception ex)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ServerDisconnectedWithErrorCallback(logger, ex);
				}
			}

			[LoggerMessage(51, LogLevel.Debug, "Invoking the Closed event handler.", EventName = "InvokingClosedEventHandler")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void InvokingClosedEventHandler(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__InvokingClosedEventHandlerCallback(logger, null);
				}
			}

			[LoggerMessage(52, LogLevel.Debug, "Stopping HubConnection.", EventName = "Stopping")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void Stopping(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__StoppingCallback(logger, null);
				}
			}

			[LoggerMessage(53, LogLevel.Debug, "Terminating receive loop.", EventName = "TerminatingReceiveLoop")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void TerminatingReceiveLoop(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__TerminatingReceiveLoopCallback(logger, null);
				}
			}

			[LoggerMessage(54, LogLevel.Debug, "Waiting for the receive loop to terminate.", EventName = "WaitingForReceiveLoopToTerminate")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void WaitingForReceiveLoopToTerminate(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__WaitingForReceiveLoopToTerminateCallback(logger, null);
				}
			}

			[LoggerMessage(56, LogLevel.Debug, "Processing {MessageLength} byte message from server.", EventName = "ProcessingMessage")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ProcessingMessage(ILogger logger, long messageLength)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ProcessingMessageCallback(logger, messageLength, null);
				}
			}

			[LoggerMessage(42, LogLevel.Trace, "Waiting on Connection Lock in {MethodName} ({FilePath}:{LineNumber}).", EventName = "WaitingOnConnectionLock")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void WaitingOnConnectionLock(ILogger logger, string methodName, string filePath, int lineNumber)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__WaitingOnConnectionLockCallback(logger, methodName, filePath, lineNumber, null);
				}
			}

			[LoggerMessage(20, LogLevel.Trace, "Releasing Connection Lock in {MethodName} ({FilePath}:{LineNumber}).", EventName = "ReleasingConnectionLock")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReleasingConnectionLock(ILogger logger, string methodName, string filePath, int lineNumber)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ReleasingConnectionLockCallback(logger, methodName, filePath, lineNumber, null);
				}
			}

			[LoggerMessage(55, LogLevel.Trace, "Unable to send cancellation for invocation '{InvocationId}'. The connection is inactive.", EventName = "UnableToSendCancellation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void UnableToSendCancellation(ILogger logger, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__UnableToSendCancellationCallback(logger, invocationId, null);
				}
			}

			[LoggerMessage(57, LogLevel.Error, "Failed to bind arguments received in invocation '{InvocationId}' of '{MethodName}'.", EventName = "ArgumentBindingFailure")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ArgumentBindingFailure(ILogger logger, string invocationId, string methodName, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ArgumentBindingFailureCallback(logger, invocationId, methodName, exception);
				}
			}

			[LoggerMessage(61, LogLevel.Trace, "Acquired the Connection Lock in order to ping the server.", EventName = "AcquiredConnectionLockForPing")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void AcquiredConnectionLockForPing(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__AcquiredConnectionLockForPingCallback(logger, null);
				}
			}

			[LoggerMessage(62, LogLevel.Trace, "Skipping ping because a send is already in progress.", EventName = "UnableToAcquireConnectionLockForPing")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void UnableToAcquireConnectionLockForPing(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__UnableToAcquireConnectionLockForPingCallback(logger, null);
				}
			}

			[LoggerMessage(63, LogLevel.Trace, "Initiating stream '{StreamId}'.", EventName = "StartingStream")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void StartingStream(ILogger logger, string streamId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__StartingStreamCallback(logger, streamId, null);
				}
			}

			[LoggerMessage(64, LogLevel.Trace, "Sending item for stream '{StreamId}'.", EventName = "StreamItemSent")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendingStreamItem(ILogger logger, string streamId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__SendingStreamItemCallback(logger, streamId, null);
				}
			}

			[LoggerMessage(65, LogLevel.Trace, "Stream '{StreamId}' has been canceled by client.", EventName = "CancelingStream")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void CancelingStream(ILogger logger, string streamId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__CancelingStreamCallback(logger, streamId, null);
				}
			}

			[LoggerMessage(66, LogLevel.Trace, "Sending completion message for stream '{StreamId}'.", EventName = "CompletingStream")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void CompletingStream(ILogger logger, string streamId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__CompletingStreamCallback(logger, streamId, null);
				}
			}

			[LoggerMessage(67, LogLevel.Error, "The HubConnection failed to transition from the {ExpectedState} state to the {NewState} state because it was actually in the {ActualState} state.", EventName = "StateTransitionFailed")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void StateTransitionFailed(ILogger logger, HubConnectionState expectedState, HubConnectionState newState, HubConnectionState actualState)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__StateTransitionFailedCallback(logger, expectedState, newState, actualState, null);
				}
			}

			[LoggerMessage(68, LogLevel.Information, "HubConnection reconnecting.", EventName = "Reconnecting")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void Reconnecting(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__ReconnectingCallback(logger, null);
				}
			}

			[LoggerMessage(69, LogLevel.Error, "HubConnection reconnecting due to an error.", EventName = "ReconnectingWithError")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReconnectingWithError(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ReconnectingWithErrorCallback(logger, exception);
				}
			}

			[LoggerMessage(70, LogLevel.Information, "HubConnection reconnected successfully after {ReconnectAttempts} attempts and {ElapsedTime} elapsed.", EventName = "Reconnected")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void Reconnected(ILogger logger, long reconnectAttempts, TimeSpan elapsedTime)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__ReconnectedCallback(logger, reconnectAttempts, elapsedTime, null);
				}
			}

			[LoggerMessage(71, LogLevel.Information, "Reconnect retries have been exhausted after {ReconnectAttempts} failed attempts and {ElapsedTime} elapsed. Disconnecting.", EventName = "ReconnectAttemptsExhausted")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReconnectAttemptsExhausted(ILogger logger, long reconnectAttempts, TimeSpan elapsedTime)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__ReconnectAttemptsExhaustedCallback(logger, reconnectAttempts, elapsedTime, null);
				}
			}

			[LoggerMessage(72, LogLevel.Trace, "Reconnect attempt number {ReconnectAttempts} will start in {RetryDelay}.", EventName = "AwaitingReconnectRetryDelay")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void AwaitingReconnectRetryDelay(ILogger logger, long reconnectAttempts, TimeSpan retryDelay)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__AwaitingReconnectRetryDelayCallback(logger, reconnectAttempts, retryDelay, null);
				}
			}

			[LoggerMessage(73, LogLevel.Trace, "Reconnect attempt failed.", EventName = "ReconnectAttemptFailed")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReconnectAttemptFailed(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ReconnectAttemptFailedCallback(logger, exception);
				}
			}

			[LoggerMessage(74, LogLevel.Error, "An exception was thrown in the handler for the Reconnecting event.", EventName = "ErrorDuringReconnectingEvent")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorDuringReconnectingEvent(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorDuringReconnectingEventCallback(logger, exception);
				}
			}

			[LoggerMessage(75, LogLevel.Error, "An exception was thrown in the handler for the Reconnected event.", EventName = "ErrorDuringReconnectedEvent")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorDuringReconnectedEvent(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorDuringReconnectedEventCallback(logger, exception);
				}
			}

			[LoggerMessage(76, LogLevel.Error, "An exception was thrown from IRetryPolicy.NextRetryDelay().", EventName = "ErrorDuringNextRetryDelay")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorDuringNextRetryDelay(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorDuringNextRetryDelayCallback(logger, exception);
				}
			}

			[LoggerMessage(77, LogLevel.Warning, "Connection not reconnecting because the IRetryPolicy returned null on the first reconnect attempt.", EventName = "FirstReconnectRetryDelayNull")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void FirstReconnectRetryDelayNull(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__FirstReconnectRetryDelayNullCallback(logger, null);
				}
			}

			[LoggerMessage(78, LogLevel.Trace, "Connection stopped during reconnect delay. Done reconnecting.", EventName = "ReconnectingStoppedDueToStateChangeDuringRetryDelay")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReconnectingStoppedDuringRetryDelay(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ReconnectingStoppedDuringRetryDelayCallback(logger, null);
				}
			}

			[LoggerMessage(79, LogLevel.Trace, "Connection stopped during reconnect attempt. Done reconnecting.", EventName = "ReconnectingStoppedDueToStateChangeDuringReconnectAttempt")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReconnectingStoppedDuringReconnectAttempt(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ReconnectingStoppedDuringReconnectAttemptCallback(logger, null);
				}
			}

			[LoggerMessage(80, LogLevel.Trace, "The HubConnection is attempting to transition from the {ExpectedState} state to the {NewState} state.", EventName = "AttemptingStateTransition")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void AttemptingStateTransition(ILogger logger, HubConnectionState expectedState, HubConnectionState newState)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__AttemptingStateTransitionCallback(logger, expectedState, newState, null);
				}
			}

			[LoggerMessage(81, LogLevel.Error, "Received an invalid handshake response.", EventName = "ErrorInvalidHandshakeResponse")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorInvalidHandshakeResponse(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorInvalidHandshakeResponseCallback(logger, exception);
				}
			}

			public static void ErrorHandshakeTimedOut(ILogger logger, TimeSpan handshakeTimeout, Exception exception)
			{
				ErrorHandshakeTimedOut(logger, handshakeTimeout.TotalSeconds, exception);
			}

			[LoggerMessage(82, LogLevel.Error, "The handshake timed out after {HandshakeTimeoutSeconds} seconds.", EventName = "ErrorHandshakeTimedOut")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void ErrorHandshakeTimedOut(ILogger logger, double HandshakeTimeoutSeconds, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorHandshakeTimedOutCallback(logger, HandshakeTimeoutSeconds, exception);
				}
			}

			[LoggerMessage(83, LogLevel.Error, "The handshake was canceled by the client.", EventName = "ErrorHandshakeCanceled")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorHandshakeCanceled(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorHandshakeCanceledCallback(logger, exception);
				}
			}

			[LoggerMessage(84, LogLevel.Trace, "Client threw an error for stream '{StreamId}'.", EventName = "ErroredStream")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErroredStream(ILogger logger, string streamId, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ErroredStreamCallback(logger, streamId, exception);
				}
			}

			[LoggerMessage(85, LogLevel.Warning, "Failed to find a value returning handler for '{Target}' method. Sending error to server.", EventName = "MissingResultHandler")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void MissingResultHandler(ILogger logger, string target)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__MissingResultHandlerCallback(logger, target, null);
				}
			}

			[LoggerMessage(86, LogLevel.Warning, "Result given for '{Target}' method but server is not expecting a result.", EventName = "ResultNotExpected")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ResultNotExpected(ILogger logger, string target)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__ResultNotExpectedCallback(logger, target, null);
				}
			}

			[LoggerMessage(87, LogLevel.Trace, "Completion message for stream '{StreamId}' was not sent because the connection is closed.", EventName = "CompletingStreamNotSent")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void CompletingStreamNotSent(ILogger logger, string streamId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__CompletingStreamNotSentCallback(logger, streamId, null);
				}
			}

			[LoggerMessage(88, LogLevel.Warning, "Error returning result for invocation '{InvocationId}' for method '{Target}' because the underlying connection is closed.", EventName = "ErrorSendingInvocationResult")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorSendingInvocationResult(ILogger logger, string invocationId, string target, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__ErrorSendingInvocationResultCallback(logger, invocationId, target, exception);
				}
			}

			[LoggerMessage(89, LogLevel.Trace, "Error sending Completion message for stream '{StreamId}'.", EventName = "ErrorSendingStreamCompletion")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorSendingStreamCompletion(ILogger logger, string streamId, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ErrorSendingStreamCompletionCallback(logger, streamId, exception);
				}
			}

			[LoggerMessage(90, LogLevel.Trace, "Dropping {MessageType} with ID '{InvocationId}'.", EventName = "DroppingMessage")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void DroppingMessage(ILogger logger, string messageType, string invocationId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__DroppingMessageCallback(logger, messageType, invocationId, null);
				}
			}

			[LoggerMessage(91, LogLevel.Trace, "Received AckMessage with Sequence ID '{SequenceId}'.", EventName = "ReceivedAckMessage")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedAckMessage(ILogger logger, long sequenceId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ReceivedAckMessageCallback(logger, sequenceId, null);
				}
			}

			[LoggerMessage(92, LogLevel.Trace, "Received SequenceMessage with Sequence ID '{SequenceId}'.", EventName = "ReceivedSequenceMessage")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedSequenceMessage(ILogger logger, long sequenceId)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__ReceivedSequenceMessageCallback(logger, sequenceId, null);
				}
			}

			[LoggerMessage(93, LogLevel.Debug, "HubProtocol '{Protocol} v{Version}' does not support Stateful Reconnect. Disabling the feature.", EventName = "DisablingReconnect")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void DisablingReconnect(ILogger logger, string protocol, int version)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__DisablingReconnectCallback(logger, protocol, version, null);
				}
			}
		}

		public static readonly TimeSpan DefaultServerTimeout = TimeSpan.FromSeconds(30.0);

		public static readonly TimeSpan DefaultHandshakeTimeout = TimeSpan.FromSeconds(15.0);

		public static readonly TimeSpan DefaultKeepAliveInterval = TimeSpan.FromSeconds(15.0);

		internal const long DefaultStatefulReconnectBufferSize = 100000L;

		private static readonly UnboundedChannelOptions _receiveLoopOptions = new UnboundedChannelOptions
		{
			SingleReader = true,
			SingleWriter = true
		};

		private static readonly MethodInfo _sendStreamItemsMethod = typeof(HubConnection).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Single((MethodInfo m) => m.Name.Equals("SendStreamItems"));

		private static readonly MethodInfo _sendIAsyncStreamItemsMethod = typeof(HubConnection).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Single((MethodInfo m) => m.Name.Equals("SendIAsyncEnumerableStreamItems"));

		private readonly ILoggerFactory _loggerFactory;

		private readonly ILogger _logger;

		private readonly ConnectionLogScope _logScope;

		private readonly IHubProtocol _protocol;

		private readonly IServiceProvider _serviceProvider;

		private readonly IConnectionFactory _connectionFactory;

		private readonly IRetryPolicy _reconnectPolicy;

		private readonly EndPoint _endPoint;

		private readonly ConcurrentDictionary<string, InvocationHandlerList> _handlers = new ConcurrentDictionary<string, InvocationHandlerList>(StringComparer.Ordinal);

		private readonly ReconnectingConnectionState _state;

		private bool _disposed;

		internal TimeSpan TickRate { get; set; } = TimeSpan.FromSeconds(1.0);


		public TimeSpan ServerTimeout { get; set; }

		public TimeSpan KeepAliveInterval { get; set; }

		public TimeSpan HandshakeTimeout { get; set; } = DefaultHandshakeTimeout;


		public string? ConnectionId => _state.CurrentConnectionStateUnsynchronized?.Connection.ConnectionId;

		public HubConnectionState State => _state.OverallState;

		public event Func<Exception?, Task>? Closed;

		public event Func<Exception?, Task>? Reconnecting;

		public event Func<string?, Task>? Reconnected;

		public HubConnection(IConnectionFactory connectionFactory, IHubProtocol protocol, EndPoint endPoint, IServiceProvider serviceProvider, ILoggerFactory loggerFactory, IRetryPolicy reconnectPolicy)
			: this(connectionFactory, protocol, endPoint, serviceProvider, loggerFactory)
		{
			_reconnectPolicy = reconnectPolicy;
		}

		public HubConnection(IConnectionFactory connectionFactory, IHubProtocol protocol, EndPoint endPoint, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
		{
			_connectionFactory = connectionFactory ?? throw new ArgumentNullException("connectionFactory");
			_protocol = protocol ?? throw new ArgumentNullException("protocol");
			_endPoint = endPoint ?? throw new ArgumentNullException("endPoint");
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException("serviceProvider");
			_loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
			_logger = _loggerFactory.CreateLogger<HubConnection>();
			_state = new ReconnectingConnectionState(_logger);
			_logScope = new ConnectionLogScope();
			IOptions<HubConnectionOptions> service = serviceProvider.GetService<IOptions<HubConnectionOptions>>();
			ServerTimeout = service?.Value.ServerTimeout ?? DefaultServerTimeout;
			KeepAliveInterval = service?.Value.KeepAliveInterval ?? DefaultKeepAliveInterval;
		}

		public virtual async Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			CheckDisposed();
			using (_logger.BeginScope(_logScope))
			{
				await StartAsyncInner(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task StartAsyncInner(CancellationToken cancellationToken = default(CancellationToken))
		{
			await _state.WaitConnectionLockAsync(cancellationToken, "StartAsyncInner", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 261).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (!_state.TryChangeState(HubConnectionState.Disconnected, HubConnectionState.Connecting))
				{
					throw new InvalidOperationException("The HubConnection cannot be started if it is not in the Disconnected state.");
				}
				if (_state.StopCts.Token.IsCancellationRequested)
				{
					throw new InvalidOperationException("The HubConnection cannot be started while StopAsync is running.");
				}
				CancellationToken linkedToken;
				using (CancellationTokenUtils.CreateLinkedToken(cancellationToken, _state.StopCts.Token, out linkedToken))
				{
					await StartAsyncCore(linkedToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				_state.ChangeState(HubConnectionState.Connecting, HubConnectionState.Connected);
			}
			catch
			{
				if (_state.TryChangeState(HubConnectionState.Connecting, HubConnectionState.Disconnected))
				{
					_state.StopCts = new CancellationTokenSource();
				}
				throw;
			}
			finally
			{
				_state.ReleaseConnectionLock("StartAsyncInner", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 294);
			}
		}

		public virtual async Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			CheckDisposed();
			using (_logger.BeginScope(_logScope))
			{
				await StopAsyncCore(disposing: false).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public virtual async ValueTask DisposeAsync()
		{
			if (!_disposed)
			{
				using (_logger.BeginScope(_logScope))
				{
					await StopAsyncCore(disposing: true).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}

		public virtual IDisposable On(string methodName, Type[] parameterTypes, Func<object?[], object, Task<object?>> handler, object state)
		{
			string methodName2 = methodName;
			Log.RegisteringHandler(_logger, methodName2);
			CheckDisposed();
			InvocationHandler invocationHandler = new InvocationHandler(parameterTypes, handler, state);
			InvocationHandlerList handlerList = _handlers.AddOrUpdate(methodName2, (string _) => new InvocationHandlerList(invocationHandler), delegate(string _, InvocationHandlerList invocations)
			{
				lock (invocations)
				{
					invocations.Add(methodName2, invocationHandler);
					return invocations;
				}
			});
			return new Subscription(invocationHandler, handlerList);
		}

		public virtual IDisposable On(string methodName, Type[] parameterTypes, Func<object?[], object, Task> handler, object state)
		{
			string methodName2 = methodName;
			Log.RegisteringHandler(_logger, methodName2);
			CheckDisposed();
			InvocationHandler invocationHandler = new InvocationHandler(parameterTypes, handler, state);
			InvocationHandlerList handlerList = _handlers.AddOrUpdate(methodName2, (string _) => new InvocationHandlerList(invocationHandler), delegate(string _, InvocationHandlerList invocations)
			{
				lock (invocations)
				{
					invocations.Add(methodName2, invocationHandler);
					return invocations;
				}
			});
			return new Subscription(invocationHandler, handlerList);
		}

		public virtual void Remove(string methodName)
		{
			CheckDisposed();
			Log.RemovingHandlers(_logger, methodName);
			_handlers.TryRemove(methodName, out var _);
		}

		public virtual async Task<ChannelReader<object?>> StreamAsChannelCoreAsync(string methodName, Type returnType, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (_logger.BeginScope(_logScope))
			{
				return await StreamAsChannelCoreAsyncCore(methodName, returnType, args, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public virtual async Task<object?> InvokeCoreAsync(string methodName, Type returnType, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (_logger.BeginScope(_logScope))
			{
				return await InvokeCoreAsyncCore(methodName, returnType, args, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public virtual async Task SendCoreAsync(string methodName, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (_logger.BeginScope(_logScope))
			{
				await SendCoreAsyncCore(methodName, args, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task StartAsyncCore(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			CheckDisposed();
			Log.Starting(_logger);
			ConnectionContext connection = await _connectionFactory.ConnectAsync(_endPoint, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			ConnectionState startingConnectionState = new ConnectionState(connection, this);
			IStatefulReconnectFeature statefulReconnectFeature = connection.Features.Get<IStatefulReconnectFeature>();
			try
			{
				int num = _protocol.Version;
				if (statefulReconnectFeature == null && _protocol.IsVersionSupported(1))
				{
					num = 1;
				}
				else if (_protocol.Version < 2 && statefulReconnectFeature != null)
				{
					Log.DisablingReconnect(_logger, _protocol.Name, _protocol.Version);
					statefulReconnectFeature.DisableReconnect();
				}
				Log.HubProtocol(_logger, _protocol.Name, num);
				await HandshakeAsync(startingConnectionState, num, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				Log.ErrorStartingConnection(_logger, ex);
				startingConnectionState.Cleanup();
				await CloseAsync(startingConnectionState.Connection).ConfigureAwait(continueOnCapturedContext: false);
				throw;
			}
			_state.CurrentConnectionStateUnsynchronized = startingConnectionState;
			if (!(connection.Features.Get<IConnectionInherentKeepAliveFeature>()?.HasInherentKeepAlive ?? false))
			{
				await SendHubMessage(startingConnectionState, PingMessage.Instance, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			startingConnectionState.ReceiveTask = ReceiveLoop(startingConnectionState);
			Log.Started(_logger);
		}

		private static ValueTask CloseAsync(ConnectionContext connection)
		{
			return connection.DisposeAsync();
		}

		private async Task StopAsyncCore(bool disposing)
		{
			_state.StopCts.Cancel();
			await _state.WaitConnectionLockAsync(default(CancellationToken), "StopAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 556).ConfigureAwait(continueOnCapturedContext: false);
			Task reconnectTask = _state.ReconnectTask;
			if (reconnectTask.Status != TaskStatus.RanToCompletion)
			{
				_state.ReleaseConnectionLock("StopAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 567);
				await reconnectTask.ConfigureAwait(continueOnCapturedContext: false);
				await _state.WaitConnectionLockAsync(default(CancellationToken), "StopAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 569).ConfigureAwait(continueOnCapturedContext: false);
			}
			Task connectionStateStopTask = Task.CompletedTask;
			try
			{
				if (disposing && _disposed)
				{
					return;
				}
				CheckDisposed();
				ConnectionState connectionState = _state.CurrentConnectionStateUnsynchronized;
				if (connectionState != null)
				{
					connectionState.Stopping = true;
					Task task = SendHubMessage(connectionState, CloseMessage.Empty);
					if (task.IsFaulted || task.IsCanceled || !task.IsCompleted)
					{
						task.ContinueWith((Task t) => t.Exception, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
					}
					connectionState.Connection.Features.Get<IStatefulReconnectFeature>()?.DisableReconnect();
				}
				else
				{
					_state.StopCts = new CancellationTokenSource();
				}
				if (disposing)
				{
					_disposed = true;
					IAsyncDisposable asyncDisposable = _serviceProvider as IAsyncDisposable;
					if (asyncDisposable != null)
					{
						await asyncDisposable.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						(_serviceProvider as IDisposable)?.Dispose();
					}
				}
				if (connectionState != null)
				{
					connectionStateStopTask = connectionState.StopAsync();
				}
			}
			finally
			{
				_state.ReleaseConnectionLock("StopAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 642);
			}
			await connectionStateStopTask.ConfigureAwait(continueOnCapturedContext: false);
		}

		public virtual IAsyncEnumerable<TResult> StreamAsyncCore<TResult>(string methodName, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
		{
			CancellationTokenSource cts = (cancellationToken.CanBeCanceled ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken) : new CancellationTokenSource());
			return AsyncEnumerableAdapters.MakeCancelableTypedAsyncEnumerable(CastIAsyncEnumerable<TResult>(methodName, args, cts), cts);
		}

		private async IAsyncEnumerable<T> CastIAsyncEnumerable<T>(string methodName, object[] args, CancellationTokenSource cts)
		{
			ChannelReader<object> reader = await StreamAsChannelCoreAsync(methodName, typeof(T), args, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				while (await reader.WaitToReadAsync(cts.Token).ConfigureAwait(continueOnCapturedContext: false))
				{
					object item;
					while (reader.TryRead(out item))
					{
						yield return (T)item;
					}
				}
			}
			finally
			{
				_ = reader.Completion.Exception;
			}
		}

		private async Task<ChannelReader<object>> StreamAsChannelCoreAsyncCore(string methodName, Type returnType, object[] args, CancellationToken cancellationToken)
		{
			CheckDisposed();
			ConnectionState connectionState = await _state.WaitForActiveConnectionAsync("StreamAsChannelCoreAsync", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				CheckDisposed();
				cancellationToken.ThrowIfCancellationRequested();
				List<string> streamIds;
				Dictionary<string, object> readers = PackageStreamingParams(connectionState, ref args, out streamIds);
				ChannelReader<object> channel;
				InvocationRequest irq2 = InvocationRequest.Stream(cancellationToken, returnType, connectionState.GetNextId(), _loggerFactory, this, out channel);
				await InvokeStreamCore(connectionState, methodName, irq2, args, streamIds?.ToArray(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (cancellationToken.CanBeCanceled)
				{
					cancellationToken.Register(delegate(object state)
					{
						OnStreamCanceled((InvocationRequest)state);
					}, irq2);
				}
				LaunchStreams(connectionState, readers, cancellationToken);
				return channel;
			}
			finally
			{
				_state.ReleaseConnectionLock("StreamAsChannelCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 746);
			}
			async Task OnStreamCanceled(InvocationRequest irq)
			{
				await _state.WaitConnectionLockAsync(default(CancellationToken), "StreamAsChannelCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 692).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					if (_state.CurrentConnectionStateUnsynchronized != null)
					{
						Log.SendingCancellation(_logger, irq.InvocationId);
						await SendHubMessage(_state.CurrentConnectionStateUnsynchronized, new CancelInvocationMessage(irq.InvocationId)).ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						Log.UnableToSendCancellation(_logger, irq.InvocationId);
					}
				}
				catch
				{
				}
				finally
				{
					_state.ReleaseConnectionLock("StreamAsChannelCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 713);
				}
				irq.Dispose();
			}
		}

		private Dictionary<string, object> PackageStreamingParams(ConnectionState connectionState, ref object[] args, out List<string> streamIds)
		{
			Dictionary<string, object> dictionary = null;
			streamIds = null;
			int num = args.Length;
			Span<bool> span = ((args.Length > 256) ? ((Span<bool>)new bool[args.Length]) : stackalloc bool[256].Slice(0, args.Length));
			Span<bool> span2 = span;
			for (int i = 0; i < args.Length; i++)
			{
				object obj = args[i];
				if (obj != null && ReflectionHelper.IsStreamingType(obj.GetType()))
				{
					span2[i] = true;
					num--;
					if (dictionary == null)
					{
						dictionary = new Dictionary<string, object>();
					}
					if (streamIds == null)
					{
						streamIds = new List<string>();
					}
					string nextId = connectionState.GetNextId();
					dictionary[nextId] = obj;
					streamIds.Add(nextId);
					Log.StartingStream(_logger, nextId);
				}
			}
			if (num == args.Length)
			{
				return null;
			}
			object[] array = ((num > 0) ? new object[num] : Array.Empty<object>());
			int num2 = 0;
			for (int j = 0; j < args.Length; j++)
			{
				if (!span2[j])
				{
					array[num2] = args[j];
					num2++;
				}
			}
			args = array;
			return dictionary;
		}

		private void LaunchStreams(ConnectionState connectionState, Dictionary<string, object> readers, CancellationToken cancellationToken)
		{
			if (readers == null)
			{
				return;
			}
			CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(connectionState.UploadStreamToken, cancellationToken);
			foreach (KeyValuePair<string, object> reader in readers)
			{
				object value = reader.Value;
				if (ReflectionHelper.IsIAsyncEnumerable(value.GetType()))
				{
					_sendIAsyncStreamItemsMethod.MakeGenericMethod(value.GetType().GetInterface("IAsyncEnumerable`1").GetGenericArguments()).Invoke(this, new object[4]
					{
						connectionState,
						reader.Key.ToString(),
						value,
						cancellationTokenSource
					});
				}
				else
				{
					_sendStreamItemsMethod.MakeGenericMethod(value.GetType().GetGenericArguments()).Invoke(this, new object[4]
					{
						connectionState,
						reader.Key.ToString(),
						value,
						cancellationTokenSource
					});
				}
			}
		}

		private Task SendStreamItems<T>(ConnectionState connectionState, string streamId, ChannelReader<T> reader, CancellationTokenSource tokenSource)
		{
			return CommonStreaming(connectionState, streamId, ReadChannelStream, tokenSource);
			async Task ReadChannelStream()
			{
				while (await reader.WaitToReadAsync(tokenSource.Token).ConfigureAwait(continueOnCapturedContext: false))
				{
					T item;
					while (!tokenSource.Token.IsCancellationRequested && reader.TryRead(out item))
					{
						await SendWithLock(connectionState, new StreamItemMessage(streamId, item), tokenSource.Token, "SendStreamItems").ConfigureAwait(continueOnCapturedContext: false);
						Log.SendingStreamItem(_logger, streamId);
					}
				}
			}
		}

		private Task SendIAsyncEnumerableStreamItems<T>(ConnectionState connectionState, string streamId, IAsyncEnumerable<T> stream, CancellationTokenSource tokenSource)
		{
			return CommonStreaming(connectionState, streamId, ReadAsyncEnumerableStream, tokenSource);
			async Task ReadAsyncEnumerableStream()
			{
				IAsyncEnumerable<T> asyncEnumerable = AsyncEnumerableAdapters.MakeCancelableTypedAsyncEnumerable(stream, tokenSource);
				await foreach (T item in asyncEnumerable)
				{
					await SendWithLock(connectionState, new StreamItemMessage(streamId, item), tokenSource.Token, "SendIAsyncEnumerableStreamItems").ConfigureAwait(continueOnCapturedContext: false);
					Log.SendingStreamItem(_logger, streamId);
				}
			}
		}

		private async Task CommonStreaming(ConnectionState connectionState, string streamId, Func<Task> createAndConsumeStream, CancellationTokenSource cts)
		{
			using (cts)
			{
				Log.StartingStream(_logger, streamId);
				string responseError = null;
				try
				{
					await createAndConsumeStream().ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException)
				{
					Log.CancelingStream(_logger, streamId);
					responseError = "Stream canceled by client.";
				}
				catch (Exception ex2)
				{
					Log.ErroredStream(_logger, streamId, ex2);
					responseError = $"Stream errored by client: '{ex2}'";
				}
				await _state.WaitConnectionLockAsync(default(CancellationToken), "CommonStreaming", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 900).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					if (_state.IsConnectionActive())
					{
						Log.CompletingStream(_logger, streamId);
						await SendHubMessage(connectionState, CompletionMessage.WithError(streamId, responseError)).ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						Log.CompletingStreamNotSent(_logger, streamId);
					}
				}
				catch (Exception exception)
				{
					Log.ErrorSendingStreamCompletion(_logger, streamId, exception);
				}
				finally
				{
					_state.ReleaseConnectionLock("CommonStreaming", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 920);
				}
			}
		}

		private async Task<object> InvokeCoreAsyncCore(string methodName, Type returnType, object[] args, CancellationToken cancellationToken)
		{
			CheckDisposed();
			ConnectionState connectionState = await _state.WaitForActiveConnectionAsync("InvokeCoreAsync", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Task<object> invocationTask;
			try
			{
				CheckDisposed();
				List<string> streamIds;
				Dictionary<string, object> readers = PackageStreamingParams(connectionState, ref args, out streamIds);
				InvocationRequest irq = InvocationRequest.Invoke(cancellationToken, returnType, connectionState.GetNextId(), _loggerFactory, this, out invocationTask);
				await InvokeCore(connectionState, methodName, irq, args, streamIds?.ToArray(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				LaunchStreams(connectionState, readers, cancellationToken);
			}
			finally
			{
				_state.ReleaseConnectionLock("InvokeCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 945);
			}
			return await invocationTask.ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task InvokeCore(ConnectionState connectionState, string methodName, InvocationRequest irq, object[] args, string[] streams, CancellationToken cancellationToken)
		{
			Log.PreparingBlockingInvocation(_logger, irq.InvocationId, methodName, irq.ResultType.FullName, args.Length);
			InvocationMessage hubMessage = new InvocationMessage(irq.InvocationId, methodName, args, streams);
			Log.RegisteringInvocation(_logger, irq.InvocationId);
			connectionState.AddInvocation(irq);
			Log.IssuingInvocation(_logger, irq.InvocationId, irq.ResultType.FullName, methodName, args);
			try
			{
				await SendHubMessage(connectionState, hubMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception exception)
			{
				Log.FailedToSendInvocation(_logger, irq.InvocationId, exception);
				connectionState.TryRemoveInvocation(irq.InvocationId, out var _);
				irq.Fail(exception);
			}
		}

		private async Task InvokeStreamCore(ConnectionState connectionState, string methodName, InvocationRequest irq, object[] args, string[] streams, CancellationToken cancellationToken)
		{
			Log.PreparingStreamingInvocation(_logger, irq.InvocationId, methodName, irq.ResultType.FullName, args.Length);
			StreamInvocationMessage hubMessage = new StreamInvocationMessage(irq.InvocationId, methodName, args, streams);
			Log.RegisteringInvocation(_logger, irq.InvocationId);
			connectionState.AddInvocation(irq);
			Log.IssuingInvocation(_logger, irq.InvocationId, irq.ResultType.FullName, methodName, args);
			try
			{
				await SendHubMessage(connectionState, hubMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception exception)
			{
				Log.FailedToSendInvocation(_logger, irq.InvocationId, exception);
				connectionState.TryRemoveInvocation(irq.InvocationId, out var _);
				irq.Fail(exception);
			}
		}

		private async Task SendHubMessage(ConnectionState connectionState, HubMessage hubMessage, CancellationToken cancellationToken = default(CancellationToken))
		{
			Log.SendingMessage(_logger, hubMessage);
			if (connectionState.UsingAcks())
			{
				await connectionState.WriteAsync(hubMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				_protocol.WriteMessage(hubMessage, connectionState.Connection.Transport.Output);
				await connectionState.Connection.Transport.Output.FlushAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			Log.MessageSent(_logger, hubMessage);
			connectionState.ResetSendPing();
		}

		private async Task SendCoreAsyncCore(string methodName, object[] args, CancellationToken cancellationToken)
		{
			CheckDisposed();
			ConnectionState connectionState = await _state.WaitForActiveConnectionAsync("SendCoreAsync", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				CheckDisposed();
				List<string> streamIds;
				Dictionary<string, object> readers = PackageStreamingParams(connectionState, ref args, out streamIds);
				Log.PreparingNonBlockingInvocation(_logger, methodName, args.Length);
				InvocationMessage hubMessage = new InvocationMessage(null, methodName, args, streamIds?.ToArray());
				await SendHubMessage(connectionState, hubMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				LaunchStreams(connectionState, readers, cancellationToken);
			}
			finally
			{
				_state.ReleaseConnectionLock("SendCoreAsyncCore", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1046);
			}
		}

		private async Task SendWithLock(ConnectionState expectedConnectionState, HubMessage message, CancellationToken cancellationToken, [CallerMemberName] string callerName = "")
		{
			CheckDisposed();
			ConnectionState connectionState = await _state.WaitForActiveConnectionAsync(callerName, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				CheckDisposed();
				await SendHubMessage(connectionState, message, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				_state.ReleaseConnectionLock("SendWithLock", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1064);
			}
		}

		private async Task<CloseMessage> ProcessMessagesAsync(HubMessage message, ConnectionState connectionState, ChannelWriter<InvocationMessage> invocationMessageWriter)
		{
			Log.ResettingKeepAliveTimer(_logger);
			connectionState.ResetTimeout();
			if (!connectionState.ShouldProcessMessage(message))
			{
				return null;
			}
			InvocationBindingFailureMessage invocationBindingFailureMessage = message as InvocationBindingFailureMessage;
			if (invocationBindingFailureMessage == null)
			{
				InvocationMessage invocationMessage = message as InvocationMessage;
				if (invocationMessage == null)
				{
					CompletionMessage completionMessage = message as CompletionMessage;
					InvocationRequest irq;
					if (completionMessage == null)
					{
						StreamItemMessage streamItemMessage = message as StreamItemMessage;
						if (streamItemMessage == null)
						{
							CloseMessage closeMessage = message as CloseMessage;
							if (closeMessage != null)
							{
								if (string.IsNullOrEmpty(closeMessage.Error))
								{
									Log.ReceivedClose(_logger);
								}
								else
								{
									Log.ReceivedCloseWithError(_logger, closeMessage.Error);
								}
								connectionState.Connection.Features.Get<IStatefulReconnectFeature>()?.DisableReconnect();
								return closeMessage;
							}
							if (!(message is PingMessage))
							{
								AckMessage ackMessage = message as AckMessage;
								if (ackMessage == null)
								{
									SequenceMessage sequenceMessage = message as SequenceMessage;
									if (sequenceMessage == null)
									{
										throw new InvalidOperationException("Unexpected message type: " + message.GetType().FullName);
									}
									Log.ReceivedSequenceMessage(_logger, sequenceMessage.SequenceId);
								}
								else
								{
									Log.ReceivedAckMessage(_logger, ackMessage.SequenceId);
									await connectionState.AckAsync(ackMessage).ConfigureAwait(continueOnCapturedContext: false);
								}
							}
							else
							{
								Log.ReceivedPing(_logger);
							}
						}
						else if (connectionState.TryGetInvocation(streamItemMessage.InvocationId, out irq))
						{
							await DispatchInvocationStreamItemAsync(streamItemMessage, irq).ConfigureAwait(continueOnCapturedContext: false);
						}
						else
						{
							Log.DroppedStreamMessage(_logger, streamItemMessage.InvocationId);
						}
					}
					else if (!connectionState.TryRemoveInvocation(completionMessage.InvocationId, out irq))
					{
						Log.DroppedCompletionMessage(_logger, completionMessage.InvocationId);
					}
					else
					{
						DispatchInvocationCompletion(completionMessage, irq);
						irq.Dispose();
					}
				}
				else
				{
					Log.ReceivedInvocation(_logger, invocationMessage.InvocationId, invocationMessage.Target, invocationMessage.Arguments);
					await invocationMessageWriter.WriteAsync(invocationMessage).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			else
			{
				Log.ArgumentBindingFailure(_logger, invocationBindingFailureMessage.InvocationId, invocationBindingFailureMessage.Target, invocationBindingFailureMessage.BindingFailure.SourceException);
				if (!string.IsNullOrEmpty(invocationBindingFailureMessage.InvocationId))
				{
					await SendWithLock(connectionState, CompletionMessage.WithError(invocationBindingFailureMessage.InvocationId, "Client failed to parse argument(s)."), default(CancellationToken), "ProcessMessagesAsync").ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			return null;
		}

		private async Task DispatchInvocationAsync(InvocationMessage invocation, ConnectionState connectionState)
		{
			bool expectsResult = !string.IsNullOrEmpty(invocation.InvocationId);
			if (!_handlers.TryGetValue(invocation.Target, out var value))
			{
				if (expectsResult)
				{
					Log.MissingResultHandler(_logger, invocation.Target);
					try
					{
						await SendWithLock(connectionState, CompletionMessage.WithError(invocation.InvocationId, "Client didn't provide a result."), default(CancellationToken), "DispatchInvocationAsync").ConfigureAwait(continueOnCapturedContext: false);
					}
					catch (Exception exception)
					{
						Log.ErrorSendingInvocationResult(_logger, invocation.InvocationId, invocation.Target, exception);
					}
				}
				else
				{
					Log.MissingHandler(_logger, invocation.Target);
				}
				return;
			}
			InvocationHandler[] handlers = value.GetHandlers();
			object result = null;
			Exception resultException = null;
			bool hasResult = false;
			InvocationHandler[] array = handlers;
			for (int i = 0; i < array.Length; i++)
			{
				InvocationHandler handler = array[i];
				try
				{
					Task task = handler.InvokeAsync(invocation.Arguments);
					if (!handler.HasResult)
					{
						goto IL_02b8;
					}
					Task<object> task2 = task as Task<object>;
					if (task2 == null)
					{
						goto IL_02b8;
					}
					result = await task2.ConfigureAwait(continueOnCapturedContext: false);
					hasResult = true;
					goto end_IL_01af;
					IL_02b8:
					await task.ConfigureAwait(continueOnCapturedContext: false);
					end_IL_01af:;
				}
				catch (Exception ex)
				{
					Log.ErrorInvokingClientSideMethod(_logger, invocation.Target, ex);
					if (handler.HasResult)
					{
						resultException = ex;
					}
				}
			}
			if (expectsResult)
			{
				try
				{
					if (resultException != null)
					{
						await SendWithLock(connectionState, CompletionMessage.WithError(invocation.InvocationId, resultException.Message), default(CancellationToken), "DispatchInvocationAsync").ConfigureAwait(continueOnCapturedContext: false);
						return;
					}
					if (hasResult)
					{
						await SendWithLock(connectionState, CompletionMessage.WithResult(invocation.InvocationId, result), default(CancellationToken), "DispatchInvocationAsync").ConfigureAwait(continueOnCapturedContext: false);
						return;
					}
					Log.MissingResultHandler(_logger, invocation.Target);
					await SendWithLock(connectionState, CompletionMessage.WithError(invocation.InvocationId, "Client didn't provide a result."), default(CancellationToken), "DispatchInvocationAsync").ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception exception2)
				{
					Log.ErrorSendingInvocationResult(_logger, invocation.InvocationId, invocation.Target, exception2);
				}
			}
			else if (hasResult)
			{
				Log.ResultNotExpected(_logger, invocation.Target);
			}
		}

		private async Task DispatchInvocationStreamItemAsync(StreamItemMessage streamItem, InvocationRequest irq)
		{
			Log.ReceivedStreamItem(_logger, irq.InvocationId);
			if (irq.CancellationToken.IsCancellationRequested)
			{
				Log.CancelingStreamItem(_logger, irq.InvocationId);
			}
			else if (!(await irq.StreamItem(streamItem.Item).ConfigureAwait(continueOnCapturedContext: false)))
			{
				Log.ReceivedStreamItemAfterClose(_logger, irq.InvocationId);
			}
		}

		private void DispatchInvocationCompletion(CompletionMessage completion, InvocationRequest irq)
		{
			Log.ReceivedInvocationCompletion(_logger, irq.InvocationId);
			if (irq.CancellationToken.IsCancellationRequested)
			{
				Log.CancelingInvocationCompletion(_logger, irq.InvocationId);
			}
			else
			{
				irq.Complete(completion);
			}
		}

		private void CheckDisposed()
		{
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EObjectDisposedThrowHelper.ThrowIf(_disposed, this);
		}

		private async Task HandshakeAsync(ConnectionState startingConnectionState, int protocolVersion, CancellationToken cancellationToken)
		{
			Log.SendingHubHandshake(_logger);
			HandshakeProtocol.WriteRequestMessage(new HandshakeRequestMessage(_protocol.Name, protocolVersion), startingConnectionState.Connection.Transport.Output);
			if ((await startingConnectionState.Connection.Transport.Output.FlushAsync(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false)).IsCompleted)
			{
				IOException ex = new IOException("The server disconnected before the handshake could be started.");
				Log.ErrorReceivingHandshakeResponse(_logger, ex);
				throw ex;
			}
			PipeReader input = startingConnectionState.Connection.Transport.Input;
			using CancellationTokenSource handshakeCts = new CancellationTokenSource(HandshakeTimeout);
			_ = 1;
			try
			{
				CancellationToken linkedToken;
				using (CancellationTokenUtils.CreateLinkedToken(cancellationToken, handshakeCts.Token, out linkedToken))
				{
					while (true)
					{
						ReadResult readResult = await input.ReadAsync(linkedToken).ConfigureAwait(continueOnCapturedContext: false);
						ReadOnlySequence<byte> buffer = readResult.Buffer;
						SequencePosition start = buffer.Start;
						SequencePosition examined = buffer.End;
						try
						{
							if (!buffer.IsEmpty && HandshakeProtocol.TryParseResponseMessage(ref buffer, out var responseMessage))
							{
								start = buffer.Start;
								examined = start;
								if (responseMessage.Error != null)
								{
									Log.HandshakeServerError(_logger, responseMessage.Error);
									throw new HubException("Unable to complete handshake with the server due to an error: " + responseMessage.Error);
								}
								Log.HandshakeComplete(_logger);
								break;
							}
							if (readResult.IsCompleted)
							{
								throw new InvalidOperationException("The server disconnected before sending a handshake response");
							}
							continue;
						}
						finally
						{
							input.AdvanceTo(start, examined);
						}
					}
				}
			}
			catch (HubException)
			{
				throw;
			}
			catch (InvalidDataException exception)
			{
				Log.ErrorInvalidHandshakeResponse(_logger, exception);
				throw;
			}
			catch (OperationCanceledException exception2)
			{
				if (handshakeCts.IsCancellationRequested)
				{
					Log.ErrorHandshakeTimedOut(_logger, HandshakeTimeout, exception2);
				}
				else
				{
					Log.ErrorHandshakeCanceled(_logger, exception2);
				}
				throw;
			}
			catch (Exception exception3)
			{
				Log.ErrorReceivingHandshakeResponse(_logger, exception3);
				throw;
			}
		}

		private async Task ReceiveLoop(ConnectionState connectionState)
		{
			Log.ReceiveLoopStarting(_logger);
			TimerAwaitable timer = new TimerAwaitable(TickRate, TickRate);
			Task timerTask = connectionState.TimerLoop(timer);
			CancellationTokenSource uploadStreamSource = new CancellationTokenSource();
			connectionState.UploadStreamToken = uploadStreamSource.Token;
			Channel<InvocationMessage> invocationMessageChannel = Channel.CreateUnbounded<InvocationMessage>(_receiveLoopOptions);
			connectionState.InvocationMessageReceiveTask = StartProcessingInvocationMessages(invocationMessageChannel.Reader);
			PipeReader input = connectionState.Connection.Transport.Input;
			try
			{
				_ = 1;
				try
				{
					while (true)
					{
						ReadResult result = await input.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
						ReadOnlySequence<byte> buffer = result.Buffer;
						try
						{
							if (result.IsCanceled)
							{
								break;
							}
							if (buffer.IsEmpty)
							{
								goto IL_02f8;
							}
							Log.ProcessingMessage(_logger, buffer.Length);
							CloseMessage closeMessage = null;
							HubMessage message;
							while (_protocol.TryParseMessage(ref buffer, connectionState, out message))
							{
								closeMessage = await ProcessMessagesAsync(message, connectionState, invocationMessageChannel.Writer).ConfigureAwait(continueOnCapturedContext: false);
								if (closeMessage != null)
								{
									if (closeMessage.Error != null)
									{
										connectionState.CloseException = new HubException("The server closed the connection with the following error: " + closeMessage.Error);
									}
									if (!closeMessage.AllowReconnect)
									{
										connectionState.Stopping = true;
									}
									break;
								}
							}
							if (closeMessage == null)
							{
								goto IL_02f8;
							}
							goto end_IL_01c2;
							IL_02f8:
							if (result.IsCompleted)
							{
								if (buffer.IsEmpty)
								{
									break;
								}
								throw new InvalidDataException("Connection terminated while reading a message.");
							}
							continue;
							end_IL_01c2:;
						}
						finally
						{
							input.AdvanceTo(buffer.Start, buffer.End);
						}
						break;
					}
				}
				catch (Exception ex)
				{
					Log.ServerDisconnectedWithError(_logger, ex);
					connectionState.CloseException = ex;
				}
			}
			finally
			{
				invocationMessageChannel.Writer.TryComplete();
				timer.Stop();
				await timerTask.ConfigureAwait(continueOnCapturedContext: false);
				uploadStreamSource.Cancel();
				await HandleConnectionClose(connectionState).ConfigureAwait(continueOnCapturedContext: false);
			}
			async Task StartProcessingInvocationMessages(ChannelReader<InvocationMessage> invocationMessageChannelReader)
			{
				while (await invocationMessageChannelReader.WaitToReadAsync().ConfigureAwait(continueOnCapturedContext: false))
				{
					InvocationMessage item;
					while (invocationMessageChannelReader.TryRead(out item))
					{
						Task task = DispatchInvocationAsync(item, connectionState);
						if (string.IsNullOrEmpty(item.InvocationId))
						{
							await task.ConfigureAwait(continueOnCapturedContext: false);
						}
					}
				}
			}
		}

		internal Task RunTimerActions()
		{
			return _state.CurrentConnectionStateUnsynchronized.RunTimerActions();
		}

		internal void OnServerTimeout()
		{
			_state.CurrentConnectionStateUnsynchronized.OnServerTimeout();
		}

		private async Task HandleConnectionClose(ConnectionState connectionState)
		{
			await _state.WaitConnectionLockAsync(default(CancellationToken), "HandleConnectionClose", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1513).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				_state.CurrentConnectionStateUnsynchronized = null;
				await CloseAsync(connectionState.Connection).ConfigureAwait(continueOnCapturedContext: false);
				connectionState.CancelOutstandingInvocations(connectionState.CloseException);
				connectionState.Cleanup();
				if (connectionState.Stopping || _reconnectPolicy == null)
				{
					if (connectionState.CloseException != null)
					{
						Log.ShutdownWithError(_logger, connectionState.CloseException);
					}
					else
					{
						Log.ShutdownConnection(_logger);
					}
					_state.ChangeState(HubConnectionState.Connected, HubConnectionState.Disconnected);
					CompleteClose(connectionState.CloseException);
				}
				else
				{
					_state.ReconnectTask = ReconnectAsync(connectionState.CloseException);
				}
			}
			finally
			{
				_state.ReleaseConnectionLock("HandleConnectionClose", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1548);
			}
		}

		private void CompleteClose(Exception closeException)
		{
			_state.StopCts = new CancellationTokenSource();
			RunCloseEvent(closeException);
		}

		private void RunCloseEvent(Exception closeException)
		{
			Func<Exception, Task> closed = this.Closed;
			if (closed != null)
			{
				RunClosedEventAsync();
			}
			async Task RunClosedEventAsync()
			{
				await AwaitableThreadPool.Yield();
				try
				{
					Log.InvokingClosedEventHandler(_logger);
					await closed(closeException).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception exception)
				{
					Log.ErrorDuringClosedEvent(_logger, exception);
				}
			}
		}

		private async Task ReconnectAsync(Exception closeException)
		{
			int previousReconnectAttempts = 0;
			DateTime reconnectStartTime = DateTime.UtcNow;
			Exception retryReason = closeException;
			TimeSpan? nextRetryDelay = GetNextRetryDelay(previousReconnectAttempts, TimeSpan.Zero, retryReason);
			if (!nextRetryDelay.HasValue)
			{
				Log.FirstReconnectRetryDelayNull(_logger);
				_state.ChangeState(HubConnectionState.Connected, HubConnectionState.Disconnected);
				CompleteClose(closeException);
				return;
			}
			_state.ChangeState(HubConnectionState.Connected, HubConnectionState.Reconnecting);
			if (closeException != null)
			{
				Log.ReconnectingWithError(_logger, closeException);
			}
			else
			{
				Log.Reconnecting(_logger);
			}
			RunReconnectingEvent(closeException);
			while (nextRetryDelay.HasValue)
			{
				Log.AwaitingReconnectRetryDelay(_logger, previousReconnectAttempts + 1, nextRetryDelay.Value);
				try
				{
					await Task.Delay(nextRetryDelay.Value, _state.StopCts.Token).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException ex)
				{
					Log.ReconnectingStoppedDuringRetryDelay(_logger);
					await _state.WaitConnectionLockAsync(default(CancellationToken), "ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1632).ConfigureAwait(continueOnCapturedContext: false);
					try
					{
						_state.ChangeState(HubConnectionState.Reconnecting, HubConnectionState.Disconnected);
						CompleteClose(GetOperationCanceledException("Connection stopped during reconnect delay. Done reconnecting.", ex, _state.StopCts.Token));
					}
					finally
					{
						_state.ReleaseConnectionLock("ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1641);
					}
					return;
				}
				await _state.WaitConnectionLockAsync(default(CancellationToken), "ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1647).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					await StartAsyncCore(_state.StopCts.Token).ConfigureAwait(continueOnCapturedContext: false);
					Log.Reconnected(_logger, previousReconnectAttempts, DateTime.UtcNow - reconnectStartTime);
					_state.ChangeState(HubConnectionState.Reconnecting, HubConnectionState.Connected);
					RunReconnectedEvent();
					return;
				}
				catch (Exception ex2)
				{
					retryReason = ex2;
					Log.ReconnectAttemptFailed(_logger, ex2);
					if (_state.StopCts.IsCancellationRequested)
					{
						Log.ReconnectingStoppedDuringReconnectAttempt(_logger);
						_state.ChangeState(HubConnectionState.Reconnecting, HubConnectionState.Disconnected);
						CompleteClose(GetOperationCanceledException("Connection stopped during reconnect attempt. Done reconnecting.", ex2, _state.StopCts.Token));
						return;
					}
					previousReconnectAttempts++;
				}
				finally
				{
					_state.ReleaseConnectionLock("ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1682);
				}
				nextRetryDelay = GetNextRetryDelay(previousReconnectAttempts, DateTime.UtcNow - reconnectStartTime, retryReason);
			}
			await _state.WaitConnectionLockAsync(default(CancellationToken), "ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1688).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				TimeSpan timeSpan = DateTime.UtcNow - reconnectStartTime;
				Log.ReconnectAttemptsExhausted(_logger, previousReconnectAttempts, timeSpan);
				_state.ChangeState(HubConnectionState.Reconnecting, HubConnectionState.Disconnected);
				string message = $"Reconnect retries have been exhausted after {previousReconnectAttempts} failed attempts and {timeSpan} elapsed. Disconnecting.";
				CompleteClose(new OperationCanceledException(message));
			}
			finally
			{
				_state.ReleaseConnectionLock("ReconnectAsync", "/_/src/SignalR/clients/csharp/Client.Core/src/HubConnection.cs", 1704);
			}
		}

		private TimeSpan? GetNextRetryDelay(long previousRetryCount, TimeSpan elapsedTime, Exception retryReason)
		{
			try
			{
				return _reconnectPolicy.NextRetryDelay(new RetryContext
				{
					PreviousRetryCount = previousRetryCount,
					ElapsedTime = elapsedTime,
					RetryReason = retryReason
				});
			}
			catch (Exception exception)
			{
				Log.ErrorDuringNextRetryDelay(_logger, exception);
				return null;
			}
		}

		private OperationCanceledException GetOperationCanceledException(string message, Exception innerException, CancellationToken cancellationToken)
		{
			return new OperationCanceledException(message, innerException);
		}

		private void RunReconnectingEvent(Exception closeException)
		{
			Func<Exception, Task> reconnecting = this.Reconnecting;
			if (reconnecting != null)
			{
				RunReconnectingEventAsync();
			}
			async Task RunReconnectingEventAsync()
			{
				await AwaitableThreadPool.Yield();
				try
				{
					await reconnecting(closeException).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception exception)
				{
					Log.ErrorDuringReconnectingEvent(_logger, exception);
				}
			}
		}

		private void RunReconnectedEvent()
		{
			Func<string, Task> reconnected = this.Reconnected;
			if (reconnected != null)
			{
				RunReconnectedEventAsync();
			}
			async Task RunReconnectedEventAsync()
			{
				await AwaitableThreadPool.Yield();
				try
				{
					await reconnected(ConnectionId).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception exception)
				{
					Log.ErrorDuringReconnectedEvent(_logger, exception);
				}
			}
		}

		[Conditional("DEBUG")]
		private static void SafeAssert(bool condition, string message, [CallerMemberName] string memberName = null, [CallerFilePath] string fileName = null, [CallerLineNumber] int lineNumber = 0)
		{
			if (!condition)
			{
				throw new InvalidOperationException($"Assertion failed in {memberName}, at {fileName}:{lineNumber}: {message}");
			}
		}
	}
}
