using Blish_HUD.Settings;

namespace Nekres.ProofLogix.Core.UI.Configs
{
	public abstract class ConfigBase
	{
		protected void SaveConfig<T>(SettingEntry<T> setting) where T : ConfigBase
		{
			if (setting != null && !((SettingEntry)setting).get_IsNull())
			{
				setting.set_Value((T)null);
				setting.set_Value(this as T);
			}
		}
	}
}
