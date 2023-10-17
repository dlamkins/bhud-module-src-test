using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules;
using Flurl.Http;
using Gw2Sharp.WebApi.Http;
using Gw2Sharp.WebApi.Middleware;

namespace BhModule.Community.ErrorSubmissionModule.WebHooks
{
	internal class WebStatMiddleware : IWebApiMiddleware
	{
		private EtmConfig _config;

		private ErrorSubmissionModule _errorSubmissionModule;

		private string _apiAddressList;

		public WebStatMiddleware(EtmConfig config, ErrorSubmissionModule errorSubmissionModule)
		{
			_config = config;
			_errorSubmissionModule = errorSubmissionModule;
			PopulateApiHost();
		}

		private void PopulateApiHost()
		{
			try
			{
				IPHostEntry hostEntry = Dns.GetHostEntry("api.guildwars2.com");
				_apiAddressList = string.Join(",", hostEntry.AddressList.OrderBy((IPAddress a) => a.Address));
			}
			catch (Exception)
			{
			}
		}

		private bool IsSuccessStatusCode(HttpStatusCode statusCode)
		{
			if (statusCode >= HttpStatusCode.OK)
			{
				return statusCode <= (HttpStatusCode)299;
			}
			return false;
		}

		public async Task<IWebApiResponse> OnRequestAsync(MiddlewareContext context, Func<MiddlewareContext, CancellationToken, Task<IWebApiResponse>> callNext, CancellationToken cancellationToken = default(CancellationToken))
		{
			Stopwatch requestTimer = Stopwatch.StartNew();
			IWebApiResponse response = await callNext(context, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if ((int)((IWebApiResponse<string>)(object)response).get_CacheState() == 0)
				{
					if (!_config.ApiReportSuccess && IsSuccessStatusCode(((IWebApiResponse<string>)(object)response).get_StatusCode()))
					{
						return response;
					}
					GeneratedExtensions.GetAsync(GeneratedExtensions.WithHeaders(_config.ApiReportUri, (object)new
					{
						api_endpoint = context.get_Request().get_Options().get_EndpointPath(),
						api_rtt = requestTimer.ElapsedMilliseconds,
						api_rc = (int)((IWebApiResponse<string>)(object)response).get_StatusCode(),
						api_hosts = _apiAddressList,
						etm_version = ((Module)_errorSubmissionModule).get_Version().ToString()
					}, true), default(CancellationToken), (HttpCompletionOption)0);
				}
			}
			catch (Exception)
			{
			}
			return response;
		}
	}
}
