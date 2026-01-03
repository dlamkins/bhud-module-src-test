using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoreBridge.Translation.Models;

namespace LoreBridge.Translation.Translators
{
	public class LibreTranslate : ITranslator, IDisposable
	{
		[CompilerGenerated]
		private TranslatorConfig _003Cconfig_003EP;

		private readonly HttpClient _httpClient;

		public LibreTranslate(TranslatorConfig config)
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
			string url = _003Cconfig_003EP.ApiUrl;
			if (string.IsNullOrWhiteSpace(text))
			{
				return "Error: No API URL provided";
			}
			StringContent content = new StringContent(JsonSerializer.Serialize<LibreTranslateRequest>(new LibreTranslateRequest
			{
				Source = "en",
				Target = targetLang,
				Query = text,
				Format = "text",
				Alternatives = 0
			}, (JsonSerializerOptions)null), Encoding.UTF8, "application/json");
			try
			{
				HttpResponseMessage response = await _httpClient.PostAsync(url + "/translate", (HttpContent)(object)content).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					response.EnsureSuccessStatusCode();
					return JsonSerializer.Deserialize<LibreTranslateResponse>(await response.get_Content().ReadAsStringAsync(), (JsonSerializerOptions)null).TranslatedText;
				}
				finally
				{
					((IDisposable)response)?.Dispose();
				}
			}
			catch (Exception e)
			{
				return "Error: " + e.Message;
			}
		}

		public void Dispose()
		{
			((HttpMessageInvoker)_httpClient).Dispose();
		}
	}
}
