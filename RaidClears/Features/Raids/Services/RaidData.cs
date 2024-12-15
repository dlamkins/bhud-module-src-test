using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;

namespace RaidClears.Features.Raids.Services
{
	[Serializable]
	public class RaidData
	{
		[JsonIgnore]
		public static string FILENAME = "raid_data.json";

		[JsonIgnore]
		public static string FILE_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/raid_data.json";

		[JsonProperty("version")]
		public string Version { get; set; } = "";


		[JsonProperty("secondsInWeek")]
		public int SecondsInWeek { get; set; } = -1;


		[JsonProperty("aerodrome")]
		public Aerodrome AeroDrome { get; set; } = new Aerodrome();


		[JsonProperty("expansions")]
		public List<ExpansionRaid> Expansions { get; set; } = new List<ExpansionRaid>();


		public ExpansionRaid GetExpansionRaidsById(string id)
		{
			foreach (ExpansionRaid expansion in Expansions)
			{
				if (expansion.Id == id)
				{
					return expansion;
				}
			}
			return new ExpansionRaid();
		}

		public ExpansionRaid GetExpansionRaidByName(string name)
		{
			foreach (ExpansionRaid expansion in Expansions)
			{
				if (expansion.Name == name)
				{
					return expansion;
				}
			}
			return new ExpansionRaid();
		}

		public RaidWing GetRaidWingByMapId(int id)
		{
			foreach (ExpansionRaid expansion in Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					if (wing.MapId == id)
					{
						return wing;
					}
				}
			}
			return new RaidWing();
		}

		public RaidWing GetRaidWingById(string id)
		{
			foreach (ExpansionRaid expansion in Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					if (wing.Id == id)
					{
						return wing;
					}
				}
			}
			return new RaidWing();
		}

		public RaidWing GetRaidWingByZeroIndex(int idx)
		{
			foreach (ExpansionRaid expansion in Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					if (wing.Number == idx + 1)
					{
						return wing;
					}
				}
			}
			return new RaidWing();
		}

		public RaidWing GetRaidWingByIndex(int idx)
		{
			foreach (ExpansionRaid expansion in Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					if (wing.Number == idx)
					{
						return wing;
					}
				}
			}
			return new RaidWing();
		}

		public RaidEncounter GetRaidEncounterByApiId(string apiId)
		{
			foreach (ExpansionRaid expansion in Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					foreach (RaidEncounter enc in wing.Encounters)
					{
						if (enc.ApiId == apiId)
						{
							return enc;
						}
					}
					if (wing.Id == apiId)
					{
						return wing.ToRaidEncounter();
					}
				}
			}
			return new RaidEncounter
			{
				Abbriviation = apiId
			};
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

		public static RaidData Load()
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

		private static RaidData LoadFileFromCache(string fileText)
		{
			RaidData loadedCharacterConfiguration = JsonConvert.DeserializeObject<RaidData>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new RaidData();
			}
			return loadedCharacterConfiguration;
		}

		public static RaidData DownloadFile()
		{
			try
			{
				using WebClient webClient = new WebClient();
				RaidData data = JsonConvert.DeserializeObject<RaidData>(webClient.DownloadString(FILE_URL));
				if (data == null)
				{
					return new RaidData();
				}
				data.Save();
				return data;
			}
			catch (Exception)
			{
				return new RaidData();
			}
		}
	}
}
