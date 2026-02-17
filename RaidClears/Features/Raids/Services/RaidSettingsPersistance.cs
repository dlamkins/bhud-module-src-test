using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Blish_HUD;
using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;

namespace RaidClears.Features.Raids.Services
{
	[Serializable]
	public class RaidSettingsPersistance : Labelable
	{
		[JsonIgnore]
		public static string FILENAME = "raid_settings.json";

		private const string CURRENT_VERSION = "3.5.0";

		private static readonly HashSet<string> SupportedVersions = new HashSet<string>(StringComparer.Ordinal) { "1.0.0", "3.5.0" };

		private static readonly HashSet<string> VersionsRequiringPriorityMigration = new HashSet<string>(StringComparer.Ordinal) { "1.0.0" };

		[JsonIgnore]
		protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new Dictionary<string, SettingEntry<bool>>();

		[JsonProperty("version")]
		public string Version { get; set; } = "3.5.0";


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
					foreach (BossEncounter encounter in wing.Encounters)
					{
						Encounters.Add(encounter.ApiId, value: true);
					}
				}
			}
		}

		public override void SetEncounterLabel(string encounterApiId, string label)
		{
			string storageKey = StorageKeyPrefixes.NormalizeStorageKey(encounterApiId);
			if (base.EncounterLabels.ContainsKey(storageKey))
			{
				base.EncounterLabels.Remove(storageKey);
			}
			base.EncounterLabels.Add(storageKey, label);
			Service.RaidWindow.UpdateEncounterLabel(encounterApiId, label);
			Service.StrikesWindow.UpdateEncounterLabel("priority_" + storageKey, label);
			Service.StrikesWindow.UpdateEncounterLabel("tomorrow_" + storageKey, label);
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
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => string.Format(Strings.Settings_Raid_ExpansionVisible_Description, expac2.Name)));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => string.Format(Strings.Settings_Raid_EnableExpansion, expac2.Name)));
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
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => string.Format(Strings.Settings_Raid_WingVisible_Description, raidWing2.Name)));
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

		public SettingEntry<bool> GetEncounterVisible(BossEncounter encounter)
		{
			BossEncounter encounter2 = encounter;
			string id = StorageKeyPrefixes.NormalizeStorageKey(encounter2.EncounterId);
			if (VirtualSettingsEnties.ContainsKey(id))
			{
				return VirtualSettingsEnties[id];
			}
			if (!Encounters.ContainsKey(id))
			{
				Encounters.Add(id, value: true);
				Save();
			}
			SettingEntry<bool> obj = new SettingEntry<bool>();
			obj.set_Value(Encounters[id]);
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => string.Format(Strings.Settings_Raid_EncounterVisible_Description, encounter2.Name)));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => encounter2.Abbriviation));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				Encounters[id] = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add(id, setting);
			return setting;
		}

		public SettingEntry<bool>? GetEncounterVisibleByApiId(string apiId)
		{
			foreach (ExpansionRaid expansion in Service.RaidData.Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					foreach (BossEncounter encounter in wing.Encounters)
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
			using StreamWriter writer = new StreamWriter(configFileInfo.FullName, append: false, Encoding.UTF8);
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
				using (StreamReader reader = new StreamReader(configFileInfo.FullName, Encoding.UTF8))
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
			if (!SupportedVersions.Contains(data.Version))
			{
				return new RaidSettingsPersistance();
			}
			if (data.Version == "3.5.0")
			{
				return data;
			}
			if (VersionsRequiringPriorityMigration.Contains(data.Version))
			{
				MigratePriorityKeysFromStorage(data);
			}
			data.Version = "3.5.0";
			data.Save();
			return data;
		}

		private static bool MigratePriorityKeysFromStorage(RaidSettingsPersistance data)
		{
			bool changed = false;
			foreach (string key2 in data.EncounterLabels.Keys.ToList())
			{
				if (!(key2 == "priority") && !(key2 == "priority_tomorrow") && (key2.StartsWith("priority_", StringComparison.Ordinal) || key2.StartsWith("tomorrow_", StringComparison.Ordinal)))
				{
					string baseKey2 = (key2.StartsWith("priority_", StringComparison.Ordinal) ? key2.Substring("priority_".Length) : key2.Substring("tomorrow_".Length));
					if (!data.EncounterLabels.ContainsKey(baseKey2))
					{
						data.EncounterLabels[baseKey2] = data.EncounterLabels[key2];
					}
					data.EncounterLabels.Remove(key2);
					changed = true;
				}
			}
			foreach (string key in data.Encounters.Keys.ToList())
			{
				if (!(key == "priority") && !(key == "priority_tomorrow") && (key.StartsWith("priority_", StringComparison.Ordinal) || key.StartsWith("tomorrow_", StringComparison.Ordinal)))
				{
					string baseKey = (key.StartsWith("priority_", StringComparison.Ordinal) ? key.Substring("priority_".Length) : key.Substring("tomorrow_".Length));
					if (!data.Encounters.ContainsKey(baseKey))
					{
						data.Encounters[baseKey] = data.Encounters[key];
					}
					data.Encounters.Remove(key);
					changed = true;
				}
			}
			return changed;
		}

		private static RaidSettingsPersistance CreateNewCharacterConfiguration()
		{
			RaidSettingsPersistance raidSettingsPersistance = new RaidSettingsPersistance();
			raidSettingsPersistance.DefineEmpty();
			raidSettingsPersistance.Save();
			return raidSettingsPersistance;
		}
	}
}
