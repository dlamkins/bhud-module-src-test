using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal sealed class AccessTokenHttpMessageHandler : DelegatingHandler
	{
		private readonly HttpConnection _httpConnection;

		private string _accessToken;

		public AccessTokenHttpMessageHandler(HttpMessageHandler inner, HttpConnection httpConnection)
			: base(inner)
		{
			_httpConnection = httpConnection;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			bool shouldRetry = true;
			if (string.IsNullOrEmpty(_accessToken) || (request.Properties.TryGetValue("IsNegotiate", out var value) && value is bool && (bool)value))
			{
				shouldRetry = false;
				_accessToken = await _httpConnection.GetAccessTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			SetAccessToken(_accessToken, request);
			HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (shouldRetry && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
			{
				HttpConnection.Log.RetryAccessToken(_httpConnection._logger, httpResponseMessage.StatusCode);
				httpResponseMessage.Dispose();
				_accessToken = await _httpConnection.GetAccessTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
				SetAccessToken(_accessToken, request);
				httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			return httpResponseMessage;
		}

		private static void SetAccessToken(string accessToken, HttpRequestMessage request)
		{
			if (!string.IsNullOrEmpty(accessToken))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			}
		}
	}
}
