using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class StaticHosting
	{
		public static string BaseUrl = "https://bhm.blishhud.com/Kenedia.Modules.BuildsManager/";

		public static async Task<StaticVersion> GetStaticVersion()
		{
			string url = BaseUrl + "DataMap.json";
			string content = string.Empty;
			try
			{
				HttpClient httpClient = new HttpClient();
				try
				{
					content = await httpClient.GetStringAsync(url);
					return JsonConvert.DeserializeObject<StaticVersion>(content, SerializerSettings.Default);
				}
				finally
				{
					((IDisposable)httpClient)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Failed to get versions from " + url);
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Fetched content: " + content);
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn($"{ex}");
			}
			return new StaticVersion();
		}

		public static async Task<ByteIntMap> GetItemMap(string fileName, CancellationToken cancellationToken)
		{
			string url = BaseUrl + fileName + ".json";
			string content = string.Empty;
			try
			{
				HttpClient httpClient = new HttpClient();
				try
				{
					HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);
					if (cancellationToken.IsCancellationRequested)
					{
						return null;
					}
					content = await response.get_Content().ReadAsStringAsync();
					return JsonConvert.DeserializeObject<ByteIntMap>(content, SerializerSettings.Default);
				}
				finally
				{
					((IDisposable)httpClient)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Failed to get item map from " + url);
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn("Fetched content: " + content);
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn($"{ex}");
			}
			return null;
		}
	}
}
