using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Settings;
using Newtonsoft.Json;

namespace RaidClears.Features.Strikes.Services
{
	[Serializable]
	public class StrikeSettingsPersistance
	{
		[JsonIgnore]
		public static string FILENAME = "strike_settings.json";

		[JsonIgnore]
		protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new Dictionary<string, SettingEntry<bool>>();

		[JsonProperty("version")]
		public string Version { get; set; } = "3.0.0";


		[JsonProperty("priority")]
		public bool Priority { get; set; } = true;


		[JsonProperty("expansions")]
		public Dictionary<string, bool> Expansions { get; set; } = new Dictionary<string, bool>();


		[JsonProperty("missions")]
		public Dictionary<string, bool> Missions { get; set; } = new Dictionary<string, bool>();


		public event EventHandler<bool>? StrikeSettingsChanged;

		public void DefineEmpty()
		{
			foreach (ExpansionStrikes expac in Service.StrikeData.Expansions)
			{
				Expansions.Add(expac.Id, value: true);
				foreach (StrikeMission miss in expac.Missions)
				{
					Missions.Add(miss.Id, value: true);
				}
			}
		}

		public SettingEntry<bool> GetPriorityVisible(ExpansionStrikes priority)
		{
			ExpansionStrikes priority2 = priority;
			if (VirtualSettingsEnties.ContainsKey("priority"))
			{
				return VirtualSettingsEnties["priority"];
			}
			SettingEntry<bool> obj = new SettingEntry<bool>();
			obj.set_Value(Priority);
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => ""));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => "Enable " + priority2.Name));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				Priority = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add("priority", setting);
			return setting;
		}

		public SettingEntry<bool> GetExpansionVisible(ExpansionStrikes expac)
		{
			ExpansionStrikes expac2 = expac;
			if (VirtualSettingsEnties.ContainsKey(expac2.Id))
			{
				return VirtualSettingsEnties[expac2.Id];
			}
			if (!Expansions.ContainsKey(expac2.Id))
			{
				Expansions.Add(expac2.Id, value: true);
				Save();
			}
			SettingEntry<bool> obj = new SettingEntry<bool>();
			obj.set_Value(Expansions[expac2.Id]);
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => ""));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => "Enable " + expac2.Name));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				Expansions[expac2.Id] = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add(expac2.Id, setting);
			return setting;
		}

		public SettingEntry<bool> GetMissionVisible(StrikeMission mission)
		{
			StrikeMission mission2 = mission;
			if (VirtualSettingsEnties.ContainsKey(mission2.Id))
			{
				return VirtualSettingsEnties[mission2.Id];
			}
			if (!Missions.ContainsKey(mission2.Id))
			{
				Missions.Add(mission2.Id, value: true);
				Save();
			}
			SettingEntry<bool> obj = new SettingEntry<bool>();
			obj.set_Value(Missions[mission2.Id]);
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => ""));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => mission2.Name ?? ""));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				Missions[mission2.Id] = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add(mission2.Id, setting);
			return setting;
		}

		public void Save()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			string serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);
			using StreamWriter writer = new StreamWriter(configFileInfo.FullName);
			writer.Write(serializedContents);
			writer.Close();
			this.StrikeSettingsChanged?.Invoke(this, e: true);
		}

		private static FileInfo GetConfigFileInfo()
		{
			return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + FILENAME);
		}

		public static StrikeSettingsPersistance Load()
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

		private static StrikeSettingsPersistance LoadExistingCharacterConfiguration(string fileText)
		{
			StrikeSettingsPersistance loadedCharacterConfiguration = JsonConvert.DeserializeObject<StrikeSettingsPersistance>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new StrikeSettingsPersistance();
			}
			return HandleVersionUpgrade(loadedCharacterConfiguration);
		}

		private static StrikeSettingsPersistance HandleVersionUpgrade(StrikeSettingsPersistance data)
		{
			if (data.Version == "3.0.0")
			{
				return data;
			}
			return new StrikeSettingsPersistance();
		}

		private static StrikeSettingsPersistance CreateNewCharacterConfiguration()
		{
			StrikeSettingsPersistance newCharacterConfiguration = new StrikeSettingsPersistance();
			newCharacterConfiguration.DefineEmpty();
			Service.Settings.StrikeSettings.ConvertToJsonFile(newCharacterConfiguration);
			newCharacterConfiguration.Save();
			return newCharacterConfiguration;
		}
	}
}
