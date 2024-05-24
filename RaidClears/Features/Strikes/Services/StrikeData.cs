using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Blish_HUD.Settings;
using Newtonsoft.Json;

namespace RaidClears.Features.Strikes.Services
{
	[Serializable]
	public class StrikeData
	{
		[JsonIgnore]
		public static string FILENAME = "strike_data.json";

		[JsonIgnore]
		public static string FILE_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/strike_data.json";

		[JsonProperty("version")]
		public string Version { get; set; } = "";


		[JsonProperty("priority")]
		public ExpansionStrikes Priority { get; set; } = new ExpansionStrikes();


		[JsonProperty("expansions")]
		public List<ExpansionStrikes> Expansions { get; set; } = new List<ExpansionStrikes>();


		public ExpansionStrikes GetExpansionStrikesById(string id)
		{
			foreach (ExpansionStrikes expansion in Expansions)
			{
				if (expansion.Id == id)
				{
					return expansion;
				}
			}
			return new ExpansionStrikes();
		}

		public ExpansionStrikes GetExpansionStrikesByName(string name)
		{
			foreach (ExpansionStrikes expansion in Expansions)
			{
				if (expansion.Name == name)
				{
					return expansion;
				}
			}
			return new ExpansionStrikes();
		}

		public StrikeMission? GetStrikeMisisonByMapId(int id)
		{
			foreach (ExpansionStrikes expansion in Expansions)
			{
				foreach (StrikeMission mission in expansion.Missions)
				{
					if (mission.MapIds.Contains(id))
					{
						return mission;
					}
				}
			}
			return null;
		}

		public StrikeMission GetStrikeMissionById(string name)
		{
			foreach (ExpansionStrikes expansion in Expansions)
			{
				foreach (StrikeMission mission in expansion.Missions)
				{
					if (mission.Id == name)
					{
						return mission;
					}
				}
			}
			return new StrikeMission();
		}

		public StrikeMission GetStrikeMissionByName(string name)
		{
			foreach (ExpansionStrikes expansion in Expansions)
			{
				foreach (StrikeMission mission in expansion.Missions)
				{
					if (mission.Name == name)
					{
						return mission;
					}
				}
			}
			return new StrikeMission();
		}

		public string GetStrikeMissionResetById(string name)
		{
			foreach (ExpansionStrikes expansion in Expansions)
			{
				foreach (StrikeMission mission in expansion.Missions)
				{
					if (mission.Id == name)
					{
						return expansion.Resets;
					}
				}
			}
			return "weekly";
		}

		public List<StrikeInfo> GetPriorityStrikes(int index)
		{
			List<StrikeInfo> list = new List<StrikeInfo>();
			foreach (ExpansionStrikes expansion in Expansions)
			{
				int todayIndex = (index + expansion.DailyPriorityOffset) % expansion.DailyPriorityModulo;
				int tomorrowIndex = (index + expansion.DailyPriorityOffset + 1) % expansion.DailyPriorityModulo;
				if (expansion.Missions.Count() >= todayIndex && expansion.Missions.Count() >= tomorrowIndex)
				{
					list.Add(new StrikeInfo(expansion.Missions[todayIndex], expansion.Missions[tomorrowIndex]));
				}
			}
			return list;
		}

		public SettingEntry<bool> GetPriorityVisible()
		{
			return Service.StrikeSettings.GetPriorityVisible(Priority);
		}

		public SettingEntry<bool> GetExpansionVisible(ExpansionStrikes expac)
		{
			return Service.StrikeSettings.GetExpansionVisible(expac);
		}

		public SettingEntry<bool> GetMissionVisible(StrikeMission mission)
		{
			return Service.StrikeSettings.GetMissionVisible(mission);
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

		public static StrikeData Load()
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

		private static StrikeData LoadFileFromCache(string fileText)
		{
			StrikeData loadedCharacterConfiguration = JsonConvert.DeserializeObject<StrikeData>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new StrikeData();
			}
			return loadedCharacterConfiguration;
		}

		public static StrikeData DownloadFile()
		{
			try
			{
				using WebClient webClient = new WebClient();
				StrikeData data = JsonConvert.DeserializeObject<StrikeData>(webClient.DownloadString(FILE_URL));
				if (data == null)
				{
					return new StrikeData();
				}
				data.Save();
				return data;
			}
			catch (Exception)
			{
				return new StrikeData();
			}
		}
	}
}
