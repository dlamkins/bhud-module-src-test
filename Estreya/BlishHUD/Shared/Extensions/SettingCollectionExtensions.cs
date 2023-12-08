using Blish_HUD.Settings;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class SettingCollectionExtensions
	{
		public static void AddLoggingEvents(this SettingCollection settings)
		{
			foreach (SettingEntry setting in settings)
			{
				typeof(SettingEntryExtensions).GetMethod("AddLoggingEvent").MakeGenericMethod(setting.get_SettingType()).Invoke(setting, new object[1] { setting });
			}
		}

		public static void RemoveLoggingEvents(this SettingCollection settings)
		{
			foreach (SettingEntry setting in settings)
			{
				typeof(SettingEntryExtensions).GetMethod("RemoveLoggingEvent").MakeGenericMethod(setting.get_SettingType()).Invoke(setting, new object[1] { setting });
			}
		}
	}
}
