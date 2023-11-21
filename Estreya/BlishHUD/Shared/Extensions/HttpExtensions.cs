using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class HttpExtensions
	{
		public static async Task<T> GetJsonAsync<T>(this HttpResponseMessage responseMessage)
		{
			using Stream stream = await responseMessage.get_Content().ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			JsonSerializer serializer = new JsonSerializer();
			using StreamReader sr = new StreamReader(stream);
			using JsonTextReader jsonTextReader = new JsonTextReader(sr);
			return serializer.Deserialize<T>(jsonTextReader);
		}
	}
}
