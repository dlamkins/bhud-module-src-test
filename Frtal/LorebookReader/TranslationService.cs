using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Frtal.LorebookReader
{
	public static class TranslationService
	{
		public static readonly (string Code, string Name)[] TargetLanguages;

		private const string Endpoint = "https://translate.googleapis.com/translate_a/single";

		static TranslationService()
		{
			TargetLanguages = new(string, string)[11]
			{
				("cs", "Czech (Čeština)"),
				("de", "German (Deutsch)"),
				("es", "Spanish (Español)"),
				("fr", "French (Français)"),
				("it", "Italian (Italiano)"),
				("pl", "Polish (Polski)"),
				("pt", "Portuguese (Português)"),
				("ru", "Russian (Русский)"),
				("ja", "Japanese (日本語)"),
				("ko", "Korean (한국어)"),
				("zh-CN", "Chinese (中文)")
			};
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
		}

		public static async Task<string> TranslateAsync(string text, string targetLang, string sourceLang = "auto", CancellationToken ct = default(CancellationToken))
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + Uri.EscapeDataString(sourceLang) + "&tl=" + Uri.EscapeDataString(targetLang) + "&dt=t&q=" + Uri.EscapeDataString(text);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";
			request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36";
			request.Timeout = 15000;
			using (ct.Register(delegate
			{
				try
				{
					request.Abort();
				}
				catch
				{
				}
			}))
			{
				using HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(continueOnCapturedContext: false));
				using Stream stream = response.GetResponseStream();
				using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
				return ParseTranslation(await reader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false));
			}
		}

		private static string ParseTranslation(string json)
		{
			object[] root = new JavaScriptSerializer().DeserializeObject(json) as object[];
			if (root != null && root.Length != 0)
			{
				object[] segments = root[0] as object[];
				if (segments != null)
				{
					StringBuilder sb = new StringBuilder();
					object[] array = segments;
					for (int i = 0; i < array.Length; i++)
					{
						object[] seg = array[i] as object[];
						if (seg != null && seg.Length != 0 && seg[0] != null)
						{
							sb.Append(seg[0].ToString());
						}
					}
					string text = sb.ToString();
					if (text.Length == 0)
					{
						throw new FormatException("Empty translation result.");
					}
					return text;
				}
			}
			throw new FormatException("Unexpected translation response.");
		}
	}
}
