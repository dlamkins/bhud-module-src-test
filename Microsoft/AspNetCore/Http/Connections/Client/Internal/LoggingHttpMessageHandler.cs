using System;
using System.CodeDom.Compiler;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Shared;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal sealed class LoggingHttpMessageHandler : DelegatingHandler
	{
		private static class Log
		{
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, HttpMethod, Uri, Exception> __SendingHttpRequestCallback = LoggerMessage.Define<HttpMethod, Uri>(LogLevel.Trace, new EventId(1, "SendingHttpRequest"), "Sending HTTP request {RequestMethod} '{RequestUrl}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, HttpStatusCode, HttpMethod, Uri, Exception> __UnsuccessfulHttpResponseCallback = LoggerMessage.Define<HttpStatusCode, HttpMethod, Uri>(LogLevel.Warning, new EventId(2, "UnsuccessfulHttpResponse"), "Unsuccessful HTTP response {StatusCode} return from {RequestMethod} '{RequestUrl}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[LoggerMessage(1, LogLevel.Trace, "Sending HTTP request {RequestMethod} '{RequestUrl}'.", EventName = "SendingHttpRequest")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendingHttpRequest(ILogger logger, HttpMethod requestMethod, Uri requestUrl)
			{
				if (logger.IsEnabled(LogLevel.Trace))
				{
					__SendingHttpRequestCallback(logger, requestMethod, requestUrl, null);
				}
			}

			[LoggerMessage(2, LogLevel.Warning, "Unsuccessful HTTP response {StatusCode} return from {RequestMethod} '{RequestUrl}'.", EventName = "UnsuccessfulHttpResponse")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void UnsuccessfulHttpResponse(ILogger logger, HttpStatusCode statusCode, HttpMethod requestMethod, Uri requestUrl)
			{
				if (logger.IsEnabled(LogLevel.Warning))
				{
					__UnsuccessfulHttpResponseCallback(logger, statusCode, requestMethod, requestUrl, null);
				}
			}
		}

		private readonly ILogger<LoggingHttpMessageHandler> _logger;

		public LoggingHttpMessageHandler(HttpMessageHandler inner, ILoggerFactory loggerFactory)
			: base(inner)
		{
			_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EArgumentNullThrowHelper.ThrowIfNull(loggerFactory, "loggerFactory");
			_logger = loggerFactory.CreateLogger<LoggingHttpMessageHandler>();
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Log.SendingHttpRequest(_logger, request.Method, request.RequestUri);
			HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode != HttpStatusCode.SwitchingProtocols)
			{
				Log.UnsuccessfulHttpResponse(_logger, httpResponseMessage.StatusCode, request.Method, request.RequestUri);
			}
			return httpResponseMessage;
		}
	}
}
