using System;
using System.Buffers;
using System.CodeDom.Compiler;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal sealed class ServerSentEventsTransport : ITransport, IDuplexPipe
	{
		private static class Log
		{
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, TransferFormat, Exception> __StartTransportCallback = LoggerMessage.Define<TransferFormat>(LogLevel.Information, new EventId(1, "StartTransport"), "Starting transport. Transfer mode: {TransferFormat}.", new LogDefineOptions
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
			private static readonly Action<ILogger, int, Exception> __MessageToApplicationCallback = LoggerMessage.Define<int>(LogLevel.Debug, new EventId(7, "MessageToApplication"), "Passing message to application. Payload size: {Count}.", new LogDefineOptions
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
			private static readonly Action<ILogger, Exception> __EventStreamEndedCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(8, "EventStreamEnded"), "Server-Sent Event Stream ended.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, long, Exception> __ParsingSSECallback = LoggerMessage.Define<long>(LogLevel.Debug, new EventId(9, "ParsingSSE"), "Received {Count} bytes. Parsing SSE frame.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[LoggerMessage(1, LogLevel.Information, "Starting transport. Transfer mode: {TransferFormat}.", EventName = "StartTransport")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void StartTransport(ILogger logger, TransferFormat transferFormat)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__StartTransportCallback(logger, transferFormat, null);
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

			[LoggerMessage(7, LogLevel.Debug, "Passing message to application. Payload size: {Count}.", EventName = "MessageToApplication")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void MessageToApplication(ILogger logger, int count)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__MessageToApplicationCallback(logger, count, null);
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

			[LoggerMessage(8, LogLevel.Debug, "Server-Sent Event Stream ended.", EventName = "EventStreamEnded")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void EventStreamEnded(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__EventStreamEndedCallback(logger, null);
				}
			}

			[LoggerMessage(9, LogLevel.Debug, "Received {Count} bytes. Parsing SSE frame.", EventName = "ParsingSSE")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ParsingSSE(ILogger logger, long count)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ParsingSSECallback(logger, count, null);
				}
			}
		}

		private readonly HttpClient _httpClient;

		private readonly ILogger _logger;

		private readonly HttpConnectionOptions _httpConnectionOptions;

		private volatile Exception _error;

		private readonly CancellationTokenSource _transportCts = new CancellationTokenSource();

		private readonly CancellationTokenSource _inputCts = new CancellationTokenSource();

		private readonly ServerSentEventsMessageParser _parser = new ServerSentEventsMessageParser();

		private IDuplexPipe _transport;

		private IDuplexPipe _application;

		internal Task Running { get; private set; } = Task.CompletedTask;


		public PipeReader Input => _transport.Input;

		public PipeWriter Output => _transport.Output;

		public ServerSentEventsTransport(HttpClient httpClient, HttpConnectionOptions? httpConnectionOptions = null, ILoggerFactory? loggerFactory = null)
		{
			_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EArgumentNullThrowHelper.ThrowIfNull(httpClient, "httpClient");
			_httpClient = httpClient;
			_logger = (loggerFactory ?? NullLoggerFactory.Instance).CreateLogger<ServerSentEventsTransport>();
			_httpConnectionOptions = httpConnectionOptions ?? new HttpConnectionOptions();
		}

		public async Task StartAsync(Uri url, TransferFormat transferFormat, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (transferFormat != TransferFormat.Text)
			{
				throw new ArgumentException($"The '{transferFormat}' transfer format is not supported by this transport.", "transferFormat");
			}
			Log.StartTransport(_logger, transferFormat);
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
			httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
			HttpResponseMessage response = null;
			try
			{
				response = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				response?.Dispose();
				Log.TransportStopping(_logger);
				throw;
			}
			DuplexPipe.DuplexPipePair duplexPipePair = DuplexPipe.CreateConnectionPair(_httpConnectionOptions.TransportPipeOptions, _httpConnectionOptions.AppPipeOptions);
			_transport = duplexPipePair.Transport;
			_application = duplexPipePair.Application;
			Running = ProcessAsync(url, response);
		}

		private async Task ProcessAsync(Uri url, HttpResponseMessage response)
		{
			Task receiving = ProcessEventStream(response, _transportCts.Token);
			Task sending = SendUtils.SendMessages(url, _application, _httpClient, _logger, _inputCts.Token);
			if (await Task.WhenAny(receiving, sending).ConfigureAwait(continueOnCapturedContext: false) == receiving)
			{
				_inputCts.Cancel();
				_application.Input.CancelPendingRead();
				await sending.ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				_error = (sending.IsFaulted ? sending.Exception.InnerException : null);
				_transportCts.Cancel();
				_application.Output.CancelPendingFlush();
				await receiving.ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task ProcessEventStream(HttpResponseMessage response, CancellationToken cancellationToken)
		{
			Log.StartReceive(_logger);
			using (response)
			{
				using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
				PipeReader reader = PipeReader.Create(stream);
				using (cancellationToken.Register(CancelReader, reader))
				{
					int num;
					_ = num - 1;
					_ = 1;
					try
					{
						while (true)
						{
							ReadResult readResult = await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
							ReadOnlySequence<byte> buffer = readResult.Buffer;
							SequencePosition consumed = buffer.Start;
							SequencePosition examined = buffer.End;
							try
							{
								if (readResult.IsCanceled)
								{
									Log.ReceiveCanceled(_logger);
								}
								else if (!buffer.IsEmpty)
								{
									Log.ParsingSSE(_logger, buffer.Length);
									byte[] message;
									ServerSentEventsMessageParser.ParseResult parseResult = _parser.ParseMessage(buffer, out consumed, out examined, out message);
									FlushResult flushResult = default(FlushResult);
									switch (parseResult)
									{
									case ServerSentEventsMessageParser.ParseResult.Completed:
										Log.MessageToApplication(_logger, message.Length);
										flushResult = await _application.Output.WriteAsync(message).ConfigureAwait(continueOnCapturedContext: false);
										_parser.Reset();
										break;
									case ServerSentEventsMessageParser.ParseResult.Incomplete:
										if (readResult.IsCompleted)
										{
											throw new FormatException("Incomplete message.");
										}
										break;
									}
									if (!flushResult.IsCanceled && !flushResult.IsCompleted)
									{
										continue;
									}
									Log.EventStreamEnded(_logger);
								}
								else if (!readResult.IsCompleted)
								{
									continue;
								}
							}
							finally
							{
								reader.AdvanceTo(consumed, examined);
							}
							break;
						}
					}
					catch (Exception error)
					{
						Exception ex = (_error = error);
					}
					finally
					{
						_application.Output.Complete(_error);
						Log.ReceiveStopped(_logger);
						reader.Complete();
					}
				}
			}
			static void CancelReader(object? state)
			{
				((PipeReader)state).CancelPendingRead();
			}
		}

		public async Task StopAsync()
		{
			Log.TransportStopping(_logger);
			if (_application != null)
			{
				_transport.Output.Complete();
				_transport.Input.Complete();
				_application.Input.CancelPendingRead();
				try
				{
					await Running.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception exception)
				{
					Log.TransportStopped(_logger, exception);
					throw;
				}
				Log.TransportStopped(_logger, null);
			}
		}
	}
}
