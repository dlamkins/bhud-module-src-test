using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace LoreBridge.Translation.Translators
{
	public class Google : ITranslator, IDisposable
	{
		[CompilerGenerated]
		private TranslatorConfig _003Cconfig_003EP;

		private const string BaseUrl = "https://translate.google.com/m?&sl={0}&tl={1}&hl={1}&q={2}";

		private static readonly Regex RegexResult = new Regex("(?<=(<div(.*)class=\"result-container\"(.*)>))[\\s\\S]*?(?=(<\\/div>))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private readonly HttpClient _httpClient;

		public Google(TranslatorConfig config)
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
			string url = string.Format("https://translate.google.com/m?&sl={0}&tl={1}&hl={1}&q={2}", "en", targetLang, HttpUtility.UrlEncode(text));
			HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				response.EnsureSuccessStatusCode();
				string responseText = await response.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				Match match = RegexResult.Match(responseText);
				return match.Success ? WebUtility.HtmlDecode(match.Value) : "Error: Failed to translate";
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
