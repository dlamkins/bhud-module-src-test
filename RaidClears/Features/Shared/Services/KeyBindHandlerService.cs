using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace RaidClears.Features.Shared.Services
{
	public class KeyBindHandlerService : IDisposable
	{
		private readonly SettingEntry<KeyBinding> _keyBindSetting;

		private readonly SettingEntry<bool> _toggleControlSetting;

		public KeyBindHandlerService(SettingEntry<KeyBinding> keyBindSetting, SettingEntry<bool> toggleControlSetting)
		{
			_keyBindSetting = keyBindSetting;
			_toggleControlSetting = toggleControlSetting;
			_keyBindSetting.get_Value().set_Enabled(true);
			_keyBindSetting.get_Value().add_Activated((EventHandler<EventArgs>)OnKeyBindActivated);
		}

		public void Dispose()
		{
			_keyBindSetting.get_Value().set_Enabled(false);
			_keyBindSetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnKeyBindActivated);
		}

		private void OnKeyBindActivated(object sender, EventArgs e)
		{
			_toggleControlSetting.set_Value(!_toggleControlSetting.get_Value());
		}
	}
}
