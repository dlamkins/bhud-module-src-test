using Blish_HUD.Settings;

namespace RaidClears.Settings.Models
{
	public static class SettingCollectionExtensions
	{
		public static SettingEntry<TEntry> DefineSetting<TEntry>(this SettingCollection collection, Setting<TEntry> setting)
		{
			return collection.DefineSetting<TEntry>(setting.Key, setting.DefaultValue, setting.Name, setting.Description);
		}

		public static SettingEntry<TEntry> DefineSettingRange<TEntry>(this SettingCollection collection, Setting<TEntry> setting, float min, float max)
		{
			SettingEntry<TEntry> obj = collection.DefineSetting<TEntry>(setting.Key, setting.DefaultValue, setting.Name, setting.Description);
			SettingComplianceExtensions.SetRange(obj as SettingEntry<float>, min, max);
			return obj;
		}
	}
}
