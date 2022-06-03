using System;
using Blish_HUD;
using Blish_HUD.Settings;

namespace Ideka.BHUDCommon
{
	public class GenericSetting<T> : IDisposable
	{
		public SettingEntry<T> Setting { get; private set; }

		public T Value
		{
			get
			{
				return Setting.get_Value();
			}
			set
			{
				Setting.set_Value(value);
			}
		}

		public Action<T> Changed { get; set; }

		protected GenericSetting()
		{
		}

		public GenericSetting(SettingCollection settings, string key, T defaultValue, Func<string> displayNameFunc, Func<string> descriptionFunc)
		{
			Initialize(settings.DefineSetting<T>(key, defaultValue, displayNameFunc, descriptionFunc));
		}

		protected void Initialize(SettingEntry<T> setting)
		{
			Setting = setting;
			Setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<T>>)SettingChanged);
		}

		protected virtual void SettingChanged(object sender, ValueChangedEventArgs<T> e)
		{
			Changed?.Invoke(Setting.get_Value());
		}

		public virtual void Dispose()
		{
			Setting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<T>>)SettingChanged);
		}
	}
}
