using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace Kenedia.Modules.Characters
{
	public class _Settings
	{
		public SettingEntry<KeyBinding> LogoutKey;

		public SettingEntry<KeyBinding> ShortcutKey;

		public SettingEntry<bool> EnterOnSwap;

		public SettingEntry<bool> AutoLogin;

		public SettingEntry<bool> DoubleClickToEnter;

		public SettingEntry<int> SwapDelay;

		public SettingEntry<int> FilterDelay;

		public int _FilterDelay = 75;
	}
}
