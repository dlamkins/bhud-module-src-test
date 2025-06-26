using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public class DynamicConfigService : IDisposable
	{
		private static readonly HttpClient HttpClient = new HttpClient();

		private static readonly Logger Logger = Logger.GetLogger<DynamicConfigService>();

		protected async Task<T?> Fetch<T>(string url)
		{
			_ = 1;
			try
			{
				HttpResponseMessage obj = await HttpClient.GetAsync(url);
				obj.EnsureSuccessStatusCode();
				return JsonConvert.DeserializeObject<T>(await obj.get_Content().ReadAsStringAsync());
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch data from " + url);
				return default(T);
			}
		}

		public async Task<ConfigModel> LoadConfig()
		{
			string path = Module.STATIC_HOST_URL + "/geoguesser_v3.json";
			Logger.Info("Getting static config from " + path);
			ConfigModel config = await Fetch<ConfigModel>(path);
			if (config == null)
			{
				Logger.Warn("Static config was null");
				return new ConfigModel();
			}
			return config;
		}

		public void Dispose()
		{
			((HttpMessageInvoker)HttpClient).Dispose();
		}
	}
}
