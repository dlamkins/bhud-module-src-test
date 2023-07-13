using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Enums;

namespace RaidClears.Features.Fractals.Services
{
	[Serializable]
	public class FractalPersistance
	{
		[JsonIgnore]
		public static string FILENAME = "fractal_clears.json";

		[JsonProperty("version")]
		public string Version { get; set; } = "2.2.0";


		[JsonProperty("accountClears")]
		public Dictionary<string, Dictionary<Encounters.Fractal, DateTime>> AccountClears { get; set; } = new Dictionary<string, Dictionary<Encounters.Fractal, DateTime>>();


		public Dictionary<Encounters.Fractal, DateTime> GetEmpty()
		{
			return new Dictionary<Encounters.Fractal, DateTime>
			{
				{
					Encounters.Fractal.AetherbladeFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.AquaticRuinsFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.CaptainMaiTrinBossFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.ChaosFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.CliffsideFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.DeepstoneFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.MoltenBossFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.MoltenFurnaceFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.NightmareFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.ShatteredObservatoryFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.SirensReefFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.SilentSurfFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.SnowblindFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.SunquaPeakFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.SolidOceanFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.SwamplandFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.ThaumanovaReactorFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.TwilightOasisFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.UncategorizedFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.UndergroundFacilityFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.UrbanBattlegroundFractal,
					default(DateTime)
				},
				{
					Encounters.Fractal.VolcanicFractal,
					default(DateTime)
				}
			};
		}

		public void SaveClear(string account, Encounters.Fractal mission)
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

		public void RemoveClear(string account, Encounters.Fractal mission)
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
			return loadedCharacterConfiguration;
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
