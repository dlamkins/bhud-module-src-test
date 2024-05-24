using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Strikes.Services;

namespace RaidClears.Shared.Services
{
	[Serializable]
	public class ModuleMetaDataService
	{
		[JsonIgnore]
		public static string FILENAME = "clears_tracker.json";

		[JsonIgnore]
		public static string FILE_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/clears_tracker.json";

		[JsonProperty("fractal_instabilities")]
		public string InstabilitiesVersion { get; set; }

		[JsonProperty("fractal_map_data")]
		public string FractalMapVersion { get; set; }

		[JsonProperty("strike_data")]
		public string StrikeDataVersion { get; set; }

		[JsonProperty("assets")]
		public List<string> Assets { get; set; } = new List<string>();


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

		public static void CheckVersions()
		{
			ModuleMetaDataService webFile = DownloadFile();
			ModuleMetaDataService localFile = Load();
			if (webFile.InstabilitiesVersion != localFile.InstabilitiesVersion)
			{
				InstabilitiesData.DownloadFile();
				Module.ModuleLogger.Info("JSON File: Instababilites UPDATED to version " + webFile.InstabilitiesVersion);
			}
			else
			{
				Module.ModuleLogger.Info("JSON File: Instababilites are current on version " + webFile.InstabilitiesVersion);
			}
			if (webFile.FractalMapVersion != localFile.FractalMapVersion)
			{
				FractalMapData.DownloadFile();
				Module.ModuleLogger.Info("JSON File: Fractal Map Data UPDATED to version " + webFile.FractalMapVersion);
			}
			else
			{
				Module.ModuleLogger.Info("JSON File: Fractal Map Data is current on version " + webFile.FractalMapVersion);
			}
			if (webFile.StrikeDataVersion != localFile.StrikeDataVersion)
			{
				StrikeData.DownloadFile();
				Module.ModuleLogger.Info("JSON File: Strike Data UPDATED to version " + webFile.StrikeDataVersion);
			}
			else
			{
				Module.ModuleLogger.Info("JSON File: Strike Data is current on version " + webFile.StrikeDataVersion);
			}
			webFile.Save();
			webFile.ValidateAssetCache(webFile.Assets);
		}

		public void ValidateAssetCache(List<string> assets)
		{
			DownloadTextureService _textures = new DownloadTextureService();
			foreach (string asset in assets)
			{
				_textures.ValidateTextureCache(asset);
			}
		}

		public static ModuleMetaDataService Load()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			if (configFileInfo != null && configFileInfo.Exists)
			{
				using (StreamReader reader = new StreamReader(configFileInfo.FullName))
				{
					string fileText = reader.ReadToEnd();
					reader.Close();
					return LoadFileFromCache(fileText);
				}
			}
			return new ModuleMetaDataService();
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
				ModuleMetaDataService metaFile = JsonConvert.DeserializeObject<ModuleMetaDataService>(webClient.DownloadString(FILE_URL));
				if (metaFile == null)
				{
					return new ModuleMetaDataService();
				}
				return metaFile;
			}
			catch (Exception)
			{
				return new ModuleMetaDataService();
			}
		}
	}
}
