using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Services;

namespace RaidClears.Features.Fractals.Services
{
	[Serializable]
	public class FractalPersistance : Labelable
	{
		[JsonIgnore]
		public static string FILENAME = "fractal_clears.json";

		[JsonProperty("version")]
		public string Version { get; set; } = "3.0.0";


		[JsonProperty("accountClears")]
		public Dictionary<string, Dictionary<string, DateTime>> AccountClears { get; set; } = new Dictionary<string, Dictionary<string, DateTime>>();


		public FractalPersistance()
		{
			_isFractal = true;
		}

		public Dictionary<string, DateTime> GetEmpty()
		{
			Dictionary<string, DateTime> empty = new Dictionary<string, DateTime>();
			foreach (KeyValuePair<string, FractalMap> map in Service.FractalMapData.Maps)
			{
				empty.Add(map.Value.ApiLabel, default(DateTime));
			}
			return empty;
		}

		public override void SetEncounterLabel(string encounterApiId, string label)
		{
			if (base.EncounterLabels.ContainsKey(encounterApiId))
			{
				base.EncounterLabels.Remove(encounterApiId);
			}
			base.EncounterLabels.Add(encounterApiId, label);
			Service.FractalWindow.UpdateEncounterLabel(encounterApiId, label);
			Save();
		}

		public void SaveClear(string account, FractalMap mission)
		{
			if (!AccountClears.TryGetValue(account, out var clears))
			{
				clears = GetEmpty();
				AccountClears.Add(account, clears);
			}
			if (!clears.ContainsKey(mission.ApiLabel))
			{
				clears.Add(mission.ApiLabel, default(DateTime));
			}
			clears[mission.ApiLabel] = DateTime.UtcNow;
			AccountClears[account] = clears;
			Save();
		}

		public void RemoveClear(string account, FractalMap mission)
		{
			if (!AccountClears.TryGetValue(account, out var clears))
			{
				clears = GetEmpty();
				AccountClears.Add(account, clears);
			}
			if (!clears.ContainsKey(mission.ApiLabel))
			{
				clears.Add(mission.ApiLabel, default(DateTime));
			}
			clears[mission.ApiLabel] = default(DateTime);
			AccountClears[account] = clears;
			Save();
		}

		public override void Save()
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

		public static FractalPersistance Load()
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

		private static FractalPersistance LoadExistingCharacterConfiguration(string fileText)
		{
			FractalPersistance loadedCharacterConfiguration = JsonConvert.DeserializeObject<FractalPersistance>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new FractalPersistance();
			}
			return HandleVersionUpgrade(loadedCharacterConfiguration);
		}

		private static FractalPersistance HandleVersionUpgrade(FractalPersistance data)
		{
			if (data.Version == "2.0.0" || data.Version == "2.2.0")
			{
				FractalPersistance newSaveFile = new FractalPersistance();
				foreach (KeyValuePair<string, Dictionary<string, DateTime>> account in data.AccountClears)
				{
					Dictionary<string, DateTime> acctClears = new Dictionary<string, DateTime>();
					foreach (KeyValuePair<string, DateTime> line in account.Value)
					{
						FractalMap frac = Service.FractalMapData.GetFractalByName(line.Key.Replace("Fractal", ""));
						if (frac.ApiLabel != "undefined")
						{
							acctClears.Add(frac.ApiLabel, line.Value);
						}
					}
					newSaveFile.AccountClears.Add(account.Key, acctClears);
				}
				newSaveFile.Save();
				return newSaveFile;
			}
			if (data.Version == "3.0.0")
			{
				return data;
			}
			return new FractalPersistance();
		}

		private static FractalPersistance CreateNewCharacterConfiguration()
		{
			FractalPersistance newCharacterConfiguration = new FractalPersistance();
			newCharacterConfiguration.AccountClears.Add("default", newCharacterConfiguration.GetEmpty());
			newCharacterConfiguration.Save();
			return newCharacterConfiguration;
		}
	}
}
