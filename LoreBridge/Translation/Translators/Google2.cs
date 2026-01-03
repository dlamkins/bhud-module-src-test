using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace LoreBridge.Translation.Translators
{
	public class Google2 : ITranslator, IDisposable
	{
		[CompilerGenerated]
		private TranslatorConfig _003Cconfig_003EP;

		private const string ApiUrl = "https://translate.googleapis.com/translate_a/single";

		private const string ApiUrlParams = "client=gtx&sl={0}&tl={1}&dt=t&q={2}";

		private readonly HttpClient _httpClient;

		public Google2(TranslatorConfig config)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			_003Cconfig_003EP = config;
			_httpClient = new HttpClient();
			base._002Ector();
		}

		public async Task<string> TranslateAsync(string text)
		{
			string targetLang = _003Cconfig_003EP.TargetLang.IsoCode.ToLower();
			string url = "https://translate.googleapis.com/translate_a/single?" + string.Format("client=gtx&sl={0}&tl={1}&dt=t&q={2}", "en", targetLang, HttpUtility.UrlEncode(text));
			HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				response.EnsureSuccessStatusCode();
				string responseText = await response.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				return string.Join(string.Empty, ((IEnumerable<JToken>)JArray.Parse(responseText).get_Item(0)).Select((JToken x) => x.get_Item((object)0)));
			}
			finally
			{
				((IDisposable)response)?.Dispose();
			}
		}

		public void Dispose()
		{
			((HttpMessageInvoker)_httpClient).Dispose();
		}
	}
}
