using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.Core.Services
{
	public class StaticHosting
	{
		public virtual string BaseUrl { get; } = "https://bhm.blishhud.com/Kenedia.Modules.BuildsManager/";


		public Logger Logger { get; }

		public StaticHosting(Logger logger)
		{
			Logger = logger;
		}

		public async Task<T> GetStaticContent<T>(string fileName)
		{
			string url = BaseUrl + fileName;
			string content = string.Empty;
			try
			{
				HttpClient httpClient = new HttpClient();
				try
				{
					content = await httpClient.GetStringAsync(url);
					return JsonConvert.DeserializeObject<T>(content, SerializerSettings.Default);
				}
				finally
				{
					((IDisposable)httpClient)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to get " + fileName + " from " + url);
				Logger.Warn("Fetched content: " + content);
				Logger.Warn($"{ex}");
			}
			return default(T);
		}
	}
}
