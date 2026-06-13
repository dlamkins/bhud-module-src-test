using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using CinemaModule.UI.Windows.Info;
using Microsoft.Xna.Framework.Input;

namespace CinemaModule.Settings
{
	public class CinemaSettings : IDisposable
	{
		private ThirdPartyNoticesWindow _thirdPartyNoticesWindow;

		public SettingEntry<bool> EnabledSetting { get; }

		public bool IsEnabled => EnabledSetting.get_Value();

		public SettingEntry<bool> KeybindsEnabled { get; }

		public SettingEntry<KeyBinding> KeybindPlayPause { get; }

		public SettingEntry<KeyBinding> KeybindLockWindow { get; }

		public SettingEntry<KeyBinding> KeybindMuteToggle { get; }

		public SettingEntry<KeyBinding> KeybindToggleEnabled { get; }

		public CinemaSettings(SettingCollection settings)
		{
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Expected O, but got Unknown
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Expected O, but got Unknown
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Expected O, but got Unknown
			EnabledSetting = settings.DefineSetting<bool>("CinemaEnabled", false, (Func<string>)(() => "Enable Cinema"), (Func<string>)(() => "Toggle the CinemaHUD video player on or off"));
			SettingCollection keybindCollection = settings.AddSubCollection("Keybinds", false);
			KeybindsEnabled = keybindCollection.DefineSetting<bool>("KeybindsEnabled", false, (Func<string>)(() => "Enable Keybinds"), (Func<string>)(() => "Toggle for all CinemaHUD keybinds"));
			KeybindPlayPause = keybindCollection.DefineSetting<KeyBinding>("KeybindPlayPause", new KeyBinding((Keys)0), (Func<string>)(() => "Play / Pause"), (Func<string>)(() => "Toggle playback play or pause"));
			KeybindLockWindow = keybindCollection.DefineSetting<KeyBinding>("KeybindLockWindow", new KeyBinding((Keys)0), (Func<string>)(() => "Lock / Unlock Window"), (Func<string>)(() => "Toggle the on-screen window lock to prevent moving or resizing"));
			KeybindMuteToggle = keybindCollection.DefineSetting<KeyBinding>("KeybindMuteToggle", new KeyBinding((Keys)0), (Func<string>)(() => "Mute / Unmute"), (Func<string>)(() => "Toggle playback audio mute"));
			KeybindToggleEnabled = keybindCollection.DefineSetting<KeyBinding>("KeybindToggleEnabled", new KeyBinding((Keys)0), (Func<string>)(() => "Toggle Cinema On/Off"), (Func<string>)(() => "Enable or disable the entire CinemaHUD module"));
		}

		public void ShowThirdPartyNotices()
		{
			if (_thirdPartyNoticesWindow == null)
			{
				_thirdPartyNoticesWindow = new ThirdPartyNoticesWindow();
			}
			((Control)_thirdPartyNoticesWindow).Show();
		}

		public void Dispose()
		{
			ThirdPartyNoticesWindow thirdPartyNoticesWindow = _thirdPartyNoticesWindow;
			if (thirdPartyNoticesWindow != null)
			{
				((Control)thirdPartyNoticesWindow).Dispose();
			}
		}
	}
}
