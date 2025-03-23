using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.BHUDCommon;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	public abstract class HsApiCache<TId, TItem> : ApiCache<TId, TItem> where TItem : IIdentifiable<TId>
	{
		protected abstract string Endpoint { get; }

		protected abstract HttpClient Client { get; }

		protected override async Task<IEnumerable<TItem>> ApiGetter(CancellationToken ct)
		{
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Get(), "https://gw2-api.hardstuck.gg/" + Endpoint + "/all");
			try
			{
				HttpResponseMessage response = await ((HttpMessageInvoker)Client).SendAsync(request, ct);
				try
				{
					string value = await response.get_Content().ReadAsStringAsync();
					ct.ThrowIfCancellationRequested();
					return JsonConvert.DeserializeObject<List<TItem>>(value) ?? throw new Exception("Hs api request deserialize resulted in null.");
				}
				finally
				{
					((IDisposable)response)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)request)?.Dispose();
			}
		}
	}
}
