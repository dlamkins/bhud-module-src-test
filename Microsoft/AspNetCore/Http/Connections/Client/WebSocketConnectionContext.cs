using System;

namespace Microsoft.AspNetCore.Http.Connections.Client
{
	internal sealed class WebSocketConnectionContext
	{
		public Uri Uri { get; }

		public HttpConnectionOptions Options { get; }

		public WebSocketConnectionContext(Uri uri, HttpConnectionOptions options)
		{
			Uri = uri;
			Options = options;
		}
	}
}
