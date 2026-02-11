using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Shared.Services
{
	public static class DailyBountyDataService
	{
		private const string FILENAME = "daily_bounties.json";

		private static string FileUrl => Module.STATIC_HOST_URL + Module.STATIC_HOST_API_VERSION + "daily_bounties.json";

		private static FileInfo GetConfigFileInfo()
		{
			return new FileInfo(Path.Combine(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH), "daily_bounties.json"));
		}

		public static DailyBountyData Load()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			if (configFileInfo != null && configFileInfo.Exists)
			{
				using (StreamReader reader = new StreamReader(configFileInfo.FullName, Encoding.UTF8))
				{
					string fileText = reader.ReadToEnd();
					reader.Close();
					return LoadFromJson(fileText);
				}
			}
			return DownloadFile();
		}

		private static DailyBountyData LoadFromJson(string fileText)
		{
			return JsonConvert.DeserializeObject<DailyBountyData>(fileText) ?? new DailyBountyData();
		}

		public static DailyBountyData DownloadFile()
		{
			try
			{
				using WebClient webClient = new WebClient();
				webClient.Encoding = Encoding.UTF8;
				DailyBountyData data = JsonConvert.DeserializeObject<DailyBountyData>(webClient.DownloadString(FileUrl));
				if (data == null)
				{
					return new DailyBountyData();
				}
				data.Save();
				return data;
			}
			catch (Exception ex)
			{
				Module.ModuleLogger.Warn(ex, "Could not download daily bounties data file");
				return new DailyBountyData();
			}
		}

		public static void Save(this DailyBountyData data)
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			string serialized = JsonConvert.SerializeObject(data, Formatting.None);
			using StreamWriter writer = new StreamWriter(configFileInfo.FullName, append: false, Encoding.UTF8);
			writer.Write(serialized);
			writer.Close();
		}
	}
}
