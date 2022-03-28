using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace Kenedia.Modules.Characters
{
	public class _Settings
	{
		public SettingEntry<KeyBinding> LogoutKey;

		public SettingEntry<bool> EnterOnSwap;

		public SettingEntry<int> SwapDelay;

		public SettingEntry<int> FilterDelay;

		public int _FilterDelay = 75;
	}
}
