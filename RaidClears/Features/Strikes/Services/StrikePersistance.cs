using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RaidClears.Features.Strikes.Services
{
	[Serializable]
	public class StrikePersistance
	{
		[JsonIgnore]
		public static string FILENAME = "strike_clears.json";

		[JsonProperty("version")]
		public string Version { get; set; } = "3.0.0";


		[JsonProperty("accountClears")]
		public Dictionary<string, Dictionary<string, DateTime>> AccountClears { get; set; } = new Dictionary<string, Dictionary<string, DateTime>>();


		public Dictionary<string, DateTime> GetEmpty()
		{
			Dictionary<string, DateTime> list = new Dictionary<string, DateTime>();
			foreach (ExpansionStrikes expansion in Service.StrikeData.Expansions)
			{
				foreach (StrikeMission miss in expansion.Missions)
				{
					list.Add(miss.Id, default(DateTime));
				}
			}
			return list;
		}

		public void SaveClear(string account, StrikeMission mission)
		{
			if (!AccountClears.TryGetValue(account, out var clears))
			{
				clears = GetEmpty();
				AccountClears.Add(account, clears);
			}
			if (!clears.ContainsKey(mission.Id))
			{
				clears.Add(mission.Id, default(DateTime));
			}
			clears[mission.Id] = DateTime.UtcNow;
			AccountClears[account] = clears;
			Save();
		}

		public void RemoveClear(string account, StrikeMission mission)
		{
			if (!AccountClears.TryGetValue(account, out var clears))
			{
				clears = GetEmpty();
				AccountClears.Add(account, clears);
			}
			if (!clears.ContainsKey(mission.Id))
			{
				clears.Add(mission.Id, default(DateTime));
			}
			clears[mission.Id] = default(DateTime);
			AccountClears[account] = clears;
			Save();
		}

		public void Save()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			string serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);
			using StreamWriter writer = new StreamWriter(configFileInfo.FullName);
			writer.Write(serializedContents);
			writer.Close();
		}

		private static FileInfo GetConfigFileInfo()
		{
			return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + FILENAME);
		}

		public static StrikePersistance Load()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			if (configFileInfo != null && configFileInfo.Exists)
			{
				using (StreamReader reader = new StreamReader(configFileInfo.FullName))
				{
					string fileText = reader.ReadToEnd();
					reader.Close();
					return LoadExistingCharacterConfiguration(fileText);
				}
			}
			return CreateNewCharacterConfiguration();
		}

		private static StrikePersistance LoadExistingCharacterConfiguration(string fileText)
		{
			StrikePersistance loadedCharacterConfiguration = JsonConvert.DeserializeObject<StrikePersistance>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new StrikePersistance();
			}
			return HandleVersionUpgrade(loadedCharacterConfiguration);
		}

		private static StrikePersistance HandleVersionUpgrade(StrikePersistance data)
		{
			if (data.Version == "2.0.0" || data.Version == "2.0.1")
			{
				Dictionary<string, string> keyRename = new Dictionary<string, string>
				{
					{ "ColdWar", "cold_war" },
					{ "Fraenir", "fraenir_of_jormag" },
					{ "ShiverpeaksPass", "shiverpeak_pass" },
					{ "VoiceAndClaw", "voice_and_claw" },
					{ "Whisper", "whisper_of_jormag" },
					{ "Boneskinner", "boneskinner" },
					{ "AetherbladeHideout", "aetherblade_hideout" },
					{ "Junkyard", "xunlai_jade_junkyard" },
					{ "Overlook", "kaineng_overlook" },
					{ "HarvestTemple", "harvest_temple" },
					{ "OldLionsCourt", "old_lion_court" },
					{ "DragonStorm", "dragonstorm" },
					{ "CosmicObservatory", "cosmic_observatory" },
					{ "TempleOfFebe", "temple_of_febe" }
				};
				StrikePersistance newFile = new StrikePersistance();
				foreach (KeyValuePair<string, Dictionary<string, DateTime>> account in data.AccountClears)
				{
					Dictionary<string, DateTime> acctclears = newFile.GetEmpty();
					foreach (KeyValuePair<string, DateTime> clear in account.Value)
					{
						if (keyRename.TryGetValue(clear.Key, out var renameTo))
						{
							if (acctclears.ContainsKey(renameTo))
							{
								acctclears[renameTo] = clear.Value;
							}
							else
							{
								acctclears.Add(renameTo, clear.Value);
							}
						}
					}
					newFile.AccountClears.Add(account.Key, acctclears);
				}
				newFile.Save();
				return newFile;
			}
			if (data.Version == "3.0.0")
			{
				return data;
			}
			return new StrikePersistance();
		}

		private static StrikePersistance CreateNewCharacterConfiguration()
		{
			StrikePersistance newCharacterConfiguration = new StrikePersistance();
			newCharacterConfiguration.AccountClears.Add("default", newCharacterConfiguration.GetEmpty());
			newCharacterConfiguration.Save();
			return newCharacterConfiguration;
		}
	}
}
