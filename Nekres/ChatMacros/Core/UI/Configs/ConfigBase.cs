using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace Nekres.ChatMacros.Core.UI.Configs
{
	public abstract class ConfigBase
	{
		protected virtual void BindingChanged()
		{
		}

		protected void SaveConfig<T>(SettingEntry<T> setting) where T : ConfigBase
		{
			if (setting != null && !((SettingEntry)setting).get_IsNull())
			{
				setting.set_Value((T)null);
				setting.set_Value(this as T);
			}
		}

		protected bool SetProperty<T>(ref T property, T value)
		{
			if (object.Equals(property, value))
			{
				return false;
			}
			property = value;
			return true;
		}

		protected KeyBinding ResetDelegates(KeyBinding oldBinding, KeyBinding newBinding)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			if (oldBinding != null)
			{
				oldBinding.set_Enabled(false);
				oldBinding.remove_BindingChanged((EventHandler<EventArgs>)OnBindingChanged);
			}
			if (newBinding == null)
			{
				newBinding = new KeyBinding();
			}
			newBinding.add_BindingChanged((EventHandler<EventArgs>)OnBindingChanged);
			newBinding.set_Enabled(true);
			return newBinding;
		}

		private void OnBindingChanged(object sender, EventArgs e)
		{
			BindingChanged();
		}
	}
}
