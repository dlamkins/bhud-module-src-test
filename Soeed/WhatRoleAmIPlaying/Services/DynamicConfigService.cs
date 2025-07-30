using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json;
using Soeed.WhatRoleAmIPlaying.Models;

namespace Soeed.WhatRoleAmIPlaying.Services
{
	public class DynamicConfigService : IDisposable
	{
		private static readonly HttpClient HttpClient = new HttpClient();

		protected async Task<T?> Fetch<T>(string url) where T : class
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
				Logger.GetLogger<WhatRoleAmIPlayingModule>().Warn(ex, "Failed to fetch config from " + url);
				return null;
			}
		}

		public async Task<RoleConfig?> LoadConfig()
		{
			string path = WhatRoleAmIPlayingModule.STATIC_HOST_URL + "/roles.json";
			Logger.GetLogger<WhatRoleAmIPlayingModule>().Info("Loading role config from " + path);
			RoleConfig config = await Fetch<RoleConfig>(path);
			if (config == null)
			{
				Logger.GetLogger<WhatRoleAmIPlayingModule>().Error("Failed to load role configuration");
				return new RoleConfig();
			}
			Logger.GetLogger<WhatRoleAmIPlayingModule>().Info($"Loaded {config.Roles.Count} roles from configuration");
			return config;
		}

		public void Dispose()
		{
		}
	}
}
