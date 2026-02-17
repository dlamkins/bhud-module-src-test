using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Blish_HUD;
using Blish_HUD.Settings;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Localization;

namespace RaidClears.Features.Strikes.Services
{
	[Serializable]
	public class StrikeSettingsPersistance : Labelable
	{
		[JsonIgnore]
		public static string FILENAME = "strike_settings.json";

		private const string CURRENT_VERSION = "3.5.0";

		private static readonly HashSet<string> SupportedVersions = new HashSet<string>(StringComparer.Ordinal) { "3.0.0", "3.5.0" };

		private static readonly HashSet<string> VersionsRequiringPriorityMigration = new HashSet<string>(StringComparer.Ordinal) { "3.0.0" };

		[JsonIgnore]
		protected Dictionary<string, SettingEntry<bool>> VirtualSettingsEnties = new Dictionary<string, SettingEntry<bool>>();

		[JsonProperty("version")]
		public string Version { get; set; } = "3.5.0";


		[JsonProperty("priority")]
		public bool Priority { get; set; } = true;


		[JsonProperty("tomorrow_bounties")]
		public bool TomorrowBounties { get; set; }

		[JsonProperty("expansions")]
		public Dictionary<string, bool> Expansions { get; set; } = new Dictionary<string, bool>();


		[JsonProperty("missions")]
		public Dictionary<string, bool> Missions { get; set; } = new Dictionary<string, bool>();


		public event EventHandler<bool>? StrikeSettingsChanged;

		public StrikeSettingsPersistance()
		{
			_isStrike = true;
		}

		public void DefineEmpty()
		{
			foreach (ExpansionStrikes expac in Service.StrikeData.Expansions)
			{
				Expansions.Add(expac.Id, value: true);
				foreach (BossEncounter miss in expac.Missions)
				{
					Missions.Add(miss.Id, value: true);
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
			Service.StrikesWindow.UpdateEncounterLabel(encounterApiId, label);
			Service.StrikesWindow.UpdateEncounterLabel("priority_" + storageKey, label);
			Service.StrikesWindow.UpdateEncounterLabel("tomorrow_" + storageKey, label);
			Save();
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
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => string.Format(Strings.StrikeSettings_EnablePriority, priority2.Name)));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				Priority = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add("priority", setting);
			return setting;
		}

		public SettingEntry<bool> GetTomorrowBountiesVisible(ExpansionStrikes priority)
		{
			ExpansionStrikes priority2 = priority;
			if (VirtualSettingsEnties.ContainsKey("priority_tomorrow"))
			{
				return VirtualSettingsEnties["priority_tomorrow"];
			}
			SettingEntry<bool> obj = new SettingEntry<bool>();
			obj.set_Value(TomorrowBounties);
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => string.Empty));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => string.Format(Strings.StrikeSettings_EnablePriority, priority2.Name)));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				TomorrowBounties = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add("priority_tomorrow", setting);
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
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => string.Format(Strings.StrikeSettings_EnableExpansion, expac2.Name)));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				Expansions[expac2.Id] = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add(expac2.Id, setting);
			return setting;
		}

		public SettingEntry<bool> GetMissionVisible(BossEncounter mission)
		{
			BossEncounter mission2 = mission;
			string id = StorageKeyPrefixes.NormalizeStorageKey(mission2.EncounterId);
			if (VirtualSettingsEnties.ContainsKey(id))
			{
				return VirtualSettingsEnties[id];
			}
			if (!Missions.ContainsKey(id))
			{
				Missions.Add(id, value: true);
				Save();
			}
			SettingEntry<bool> obj = new SettingEntry<bool>();
			obj.set_Value(Missions[id]);
			((SettingEntry)obj).set_GetDescriptionFunc((Func<string>)(() => ""));
			((SettingEntry)obj).set_GetDisplayNameFunc((Func<string>)(() => mission2.Name));
			SettingEntry<bool> setting = obj;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				Missions[id] = e.get_NewValue();
				Save();
			});
			VirtualSettingsEnties.Add(id, setting);
			return setting;
		}

		public override void Save()
		{
			FileInfo configFileInfo = GetConfigFileInfo();
			string serializedContents = JsonConvert.SerializeObject(this, Formatting.Indented);
			using StreamWriter writer = new StreamWriter(configFileInfo.FullName, append: false, Encoding.UTF8);
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
				using (StreamReader reader = new StreamReader(configFileInfo.FullName, Encoding.UTF8))
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
			if (!SupportedVersions.Contains(data.Version))
			{
				return new StrikeSettingsPersistance();
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

		private static bool MigratePriorityKeysFromStorage(StrikeSettingsPersistance data)
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
			foreach (string key in data.Missions.Keys.ToList())
			{
				if (!(key == "priority") && !(key == "priority_tomorrow") && (key.StartsWith("priority_", StringComparison.Ordinal) || key.StartsWith("tomorrow_", StringComparison.Ordinal)))
				{
					string baseKey = (key.StartsWith("priority_", StringComparison.Ordinal) ? key.Substring("priority_".Length) : key.Substring("tomorrow_".Length));
					if (!data.Missions.ContainsKey(baseKey))
					{
						data.Missions[baseKey] = data.Missions[key];
					}
					data.Missions.Remove(key);
					changed = true;
				}
			}
			return changed;
		}

		private static StrikeSettingsPersistance CreateNewCharacterConfiguration()
		{
			StrikeSettingsPersistance strikeSettingsPersistance = new StrikeSettingsPersistance();
			strikeSettingsPersistance.DefineEmpty();
			strikeSettingsPersistance.Save();
			return strikeSettingsPersistance;
		}
	}
}
