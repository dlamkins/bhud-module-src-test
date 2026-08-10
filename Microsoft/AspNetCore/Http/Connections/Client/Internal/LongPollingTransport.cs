using System;
using System.CodeDom.Compiler;
using System.IO.Pipelines;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal sealed class LongPollingTransport : ITransport, IDuplexPipe
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
			private static readonly Action<ILogger, Exception> __ClosingConnectionCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(7, "ClosingConnection"), "The server is closing the connection.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ReceivedMessagesCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(8, "ReceivedMessages"), "Received messages from the server.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Uri, Exception> __ErrorPollingCallback = LoggerMessage.Define<Uri>(LogLevel.Error, new EventId(9, "ErrorPolling"), "Error while polling '{PollUrl}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, int, long, Exception> __PollResponseReceivedCallback = LoggerMessage.Define<int, long>(LogLevel.Trace, new EventId(10, "PollResponseReceived"), "Poll response with status code {StatusCode} received from server. Content length: {ContentLength}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Uri, Exception> __SendingDeleteRequestCallback = LoggerMessage.Define<Uri>(LogLevel.Debug, new EventId(11, "SendingDeleteRequest"), "Sending DELETE request to '{PollUrl}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Uri, Exception> __DeleteRequestAcceptedCallback = LoggerMessage.Define<Uri>(LogLevel.Debug, new EventId(12, "DeleteRequestAccepted"), "DELETE request to '{PollUrl}' accepted.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Uri, Exception> __ErrorSendingDeleteRequestCallback = LoggerMessage.Define<Uri>(LogLevel.Error, new EventId(13, "ErrorSendingDeleteRequest"), "Error sending DELETE request to '{PollUrl}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Uri, Exception> __ConnectionAlreadyClosedSendingDeleteRequestCallback = LoggerMessage.Define<Uri>(LogLevel.Debug, new EventId(14, "ConnectionAlreadyClosedSendingDeleteRequest"), "A 404 response was returned from sending DELETE request to '{PollUrl}', likely because the transport was already closed on the server.", new LogDefineOptions
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

			[LoggerMessage(7, LogLevel.Debug, "The server is closing the connection.", EventName = "ClosingConnection")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ClosingConnection(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ClosingConnectionCallback(logger, null);
				}
			}

			[LoggerMessage(8, LogLevel.Debug, "Received messages from the server.", EventName = "ReceivedMessages")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ReceivedMessages(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ReceivedMessagesCallback(logger, null);
				}
			}

			[LoggerMessage(9, LogLevel.Error, "Error while polling '{PollUrl}'.", EventName = "ErrorPolling")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorPolling(ILogger logger, Uri pollUrl, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorPollingCallback(logger, pollUrl, exception);
				}
			}

			public static void PollResponseReceived(ILogger logger, HttpResponseMessage response)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					PollResponseReceived(logger, (int)response.StatusCode, response.Content.Headers.ContentLength ?? (-1));
				}
			}

			[LoggerMessage(10, LogLevel.Trace, "Poll response with status code {StatusCode} received from server. Content length: {ContentLength}.", EventName = "PollResponseReceived", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void PollResponseReceived(ILogger logger, int statusCode, long contentLength)
			{
				__PollResponseReceivedCallback(logger, statusCode, contentLength, null);
			}

			[LoggerMessage(11, LogLevel.Debug, "Sending DELETE request to '{PollUrl}'.", EventName = "SendingDeleteRequest")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendingDeleteRequest(ILogger logger, Uri pollUrl)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendingDeleteRequestCallback(logger, pollUrl, null);
				}
			}

			[LoggerMessage(12, LogLevel.Debug, "DELETE request to '{PollUrl}' accepted.", EventName = "DeleteRequestAccepted")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void DeleteRequestAccepted(ILogger logger, Uri pollUrl)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__DeleteRequestAcceptedCallback(logger, pollUrl, null);
				}
			}

			[LoggerMessage(13, LogLevel.Error, "Error sending DELETE request to '{PollUrl}'.", EventName = "ErrorSendingDeleteRequest")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorSendingDeleteRequest(ILogger logger, Uri pollUrl, Exception ex)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorSendingDeleteRequestCallback(logger, pollUrl, ex);
				}
			}

			[LoggerMessage(14, LogLevel.Debug, "A 404 response was returned from sending DELETE request to '{PollUrl}', likely because the transport was already closed on the server.", EventName = "ConnectionAlreadyClosedSendingDeleteRequest")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ConnectionAlreadyClosedSendingDeleteRequest(ILogger logger, Uri pollUrl)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ConnectionAlreadyClosedSendingDeleteRequestCallback(logger, pollUrl, null);
				}
			}
		}

		private readonly HttpClient _httpClient;

		private readonly ILogger _logger;

		private readonly HttpConnectionOptions _httpConnectionOptions;

		private IDuplexPipe _application;

		private IDuplexPipe _transport;

		private volatile Exception _error;

		private readonly CancellationTokenSource _transportCts = new CancellationTokenSource();

		internal Task Running { get; private set; } = Task.CompletedTask;


		public PipeReader Input => _transport.Input;

		public PipeWriter Output => _transport.Output;

		public LongPollingTransport(HttpClient httpClient, HttpConnectionOptions? httpConnectionOptions = null, ILoggerFactory? loggerFactory = null)
		{
			_httpClient = httpClient;
			_logger = (loggerFactory ?? NullLoggerFactory.Instance).CreateLogger<LongPollingTransport>();
			_httpConnectionOptions = httpConnectionOptions ?? new HttpConnectionOptions();
		}

		public async Task StartAsync(Uri url, TransferFormat transferFormat, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (transferFormat != TransferFormat.Binary && transferFormat != TransferFormat.Text)
			{
				throw new ArgumentException($"The '{transferFormat}' transfer format is not supported by this transport.", "transferFormat");
			}
			Log.StartTransport(_logger, transferFormat);
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
			using (HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
			{
				httpResponseMessage.EnsureSuccessStatusCode();
			}
			DuplexPipe.DuplexPipePair duplexPipePair = DuplexPipe.CreateConnectionPair(_httpConnectionOptions.TransportPipeOptions, _httpConnectionOptions.AppPipeOptions);
			_transport = duplexPipePair.Transport;
			_application = duplexPipePair.Application;
			Running = ProcessAsync(url);
		}

		private async Task ProcessAsync(Uri url)
		{
			Task receiving = Poll(url, _transportCts.Token);
			Task sending = SendUtils.SendMessages(url, _application, _httpClient, _logger);
			if (await Task.WhenAny(receiving, sending).ConfigureAwait(continueOnCapturedContext: false) == receiving)
			{
				_application.Input.CancelPendingRead();
				await sending.ConfigureAwait(continueOnCapturedContext: false);
				return;
			}
			_error = (sending.IsFaulted ? sending.Exception.InnerException : null);
			_transportCts.Cancel();
			_application.Output.CancelPendingFlush();
			await receiving.ConfigureAwait(continueOnCapturedContext: false);
			await SendDeleteRequest(url).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task StopAsync()
		{
			Log.TransportStopping(_logger);
			if (_application != null)
			{
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
				_transport.Output.Complete();
				_transport.Input.Complete();
				Log.TransportStopped(_logger, null);
			}
		}

		private async Task Poll(Uri pollUrl, CancellationToken cancellationToken)
		{
			Log.StartReceive(_logger);
			_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EPipeWriterStream applicationStream = new _003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EPipeWriterStream(_application.Output);
			try
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, pollUrl);
					HttpResponseMessage httpResponseMessage;
					try
					{
						httpResponseMessage = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
					catch (OperationCanceledException)
					{
						continue;
					}
					catch (WebException ex2) when (!OperatingSystem.IsBrowser() && ex2.Status == WebExceptionStatus.RequestCanceled)
					{
						continue;
					}
					Log.PollResponseReceived(_logger, httpResponseMessage);
					httpResponseMessage.EnsureSuccessStatusCode();
					if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent || cancellationToken.IsCancellationRequested)
					{
						Log.ClosingConnection(_logger);
						break;
					}
					Log.ReceivedMessages(_logger);
					await httpResponseMessage.Content.CopyToAsync(applicationStream).ConfigureAwait(continueOnCapturedContext: false);
					FlushResult flushResult = await _application.Output.FlushAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					if (flushResult.IsCanceled || flushResult.IsCompleted)
					{
						break;
					}
				}
			}
			catch (OperationCanceledException)
			{
				Log.ReceiveCanceled(_logger);
			}
			catch (Exception ex4)
			{
				Log.ErrorPolling(_logger, pollUrl, ex4);
				_error = ex4;
			}
			finally
			{
				_application.Output.Complete(_error);
				Log.ReceiveStopped(_logger);
			}
		}

		private async Task SendDeleteRequest(Uri url)
		{
			try
			{
				Log.SendingDeleteRequest(_logger, url);
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
				HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
				if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
				{
					Log.ConnectionAlreadyClosedSendingDeleteRequest(_logger, url);
					return;
				}
				httpResponseMessage.EnsureSuccessStatusCode();
				Log.DeleteRequestAccepted(_logger, url);
			}
			catch (Exception ex)
			{
				Log.ErrorSendingDeleteRequest(_logger, url, ex);
			}
		}
	}
}
