using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.Shared;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.SignalR.Client
{
	internal static class HubConnectionBuilderHttpExtensions
	{
		private sealed class HttpConnectionOptionsDerivedHttpEndPoint : UriEndPoint
		{
			public HttpConnectionOptionsDerivedHttpEndPoint(IOptions<HttpConnectionOptions> httpConnectionOptions)
				: base(httpConnectionOptions.Value.Url)
			{
			}
		}

		private sealed class HubProtocolDerivedHttpOptionsConfigurer : IConfigureNamedOptions<HttpConnectionOptions>, IConfigureOptions<HttpConnectionOptions>
		{
			private readonly TransferFormat _defaultTransferFormat;

			public HubProtocolDerivedHttpOptionsConfigurer(IHubProtocol hubProtocol)
			{
				_defaultTransferFormat = hubProtocol.TransferFormat;
			}

			public void Configure(string name, HttpConnectionOptions options)
			{
				Configure(options);
			}

			public void Configure(HttpConnectionOptions options)
			{
				options.DefaultTransferFormat = _defaultTransferFormat;
			}
		}

		public static IHubConnectionBuilder WithStatefulReconnect(this IHubConnectionBuilder hubConnectionBuilder)
		{
			hubConnectionBuilder.Services.Configure(delegate(HttpConnectionOptions options)
			{
				options.UseStatefulReconnect = true;
			});
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder hubConnectionBuilder, [_003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EStringSyntax("Uri")] string url)
		{
			hubConnectionBuilder.WithUrlCore(new Uri(url), null, null);
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder hubConnectionBuilder, [_003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EStringSyntax("Uri")] string url, Action<HttpConnectionOptions> configureHttpConnection)
		{
			hubConnectionBuilder.WithUrlCore(new Uri(url), null, configureHttpConnection);
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder hubConnectionBuilder, [_003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EStringSyntax("Uri")] string url, HttpTransportType transports)
		{
			hubConnectionBuilder.WithUrlCore(new Uri(url), transports, null);
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder hubConnectionBuilder, [_003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EStringSyntax("Uri")] string url, HttpTransportType transports, Action<HttpConnectionOptions> configureHttpConnection)
		{
			hubConnectionBuilder.WithUrlCore(new Uri(url), transports, configureHttpConnection);
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder hubConnectionBuilder, Uri url)
		{
			hubConnectionBuilder.WithUrlCore(url, null, null);
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder hubConnectionBuilder, Uri url, Action<HttpConnectionOptions> configureHttpConnection)
		{
			hubConnectionBuilder.WithUrlCore(url, null, configureHttpConnection);
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder hubConnectionBuilder, Uri url, HttpTransportType transports)
		{
			hubConnectionBuilder.WithUrlCore(url, transports, null);
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder hubConnectionBuilder, Uri url, HttpTransportType transports, Action<HttpConnectionOptions> configureHttpConnection)
		{
			hubConnectionBuilder.WithUrlCore(url, transports, configureHttpConnection);
			return hubConnectionBuilder;
		}

		private static IHubConnectionBuilder WithUrlCore(this IHubConnectionBuilder hubConnectionBuilder, Uri url, HttpTransportType? transports, Action<HttpConnectionOptions> configureHttpConnection)
		{
			_003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EArgumentNullThrowHelper.ThrowIfNull(hubConnectionBuilder, "hubConnectionBuilder");
			hubConnectionBuilder.Services.Configure(delegate(HttpConnectionOptions o)
			{
				o.Url = url;
				if (transports.HasValue)
				{
					o.Transports = transports.Value;
				}
			});
			if (configureHttpConnection != null)
			{
				hubConnectionBuilder.Services.Configure(configureHttpConnection);
			}
			hubConnectionBuilder.Services.AddSingleton<EndPoint, HttpConnectionOptionsDerivedHttpEndPoint>();
			hubConnectionBuilder.Services.AddSingleton<IConfigureOptions<HttpConnectionOptions>, HubProtocolDerivedHttpOptionsConfigurer>();
			hubConnectionBuilder.Services.AddSingleton<IConnectionFactory, HttpConnectionFactory>();
			return hubConnectionBuilder;
		}
	}
}
