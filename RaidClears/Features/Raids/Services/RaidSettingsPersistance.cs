using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;

namespace RaidClears.Features.Raids.Services
{
	[Serializable]
	public class RaidSettingsPersistance : Labelable
	{
		[JsonIgnore]
		public static string FILENAME = "raid_settings.json";

		[JsonIgnore]
		protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new Dictionary<string, SettingEntry<bool>>();

		[JsonProperty("version")]
		public string Version { get; set; } = "1.0.0";


		[JsonProperty("expansions")]
		public Dictionary<string, bool> Expansions { get; set; } = new Dictionary<string, bool>();


		[JsonProperty("wings")]
		public Dictionary<string, bool> Wings { get; set; } = new Dictionary<string, bool>();


		[JsonProperty("encounters")]
		public Dictionary<string, bool> Encounters { get; set; } = new Dictionary<string, bool>();


		public event EventHandler<bool>? RaidSettingsChanged;

		public RaidSettingsPersistance()
		{
			_isRaid = true;
		}

		public void DefineEmpty()
		{
			foreach (ExpansionRaid expac in Service.RaidData.Expansions)
			{
				Expansions.Add(expac.Id, value: true);
				foreach (RaidWing wing in expac.Wings)
				{
					Wings.Add(wing.Id, value: true);
					foreach (RaidEncounter encounter in wing.Encounters)
					{
						Encounters.Add(encounter.ApiId, value: true);
					}
				}
			}
		}

		public override void SetEncounterLabel(string encounterApiId, string label)
		{
			if (base.EncounterLabels.ContainsKey(encounterApiId))
			{
				base.EncounterLabels.Remove(encounterApiId);
			}
			base.EncounterLabels.Add(encounterApiId, label);
			Service.RaidWindow.UpdateEncounterLabel(encounterApiId, label);
			Save();
		}

		public SettingEntry<bool> GetExpansionVisible(ExpansionRaid expac)
		{
			ExpansionRaid expac2 = expac;
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
				foreach (RaidWing current in expac2.Wings)
				{
					GetWingVisible(current).set_Value(e.get_NewValue());
				}
				Save();
			});
			VirtualSettingsEnties.Add(expac2.Id, setting);
			return setting;
		}

		public SettingEntry<bool> GetWingVisible(RaidWing raidWing)
		{
			RaidWing raidWing2 = raidWing;
			if (VirtualSettingsEnties.ContainsKey(raidWing2.Id))
			{
				return VirtualSettingsEnties[raidWing2.Id];
			}
			if (!Wings.ContainsKey(raidWing2.Id))
			{
				Wings.Add(raidWing2.Id, value: true);
				Save();
			}
			SettingEntry<bool> obj = new SettingEntry<bool>();
			obj.set_Value(Wings[raidWing2.Id]);
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => "Show " + raidWing2.Name + " on the raid overlay"));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => raidWing2.Name ?? ""));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				Wings[raidWing2.Id] = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add(raidWing2.Id, setting);
			return setting;
		}

		public SettingEntry<bool> GetEncounterVisible(RaidEncounter encounter)
		{
			RaidEncounter encounter2 = encounter;
			if (VirtualSettingsEnties.ContainsKey(encounter2.ApiId))
			{
				return VirtualSettingsEnties[encounter2.ApiId];
			}
			if (!Encounters.ContainsKey(encounter2.ApiId))
			{
				Encounters.Add(encounter2.ApiId, value: true);
				Save();
			}
			SettingEntry<bool> obj = new SettingEntry<bool>();
			obj.set_Value(Encounters[encounter2.ApiId]);
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => "Show " + encounter2.Name + " on the raid overlay"));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => encounter2.Abbriviation ?? ""));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				Encounters[encounter2.ApiId] = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add(encounter2.ApiId, setting);
			return setting;
		}

		public SettingEntry<bool>? GetEncounterVisibleByApiId(string apiId)
		{
			foreach (ExpansionRaid expansion in Service.RaidData.Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					foreach (RaidEncounter encounter in wing.Encounters)
					{
						if (encounter.ApiId == apiId)
						{
							return GetEncounterVisible(encounter);
						}
					}
				}
			}
			return null;
		}

		public override void Save()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			string serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);
			using StreamWriter writer = new StreamWriter(configFileInfo.FullName);
			writer.Write(serializedContents);
			writer.Close();
			this.RaidSettingsChanged?.Invoke(this, e: true);
		}

		private static FileInfo GetConfigFileInfo()
		{
			return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + FILENAME);
		}

		public static RaidSettingsPersistance Load()
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

		private static RaidSettingsPersistance LoadExistingCharacterConfiguration(string fileText)
		{
			RaidSettingsPersistance loadedCharacterConfiguration = JsonConvert.DeserializeObject<RaidSettingsPersistance>(fileText);
			if (loadedCharacterConfiguration == null)
			{
				loadedCharacterConfiguration = new RaidSettingsPersistance();
			}
			return HandleVersionUpgrade(loadedCharacterConfiguration);
		}

		private static RaidSettingsPersistance HandleVersionUpgrade(RaidSettingsPersistance data)
		{
			if (data.Version == "1.0.0")
			{
				return data;
			}
			return new RaidSettingsPersistance();
		}

		private static RaidSettingsPersistance CreateNewCharacterConfiguration()
		{
			RaidSettingsPersistance newCharacterConfiguration = new RaidSettingsPersistance();
			newCharacterConfiguration.DefineEmpty();
			Service.Settings.RaidSettings.ConvertToJsonFile(newCharacterConfiguration, Service.RaidData);
			newCharacterConfiguration.Save();
			return newCharacterConfiguration;
		}
	}
}
