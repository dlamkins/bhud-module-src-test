using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Connections.Abstractions;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http.Connections.Client.Internal;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.Http.Connections.Client
{
	internal class HttpConnection : ConnectionContext, IConnectionInherentKeepAliveFeature
	{
		internal static class Log
		{
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __StartingCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(1, "Starting"), "Starting HttpConnection.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SkippingStartCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(2, "SkippingStart"), "Skipping start, connection is already started.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __StartedCallback = LoggerMessage.Define(LogLevel.Information, new EventId(3, "Started"), "HttpConnection Started.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __DisposingHttpConnectionCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(4, "DisposingHttpConnection"), "Disposing HttpConnection.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SkippingDisposeCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(5, "SkippingDispose"), "Skipping dispose, connection is already disposed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __DisposedCallback = LoggerMessage.Define(LogLevel.Information, new EventId(6, "Disposed"), "HttpConnection Disposed.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Uri, Exception> __StartingTransportCallback = LoggerMessage.Define<string, Uri>(LogLevel.Debug, new EventId(7, "StartingTransport"), "Starting transport '{Transport}' with Url: {Url}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Uri, Exception> __EstablishingConnectionCallback = LoggerMessage.Define<Uri>(LogLevel.Debug, new EventId(8, "EstablishingConnection"), "Establishing connection with server at '{Url}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __ConnectionEstablishedCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(9, "Established"), "Established connection '{ConnectionId}' with the server.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Uri, Exception> __ErrorWithNegotiationCallback = LoggerMessage.Define<Uri>(LogLevel.Error, new EventId(10, "ErrorWithNegotiation"), "Failed to start connection. Error getting negotiation response from '{Url}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, HttpTransportType, Exception> __ErrorStartingTransportCallback = LoggerMessage.Define<HttpTransportType>(LogLevel.Error, new EventId(11, "ErrorStartingTransport"), "Failed to start connection. Error starting transport '{Transport}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __TransportNotSupportedCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(12, "TransportNotSupported"), "Skipping transport {TransportName} because it is not supported by this client.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, string, Exception> __TransportDoesNotSupportTransferFormatCallback = LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(13, "TransportDoesNotSupportTransferFormat"), "Skipping transport {TransportName} because it does not support the requested transfer format '{TransferFormat}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __TransportDisabledByClientCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(14, "TransportDisabledByClient"), "Skipping transport {TransportName} because it was disabled by the client.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, string, Exception> __TransportFailedCallback = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(15, "TransportFailed"), "Skipping transport {TransportName} because it failed to initialize.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __WebSocketsNotSupportedByOperatingSystemCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(16, "WebSocketsNotSupportedByOperatingSystem"), "Skipping WebSockets because they are not supported by the operating system.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __TransportThrewExceptionOnStopCallback = LoggerMessage.Define(LogLevel.Error, new EventId(17, "TransportThrewExceptionOnStop"), "The transport threw an exception while stopping.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, HttpTransportType, Exception> __TransportStartedCallback = LoggerMessage.Define<HttpTransportType>(LogLevel.Debug, new EventId(18, "TransportStarted"), "Transport '{Transport}' started.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __ServerSentEventsNotSupportedByBrowserCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(19, "ServerSentEventsNotSupportedByBrowser"), "Skipping ServerSentEvents because they are not supported by the browser.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __CookiesNotSupportedCallback = LoggerMessage.Define(LogLevel.Trace, new EventId(20, "CookiesNotSupported"), "Cookies are not supported on this platform.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, HttpStatusCode, Exception> __RetryAccessTokenCallback = LoggerMessage.Define<HttpStatusCode>(LogLevel.Debug, new EventId(21, "RetryAccessToken"), "{StatusCode} received, getting a new access token and retrying request.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[LoggerMessage(1, LogLevel.Debug, "Starting HttpConnection.", EventName = "Starting")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void Starting(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__StartingCallback(logger, null);
				}
			}

			[LoggerMessage(2, LogLevel.Debug, "Skipping start, connection is already started.", EventName = "SkippingStart")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SkippingStart(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SkippingStartCallback(logger, null);
				}
			}

			[LoggerMessage(3, LogLevel.Information, "HttpConnection Started.", EventName = "Started")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void Started(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__StartedCallback(logger, null);
				}
			}

			[LoggerMessage(4, LogLevel.Debug, "Disposing HttpConnection.", EventName = "DisposingHttpConnection")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void DisposingHttpConnection(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__DisposingHttpConnectionCallback(logger, null);
				}
			}

			[LoggerMessage(5, LogLevel.Debug, "Skipping dispose, connection is already disposed.", EventName = "SkippingDispose")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SkippingDispose(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SkippingDisposeCallback(logger, null);
				}
			}

			[LoggerMessage(6, LogLevel.Information, "HttpConnection Disposed.", EventName = "Disposed")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void Disposed(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Information))
				{
					__DisposedCallback(logger, null);
				}
			}

			public static void StartingTransport(ILogger logger, HttpTransportType transportType, Uri url)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					StartingTransport(logger, transportType.ToString(), url);
				}
			}

			[LoggerMessage(7, LogLevel.Debug, "Starting transport '{Transport}' with Url: {Url}.", EventName = "StartingTransport", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void StartingTransport(ILogger logger, string transport, Uri url)
			{
				__StartingTransportCallback(logger, transport, url, null);
			}

			[LoggerMessage(8, LogLevel.Debug, "Establishing connection with server at '{Url}'.", EventName = "EstablishingConnection")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void EstablishingConnection(ILogger logger, Uri url)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__EstablishingConnectionCallback(logger, url, null);
				}
			}

			[LoggerMessage(9, LogLevel.Debug, "Established connection '{ConnectionId}' with the server.", EventName = "Established")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ConnectionEstablished(ILogger logger, string connectionId)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ConnectionEstablishedCallback(logger, connectionId, null);
				}
			}

			[LoggerMessage(10, LogLevel.Error, "Failed to start connection. Error getting negotiation response from '{Url}'.", EventName = "ErrorWithNegotiation")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorWithNegotiation(ILogger logger, Uri url, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorWithNegotiationCallback(logger, url, exception);
				}
			}

			[LoggerMessage(11, LogLevel.Error, "Failed to start connection. Error starting transport '{Transport}'.", EventName = "ErrorStartingTransport")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorStartingTransport(ILogger logger, HttpTransportType transport, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorStartingTransportCallback(logger, transport, exception);
				}
			}

			[LoggerMessage(12, LogLevel.Debug, "Skipping transport {TransportName} because it is not supported by this client.", EventName = "TransportNotSupported")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void TransportNotSupported(ILogger logger, string transportName)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__TransportNotSupportedCallback(logger, transportName, null);
				}
			}

			public static void TransportDoesNotSupportTransferFormat(ILogger logger, HttpTransportType transport, TransferFormat transferFormat)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					TransportDoesNotSupportTransferFormat(logger, transport.ToString(), transferFormat.ToString());
				}
			}

			[LoggerMessage(13, LogLevel.Debug, "Skipping transport {TransportName} because it does not support the requested transfer format '{TransferFormat}'.", EventName = "TransportDoesNotSupportTransferFormat", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static void TransportDoesNotSupportTransferFormat(ILogger logger, string transportName, string transferFormat)
			{
				__TransportDoesNotSupportTransferFormatCallback(logger, transportName, transferFormat, null);
			}

			public static void TransportDisabledByClient(ILogger logger, HttpTransportType transport)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					TransportDisabledByClient(logger, transport.ToString());
				}
			}

			[LoggerMessage(14, LogLevel.Debug, "Skipping transport {TransportName} because it was disabled by the client.", EventName = "TransportDisabledByClient", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void TransportDisabledByClient(ILogger logger, string transportName)
			{
				__TransportDisabledByClientCallback(logger, transportName, null);
			}

			public static void TransportFailed(ILogger logger, HttpTransportType transport, Exception ex)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					TransportFailed(logger, transport.ToString(), ex);
				}
			}

			[LoggerMessage(15, LogLevel.Debug, "Skipping transport {TransportName} because it failed to initialize.", EventName = "TransportFailed", SkipEnabledCheck = true)]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void TransportFailed(ILogger logger, string transportName, Exception ex)
			{
				__TransportFailedCallback(logger, transportName, ex);
			}

			[LoggerMessage(16, LogLevel.Debug, "Skipping WebSockets because they are not supported by the operating system.", EventName = "WebSocketsNotSupportedByOperatingSystem")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void WebSocketsNotSupportedByOperatingSystem(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__WebSocketsNotSupportedByOperatingSystemCallback(logger, null);
				}
			}

			[LoggerMessage(17, LogLevel.Error, "The transport threw an exception while stopping.", EventName = "TransportThrewExceptionOnStop")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void TransportThrewExceptionOnStop(ILogger logger, Exception ex)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__TransportThrewExceptionOnStopCallback(logger, ex);
				}
			}

			[LoggerMessage(18, LogLevel.Debug, "Transport '{Transport}' started.", EventName = "TransportStarted")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void TransportStarted(ILogger logger, HttpTransportType transport)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__TransportStartedCallback(logger, transport, null);
				}
			}

			[LoggerMessage(19, LogLevel.Debug, "Skipping ServerSentEvents because they are not supported by the browser.", EventName = "ServerSentEventsNotSupportedByBrowser")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ServerSentEventsNotSupportedByBrowser(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__ServerSentEventsNotSupportedByBrowserCallback(logger, null);
				}
			}

			[LoggerMessage(20, LogLevel.Trace, "Cookies are not supported on this platform.", EventName = "CookiesNotSupported")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void CookiesNotSupported(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__CookiesNotSupportedCallback(logger, null);
				}
			}

			[LoggerMessage(21, LogLevel.Debug, "{StatusCode} received, getting a new access token and retrying request.", EventName = "RetryAccessToken")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void RetryAccessToken(ILogger logger, HttpStatusCode statusCode)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__RetryAccessTokenCallback(logger, statusCode, null);
				}
			}
		}

		private const int _maxRedirects = 100;

		private const int _protocolVersionNumber = 1;

		private static readonly Task<string> _noAccessToken = Task.FromResult<string>(null);

		private static readonly TimeSpan HttpClientTimeout = TimeSpan.FromSeconds(120.0);

		internal readonly ILogger _logger;

		private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

		private bool _started;

		private bool _disposed;

		private bool _hasInherentKeepAlive;

		private readonly HttpClient _httpClient;

		private readonly HttpConnectionOptions _httpConnectionOptions;

		private ITransport _transport;

		private readonly ITransportFactory _transportFactory;

		private string _connectionId;

		private readonly ConnectionLogScope _logScope;

		private readonly ILoggerFactory _loggerFactory;

		private readonly Uri _url;

		private Func<Task<string>> _accessTokenProvider;

		public override IDuplexPipe Transport
		{
			get
			{
				CheckDisposed();
				if (_transport == null)
				{
					throw new InvalidOperationException("Cannot access the Transport pipe before the connection has started.");
				}
				return _transport;
			}
			set
			{
				throw new NotSupportedException("The transport pipe isn't settable.");
			}
		}

		public override IFeatureCollection Features { get; } = new FeatureCollection();


		public override string? ConnectionId
		{
			get
			{
				return _connectionId;
			}
			set
			{
				throw new InvalidOperationException("The ConnectionId is set internally and should not be set by user code.");
			}
		}

		public override IDictionary<object, object?> Items { get; set; } = new ConnectionItems();


		bool IConnectionInherentKeepAliveFeature.HasInherentKeepAlive => _hasInherentKeepAlive;

		public HttpConnection(Uri url)
			: this(url, HttpTransports.All)
		{
		}

		public HttpConnection(Uri url, HttpTransportType transports)
			: this(url, transports, null)
		{
		}

		public HttpConnection(Uri url, HttpTransportType transports, ILoggerFactory? loggerFactory)
			: this(CreateHttpOptions(url, transports), loggerFactory)
		{
		}

		private static HttpConnectionOptions CreateHttpOptions(Uri url, HttpTransportType transports)
		{
			_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EArgumentNullThrowHelper.ThrowIfNull(url, "url");
			return new HttpConnectionOptions
			{
				Url = url,
				Transports = transports
			};
		}

		public HttpConnection(HttpConnectionOptions httpConnectionOptions, ILoggerFactory? loggerFactory)
		{
			_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EArgumentNullThrowHelper.ThrowIfNull(httpConnectionOptions, "httpConnectionOptions");
			if (httpConnectionOptions.Url == null)
			{
				throw new ArgumentException("Options does not have a URL specified.", "httpConnectionOptions");
			}
			_loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
			_logger = _loggerFactory.CreateLogger<HttpConnection>();
			_httpConnectionOptions = httpConnectionOptions;
			_url = _httpConnectionOptions.Url;
			if (!httpConnectionOptions.SkipNegotiation || httpConnectionOptions.Transports != HttpTransportType.WebSockets)
			{
				_httpClient = CreateHttpClient();
			}
			if (httpConnectionOptions.Transports == HttpTransportType.ServerSentEvents && OperatingSystem.IsBrowser())
			{
				throw new ArgumentException("ServerSentEvents can not be the only transport specified when running in the browser.", "httpConnectionOptions");
			}
			_transportFactory = new DefaultTransportFactory(httpConnectionOptions.Transports, _loggerFactory, _httpClient, httpConnectionOptions, new Func<Task<string>>(GetAccessTokenAsync));
			_logScope = new ConnectionLogScope();
			Features.Set((IConnectionInherentKeepAliveFeature?)this);
		}

		internal HttpConnection(HttpConnectionOptions httpConnectionOptions, ILoggerFactory loggerFactory, ITransportFactory transportFactory)
			: this(httpConnectionOptions, loggerFactory)
		{
			_transportFactory = transportFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return StartAsync(_httpConnectionOptions.DefaultTransferFormat, cancellationToken);
		}

		public async Task StartAsync(TransferFormat transferFormat, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (_logger.BeginScope(_logScope))
			{
				await StartAsyncCore(transferFormat, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task StartAsyncCore(TransferFormat transferFormat, CancellationToken cancellationToken)
		{
			CheckDisposed();
			if (_started)
			{
				Log.SkippingStart(_logger);
				return;
			}
			await _connectionLock.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				CheckDisposed();
				if (_started)
				{
					Log.SkippingStart(_logger);
					return;
				}
				Log.Starting(_logger);
				await SelectAndStartTransport(transferFormat, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				_started = true;
				Log.Started(_logger);
			}
			finally
			{
				_connectionLock.Release();
			}
		}

		public override async ValueTask DisposeAsync()
		{
			using (_logger.BeginScope(_logScope))
			{
				await DisposeAsyncCore().ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task DisposeAsyncCore()
		{
			if (_disposed)
			{
				return;
			}
			await _connectionLock.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (!_disposed && _started)
				{
					Log.DisposingHttpConnection(_logger);
					try
					{
						await _transport.StopAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
					catch (Exception ex)
					{
						Log.TransportThrewExceptionOnStop(_logger, ex);
					}
					Log.Disposed(_logger);
				}
				else
				{
					Log.SkippingDispose(_logger);
				}
				_httpClient?.Dispose();
			}
			finally
			{
				if (!_disposed)
				{
					_disposed = true;
				}
				_connectionLock.Release();
			}
		}

		private async Task SelectAndStartTransport(TransferFormat transferFormat, CancellationToken cancellationToken)
		{
			Uri uri = _url;
			_accessTokenProvider = _httpConnectionOptions.AccessTokenProvider;
			List<Exception> transportExceptions = new List<Exception>();
			if (_httpConnectionOptions.SkipNegotiation)
			{
				if (_httpConnectionOptions.Transports != HttpTransportType.WebSockets)
				{
					throw new InvalidOperationException("Negotiation can only be skipped when using the WebSocket transport directly.");
				}
				Log.StartingTransport(_logger, _httpConnectionOptions.Transports, uri);
				await StartTransport(uri, _httpConnectionOptions.Transports, transferFormat, cancellationToken, useStatefulReconnect: false).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				int redirects = 0;
				NegotiationResponse negotiationResponse;
				do
				{
					negotiationResponse = await GetNegotiationResponseAsync(uri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					if (negotiationResponse.Url != null)
					{
						uri = new Uri(negotiationResponse.Url);
					}
					if (negotiationResponse.AccessToken != null)
					{
						string accessToken = negotiationResponse.AccessToken;
						_accessTokenProvider = () => Task.FromResult(accessToken);
					}
					redirects++;
				}
				while (negotiationResponse.Url != null && redirects < 100);
				if (redirects == 100 && negotiationResponse.Url != null)
				{
					throw new InvalidOperationException("Negotiate redirection limit exceeded.");
				}
				Uri connectUrl = CreateConnectUrl(uri, negotiationResponse.ConnectionToken);
				string transferFormatString = transferFormat.ToString();
				foreach (AvailableTransport item in negotiationResponse.AvailableTransports!)
				{
					if (!Enum.TryParse<HttpTransportType>(item.Transport, out var transportType))
					{
						Log.TransportNotSupported(_logger, item.Transport);
						transportExceptions.Add(new TransportFailedException(item.Transport, "The transport is not supported by the client."));
						continue;
					}
					if (transportType == HttpTransportType.WebSockets && !IsWebSocketsSupported())
					{
						Log.WebSocketsNotSupportedByOperatingSystem(_logger);
						transportExceptions.Add(new TransportFailedException("WebSockets", "The transport is not supported on this operating system."));
						continue;
					}
					if (transportType == HttpTransportType.ServerSentEvents && OperatingSystem.IsBrowser())
					{
						Log.ServerSentEventsNotSupportedByBrowser(_logger);
						transportExceptions.Add(new TransportFailedException("ServerSentEvents", "The transport is not supported in the browser."));
						continue;
					}
					try
					{
						if ((transportType & _httpConnectionOptions.Transports) == 0)
						{
							Log.TransportDisabledByClient(_logger, transportType);
							transportExceptions.Add(new TransportFailedException(transportType.ToString(), "The transport is disabled by the client."));
							continue;
						}
						if (!item.TransferFormats.Contains<string>(transferFormatString, StringComparer.Ordinal))
						{
							Log.TransportDoesNotSupportTransferFormat(_logger, transportType, transferFormat);
							transportExceptions.Add(new TransportFailedException(transportType.ToString(), $"The transport does not support the '{transferFormat}' transfer format."));
							continue;
						}
						if (negotiationResponse == null)
						{
							_httpConnectionOptions.UseStatefulReconnect = transportType == HttpTransportType.WebSockets && _httpConnectionOptions.UseStatefulReconnect;
							negotiationResponse = await GetNegotiationResponseAsync(uri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
							connectUrl = CreateConnectUrl(uri, negotiationResponse.ConnectionToken);
						}
						Log.StartingTransport(_logger, transportType, uri);
						await StartTransport(connectUrl, transportType, transferFormat, cancellationToken, negotiationResponse.UseStatefulReconnect).ConfigureAwait(continueOnCapturedContext: false);
						goto IL_05e9;
					}
					catch (Exception ex)
					{
						Log.TransportFailed(_logger, transportType, ex);
						transportExceptions.Add(new TransportFailedException(transportType.ToString(), ex.Message, ex));
						negotiationResponse = null;
					}
				}
			}
			goto IL_05e9;
			IL_05e9:
			if (_transport == null)
			{
				if (transportExceptions.Count > 0)
				{
					throw new AggregateException("Unable to connect to the server with any of the available transports.", transportExceptions);
				}
				throw new NoTransportSupportedException("None of the transports supported by the client are supported by the server.");
			}
		}

		private async Task<NegotiationResponse> NegotiateAsync(Uri url, HttpClient httpClient, ILogger logger, CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				Log.EstablishingConnection(logger, url);
				UriBuilder uriBuilder = new UriBuilder(url);
				if (!uriBuilder.Path.EndsWith("/", StringComparison.Ordinal))
				{
					uriBuilder.Path += "/";
				}
				uriBuilder.Path += "negotiate";
				Uri uri = ((!uriBuilder.Query.Contains("negotiateVersion")) ? Utils.AppendQueryString(uriBuilder.Uri, $"negotiateVersion={1}") : uriBuilder.Uri);
				if (_httpConnectionOptions.UseStatefulReconnect)
				{
					uri = Utils.AppendQueryString(uri, "useStatefulReconnect=true");
				}
				using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
				request.Properties.Add("IsNegotiate", true);
				using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				response.EnsureSuccessStatusCode();
				NegotiationResponse negotiationResponse = NegotiateProtocol.ParseResponse(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(continueOnCapturedContext: false));
				if (!string.IsNullOrEmpty(negotiationResponse.Error))
				{
					throw new InvalidOperationException(negotiationResponse.Error);
				}
				Log.ConnectionEstablished(_logger, negotiationResponse.ConnectionId);
				return negotiationResponse;
			}
			catch (Exception exception)
			{
				Log.ErrorWithNegotiation(logger, url, exception);
				throw;
			}
		}

		private static Uri CreateConnectUrl(Uri url, string connectionId)
		{
			if (string.IsNullOrWhiteSpace(connectionId))
			{
				throw new FormatException("Invalid connection id.");
			}
			return Utils.AppendQueryString(url, "id=" + connectionId);
		}

		private async Task StartTransport(Uri connectUrl, HttpTransportType transportType, TransferFormat transferFormat, CancellationToken cancellationToken, bool useStatefulReconnect)
		{
			ITransport transport = _transportFactory.CreateTransport(transportType, useStatefulReconnect);
			try
			{
				await transport.StartAsync(connectUrl, transferFormat, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception exception)
			{
				Log.ErrorStartingTransport(_logger, transportType, exception);
				_transport = null;
				throw;
			}
			_hasInherentKeepAlive = transportType == HttpTransportType.LongPolling;
			_transport = transport;
			if (useStatefulReconnect)
			{
				IStatefulReconnectFeature statefulReconnectFeature = _transport as IStatefulReconnectFeature;
				if (statefulReconnectFeature != null)
				{
					Features.Set(statefulReconnectFeature);
				}
			}
			Log.TransportStarted(_logger, transportType);
		}

		private HttpClient CreateHttpClient()
		{
			HttpClientHandler httpClientHandler = new HttpClientHandler();
			HttpMessageHandler inner = httpClientHandler;
			bool flag = OperatingSystem.IsBrowser();
			bool flag2 = true;
			if (_httpConnectionOptions != null)
			{
				if (!flag)
				{
					if (_httpConnectionOptions.Proxy != null)
					{
						httpClientHandler.Proxy = _httpConnectionOptions.Proxy;
					}
					try
					{
						httpClientHandler.CookieContainer = _httpConnectionOptions.Cookies;
					}
					catch (Exception ex) when (ex is NotSupportedException || ex is NotImplementedException)
					{
						Log.CookiesNotSupported(_logger);
					}
					X509CertificateCollection clientCertificates = _httpConnectionOptions.ClientCertificates;
					if (clientCertificates != null && clientCertificates.Count > 0)
					{
						httpClientHandler.ClientCertificates.AddRange(clientCertificates);
					}
					if (_httpConnectionOptions.UseDefaultCredentials.HasValue)
					{
						httpClientHandler.UseDefaultCredentials = _httpConnectionOptions.UseDefaultCredentials.Value;
						flag2 = !_httpConnectionOptions.UseDefaultCredentials.Value;
					}
					if (_httpConnectionOptions.Credentials != null)
					{
						httpClientHandler.Credentials = _httpConnectionOptions.Credentials;
						flag2 = false;
					}
				}
				inner = httpClientHandler;
				if (_httpConnectionOptions.HttpMessageHandlerFactory != null)
				{
					inner = _httpConnectionOptions.HttpMessageHandlerFactory!(httpClientHandler);
					if (inner == null)
					{
						throw new InvalidOperationException("Configured HttpMessageHandlerFactory did not return a value.");
					}
				}
				inner = new AccessTokenHttpMessageHandler(inner, this);
			}
			inner = new LoggingHttpMessageHandler(inner, _loggerFactory);
			if (flag2)
			{
				inner = new Http2HttpMessageHandler(inner);
			}
			HttpClient httpClient = new HttpClient(inner);
			httpClient.Timeout = HttpClientTimeout;
			bool flag3 = false;
			if (_httpConnectionOptions?.Headers != null)
			{
				foreach (KeyValuePair<string, string> header in _httpConnectionOptions.Headers)
				{
					if (string.Equals(header.Key, Constants.UserAgent, StringComparison.OrdinalIgnoreCase))
					{
						flag3 = true;
						if (string.IsNullOrEmpty(header.Value))
						{
							httpClient.DefaultRequestHeaders.Remove(header.Key);
						}
						else if (httpClient.DefaultRequestHeaders.Contains(header.Key))
						{
							httpClient.DefaultRequestHeaders.Remove(header.Key);
							httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
						}
						else
						{
							httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
						}
					}
					else
					{
						httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
					}
				}
			}
			if (!flag3)
			{
				httpClient.DefaultRequestHeaders.Add(Constants.UserAgent, Constants.UserAgentHeader);
			}
			httpClient.DefaultRequestHeaders.Remove("X-Requested-With");
			httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
			return httpClient;
		}

		internal Task<string?> GetAccessTokenAsync()
		{
			if (_accessTokenProvider == null)
			{
				return _noAccessToken;
			}
			return _accessTokenProvider();
		}

		private void CheckDisposed()
		{
			ObjectDisposedThrowHelper.ThrowIf(_disposed, this);
		}

		private static bool IsWebSocketsSupported()
		{
			try
			{
				new ClientWebSocket().Dispose();
				return true;
			}
			catch
			{
				return false;
			}
		}

		private async Task<NegotiationResponse> GetNegotiationResponseAsync(Uri uri, CancellationToken cancellationToken)
		{
			NegotiationResponse negotiationResponse = await NegotiateAsync(uri, _httpClient, _logger, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			_connectionId = negotiationResponse.ConnectionId;
			if (negotiationResponse.Version == 0)
			{
				negotiationResponse.ConnectionToken = _connectionId;
			}
			_logScope.ConnectionId = _connectionId;
			return negotiationResponse;
		}
	}
}
