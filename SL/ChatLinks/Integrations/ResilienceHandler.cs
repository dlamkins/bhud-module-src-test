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
		{
			_pipeline = pipeline;
		}

		public ResilienceHandler(ResiliencePipeline<HttpResponseMessage> pipeline, HttpMessageHandler innerHandler)
			: base(innerHandler)
		{
			_pipeline = pipeline;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			HttpRequestMessage request2 = request;
			return await _pipeline.ExecuteAsync<HttpResponseMessage>(async (CancellationToken ct) => await base.SendAsync(request2, ct).ConfigureAwait(continueOnCapturedContext: false), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
