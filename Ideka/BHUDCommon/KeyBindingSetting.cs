using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace Ideka.BHUDCommon
{
	public class KeyBindingSetting : GenericSetting<KeyBinding>
	{
		private Action _action;

		public KeyBindingSetting(SettingCollection settings, string key, KeyBinding defaultValue, Func<string> displayNameFunc, Func<string> descriptionFunc)
		{
			Initialize(settings.DefineSetting<KeyBinding>(key, defaultValue, displayNameFunc, descriptionFunc));
		}

		public void OnTrigger(Action action)
		{
			base.Setting.get_Value().set_Enabled(true);
			base.Setting.get_Value().add_Activated((EventHandler<EventArgs>)Activated);
			_action = action;
		}

		private void Activated(object sender, EventArgs e)
		{
			_action?.Invoke();
		}

		public override void Dispose()
		{
			base.Dispose();
			base.Setting.get_Value().set_Enabled(false);
			base.Setting.get_Value().remove_Activated((EventHandler<EventArgs>)Activated);
		}
	}
}
