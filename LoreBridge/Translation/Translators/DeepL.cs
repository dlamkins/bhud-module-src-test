using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LoreBridge.Translation.Translators
{
	public class DeepL : ITranslator, IDisposable
	{
		private const string ApiUrl = "https://www2.deepl.com/jsonrpc";

		private readonly TranslatorConfig _config;

		private readonly CookieContainer _cookies;

		private readonly HttpClient _httpClient;

		private long Id { get; set; }

		public DeepL(TranslatorConfig config)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			_config = config;
			Id = GenerateId();
			_cookies = new CookieContainer();
			HttpClientHandler val = new HttpClientHandler();
			val.set_CookieContainer(_cookies);
			HttpClientHandler handler = val;
			_httpClient = new HttpClient((HttpMessageHandler)(object)handler);
			_httpClient.get_DefaultRequestHeaders().get_Accept().Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_httpClient.get_DefaultRequestHeaders().set_Referrer(new Uri("https://www.deepl.com/"));
		}

		public async Task<string> TranslateAsync(string text)
		{
			throw new Exception("DeepL translator not implemented");
		}

		public void Dispose()
		{
			((HttpMessageInvoker)_httpClient).Dispose();
		}

		private long GenerateId()
		{
			long num = 10000L;
			Random random = new Random();
			return num * (long)Math.Round((double)num * random.NextDouble());
		}
	}
}
