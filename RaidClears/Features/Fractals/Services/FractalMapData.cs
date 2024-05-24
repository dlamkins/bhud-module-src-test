using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace RaidClears.Features.Fractals.Services
{
	[Serializable]
	public class FractalMapData
	{
		[JsonIgnore]
		public static string FILENAME = "fractal_maps.json";

		[JsonIgnore]
		public static string FILE_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/fractal_maps.json";

		private List<int>? _mapIds;

		[JsonProperty("DailyTier")]
		public List<List<string>> DailyTier { get; set; } = new List<List<string>>();


		[JsonProperty("Recs")]
		public List<List<int>> Recs { get; set; } = new List<List<int>>();


		[JsonProperty("maps")]
		public Dictionary<string, FractalMap> Maps { get; set; } = new Dictionary<string, FractalMap>();


		[JsonProperty("challengeMotes")]
		public int[] ChallengeMotes { get; set; } = new int[0];


		[JsonProperty("scales")]
		public Dictionary<string, string> Scales { get; set; } = new Dictionary<string, string>();


		public FractalMap GetFractalForScale(int scale)
		{
			if (!Scales.ContainsKey(scale.ToString()))
			{
				return new FractalMap();
			}
			string mapName = Scales[scale.ToString()];
			if (Maps.ContainsKey(mapName))
			{
				return Maps[mapName];
			}
			return new FractalMap();
		}

		public FractalMap GetFractalByName(string name)
		{
			if (Maps.ContainsKey(name))
			{
				return Maps[name];
			}
			foreach (FractalMap map in Maps.Values)
			{
				if (map.ApiLabel == name)
				{
					return map;
				}
				if (map.Label == name)
				{
					return map;
				}
			}
			return new FractalMap();
		}

		public FractalMap GetFractalByApiName(string name)
		{
			foreach (FractalMap map in Maps.Values)
			{
				if (map.ApiLabel == name)
				{
					return map;
				}
			}
			return new FractalMap();
		}

		public FractalMap? GetFractalMapById(int mapId)
		{
			foreach (FractalMap map in Maps.Values)
			{
				if (map.MapId == mapId)
				{
					return map;
				}
			}
			return null;
		}

		public List<int> GetFractalMapIds()
		{
			if (_mapIds == null)
			{
				_mapIds = new List<int>();
				foreach (KeyValuePair<string, FractalMap> map in Maps)
				{
					_mapIds!.Add(map.Value.MapId);
				}
			}
			return _mapIds;
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

		public static FractalMapData Load()
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

		private static FractalMapData LoadFileFromCache(string fileText)
		{
			FractalMapData loadedCharacterConfiguration = JsonConvert.DeserializeObject<FractalMapData>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new FractalMapData();
			}
			return loadedCharacterConfiguration;
		}

		public static FractalMapData DownloadFile()
		{
			try
			{
				using WebClient webClient = new WebClient();
				FractalMapData data = JsonConvert.DeserializeObject<FractalMapData>(webClient.DownloadString(FILE_URL));
				if (data == null)
				{
					return new FractalMapData();
				}
				data.Save();
				return data;
			}
			catch (Exception)
			{
				return new FractalMapData();
			}
		}
	}
}
