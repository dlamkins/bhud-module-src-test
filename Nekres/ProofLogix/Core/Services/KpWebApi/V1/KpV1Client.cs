using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Nekres.ProofLogix.Core.Utils;
using Newtonsoft.Json.Linq;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1
{
	public class KpV1Client
	{
		private readonly string _uri = "https://killproof.me/api/";

		public async Task<Profile> GetProfile(string id)
		{
			return (await HttpUtil.RetryAsync<Profile>(() => _uri.AppendPathSegments("kp", id).GetAsync(default(CancellationToken), (HttpCompletionOption)0))) ?? Profile.Empty;
		}

		public async Task<Profile> GetProfileByCharacter(string name)
		{
			return (await HttpUtil.RetryAsync<Profile>(() => _uri.AppendPathSegments("character", name, "kp").GetAsync(default(CancellationToken), (HttpCompletionOption)0))) ?? Profile.Empty;
		}

		public async Task<List<Clear>> GetClears(string id)
		{
			return FormatClears(await HttpUtil.RetryAsync<JObject>(() => _uri.AppendPathSegments("clear", id).GetAsync(default(CancellationToken), (HttpCompletionOption)0)));
		}

		public async Task<List<Clear>> GetClearsByCharacter(string name)
		{
			return FormatClears(await HttpUtil.RetryAsync<JObject>(() => _uri.AppendPathSegments("character", name, "clear").GetAsync(default(CancellationToken), (HttpCompletionOption)0)));
		}

		public async Task<bool> Refresh(string id)
		{
			return (await HttpUtil.RetryAsync<Refresh>(() => ("https://killproof.me/proof/" + id + "/refresh").GetAsync(default(CancellationToken), (HttpCompletionOption)0)))?.Status.Equals("ok") ?? false;
		}

		public async Task<bool> CheckProofBusy(string id)
		{
			return (await HttpUtil.RetryAsync<ProofBusy>(() => ("https://killproof.me/proofbusy/" + id).GetAsync(default(CancellationToken), (HttpCompletionOption)0))).Busy != 2;
		}

		public async Task<Opener> GetOpener(string encounter, Opener.ServerRegion region)
		{
			Opener response = await HttpUtil.RetryAsync<Opener>(() => _uri.AppendPathSegment("opener").SetQueryParams("encounter=" + encounter, $"region={region}").GetAsync(default(CancellationToken), (HttpCompletionOption)0));
			if (response == null)
			{
				return Opener.Empty;
			}
			return (response.Volunteers?.Any() ?? false) ? response : Opener.Empty;
		}

		public async Task<AddKey> AddKey(string apiKey, bool opener)
		{
			return (await HttpUtil.RetryAsync<AddKey>(delegate
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Expected O, but got Unknown
				Url url = _uri.AppendPathSegment("addkey");
				JObject val = new JObject();
				val.set_Item("key", JToken.op_Implicit(apiKey));
				val.set_Item("opener", JToken.op_Implicit(Convert.ToInt32(opener)));
				return url.PostJsonAsync((object)val, default(CancellationToken), (HttpCompletionOption)0);
			})) ?? new AddKey
			{
				Error = "No response."
			};
		}

		private static List<Clear> FormatClears(JObject response)
		{
			if (response == null)
			{
				return Enumerable.Empty<Clear>().ToList();
			}
			return response.Properties().Select(delegate(JProperty property)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				JObject val = new JObject();
				string name = property.get_Name();
				val.set_Item(name, property.get_Value());
				return ((JToken)val).ToObject<Clear>();
			}).ToList();
		}
	}
}
