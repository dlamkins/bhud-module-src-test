using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Enums;

namespace RaidClears.Features.Strikes.Services
{
	[Serializable]
	public class StrikePersistance
	{
		[JsonIgnore]
		public static string FILENAME = "strike_clears.json";

		[JsonProperty("version")]
		public string Version { get; set; } = "2.0.1";


		[JsonProperty("accountClears")]
		public Dictionary<string, Dictionary<Encounters.StrikeMission, DateTime>> AccountClears { get; set; } = new Dictionary<string, Dictionary<Encounters.StrikeMission, DateTime>>();


		public Dictionary<Encounters.StrikeMission, DateTime> GetEmpty()
		{
			return new Dictionary<Encounters.StrikeMission, DateTime>
			{
				{
					Encounters.StrikeMission.ColdWar,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.Fraenir,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.ShiverpeaksPass,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.VoiceAndClaw,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.Whisper,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.Boneskinner,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.AetherbladeHideout,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.Junkyard,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.Overlook,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.HarvestTemple,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.OldLionsCourt,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.DragonStorm,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.CosmicObservatory,
					default(DateTime)
				},
				{
					Encounters.StrikeMission.TempleOfFebe,
					default(DateTime)
				}
			};
		}

		public void SaveClear(string account, Encounters.StrikeMission mission)
		{
			if (!AccountClears.TryGetValue(account, out var clears))
			{
				clears = GetEmpty();
				AccountClears.Add(account, clears);
			}
			clears[mission] = DateTime.UtcNow;
			AccountClears[account] = clears;
			Save();
		}

		public void RemoveClear(string account, Encounters.StrikeMission mission)
		{
			if (!AccountClears.TryGetValue(account, out var clears))
			{
				clears = GetEmpty();
				AccountClears.Add(account, clears);
			}
			clears[mission] = default(DateTime);
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
			return loadedCharacterConfiguration;
		}

		private static StrikePersistance CreateNewCharacterConfiguration()
		{
			StrikePersistance newCharacterConfiguration = new StrikePersistance();
			newCharacterConfiguration.AccountClears.Add("test", newCharacterConfiguration.GetEmpty());
			newCharacterConfiguration.Save();
			return newCharacterConfiguration;
		}
	}
}
