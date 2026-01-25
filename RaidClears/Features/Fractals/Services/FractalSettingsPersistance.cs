using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Blish_HUD;
using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Localization;

namespace RaidClears.Features.Fractals.Services
{
	[Serializable]
	public class FractalSettingsPersistance
	{
		[JsonIgnore]
		public static string FILENAME = "fractal_settings.json";

		[JsonIgnore]
		protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new Dictionary<string, SettingEntry<bool>>();

		[JsonProperty("version")]
		public string Version { get; set; } = "1.0.0";


		[JsonProperty("challengeMotes")]
		public Dictionary<string, bool> ChallengeMotes { get; set; } = new Dictionary<string, bool>();


		public event EventHandler<bool>? FractalSettingsChanged;

		public void DefineEmpty()
		{
			int[] challengeMotes = Service.FractalMapData.ChallengeMotes;
			foreach (int scale in challengeMotes)
			{
				FractalMap fractal = Service.FractalMapData.GetFractalForScale(scale);
				if (fractal.ApiLabel != "undefined")
				{
					ChallengeMotes.Add(fractal.ApiLabel, value: true);
				}
			}
		}

		public SettingEntry<bool> GetChallengeMoteVisible(FractalMap fractal)
		{
			FractalMap fractal2 = fractal;
			if (VirtualSettingsEnties.ContainsKey(fractal2.ApiLabel))
			{
				return VirtualSettingsEnties[fractal2.ApiLabel];
			}
			if (!ChallengeMotes.ContainsKey(fractal2.ApiLabel))
			{
				ChallengeMotes.Add(fractal2.ApiLabel, value: true);
				Save();
			}
			SettingEntry<bool> obj = new SettingEntry<bool>();
			obj.set_Value(ChallengeMotes[fractal2.ApiLabel]);
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => string.Format(Strings.Settings_Fractal_ChallengeMoteVisible_Description, fractal2.Label)));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => fractal2.Label ?? ""));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				ChallengeMotes[fractal2.ApiLabel] = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add(fractal2.ApiLabel, setting);
			return setting;
		}

		public SettingEntry<bool>? GetChallengeMoteVisibleByApiId(string apiId)
		{
			int[] challengeMotes = Service.FractalMapData.ChallengeMotes;
			foreach (int scale in challengeMotes)
			{
				FractalMap fractal = Service.FractalMapData.GetFractalForScale(scale);
				if (fractal.ApiLabel == apiId)
				{
					return GetChallengeMoteVisible(fractal);
				}
			}
			return null;
		}

		public void Save()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			string serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);
			using StreamWriter writer = new StreamWriter(configFileInfo.FullName, append: false, Encoding.UTF8);
			writer.Write(serializedContents);
			writer.Close();
			this.FractalSettingsChanged?.Invoke(this, e: true);
		}

		private static FileInfo GetConfigFileInfo()
		{
			return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + FILENAME);
		}

		public static FractalSettingsPersistance Load()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			if (configFileInfo != null && configFileInfo.Exists)
			{
				using (StreamReader reader = new StreamReader(configFileInfo.FullName, Encoding.UTF8))
				{
					string fileText = reader.ReadToEnd();
					reader.Close();
					return LoadExistingCharacterConfiguration(fileText);
				}
			}
			return CreateNewCharacterConfiguration();
		}

		private static FractalSettingsPersistance LoadExistingCharacterConfiguration(string fileText)
		{
			FractalSettingsPersistance loadedCharacterConfiguration = JsonConvert.DeserializeObject<FractalSettingsPersistance>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new FractalSettingsPersistance();
			}
			return HandleVersionUpgrade(loadedCharacterConfiguration);
		}

		private static FractalSettingsPersistance HandleVersionUpgrade(FractalSettingsPersistance data)
		{
			if (data.Version == "1.0.0")
			{
				return data;
			}
			return new FractalSettingsPersistance();
		}

		private static FractalSettingsPersistance CreateNewCharacterConfiguration()
		{
			FractalSettingsPersistance fractalSettingsPersistance = new FractalSettingsPersistance();
			fractalSettingsPersistance.DefineEmpty();
			fractalSettingsPersistance.Save();
			return fractalSettingsPersistance;
		}
	}
}
