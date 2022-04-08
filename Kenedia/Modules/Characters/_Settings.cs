using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace Kenedia.Modules.Characters
{
	public class _Settings
	{
		public SettingEntry<KeyBinding> LogoutKey;

		public SettingEntry<KeyBinding> ShortcutKey;

		public SettingEntry<KeyBinding> SwapModifier;

		public SettingEntry<bool> EnterOnSwap;

		public SettingEntry<bool> AutoLogin;

		public SettingEntry<bool> DoubleClickToEnter;

		public SettingEntry<bool> FadeSubWindows;

		public SettingEntry<bool> OnlyMaxCrafting;

		public SettingEntry<bool> FocusFilter;

		public SettingEntry<bool> EnterToLogin;

		public SettingEntry<int> SwapDelay;

		public SettingEntry<int> FilterDelay;

		public int _FilterDelay = 75;

		public DateTime SwapModifierPressed;

		public bool isSwapModifierPressed()
		{
			Module.Logger.Debug("Time Since Logout Mod Click: " + DateTime.Now.Subtract(SwapModifierPressed).TotalMilliseconds);
			return DateTime.Now.Subtract(SwapModifierPressed).TotalMilliseconds <= 2500.0;
		}
	}
}
