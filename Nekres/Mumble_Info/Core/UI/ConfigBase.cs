using Blish_HUD.Settings;

namespace Nekres.Mumble_Info.Core.UI
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

		protected bool SetProperty<T>(ref T property, T newValue)
		{
			if (object.Equals(property, newValue))
			{
				return false;
			}
			property = newValue;
			return true;
		}
	}
}
