using System;
using System.Buffers;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Connections.Abstractions;
using Microsoft.AspNetCore.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal sealed class WebSocketsTransport : ITransport, IDuplexPipe, IStatefulReconnectFeature
	{
		private static class Log
		{
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, TransferFormat, Uri, Exception> __StartTransportCallback = LoggerMessage.Define<TransferFormat, Uri>(LogLevel.Information, new EventId(1, "StartTransport"), "Starting transport. Transfer mode: {TransferFormat}. Url: '{WebSocketUrl}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __TransportStoppedCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(2, "TransportStopped"), "Transport stopped.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __StartReceiveCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(3, "StartReceive"), "Starting receive loop.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __TransportStoppingCallback = LoggerMessage.Define(LogLevel.Information, new EventId(6, "TransportStopping"), "Transport is stopping.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, int, Exception> __MessageToAppCallback = LoggerMessage.Define<int>(LogLevel.Debug, new EventId(10, "MessageToApp"), "Passing message to application. Payload size: {Count}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReceiveCanceledCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(5, "ReceiveCanceled"), "Receive loop canceled.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReceiveStoppedCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "ReceiveStopped"), "Receive loop stopped.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SendStartedCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(7, "SendStarted"), "Starting the send loop.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SendCanceledCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(9, "SendCanceled"), "Send loop canceled.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SendStoppedCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(8, "SendStopped"), "Send loop stopped.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, WebSocketCloseStatus?, Exception> __WebSocketClosedCallback = LoggerMessage.Define<WebSocketCloseStatus?>(LogLevel.Information, new EventId(11, "WebSocketClosed"), "WebSocket closed by the server. Close status {CloseStatus}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, WebSocketMessageType, int, bool, Exception> __MessageReceivedCallback = LoggerMessage.Define<WebSocketMessageType, int, bool>(LogLevel.Debug, new EventId(12, "MessageReceived"), "Message received. Type: {MessageType}, size: {Count}, EndOfMessage: {EndOfMessage}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, long, Exception> __ReceivedFromAppCallback = LoggerMessage.Define<long>(LogLevel.Debug, new EventId(13, "ReceivedFromApp"), "Received message from application. Payload size: {Count}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SendMessageCanceledCallback = LoggerMessage.Define(LogLevel.Information, new EventId(14, "SendMessageCanceled"), "Sending a message canceled.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ErrorSendingMessageCallback = LoggerMessage.Define(LogLevel.Error, new EventId(15, "ErrorSendingMessage"), "Error while sending a message.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ClosingWebSocketCallback = LoggerMessage.Define(LogLevel.Information, new EventId(16, "ClosingWebSocket"), "Closing WebSocket.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ClosingWebSocketFailedCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(17, "ClosingWebSocketFailed"), "Closing webSocket failed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __CancelMessageCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(18, "CancelMessage"), "Canceled passing message to application.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __StartedTransportCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(19, "StartedTransport"), "Started transport.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __HeadersNotSupportedCallback = LoggerMessage.Define(LogLevel.Warning, new EventId(20, "HeadersNotSupported"), "Configuring request headers using HttpConnectionOptions.Headers is not supported when using websockets transport on the browser platform.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReceiveErroredCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(21, "ReceiveErrored"), "Receive loop errored.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SendErroredCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(22, "SendErrored"), "Send loop errored.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[LoggerMessage(1, LogLevel.Information, "Starting transport. Transfer mode: {TransferFormat}. Url: '{WebSocketUrl}'.", EventName = "StartTransport")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void StartTransport(ILogger logger, TransferFormat transferFormat, Uri webSocketUrl)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__StartTransportCallback(logger, transferFormat, webSocketUrl, null);
				}
			}

			[LoggerMessage(2, LogLevel.Debug, "Transport stopped.", EventName = "TransportStopped")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void TransportStopped(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__TransportStoppedCallback(logger, exception);
				}
			}

			[LoggerMessage(3, LogLevel.Debug, "Starting receive loop.", EventName = "StartReceive")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void StartReceive(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__StartReceiveCallback(logger, null);
				}
			}

			[LoggerMessage(6, LogLevel.Information, "Transport is stopping.", EventName = "TransportStopping")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void TransportStopping(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__TransportStoppingCallback(logger, null);
				}
			}

			[LoggerMessage(10, LogLevel.Debug, "Passing message to application. Payload size: {Count}.", EventName = "MessageToApp")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void MessageToApp(ILogger logger, int count)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__MessageToAppCallback(logger, count, null);
				}
			}

			[LoggerMessage(5, LogLevel.Debug, "Receive loop canceled.", EventName = "ReceiveCanceled")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceiveCanceled(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ReceiveCanceledCallback(logger, null);
				}
			}

			[LoggerMessage(4, LogLevel.Debug, "Receive loop stopped.", EventName = "ReceiveStopped")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceiveStopped(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ReceiveStoppedCallback(logger, null);
				}
			}

			[LoggerMessage(7, LogLevel.Debug, "Starting the send loop.", EventName = "SendStarted")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendStarted(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendStartedCallback(logger, null);
				}
			}

			[LoggerMessage(9, LogLevel.Debug, "Send loop canceled.", EventName = "SendCanceled")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendCanceled(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendCanceledCallback(logger, null);
				}
			}

			[LoggerMessage(8, LogLevel.Debug, "Send loop stopped.", EventName = "SendStopped")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendStopped(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendStoppedCallback(logger, null);
				}
			}

			[LoggerMessage(11, LogLevel.Information, "WebSocket closed by the server. Close status {CloseStatus}.", EventName = "WebSocketClosed")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void WebSocketClosed(ILogger logger, WebSocketCloseStatus? closeStatus)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__WebSocketClosedCallback(logger, closeStatus, null);
				}
			}

			[LoggerMessage(12, LogLevel.Debug, "Message received. Type: {MessageType}, size: {Count}, EndOfMessage: {EndOfMessage}.", EventName = "MessageReceived")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void MessageReceived(ILogger logger, WebSocketMessageType messageType, int count, bool endOfMessage)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__MessageReceivedCallback(logger, messageType, count, endOfMessage, null);
				}
			}

			[LoggerMessage(13, LogLevel.Debug, "Received message from application. Payload size: {Count}.", EventName = "ReceivedFromApp")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedFromApp(ILogger logger, long count)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ReceivedFromAppCallback(logger, count, null);
				}
			}

			[LoggerMessage(14, LogLevel.Information, "Sending a message canceled.", EventName = "SendMessageCanceled")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendMessageCanceled(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__SendMessageCanceledCallback(logger, null);
				}
			}

			[LoggerMessage(15, LogLevel.Error, "Error while sending a message.", EventName = "ErrorSendingMessage")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorSendingMessage(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorSendingMessageCallback(logger, exception);
				}
			}

			[LoggerMessage(16, LogLevel.Information, "Closing WebSocket.", EventName = "ClosingWebSocket")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ClosingWebSocket(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__ClosingWebSocketCallback(logger, null);
				}
			}

			[LoggerMessage(17, LogLevel.Debug, "Closing webSocket failed.", EventName = "ClosingWebSocketFailed")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ClosingWebSocketFailed(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ClosingWebSocketFailedCallback(logger, exception);
				}
			}

			[LoggerMessage(18, LogLevel.Debug, "Canceled passing message to application.", EventName = "CancelMessage")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void CancelMessage(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__CancelMessageCallback(logger, null);
				}
			}

			[LoggerMessage(19, LogLevel.Debug, "Started transport.", EventName = "StartedTransport")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void StartedTransport(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__StartedTransportCallback(logger, null);
				}
			}

			[LoggerMessage(20, LogLevel.Warning, "Configuring request headers using HttpConnectionOptions.Headers is not supported when using websockets transport on the browser platform.", EventName = "HeadersNotSupported")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void HeadersNotSupported(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__HeadersNotSupportedCallback(logger, null);
				}
			}

			[LoggerMessage(21, LogLevel.Debug, "Receive loop errored.", EventName = "ReceiveErrored")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceiveErrored(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ReceiveErroredCallback(logger, exception);
				}
			}

			[LoggerMessage(22, LogLevel.Debug, "Send loop errored.", EventName = "SendErrored")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendErrored(ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendErroredCallback(logger, exception);
				}
			}
		}

		private WebSocket _webSocket;

		private IDuplexPipe _application;

		private WebSocketMessageType _webSocketMessageType;

		private readonly ILogger _logger;

		private readonly TimeSpan _closeTimeout;

		private volatile bool _aborted;

		private readonly HttpConnectionOptions _httpConnectionOptions;

		private readonly HttpClient _httpClient;

		private CancellationTokenSource _stopCts;

		private bool _useStatefulReconnect;

		private IDuplexPipe _transport;

		private bool _gracefulClose;

		private Func<PipeWriter, Task> _notifyOnReconnect;

		internal Task Running { get; private set; } = Task.CompletedTask;


		public PipeReader Input => _transport.Input;

		public PipeWriter Output => _transport.Output;

		public void OnReconnected(Func<PipeWriter, Task> notifyOnReconnect)
		{
			Func<PipeWriter, Task> notifyOnReconnect2 = notifyOnReconnect;
			if (_notifyOnReconnect == null)
			{
				_notifyOnReconnect = notifyOnReconnect2;
				return;
			}
			Func<PipeWriter, Task> localNotifyOnReconnect = _notifyOnReconnect;
			_notifyOnReconnect = async delegate(PipeWriter writer)
			{
				await localNotifyOnReconnect(writer).ConfigureAwait(continueOnCapturedContext: false);
				await notifyOnReconnect2(writer).ConfigureAwait(continueOnCapturedContext: false);
			};
		}

		public WebSocketsTransport(HttpConnectionOptions httpConnectionOptions, ILoggerFactory loggerFactory, Func<Task<string?>> accessTokenProvider, HttpClient? httpClient, bool useStatefulReconnect = false)
		{
			_useStatefulReconnect = useStatefulReconnect;
			_logger = (loggerFactory ?? NullLoggerFactory.Instance).CreateLogger<WebSocketsTransport>();
			_httpConnectionOptions = httpConnectionOptions ?? new HttpConnectionOptions();
			_closeTimeout = _httpConnectionOptions.CloseTimeout;
			_httpConnectionOptions.AccessTokenProvider = accessTokenProvider;
			_httpClient = httpClient;
		}

		private async ValueTask<WebSocket> DefaultWebSocketFactory(WebSocketConnectionContext context, CancellationToken cancellationToken)
		{
			ClientWebSocket webSocket = new ClientWebSocket();
			Uri url = context.Uri;
			bool flag = OperatingSystem.IsBrowser();
			if (!flag)
			{
				webSocket.Options.SetRequestHeader("X-SignalR-User-Agent", Constants.UserAgentHeader.ToString());
				webSocket.Options.SetRequestHeader("X-Requested-With", "XMLHttpRequest");
			}
			if (context.Options != null)
			{
				if (context.Options.Headers.Count > 0)
				{
					if (flag)
					{
						Log.HeadersNotSupported(_logger);
					}
					else
					{
						foreach (KeyValuePair<string, string> header in context.Options.Headers)
						{
							webSocket.Options.SetRequestHeader(header.Key, header.Value);
						}
					}
				}
				if (!flag)
				{
					if (context.Options.Cookies != null)
					{
						webSocket.Options.Cookies = context.Options.Cookies;
					}
					X509CertificateCollection clientCertificates = context.Options.ClientCertificates;
					if (clientCertificates != null && clientCertificates.Count > 0)
					{
						webSocket.Options.ClientCertificates.AddRange(context.Options.ClientCertificates);
					}
					if (context.Options.Credentials != null)
					{
						webSocket.Options.Credentials = context.Options.Credentials;
					}
					_ = webSocket.Options.Proxy;
					if (context.Options.Proxy != null)
					{
						webSocket.Options.Proxy = context.Options.Proxy;
					}
					if (context.Options.UseDefaultCredentials.HasValue)
					{
						webSocket.Options.UseDefaultCredentials = context.Options.UseDefaultCredentials.Value;
						_ = context.Options.UseDefaultCredentials.Value;
					}
					context.Options.WebSocketConfiguration?.Invoke(webSocket.Options);
				}
			}
			if (_httpConnectionOptions.AccessTokenProvider != null)
			{
				string text = await _httpConnectionOptions.AccessTokenProvider!().ConfigureAwait(continueOnCapturedContext: false);
				if (!string.IsNullOrWhiteSpace(text))
				{
					if (OperatingSystem.IsBrowser())
					{
						string text2 = UrlEncoder.Default.Encode(text);
						text2 = "access_token=" + text2;
						url = Utils.AppendQueryString(url, text2);
					}
					else
					{
						webSocket.Options.SetRequestHeader("Authorization", "Bearer " + text);
					}
				}
			}
			try
			{
				await webSocket.ConnectAsync(url, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				return webSocket;
			}
			catch
			{
				webSocket.Dispose();
				throw;
			}
		}

		public async Task StartAsync(Uri url, TransferFormat transferFormat, CancellationToken cancellationToken = default(CancellationToken))
		{
			_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EArgumentNullThrowHelper.ThrowIfNull(url, "url");
			if (transferFormat != TransferFormat.Binary && transferFormat != TransferFormat.Text)
			{
				throw new ArgumentException($"The '{transferFormat}' transfer format is not supported by this transport.", "transferFormat");
			}
			_webSocketMessageType = ((transferFormat == TransferFormat.Binary) ? WebSocketMessageType.Binary : WebSocketMessageType.Text);
			Uri uri = ResolveWebSocketsUrl(url);
			Log.StartTransport(_logger, transferFormat, uri);
			WebSocketConnectionContext arg = new WebSocketConnectionContext(uri, _httpConnectionOptions);
			_webSocket = await (_httpConnectionOptions.WebSocketFactory ?? new Func<WebSocketConnectionContext, CancellationToken, ValueTask<WebSocket>>(DefaultWebSocketFactory))!(arg, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (_webSocket == null)
			{
				throw new InvalidOperationException("Configured WebSocketFactory did not return a value.");
			}
			Log.StartedTransport(_logger);
			_stopCts = new CancellationTokenSource();
			bool isReconnect = false;
			if (_transport == null)
			{
				DuplexPipe.DuplexPipePair duplexPipePair = DuplexPipe.CreateConnectionPair(_httpConnectionOptions.TransportPipeOptions, _httpConnectionOptions.AppPipeOptions);
				_transport = duplexPipePair.Transport;
				_application = duplexPipePair.Application;
			}
			else
			{
				isReconnect = true;
			}
			Running = ProcessSocketAsync(_webSocket, url, isReconnect);
		}

		private async Task ProcessSocketAsync(WebSocket socket, Uri url, bool isReconnect)
		{
			using (socket)
			{
				Task receiving = StartReceiving(socket);
				Task sending = StartSending(socket, isReconnect);
				if (isReconnect)
				{
					await _notifyOnReconnect(_transport.Output).ConfigureAwait(continueOnCapturedContext: false);
				}
				Task obj = await Task.WhenAny(receiving, sending).ConfigureAwait(continueOnCapturedContext: false);
				_stopCts.CancelAfter(_closeTimeout);
				if (obj == receiving)
				{
					_application.Input.CancelPendingRead();
					if (await Task.WhenAny(sending, Task.Delay(_closeTimeout, _stopCts.Token)).ConfigureAwait(continueOnCapturedContext: false) != sending)
					{
						_aborted = true;
						socket.Abort();
						await sending.ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				else
				{
					_aborted = true;
					socket.Abort();
					if (_gracefulClose)
					{
						_application.Output.CancelPendingFlush();
					}
					await receiving.ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			bool cleanup = true;
			try
			{
				if (!_gracefulClose && UpdateConnectionPair())
				{
					try
					{
						await StartAsync(url, (_webSocketMessageType == WebSocketMessageType.Binary) ? TransferFormat.Binary : TransferFormat.Text).ConfigureAwait(continueOnCapturedContext: false);
						cleanup = false;
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException("Reconnect attempt failed.", innerException);
					}
				}
			}
			finally
			{
				if (cleanup)
				{
					_application.Output.Complete();
					_application.Input.Complete();
				}
			}
		}

		private async Task StartReceiving(WebSocket socket)
		{
			_ = 1;
			try
			{
				FlushResult flushResult;
				do
				{
					MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)_application.Output.GetMemory(), out ArraySegment<byte> segment);
					WebSocketReceiveResult webSocketReceiveResult = await socket.ReceiveAsync(segment, _stopCts.Token).ConfigureAwait(continueOnCapturedContext: false);
					if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
					{
						_gracefulClose = true;
						Log.WebSocketClosed(_logger, socket.CloseStatus);
						if (socket.CloseStatus != WebSocketCloseStatus.NormalClosure)
						{
							throw new InvalidOperationException($"Websocket closed with error: {socket.CloseStatus}.");
						}
						break;
					}
					Log.MessageReceived(_logger, webSocketReceiveResult.MessageType, webSocketReceiveResult.Count, webSocketReceiveResult.EndOfMessage);
					_application.Output.Advance(webSocketReceiveResult.Count);
					flushResult = await _application.Output.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				while (!flushResult.IsCanceled && !flushResult.IsCompleted);
			}
			catch (OperationCanceledException)
			{
				Log.ReceiveCanceled(_logger);
			}
			catch (Exception exception)
			{
				if (!_aborted)
				{
					if (_gracefulClose || !_useStatefulReconnect)
					{
						_application.Output.Complete(exception);
					}
					else
					{
						Log.ReceiveErrored(_logger, exception);
					}
				}
			}
			finally
			{
				if (_gracefulClose || !_useStatefulReconnect)
				{
					_application.Output.Complete();
				}
				Log.ReceiveStopped(_logger);
			}
		}

		private async Task StartSending(WebSocket socket, bool ignoreFirstCanceled)
		{
			Exception error = null;
			try
			{
				_ = 1;
				try
				{
					while (true)
					{
						ReadResult readResult = await _application.Input.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
						ReadOnlySequence<byte> buffer = readResult.Buffer;
						try
						{
							if (readResult.IsCanceled && !ignoreFirstCanceled)
							{
								break;
							}
							ignoreFirstCanceled = false;
							if (!buffer.IsEmpty)
							{
								try
								{
									Log.ReceivedFromApp(_logger, buffer.Length);
									if (WebSocketCanSend(socket))
									{
										await socket.SendAsync(buffer, _webSocketMessageType, _stopCts.Token).ConfigureAwait(continueOnCapturedContext: false);
										continue;
									}
								}
								catch (Exception exception)
								{
									if (!_aborted)
									{
										Log.ErrorSendingMessage(_logger, exception);
									}
								}
							}
							else if (!readResult.IsCompleted)
							{
								continue;
							}
						}
						finally
						{
							_application.Input.AdvanceTo(buffer.End);
						}
						break;
					}
				}
				catch (Exception ex)
				{
					error = ex;
				}
			}
			finally
			{
				if (WebSocketCanSend(socket))
				{
					try
					{
						if (OperatingSystem.IsBrowser())
						{
							await socket.CloseAsync((error != null) ? WebSocketCloseStatus.InternalServerError : WebSocketCloseStatus.NormalClosure, "", _stopCts.Token).ConfigureAwait(continueOnCapturedContext: false);
						}
						else
						{
							await socket.CloseOutputAsync((error != null) ? WebSocketCloseStatus.InternalServerError : WebSocketCloseStatus.NormalClosure, "", _stopCts.Token).ConfigureAwait(continueOnCapturedContext: false);
						}
					}
					catch (Exception exception2)
					{
						Log.ClosingWebSocketFailed(_logger, exception2);
					}
				}
				if (_gracefulClose || !_useStatefulReconnect)
				{
					_application.Input.Complete();
				}
				else if (error != null)
				{
					Log.SendErrored(_logger, error);
				}
				Log.SendStopped(_logger);
			}
		}

		private static bool WebSocketCanSend(WebSocket ws)
		{
			if (ws.State != WebSocketState.Aborted && ws.State != WebSocketState.Closed)
			{
				return ws.State != WebSocketState.CloseSent;
			}
			return false;
		}

		private static Uri ResolveWebSocketsUrl(Uri url)
		{
			UriBuilder uriBuilder = new UriBuilder(url);
			if (url.Scheme == "http")
			{
				uriBuilder.Scheme = "ws";
			}
			else if (url.Scheme == "https")
			{
				uriBuilder.Scheme = "wss";
			}
			return uriBuilder.Uri;
		}

		public async Task StopAsync()
		{
			_gracefulClose = true;
			Log.TransportStopping(_logger);
			if (_application == null)
			{
				return;
			}
			_transport.Output.Complete();
			_transport.Input.Complete();
			_application.Input.CancelPendingRead();
			_stopCts.CancelAfter(_closeTimeout);
			try
			{
				await Running.ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception exception)
			{
				Log.TransportStopped(_logger, exception);
				return;
			}
			finally
			{
				_webSocket?.Dispose();
				_stopCts.Dispose();
			}
			Log.TransportStopped(_logger, null);
		}

		private bool UpdateConnectionPair()
		{
			lock (this)
			{
				if (!_useStatefulReconnect)
				{
					return false;
				}
				Pipe pipe = new Pipe(_httpConnectionOptions.TransportPipeOptions);
				DuplexPipe transport = new DuplexPipe(_transport.Input, pipe.Writer);
				DuplexPipe duplexPipe = (DuplexPipe)(_application = new DuplexPipe(pipe.Reader, _application.Output));
				_transport = transport;
			}
			return true;
		}

		public void DisableReconnect()
		{
			lock (this)
			{
				_useStatefulReconnect = false;
			}
		}
	}
}
