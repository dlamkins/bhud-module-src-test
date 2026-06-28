using System.Net.Http;
using System.Net.Http.Headers;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public static class ModuleHttpClient
	{
		public static string UserAgent => "GeoGuesser-Blish/" + Module.MODULE_VERSION;

		public static HttpClient Instance { get; } = CreateClient();


		private static HttpClient CreateClient()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			((HttpHeaders)val.get_DefaultRequestHeaders()).TryAddWithoutValidation("User-Agent", UserAgent);
			return val;
		}
	}
}
