using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace SL.ChatLinks.Integrations
{
	public class ResilienceHandler : DelegatingHandler
	{
		private readonly ResiliencePipeline<HttpResponseMessage> _pipeline;

		public ResilienceHandler(ResiliencePipeline<HttpResponseMessage> pipeline)
			: this()
		{
			_pipeline = pipeline;
		}

		public ResilienceHandler(ResiliencePipeline<HttpResponseMessage> pipeline, HttpMessageHandler innerHandler)
			: this(innerHandler)
		{
			_pipeline = pipeline;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			HttpRequestMessage request2 = request;
			return await _pipeline.ExecuteAsync<HttpResponseMessage>(async (CancellationToken ct) => await _003C_003En__0(request2, ct).ConfigureAwait(continueOnCapturedContext: false), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
