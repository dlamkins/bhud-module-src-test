using System;
using Blish_HUD.Input;
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

		protected bool SetProperty(ref KeyBinding oldBinding, KeyBinding newBinding)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (oldBinding == newBinding)
			{
				return false;
			}
			if (oldBinding != null)
			{
				oldBinding.set_Enabled(false);
				oldBinding.remove_BindingChanged((EventHandler<EventArgs>)OnBindingChanged);
			}
			oldBinding = (KeyBinding)(((object)newBinding) ?? ((object)new KeyBinding()));
			oldBinding.add_BindingChanged((EventHandler<EventArgs>)OnBindingChanged);
			oldBinding.set_Enabled(true);
			return true;
		}

		protected virtual void BindingChanged()
		{
		}

		private void OnBindingChanged(object sender, EventArgs e)
		{
			BindingChanged();
		}
	}
}
