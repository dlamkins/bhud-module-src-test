using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal class Http2HttpMessageHandler : DelegatingHandler
	{
		public Http2HttpMessageHandler(HttpMessageHandler inner)
			: base(inner)
		{
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return base.SendAsync(request, cancellationToken);
		}
	}
}
