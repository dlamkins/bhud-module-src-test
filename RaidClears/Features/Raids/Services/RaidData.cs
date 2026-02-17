using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Services
{
	[Serializable]
	public class RaidData
	{
		private static readonly HashSet<string> DefaultEventEncounterApiIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "spirit_woods", "bandit_trio", "escort", "twisted_castle", "river_of_souls", "statues_of_grenth", "gate", "camp" };

		[JsonIgnore]
		public static string FILENAME = "raid_data.json";

		[JsonIgnore]
		public static string FILE_URL = Module.STATIC_HOST_URL + Module.STATIC_HOST_API_VERSION + FILENAME;

		[JsonProperty("version")]
		public string Version { get; set; } = "";


		[JsonProperty("secondsInWeek")]
		public int SecondsInWeek { get; set; } = -1;


		[JsonProperty("eventEncounterApiIds")]
		public List<string> EventEncounterApiIds { get; set; } = new List<string>();


		[JsonProperty("powerDamageAssetId")]
		public int PowerDamageAssetId { get; set; } = 993687;


		[JsonProperty("condiDamageAssetId")]
		public int CondiDamageAssetId { get; set; } = 156600;


		[JsonProperty("defianceAssetId")]
		public int DefianceAssetId { get; set; }

		[JsonProperty("mentorAssetId")]
		public int MentorAssetId { get; set; } = 155062;


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

		public bool IsEventEncounter(string apiId)
		{
			if (string.IsNullOrEmpty(apiId))
			{
				return false;
			}
			return ((EventEncounterApiIds != null && EventEncounterApiIds.Count > 0) ? new HashSet<string>(EventEncounterApiIds, StringComparer.OrdinalIgnoreCase) : DefaultEventEncounterApiIds).Contains(apiId);
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

		public BossEncounter GetRaidEncounterByApiId(string apiId)
		{
			foreach (ExpansionRaid expansion in Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					foreach (BossEncounter enc in wing.Encounters)
					{
						if (enc.ApiId == apiId)
						{
							return enc;
						}
					}
					if (wing.Id == apiId)
					{
						return wing.ToBossEncounter();
					}
				}
			}
			BossEncounter strike = Service.StrikeData.GetBossEncounterById(apiId);
			if (strike.Name != "undefined")
			{
				return new BossEncounter
				{
					Abbriviation = strike.Abbriviation,
					ApiId = strike.Id,
					Id = strike.Id,
					AssetId = strike.AssetId,
					Name = strike.Name,
					MapIds = ((strike.MapIds != null && strike.MapIds.Count > 0) ? new List<int>(strike.MapIds) : new List<int>()),
					DailyBountyAchievementId = strike.DailyBountyAchievementId
				};
			}
			return new BossEncounter
			{
				Abbriviation = apiId
			};
		}

		public BossEncounter? GetEncounterByMentorAchievementId(int mentorAchievementId)
		{
			foreach (ExpansionRaid expansion in Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					foreach (BossEncounter enc in wing.Encounters)
					{
						if (enc.MentorAchievementId == mentorAchievementId)
						{
							return enc;
						}
					}
				}
			}
			return null;
		}

		private static FileInfo GetConfigFileInfo()
		{
			return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + FILENAME);
		}

		public void Save()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			string serializedContents = JsonConvert.SerializeObject(this, Formatting.None);
			using StreamWriter writer = new StreamWriter(configFileInfo.FullName, append: false, Encoding.UTF8);
			writer.Write(serializedContents);
			writer.Close();
		}

		public static RaidData Load()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			if (configFileInfo != null && configFileInfo.Exists)
			{
				using (StreamReader reader = new StreamReader(configFileInfo.FullName, Encoding.UTF8))
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
				webClient.Encoding = Encoding.UTF8;
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
