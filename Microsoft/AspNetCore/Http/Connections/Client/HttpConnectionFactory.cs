using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Http.Connections.Client
{
	internal class HttpConnectionFactory : IConnectionFactory
	{
		private readonly HttpConnectionOptions _httpConnectionOptions;

		private readonly ILoggerFactory _loggerFactory;

		public HttpConnectionFactory(IOptions<HttpConnectionOptions> options, ILoggerFactory loggerFactory)
		{
			_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EArgumentNullThrowHelper.ThrowIfNull(options, "options");
			_httpConnectionOptions = options.Value;
			_loggerFactory = loggerFactory ?? throw new ArgumentNullException("loggerFactory");
		}

		public async ValueTask<ConnectionContext> ConnectAsync(EndPoint endPoint, CancellationToken cancellationToken = default(CancellationToken))
		{
			_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EArgumentNullThrowHelper.ThrowIfNull(endPoint, "endPoint");
			UriEndPoint uriEndPoint = endPoint as UriEndPoint;
			if (uriEndPoint == null)
			{
				throw new NotSupportedException("The provided EndPoint must be of type UriEndPoint.");
			}
			if (_httpConnectionOptions.Url != null && _httpConnectionOptions.Url != uriEndPoint.Uri)
			{
				throw new InvalidOperationException("If HttpConnectionOptions.Url was set, it must match the UriEndPoint.Uri passed to ConnectAsync.");
			}
			HttpConnectionOptions httpConnectionOptions = ShallowCopyHttpConnectionOptions(_httpConnectionOptions);
			httpConnectionOptions.Url = uriEndPoint.Uri;
			HttpConnection connection = new HttpConnection(httpConnectionOptions, _loggerFactory);
			try
			{
				await connection.StartAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				return connection;
			}
			catch
			{
				await connection.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
				throw;
			}
		}

		internal static HttpConnectionOptions ShallowCopyHttpConnectionOptions(HttpConnectionOptions options)
		{
			HttpConnectionOptions httpConnectionOptions = new HttpConnectionOptions
			{
				HttpMessageHandlerFactory = options.HttpMessageHandlerFactory,
				Headers = options.Headers,
				Url = options.Url,
				Transports = options.Transports,
				SkipNegotiation = options.SkipNegotiation,
				AccessTokenProvider = options.AccessTokenProvider,
				CloseTimeout = options.CloseTimeout,
				DefaultTransferFormat = options.DefaultTransferFormat,
				ApplicationMaxBufferSize = options.ApplicationMaxBufferSize,
				TransportMaxBufferSize = options.TransportMaxBufferSize,
				UseStatefulReconnect = options.UseStatefulReconnect
			};
			if (!OperatingSystem.IsBrowser())
			{
				httpConnectionOptions.Cookies = options.Cookies;
				httpConnectionOptions.ClientCertificates = options.ClientCertificates;
				httpConnectionOptions.Credentials = options.Credentials;
				httpConnectionOptions.Proxy = options.Proxy;
				httpConnectionOptions.UseDefaultCredentials = options.UseDefaultCredentials;
				httpConnectionOptions.WebSocketConfiguration = options.WebSocketConfiguration;
				httpConnectionOptions.WebSocketFactory = options.WebSocketFactory;
			}
			return httpConnectionOptions;
		}
	}
}
