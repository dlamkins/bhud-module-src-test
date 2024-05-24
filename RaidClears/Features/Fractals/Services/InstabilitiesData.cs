using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace RaidClears.Features.Fractals.Services
{
	[Serializable]
	public class InstabilitiesData
	{
		[JsonIgnore]
		public static string FILENAME = "instabilities.json";

		[JsonIgnore]
		public static string FILE_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/fractal_instabilities.json";

		[JsonProperty("instabilities")]
		public Dictionary<string, int[][]> Instabilities { get; set; } = new Dictionary<string, int[][]>();


		[JsonProperty("instability_names")]
		public string[] Names { get; set; } = new string[0];


		public List<string> GetInstabsForLevelOnDay(int level, int day)
		{
			List<string> instabs = new List<string>();
			Instabilities.TryGetValue(level.ToString(), out var value);
			if (value != null && value.Length >= day)
			{
				int[] array = value[day];
				foreach (int i in array)
				{
					if (Names.Length >= i)
					{
						instabs.Add(Names[i]);
					}
				}
			}
			return instabs;
		}

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

		public static InstabilitiesData Load()
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
			return DownloadFile();
		}

		private static InstabilitiesData LoadFileFromCache(string fileText)
		{
			InstabilitiesData loadedCharacterConfiguration = JsonConvert.DeserializeObject<InstabilitiesData>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new InstabilitiesData();
			}
			return loadedCharacterConfiguration;
		}

		public static InstabilitiesData DownloadFile()
		{
			try
			{
				using WebClient webClient = new WebClient();
				InstabilitiesData instabs = JsonConvert.DeserializeObject<InstabilitiesData>(webClient.DownloadString(FILE_URL));
				if (instabs == null)
				{
					return new InstabilitiesData();
				}
				instabs.Save();
				return instabs;
			}
			catch (Exception)
			{
				return new InstabilitiesData();
			}
		}
	}
}
