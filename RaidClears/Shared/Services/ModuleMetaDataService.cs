using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace RaidClears.Shared.Services
{
	[Serializable]
	public class ModuleMetaDataService
	{
		[JsonIgnore]
		public static string FILENAME = "clears_tracker.json";

		[JsonIgnore]
		public static string FILE_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/clears_tracker.json";

		[JsonProperty("fracal_instabilities")]
		public string Instabilities { get; set; }

		[JsonProperty("fractal_map_data")]
		public string MapData { get; set; }

		private static FileInfo GetConfigFileInfo()
		{
			return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + FILENAME);
		}

		public void Save()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			string serializedContents = JsonConvert.SerializeObject(this, Formatting.None);
			using StreamWriter writer = new StreamWriter(configFileInfo.FullName);
			writer.Write(serializedContents);
			writer.Close();
		}

		public static ModuleMetaDataService Load()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			if (configFileInfo != null && configFileInfo.Exists)
			{
				DateTime lastWriteTime = configFileInfo.LastWriteTime;
				if ((DateTime.Now - lastWriteTime).TotalDays < 1.0)
				{
					using (StreamReader reader = new StreamReader(configFileInfo.FullName))
					{
						string fileText = reader.ReadToEnd();
						reader.Close();
						return LoadFileFromCache(fileText);
					}
				}
			}
			return DownloadFile();
		}

		private static ModuleMetaDataService LoadFileFromCache(string fileText)
		{
			ModuleMetaDataService loadedCharacterConfiguration = JsonConvert.DeserializeObject<ModuleMetaDataService>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new ModuleMetaDataService();
			}
			return loadedCharacterConfiguration;
		}

		private static ModuleMetaDataService DownloadFile()
		{
			try
			{
				using WebClient webClient = new WebClient();
				ModuleMetaDataService instabs = JsonConvert.DeserializeObject<ModuleMetaDataService>(webClient.DownloadString(FILE_URL));
				if (instabs == null)
				{
					return new ModuleMetaDataService();
				}
				instabs.Save();
				return instabs;
			}
			catch (Exception)
			{
				return new ModuleMetaDataService();
			}
		}
	}
}
