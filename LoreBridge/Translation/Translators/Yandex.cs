using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LoreBridge.Translation.Models;
using LoreBridge.Translation.Utils;

namespace LoreBridge.Translation.Translators
{
	public class Yandex : ITranslator, IDisposable
	{
		private const string ApiUrl = "https://translate.yandex.net/api/v1/tr.json";

		private const string UserAgent = "ru.yandex.translate/3.20.2024";

		private readonly TranslatorConfig _config;

		private readonly HttpClient _httpClient = new HttpClient();

		private readonly YandexUcid _ucid = new YandexUcid();

		public Yandex(TranslatorConfig config)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			_config = config;
			_httpClient.get_DefaultRequestHeaders().get_UserAgent().ParseAdd("ru.yandex.translate/3.20.2024");
		}

		public async Task<string> TranslateAsync(string text)
		{
			string targetLang = _config.TargetLang.IsoCode;
			string query = $"?ucid={_ucid.Get():N}&srv=android&format=text";
			Dictionary<string, string> data = new Dictionary<string, string>
			{
				{ "text", text },
				{
					"lang",
					"en-" + targetLang
				}
			};
			FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)data);
			try
			{
				HttpResponseMessage response = await _httpClient.PostAsync(new Uri("https://translate.yandex.net/api/v1/tr.json/translate" + query), (HttpContent)(object)content).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					response.EnsureSuccessStatusCode();
					YandexResponse result = JsonSerializer.Deserialize<YandexResponse>(response.get_Content().ReadAsStringAsync().Result, (JsonSerializerOptions)null);
					if (result.Code != HttpStatusCode.OK)
					{
						throw new Exception(result.Message);
					}
					return result.Text[0];
				}
				finally
				{
					((IDisposable)response)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)content)?.Dispose();
			}
		}

		public void Dispose()
		{
			((HttpMessageInvoker)_httpClient).Dispose();
		}
	}
}
